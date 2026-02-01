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
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries/238d0426-6f57-4c0f-8983-1d1addf8c47a (level 1)<br/>
/// 1: Центральный аппарат (ЦА)<br/>
/// 2: Территориальное учреждение (ТУ)<br/>
/// 3: Руководство Банка России (РУ)<br/>
/// 4: Банк России (АД)
/// </summary>
public record Level1Item
(
    /// <summary>
    /// Example: "4"
    /// </summary>
    string? Code,

    /// <summary>
    /// Example: "Банк России"
    /// </summary>
    string? Subjects1,

    /// <summary>
    /// Example: "АД"
    /// </summary>
    string? TypeIE,

    /// <summary>
    /// Example: "6fc60350-fa90-450e-9fea-1b0703501d6a"
    /// </summary>
    string? Id
);

#region Mock
//public static class MockLevel1Item
//{
//    /// <summary>
//    /// dictionaries/238d0426-6f57-4c0f-8983-1d1addf8c47a[0]
//    /// </summary>
//    /// <returns></returns>
//    public static string Text() =>
//        """
//        {

//            "Code": "1",
//            "Subjects1": "Центральный аппарат",
//            "TypeIE": "ЦА",
//            "Id": "41941a8b-a18a-406c-b1a1-eb6546a7e033"
//        }
//        """;
//}
#endregion
