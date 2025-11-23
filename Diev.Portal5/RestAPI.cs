#region License
/*
Copyright 2022-2025 Dmitrii Evdokimov
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
/// <param name="debug">Debug HTTP.</param>
public class RestAPI(Credential cred, bool trace, bool debug)
    : RestAPICore(cred, trace, debug)
{
    /// <summary>
    /// 3.1.3 Отправка сообщений комплексно по задачам
    /// </summary>
    /// <param name="task"></param>
    /// <param name="title"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<ApiResult<string>> UploadDirectoryAsync(string task, string? title, string path)
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
            
            //string type = "Document";

            //if (src.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase)
            //    && (src.Name.StartsWith("DOVER_CBR_", StringComparison.OrdinalIgnoreCase)
            //        || src.Name.StartsWith("ON_EMCHD_", StringComparison.OrdinalIgnoreCase)))
            //{
            //    type = "PowerOfAttorney"; // МЧД
            //}

            if (enc.Exists) // Encrypted (ДСП)
            {
                draft.Files.Add(new DraftMessageFile
                {
                    Name = enc.Name,
                    Encrypted = true,
                    //FileType = type,
                    Size = enc.Length
                });

                draft.Files.Add(new DraftMessageFile
                {
                    Name = sig.Name,
                    //FileType = "Sign",
                    SignedFile = enc.Name,
                    Size = sig.Length
                });
            }
            else if (src.Exists) // Открытая информация
            {
                draft.Files.Add(new DraftMessageFile
                {
                    Name = src.Name,
                    //FileType = type,
                    Size = src.Length
                });

                draft.Files.Add(new DraftMessageFile
                {
                    Name = sig.Name,
                    //FileType = "Sign",
                    SignedFile = src.Name,
                    Size = sig.Length
                });
            }
            else
            {
                //throw new FileNotFoundException("Файл не найден", src.Name);
                return ApiResult<string>.CreateError($"Файл не найден {src.Name.PathQuoted()}");
            }
        }

        // Step 1 - post request for new message
        Logger.Title("1. Post request for new message.");

        var messageResult = await PostMessageRequestAsync(draft);

        if (!messageResult.OK)
        {
            return ApiResult<string>.CreateError(messageResult.Error);
        }

        var message = messageResult.Data!;
        string msgId = message.Id;
        int sent = 0;

        // Upload files
        foreach (var messageFile in message.Files)
        {
            // Step 2 - post request for new file
            Logger.Title($"2.{sent + 1}. Post request for new file.");

            var uploadSessionResult = await PostUploadRequestAsync(msgId, messageFile.Id);

            if (!uploadSessionResult.OK)
                break;

            var uploadSession = uploadSessionResult.Data!;
            var expiration = uploadSession.ExpirationDateTime.ToLocalTime();

            // Step3 - upload new file
            Logger.Title($"3.{sent + 1}. Upload new file {messageFile.Name.PathQuoted()}.");

            string file = Path.Combine(path, messageFile.Name);
            var uploadFileResult = await UploadFileAsync(file, messageFile.Size, uploadSession.UploadUrl);

            if (!uploadFileResult.OK)
                break;

            sent++;

            if (DateTime.Now > expiration)
                break;
        }

        // Step 4 - post ready message
        Logger.Title("4. Post ready message.");

        if (sent == message.Files.Count)
        {
            var postMessageResult = await PostMessageAsync(msgId);

            if (postMessageResult.OK)
            {
                return ApiResult<string>.CreateOK(msgId);
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

        return ApiResult<string>.CreateError("Upload directory fail.");
    }

    /// <summary>
    /// 3.1.3 Отправка сообщений комплексно по задачам
    /// </summary>
    /// <param name="task"></param>
    /// <param name="title"></param>
    /// <param name="path"></param>
    /// <param name="corrId"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<ApiResult<string>> UploadEncFileAsync(string task, string? title, string path, Guid? corrId = null)
    {
        Logger.Title($"Upload file {path.PathQuoted()}");

        var draft = new DraftMessage
        {
            Task = task,
            Title = title,
        };

        if (corrId is not null)
            draft.CorrelationId = corrId.ToString();

        var enc = new FileInfo(path);

        draft.Files.Add(new DraftMessageFile
        {
            Name = enc.Name,
            Encrypted = true,
            Size = enc.Length
        });

        // Step 1 - post request for new message
        Logger.Title("1. Post request for new message.");

        var newMessageResult = await PostMessageRequestAsync(draft);

        if (!newMessageResult.OK)
        {
            return ApiResult<string>.CreateError(newMessageResult.Error);
        }

        var message = newMessageResult.Data!;
        string msgId = message.Id;
        var messageFile = message.Files[0];

        // Step 2 - post request for new file
        Logger.Title($"2. Post request for new file.");

        var uploadSessionResult = await PostUploadRequestAsync(msgId, messageFile.Id);

        if (!uploadSessionResult.OK)
        {
            return ApiResult<string>.CreateError(uploadSessionResult.Error);
        }

        var uploadSession = uploadSessionResult.Data!;
        var expiration = uploadSession.ExpirationDateTime.ToLocalTime();

        // Step3 - upload new file
        Logger.Title($"3. Upload new file {messageFile.Name.PathQuoted()}.");

        string file = Path.Combine(path, messageFile.Name);
        var uploadFileResult = await UploadFileAsync(file, messageFile.Size, uploadSession.UploadUrl);

        if (!uploadFileResult.OK)
        {
            return ApiResult<string>.CreateError(uploadFileResult.Error);
        }

        if (DateTime.Now > expiration)
        {
            return ApiResult<string>.CreateError("Time to upload expired.");
        }

        // Step 4 - post ready message
        Logger.Title("4. Post ready message.");

        var postMessageResult = await PostMessageAsync(msgId);

        if (postMessageResult.OK)
        {
            return ApiResult<string>.CreateOK(msgId);
        }

        // Try to clean expired uploaded file
        Logger.Line("Try to clean expired uploaded file.");

        try
        {
            await DeleteMessageFileAsync(msgId, messageFile.Id);
        }
        catch { }

        return ApiResult<string>.CreateError("Upload file fail.");
    }

    /// <summary>
    /// 3.1.4.1 Для получения всех сообщений с учетом фильтра используется метод GET.
    /// GET: */messages
    /// Осторожно: эта функция загружает в память ВСЕ сообщения - 
    /// без разбиения на страницы по 100 сообщений.
    /// Бонус: в фильтре можно указать несколько задач через запятую.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Все сообщения с учетом фильтра.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<ApiResult<IReadOnlyList<Message>>> GetMessagesAsync(MessagesFilter filter)
    {
        Logger.Line("### Get messages.");

        if (filter.Task is null || !filter.Task.Contains(','))
        {
            var messages = await GetMessagesCoreAsync(filter);
            return ApiResult<IReadOnlyList<Message>>.CreateOK(messages.Data);
        }

        List<Message> rangeMessages = [];
        var tasks = filter.Task!.Split(',');

        foreach (var task in tasks)
        {
            filter.Task = task;
            filter.Page = null;

            var rangeResult = await GetMessagesCoreAsync(filter);

            if (!rangeResult.OK)
            {
                return ApiResult<IReadOnlyList<Message>>.CreateError("Range is null.");
            }

            var range = rangeResult.Data!;

            if (range.Count > 0)
            {
                rangeMessages.AddRange(range);
            }
        }

        return ApiResult<IReadOnlyList<Message>>.CreateOK(rangeMessages);
    }

    private async Task<ApiResult<IReadOnlyList<Message>>> GetMessagesCoreAsync(MessagesFilter filter)
    {
        List<Message> messages = [];

        // Get first page of 100
        var messagesPageResult = await GetMessagesPageAsync(filter.GetQuery());

        if (!messagesPageResult.OK)
        {
            ApiResult<IReadOnlyList<Message>>.CreateError(messagesPageResult.Error);
        }

        var messagesPage = messagesPageResult.Data!;

        while (messagesPage.Messages.Count > 0)
        {
            // Do action
            messages.AddRange(messagesPage.Messages);

            // Exit if last page
            if (messagesPage.Pages.CurrentPage == messagesPage.Pages.TotalPages)
                break;

            // Get next page of 100
            filter.Page = (uint)messagesPage.Pages.CurrentPage + 1;
            messagesPageResult = await GetMessagesPageAsync(filter.GetQuery());

            if (!messagesPageResult.OK)
            {
                ApiResult<IReadOnlyList<Message>>.CreateError(messagesPageResult.Error);
            }

            messagesPage = messagesPageResult.Data!;
        }

        return ApiResult<IReadOnlyList<Message>>.CreateOK(messages);
    }

    /// <summary>
    /// Сохранение JSON информации о конкретном сообщении в файл.
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<ApiResult<bool>> DownloadMessageJsonAsync(string msgId, string path, bool overwrite = false)
    {
        Logger.Line($"Download {path.PathQuoted()}.");

        if (SkipExisting(path, overwrite))
            return ApiResult<bool>.CreateOK(true);

        string url = Api + $"messages/{msgId}";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            using var file = File.OpenWrite(path);
            await stream.CopyToAsync(file);
            await stream.FlushAsync();
        }

        return ApiResult<bool>.CreateOK(File.Exists(path));
    }

    /// <summary>
    /// Сохранение JSON информации о конкретном сообщении в файл.
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<ApiResult<bool>> AppendMessageJsonAsync(string msgId, FileStream file)
    {
        string url = Api + $"messages/{msgId}";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            await stream.CopyToAsync(file);
            await stream.FlushAsync();

            return ApiResult<bool>.CreateOK(true);
        }

        return ApiResult<bool>.CreateError(response);
    }

    /// <summary>
    /// 3.1.5 Удаление сообщений с учетом фильтра.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>True если все удалены или false если какие-то (или все) могли остаться.</returns>
    public async Task<ApiResult<bool>> DeleteMessagesAsync(MessagesFilter filter)
    {
        var messagesResult = await GetMessagesAsync(filter);

        if (!messagesResult.OK)
        {
            return ApiResult<bool>.CreateError(messagesResult.Error);
        }

        var messages = messagesResult.Data!;
        var ok = true;

        foreach (var message in messages)
        {
            try
            {
                var deleteResult = await DeleteMessageAsync(message.Id);

                if (!deleteResult.OK)
                {
                    ok = false;
                }
            }
            catch
            {
                ok = false;
            }
        }

        return ok
            ? ApiResult<bool>.CreateOK(ok)
            : ApiResult<bool>.CreateError("Deleted files not all.");
    }
}
