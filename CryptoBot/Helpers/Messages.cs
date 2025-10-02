#region License
/*
Copyright 2024-2025 Dmitrii Evdokimov
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
using System.Text.Encodings.Web;
using System.Text.Json;

using Diev.Extensions.LogFile;
using Diev.Extensions.Tools;
using Diev.Portal5;
using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Tools;

namespace CryptoBot.Helpers;

internal static class Messages
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
    };

    public static (string json, string zip) GetZipStore(Message message, string root)
    {
        string download = Path.Combine(
            Path.GetFullPath(root),
            message.Type,
            message.TaskName,
            message.CreationDate.ToString("yyyy-MM"));

        if (!Directory.CreateDirectory(download).Exists)
            throw new DirectoryNotFoundException($"Не удалось создать директорию {download.PathQuoted()}.");

        string id = message.Id!;
        string name = $"{message.CreationDate:yyyy-MM-dd}-{id}";
        string path = Path.Combine(download, name);

        string json = path + ".json";
        string zip = path + ".zip";

        return (json, zip);
    }

    public static string GetDocStore(Message message, MessageInfo msgInfo, string root)
    {
        //return Path.Combine(
        //    Path.GetFullPath(root),
        //    message.Type,
        //    message.TaskName,
        //    msgInfo.Date[0..7], // 2024-10-04 => 2024-10
        //    msgInfo.PathName!);

        return Path.Combine(
            Path.GetFullPath(root),
            message.Outbox ? "Исх.ЦБ" : "Вх.ЦБ", // "OUT" : "INC",
            msgInfo.Date[0..7], // 2024-10-04 => 2024-10
            msgInfo.Name!);
    }

    public static string GetDocStore2(Message message, MessageInfo msgInfo, string root)
    {
        return Path.Combine(
            Path.GetFullPath(root),
            message.Outbox ? "Исх.ЦБ" : "Вх.ЦБ",
            //msgInfo.Date[0..4], // 2024-10-04 => 2024
            msgInfo.Date[0..7], // 2024-10-04 => 2024-10
            msgInfo.Name!);
    }

    public static async Task<List<Message>?> GetMessagesAsync(MessagesFilter filter)
    {
        return await Program.RestAPI.GetMessagesAsync(filter)
            ?? throw new TaskException("Не получено сообщений.");
    }

    public static async Task<bool> SaveMessageJsonAsync(Message message, string path)
    {
        using var stream = File.OpenWrite(path);
        await JsonSerializer.SerializeAsync(stream, message, _jsonOptions);

        return File.Exists(path);
    }

    public static async Task<bool> SaveMessageZipAsync(string id, string path)
    {
        return await Program.RestAPI.DownloadMessageZipAsync(id, path);
    }

    public static async Task<MessageInfo> DecryptMessageZipAsync(Message message, string zip, string path, string? path2)
    {
        string temp = Files.CreateTempDir();
        await Files.UnzipToDirectoryAsync(zip, temp);

        var source = Directory.GetDirectories(temp)[0];
        string xml = Path.Combine(source, message.Outbox ? "form.xml" : "passport.xml");
        string enc = xml + ".enc";

        if (!File.Exists(xml) && File.Exists(enc))
        {
            await Program.Crypto.DecryptFileAsync(enc, xml);
        }

        var msgInfo = new MessageInfo(message, xml);
        var corrId = message.CorrelationId;

        if (!string.IsNullOrEmpty(corrId))
        {
            var corrMessage = await Program.RestAPI.GetMessageAsync(corrId);

            if (corrMessage != null)
            {
                var corrInfo = new MessageInfo(corrMessage);
                msgInfo.Notes = "На " + corrInfo.ToString();
            }
        }

        string docs = GetDocStore(message, msgInfo, path);
        await ExtractFilesToDirectoryAsync(message, source, docs);

        if (path2 is not null)
        {
            string docs2 = GetDocStore2(message, msgInfo, path2);

            if (!docs2.Equals(docs))
            {
                Directory.CreateDirectory(docs2);
                msgInfo.FullName = docs2;
                await File.WriteAllTextAsync(Path.Combine(docs2, "info.txt"), msgInfo.ToString());

                foreach (var file in Directory.GetFiles(docs))
                    File.Copy(file, Path.Combine(docs2, Path.GetFileName(file)), false);
            }
        }

        msgInfo.FullName = docs;
        await File.WriteAllTextAsync(Path.Combine(docs, "info.txt"), msgInfo.ToString());

        Directory.Delete(temp, true);

        return msgInfo;
    }

    public static async Task<MessageInfo> DecryptMessageFilesAsync(Message message, string path, string? path2)
    {
        string temp = Files.CreateTempDir();
        StringBuilder notes = new();

        foreach (var file in message.Files)
        {
            string name = Path.Combine(temp, file.Name);

            if (!await Program.RestAPI.DownloadMessageFileAsync(message.Id, file.Id, name, true))
            {
                string error = $"Файл вложения {file.Name.PathQuoted()} не скачать.";
                Logger.TimeZZZLine(error);
                notes.AppendLine(error);
            }
        }

        string xml = Path.Combine(temp, message.Outbox ? "form.xml" : "passport.xml");
        string enc = xml + ".enc";

        if (!File.Exists(xml) && File.Exists(enc))
        {
            await Program.Crypto.DecryptFileAsync(enc, xml);
        }

        var msgInfo = new MessageInfo(message, xml);
        var corrId = message.CorrelationId;

        if (!string.IsNullOrEmpty(corrId))
        {
            var corrMessage = await Program.RestAPI.GetMessageAsync(corrId);

            if (corrMessage != null)
            {
                var corrInfo = new MessageInfo(corrMessage);
                notes.Append("На " + corrInfo.ToString());
            }
        }

        string docs = GetDocStore(message, msgInfo, path);
        await ExtractFilesToDirectoryAsync(message, temp, docs);
        msgInfo.FullName = docs;
        msgInfo.Notes = notes.ToString();

        await File.WriteAllTextAsync(Path.Combine(docs, "info.txt"), msgInfo.ToString());

        if (path2 is not null)
        {
            string docs2 = GetDocStore2(message, msgInfo, path2);
            Directory.CreateDirectory(docs2);

            foreach (var file in Directory.GetFiles(docs))
                File.Copy(file, Path.Combine(docs2, Path.GetFileName(file)), true);
        }

        Directory.Delete(temp, true);

        return msgInfo;
    }

    public static async Task<MessageInfo?> GetMessageInfoAsync(string msgId)
    {
        var message = await Program.RestAPI.GetMessageAsync(msgId);

        if (message is null)
            return null;

        foreach (var file in message.Files)
        {
            if (file.Name.StartsWith("passport.xml", StringComparison.OrdinalIgnoreCase)
                || file.Name.StartsWith("form.xml", StringComparison.OrdinalIgnoreCase))
            {
                string xml = Files.GetTempName();

                if (file.Encrypted)
                {
                    string enc = Files.GetTempName();
                    await Program.RestAPI.DownloadMessageFileAsync(msgId, file.Id, enc);
                    await Program.Crypto.DecryptFileAsync(enc, xml);
                    File.Delete(enc);
                }
                else
                {
                    await Program.RestAPI.DownloadMessageFileAsync(msgId, file.Id, xml);
                }

                var msgInfo = new MessageInfo(message, xml);
                File.Delete(xml);

                return msgInfo;
            }
        }

        return new MessageInfo(message);
    }

    public static async Task ExtractFilesToDirectoryAsync(Message message, string source, string path)
    {
        if (!Directory.CreateDirectory(path).Exists)
            throw new TaskException($"Невозможно создать директорию {path.PathQuoted()}.");

        foreach (var file in message.Files)
        {
            //if (SkipFile(source, file)) //TODO
            //    continue;

            string src = Path.Combine(source, file.Name);
            string dst = Path.Combine(path, file.Name);

            if (!File.Exists(src))
            {
                Logger.TimeZZZLine($"В пакете не хватает файла {file.Name.PathQuoted()}");

                try
                {
                    await Program.RestAPI.DownloadMessageFileAsync(message.Id, file.Id!, src);
                }
                catch (Exception ex) // 400 - Общая ошибка запроса
                {
                    Logger.TimeZZZLine($"Его реально не скачать! {ex.Message}"); //TODO
                    continue;
                }
            }

            if (!File.Exists(src))
            {
                //Logger.TimeZZZLine("Его реально не скачать - пропускаем!"); //TODO
                continue;
            }

            if (file.Encrypted)
            {
                //Logger.TimeZZZLine($"Расшифровка {file.Name.PathQuoted()}");
                await ExtractFileToDirectoryAsync(src, path);
                continue;
            }

            if (file.SignedFile is not null)
                continue;

            if (!File.Exists(dst))
            {
                //Logger.TimeZZZLine($"Перенос {file.Name.PathQuoted()}");
                await Files.MoveToDirectoryAsync(src, path);
            }
        }
    }

    public static async Task ExtractFileToDirectoryAsync(string src, string path)
    {
        if (src.EndsWith(".enc", StringComparison.OrdinalIgnoreCase))
        {
            src = await Files.DecryptAsync(src);
        }

        if (src.EndsWith(".sig", StringComparison.OrdinalIgnoreCase))
        {
            src = await Files.UnsignAsync(src);
        }

        await Files.MoveToDirectoryAsync(src, path);
    }

    private static bool SkipFile(string dir, MessageFile file)
    {
        if (file.SignedFile is not null) // file.Name.EndsWith(".sig", StringComparison.Ordinal)
            return true;

        string path = Path.Combine(dir, file.Name);
        string decrypted = Path.ChangeExtension(path, null);
        string src = Path.ChangeExtension(decrypted, null);

        return file.Encrypted &&
            (File.Exists(path) || File.Exists(decrypted) || File.Exists(src));
    }

    /// <summary>
    /// Получение регистрации электронного сообщения.
    /// </summary>
    /// <param name="msgId">Идентификатор сообщения.</param>
    /// <param name="minutes">Время ожидания в минутах, прежде чем прекратить попытки.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<Message> CheckStatusAsync(string msgId, int minutes)
    {
        var end = DateTime.Now.AddMinutes(minutes);
        Message? message = null;

        while (DateTime.Now < end)
        {
            message = await Program.RestAPI.GetMessageAsync(msgId);

            if (message is not null)
            {
                if (message.Status == MessageOutStatus.Registered)
                    return message; // OK

                if (message.Status == MessageOutStatus.Error ||
                    message.Status == MessageOutStatus.Rejected)
                {
                    if (message.Receipts is not null)
                    {
                        foreach (var receipt in message.Receipts)
                        {
                            if (receipt.Status == ReceiptOutStatus.Error &&
                                receipt.Message is not null)
                                throw new TaskException(
                                    "Получена ошибка: " + receipt.Message);

                            if (receipt.Status == ReceiptOutStatus.Rejected &&
                                receipt.Message is not null)
                                throw new TaskException(
                                    "Получен отказ: " + receipt.Message);
                        }
                    }

                    throw new TaskException(
                        "Получены ошибка или отказ, но нет квитанции.");
                }
            }

            Thread.Sleep(30000);
        }

        if (message is not null)
        {
            //throw new Exception($"За {minutes} минут статус лишь '{message.Status}'.");
            return message; // sent, delivered, [-error], processing, [-registered]
        }

        throw new TaskException(
            $"За {minutes} минут так ничего и не получено по отправке.");
    }
}
