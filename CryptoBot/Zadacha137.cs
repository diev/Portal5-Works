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
 
21:00 CryptoBot ЗСК. Отправка перечня клиентов в портал5: /usr/local/zsk_bin/upload.bat
        source  C:\FORMS\ZSK\OUT\20231212\KYCCL_7831001422_3194_20231212_000001.zip
        copy    C:\FORMS\ZSK\ARCHIVE\20231212\KYCCL_7831001422_3194_20231212_000001.zip
        sign    C:\FORMS\ZSK\OUT\KYCCL_7831001422_3194_20231212_000001.zip.sig
        enc     C:\FORMS\ZSK\OUT\KYCCL_7831001422_3194_20231212_000001.zip.enc
        upload  C:\FORMS\ZSK\OUT
            upload  KYCCL_7831001422_3194_20231212_000001.zip.enc
            upload  KYCCL_7831001422_3194_20231212_000001.zip.sig
        check registration

var json = await restClient.UploadDirectoryAsync("Zadacha_137", "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)","Upload");

*/

internal class Zadacha137(RestClient restClient)
{
    private readonly EnumerationOptions _enumOptions= new();

    public bool Sign { get; set; }
    public bool Encrypt { get; set; }
    public bool Upload { get; set; }

    public string My { get; set; } = null!;
    public string? PIN { get; set; }
    public string[]? To { get; set; }

    public string UploadPath { get; set; } = "Upload";
    public string ArchivePath { get; set; } = "Upload/Archive";

    public async Task Run()
    {
        Program.Settings.Bind(nameof(Zadacha137), this);
        Program.Settings.Bind(nameof(CryptoPro), this);

        if (Sign)
        {
            await SignAsync();
        }

        if (Encrypt)
        {
            await EncryptAsync();
        }
        
        if (Upload)
        {
            if (!Directory.Exists(ArchivePath))
                Directory.CreateDirectory(ArchivePath);

            await UploadAsync();
        }
    }

    public async Task SignAsync()
    {
        foreach (var zip in Directory.EnumerateFiles(UploadPath, "*.zip", _enumOptions))
        {
            await CryptoPro.SignDetachedFileAsync(zip, zip + ".sig", My, PIN);
        }
    }

    public async Task EncryptAsync()
    {
        foreach (var zip in Directory.EnumerateFiles(UploadPath, "*.zip", _enumOptions))
        {
            await CryptoPro.EncryptFileAsync(zip, zip + ".enc", My, To);
        }
    }

    public async Task UploadAsync()
    {
        foreach (var zip in Directory.EnumerateFiles(UploadPath, "*.zip", _enumOptions))
        {
            File.Move(zip, ArchivePath, true);
        }

        bool result = await restClient.UploadDirectoryAsync("Zadacha_137", 
            "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)", UploadPath);

        if (result)
        {
            foreach (var each in Directory.EnumerateFiles(UploadPath, "*", _enumOptions))
            {
                File.Move(each, ArchivePath, true);
            }
        }

        Console.WriteLine(result ? "OK" : "Fail");
    }
}
