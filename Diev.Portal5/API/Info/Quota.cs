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

/// <summary>
/// Информации о квоте профиля.
/// GET https://portal5.cbr.ru/back/rapi2/profile/quota
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

/*
{
    "TotalQuota": 10737418240,
    "UsedQuota": 7518173722,
    "MessageSize": 2147483648
}
*/
