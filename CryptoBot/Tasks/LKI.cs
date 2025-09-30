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

using CryptoBot.Helpers;

using Diev.Extensions.LogFile;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Exceptions;

namespace CryptoBot.Tasks;

/// <summary>
/// Zadacha_3-1 ЛК ЕИО (inbox)
/// </summary>
internal static class LKI
{
    private const string _task = nameof(LKI);

    //config
    public static string ZipPath { get; }
    public static string DocPath { get; }
    public static string? DocPath2 { get; }
    public static string[] Subscribers { get; }

    static LKI()
    {
        var config = Program.Config.GetSection(_task);

        ZipPath = Path.GetFullPath(config[nameof(ZipPath)] ?? ".");
        DocPath = Path.GetFullPath(config[nameof(DocPath)] ?? ".");
        DocPath2 = config[nameof(DocPath2)] is null
            ? null
            : Path.GetFullPath(config[nameof(DocPath2)]!);

        Subscribers = JsonSection.Subscribers(config);
    }

    public static async Task RunAsync(uint? days)
    {
        try
        {
            var filter = new MessagesFilter
            {
                //Type = MessageType.Inbox,
                Task = "3-1",
                //Status = MessageInStatus.New,
                MinDateTime = DateTime.Today.AddDays(-days ?? 0)
            };

            //Logger.TimeZZZLine("Получение списка сообщений по фильтру");

            var messages = await Messages.GetMessagesAsync(filter);

            if (messages!.Count == 0)
            {
                //throw new TaskException("Получен пустой список сообщений.");
                Logger.TimeZZZLine("Нет сообщений.");
                return;
            }

            string text = $"В списке сообщений {messages.Count} шт.";
            Logger.TimeZZZLine(text);
            Program.RestAPI.SkipExceptions = true;

            // Приступаем к загрузке
            foreach (var message in messages)
            {
                (string json, string zip) = Messages.GetZipStore(message, ZipPath);

                if (!File.Exists(json))
                {
                    if (!await Messages.SaveMessageJsonAsync(message, json))
                        continue; //TODO alert!
                }

                if (File.Exists(zip) || File.Exists(zip + ".err"))
                    continue;

                if (await Messages.SaveMessageZipAsync(message.Id, zip))
                {
                    var msgInfo = await Messages.DecryptMessageZipAsync(message, zip, DocPath, DocPath2);
                    string docs = msgInfo.FullName!;
                    string pdf = Path.Combine(docs, "ВизуализацияЭД.PDF");

                    var files = File.Exists(pdf)
                        ? [pdf]
                        : Directory.GetFiles(docs, "*.pdf");

                    Program.Send($"ЛК ЦБ вх: {msgInfo.Subject}",
                        msgInfo.ToString(), Subscribers, files);
                }
                else
                {
                    string error = $"Файл сообщения {message.Id}.zip не скачать.";
                    Logger.TimeZZZLine(error);

                    //var msgInfo = new MessageInfo(message);
                    var msgInfo = await Messages.DecryptMessageFilesAsync(message, DocPath, DocPath2);
                    error += Environment.NewLine + msgInfo.Notes;
                    msgInfo.Notes += error;
                    await File.WriteAllTextAsync(zip + ".err", error);

                    Program.Send($"ЛК ЦБ вх: {msgInfo.Subject} [ОШИБКИ]",
                        msgInfo.ToString(), Subscribers);

                    Program.Fail(_task, $"Не скачать файлы к {message.Id}");
                }
            }

            Program.RestAPI.SkipExceptions = false;
            Logger.TimeZZZLine("Список обработан.");
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
}
