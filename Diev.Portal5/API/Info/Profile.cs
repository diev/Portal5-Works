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

namespace Diev.Portal5.API.Info;

/// <summary>
/// 3.1.6.2. Информация о профиле.
/// GET https://portal5.cbr.ru/back/rapi2/profile
/// 200 OK
/// </summary>
public record Profile
(
    /// <summary>
    /// Краткое наименование компании.
    /// </summary>
    string? ShortName,

    /// <summary>
    /// Полное наименование компании.
    /// </summary>
    string? FullName,

    /// <summary>
    /// Список видов деятельностей компании.
    /// </summary>
    Activities[]? Activities,

    /// <summary>
    /// Индивидуальный номер налогоплательщика компании.
    /// </summary>
    string? Inn,

    /// <summary>
    /// Основной государственный регистрационный номер компании.
    /// </summary>
    string? Ogrn,

    /// <summary>
    /// Международный идентификатор (необязательное, зарезервированное поле).
    /// </summary>
    string? InternationalId,

    /// <summary>
    /// Организационно-правовая форма компании.
    /// </summary>
    string? Opf,

    /// <summary>
    /// Электронный адрес компании.
    /// </summary>
    string? Email,

    /// <summary>
    /// Почтовый адрес компании.
    /// </summary>
    string? Address,

    /// <summary>
    /// Контактный телефон компании.
    /// </summary>
    string? Phone,

    /// <summary>
    /// Дата создания ЛК компании.
    /// </summary>
    DateTime CreationDate,

    /// <summary>
    /// Текущий статус ЛК компании.
    /// </summary>
    string? Status
);

/// <summary>
/// Краткое наименование вида деятельности.
/// </summary>
public record Activities
(
    /// <summary>
    /// Полное наименование вида деятельности (необязательное поле).
    /// </summary>
    string? FullName,

    /// <summary>
    /// Краткое наименование вида деятельности (необязательное поле).
    /// </summary>
    string? ShortName,

    /// <summary>
    /// Поднадзорное подразделение:
    /// Name – наименование поднадзорного подразделения Банка России (необязательное поле).
    /// </summary>
    SupervisionDevision? SupervisionDevision // Division?
);

/// <summary>
/// Поднадзорное подразделение.
/// </summary>
public record SupervisionDevision
(
    /// <summary>
    /// Наименование поднадзорного подразделения Банка России (необязательное поле).
    /// </summary>
    string? Name
);

#region Mock
//public static class MockProfile
//{
//    /// <summary>
//    /// profile
//    /// </summary>
//    /// <returns></returns>
//    public static string Text() =>
//        """
//        {
//            "ShortName": "АО \"Банк\"",
//            "FullName": "Акционерное общество \"Банк\"",
//            "Activities": [
//                {
//                    "FullName": "Брокеры",
//                    "ShortName": "Брокеры",
//                    "SupervisionDevision": {
//                        "Name": "Руководство"
//                    }
//                },
//                {
//                    "FullName": "Доверительные управляющие",
//                    "ShortName": "Доверительные управляющие",
//                    "SupervisionDevision": {
//                        "Name": "Руководство"
//                    }
//                },
//                {
//                    "FullName": "Депозитарии",
//                    "ShortName": "Депозитарии",
//                    "SupervisionDevision": {
//                        "Name": "Руководство"
//                    }
//                },
//                {
//                    "FullName": "Банки",
//                    "ShortName": "Кредитные организации",
//                    "SupervisionDevision": {
//                        "Name": "Руководство"
//                    }
//                }
//            ],
//            "Inn": "7831000000",
//            "Ogrn": "1027800000000",
//            "InternationalId": null,
//            "Opf": "Акционерные общества",
//            "Email": "lk-cbr@bank.ru",
//            "Address": "191000, г. Санкт-Петербург",
//            "Phone": "+78120000000",
//            "CreationDate": "0001-01-01T00:00:00Z",
//            "Status": "Active"
//        }
//        """;
//}
#endregion
