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

using Diev.Portal5.API.Tools;

namespace Diev.Portal5.API.Errors;

/// <summary>
/// В случае ошибок REST методов из класса HTTP 404,
/// в теле ответа передается JSON-объект MoreInfo с дополнительной информацией.
/// </summary>
public record MissedFile404
(
    /// <summary>
    /// Уникальный идентификатор файла.
    /// </summary>
    string Id,

    /// <summary>
    /// Имя файла.
    /// </summary>
    string FileName,

    /// <summary>
    /// Информация о репозиториях (описание репозитория в котором расположен файл.
    /// Опционально.
    /// </summary>
    Repository RepositoriInfo
);
