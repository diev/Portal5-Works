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
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries/25338cfb-5713-4634-bc53-a81129483752 (level 2)
/// </summary>
public record Level2Item
(
    /// <summary>
    /// Example: "147"
    /// </summary>
    string? Code,

    /// <summary>
    /// Example: "Департамент статистики"
    /// </summary>
    string? Subjects2,

    /// <summary>
    /// Example: "ЦА"
    /// </summary>
    string? TypeIE,

    /// <summary>
    /// Example: ""
    /// </summary>
    string? TypeIE2,

    /// <summary>
    /// Example: "Департамент статистики"
    /// </summary>
    string? Addresse,

    /// <summary>
    /// Example: "48_lk"
    /// </summary>
    string? DirSDS,

    /// <summary>
    /// Example: "Банк России"
    /// </summary>
    string? Organization,

    /// <summary>
    /// Example: "6fc60350-fa90-450e-9fea-1b0703501d6a"
    /// </summary>
    string? Id
);

public static class MockLevel2Item
{
    /// <summary>
    /// dictionaries/25338cfb-5713-4634-bc53-a81129483752[0]
    /// </summary>
    /// <returns></returns>
    public static string Text() =>
        """
        {
            "Code": "127",
            "Subjects2": "Департамент финансовой стабильности",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент финансовой стабильности",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "0000a038-0b54-4b53-a9bb-b2ce00a14930"
        }
        """;
}
