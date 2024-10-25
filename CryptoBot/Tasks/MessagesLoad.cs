#region License
/*
Copyright 2024 Dmitrii Evdokimov
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

using Diev.Extensions.LogFile;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Exceptions;

namespace CryptoBot.Tasks;

internal static class MessagesLoad
{
    public static async Task RunAsync(string? msgId, MessagesFilter filter)
    {
        if (msgId is not null)
        {
            await MessageLoad.RunAsync(msgId);
            return;
        }

        try
        {
            //GET: */messages?Type=inbox&Status=read&Page=2
            // получить прочитанные Входящие

            //GET: */messages?Task=Zadacha_2-1&Status=registered
            // получить все документы, отправленные участником и зарегистрированные Банком России

            var messagesPage = await Program.RestAPI.GetMessagesPageAsync(filter)
                ?? throw new Exception("Не получено сообщений.");

            if (messagesPage.Messages.Count == 0)
            {
                //throw new Exception("Получен пустой список сообщений.");
                Console.WriteLine("Нет сообщений.");
                Logger.TimeLine("No messages.");
                return;
            }

            Console.WriteLine($"Сообщений: {messagesPage.Pages.TotalRecords}.");
            filter.Page ??= 1;

            while (true)
            {
                Console.WriteLine($"--- Page {filter.Page} ---");

                foreach (var message in messagesPage!.Messages)
                {
                    if (SkipZadacha(message.TaskName!))
                        continue;

                    await MessageLoad.LoadMessageAsync(message);
                }

                if (messagesPage.Pages.CurrentPage == messagesPage.Pages.TotalPages)
                    break;

                filter.Page++;
                messagesPage = await Program.RestAPI.GetMessagesPageAsync(filter);
            }

            Console.WriteLine("--- Page end ---");
        }
        catch (Portal5Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(MessagesLoad), "API: " + ex.Message, MessageLoad.Subscribers);
            Program.ExitCode = 3;
        }
        catch (TaskException ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(MessagesLoad), "Task: " + ex.Message, MessageLoad.Subscribers);
            Program.ExitCode = 2;
        }
        catch (Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(MessagesLoad), ex, MessageLoad.Subscribers);
            Program.ExitCode = 1;
        }
    }

    private static bool SkipZadacha(string Zadacha)
    {
        string[] tasks = [
            // inbox
            "Zadacha_97",  // Извещение о результатах контроля информации о ВПОДК и их результатах (ИЭС1)
            "Zadacha_107", // Извещение о результатах контроля отчетности субъектов НПС (ИЭС1)
            "Zadacha_114", // Извещение о результатах контроля представления формы 0409310 (ИЭС1)
            "Zadacha_123", // Извещение о результатах контроля представления формы 0409310 (ИЭС2)
            "Zadacha_130", // Получение информации об уровне риска ЮЛ/ИП
            "Zadacha_133", // Извещение о результатах контроля отчетности субъектов НПС (ИЭС2)
            "Zadacha_140", // Извещение о результатах контроля информации о ВПОДК и их результатах (ИЭС2)
            "Zadacha_156", // Извещение о результатах контроля представления формы 0409601 отчет ко(нко) (ИЭС1) и др.
            "Zadacha_159", // Извещение о результатах контроля представления формы 0409601 отчет ко(нко) (ИЭС2) и др.
            // outbox
            "Zadacha_155"  // Представление отчетности КО в Банк России
            ];

        if (tasks.Contains(Zadacha))
            return true;

        return false;
    }
}
