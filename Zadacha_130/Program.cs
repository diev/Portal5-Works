#region License
/*
Copyright 2023 Dmitrii Evdokimov
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

using Diev.Portal5;
using Diev.Portal5.API;

namespace Zadacha_130;

/*
 
22:00 CryptoBot ЗСК. Получение реестра с портала5: /usr/local/zsk_bin/download.bat
        download C:\FORMS\ZSK\IN
            download KYC_20231212.xml.zip.enc
            download KYC_20231212.xml.zip.sig
        decr    C:\FORMS\ZSK\IN\KYC_20231212.xml.zip

var json = await restClient.GetMessagesAsync("Zadacha_130", DateTime.Now.AddDays(-7), DateTime.Now);
var json = await restClient.GetFilesAsync("Zadacha_130", DateTime.Now.AddDays(-7), DateTime.Now, "Download");

*/

internal class Program
{
    private static readonly RestAPI _restAPI = new();

    static int Main(string[] args)
    {
        try
        {
            if (args.Length != 3)
            {
                throw new Exception("Укажите директорию для получения, логин и пароль.");
            }

            string path = args[0];

            if (!Directory.Exists(path))
            {
                throw new Exception("Указанная директория не существует.");
            }

            _restAPI.Login(args[1], args[2]);

            for (int retries = 1; retries <= 10; retries++)
            {
                if (DownloadLastFile(7, path))
                {
                    foreach (var file in Directory.GetFiles(path))
                    {
                        File.Delete(file);
                    }

                    Console.WriteLine("Файл получен.");
                    return 0;
                }

                Thread.Sleep(retries * 2000);
            }

            throw new Exception("Ничего не получено.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 1;
        }
    }

    private static bool DownloadLastFile(int days, string path)
    {
        // url = "back/rapi2/messages/8a3306a7-2025-4726-8d7c-ae3200aacaf0/files/948b6d20-c122-417c-9b92-2c3a14ec8de3/download";
        // path = "KGR_20220204_132116_request_128779.zip";

        var filter = new MessagesFilter()
        {
            Task = "Zadacha_130",
            MinDateTime = DateTime.Now.AddDays(-days)
        };

        MessagesPage? messagesPage = null;

        for (int retries = 1; retries <= 10; retries++)
        {
            messagesPage = _restAPI.GetMessagesPageAsync(filter).Result;

            if (messagesPage != null && messagesPage.Messages.Count > 0)
            {
                break;
            }

            Thread.Sleep(retries * 2000);
        }

        if (messagesPage is null || messagesPage.Messages.Count == 0)
        {
            return false;
        }

        string lastName = "KYC_20230000.xml.zip.enc";
        string? messageId = null;
        string? fileId = null;

        foreach (var message in messagesPage.Messages)
        {
            foreach (var messageFile in message.Files)
            {
                if (messageFile.Encrypted)
                {
                    if (string.Compare(messageFile.Name, lastName, true) > 0)
                    {
                        lastName = messageFile.Name;

                        messageId = message.Id;
                        fileId = messageFile.Id;
                    }
                }
            }
        }

        if (messageId is null)
            throw new ArgumentNullException(messageId, "Не получено подходящего сообщения.");

        if (fileId is null)
            throw new ArgumentNullException(fileId, "Не получено подходящего файла в сообщении.");

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string file = Path.Combine(path, lastName);

        for (int retries = 1; retries <= 10; retries++)
        {
            _restAPI.DownloadMessageFileAsync(messageId, fileId, file, true).Wait();

            if (File.Exists(file))
            {
                return true;
            }

            Thread.Sleep(retries * 2000);
        }

        return false;
    }
}
