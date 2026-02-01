#region License
/*
Copyright 2024-2025 Dmitrii Evdokimov
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

using System.Text;

using CryptoBot.Helpers;
using CryptoBot.Services;

using Diev.Extensions.Tools;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Interfaces;
using Diev.Portal5.Tools;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CryptoBot.Tasks.Load;

internal class Loader(
    ILogger<Loader> logger,
    IOptions<LoaderSettings> options,
    IPathService paths,
    IApiService api,
    IPortalService portal,
    IConfiguration config,
    INotifyService notifier) : ILoader
{
    //private static readonly JsonSerializerOptions _jsonOptions = new()
    //{
    //    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    //    WriteIndented = true
    //};

    private readonly string zipPath = options.Value.ZipPath;
    private readonly string docPath = options.Value.DocPath;

    public async Task<int> RunAsync(Guid? guid, MessagesFilter filter, bool lk = false)
    {
        int errors = 0;

        if (guid is not null && guid.HasValue)
        {
            await SaveMessageDocsAsync(guid.ToString()!);
            return 0;
        }

        List<string> exclude = [];

        foreach (var item in options.Value.Exclude)
        {
            exclude.Add($"Zadacha_{item}");
        }

        //if (filter.IsEmpty())
        //    throw new TaskException(
        //        "Не задан фильтр сообщений для выполнения операции с ними - это опасно!");

        logger.LogTrace("Получение списка сообщений по фильтру");

        //var messages = await Messages.GetMessagesAsync(filter);
        var result = await api.GetMessagesPageAsync(filter); // up to 100 messages

        if (!result.OK)
        {
            if (result.Error!.HTTPStatus == 401)
            {
                logger.LogCritical("В доступе отказано {Status}", result.Error.HTTPStatus);
                return 2;
            }

            logger.LogCritical("Ничего не получено {Status}", result.Error.HTTPStatus);
            return 1;
        }

        var messagesPage = result.Data!;

        if (messagesPage.Messages.Length == 0)
        {
            logger.LogInformation("Нет сообщений");
            return 0;
        }

        logger.LogInformation("В списке сообщений {Pages} стр. по {Messages} шт.",
            messagesPage.Pages.TotalPages,
            messagesPage.Messages.Length);

        StringBuilder report = new();
        report
            .Append($"В списке сообщений {messagesPage.Pages.TotalPages} стр.")
            .AppendLine($" по {messagesPage.Messages.Length} шт.")
            .AppendLine();
        int num = 0;

        // Приступаем к загрузке
        while (messagesPage!.Messages.Length > 0)
        {
            foreach (var message in messagesPage.Messages)
            {
                // Пропускаем исходящие без регистрации или успеха
                if (message.Outbox && !message.Registered && !message.Success)
                    continue;

                // Пропускаем игнорируемые задачи
                if (exclude.Contains(message.TaskName))
                    continue;

                (string json, string zip) = paths.GetZipStore(message, zipPath);

                if (!File.Exists(json))
                {
                    if (!(await portal.SaveMessageJsonAsync(message, json)).OK)
                        continue; //TODO alert!
                }

                if (File.Exists(zip) || File.Exists(zip + ".err"))
                    continue;

                MessageInfo msgInfo;

                if ((await portal.SaveMessageZipAsync(message.Id, zip)).OK)
                {
                    var resulti = await portal.DecryptMessageZipAsync(message, zip, docPath);
                    msgInfo = resulti.Data!;
                    string msgInfoText = msgInfo.ToString();

                    if (lk)
                    {
                        string key = $"{nameof(Loader)}:{message.TaskName}";
                        var subscribers = JsonSection.Subscribers(config, key);

                        if (message.Inbox)
                        {
                            string docs = msgInfo.FullName!;
                            string pdf = Path.Combine(docs, "ВизуализацияЭД.PDF");

                            var files = File.Exists(pdf)
                                ? [pdf]
                                : Directory.GetFiles(docs, "*.pdf");

                            await notifier.SendAsync($"ЛК ЦБ вх: {msgInfo.Subject}",
                                msgInfoText, subscribers, files);
                        }
                        else
                        {
                            await notifier.SendAsync("ЛК ЦБ исх: " + msgInfo.Subject,
                                msgInfoText, subscribers);
                        }
                    }
                    else // batch
                    {
                        report
                            .AppendLine($"-{++num}-")
                            .AppendLine(msgInfoText)
                            .AppendLine();
                    }
                }
                else // no zip
                {
                    logger.LogWarning("Файл сообщения '{Id}.zip' не скачать", message.Id);

                    string error = $"Файл сообщения '{message.Id}.zip' не скачать";
                    var resulti = await portal.DecryptMessageFilesAsync(message, docPath);

                    if (resulti.OK)
                    {
                        msgInfo = resulti.Data!;

                        error += Environment.NewLine + msgInfo.Notes;
                        msgInfo.Notes += error;
                        string msgInfoText = msgInfo.ToString();

                        report
                            .AppendLine($"-{++num} не скачать-")
                            .AppendLine(msgInfoText)
                            .AppendLine();
                    }
                    else
                    {
                        report
                            .AppendLine($"-{++num} не скачать-")
                            .AppendLine("Нет дополнительной информации")
                            .AppendLine();
                    }

                    await File.WriteAllTextAsync(zip + ".err", error);
                    errors++;
                }

                //if (docPath2 is not null)
                //{
                //    string docs2 = paths.GetDocStore2(message, msgInfo, docPath);
                //    Directory.CreateDirectory(docs2);

                //    foreach (var file in Directory.GetFiles(docPath))
                //        File.Copy(file, Path.Combine(docs2, Path.GetFileName(file)), true);
                //}
            }

            // Exit if last page
            if (messagesPage.Pages.CurrentPage == messagesPage.Pages.TotalPages)
                break;

            // Get next page of 100
            filter.Page = (uint)messagesPage.Pages.CurrentPage + 1;
            messagesPage = (await api.GetMessagesPageAsync(filter)).Data; //TODO error!
        }

        // Загрузка окончена
        string reportText = report.ToString();

        if (lk)
        {
            return errors == 0
                ? 0
                : await notifier.FailAsync(null, reportText);
        }
        else
        {
            return await notifier.DoneAsync($"ЛК ЦБ: Загружено {num} шт., ошибок {errors}", reportText);
        }
    }

    private async Task SaveMessageDocsAsync(string id)
    {
        logger.LogInformation("Получение '{Id}'", id);

        var result = await api.GetMessageAsync(id);

        if (!result.OK)
            throw new TaskException($"Сообщение '{id}' не найдено");

        var message = result.Data!;
        (string json, string zip) = paths.GetZipStore(message, zipPath);

        if (!File.Exists(json))
        {
            if (!(await portal.SaveMessageJsonAsync(message, json)).OK)
                throw new TaskException($"Невозможно сохранить {json.PathQuoted()}");
        }

        if (File.Exists(zip))
            throw new TaskException($"Файл {zip.PathQuoted()} уже есть");

        if (!(await portal.SaveMessageZipAsync(id, zip)).OK)
            throw new TaskException($"Сообщение '{id}' не скачать");

        var resultd = await portal.DecryptMessageZipAsync(message, zip, docPath);

        //if (docPath2 is not null)
        //{
        //    string docs2 = paths.GetDocStore2(message, msgInfo, docPath2);
        //    Directory.CreateDirectory(docs2);

        //    foreach (var file in Directory.GetFiles(docPath))
        //        File.Copy(file, Path.Combine(docs2, Path.GetFileName(file)), true);
        //}

        if (!resultd.OK)
        {
            var msgInfo = resultd.Data!;
            Console.WriteLine(msgInfo.ToString());
        }
    }
}
