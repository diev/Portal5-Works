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

using Diev.Extensions.Crypto;
using Diev.Extensions.Tools;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Exceptions;

namespace CryptoBot.Helpers;

internal static class Files
{
    public static async Task<string> DownloadLastEncryptedAsync(MessagesFilter filter, string path)
    {
        // url = "back/rapi2/messages/8a3306a7-2025-4726-8d7c-ae3200aacaf0/files/948b6d20-c122-417c-9b92-2c3a14ec8de3/download";
        // path = "KGR_20220204_132116_request_128779.zip";

        var messages = await Program.RestAPI.GetMessagesAsync(filter)
            ?? throw new TaskException(
                $"Не получено никаких сообщений.");

        int count = messages.Count;

        if (count == 0)
        {
            throw new NoMessagesException(
                $"Получено ноль сообщений.");
            //return null;
        }

        string? lastName = null;
        string? fileId = null;

        var message = messages[count - 1];
        string msgId = message.Id;

        foreach (var msgFile in message.Files)
        {
            if (msgFile.Encrypted)
            {
                if (msgFile.Name.Equals("form.xml.enc", StringComparison.Ordinal))
                    continue;

                lastName = msgFile.Name; // "KYC_yyyyMMdd.xml.zip.enc";
                fileId = msgFile.Id;
                break;
            }
        }

        if (fileId is null)
            throw new TaskException(
                $"Не получен Id файла из сообщения '{msgId}'.");

        if (!Directory.CreateDirectory(path).Exists)
            throw new TaskException(
                $"Не удалось создать папку {path.PathQuoted()}.");

        string file = Path.Combine(path, lastName!);

        if (await Program.RestAPI.DownloadMessageFileAsync(msgId, fileId, file))
            return file;

        throw new TaskException(
            $"Не удалось получить файл {lastName?.PathQuoted()}.");
    }

    /// <summary>
    /// Расшифровка файла .enc.
    /// </summary>
    /// <param name="enc">Исходный файл .enc.</param>
    /// <returns>Расшифрованный файл без .enc.</returns>
    /// <exception cref="TaskException"></exception>
    public static async Task<string> DecryptAsync(string enc)
    {
        string orig = Path.ChangeExtension(enc, null);

        if (await Program.Crypto.DecryptFileAsync(enc, orig))
        {
            File.Delete(enc);
            return orig;
        }

        //throw new TaskException(
        //    $"Файл {Path.GetFileName(enc).PathQuoted()} расшифровать не удалось.");

        return enc;
    }

    public static async Task<string> UnsignAsync(string sig)
    {
        string orig = Path.ChangeExtension(sig, null);

        //await CryptoPro.VerifyFileAsync(sig, path+);
        //await PKCS7.CleanSignAsync(sig, path+);
        if (await ASN1.CleanSignAsync(sig, orig))
        {
            File.Delete(sig);
            return orig;
        }

        //throw new TaskException(
        //    $"С файла {Path.GetFileName(sig).PathQuoted()} снять подпись не удалось.");

        return sig;
    }

    public static async Task UnzipToDirectoryAsync(string zip, string path)
    {
        await Task.Run(() =>
        {
            ZipFile.ExtractToDirectory(zip, path, true);
        });
    }

    public static async Task ZipDirectoryAsync(string path, string zip)
    {
        await Task.Run(() =>
        {
            ZipFile.CreateFromDirectory(path, zip);
        });
    }

    public static async Task MoveToDirectoryAsync(string zip, string path)
    {
        await Task.Run(() =>
        {
            string dst = Path.Combine(path, Path.GetFileName(zip));
            File.Move(zip, dst, true); //TODO real Async
        });
    }

    public static string GetTempName() =>
        Path.Combine(Path.GetTempPath(), nameof(CryptoBot) + Path.GetRandomFileName());

    public static string CreateTempDir() =>
        Directory.CreateTempSubdirectory(nameof(CryptoBot)).FullName;

    public static string MakeUrl(string path) =>
        "[InternetShortcut]" + Environment.NewLine +
        "URL=file://localhost/C" + path[2..].Replace('\\', '/').Replace(" ", "%20");

    /// <summary>
    /// Создание файлов подписи и зашифрованного.
    /// </summary>
    /// <param name="path">Исходный файл.</param>
    /// <param name="encryptTo">Отпечатки сертификатов получателей.</param>
    /// <param name="temp">Временная папка с создаваемыми для отправки файлами.</param>
    /// <returns></returns>
    /// <exception cref="TaskException"></exception>
    public static async Task SignAndEncryptToDirectoryAsync(string path, string encryptTo, string store)
    {
        string file = Path.GetFileName(path);
        string sig = Path.Combine(store, file + ".sig");
        string enc = Path.Combine(store, file + ".enc");

        if (!await Program.Crypto.SignDetachedFileAsync(path, sig))
            throw new TaskException(
                $"Подписать файл {path.PathQuoted()} не удалось.");

        if (!await Program.Crypto.EncryptFileAsync(path, enc, encryptTo))
            throw new TaskException(
                $"Зашифровать файл {path.PathQuoted()} не удалось.");
    }
}
