#region License
/*
Copyright 2022-2023 Dmitrii Evdokimov
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

namespace Diev.Portal5.API;

public class ProfileInfo
{
    /// <summary>
    /// Краткое наименование компании.
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// Полное наименование компании.
    /// </summary>
    public string FullName { get; set; }
    
    /// <summary>
    /// Список видов деятельностей компании.
    /// </summary>
    public List<Activities> Activities { get; set; }

    /// <summary>
    /// Индивидуальный номер налогоплательщика компании.
    /// </summary>
    public int Inn { get; set; }

    /// <summary>
    /// Основной государственный регистрационный номер компании.
    /// </summary>
    public int Ogrn { get; set; }

    /// <summary>
    /// Международный идентификатор (необязательное, зарезервированное поле).
    /// </summary>
    public int? InternationalId { get; set; }

    /// <summary>
    /// Организационно-правовая форма компании.
    /// </summary>
    public string Opf { get; set; }

    /// <summary>
    /// Электронный адрес компании.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Почтовый адрес компании.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Контактный телефон компании.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Дата создания ЛК компании.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Текущий статус ЛК компании.
    /// </summary>
    public string Status { get; set; }
}

/*
{
    "ShortName": "АО \"Сити Инвест Банк\"",
    "FullName": "Акционерное общество \"Сити Инвест Банк\"",
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
    "Inn": "7831001422",
    "Ogrn": "1027800000095",
    "InternationalId": null,
    "Opf": "Акционерные общества",
    "Email": "lk-cbr@cibank.ru",
    "Address": "191187, г. Санкт-Петербург, ул.Шпалерная, д. 2/4, литер А. ",
    "Phone": "+78123240690",
    "CreationDate": "0001-01-01T00:00:00Z",
    "Status": "Active"
}
*/
