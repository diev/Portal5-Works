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

using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;

using Diev.Extensions.Crypto;
using Diev.Extensions.Smtp;
using Diev.Portal5;
using Diev.Portal5.API.Tools;

using Microsoft.Extensions.Configuration;

namespace CryptoBot;

internal class Zadacha130(RestAPI restAPI, IConfiguration config)
{
    private string TaskName => nameof(Zadacha130);
    private readonly EnumerationOptions _enumOptions = new();
    private readonly Smtp _smtp = new();

    public string DownloadPath { get; set; } = "Download";
    public bool DoDownload { get; set; }
    public bool DoDecrypt { get; set; }
    public string[]? SubscribersDone { get; set; }
    public string[]? SubscribersFail { get; set; }

    [RequiresUnreferencedCode(
        "Calls Microsoft.Extensions.Configuration.ConfigurationBinder.Bind(String, Object)")]
    public async Task Run()
    {
        config.Bind(TaskName, this);
        _smtp.Subscribers = SubscribersDone;

        if (DoDownload)
        {
            await DownloadLastFileAsync();
        }

        if (DoDecrypt)
        {
            await DecryptAsync();
            await UnzipAsync();
        }
    }

    public async Task DownloadLastFileAsync()
    {
        // url = "back/rapi2/messages/8a3306a7-2025-4726-8d7c-ae3200aacaf0/files/948b6d20-c122-417c-9b92-2c3a14ec8de3/download";
        // path = "KGR_20220204_132116_request_128779.zip";

        var filter = new MessagesFilter()
        {
            Task = "Zadacha_130",
            MinDateTime = DateTime.Now.AddDays(-14)
        };

        var messagesPage = await restAPI.GetMessagesPageAsync(filter)
            ?? throw new Exception("Ничего не получено с сервера по '{filter.Task}'.");

        int count = messagesPage.Messages.Count;
        if (count == 0)
            throw new Exception($"Не получено ни одного сообщения по '{filter.Task}'.");

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
            throw new ArgumentNullException(fileId, $"Не получено подходящего файла в сообщении '{msgId}'.");

        Directory.CreateDirectory(DownloadPath);
        string path = Path.Combine(DownloadPath, lastName!);
        await restAPI.DownloadMessageFileAsync(msgId, fileId, path);

        //if (!File.Exists(path))
        //    throw new ApplicationException($"Неудача скачивания последнего файла '{lastName}'.");
    }

    public async Task DecryptAsync()
    {
        CryptoPro crypto = new();

        foreach (var enc in Directory.EnumerateFiles(DownloadPath, "*.enc", _enumOptions))
        {
            string zip = Path.ChangeExtension(enc, null);
            await crypto.DecryptFileAsync(enc, zip);

            string name = Path.GetFileName(zip);
            await _smtp.SendMessageAsync($"{TaskName}: OK",
                $"Получен файл '{name}'.", [zip]);

            //string path = Path.Combine(ArchivePath, Path.GetFileName(enc));
            //File.Move(enc, path, true); ;
        }
    }

    public async Task UnzipAsync()
    {
        foreach (var zip in Directory.EnumerateFiles(DownloadPath, "*.zip", _enumOptions))
        {
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(zip, DownloadPath, true); //TODO skip existing file
                //File.Move(zip, Path.Combine(ArchivePath, Path.GetFileName(zip)), true);
            });
        }
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
