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

using System.IO.Compression;

using CryptoBot.Helpers;

using Diev.Extensions.LogFile;
using Diev.Extensions.Tools;

namespace CryptoBot.Tasks;

internal class Zadacha221(string uploadPath, string archivePath, string zip, string[] subscribers)
{
    private const string _task = "Zadacha_221";
    private const string _title =
        "Ответ на Запрос информации о платежах КО";

    private static int _n = 0;

    public async Task<int> RunAsync(Guid guid)
    {
        if (string.IsNullOrEmpty(Program.EncryptTo))
            throw new TaskException("В конфиге не указано на кого шифровать.");

        string id = guid.ToString();
        var message = await Program.RestAPI.GetMessageAsync(id)
            ?? throw new TaskException($"Запрос '{id}' не найден.");

        var upload = new DirectoryInfo(uploadPath);

        // PB1_ZBR_3194_NOCRD_000020250618000000516701.xml
        foreach (var xmlFile in upload.GetFiles("PB1_ZBR_*.xml"))
        {
            Logger.TimeZZZLine("Подпись и шифрование");

            // AFN_4030702_0000000_20250623_00001.zip
            string zipFile = Path.Combine(uploadPath, string.Format(zip, DateTime.Now, ++_n));
            FileCopyToZip(xmlFile, zipFile);

            // AFN_4030702_0000000_20250623_00001.zip.sig
            string sigFile = zipFile + ".sig";
            if (!await Program.Crypto.SignDetachedFileAsync(zipFile, sigFile))
                throw new TaskException(
                    $"Подписать файл {zipFile.PathQuoted()} не удалось.");

            // AFN_4030702_0000000_20250623_00001.tpt.zip
            string tptFile = Path.ChangeExtension(zipFile, "tpt.zip");
            File2CopyToZip(zipFile, sigFile, tptFile);

            // AFN_4030702_0000000_20250623_00001.tpt.zip.enc
            string encFile = tptFile + ".enc";
            if (!await Program.Crypto.EncryptFileAsync(tptFile, encFile, Program.EncryptTo))
                throw new TaskException(
                    $"Зашифровать файл {tptFile.PathQuoted()} не удалось.");

            try
            {
                // Backup
                File.Move(tptFile, Path.Combine(archivePath, Path.GetFileName(tptFile)), true);
                File.Copy(encFile, Path.Combine(archivePath, Path.GetFileName(encFile)), true);

                // Clean
                xmlFile.Delete();
                File.Delete(zipFile);
                File.Delete(sigFile);
            }
            catch
            {
                Logger.TimeLine("Очистить папку отправки не удалось");
            }

            Logger.TimeZZZLine("Отправка файла");

            string msgId = await UploadAsync(encFile, guid);

            Thread.Sleep(60000);

            Logger.TimeZZZLine("Запрос статуса принятия");

            message = await Messages.CheckStatusAsync(msgId, 20);

            string report = $"Файл {encFile.PathQuoted()}, статус '{message.Status}'.{Environment.NewLine}{_title}";

            Program.Done(nameof(Zadacha221), report, subscribers);
            File.Delete(encFile);
        }

        // BVD1_ZBR_3194_NOCRD_000020250618000000516701_20250623_0000_000001_000001_000001.xml
        // BVS1_ZBR_3194_NOCRD_000020250618000000516701_20250623_0000_000001.xml
        var files = upload.GetFiles("BV?1_ZBR_*.xml");

        if (files.Length > 0)
        {
            Logger.TimeZZZLine("Подпись и шифрование");

            // AFN_4030702_0000000_20250623_00002.zip
            string zipFile = Path.Combine(uploadPath, string.Format(zip, DateTime.Now, ++_n));
            FilesCopyToZip(files, zipFile);

            // AFN_4030702_0000000_20250623_00002.zip.sig
            string sigFile = zipFile + ".sig";
            if (!await Program.Crypto.SignDetachedFileAsync(zipFile, sigFile))
                throw new TaskException(
                    $"Подписать файл {zipFile.PathQuoted()} не удалось.");

            // AFN_4030702_0000000_20250623_00002.tpt.zip
            string tptFile = Path.ChangeExtension(zip, "tpt.zip");
            File2CopyToZip(zipFile, sigFile, tptFile);

            // AFN_4030702_0000000_20250623_00002.tpt.zip.enc
            string encFile = tptFile + ".enc";
            if (!await Program.Crypto.EncryptFileAsync(tptFile, encFile, Program.EncryptTo))
                throw new TaskException(
                    $"Зашифровать файл {tptFile.PathQuoted()} не удалось.");

            try
            {
                // Backup
                File.Move(tptFile, Path.Combine(archivePath, Path.GetFileName(tptFile)), true);
                File.Copy(encFile, Path.Combine(archivePath, Path.GetFileName(encFile)), true);

                // Clean
                foreach (var file in files)
                    file.Delete();

                File.Delete(zipFile);
                File.Delete(sigFile);
            }
            catch
            {
                Logger.TimeLine("Очистить папку отправки не удалось");
            }

            Logger.TimeZZZLine("Отправка файла");

            string msgId = await UploadAsync(encFile, guid);

            Thread.Sleep(60000);

            Logger.TimeZZZLine("Запрос статуса принятия");

            message = await Messages.CheckStatusAsync(msgId, 20);

            string report = $"Файл {encFile.PathQuoted()}, статус '{message.Status}'.{Environment.NewLine}{_title}";

            Program.Done(_task, report, subscribers);
            File.Delete(encFile);
        }

        if (_n == 0)
            throw new TaskException("Нет файлов для отправки.");

        return 0;
    }

    private static void FileCopyToZip(FileInfo file, string zipFile)
    {
        if (File.Exists(zipFile))
            throw new TaskException(
                "В папке отправки не должно быть прежних архивов.");

        //var mode = File.Exists(zip) ? ZipArchiveMode.Update : ZipArchiveMode.Create;
        var mode = ZipArchiveMode.Create;
        using var archive = ZipFile.Open(zipFile, mode);
        archive.CreateEntryFromFile(file.FullName, file.Name);
    }

    private static void FilesCopyToZip(FileInfo[] files, string zipFile)
    {
        if (File.Exists(zipFile))
            throw new TaskException(
                "В папке отправки не должно быть прежних архивов.");

        //var mode = File.Exists(zip) ? ZipArchiveMode.Update : ZipArchiveMode.Create;
        var mode = ZipArchiveMode.Create;
        using var archive = ZipFile.Open(zipFile, mode);
        foreach (var file in files)
        {
            archive.CreateEntryFromFile(file.FullName, file.Name);
        }
    }

    private static void File2CopyToZip(string file, string sigFile, string zipFile)
    {
        if (File.Exists(zipFile))
            throw new TaskException(
                "В папке отправки не должно быть прежних архивов.");

        //var mode = File.Exists(zip) ? ZipArchiveMode.Update : ZipArchiveMode.Create;
        var mode = ZipArchiveMode.Create;
        using var archive = ZipFile.Open(zipFile, mode);
        archive.CreateEntryFromFile(file, Path.GetFileName(file));
        archive.CreateEntryFromFile(sigFile, Path.GetFileName(sigFile));
    }

    /// <summary>
    /// Отправляет файл на сервер.
    /// </summary>
    /// <param name="path">Путь к файлу для отправки.</param>
    /// <param name="guid">Идентификатор запроса.</param>
    /// <returns>Идентификатор созданного сообщения.</returns>
    /// <exception cref="TaskException"></exception>
    private static async Task<string> UploadAsync(string path, Guid guid)
    {
        var msgId = await Program.RestAPI.UploadEncFileAsync(_task, _title, path, guid);

        if (msgId is not null)
            return msgId;

        throw new TaskException("Отправить файл не удалось.");
    }

    #region Mock
    // GET messages?Task=Zadacha_221
    //public static string MockText() =>
    //    """
    //    [
    //        {
    //            "Id": "bd04e375-8c6f-4e98-93cb-b30500dff33c",
    //            "CorrelationId": "53853734-68cf-42c5-b3c6-b300008ff242",
    //            "GroupId": null,
    //            "Type": "outbox",
    //            "Title": "Ответ на Запрос информации о платежах КО",
    //            "Text": null,
    //            "CreationDate": "2025-06-23T13:35:22Z",
    //            "UpdatedDate": "2025-06-23T13:36:11Z",
    //            "Status": "registered",
    //            "TaskName": "Zadacha_221",
    //            "RegNumber": null,
    //            "TotalSize": 4703,
    //            "Sender": {
    //                "Inn": "7831001422",
    //                "Ogrn": "1027800000095",
    //                "Bik": "044030702",
    //                "RegNum": "3194",
    //                "DivisionCode": "0000"
    //            },
    //            "Files": [
    //                {
    //                    "Id": "61b2dd84-75cf-4697-9995-b30500dff388",
    //                    "Name": "AFN_4030702_0000000_20250623_00001.tpt.zip.enc",
    //                    "Description": null,
    //                    "Encrypted": true,
    //                    "SignedFile": null,
    //                    "Size": 4703,
    //                    "RepositoryInfo": [
    //                        {
    //                            "RepositoryType": "http",
    //                            "Host": "https://portal5.cbr.ru",
    //                            "Port": 81,
    //                            "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/files/61b2dd84-75cf-4697-9995-b30500dff388/download"
    //                        }
    //                    ]
    //                }
    //            ],
    //            "Receipts": [
    //                {
    //                    "Id": "8ee81136-9123-4cdb-91ab-b30500dff53d",
    //                    "ReceiveTime": "2025-06-23T13:35:24Z",
    //                    "StatusTime": "2025-06-23T13:35:24Z",
    //                    "Status": "sent",
    //                    "Message": null,
    //                    "Files": []
    //                },
    //                {
    //                    "Id": "f386404e-e8fa-437e-9e2c-b30500e02c93",
    //                    "ReceiveTime": "2025-06-23T13:36:11Z",
    //                    "StatusTime": "2025-06-23T13:35:59Z",
    //                    "Status": "registered",
    //                    "Message": null,
    //                    "Files": [
    //                        {
    //                            "Id": "fd73b890-938b-45af-a9ec-74e282a9659f",
    //                            "Name": "IESr2Protocol.xml",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": null,
    //                            "Size": 300,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/receipts/f386404e-e8fa-437e-9e2c-b30500e02c93/files/fd73b890-938b-45af-a9ec-74e282a9659f/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "49691527-dc70-46e6-8f74-bfb8bf9c0b6f",
    //                            "Name": "Status.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "860b07c2-54d5-4943-bf89-e40dddcc3081",
    //                            "Size": 7821,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/receipts/f386404e-e8fa-437e-9e2c-b30500e02c93/files/49691527-dc70-46e6-8f74-bfb8bf9c0b6f/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "1d9e4017-829d-49c6-b1d0-a3c4bd4bbf86",
    //                            "Name": "IESr2Protocol.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "fd73b890-938b-45af-a9ec-74e282a9659f",
    //                            "Size": 7820,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/receipts/f386404e-e8fa-437e-9e2c-b30500e02c93/files/1d9e4017-829d-49c6-b1d0-a3c4bd4bbf86/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "860b07c2-54d5-4943-bf89-e40dddcc3081",
    //                            "Name": "Status.xml",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": null,
    //                            "Size": 265,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/receipts/f386404e-e8fa-437e-9e2c-b30500e02c93/files/860b07c2-54d5-4943-bf89-e40dddcc3081/download"
    //                                }
    //                            ]
    //                        }
    //                    ]
    //                },
    //                {
    //                    "Id": "a470949b-1944-4186-b3cb-b30500e00dcd",
    //                    "ReceiveTime": "2025-06-23T13:35:45Z",
    //                    "StatusTime": "2025-06-23T13:35:36Z",
    //                    "Status": "processing",
    //                    "Message": null,
    //                    "Files": [
    //                        {
    //                            "Id": "ab81e1c9-c6e0-4914-bdf5-0ff0dc49402a",
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
    //                                    "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/receipts/a470949b-1944-4186-b3cb-b30500e00dcd/files/ab81e1c9-c6e0-4914-bdf5-0ff0dc49402a/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "2317bc21-6982-4ee2-a350-9f14077a8cf1",
    //                            "Name": "ESODReceipt.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "ab81e1c9-c6e0-4914-bdf5-0ff0dc49402a",
    //                            "Size": 7820,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/receipts/a470949b-1944-4186-b3cb-b30500e00dcd/files/2317bc21-6982-4ee2-a350-9f14077a8cf1/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "a018fb9b-529e-4467-b38d-cbcc7ee0b895",
    //                            "Name": "status.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "1e873ef5-171e-4989-899b-cd22e59fa134",
    //                            "Size": 7820,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/receipts/a470949b-1944-4186-b3cb-b30500e00dcd/files/a018fb9b-529e-4467-b38d-cbcc7ee0b895/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "1e873ef5-171e-4989-899b-cd22e59fa134",
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
    //                                    "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/receipts/a470949b-1944-4186-b3cb-b30500e00dcd/files/1e873ef5-171e-4989-899b-cd22e59fa134/download"
    //                                }
    //                            ]
    //                        }
    //                    ]
    //                },
    //                {
    //                    "Id": "c9a25eaa-19fc-42da-8040-b30500e004a2",
    //                    "ReceiveTime": "2025-06-23T13:35:37Z",
    //                    "StatusTime": "2025-06-23T13:35:33Z",
    //                    "Status": "delivered",
    //                    "Message": null,
    //                    "Files": [
    //                        {
    //                            "Id": "e5f26358-9c0a-4693-a381-66c6507e2423",
    //                            "Name": "status.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "a5169f29-19ec-4ac9-849e-5007e747f72f",
    //                            "Size": 7820,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/receipts/c9a25eaa-19fc-42da-8040-b30500e004a2/files/e5f26358-9c0a-4693-a381-66c6507e2423/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "6ae3fd6e-d26a-427d-b063-bce11214da37",
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
    //                                    "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/receipts/c9a25eaa-19fc-42da-8040-b30500e004a2/files/6ae3fd6e-d26a-427d-b063-bce11214da37/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "3910d0ee-5b94-4199-b003-3fcd51db38c5",
    //                            "Name": "ESODReceipt.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "6ae3fd6e-d26a-427d-b063-bce11214da37",
    //                            "Size": 7821,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/receipts/c9a25eaa-19fc-42da-8040-b30500e004a2/files/3910d0ee-5b94-4199-b003-3fcd51db38c5/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "a5169f29-19ec-4ac9-849e-5007e747f72f",
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
    //                                    "Path": "back/rapi2/messages/bd04e375-8c6f-4e98-93cb-b30500dff33c/receipts/c9a25eaa-19fc-42da-8040-b30500e004a2/files/a5169f29-19ec-4ac9-849e-5007e747f72f/download"
    //                                }
    //                            ]
    //                        }
    //                    ]
    //                }
    //            ]
    //        },
    //        {
    //            "Id": "cd17b437-cc22-47df-8f3e-b30500e04daf",
    //            "CorrelationId": "53853734-68cf-42c5-b3c6-b300008ff242",
    //            "GroupId": null,
    //            "Type": "outbox",
    //            "Title": "Ответ на Запрос информации о платежах КО",
    //            "Text": null,
    //            "CreationDate": "2025-06-23T13:36:39Z",
    //            "UpdatedDate": "2025-06-23T13:43:18Z",
    //            "Status": "registered",
    //            "TaskName": "Zadacha_221",
    //            "RegNumber": null,
    //            "TotalSize": 24795,
    //            "Sender": {
    //                "Inn": "7831001422",
    //                "Ogrn": "1027800000095",
    //                "Bik": "044030702",
    //                "RegNum": "3194",
    //                "DivisionCode": "0000"
    //            },
    //            "Files": [
    //                {
    //                    "Id": "86e1145c-30d6-498a-b448-b30500e04df8",
    //                    "Name": "AFN_4030702_0000000_20250623_00002.tpt.zip.enc",
    //                    "Description": null,
    //                    "Encrypted": true,
    //                    "SignedFile": null,
    //                    "Size": 24795,
    //                    "RepositoryInfo": [
    //                        {
    //                            "RepositoryType": "http",
    //                            "Host": "https://portal5.cbr.ru",
    //                            "Port": 81,
    //                            "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/files/86e1145c-30d6-498a-b448-b30500e04df8/download"
    //                        }
    //                    ]
    //                }
    //            ],
    //            "Receipts": [
    //                {
    //                    "Id": "789b764b-8d98-4def-be0c-b30500e04f78",
    //                    "ReceiveTime": "2025-06-23T13:36:41Z",
    //                    "StatusTime": "2025-06-23T13:36:41Z",
    //                    "Status": "sent",
    //                    "Message": null,
    //                    "Files": []
    //                },
    //                {
    //                    "Id": "0bac3e1a-c2e7-445f-b873-b30500e06ae9",
    //                    "ReceiveTime": "2025-06-23T13:37:04Z",
    //                    "StatusTime": "2025-06-23T13:36:54Z",
    //                    "Status": "processing",
    //                    "Message": null,
    //                    "Files": [
    //                        {
    //                            "Id": "bd26a5fb-73ac-40af-a693-5cb235b3d23d",
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
    //                                    "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/receipts/0bac3e1a-c2e7-445f-b873-b30500e06ae9/files/bd26a5fb-73ac-40af-a693-5cb235b3d23d/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "97fa2655-0141-4f93-b348-7a072fc4e351",
    //                            "Name": "status.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "bd26a5fb-73ac-40af-a693-5cb235b3d23d",
    //                            "Size": 7821,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/receipts/0bac3e1a-c2e7-445f-b873-b30500e06ae9/files/97fa2655-0141-4f93-b348-7a072fc4e351/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "7a177d61-d284-4cce-ba70-c15b8bbd6d5e",
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
    //                                    "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/receipts/0bac3e1a-c2e7-445f-b873-b30500e06ae9/files/7a177d61-d284-4cce-ba70-c15b8bbd6d5e/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "775732e1-b03d-4af6-b835-a7f0c6055332",
    //                            "Name": "ESODReceipt.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "7a177d61-d284-4cce-ba70-c15b8bbd6d5e",
    //                            "Size": 7821,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/receipts/0bac3e1a-c2e7-445f-b873-b30500e06ae9/files/775732e1-b03d-4af6-b835-a7f0c6055332/download"
    //                                }
    //                            ]
    //                        }
    //                    ]
    //                },
    //                {
    //                    "Id": "bc5bf18b-41ab-4ebe-8bbb-b30500e2215e",
    //                    "ReceiveTime": "2025-06-23T13:43:18Z",
    //                    "StatusTime": "2025-06-23T13:43:06Z",
    //                    "Status": "registered",
    //                    "Message": null,
    //                    "Files": [
    //                        {
    //                            "Id": "c9d12191-5670-408a-9fb0-c7c0233560da",
    //                            "Name": "IESr2Protocol.xml",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": null,
    //                            "Size": 13956,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/receipts/bc5bf18b-41ab-4ebe-8bbb-b30500e2215e/files/c9d12191-5670-408a-9fb0-c7c0233560da/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "9ba248df-45db-4f49-922b-72b5349ded16",
    //                            "Name": "Status.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "d433907e-fc6f-4799-8af5-626e03dc2bb8",
    //                            "Size": 7822,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/receipts/bc5bf18b-41ab-4ebe-8bbb-b30500e2215e/files/9ba248df-45db-4f49-922b-72b5349ded16/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "262fb17d-c485-487a-bdaf-acbade1646b2",
    //                            "Name": "IESr2Protocol.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "c9d12191-5670-408a-9fb0-c7c0233560da",
    //                            "Size": 7822,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/receipts/bc5bf18b-41ab-4ebe-8bbb-b30500e2215e/files/262fb17d-c485-487a-bdaf-acbade1646b2/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "d433907e-fc6f-4799-8af5-626e03dc2bb8",
    //                            "Name": "Status.xml",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": null,
    //                            "Size": 265,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/receipts/bc5bf18b-41ab-4ebe-8bbb-b30500e2215e/files/d433907e-fc6f-4799-8af5-626e03dc2bb8/download"
    //                                }
    //                            ]
    //                        }
    //                    ]
    //                },
    //                {
    //                    "Id": "1ced3c32-065e-4343-9ddc-b30500e05f65",
    //                    "ReceiveTime": "2025-06-23T13:36:54Z",
    //                    "StatusTime": "2025-06-23T13:36:50Z",
    //                    "Status": "delivered",
    //                    "Message": null,
    //                    "Files": [
    //                        {
    //                            "Id": "b9084379-d21d-4acc-8fbb-95e48cfc4557",
    //                            "Name": "status.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "d4c4eedb-3d8c-490f-908a-96160a3201e6",
    //                            "Size": 7820,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/receipts/1ced3c32-065e-4343-9ddc-b30500e05f65/files/b9084379-d21d-4acc-8fbb-95e48cfc4557/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "7ffb526e-f3a9-4a8b-a3cb-085eedfebd80",
    //                            "Name": "ESODReceipt.xml.sig",
    //                            "Description": null,
    //                            "Encrypted": false,
    //                            "SignedFile": "53216b6a-7bb0-4dde-8170-03029dbbeed7",
    //                            "Size": 7820,
    //                            "RepositoryInfo": [
    //                                {
    //                                    "RepositoryType": "http",
    //                                    "Host": "https://portal5.cbr.ru",
    //                                    "Port": 81,
    //                                    "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/receipts/1ced3c32-065e-4343-9ddc-b30500e05f65/files/7ffb526e-f3a9-4a8b-a3cb-085eedfebd80/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "d4c4eedb-3d8c-490f-908a-96160a3201e6",
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
    //                                    "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/receipts/1ced3c32-065e-4343-9ddc-b30500e05f65/files/d4c4eedb-3d8c-490f-908a-96160a3201e6/download"
    //                                }
    //                            ]
    //                        },
    //                        {
    //                            "Id": "53216b6a-7bb0-4dde-8170-03029dbbeed7",
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
    //                                    "Path": "back/rapi2/messages/cd17b437-cc22-47df-8f3e-b30500e04daf/receipts/1ced3c32-065e-4343-9ddc-b30500e05f65/files/53216b6a-7bb0-4dde-8170-03029dbbeed7/download"
    //                                }
    //                            ]
    //                        }
    //                    ]
    //                }
    //            ]
    //        }
    //    ]
    //    """;
    #endregion
}
