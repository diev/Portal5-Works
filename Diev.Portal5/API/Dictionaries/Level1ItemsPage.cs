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

using Diev.Portal5.API.Tools;

namespace Diev.Portal5.API.Dictionaries;

/// <summary>
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries/238d0426-6f57-4c0f-8983-1d1addf8c47a (level 1)
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries/238d0426-6f57-4c0f-8983-1d1addf8c47a?page=1
/// 200 OK
/// </summary>
public record Level1ItemsPage
(
    /// <summary>
    /// Example: "[{...}, ...]"
    /// </summary>
    IReadOnlyList<Level1Item> Items,

    /// <summary>
    /// Example: {
    /// "TotalRecords": 124,
    /// "TotalPages": 2,
    /// "CurrentPage": 1,
    /// "PerCurrentPage": 100,
    /// "PerNextPage": 24,
    /// "MaxPerPage": 100
    /// }
    /// </summary>
    Pagination Pages
);

/*
{
    "Items": [
        {
            "Code": "4",
            "Subjects1": "Банк России",
            "TypeIE": "АД",
            "Id": "6fc60350-fa90-450e-9fea-1b0703501d6a"
        },
        {
            "Code": "3",
            "Subjects1": "Руководство Банка России",
            "TypeIE": "РУ",
            "Id": "4f1f9428-63b0-437f-bdb1-4b25d4f89007"
        },
        {
            "Code": "1",
            "Subjects1": "Центральный аппарат",
            "TypeIE": "ЦА",
            "Id": "41941a8b-a18a-406c-b1a1-eb6546a7e033"
        },
        {
            "Code": "2",
            "Subjects1": "Территориальное учреждение",
            "TypeIE": "ТУ",
            "Id": "eda671ab-7270-4c27-82d9-ed2f7e1c6624"
        }
    ],
    "PaginationInfo": {
        "TotalRecords": 4,
        "TotalPages": 1,
        "CurrentPage": 1,
        "PerCurrentPage": 4,
        "PerNextPage": null,
        "MaxPerPage": 100
    }
}
*/
