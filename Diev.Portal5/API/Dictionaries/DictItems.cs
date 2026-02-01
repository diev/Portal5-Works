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
/// Записи справочника с информацией о пагинации.<br/>
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries<br/>
/// 200 OK
/// </summary>
public record DictItems
(
    /// <summary>
    /// Массив записей справочника, в зависимости от его структуры.
    /// В массиве возвращаются записи справочника со статусом не равным «удален»,
    /// не более 100 за один запрос.
    /// Example: "[{...}, ...]"
    /// </summary>
    DictItem[] Items
);

#region Mock
//public static class MockDictItems
//{
//    /// <summary>
//    /// dictionaries
//    /// </summary>
//    /// <returns></returns>
//    public static string Info() =>
//        """
//        [
//            {
//                "Id": "238d0426-6f57-4c0f-8983-1d1addf8c47a",
//                "Text": "Справочник Тематики 1 уровня",
//                "Date": "2019-07-06T16:56:53Z"
//            },
//            {
//                "Id": "e814a9b9-aaf9-4a99-9508-6c9083f3097b",
//                "Text": "ОКВЭД2",
//                "Date": "2025-04-09T08:57:21Z"
//            },
//            {
//                "Id": "59005487-72d0-4126-a5bd-9dda11901da8",
//                "Text": "Расписание депозитных операций",
//                "Date": "2025-06-03T06:41:18Z"
//            },
//            {
//                "Id": "e436b291-b8ed-4aa4-bce5-279de941a36e",
//                "Text": "ОКВ",
//                "Date": "2018-11-13T12:36:26Z"
//            },
//            {
//                "Id": "b9fde2e8-4a8d-4247-830f-5ef395402a96",
//                "Text": "ОКТМО",
//                "Date": "2018-11-13T12:36:26Z"
//            },
//            {
//                "Id": "f77045f7-b8a5-4309-b4d5-389ecef88291",
//                "Text": "Справочник видов документа ЭФЮДКО",
//                "Date": "2025-04-03T09:32:26Z"
//            },
//            {
//                "Id": "acf79d8a-8ef8-4987-ae6a-921df279c3f3",
//                "Text": "ОКФС",
//                "Date": "2018-11-13T12:36:26Z"
//            },
//            {
//                "Id": "8a6a8d3b-c726-4a94-9fed-97d19ea8d202",
//                "Text": "Справочник субъектов РФ",
//                "Date": "2023-02-07T14:58:18Z"
//            },
//            {
//                "Id": "3d28828b-ddcb-47e6-a2ef-9a34a7ad9f98",
//                "Text": "Пользовательский справочник реквизитов КО / филиала КО v.2",
//                "Date": "2019-04-02T13:32:22Z"
//            },
//            {
//                "Id": "abbbb815-64a5-440c-9852-43f884ee124f",
//                "Text": "Рубрики САДД для формы",
//                "Date": "2020-02-19T14:25:38Z"
//            },
//            {
//                "Id": "aef99b32-dc50-4cbe-80e0-d326846a2954",
//                "Text": "Тематика обращения",
//                "Date": "2023-09-30T16:53:32Z"
//            },
//            {
//                "Id": "f58f4b2b-b53b-4083-8205-db37283ad93c",
//                "Text": "OKOPF",
//                "Date": "2020-02-10T14:25:04Z"
//            },
//            {
//                "Id": "1d5e518f-1c69-4864-926c-149834cd783f",
//                "Text": "Справочник должностей",
//                "Date": "2024-07-19T12:44:19Z"
//            },
//            {
//                "Id": "0a991e18-46d3-427b-b4c6-e9ce79cef94b",
//                "Text": "ОКСМ",
//                "Date": "2018-11-13T12:36:26Z"
//            },
//            {
//                "Id": "25338cfb-5713-4634-bc53-a81129483752",
//                "Text": "Справочник адресатов 2 уровня",
//                "Date": "2025-04-29T09:47:13Z"
//            },
//            {
//                "Id": "f56c8cb1-61d1-49f0-8b8e-f4fb7acab9ca",
//                "Text": "Полномочия по доверенностям",
//                "Date": "2021-12-18T11:40:18Z"
//            },
//            {
//                "Id": "a04c22fb-ab4f-494c-be23-b21fd585d472",
//                "Text": "DictContractTypes",
//                "Date": "2024-08-30T13:15:56Z"
//            },
//            {
//                "Id": "e88c4281-7109-438e-b72b-139fe82308a1",
//                "Text": "Расписание кредитных операций",
//                "Date": "2025-06-25T03:32:01Z"
//            },
//            {
//                "Id": "64529d5a-b1d9-453c-96f3-f380ea577314",
//                "Text": "Справочник адресатов 3 уровня",
//                "Date": "2025-06-20T13:44:55Z"
//            },
//            {
//                "Id": "17d9d138-b50c-4a85-80db-6cea7908ac9f",
//                "Text": "Информация о ценных бумагах",
//                "Date": "2025-06-24T17:02:12Z"
//            },
//            {
//                "Id": "3e8b9f5c-af57-4724-8286-873968c48ef7",
//                "Text": "ЦСО",
//                "Date": "2025-06-24T18:40:51Z"
//            }
//        ]
//        """;
//}
#endregion
