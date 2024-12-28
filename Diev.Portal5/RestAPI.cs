#region License
/*
Copyright 2022-2024 Dmitrii Evdokimov
Open source software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System.Net;

using Diev.Extensions.Credentials;
using Diev.Extensions.Http;
using Diev.Extensions.LogFile;
using Diev.Extensions.Tools;
using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Messages.Create;
using Diev.Portal5.API.Tools;

namespace Diev.Portal5;

/// <summary>
/// REST API of Portal5.
/// </summary>
/// <param name="cred">Windows Credential Manager credential.</param>
/// <param name="trace">Trace HTTP.</param>
public class RestAPI(Credential cred, bool trace)
    : RestAPICore(cred, trace)
{
    /// <summary>
    /// 3.1.3 Отправка сообщений комплексно по задачам
    /// </summary>
    /// <param name="task"></param>
    /// <param name="title"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<string?> UploadDirectoryAsync(string task, string? title, string path)
    {
        Logger.Title($"Upload directory {path.PathQuoted()}");

        var draft = new DraftMessage
        {
            Task = task,
            Title = title,
        };

        foreach (var sig in new DirectoryInfo(path).EnumerateFiles("*.sig")) //TODO *.*
        {
            var enc = new FileInfo(Path.ChangeExtension(sig.FullName, "enc"));
            var src = new FileInfo(Path.ChangeExtension(sig.FullName, null));

            if (enc.Exists) // Encrypted (ДСП)
            {
                draft.Files.Add(new DraftMessageFile()
                {
                    Name = enc.Name,
                    Encrypted = true,
                    Size = enc.Length
                });

                draft.Files.Add(new DraftMessageFile()
                {
                    Name = sig.Name,
                    SignedFile = enc.Name,
                    Size = sig.Length
                });
            }
            else if (src.Exists) // Открытая информация
            {
                draft.Files.Add(new DraftMessageFile()
                {
                    Name = src.Name,
                    Size = src.Length
                });

                draft.Files.Add(new DraftMessageFile()
                {
                    Name = sig.Name,
                    SignedFile = src.Name,
                    Size = sig.Length
                });
            }
            else
            {
                throw new FileNotFoundException("Файл не найден", src.Name);
            }
        }

        // Step 1 - post request for new message
        Logger.Title("1. Post request for new message.");

        var message = await PostMessageRequestAsync(draft);

        if (message is null)
            return null;

        string msgId = message.Id;
        int sent = 0;

        // Upload files
        foreach (var messageFile in message.Files)
        {
            // Step 2 - post request for new file
            Logger.Title($"2.{sent + 1}. Post request for new file.");

            var uploadSession = await PostUploadRequestAsync(msgId, messageFile.Id);

            if (uploadSession is null)
                break;

            var expiration = uploadSession.ExpirationDateTime.ToLocalTime();

            // Step3 - upload new file
            Logger.Title($"3.{sent + 1}. Upload new file {messageFile.Name.PathQuoted()}.");

            string file = Path.Combine(path, messageFile.Name);

            if (!await UploadFileAsync(file, messageFile.Size, uploadSession.UploadUrl))
                break;

            sent++;

            if (DateTime.Now > expiration)
                break;
        }

        // Step 4 - post ready message
        Logger.Title("4. Post ready message.");

        if (sent == message.Files.Count)
        {
            if (await PostMessageAsync(msgId))
            {
                Logger.Title("Upload directory done.");
                Logger.Flush(2);
                return msgId;
            }
        }

        // Try to clean expired uploaded files
        Logger.Line("Try to clean expired uploaded files.");

        foreach (var file in message.Files)
        {
            sent--;

            try
            {
                await DeleteMessageFileAsync(msgId, file.Id);
            }
            catch { }

            if (sent == 0)
                break;
        }

        Logger.Title("Upload directory fail.");
        Logger.Flush(2);
        return null;
    }

    /// <summary>
    /// 3.1.4.1 Для получения всех сообщений с учетом фильтра используется метод GET.
    /// GET: */messages
    /// Осторожно: эта функция загружает в память ВСЕ сообщения - 
    /// без разбиения на страницы по 100 сообщений.
    /// Бонус: в фильтре можно указать несколько задач через запятую.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Все сообщения с учетом фильтра или NULL.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<Message>?> GetMessagesAsync(MessagesFilter filter)
    {
        Logger.Line("### Get messages.");

        if (filter.Task is null || !filter.Task.Contains(','))
        {
            return await GetMessagesCoreAsync(filter);
        }

        List<Message> messages = [];
        var tasks = filter.Task.Split(',');

        foreach (var task in tasks)
        {
            filter.Task = task;
            filter.Page = null;

            var range = await GetMessagesCoreAsync(filter);

            if (range is null)
                return null;

            if (range.Count > 0)
                messages.AddRange(range);
        }

        return messages;
    }

    private async Task<List<Message>?> GetMessagesCoreAsync(MessagesFilter filter)
    {
        List<Message> messages = [];

        // Get first page of 100
        var messagesPage = await GetMessagesPageAsync(filter.GetQuery());

        if (messagesPage is null)
            return null;

        while (messagesPage.Messages.Count > 0)
        {
            // Do action
            messages.AddRange(messagesPage.Messages);

            // Exit if last page
            if (messagesPage.Pages.CurrentPage == messagesPage.Pages.TotalPages)
                break;

            // Get next page of 100
            filter.Page = (uint)messagesPage.Pages.CurrentPage + 1;
            messagesPage = await GetMessagesPageAsync(filter.GetQuery());

            if (messagesPage is null)
                return null;
        }

        Logger.Flush(2);
        return messages;
    }

    /// <summary>
    /// Сохранение JSON информации о конкретном сообщении в файл.
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<bool> DownloadMessageJsonAsync(string msgId, string path, bool overwrite = false)
    {
        Logger.Line($"Download {path.PathQuoted()}.");

        if (SkipExisting(path, overwrite))
            return true;

        string url = Api + $"messages/{msgId}";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            using var file = File.OpenWrite(path);
            await stream.CopyToAsync(file);
        }

        return File.Exists(path);
    }

    /// <summary>
    /// Сохранение JSON информации о конкретном сообщении в файл.
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<bool> AppendMessageJsonAsync(string msgId, FileStream file)
    {
        string url = Api + $"messages/{msgId}";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            await stream.CopyToAsync(file);

            return true;
        }

        return false;
    }

    /// <summary>
    /// 3.1.5 Удаление сообщений с учетом фильтра.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>True если все удалены или false если какие-то (или все) могли остаться.</returns>
    public async Task<bool> DeleteMessagesAsync(MessagesFilter filter)
    {
        var messages = await GetMessagesAsync(filter);
        bool ok = true;

        if (messages is null)
            return false;

        foreach (var message in messages)
        {
            try
            {
                if (!await DeleteMessageAsync(message.Id))
                    ok = false;
            }
            catch
            {
                ok = false;
            }
        }

        return ok;
    }
}
