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

using CryptoBot.Helpers;

using Diev.Extensions.LogFile;
using Diev.Extensions.Tools;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Exceptions;

namespace CryptoBot.Tasks;

internal static class Zadacha130
{
    private const string _task = "Zadacha_130";

    //config
    private static string DownloadPath { get; }
    private static string[] Subscribers { get; }

    static Zadacha130()
    {
        var config = Program.Config.GetSection(_task);

        DownloadPath = config[nameof(DownloadPath)] ?? ".";
        Subscribers = JsonSection.Subscribers(config);
    }

    public static async Task RunAsync(uint? days)
    {
        try
        {
            var filter = new MessagesFilter()
            {
                Task = _task,
                MinDateTime = DateTime.Today.AddDays(-days ?? 0)
            };

            Logger.TimeZZZLine("Запрос загрузки последнего файла");

            string enc = await Files.DownloadLastEncryptedAsync(filter, DownloadPath);

            Logger.TimeZZZLine("Расшифровка полученного файла");

            string zip = await Files.DecryptAsync(enc);
            //await Files.UnzipToDirectoryAsync(zip, DownloadPath);

            string report = $"Получен файл {Path.GetFileName(zip).PathQuoted()}.";

            Program.Done(_task, report, Subscribers);
        }
        catch (NoMessagesException ex)
        {
            Program.Done(_task, ex.Message, Subscribers);
        }
        catch (Portal5Exception ex)
        {
            Program.FailAPI(_task, ex, Subscribers);
        }
        catch (TaskException ex)
        {
            Program.FailTask(_task, ex, Subscribers);
        }
        catch (Exception ex)
        {
            Program.Fail(_task, ex, Subscribers);
        }
    }

    // GET https://portal5.cbr.ru/back/rapi2/messages?Task=Zadacha_130&MinDateTime=2023-12-19T00:00:00Z
    public static string MockText() =>
        """
        [
            {
                "Id": "ca103622-c6f5-478d-8c7b-b0dd01099639",
                "CorrelationId": null,
                "GroupId": null,
                "Type": "inbox",
                "Title": "Получение информации об уровне риска ЮЛ/ИП",
                "Text": "",
                "CreationDate": "2023-12-19T16:07:08Z",
                "UpdatedDate": null,
                "Status": "read",
                "TaskName": "Zadacha_130",
                "RegNumber": null,
                "TotalSize": 3333178,
                "Sender": null,
                "Files": [
                    {
                        "Id": "159f726c-32fd-4f68-b45d-482f3addeccc",
                        "Name": "KYC_20231219.xml.zip.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "21b49104-2640-46ed-b364-c0980c37f9b8",
                        "Size": 3399,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/ca103622-c6f5-478d-8c7b-b0dd01099639/files/159f726c-32fd-4f68-b45d-482f3addeccc/download"
                            }
                        ]
                    },
                    {
                        "Id": "21b49104-2640-46ed-b364-c0980c37f9b8",
                        "Name": "KYC_20231219.xml.zip.enc",
                        "Description": null,
                        "Encrypted": true,
                        "SignedFile": null,
                        "Size": 3329779,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/ca103622-c6f5-478d-8c7b-b0dd01099639/files/21b49104-2640-46ed-b364-c0980c37f9b8/download"
                            }
                        ]
                    }
                ],
                "Receipts": []
            }
            {
                "Id": "ca103622-c6f5-478d-8c7b-b0dd01099639",
                "CorrelationId": null,
                "GroupId": null,
                "Type": "inbox",
                "Title": "Получение информации об уровне риска ЮЛ/ИП",
                "Text": "",
                "CreationDate": "2023-12-19T16:07:08Z",
                "UpdatedDate": null,
                "Status": "read",
                "TaskName": "Zadacha_130",
                "RegNumber": null,
                "TotalSize": 3333178,
                "Sender": null,
                "Files": [
                    {
                        "Id": "159f726c-32fd-4f68-b45d-482f3addeccc",
                        "Name": "KYC_20231219.xml.zip.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "21b49104-2640-46ed-b364-c0980c37f9b8",
                        "Size": 3399,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/ca103622-c6f5-478d-8c7b-b0dd01099639/files/159f726c-32fd-4f68-b45d-482f3addeccc/download"
                            }
                        ]
                    },
                    {
                        "Id": "21b49104-2640-46ed-b364-c0980c37f9b8",
                        "Name": "KYC_20231219.xml.zip.enc",
                        "Description": null,
                        "Encrypted": true,
                        "SignedFile": null,
                        "Size": 3329779,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/ca103622-c6f5-478d-8c7b-b0dd01099639/files/21b49104-2640-46ed-b364-c0980c37f9b8/download"
                            }
                        ]
                    }
                ],
                "Receipts": []
            }
        ]
        """;
}
