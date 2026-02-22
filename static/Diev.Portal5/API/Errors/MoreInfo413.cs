#region License
/*
Copyright 2024 Dmitrii Evdokimov
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

namespace Diev.Portal5.API.Errors;

/// <summary>
/// В случае ошибок REST методов из класса HTTP 413,
/// в теле ответа передается JSON-объект MoreInfo с дополнительной информацией.
/// </summary>
public record MoreInfo413
(
    /// <summary>
    /// Общая квота ЛК в Мб.
    /// </summary>
    int AccountQuota,

    /// <summary>
    /// Остаток квоты ЛК в Мб.
    /// </summary>
    int RestOfQuota,

    /// <summary>
    /// Квота на размер сообщения в Мб.
    /// Опционально.
    /// </summary>
    int MessageQuota
);
