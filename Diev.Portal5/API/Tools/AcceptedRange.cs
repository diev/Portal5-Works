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

namespace Diev.Portal5.API.Tools;

/// <summary>
/// Параметры сессии отправки по HTTP.
/// </summary>
/// <param name="NextExpectedRange">Путь для загрузки файла.</param>
/// <param name="ExpirationDateTime">Дата и время истечения сессии.</param>
public record class AcceptedRange
(
    string[] NextExpectedRange,
    DateTime ExpirationDateTime
);

#region Mock
//public static class MockAcceptedRange
//{
//    public static string Text() =>
//        """
//        {
//          "NextExpectedRange":["4096-8191","8192-8713"],
//          "ExpirationDateTime":"2023-11-29T09:38:35Z"
//        }
//        """;
//}
#endregion
