﻿#region License
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

namespace Diev.Portal5.API.Info;

/// <summary>
/// 3.1.6.3. Информации о квоте профиля.<br/>
/// GET https://portal5.cbr.ru/back/rapi2/profile/quota<br/>
/// 200 OK
/// </summary>
public record Quota
(
    /// <summary>
    /// Информация о доступной квоте в байтах.
    /// Example: 10737418240 // 10Gb
    /// </summary>
    long TotalQuota,

    /// <summary>
    /// Информация об использованной квоте в байтах.
    /// Example: 7478392970 // 70%
    /// </summary>
    long UsedQuota,

    /// <summary>
    /// Информация о максимальном размере сообщения в байтах.
    /// Example: 2147483648 // 2Gb
    /// </summary>
    long MessageSize
);

public static class MockQuota
{
    /// <summary>
    /// profile/quota
    /// </summary>
    /// <returns></returns>
    public static string Text() =>
        """
        {
            "TotalQuota": 16106127360,
            "UsedQuota": 1506413991,
            "MessageSize": 2147483648
        }
        """;
}
