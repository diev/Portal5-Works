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

using System.Text;

using CryptoBot.Services;

using Diev.Extensions.Crypto;
using Diev.Extensions.Tools;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Exceptions;
using Diev.Portal5.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CryptoBot.Tasks.Z222;

internal class Zadacha222(
    ILogger<Zadacha222> logger,
    IOptions<Zadacha222Settings> options,
    ICryptoService cryptor,
    IApiService api,
    IPortalService portal,
    INotifyService notifier
    ) : IZadacha222
{
    public async Task<int> RunAsync(uint? days)
    {
        StringBuilder sb = new();

        var filter = new MessagesFilter()
        {
            Task = "Zadacha_222",
            MinDateTime = DateTime.Today.AddDays(-days ?? 0)
        };

        var messagesResult = await portal.GetMessagesAsync(filter);

        if (!messagesResult.OK)
            throw new TaskException("Не получено сообщений");

        var messages = messagesResult.Data!;
        int count = messages.Length;

        if (count == 0)
        {
            throw new NoMessagesException(
                "Получено ноль сообщений");
        }
        else
        {
            string downloadPath = options.Value.DownloadPath;

            if (!Directory.CreateDirectory(downloadPath).Exists)
                throw new TaskException(
                    $"Не удалось создать папку {downloadPath.PathQuoted()}");

            foreach (var message in messages)
            {
                string msgId = message.Id;
                string date = message.CreationDate.ToLocalTime().ToString("ddd dd.MM.yyyy");
                string xml = string.Empty;
                string sig = string.Empty;

                foreach (var msgFile in message.Files)
                {
                    string fileId = msgFile.Id;
                    string path = Path.Combine(downloadPath, msgFile.Name);

                    if (msgFile.Encrypted)
                    {
                        // ZBR_3194_NOCRD_000020250411000000286401.xml.enc
                        var result = await api.DownloadMessageFileAsync(msgId, fileId, path);

                        if (result.OK)
                        {
                            logger.LogTrace("Расшифровка полученного файла");

                            // ZBR_3194_NOCRD_000020250411000000286401.xml
                            xml = await portal.DecryptAsync(path);
                            sb.AppendLine($"{date}  '{msgId}'  {xml.PathQuoted()}");

                            logger.LogInformation("{Date} '{Id}' {File}", date, msgId, xml.PathQuoted());
                        }
                    }
                    else
                    {
                        // ZBR_3194_NOCRD_000020250411000000286401.xml.sig
                        var result = await api.DownloadMessageFileAsync(msgId, fileId, path);

                        if (result.OK)
                            sig = path;
                    }
                }

                if (!await cryptor.VerifyDetachedFileAsync(xml, sig))
                    //throw new TaskException(
                    //    $"Подпись файла {xml.PathQuoted()} не верна");
                    sb.AppendLine(" -- Подпись не верна!");
            }

            return await notifier.DoneAsync(null, sb.ToString(), options.Value.Subscribers);
        }
    }

    #region Mock
    // GET https://portal5.cbr.ru/back/rapi2/messages?Task=Zadacha_222
    //public static string MockText() =>
    //    """
    //    [
    //        {
    //            "Id": "0e4cd18f-76f4-441e-b97e-b2bf00acaf48",
    //            "CorrelationId": null,
    //            "GroupId": null,
    //            "Type": "inbox",
    //            "Title": "Запрос информации о платежах КО",
    //            "Text": "",
    //            "CreationDate": "2025-04-14T10:28:43Z",
    //            "UpdatedDate": null,
    //            "Status": "read",
    //            "TaskName": "Zadacha_222",
    //            "RegNumber": null,
    //            "TotalSize": 9779,
    //            "Sender": null,
    //            "Files": [
    //                {
    //                    "Id": "0860198a-535a-4c3a-bc0e-0aa9ff22bfb8",
    //                    "Name": "ZBR_3194_NOCRD_000020250411000000286401.xml.sig",
    //                    "Description": null,
    //                    "Encrypted": false,
    //                    "SignedFile": "5642df39-55fb-498d-a1d8-d6a65e32dc61",
    //                    "Size": 7820,
    //                    "RepositoryInfo": [
    //                        {
    //                            "RepositoryType": "http",
    //                            "Host": "https://portal5.cbr.ru",
    //                            "Port": 81,
    //                            "Path": "back/rapi2/messages/0e4cd18f-76f4-441e-b97e-b2bf00acaf48/files/0860198a-535a-4c3a-bc0e-0aa9ff22bfb8/download"
    //                        }
    //                    ]
    //                },
    //                {
    //                    "Id": "5642df39-55fb-498d-a1d8-d6a65e32dc61",
    //                    "Name": "ZBR_3194_NOCRD_000020250411000000286401.xml.enc",
    //                    "Description": null,
    //                    "Encrypted": true,
    //                    "SignedFile": null,
    //                    "Size": 1959,
    //                    "RepositoryInfo": [
    //                        {
    //                            "RepositoryType": "http",
    //                            "Host": "https://portal5.cbr.ru",
    //                            "Port": 81,
    //                            "Path": "back/rapi2/messages/0e4cd18f-76f4-441e-b97e-b2bf00acaf48/files/5642df39-55fb-498d-a1d8-d6a65e32dc61/download"
    //                        }
    //                    ]
    //                }
    //            ],
    //            "Receipts": []
    //        },
    //        {
    //            "Id": "53853734-68cf-42c5-b3c6-b300008ff242",
    //            "CorrelationId": null,
    //            "GroupId": null,
    //            "Type": "inbox",
    //            "Title": "Запрос информации о платежах КО",
    //            "Text": "",
    //            "CreationDate": "2025-06-18T08:44:05Z",
    //            "UpdatedDate": null,
    //            "Status": "read",
    //            "TaskName": "Zadacha_222",
    //            "RegNumber": null,
    //            "TotalSize": 11122,
    //            "Sender": null,
    //            "Files": [
    //                {
    //                    "Id": "3f51afb9-1565-472e-98aa-ef18f9e387e8",
    //                    "Name": "ZBR_3194_NOCRD_000020250618000000516701.xml.enc",
    //                    "Description": null,
    //                    "Encrypted": true,
    //                    "SignedFile": null,
    //                    "Size": 3301,
    //                    "RepositoryInfo": [
    //                        {
    //                            "RepositoryType": "http",
    //                            "Host": "https://portal5.cbr.ru",
    //                            "Port": 81,
    //                            "Path": "back/rapi2/messages/53853734-68cf-42c5-b3c6-b300008ff242/files/3f51afb9-1565-472e-98aa-ef18f9e387e8/download"
    //                        }
    //                    ]
    //                },
    //                {
    //                    "Id": "7d044e26-ff69-4539-ae6f-ffa04126c873",
    //                    "Name": "ZBR_3194_NOCRD_000020250618000000516701.xml.sig",
    //                    "Description": null,
    //                    "Encrypted": false,
    //                    "SignedFile": "3f51afb9-1565-472e-98aa-ef18f9e387e8",
    //                    "Size": 7821,
    //                    "RepositoryInfo": [
    //                        {
    //                            "RepositoryType": "http",
    //                            "Host": "https://portal5.cbr.ru",
    //                            "Port": 81,
    //                            "Path": "back/rapi2/messages/53853734-68cf-42c5-b3c6-b300008ff242/files/7d044e26-ff69-4539-ae6f-ffa04126c873/download"
    //                        }
    //                    ]
    //                }
    //            ],
    //            "Receipts": []
    //        }
    //    ]
    //    """;
    #endregion
}
