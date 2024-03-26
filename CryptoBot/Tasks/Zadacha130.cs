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

using System.IO.Compression;

using Diev.Extensions.Crypto;
using Diev.Extensions.LogFile;
using Diev.Portal5.API.Tools;

namespace CryptoBot.Tasks;

internal static class Zadacha130
{
    private static readonly string _task = "Zadacha_130";

    //config
    private static readonly string DownloadPath;
    private static readonly string? Subscribers;

    static Zadacha130()
    {
        var config = Program.Config.GetSection(_task);

        DownloadPath = config[nameof(DownloadPath)] ?? ".";
        Subscribers = config[nameof(Subscribers)];
    }

    public static async Task RunAsync()
    {
        try
        {
            string enc = await DownloadAsync();
            string zip = await DecryptAsync(enc);
            //await UnzipAsync(zip);

            await Program.SendDoneAsync(_task, @$"Получен файл ""{Path.GetFileName(zip)}"".", Subscribers);
        }
        catch (Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(_task, ex.Message, Subscribers);
            Program.ExitCode = 1;
        }
    }

    private static async Task<string> DownloadAsync()
    {
        // url = "back/rapi2/messages/8a3306a7-2025-4726-8d7c-ae3200aacaf0/files/948b6d20-c122-417c-9b92-2c3a14ec8de3/download";
        // path = "KGR_20220204_132116_request_128779.zip";

        var filter = new MessagesFilter()
        {
            Task = _task,
            MinDateTime = DateTime.Now // required Today only!
        };

        var report = filter.MinDateTime?.ToString("dd.MM.yyyy");

        var messagesPage = await Program.RestAPI.GetMessagesPageAsync(filter)
            ?? throw new Exception(
                $"Не получено сообщений за {report}.");

        int count = messagesPage.Messages.Count;
        if (count == 0)
            throw new Exception(
                $"Ноль сообщений за {report}.");

        string? lastName = null;
        string? fileId = null;

        var message = messagesPage.Messages[count - 1];
        string msgId = message.Id;

        foreach (var file in message.Files)
        {
            if (file.Encrypted)
            {
                lastName = file.Name; // "KYC_yyyyMMdd.xml.zip.enc";
                fileId = file.Id;
                break;
            }
        }

        if (fileId is null)
            throw new Exception(
                $"Не получен Id файла из сообщения '{msgId}' за {report}.");

        Directory.CreateDirectory(DownloadPath);
        string path = Path.Combine(DownloadPath, lastName!);

        if (await Program.RestAPI.DownloadMessageFileAsync(msgId, fileId, path))
            return path;

        throw new Exception(
            @$"Не удалось получить файл ""{lastName}"" за {report}.");
    }

    private static async Task<string> DecryptAsync(string enc)
    {
        CryptoPro crypto = new();
        string zip = Path.ChangeExtension(enc, null);

        if (await crypto.DecryptFileAsync(enc, zip))
        {
            File.Delete(enc);
            return zip;
        }

        throw new Exception(
            @$"Получен файл ""{Path.GetFileName(enc)}"", но расшифровать его не удалось.");
    }

    private static async Task UnzipAsync(string zip)
    {
        await Task.Run(() =>
        {
            ZipFile.ExtractToDirectory(zip, DownloadPath, true); //TODO skip existing file
        });
    }
}

/*
GET https://portal5.cbr.ru/back/rapi2/messages?Task=Zadacha_130&MinDateTime=2023-12-19T00:00:00Z

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
]
*/
