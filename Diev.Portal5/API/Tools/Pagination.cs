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

namespace Diev.Portal5.API.Tools;

/// <summary>
/// Информация о пагинации (разбиении на страницы).
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries/238d0426-6f57-4c0f-8983-1d1addf8c47a (level 1)
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries/25338cfb-5713-4634-bc53-a81129483752 (level 2)
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries/64529d5a-b1d9-453c-96f3-f380ea577314?page=2 (level 3 Addresse)
/// 200 OK
/// </summary>
public record Pagination
(
    /// <summary>
    /// Всего записей в справочнике.
    /// Example (4): 4
    /// Example (124): 124
    /// </summary>
    //[JsonProperty("EPVV-Total")]
    int TotalRecords,

    /// <summary>
    /// Всего страниц с разбивкой не более 100 записей на странице.
    /// Example (4): 1
    /// Example (124): 2
    /// </summary>
    //[JsonProperty("EPVV-TotalPages")]
    int TotalPages,

    /// <summary>
    /// Текущая страница (соответствует n из запроса page={n}).
    /// Example (4): 1
    /// Example (124): 1
    /// </summary>
    //[JsonProperty("EPVV-CurrentPage")]
    int CurrentPage,

    /// <summary>
    /// Число записей на текущей странице или null, если запрошенная страница не существует.
    /// Example (4): 4
    /// Example (124): 100
    /// </summary>
    //[JsonProperty("EPVV-PerCurrentPage")]
    int? PerCurrentPage,

    /// <summary>
    /// Число записей на следующей страница или null, если страница не существует.
    /// Example (4): null
    /// Example (124): 24
    /// </summary>
    //[JsonProperty("EPVV-PerNextPage")]
    int? PerNextPage,

    /// <summary>
    /// Максимальное количество записей на странице (всегда 100).
    /// Example: 100
    /// </summary>
    //[JsonProperty("EPVV-MaxPerPage")]
    int MaxPerPage
);

/*
{
    "TotalRecords": 124,
    "TotalPages": 2,
    "CurrentPage": 1,
    "PerCurrentPage": 100,
    "PerNextPage": 24,
    "MaxPerPage": 100
}

{
    "TotalRecords": 124,
    "TotalPages": 2,
    "CurrentPage": 2,
    "PerCurrentPage": 24,
    "PerNextPage": null,
    "MaxPerPage": 100
}
*/
