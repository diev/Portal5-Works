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

using Diev.Extensions.Crypto;

namespace CryptoBot.Tasks;

internal static class Zadacha137
{
    private static readonly string _task = "Zadacha_137";
    private static readonly string _title =
        "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)";
    private static readonly EnumerationOptions _enumOptions = new();

    //config
    private static readonly string UploadPath;
    private static readonly string? EncryptTo;
    private static readonly string? Subscribers;

    static Zadacha137()
    {
        var config = Program.Config.GetSection(_task);

        UploadPath = config[nameof(UploadPath)] ?? ".";
        EncryptTo = config[nameof(EncryptTo)];
        Subscribers = config[nameof(Subscribers)];
    }

    public static async Task RunAsync()
    {
        try
        {
            await SignAndEncryptAsync();
            await UploadAsync();
            
            await Program.SendDoneAsync(_task, _title, Subscribers);
        }
        catch (Exception ex)
        {
            await Program.SendFailAsync(_task, ex.Message, Subscribers);
            Program.ExitCode = 1;
        }
    }

    private static async Task SignAndEncryptAsync()
    {
        CryptoPro crypto = new();
        int count = 0;

        foreach (var zip in Directory.EnumerateFiles(UploadPath, "*.zip", _enumOptions))
        {
            count++;
            string sig = zip + ".sig";
            string enc = zip + ".enc";

            if (!File.Exists(sig))
            {
                await crypto.SignDetachedFileAsync(zip, sig);
            }

            if (!File.Exists(enc))
            {
                await crypto.EncryptFileAsync(zip, enc, EncryptTo);
            }
        }

        if (count > 0)
            return;

        throw new Exception("Нет файла zip для подписи/шифрования.");
    }

    private static async Task UploadAsync()
    {
        if (await Program.RestAPI.UploadDirectoryAsync(_task, _title, UploadPath))
        {
            foreach (var file in Directory.EnumerateFiles(UploadPath, "*.*", _enumOptions))
            {
                File.Delete(file);
            }

            return;
        }

        throw new Exception("Отправить файл не удалось.");
    }
}

/*
GET https://portal5.cbr.ru/back/rapi2/messages?Task=Zadacha_137&MinDateTime=2023-12-19T00:00:00Z

[
    {
        "Id": "4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-12-19T18:10:26Z",
        "UpdatedDate": "2023-12-19T18:11:29Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00810528",
        "TotalSize": 12516,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "8f0abb94-230c-45fe-a794-b0dd012b7f6a",
                "Name": "KYCCL_7831001422_3194_20231219_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 8644,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/files/8f0abb94-230c-45fe-a794-b0dd012b7f6a/download"
                    }
                ]
            },
            {
                "Id": "6d6f8c5c-b7a0-4c0d-aa05-b0dd012b7f6c",
                "Name": "KYCCL_7831001422_3194_20231219_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "8f0abb94-230c-45fe-a794-b0dd012b7f6a",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/files/6d6f8c5c-b7a0-4c0d-aa05-b0dd012b7f6c/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "9b11c093-e87e-4e7c-820a-b0dd012b809d",
                "ReceiveTime": "2023-12-19T18:10:27Z",
                "StatusTime": "2023-12-19T18:10:27Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "684ec863-b79a-4246-906e-b0dd012b8d76",
                "ReceiveTime": "2023-12-19T18:10:38Z",
                "StatusTime": "2023-12-19T18:10:34Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "0bf423b2-a971-46f7-ac40-23d1aa7eb426",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 314,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/684ec863-b79a-4246-906e-b0dd012b8d76/files/0bf423b2-a971-46f7-ac40-23d1aa7eb426/download"
                            }
                        ]
                    },
                    {
                        "Id": "4e3daee4-7cf6-443b-a8e2-73028a64489f",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "74b0a0d5-3255-431f-bad8-8ac885a5eaf3",
                        "Size": 3399,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/684ec863-b79a-4246-906e-b0dd012b8d76/files/4e3daee4-7cf6-443b-a8e2-73028a64489f/download"
                            }
                        ]
                    },
                    {
                        "Id": "74b0a0d5-3255-431f-bad8-8ac885a5eaf3",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1002,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/684ec863-b79a-4246-906e-b0dd012b8d76/files/74b0a0d5-3255-431f-bad8-8ac885a5eaf3/download"
                            }
                        ]
                    },
                    {
                        "Id": "b8d7dc71-790a-4e83-91f1-c3a757dd90ac",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "0bf423b2-a971-46f7-ac40-23d1aa7eb426",
                        "Size": 3399,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/684ec863-b79a-4246-906e-b0dd012b8d76/files/b8d7dc71-790a-4e83-91f1-c3a757dd90ac/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "77b3d6e0-2e82-4267-8a55-b0dd012b91d4",
                "ReceiveTime": "2023-12-19T18:10:41Z",
                "StatusTime": "2023-12-19T18:10:37Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "315c3faf-bbfd-49f9-b6f3-06612d03c3c7",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1003,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/77b3d6e0-2e82-4267-8a55-b0dd012b91d4/files/315c3faf-bbfd-49f9-b6f3-06612d03c3c7/download"
                            }
                        ]
                    },
                    {
                        "Id": "9f78bc59-41cf-4532-904e-265daa82fb57",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "315c3faf-bbfd-49f9-b6f3-06612d03c3c7",
                        "Size": 3399,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/77b3d6e0-2e82-4267-8a55-b0dd012b91d4/files/9f78bc59-41cf-4532-904e-265daa82fb57/download"
                            }
                        ]
                    },
                    {
                        "Id": "eb0fc523-8e3d-496d-83a9-c6b7ec2649a6",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "37dadc78-9ef6-4463-a1a7-d5a48144aec1",
                        "Size": 3399,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/77b3d6e0-2e82-4267-8a55-b0dd012b91d4/files/eb0fc523-8e3d-496d-83a9-c6b7ec2649a6/download"
                            }
                        ]
                    },
                    {
                        "Id": "37dadc78-9ef6-4463-a1a7-d5a48144aec1",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 315,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/77b3d6e0-2e82-4267-8a55-b0dd012b91d4/files/37dadc78-9ef6-4463-a1a7-d5a48144aec1/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "54a8fde1-9f7d-413e-8a68-b0dd012bc8ee",
                "ReceiveTime": "2023-12-19T18:11:29Z",
                "StatusTime": "2023-12-19T18:11:18Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "7983548c-cedb-4df4-8268-8e3d06869b65",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/54a8fde1-9f7d-413e-8a68-b0dd012bc8ee/files/7983548c-cedb-4df4-8268-8e3d06869b65/download"
                            }
                        ]
                    },
                    {
                        "Id": "24111f98-0749-42df-818d-e176f7ca8c6c",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "7983548c-cedb-4df4-8268-8e3d06869b65",
                        "Size": 3399,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/54a8fde1-9f7d-413e-8a68-b0dd012bc8ee/files/24111f98-0749-42df-818d-e176f7ca8c6c/download"
                            }
                        ]
                    }
                ]
            }
        ]
    }
]
*/
