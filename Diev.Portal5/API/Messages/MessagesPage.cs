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

using Diev.Portal5.API.Tools;

namespace Diev.Portal5.API.Messages;

/// <summary>
/// Мой тип для добавления страниц (из response.Header) к списку сообщений.
/// </summary>
public record MessagesPage
(
    /// <summary>
    /// Example: "[{...}, ...]"
    /// </summary>
    Message[] Messages,

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
