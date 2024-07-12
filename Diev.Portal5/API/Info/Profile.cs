#region License
/*
Copyright 2022-2024 Dmitrii Evdokimov
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
    IReadOnlyList<Activities>? Activities,

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

/*
{
    "ShortName": "АО \"xxxx xxxxст Банк\"",
    "FullName": "Акционерное общество \"xxxx xxxxст Банк\"",
    "Activities": [
        {
            "FullName": "Депозитарии",
            "ShortName": "Депозитарии",
            "SupervisionDevision": {
                "Name": "Руководство"
            }
        },
        {
            "FullName": "Банки",
            "ShortName": "Кредитные организации",
            "SupervisionDevision": {
                "Name": "Руководство"
            }
        },
        {
            "FullName": "Брокеры",
            "ShortName": "Брокеры",
            "SupervisionDevision": {
                "Name": "Руководство"
            }
        },
        {
            "FullName": "Доверительные управляющие",
            "ShortName": "Доверительные управляющие",
            "SupervisionDevision": {
                "Name": "Руководство"
            }
        }
    ],
    "Inn": "783100xxxx",
    "Ogrn": "102780000xxxx",
    "InternationalId": null,
    "Opf": "Акционерные общества",
    "Email": "xxxx@xxxxnk.ru",
    "Address": "19xxxx, г. Санкт-Петерxxxx, ул.xxxxерная, д.xxxx, литер А. ",
    "Phone": "+7812324xxxx",
    "CreationDate": "0001-01-01T00:00:00Z",
    "Status": "Active"
}
*/
