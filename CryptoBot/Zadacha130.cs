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
using Diev.Portal5;

using Microsoft.Extensions.Configuration;

namespace CryptoBot;

/*
 
22:00 CryptoBot ЗСК. Получение реестра с портала5: /usr/local/zsk_bin/download.bat
        download C:\FORMS\ZSK\IN
            download KYC_20231212.xml.zip.enc
            download KYC_20231212.xml.zip.sig
        decr    C:\FORMS\ZSK\IN\KYC_20231212.xml.zip

var json = await restClient.GetMessagesAsync("Zadacha_130", DateTime.Now.AddDays(-7), DateTime.Now);
var json = await restClient.GetFilesAsync("Zadacha_130", DateTime.Now.AddDays(-7), DateTime.Now, "Download");

*/

internal class Zadacha130(RestClient restClient)
{
    private readonly EnumerationOptions _enumOptions = new();

    public string DownloadPath { get; set; } = "Download";

    public bool Download { get; set; }
    public int Days { get; set; } = 1;
    public bool Decrypt { get; set; }

    public string My { get; set; } = null!;
    public string? PIN { get; set; }

    public bool Overwrite { get; set; }

    public string ArchivePath { get; set; } = "Download/Archive";

    public async Task Run()
    {
        Program.Settings.Bind(nameof(Zadacha130), this);
        Program.Settings.Bind(nameof(CryptoPro), this);

        if (Download)
        {
            if (!Directory.Exists(DownloadPath))
                Directory.CreateDirectory(DownloadPath);

            await DownloadLastFileAsync();
            //await DownloadLastMessageAsync();
        }

        if (Decrypt)
        {
            if (!Directory.Exists(ArchivePath))
                Directory.CreateDirectory(ArchivePath);

            await DecryptAsync();
            await UnzipAsync();
        }
    }

    public async Task DownloadLastFileAsync()
    {
        // url = "back/rapi2/messages/8a3306a7-2025-4726-8d7c-ae3200aacaf0/files/948b6d20-c122-417c-9b92-2c3a14ec8de3/download";
        // path = "KGR_20220204_132116_request_128779.zip";

        var filter = new MessageFilter()
        {
            Task = "Zadacha_130",
            MinDateTime = DateTime.Now.AddDays(-Days)
        };

        var messagePages = await restClient.GetMessagePagesAsync(filter);

        if (messagePages.Messages.Count == 0)
            throw new ApplicationException("Получен пустой список сообщений.");

        string lastName = "KYC_20230000.xml.zip.enc";
        string? messageId = null;
        string? fileId = null;

        foreach (var message in messagePages.Messages)
        {
            foreach (var file in message.Files)
            {
                if (file.Encrypted)
                {
                    if (string.Compare(file.Name, lastName, true) > 0)
                    {
                        lastName = file.Name;

                        messageId = message.Id;
                        fileId = file.Id;
                    }
                }
            }
        }

        if (messageId is null)
            throw new ArgumentNullException(messageId, "Не получено подходящего сообщения.");

        if (fileId is null)
            throw new ArgumentNullException(fileId, "Не получено подходящего файла в сообщении.");

        if (!Directory.Exists(DownloadPath))
            Directory.CreateDirectory(DownloadPath);

        string path = Path.Combine(DownloadPath, lastName);

        await restClient.DownloadMessageFileAsync(messageId, fileId, path, Overwrite);

        //if (!File.Exists(path))
        //    throw new ApplicationException($"Неудача скачивания последнего файла '{lastName}'.");
    }

    public async Task DownloadLastMessageAsync()
    {
        // url = "back/rapi2/messages/8a3306a7-2025-4726-8d7c-ae3200aacaf0/files/948b6d20-c122-417c-9b92-2c3a14ec8de3/download";
        // path = "KGR_20220204_132116_request_128779.zip";

        var filter = new MessageFilter()
        {
            Task = "Zadacha_130",
            MinDateTime = DateTime.Now.AddDays(-Days)
        };

        var messagePages = await restClient.GetMessagePagesAsync(filter);

        if (messagePages.Messages.Count == 0)
            throw new ApplicationException("Получен пустой список сообщений.");

        DateTime? lastDate = DateTime.Now.AddDays(-Days);
        string? messageId = null;

        foreach (var message in messagePages.Messages)
        {
            if (message.CreationDate > lastDate)
            {
                lastDate = message.CreationDate;

                messageId = message.Id;
            }
        }

        if (messageId is null)
            throw new ArgumentNullException(messageId, "Не получено подходящего сообщения.");

        if (!Directory.Exists(DownloadPath))
            Directory.CreateDirectory(DownloadPath);

        string path = Path.Combine(DownloadPath, $"KYC_{lastDate:yyyyMMdd}.zip");

        await restClient.DownloadMessageAsync(messageId, path, true);

        //if (!File.Exists(path))
        //    throw new ApplicationException($"Неудача скачивания последнего сообщения.");
    }

    public async Task DecryptAsync()
    {
        //CryptoProSettings settings = new();
        //Program.Settings.Bind(nameof(CryptoPro), settings);

        foreach (var enc in Directory.EnumerateFiles(DownloadPath, "*.enc", _enumOptions))
        {
            string zip = Path.ChangeExtension(enc, null);
            await CryptoPro.DecryptFileAsync(enc, zip, My, PIN);

            string name = Path.GetFileName(zip);
            await Program.Smtp.SendMessageAsync($"Portal5: Zadacha_130: OK",
                $"Получен файл '{name}'.", [zip]);

            string path = Path.Combine(ArchivePath, Path.GetFileName(enc));
            File.Move(enc, path, true); ;
        }
    }

    public async Task UnzipAsync()
    {
        foreach (var zip in Directory.EnumerateFiles(DownloadPath, "*.zip", _enumOptions))
        {
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(zip, DownloadPath, Overwrite); //TODO skip existing file if no overwrite
                //File.Move(zip, Path.Combine(ArchivePath, Path.GetFileName(zip)), true);
            });
        }
    }
}
