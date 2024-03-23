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

using Diev.Portal5.API.Tools;

namespace CryptoBot.Tasks;

internal static class MessagesClean
{
    public static async Task RunAsync()
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

        foreach (string task in tasks)
        {
            await DeleteByTask(task);
        }
    }

    private static async Task DeleteByTask(string task, int leaveDays = 30)
    {
        var filter = new MessagesFilter()
        {
            Task = task,
            MaxDateTime = DateTime.Now.AddDays(-leaveDays)
        };

        await Program.RestAPI.DeleteMessagesAsync(filter);
    }
}
