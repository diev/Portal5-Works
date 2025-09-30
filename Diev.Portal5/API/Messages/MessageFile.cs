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

using Diev.Portal5.API.Tools;

namespace Diev.Portal5.API.Messages;

/// <summary>
/// Файл включенный в сообщение.
/// </summary>
public record MessageFile
(
    /// <summary>
    /// Уникальный идентификатор файла.
    /// Устанавливается сервером в ответном сообщении.
    /// Example: "6e16a6ad-018f-4136-a8c6-b088010899bc"
    /// </summary>
    string Id,

    /// <summary>
    /// Имя файла.
    /// Example: "passport.xml"
    /// Example: "ЭД_Письмо.pdf"
    /// Example (E): "KYC_20230925.xml.zip.enc"
    /// Example (S): "KYC_20230925.xml.zip.sig"
    /// </summary>
    string Name,

    /// <summary>
    /// Описание файла (необязательное поле, для запросов и предписаний из Банка России
    /// содержит имя файла с расширением, однако может содержать запрещённые символы Windows).
    /// Example: null
    /// Example: "Паспорт РК"
    /// Example: "612613485.pdf"
    /// </summary>
    string? Description,

    /// <summary>
    /// Признак зашифрованности файла (ДСП).
    /// Example (E): true
    /// Example (S): false
    /// </summary>
    bool Encrypted,

    /// <summary>
    /// Идентификатор файла, подписью для которого является данный файл (заполняется только для файлов подписи *.sig).
    /// Example (E): null
    /// Example (S): "KYC_20230925.xml.zip.enc"
    /// </summary>
    string? SignedFile,

    /// <summary>
    /// Example: "http"
    /// </summary>
    string RepositoryType,

    /// <summary>
    /// Общий размер файла в байтах (uint64).
    /// Example (E): 3238155
    /// Example (S): 3399
    /// </summary>
    long Size,

    /// <summary>
    /// Информация о репозиториях (описание репозитория в котором расположен файл.
    /// Данная информация используется как для загрузки файла, так и при его выгрузке).
    /// Example: [{
    /// "RepositoryType": "http",
    /// "Host": "https://portal5.cbr.ru",
    /// "Port": 81,
    /// "Path": "back/rapi2/messages/1d018a30-de5d-4f20-9eb9-b0890102f4be/files/14c80cb0-135d-42e2-b5a8-f1b04108d4ba/download"
    /// }]
    /// </summary>
    IReadOnlyList<Repository>? RepositoryInfo
);

#region Mock
//public static class MockMessageFile
//{
//    public static string Text(bool encrypted = false) => encrypted ?
//        """
//        {
//            "Id": "3d9f1174-ad1d-485e-8149-109ae7353688",
//            "Name": "KYC_20231031.xml.zip.enc",
//            "Description": null,
//            "Encrypted": true,
//            "SignedFile": null,
//            "Size": 3324863,
//            "RepositoryInfo": [
//                {
//                    "RepositoryType": "http",
//                    "Host": "https://portal5.cbr.ru",
//                    "Port": 81,
//                    "Path": "back/rapi2/messages/6fbc3cf9-b48c-4a15-ba8c-b0ad002c489c/files/3d9f1174-ad1d-485e-8149-109ae7353688/download"
//                }
//            ]
//        }
//        """
//        :
//        """
//        {
//            "Id": "d2a087db-55b8-4348-9ec7-5313c935ec41",
//            "Name": "KYC_20231031.xml.zip.sig",
//            "Description": null,
//            "Encrypted": false,
//            "SignedFile": "3d9f1174-ad1d-485e-8149-109ae7353688",
//            "Size": 3399,
//            "RepositoryInfo": [
//                {
//                    "RepositoryType": "http",
//                    "Host": "https://portal5.cbr.ru",
//                    "Port": 81,
//                    "Path": "back/rapi2/messages/6fbc3cf9-b48c-4a15-ba8c-b0ad002c489c/files/d2a087db-55b8-4348-9ec7-5313c935ec41/download"
//                }
//            ]
//        }
//        """;
//}
#endregion
