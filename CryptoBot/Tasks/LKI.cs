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
    //config
    public static string ZipPath { get; }
    public static string DocPath { get; }
    public static string? DocPath2 { get; }
    public static string? Subscribers { get; }

    static LKI()
    {
        var config = Program.Config.GetSection(nameof(LKI));

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

                    await Program.SendAsync($"ЛК ЦБ вх: {msgInfo.Subject}",
                        msgInfo.ToString(), Subscribers, files);

                    //string temp = Files.CreateTempDir();
                    //var msgInfo = await Messages.DecryptMessageZipAsync(message, zip, DocPath);

                    //string repack = Path.Combine(temp, msgInfo.Name + ".zip");
                    //await Files.ZipDirectoryAsync(msgInfo.FullName!, repack);

                    //string url = Path.Combine(temp, msgInfo.Name + ".url");
                    //await File.WriteAllTextAsync(url, Files.MakeUrl(msgInfo.FullName!));

                    //var attach = new string[] { repack, url };
                    //await Program.SendAsync("ЛК ЦБ: " + msgInfo.Subject, msgInfo.ToString(), Subscribers, attach);
                    //Directory.Delete(temp, true);
                }
                else
                {
                    string error = $"Файл сообщения {message.Id}.zip не скачать.";
                    Logger.TimeZZZLine(error);

                    //var msgInfo = new MessageInfo(message);
                    var msgInfo = await Messages.DecryptMessageFilesAsync(message, DocPath, DocPath2);
                    error += Environment.NewLine + msgInfo.Notes;
                    msgInfo.Notes = error;
                    await File.WriteAllTextAsync(zip + ".err", error);

                    await Program.SendFailAsync(nameof(LKI), $"Не скачать файлы к {message.Id}");

                    await Program.SendAsync($"ЛК ЦБ вх: {msgInfo.Subject} [ОШИБКИ]",
                        msgInfo.ToString(), Subscribers);
                }
            }

            Program.RestAPI.SkipExceptions = false;
            Logger.TimeZZZLine("Список обработан.");
        }
        catch (Portal5Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(LKI), "API: " + ex.Message);
            Program.ExitCode = 3;
        }
        catch (TaskException ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(LKI), "Task: " + ex.Message);
            Program.ExitCode = 2;
        }
        catch (Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(LKI), ex);
            Program.ExitCode = 1;
        }
    }
}
