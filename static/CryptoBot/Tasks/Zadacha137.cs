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
using Diev.Extensions.Xml;

namespace CryptoBot.Tasks;

internal class Zadacha137(string uploadPath, string zip, string? xsd, string[] subscribers)
{
    private const string _task = "Zadacha_137";
    private const string _title =
        "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)";

    public async Task<int> RunAsync(uint? days)
    {
        if (string.IsNullOrEmpty(xsd))
            throw new TaskException("В конфиге не указан файл схемы XSD.");

        if (!File.Exists(xsd))
            throw new TaskException($"Не найден файл схемы {xsd.PathQuoted}.");

        if (string.IsNullOrEmpty(Program.EncryptTo))
            throw new TaskException("В конфиге не указано на кого шифровать.");

        string zipFile = GetZipToUpload(uploadPath, zip, days ?? 0);
        string zipFullName = Path.Combine(uploadPath, zipFile);
        string temp = Paths.GetTempPath(uploadPath);

        await Files.UnzipToDirectoryAsync(zipFullName, temp);

        Logger.TimeZZZLine("Проверка XML по схеме XSD");

        foreach (var xml in Directory.EnumerateFiles(temp, "*.xml"))
        {
            await XsdChecker.CheckAsync(xml, xsd);
            File.Delete(xml);
        }

        Logger.TimeZZZLine("Подпись и шифрование");

        await Files.SignAndEncryptToDirectoryAsync(zipFullName, Program.EncryptTo, temp);

        Logger.TimeZZZLine("Отправка файла");

        string msgId = await UploadAsync(temp);

        Thread.Sleep(60000);

        Logger.TimeZZZLine("Запрос статуса принятия");

        var message = await Messages.CheckStatusAsync(msgId, 20);

        string report = $"Файл {zip.PathQuoted()}, статус '{message.Status}'.{Environment.NewLine}{_title}";

        return Program.Done(nameof(Zadacha137), report, subscribers);
    }

    /// <summary>
    /// Получить имя файла ZIP для отправки.
    /// </summary>
    /// <param name="path">Путь исходных файлов UploadPath.</param>
    /// <param name="format">Маска имени файла zip с подстановочными символами для даты.</param>
    /// <param name="days">Сколько дней назад перебрать в поисках последнего файла
    /// или 0 - только с текущей датой.</param>
    /// <returns>Имя найденного последнего файла zip без пути.</returns>
    /// <exception cref="TaskException"></exception>
    private static string GetZipToUpload(string path, string format, uint days = 0)
    {
        if (days == 0)
        {
            string zip = string.Format(format, DateTime.Now);

            if (File.Exists(Path.Combine(path, zip)))
            {
                return zip;
            }

            throw new TaskException(
                "Сегодня нет файла для отправки.");
        }

        for (int i = 0; i >= -days; i--)
        {
            string zip = string.Format(format, DateTime.Now.AddDays(i));

            if (File.Exists(Path.Combine(path, zip)))
            {
                return zip;
            }
        }

        throw new TaskException(
            $"За {days} последних дней ни одного файла для отправки.");
    }

    /// <summary>
    /// Отправляет папку файлов на сервер.
    /// Obsoleted: Прикладывает МЧД (xml + sig), если указан массив файлов xml.
    /// </summary>
    /// <param name="path">Путь к папке для отправки.</param>
    /// <returns>Идентификатор созданного сообщения.</returns>
    /// <exception cref="TaskException"></exception>
    private static async Task<string> UploadAsync(string path)
    {
        //const string sig = ".sig";

        //if (Program.DoverXml.Length > 0)
        //{
        //    foreach (var xml in Program.DoverXml)
        //    {
        //        string name = Path.GetFileName(xml);
        //        File.Copy(xml, Path.Combine(path, name), true);
        //        File.Copy(xml + sig, Path.Combine(path, name + sig), true);
        //    }
        //}

        var msgIdResult = await Program.RestAPI.UploadDirectoryAsync(_task, _title, path);

        if (msgIdResult.OK)
            return msgIdResult.Data!;

        throw new TaskException(
            "Отправить файл не удалось.");
    }

    #region Mock
    // GET messages?Task=Zadacha_137&MinDateTime=2023-12-19T00:00:00Z
    //public static string MockText(bool valid = true) => valid ?
    //    """
    //    [
    //        {
    //            "Id": "4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e",
    //            "CorrelationId": null,
    //            "GroupId": null,
    //            "Type": "outbox",
    //            "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
    //            "Text": null,
    //            "CreationDate": "2023-12-19T18:10:26Z",
    //            "UpdatedDate": "2023-12-19T18:11:29Z",
    //            "Status": "registered",
    //            "TaskName": "Zadacha_137",
    //            "RegNumber": "17_001_001_00810528",
    //            "TotalSize": 12516,
    //            "Sender": {
    //                "Inn": "7831001422",
    //                "Ogrn": "1027800000095",
    //                "Bik": "044030702",
    //                "RegNum": "3194",
    //                "DivisionCode": "0000"
    //            },
    //            "Files": [
    //                {
    //                    "Id": "8f0abb94-230c-45fe-a794-b0dd012b7f6a",
    //                    "Name": "KYCCL_7831001422_3194_20231219_000001.zip.enc",
    //                    "Description": null,
    //                    "Encrypted": true,
    //                    "SignedFile": null,
    //                    "Size": 8644,
    //                    "RepositoryInfo": [
    //                        {
    //                            "RepositoryType": "http",
    //                            "Host": "https://portal5.cbr.ru",
    //                            "Port": 81,
    //                            "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/files/8f0abb94-230c-45fe-a794-b0dd012b7f6a/download"
    //                        }
    //                    ]
    //                },
    //                {
    //                    "Id": "6d6f8c5c-b7a0-4c0d-aa05-b0dd012b7f6c",
    //                    "Name": "KYCCL_7831001422_3194_20231219_000001.zip.sig",
    //                    "Description": null,
    //                    "Encrypted": false,
    //                    "SignedFile": "8f0abb94-230c-45fe-a794-b0dd012b7f6a",
    //                    "Size": 3872,
    //                    "RepositoryInfo": [
    //                        {
    //                            "RepositoryType": "http",
    //                            "Host": "https://portal5.cbr.ru",
    //                            "Port": 81,
    //                            "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/files/6d6f8c5c-b7a0-4c0d-aa05-b0dd012b7f6c/download"
    //                        }
    //                    ]
    //                }
    //            ],
    //            "Receipts": [
    //                {
    //                    "Id": "9b11c093-e87e-4e7c-820a-b0dd012b809d",
    //                    "ReceiveTime": "2023-12-19T18:10:27Z",
    //                    "StatusTime": "2023-12-19T18:10:27Z",
    //                    "Status": "sent",
    //                    "Message": null,
    //                    "Files": []
    //                },
    //                {
    //                    "Id": "684ec863-b79a-4246-906e-b0dd012b8d76",
    //                    "ReceiveTime": "2023-12-19T18:10:38Z",
    //                    "StatusTime": "2023-12-19T18:10:34Z",
    //                    "Status": "delivered",
    //                    "Message": null,
    //                    "Files": [
    //                        {
    //                            "Id": "0bf423b2-a971-46f7-ac40-23d1aa7eb426",
    //                            "Name": "status.xml",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": null,
    //                            "Size": 314,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/684ec863-b79a-4246-906e-b0dd012b8d76/files/0bf423b2-a971-46f7-ac40-23d1aa7eb426/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "4e3daee4-7cf6-443b-a8e2-73028a64489f",
    //                            "Name": "ESODReceipt.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "74b0a0d5-3255-431f-bad8-8ac885a5eaf3",
    //                            "Size": 3399,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/684ec863-b79a-4246-906e-b0dd012b8d76/files/4e3daee4-7cf6-443b-a8e2-73028a64489f/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "74b0a0d5-3255-431f-bad8-8ac885a5eaf3",
    //                            "Name": "ESODReceipt.xml",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": null,
    //                            "Size": 1002,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/684ec863-b79a-4246-906e-b0dd012b8d76/files/74b0a0d5-3255-431f-bad8-8ac885a5eaf3/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "b8d7dc71-790a-4e83-91f1-c3a757dd90ac",
    //                            "Name": "status.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "0bf423b2-a971-46f7-ac40-23d1aa7eb426",
    //                            "Size": 3399,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/684ec863-b79a-4246-906e-b0dd012b8d76/files/b8d7dc71-790a-4e83-91f1-c3a757dd90ac/download"
    //                                }
    //                            ]
    //                        }
    //                    ]
    //                },
    //                {
    //                    "Id": "77b3d6e0-2e82-4267-8a55-b0dd012b91d4",
    //                    "ReceiveTime": "2023-12-19T18:10:41Z",
    //                    "StatusTime": "2023-12-19T18:10:37Z",
    //                    "Status": "processing",
    //                    "Message": null,
    //                    "Files": [
    //                        {
    //                            "Id": "315c3faf-bbfd-49f9-b6f3-06612d03c3c7",
    //                            "Name": "ESODReceipt.xml",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": null,
    //                            "Size": 1003,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/77b3d6e0-2e82-4267-8a55-b0dd012b91d4/files/315c3faf-bbfd-49f9-b6f3-06612d03c3c7/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "9f78bc59-41cf-4532-904e-265daa82fb57",
    //                            "Name": "ESODReceipt.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "315c3faf-bbfd-49f9-b6f3-06612d03c3c7",
    //                            "Size": 3399,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/77b3d6e0-2e82-4267-8a55-b0dd012b91d4/files/9f78bc59-41cf-4532-904e-265daa82fb57/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "eb0fc523-8e3d-496d-83a9-c6b7ec2649a6",
    //                            "Name": "status.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "37dadc78-9ef6-4463-a1a7-d5a48144aec1",
    //                            "Size": 3399,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/77b3d6e0-2e82-4267-8a55-b0dd012b91d4/files/eb0fc523-8e3d-496d-83a9-c6b7ec2649a6/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "37dadc78-9ef6-4463-a1a7-d5a48144aec1",
    //                            "Name": "status.xml",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": null,
    //                            "Size": 315,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/77b3d6e0-2e82-4267-8a55-b0dd012b91d4/files/37dadc78-9ef6-4463-a1a7-d5a48144aec1/download"
    //                                }
    //                            ]
    //                        }
    //                    ]
    //                },
    //                {
    //                    "Id": "54a8fde1-9f7d-413e-8a68-b0dd012bc8ee",
    //                    "ReceiveTime": "2023-12-19T18:11:29Z",
    //                    "StatusTime": "2023-12-19T18:11:18Z",
    //                    "Status": "registered",
    //                    "Message": null,
    //                    "Files": [
    //                        {
    //                            "Id": "7983548c-cedb-4df4-8268-8e3d06869b65",
    //                            "Name": "status.xml",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": null,
    //                            "Size": 378,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/54a8fde1-9f7d-413e-8a68-b0dd012bc8ee/files/7983548c-cedb-4df4-8268-8e3d06869b65/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "24111f98-0749-42df-818d-e176f7ca8c6c",
    //                            "Name": "status.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "7983548c-cedb-4df4-8268-8e3d06869b65",
    //                            "Size": 3399,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/54a8fde1-9f7d-413e-8a68-b0dd012bc8ee/files/24111f98-0749-42df-818d-e176f7ca8c6c/download"
    //                                }
    //                            ]
    //                        }
    //                    ]
    //                }
    //            ]
    //        }
    //    ]
    //    """
    //    : // error
    //    """
    //    {
    //        "Id": "ce8c33f6-aa59-443f-8165-b13e0128a28a",
    //        "CorrelationId": null,
    //        "GroupId": null,
    //        "Type": "outbox",
    //        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
    //        "Text": null,
    //        "CreationDate": "2024-03-25T18:00:00Z",
    //        "UpdatedDate": "2024-03-25T18:00:20Z",
    //        "Status": "error",
    //        "TaskName": "Zadacha_137",
    //        "RegNumber": null,
    //        "TotalSize": 25096,
    //        "Sender": {
    //            "Inn": "7831001422",
    //            "Ogrn": "1027800000095",
    //            "Bik": "044030702",
    //            "RegNum": "3194",
    //            "DivisionCode": "0000"
    //        },
    //        "Files": [
    //            {
    //                "Id": "0898d882-017e-41c5-8652-b13e0128a27a",
    //                "Name": "KYCCL_7831001422_3194_20240325_000001.zip.sig",
    //                "Description": null,
    //                "Encrypted": false,
    //                "SignedFile": "9b7386af-6ede-4426-bb26-b13e0128a268",
    //                "Size": 4070,
    //                "RepositoryInfo": [
    //                    {
    //                        "RepositoryType": "http",
    //                        "Host": "https://portal5.cbr.ru",
    //                        "Port": 81,
    //                        "Path": "back/rapi2/messages/ce8c33f6-aa59-443f-8165-b13e0128a28a/files/0898d882-017e-41c5-8652-b13e0128a27a/download"
    //                    }
    //                ]
    //            },
    //            {
    //                "Id": "4e6c3df5-3d6f-407f-a9f9-b13e0128a27a",
    //                "Name": "KYCCL_7831001422_3194_20240324_000001.zip.sig",
    //                "Description": null,
    //                "Encrypted": false,
    //                "SignedFile": "5345baa4-4e05-4ec7-9ccd-b13e0128a268",
    //                "Size": 4070,
    //                "RepositoryInfo": [
    //                    {
    //                        "RepositoryType": "http",
    //                        "Host": "https://portal5.cbr.ru",
    //                        "Port": 81,
    //                        "Path": "back/rapi2/messages/ce8c33f6-aa59-443f-8165-b13e0128a28a/files/4e6c3df5-3d6f-407f-a9f9-b13e0128a27a/download"
    //                    }
    //                ]
    //            },
    //            {
    //                "Id": "5345baa4-4e05-4ec7-9ccd-b13e0128a268",
    //                "Name": "KYCCL_7831001422_3194_20240324_000001.zip.enc",
    //                "Description": null,
    //                "Encrypted": true,
    //                "SignedFile": null,
    //                "Size": 8476,
    //                "RepositoryInfo": [
    //                    {
    //                        "RepositoryType": "http",
    //                        "Host": "https://portal5.cbr.ru",
    //                        "Port": 81,
    //                        "Path": "back/rapi2/messages/ce8c33f6-aa59-443f-8165-b13e0128a28a/files/5345baa4-4e05-4ec7-9ccd-b13e0128a268/download"
    //                    }
    //                ]
    //            },
    //            {
    //                "Id": "9b7386af-6ede-4426-bb26-b13e0128a268",
    //                "Name": "KYCCL_7831001422_3194_20240325_000001.zip.enc",
    //                "Description": null,
    //                "Encrypted": true,
    //                "SignedFile": null,
    //                "Size": 8480,
    //                "RepositoryInfo": [
    //                    {
    //                        "RepositoryType": "http",
    //                        "Host": "https://portal5.cbr.ru",
    //                        "Port": 81,
    //                        "Path": "back/rapi2/messages/ce8c33f6-aa59-443f-8165-b13e0128a28a/files/9b7386af-6ede-4426-bb26-b13e0128a268/download"
    //                    }
    //                ]
    //            }
    //        ],
    //        "Receipts": [
    //            {
    //                "Id": "9e5e90ee-850d-45de-a260-b13e0128b3bf",
    //                "ReceiveTime": "2024-03-25T18:00:15Z",
    //                "StatusTime": "2024-03-25T18:00:15Z",
    //                "Status": "sent",
    //                "Message": null,
    //                "Files": []
    //            },
    //            {
    //                "Id": "36827fd6-0cbb-47b9-a17f-b13e0128b8f5",
    //                "ReceiveTime": "2024-03-25T18:00:20Z",
    //                "StatusTime": "2024-03-25T18:00:20Z",
    //                "Status": "error",
    //                "Message": "Ожидается не более 1 файлов с типом Document, найдено: 2",
    //                "Files": [
    //                    {
    //                        "Id": "25099dfc-9204-4c08-870e-ad15b2a8a67d",
    //                        "Name": "ESODReceipt.xml",
    //                        "Description": null,
    //                        "Encrypted": false,
    //                        "SignedFile": null,
    //                        "Size": 1093,
    //                        "RepositoryInfo": [
    //                            {
    //                                "RepositoryType": "http",
    //                                "Host": "https://portal5.cbr.ru",
    //                                "Port": 81,
    //                                "Path": "back/rapi2/messages/ce8c33f6-aa59-443f-8165-b13e0128a28a/receipts/36827fd6-0cbb-47b9-a17f-b13e0128b8f5/files/25099dfc-9204-4c08-870e-ad15b2a8a67d/download"
    //                            }
    //                        ]
    //                    },
    //                    {
    //                        "Id": "0322e280-2861-4370-82b1-5ee00dd83752",
    //                        "Name": "status.xml.sig",
    //                        "Description": null,
    //                        "Encrypted": false,
    //                        "SignedFile": "b4cead28-c051-4bc6-801a-feffd389dc94",
    //                        "Size": 3399,
    //                        "RepositoryInfo": [
    //                            {
    //                                "RepositoryType": "http",
    //                                "Host": "https://portal5.cbr.ru",
    //                                "Port": 81,
    //                                "Path": "back/rapi2/messages/ce8c33f6-aa59-443f-8165-b13e0128a28a/receipts/36827fd6-0cbb-47b9-a17f-b13e0128b8f5/files/0322e280-2861-4370-82b1-5ee00dd83752/download"
    //                            }
    //                        ]
    //                    },
    //                    {
    //                        "Id": "f5304c07-1d06-434d-aa85-4158ebe405b3",
    //                        "Name": "ESODReceipt.xml.sig",
    //                        "Description": null,
    //                        "Encrypted": false,
    //                        "SignedFile": "25099dfc-9204-4c08-870e-ad15b2a8a67d",
    //                        "Size": 3399,
    //                        "RepositoryInfo": [
    //                            {
    //                                "RepositoryType": "http",
    //                                "Host": "https://portal5.cbr.ru",
    //                                "Port": 81,
    //                                "Path": "back/rapi2/messages/ce8c33f6-aa59-443f-8165-b13e0128a28a/receipts/36827fd6-0cbb-47b9-a17f-b13e0128b8f5/files/f5304c07-1d06-434d-aa85-4158ebe405b3/download"
    //                            }
    //                        ]
    //                    },
    //                    {
    //                        "Id": "b4cead28-c051-4bc6-801a-feffd389dc94",
    //                        "Name": "status.xml",
    //                        "Description": null,
    //                        "Encrypted": false,
    //                        "SignedFile": null,
    //                        "Size": 439,
    //                        "RepositoryInfo": [
    //                            {
    //                                "RepositoryType": "http",
    //                                "Host": "https://portal5.cbr.ru",
    //                                "Port": 81,
    //                                "Path": "back/rapi2/messages/ce8c33f6-aa59-443f-8165-b13e0128a28a/receipts/36827fd6-0cbb-47b9-a17f-b13e0128b8f5/files/b4cead28-c051-4bc6-801a-feffd389dc94/download"
    //                            }
    //                        ]
    //                    }
    //                ]
    //            }
    //        ]
    //    }
    //    """;
    #endregion
}
