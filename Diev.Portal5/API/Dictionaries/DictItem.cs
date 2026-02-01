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

namespace Diev.Portal5.API.Dictionaries;

/// <summary>
/// 3.1.6.5. Список справочников.<br/>
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries
/// </summary>
public record DictItem
(
    /// <summary>
    /// Уникальный идентификатор справочника, используется для идентификации задачи.
    /// Example: "e88c4281-7109-438e-b72b-139fe82308a1"
    /// </summary>
    string Id,

    /// <summary>
    /// Текстовое наименование справочника.
    /// Example: "Расписание кредитных операций"
    /// </summary>
    string Text,

    /// <summary>
    /// Дата последнего обновления справочника.
    /// Example: "2023-11-09T03:22:00Z"
    /// </summary>
    string Date
);

#region Mock
//public static class MockDictItem
//{
//    /// <summary>
//    /// dictionaries[0]
//    /// </summary>
//    /// <returns></returns>
//    public static string Text() =>
//        """
//        {
//            "Id": "238d0426-6f57-4c0f-8983-1d1addf8c47a",
//            "Text": "Справочник Тематики 1 уровня",
//            "Date": "2019-07-06T16:56:53Z"
//        }
//        """;
//}
#endregion
