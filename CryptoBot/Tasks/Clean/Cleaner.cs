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

using Diev.Portal5.API.Tools;
using Diev.Portal5.Interfaces;

using Microsoft.Extensions.Logging;

namespace CryptoBot.Tasks.Clean;

internal class Cleaner(
    ILogger<Cleaner> logger,
    IApiService api,
    IPortalService portal
    ) : ICleaner
{
    public async Task<int> RunAsync(Guid? guid, MessagesFilter filter)
    {
        if (guid is not null && guid.HasValue)
        {
            string id = guid.ToString()!;

            logger.LogInformation("Удаление сообщения '{Id}'", id);

            var result = await api.DeleteMessageAsync(id);

            if (!result.OK)
                throw new TaskException($"Ошибка удаления сообщения '{id}'");

            return 0;
        }

        if (filter.IsEmpty())
            throw new TaskException(
                "Не задан фильтр сообщений для выполнения операции с ними - это опасно!");

        logger.LogInformation("Удаление сообщений по фильтру");

        var messagesResult = await portal.DeleteMessagesAsync(filter);

        return messagesResult.OK
            ? 0
            : 2; //FailTask
    }
}
