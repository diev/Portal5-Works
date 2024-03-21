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
/// Создание сессии отправки по http.
/// POST https://portal5.cbr.ru/back/rapi2/messages/{MessageId}/files/{FileId}/createUploadSession
/// 200 OK
/// </summary>
public record UploadSession
(
    /// <summary>
    /// Путь для загрузки файла.
    /// Example: "https://portal5.cbr.ru/back/rapi2/messages/{MessageId}/files/{FileId}"
    /// </summary>
    string UploadUrl,

    /// <summary>
    /// Дата и время истечения сессии.
    /// Example: "2023-09-25T16:03:31.81615122Z"
    /// </summary>
    DateTime ExpirationDateTime
);
