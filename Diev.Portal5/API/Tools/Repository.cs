#region License
/*
Copyright 2022-2026 Dmitrii Evdokimov
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

namespace Diev.Portal5.API.Tools;

/// <summary>
/// Информация о репозиториях (описание репозитория в котором расположен файл. 
/// Данная информация используется как для загрузки файла, так и при его выгрузке).
/// </summary>
public record Repository
(
    /// <summary>
    /// Тип репозитория (значения: aspera, http).<br/>
    /// Example: "http"
    /// </summary>
    string? RepositoryType,

    /// <summary>
    /// IP адрес или имя узла репозитория.<br/>
    /// Example: "https://portal5.cbr.ru"<br/>
    /// Example: "https://portal5test.cbr.ru"
    /// </summary>
    string? Host,

    /// <summary>
    /// Порт для обращения к репозиторию.<br/>
    /// Example: 81<br/>
    /// Example: 443
    /// </summary>
    int? Port,

    /// <summary>
    /// Контрольная сумма файла, необходимая для контроля его целостности. Берется пользователем из «манифеста», формируемого ТПС «Aspera» после загрузки файла.
    /// </summary>
    string? CheckSum,

    /// <summary>
    /// Алгоритм расчёта контрольной суммы файла, в зависимости от установок ТПС «Aspera». Берется пользователем из «манифеста», формируемого ТПС «Aspera» после загрузки файла.
    /// </summary>
    string? CheckSumType,

    /// <summary>
    /// Путь к файлу в репозитории.<br/>
    /// Example: "back/rapi2/messages/6e16a6ad-018f-4136-a8c6-b088010899bc/files/af868884-9e08-4a0c-9f21-bd25a3528085/download"
    /// </summary>
    string? Path
);

#region Mock
//public static class MockRepository
//{
//    public static string Text() =>
//        """
//        [
//            {
//                "RepositoryType": "http",
//                "Host": "https://portal5.cbr.ru",
//                "Port": 81,
//                "Path": "back/rapi2/messages/6fbc3cf9-b48c-4a15-ba8c-b0ad002c489c/files/3d9f1174-ad1d-485e-8149-109ae7353688/download"
//            }
//        ]
//        """;
//}
#endregion
