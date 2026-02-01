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

using CryptoBot.Services;

using Diev.Extensions.Tools;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CryptoBot.Tasks.Z130;

internal class Zadacha130(
    ILogger<Zadacha130> logger,
    IOptions<Zadacha130Settings> options,
    IPortalService portal,
    INotifyService notifier
    ) : IZadacha130
{
    public async Task<int> RunAsync(uint? days)
    {
        var filter = new MessagesFilter()
        {
            Task = "Zadacha_130",
            MinDateTime = DateTime.Today.AddDays(-days ?? 0)
        };

        logger.LogTrace("Запрос загрузки последнего файла");

        var result = await portal.DownloadLastEncryptedFileAsync(filter, options.Value.DownloadPath);

        if (result.OK)
        {
            string enc = result.Data!;
            string filename = Path.GetFileName(enc).PathQuoted();

            logger.LogInformation("Расшифровка полученного файла {File}", filename);

            string zip = await portal.DecryptAsync(enc);
            filename = Path.GetFileName(zip).PathQuoted();

            logger.LogInformation("Получен файл {File}", filename);
            string report = $"Получен файл {filename}";

            return await notifier.DoneAsync(null, report, options.Value.Subscribers);
        }

        return await notifier.FailAsync(
            null, result.Error!.ErrorMessage, options.Value.Subscribers);
    }
}

#region Mock
// GET https://portal5.cbr.ru/back/rapi2/messages?Task=Zadacha_130&MinDateTime=2023-12-19T00:00:00Z
//public static string MockText() =>
//    """
//    [
//        {
//            "Id": "ca103622-c6f5-478d-8c7b-b0dd01099639",
//            "CorrelationId": null,
//            "GroupId": null,
//            "Type": "inbox",
//            "Title": "Получение информации об уровне риска ЮЛ/ИП",
//            "Text": "",
//            "CreationDate": "2023-12-19T16:07:08Z",
//            "UpdatedDate": null,
//            "Status": "read",
//            "TaskName": "Zadacha_130",
//            "RegNumber": null,
//            "TotalSize": 3333178,
//            "Sender": null,
//            "Files": [
//                {
//                    "Id": "159f726c-32fd-4f68-b45d-482f3addeccc",
//                    "Name": "KYC_20231219.xml.zip.sig",
//                    "Description": null,
//                    "Encrypted": false,
//                    "SignedFile": "21b49104-2640-46ed-b364-c0980c37f9b8",
//                    "Size": 3399,
//                    "RepositoryInfo": [
//                        {
//                            "RepositoryType": "http",
//                            "Host": "https://portal5.cbr.ru",
//                            "Port": 81,
//                            "Path": "back/rapi2/messages/ca103622-c6f5-478d-8c7b-b0dd01099639/files/159f726c-32fd-4f68-b45d-482f3addeccc/download"
//                        }
//                    ]
//                },
//                {
//                    "Id": "21b49104-2640-46ed-b364-c0980c37f9b8",
//                    "Name": "KYC_20231219.xml.zip.enc",
//                    "Description": null,
//                    "Encrypted": true,
//                    "SignedFile": null,
//                    "Size": 3329779,
//                    "RepositoryInfo": [
//                        {
//                            "RepositoryType": "http",
//                            "Host": "https://portal5.cbr.ru",
//                            "Port": 81,
//                            "Path": "back/rapi2/messages/ca103622-c6f5-478d-8c7b-b0dd01099639/files/21b49104-2640-46ed-b364-c0980c37f9b8/download"
//                        }
//                    ]
//                }
//            ],
//            "Receipts": []
//        }
//        {
//            "Id": "ca103622-c6f5-478d-8c7b-b0dd01099639",
//            "CorrelationId": null,
//            "GroupId": null,
//            "Type": "inbox",
//            "Title": "Получение информации об уровне риска ЮЛ/ИП",
//            "Text": "",
//            "CreationDate": "2023-12-19T16:07:08Z",
//            "UpdatedDate": null,
//            "Status": "read",
//            "TaskName": "Zadacha_130",
//            "RegNumber": null,
//            "TotalSize": 3333178,
//            "Sender": null,
//            "Files": [
//                {
//                    "Id": "159f726c-32fd-4f68-b45d-482f3addeccc",
//                    "Name": "KYC_20231219.xml.zip.sig",
//                    "Description": null,
//                    "Encrypted": false,
//                    "SignedFile": "21b49104-2640-46ed-b364-c0980c37f9b8",
//                    "Size": 3399,
//                    "RepositoryInfo": [
//                        {
//                            "RepositoryType": "http",
//                            "Host": "https://portal5.cbr.ru",
//                            "Port": 81,
//                            "Path": "back/rapi2/messages/ca103622-c6f5-478d-8c7b-b0dd01099639/files/159f726c-32fd-4f68-b45d-482f3addeccc/download"
//                        }
//                    ]
//                },
//                {
//                    "Id": "21b49104-2640-46ed-b364-c0980c37f9b8",
//                    "Name": "KYC_20231219.xml.zip.enc",
//                    "Description": null,
//                    "Encrypted": true,
//                    "SignedFile": null,
//                    "Size": 3329779,
//                    "RepositoryInfo": [
//                        {
//                            "RepositoryType": "http",
//                            "Host": "https://portal5.cbr.ru",
//                            "Port": 81,
//                            "Path": "back/rapi2/messages/ca103622-c6f5-478d-8c7b-b0dd01099639/files/21b49104-2640-46ed-b364-c0980c37f9b8/download"
//                        }
//                    ]
//                }
//            ],
//            "Receipts": []
//        }
//    ]
//    """;
#endregion
