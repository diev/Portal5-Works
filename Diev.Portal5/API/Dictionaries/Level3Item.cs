#region License
/*
Copyright 2022-2025 Dmitrii Evdokimov
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
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries/64529d5a-b1d9-453c-96f3-f380ea577314 (level 3)
/// </summary>
public record Level3Item
(
    /// <summary>
    /// Example: "36"
    /// </summary>
    string? Code,

    /// <summary>
    /// Example: "Волго-Вятское ГУ Банка России"
    /// </summary>
    string? Subject,

    /// <summary>
    /// Example: "Волго-Вятское ГУ Банка России"
    /// </summary>
    string? Organization,

    /// <summary>
    /// Example: "Административное управление"
    /// </summary>
    string? Addresse,

    /// <summary>
    /// Example: "22_lk"
    /// </summary>
    string? DirSDS,

    /// <summary>
    /// Example: "ВВГУ"
    /// </summary>
    string? TypeIE2,

    /// <summary>
    /// Example: "ALL"
    /// </summary>
    string? Prefix,

    /// <summary>
    /// Example: "01399905-1fb8-41ad-8053-b0ac00f5e6bd"
    /// </summary>
    string? Id,

    //[JsonPropertyName("row_num")]
    int? Row_Num
);

public static class MockLevel3Item
{
    /// <summary>
    /// dictionaries/64529d5a-b1d9-453c-96f3-f380ea577314[0]
    /// </summary>
    /// <returns></returns>
    public static string Text() =>
        """
        {
            "Code": "106",
            "Subject": "Северо-Западный межрегиональный центр инспектирования ГИ Банка России",
            "Organization": "Северо-Западный межрегиональный центр инспектирования ГИ Банка России",
            "Addressee": "Северо-Западный межрегиональный центр инспектирования ГИ Банка России",
            "DirSDS": "40_lk",
            "TypeIE2": "СЗГУ",
            "Prefix": "ALL",
            "Id": "023c71f8-8fc7-4cee-bcfd-b30200e29356"
        }
        """;
}
