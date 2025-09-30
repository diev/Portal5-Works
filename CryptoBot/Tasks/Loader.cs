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
using System.Text.Encodings.Web;
using System.Text.Json;

using CryptoBot.Helpers;

using Diev.Extensions.LogFile;
using Diev.Extensions.Tools;
using Diev.Portal5;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Exceptions;

namespace CryptoBot.Tasks;

internal static class Loader
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
    };

    private const string _task = nameof(Loader);

    //config
    public static string ZipPath { get; }
    public static string DocPath { get; }
    public static string? DocPath2 { get; }
    public static string[] Exclude { get; }
    public static string[] Subscribers { get; }

    static Loader()
    {
        var config = Program.Config.GetSection(_task);

        ZipPath = Path.GetFullPath(config[nameof(ZipPath)] ?? ".");
        DocPath = Path.GetFullPath(config[nameof(DocPath)] ?? ".");
        DocPath2 = config[nameof(DocPath2)] is null
            ? null
            : Path.GetFullPath(config[nameof(DocPath2)]!);

        Exclude = JsonSection.Values(config, nameof(Exclude));
        Subscribers = JsonSection.Subscribers(config);
    }

    public static async Task RunAsync(Guid? guid, MessagesFilter filter)
    {
        try
        {
            if (guid is not null && guid.HasValue)
            {
                await SaveMessageDocsAsync(guid.ToString()!);
                return;
            }

            if (filter.IsEmpty())
                throw new TaskException(
                    "Не задан фильтр сообщений для выполнения операции с ними - это опасно!");

            Logger.TimeZZZLine("Получение списка сообщений по фильтру");

            var messages = await Messages.GetMessagesAsync(filter);

            if (messages!.Count == 0)
            {
                //throw new TaskException("Получен пустой список сообщений.");
                Logger.TimeZZZLine("Нет сообщений.");
                return;
            }

            string text = $"В списке сообщений {messages.Count} шт.";
            Logger.TimeZZZLine(text);
            StringBuilder report = new();
            report
                .AppendLine(text)
                .AppendLine();
            int num = 0;

            // Приступаем к загрузке
            foreach (var message in messages)
            {
                // Пропускаем исходящие без регистрации или успеха
                if (message.Outbox && !message.Registered && !message.Success)
                    continue;

                // Пропускаем игнорируемые задачи
                string task = message.TaskName.Split('_')[1];
                if (Exclude.Contains(task))
                    continue;

                (string json, string zip) = Messages.GetZipStore(message, ZipPath);

                if (!File.Exists(json))
                {
                    if (!await Messages.SaveMessageJsonAsync(message, json))
                        continue; //TODO alert!
                }

                if (File.Exists(zip))
                    continue;

                if (await Messages.SaveMessageZipAsync(message.Id, zip))
                {
                    var msgInfo = await Messages.DecryptMessageZipAsync(message, zip, DocPath, DocPath2);
                    report
                        .AppendLine($"-{++num}-")
                        .AppendLine(msgInfo.ToString())
                        .AppendLine();
                }
                else
                {
                    Logger.TimeZZZLine($"Файл '{message.Id}.zip' не скачать.");

                    var msgInfo = new MessageInfo(message);
                    var corrId = message.CorrelationId;

                    if (!string.IsNullOrEmpty(corrId))
                    {
                        var corrMessage = await Program.RestAPI.GetMessageAsync(corrId);

                        if (corrMessage != null)
                        {
                            var corrInfo = new MessageInfo(corrMessage);
                            msgInfo.Notes += corrInfo.ToString();
                        }
                    }

                    report
                        .AppendLine($"-{++num} не скачать-")
                        .AppendLine(msgInfo.ToString())
                        .AppendLine();
                }
            }

            Program.Done($"ЛК ЦБ: Загружено {num} шт.", report.ToString(), Subscribers);
        }
        catch (NoMessagesException ex)
        {
            Program.Done(_task, ex.Message, Subscribers);
        }
        catch (Portal5Exception ex)
        {
            Program.FailAPI(_task, ex, Subscribers);
        }
        catch (TaskException ex)
        {
            Program.FailTask(_task, ex, Subscribers);
        }
        catch (Exception ex)
        {
            Program.Fail(_task, ex, Subscribers);
        }
    }

    private static async Task SaveMessageDocsAsync(string id)
    {
        Logger.TimeZZZLine($"Получение '{id}'");

        var message = await Program.RestAPI.GetMessageAsync(id)
            ?? throw new TaskException($"Сообщение '{id}' не найдено.");

        (string json, string zip) = Messages.GetZipStore(message, ZipPath);

        if (!File.Exists(json))
        {
            if (!await Messages.SaveMessageJsonAsync(message, json))
                throw new TaskException($"Невозможно сохранить {json.PathQuoted()}.");
        }

        if (File.Exists(zip))
            throw new TaskException($"Файл {zip.PathQuoted()} уже есть.");

        if (!await Messages.SaveMessageZipAsync(id, zip))
            throw new TaskException($"Сообщение '{id}' не скачать.");

        var msgInfo = await Messages.DecryptMessageZipAsync(message, zip, DocPath, DocPath2);

        Console.WriteLine(msgInfo.ToString());
    }
}
