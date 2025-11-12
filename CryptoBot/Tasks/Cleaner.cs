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

using Diev.Extensions.LogFile;
using Diev.Portal5.API.Tools;

namespace CryptoBot.Tasks;

internal class Cleaner
{
    public async Task<int> RunAsync(Guid? guid, MessagesFilter filter)
    {
        if (guid is not null && guid.HasValue)
        {
            string id = guid.ToString()!;

            Logger.TimeZZZLine($"Удаление сообщения '{id}'");

            if (!await Program.RestAPI.DeleteMessageAsync(id))
                throw new TaskException($"Ошибка удаления сообщения '{id}'.");

            return 0;
        }

        if (filter.IsEmpty())
            throw new TaskException(
                "Не задан фильтр сообщений для выполнения операции с ними - это опасно!");

        Logger.TimeZZZLine("Удаление сообщений по фильтру");

        return await Program.RestAPI.DeleteMessagesAsync(filter)
            ? 0
            : 2; //FailTask
    }
}
