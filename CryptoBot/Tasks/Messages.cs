#region License
/*
Copyright 2024 Dmitrii Evdokimov
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
using Diev.Portal5.Exceptions;

namespace CryptoBot.Tasks;

internal static class Messages
{
    private static readonly EnumerationOptions _enumOptions = new();
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
    };
    private static readonly char[] _separator = [' ', ',', ';', ':', '\t', '\n', '\r'];

    //config
    public static string DownloadPath { get; }
    public static bool Overwrite { get; }
    public static bool Decrypt { get; }
    public static string? DecryptTo { get; }
    public static bool Delete { get; }
    public static string? Subscribers { get; }

    static Messages()
    {
        var config = Program.Config.GetSection(nameof(Messages));

        DownloadPath = Path.GetFullPath(config[nameof(DownloadPath)] ?? ".");
        Overwrite = bool.Parse(config[nameof(Overwrite)] ?? "false");
        Decrypt = bool.Parse(config[nameof(Decrypt)] ?? "false");
        Delete = bool.Parse(config[nameof(Delete)] ?? "false");
        Subscribers = config[nameof(Subscribers)];
    }

    public static async Task LoadAsync(Guid? guid, MessagesFilter filter)
    {
        try
        {
            if (guid is not null && guid.HasValue)
            {
                string id = guid.ToString()!;

                Logger.TimeZZZLine($"Получение {id}");

                if (Program.Debug)
                    throw new TaskException("Программа в отладочном режиме");

                var message = await Program.RestAPI.GetMessageAsync(id)
                    ?? throw new TaskException($"Сообщение {id} не найдено.");

                await LoadMessageAsync(message);
                return;
            }

            if (filter.IsEmpty())
                throw new TaskException(
                    "Не задан фильтр сообщений для выполнения операции с ними - это опасно!");

            Logger.TimeZZZLine("Получение списка сообщений по фильтру");

            if (Program.Debug)
                throw new TaskException("Программа в отладочном режиме");

            var messages = await Program.RestAPI.GetMessagesAsync(filter)
                ?? throw new TaskException("Не получено сообщений.");

            if (messages.Count == 0)
            {
                //throw new TaskException("Получен пустой список сообщений.");
                Logger.TimeZZZLine("Нет сообщений.");
                return;
            }

            string text = $"В списке сообщений {messages.Count} шт.";
            Logger.TimeZZZLine(text);
            StringBuilder report = new();
            report
                .AppendLine(text)
                .AppendLine();
            int i = 0;

            foreach (var message in messages)
            {
                var msgInfo = await LoadMessageAsync(message);
                report
                    .AppendLine($"-{++i}-")
                    .AppendLine(msgInfo.Description)
                    .AppendLine();
            }

            Logger.TimeZZZLine("Список обработан.");

            switch (filter.Task)
            {
                case "2-1":
                    await Program.SendDoneAsync(
                        $"Messages.Zadacha_{filter.Task} Исходящие {messages.Count} шт.",
                        report.ToString(), Subscribers);
                    break;

                case "3-1":
                    await Program.SendDoneAsync(
                        $"Messages.Zadacha_{filter.Task} Входящие {messages.Count} шт.",
                        report.ToString(), Subscribers);
                    break;

                case "54":
                    await Program.SendDoneAsync(
                        $"Messages.Zadacha_{filter.Task} Запросы ЦИК {messages.Count} шт.",
                        report.ToString(), Subscribers);
                    break;

                case "130":
                case "137":
                default:
                    break;
            }
        }
        catch (Portal5Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(Messages), "API: " + ex.Message, Subscribers);
            Program.ExitCode = 3;
        }
        catch (TaskException ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(Messages), "Task: " + ex.Message, Subscribers);
            Program.ExitCode = 2;
        }
        catch (Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(Messages), ex, Subscribers);
            Program.ExitCode = 1;
        }
    }

    public static async Task CleanAsync(Guid? guid, MessagesFilter filter)
    {
        try
        {
            if (guid is not null && guid.HasValue)
            {
                string id = guid.ToString()!;

                Logger.TimeZZZLine($"Удаление сообщения {id}");

                if (!await Program.RestAPI.DeleteMessageAsync(id))
                    throw new TaskException($"Ошибка удаления сообщения {id}.");
                return;
            }

            if (filter.IsEmpty())
                throw new TaskException(
                    "Не задан фильтр сообщений для выполнения операции с ними - это опасно!");

            Logger.TimeZZZLine("Удаление сообщений по фильтру");

            await Program.RestAPI.DeleteMessagesAsync(filter);
        }
        catch (Portal5Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(Messages), "API: " + ex.Message, Subscribers);
            Program.ExitCode = 3;
        }
        catch (TaskException ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(Messages), "Task: " + ex.Message, Subscribers);
            Program.ExitCode = 2;
        }
        catch (Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(Messages), ex, Subscribers);
            Program.ExitCode = 1;
        }
    }

    public static async Task<MessageInfo> LoadMessageAsync(Message message)
    {
        string download = Path.Combine(
            Path.GetFullPath(DownloadPath),
            message.Type,
            message.TaskName,
            message.CreationDate.ToString("yyyy-MM"));

        if (!Directory.CreateDirectory(download).Exists)
            throw new DirectoryNotFoundException($"Не удалось создать директорию {download.PathQuoted()}.");

        string id = message.Id!;
        string name = $"{message.CreationDate:yyyy-MM-dd}-{id}";
        string path = Path.Combine(download, name);
        string json = path + ".json";

        if (!File.Exists(json) || Overwrite)
        {
            using var stream = File.OpenWrite(json);
            await JsonSerializer.SerializeAsync(stream, message, _jsonOptions);
        }

        string zip = path + ".zip";

        Logger.TimeZZZLine($"Получение {name}.zip [{message.TotalSize:N0}]");

        if (!File.Exists(zip) || Overwrite)
        {
            if (!await Program.RestAPI.DownloadMessageZipAsync(id, zip, Overwrite))
            {
                string error = $"Файл {name}.zip не получен.";
                Logger.TimeZZZLine(error);
                return new MessageInfo(message, error);
            }
        }

        MessageInfo msgInfo;

        if (Decrypt && message.Files.Count > 0)
        {
            Logger.TimeZZZLine($"Распаковка {name}.zip");
            msgInfo = await DecryptMessageZipAsync(message, zip);
        }
        else
        {
            string error = $"Файл {name}.zip не распакован.";
            Logger.TimeZZZLine(error);
            msgInfo = new MessageInfo(message, error);
        }

        if (Delete)
        {
            Logger.TimeZZZLine($"Удаление {id}.");
            await Program.RestAPI.DeleteMessageAsync(id);
        }

        return msgInfo;
    }

    public static async Task<MessageInfo> DecryptMessageZipAsync(Message message, string zip)
    {
        string temp = Path.Combine(Path.GetTempPath(), nameof(CryptoBot) + Path.GetRandomFileName());
        await Files.UnzipToDirectoryAsync(zip, temp);

        var source = Directory.GetDirectories(temp)[0];
        bool outbox = message.Type.Equals("outbox", StringComparison.OrdinalIgnoreCase);
        string form = Path.Combine(source, outbox ? "form.xml" : "passport.xml");
        string enc = form + ".enc";

        if (!File.Exists(form) && File.Exists(enc))
        {
            await Program.Crypto.DecryptFileAsync(enc, form);
        }

        var msgInfo = new MessageInfo(message, outbox, form);
        string store = Path.Combine(
            Path.GetFullPath(DownloadPath),
            message.Type,
            message.TaskName,
            msgInfo.Date[0..7], // 2024-10-04 => 2024-10
            msgInfo.PathName!);

        Logger.TimeZZZLine($"- {msgInfo.PathName}");

        await ExtractFilesToDirectoryAsync(message, source, store);
        await File.WriteAllTextAsync(Path.Combine(store, "info.txt"), msgInfo.Description);

        Directory.Delete(temp, true);

        return msgInfo;
    }

    private static async Task ExtractFilesToDirectoryAsync(Message message, string source, string store)
    {
        if (!Directory.CreateDirectory(store).Exists)
            throw new TaskException($"Невозможно создать директорию {store.PathQuoted()}.");

        foreach (var file in message.Files)
        {
            if (SkipFile(source, file)) //TODO
                continue;

            string src = Path.Combine(source, file.Name);
            string dst = Path.Combine(store, file.Name);

            if (!File.Exists(src))
            {
                Logger.TimeZZZLine($"В пакете не хватает файла {file.Name}");

                try
                {
                    await Program.RestAPI.DownloadMessageFileAsync(message.Id, file.Id!, src, Overwrite);
                }
                catch (Exception ex) // 400 - Общая ошибка запроса
                {
                    Logger.TimeZZZLine($"Его реально не скачать! {ex.Message}"); //TODO
                    continue;
                }
            }

            if (file.Encrypted
                // || file.Name.EndsWith(".enc", StringComparison.Ordinal)
                )
            {
                Logger.TimeZZZLine($"Расшифровка {file.Name}");
                await ExtractFileToDirectoryAsync(src, store);
                continue;
            }

            if (file.SignedFile is not null
                // || file.Name.EndsWith(".sig", StringComparison.Ordinal)
                )
                continue;

            if (!File.Exists(dst) || Overwrite)
            {
                Logger.TimeZZZLine($"Перенос {file.Name}");
                await Files.MoveToDirectoryAsync(src, store);
            }
        }
    }

    public static async Task ExtractFileToDirectoryAsync(string src, string store)
    {
        if (src.EndsWith(".enc", StringComparison.OrdinalIgnoreCase))
        {
            src = await Files.DecryptAsync(src);
        }

        if (src.EndsWith(".sig", StringComparison.OrdinalIgnoreCase))
        {
            src = await Files.UnsignAsync(src);
        }

        await Files.MoveToDirectoryAsync(src, store);
    }

    private static bool SkipFile(string dir, MessageFile file)
    {
        if (file.SignedFile is not null) // file.Name.EndsWith(".sig", StringComparison.Ordinal)
            return true;

        string path = Path.Combine(dir, file.Name);
        string decrypted = Path.ChangeExtension(path, null);
        string src = Path.ChangeExtension(decrypted, null);

        return file.Encrypted && !Overwrite &&
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
                            if ((receipt.Status == ReceiptOutStatus.Error) &&
                                (receipt.Message is not null))
                                throw new TaskException(
                                    "Получена ошибка: " + receipt.Message);

                            if ((receipt.Status == ReceiptOutStatus.Rejected) &&
                                (receipt.Message is not null))
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
