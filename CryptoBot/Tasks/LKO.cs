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

using Diev.Extensions.LogFile;
using Diev.Portal5;
using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Exceptions;

namespace CryptoBot.Tasks;

/// <summary>
/// Zadacha_2-1 ЛК ЕИО (outbox)
/// </summary>
internal static class LKO
{
    //config
    public static string ZipPath { get; }
    public static string DocPath { get; }
    public static string? DocPath2 { get; }
    public static string? Subscribers { get; }

    static LKO()
    {
        var config = Program.Config.GetSection(nameof(LKO));

        ZipPath = Path.GetFullPath(config[nameof(ZipPath)] ?? ".");
        DocPath = Path.GetFullPath(config[nameof(DocPath)] ?? ".");
        DocPath2 = config[nameof(DocPath2)] is null
            ? null
            : Path.GetFullPath(config[nameof(DocPath2)]!);
        Subscribers = config[nameof(Subscribers)];
    }

    public static async Task RunAsync(uint? days)
    {
        try
        {
            var filter = new MessagesFilter
            {
                //Type = MessageType.Outbox,
                Task = "2-1",
                Status = MessageOutStatus.Registered,
                MinDateTime = DateTime.Today.AddDays(-days ?? 0)
            };

            //Logger.TimeZZZLine("Получение списка исходящих сообщений по фильтру");

            var messages = await Messages.GetMessagesAsync(filter);

            if (messages!.Count == 0)
            {
                //throw new TaskException("Получен пустой список сообщений.");
                Logger.TimeZZZLine("Нет сообщений.");
                return;
            }

            Logger.TimeZZZLine($"В списке сообщений {messages.Count} шт.");
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
                    await Program.SendAsync("ЛК ЦБ исх: " + msgInfo.Subject, msgInfo.ToString(), Subscribers);
                }
                else
                {
                    string error = $"Файл сообщения {message.Id}.zip не скачать.";
                    Logger.TimeZZZLine(error);

                    var msgInfo = await Messages.DecryptMessageFilesAsync(message, DocPath, DocPath2);
                    error += Environment.NewLine + msgInfo.Notes;
                    msgInfo.Notes = error;
                    await File.WriteAllTextAsync(zip + ".err", error);

                    await Program.SendFailAsync(nameof(LKO), $"Не скачать файлы к {message.Id}");

                    await Program.SendAsync($"ЛК ЦБ исх: {msgInfo.Subject} [ОШИБКИ]",
                        msgInfo.ToString(), Subscribers);
                }
            }

            Program.RestAPI.SkipExceptions = false;
        }
        catch (Portal5Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(LKO), "API: " + ex.Message);
            Program.ExitCode = 3;
        }
        catch (TaskException ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(LKO), "Task: " + ex.Message);
            Program.ExitCode = 2;
        }
        catch (Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(LKO), ex);
            Program.ExitCode = 1;
        }
    }
}
