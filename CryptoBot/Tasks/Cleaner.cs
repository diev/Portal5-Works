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
using Diev.Portal5.Exceptions;

namespace CryptoBot.Tasks;

internal static class Cleaner
{
    private const string _task = nameof(Cleaner);

    public static async Task RunAsync(Guid? guid, MessagesFilter filter)
    {
        try
        {
            if (guid is not null && guid.HasValue)
            {
                string id = guid.ToString()!;

                Logger.TimeZZZLine($"Удаление сообщения '{id}'");

                if (!await Program.RestAPI.DeleteMessageAsync(id))
                    throw new TaskException($"Ошибка удаления сообщения '{id}'.");
                return;
            }

            if (filter.IsEmpty())
                throw new TaskException(
                    "Не задан фильтр сообщений для выполнения операции с ними - это опасно!");

            Logger.TimeZZZLine("Удаление сообщений по фильтру");

            await Program.RestAPI.DeleteMessagesAsync(filter);
        }
        catch (Portal5Exception ex)
        {
            Program.FailAPI(_task, ex);
        }
        catch (TaskException ex)
        {
            Program.FailTask(_task, ex);
        }
        catch (Exception ex)
        {
            Program.Fail(_task, ex);
        }
    }
}
