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

namespace CryptoBot.Tasks.Z296;

internal class Zadacha296(
    ILogger<Zadacha296> logger,
    IOptions<Zadacha296Settings> options,
    IPortalService portal,
    INotifyService notifier
    ) : IZadacha296
{
    public async Task<int> RunAsync(uint? days)
    {
        var filter = new MessagesFilter()
        {
            Task = "Zadacha_296",
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

    #region Mock
    // GET https://portal5test.cbr.ru/back/rapi2/messages?Task=Zadacha_296&MinDateTime=2026-03-13T00:00:00Z
    //public static string MockText() =>
    //    """
    //    [
    //      {
    //        "Id": "352ca5cd-4f86-49a3-b758-b40c01478369",
    //        "CorrelationId": null,
    //        "GroupId": null,
    //        "Type": "inbox",
    //        "Title": "Размещение фидов, фидов+",
    //        "Text": "",
    //        "CreationDate": "2026-03-13T19:52:27Z",
    //        "UpdatedDate": null,
    //        "Status": "read",
    //        "TaskName": "Zadacha_296",
    //        "RegNumber": null,
    //        "TotalSize": 8823722,
    //        "Sender": null,
    //        "Files": [
    //          {
    //            "Id": "92950729-2268-46be-a144-e8539972bae4",
    //            "Name": "pis.service.metadata.json.sig",
    //            "Description": null,
    //            "Encrypted": false,
    //            "SignedFile": "b3a9abe9-a5fa-48ee-b672-fccc4ac66893",
    //            "Size": 8151,
    //            "RepositoryInfo": [
    //              {
    //                "RepositoryType": "http",
    //                "Host": "https://portal5test.cbr.ru",
    //                "Port": 443,
    //                "Path": "back/rapi2/messages/352ca5cd-4f86-49a3-b758-b40c01478369/files/92950729-2268-46be-a144-e8539972bae4/download"
    //              }
    //            ]
    //          },
    //          {
    //            "Id": "d2e7671e-5585-415e-9d65-0d870bfd5103",
    //            "Name": "feeds_2026-03-13_22-28-14.zip.sig",
    //            "Description": null,
    //            "Encrypted": false,
    //            "SignedFile": "dce0a484-154a-4dd4-8a88-696ea924d30e",
    //            "Size": 8150,
    //            "RepositoryInfo": [
    //              {
    //                "RepositoryType": "http",
    //                "Host": "https://portal5test.cbr.ru",
    //                "Port": 443,
    //                "Path": "back/rapi2/messages/352ca5cd-4f86-49a3-b758-b40c01478369/files/d2e7671e-5585-415e-9d65-0d870bfd5103/download"
    //              }
    //            ]
    //          },
    //          {
    //            "Id": "dce0a484-154a-4dd4-8a88-696ea924d30e",
    //            "Name": "feeds_2026-03-13_22-28-14.zip.enc",
    //            "Description": null,
    //            "Encrypted": true,
    //            "SignedFile": null,
    //            "Size": 8807396,
    //            "RepositoryInfo": [
    //              {
    //                "RepositoryType": "http",
    //                "Host": "https://portal5test.cbr.ru",
    //                "Port": 443,
    //                "Path": "back/rapi2/messages/352ca5cd-4f86-49a3-b758-b40c01478369/files/dce0a484-154a-4dd4-8a88-696ea924d30e/download"
    //              }
    //            ]
    //          }
    //        ],
    //        "Receipts": []
    //      }
    //    ]
    //    """;
    #endregion
}
