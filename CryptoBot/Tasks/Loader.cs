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

using CryptoBot;
using CryptoBot.Helpers;

using Diev.Extensions.LogFile;
using Diev.Extensions.Tools;
using Diev.Portal5;
using Diev.Portal5.API.Tools;

using Microsoft.Extensions.Configuration;

namespace CryptoBot.Tasks;

internal class Loader(string zipPath, string docPath, string? docPath2, string[] exclude,
    //string[] subscribers, string[] iSubscribers, string[] oSubscribers)
    IConfigurationSection subscribers)
{
    //private static readonly JsonSerializerOptions _jsonOptions = new()
    //{
    //    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    //    WriteIndented = true
    //};

    public async Task<int> RunAsync(Guid? guid, MessagesFilter filter, bool lk = false)
    {
        int errors = 0;

        if (guid is not null && guid.HasValue)
        {
            await SaveMessageDocsAsync(guid.ToString()!);
            return 0;
        }

        //if (filter.IsEmpty())
        //    throw new TaskException(
        //        "Не задан фильтр сообщений для выполнения операции с ними - это опасно!");

        Logger.TimeZZZLine("Получение списка сообщений по фильтру");

        //var messages = await Messages.GetMessagesAsync(filter);
        var messagesPage = await Messages.GetMessagesPageAsync(filter); // up to 100 messages

        //if (messages!.Count == 0)
        if (messagesPage.Messages.Count == 0)
        {
            //throw new TaskException("Получен пустой список сообщений.");
            Logger.TimeZZZLine("Нет сообщений.");
            return 0;
        }

        string text = $"В списке сообщений {messagesPage.Messages.Count} шт., {messagesPage.Pages.TotalPages} стр.";
        Logger.TimeZZZLine(text);
        StringBuilder report = new();
        report
            .AppendLine(text)
            .AppendLine();
        int num = 0;

        // Приступаем к загрузке
        while (messagesPage.Messages.Count > 0)
        {
            foreach (var message in messagesPage.Messages)
            {
                // Пропускаем исходящие без регистрации или успеха
                if (message.Outbox && !message.Registered && !message.Success)
                    continue;

                // Пропускаем игнорируемые задачи
                string task = message.TaskName.Split('_')[1];
                if (exclude.Contains(task))
                    continue;

                (string json, string zip) = Messages.GetZipStore(message, zipPath);

                if (!File.Exists(json))
                {
                    if (!await Messages.SaveMessageJsonAsync(message, json))
                        continue; //TODO alert!
                }

                if (File.Exists(zip) || File.Exists(zip + ".err"))
                    continue;

                MessageInfo msgInfo;

                if (await Messages.SaveMessageZipAsync(message.Id, zip))
                {
                    msgInfo = await Messages.DecryptMessageZipAsync(message, zip, docPath);
                    string msgInfoText = msgInfo.ToString();

                    if (lk)
                    {
                        if (subscribers.GetSection(message.TaskName) is null)
                        {
                            Notifications.Send("ЛК ЦБ NEW: " + msgInfo.Subject, msgInfoText);
                        }
                        else
                        {
                            var taskSubscribers = JsonSection.Values(subscribers, message.TaskName);

                            if (taskSubscribers.Length > 0)
                            {
                                if (message.Inbox)
                                {
                                    string docs = msgInfo.FullName!;
                                    string pdf = Path.Combine(docs, "ВизуализацияЭД.PDF");

                                    var files = File.Exists(pdf)
                                        ? [pdf]
                                        : Directory.GetFiles(docs, "*.pdf");

                                    Notifications.Send($"ЛК ЦБ вх: {msgInfo.Subject}",
                                        msgInfoText, taskSubscribers, files);
                                }
                                else
                                {
                                    Notifications.Send("ЛК ЦБ исх: " + msgInfo.Subject,
                                        msgInfoText, taskSubscribers);
                                }
                            }
                        }
                    }
                    else
                    {
                        report
                            .AppendLine($"-{++num}-")
                            .AppendLine(msgInfoText)
                            .AppendLine();
                    }
                }
                else
                {
                    string error = $"Файл сообщения '{message.Id}.zip' не скачать.";
                    Logger.TimeZZZLine(error);

                    msgInfo = await Messages.DecryptMessageFilesAsync(message, docPath);
                    error += Environment.NewLine + msgInfo.Notes;
                    msgInfo.Notes += error;
                    string msgInfoText = msgInfo.ToString();

                    report
                        .AppendLine($"-{++num} не скачать-")
                        .AppendLine(msgInfoText)
                        .AppendLine();

                    await File.WriteAllTextAsync(zip + ".err", error);
                    errors++;
                }

                if (docPath2 is not null)
                {
                    string docs2 = Messages.GetDocStore2(message, msgInfo, docPath2);
                    Directory.CreateDirectory(docs2);

                    foreach (var file in Directory.GetFiles(docPath))
                        File.Copy(file, Path.Combine(docs2, Path.GetFileName(file)), true);
                }
            }

            // Exit if last page
            if (messagesPage.Pages.CurrentPage == messagesPage.Pages.TotalPages)
                break;

            // Get next page of 100
            filter.Page = (uint)messagesPage.Pages.CurrentPage + 1;
            messagesPage = await Messages.GetMessagesPageAsync(filter);
        }

        // Загрузка окончена
        string reportText = report.ToString();

        if (lk)
        {
            return errors == 0
                ? 0
                : Notifications.Fail(null, reportText);
        }
        else
        {
            return Notifications.Done($"ЛК ЦБ: Загружено {num} шт., ошибок {errors}", reportText);
        }
    }

    private async Task SaveMessageDocsAsync(string id)
    {
        Logger.TimeZZZLine($"Получение '{id}'");

        var messageResult = await Program.RestAPI.GetMessageAsync(id);

        if (!messageResult.OK)
            throw new TaskException($"Сообщение '{id}' не найдено.");

        var message = messageResult.Data!;
        (string json, string zip) = Messages.GetZipStore(message, zipPath);

        if (!File.Exists(json))
        {
            if (!await Messages.SaveMessageJsonAsync(message, json))
                throw new TaskException($"Невозможно сохранить {json.PathQuoted()}.");
        }

        if (File.Exists(zip))
            throw new TaskException($"Файл {zip.PathQuoted()} уже есть.");

        if (!await Messages.SaveMessageZipAsync(id, zip))
            throw new TaskException($"Сообщение '{id}' не скачать.");

        var msgInfo = await Messages.DecryptMessageZipAsync(message, zip, docPath);

        if (docPath2 is not null)
        {
            string docs2 = Messages.GetDocStore2(message, msgInfo, docPath2);
            Directory.CreateDirectory(docs2);

            foreach (var file in Directory.GetFiles(docPath))
                File.Copy(file, Path.Combine(docs2, Path.GetFileName(file)), true);
        }

        Console.WriteLine(msgInfo.ToString());
    }
}
