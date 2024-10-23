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

using Diev.Extensions;
using Diev.Extensions.Crypto;
using Diev.Extensions.LogFile;
using Diev.Portal5;
using Diev.Portal5.API.Messages;
using Diev.Portal5.Exceptions;

namespace CryptoBot.Tasks;

internal static class MessageLoad
{
    private static readonly EnumerationOptions _enumOptions = new();
    private static readonly char[] _separator = [' ', ',', ';', ':', '\t', '\n', '\r'];
    private static bool _ok;

    private static readonly CryptoPro? _crypto;

    //config
    public static string DownloadPath { get; }
    public static bool Overwrite { get; }
    public static bool Decrypt { get; }
    public static string? DecryptTo { get; }
    public static bool Delete { get; } //TODO
    public static string? Subscribers { get; }

    static MessageLoad()
    {
        var config = Program.Config.GetSection(nameof(MessagesLoad));

        DownloadPath = Path.GetFullPath(config[nameof(DownloadPath)] ?? ".");
        Overwrite = bool.Parse(config[nameof(Overwrite)] ?? "false");
        Decrypt = bool.Parse(config[nameof(Decrypt)] ?? "false");
        Delete = bool.Parse(config[nameof(Delete)] ?? "false");
        Subscribers = config[nameof(Subscribers)];

        if (Decrypt)
        {
            _crypto = new(Program.UtilName, Program.CryptoName);
            DecryptTo = config[nameof(DecryptTo)];

            if (!string.IsNullOrWhiteSpace(DecryptTo))
            {
                _crypto.MyOld = DecryptTo.Split(_separator,
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }

    public static async Task RunAsync(string id)
    {
        try
        {
            var message = await Program.RestAPI.GetMessageAsync(id)
                ?? throw new Exception("Не получено сообщения.");

            await LoadMessageAsync(message);
        }
        catch (Portal5Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(MessagesLoad), "API: " + ex.Message, Subscribers);
            Program.ExitCode = 3;
        }
        catch (TaskException ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(MessagesLoad), "Task: " + ex.Message, Subscribers);
            Program.ExitCode = 2;
        }
        catch (Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);

            await Program.SendFailAsync(nameof(MessagesLoad), ex, Subscribers);
            Program.ExitCode = 1;
        }
    }

    public static async Task LoadMessageAsync(Message message)
    {
        if (Decrypt)
        {
            await DecryptMessageAsync(message);
        }
        else
        {
            await StoreMessageAsync(message);
        }
    }

    public static async Task DecryptMessageAsync(Message message)
    {
        string temp = Program.GetTempPath(DownloadPath);
        _ok = true;

        string msgId = message.Id!;
        string json = Path.Combine(temp, msgId + ".json"); //"message.json");
        string info = Path.Combine(temp, "info.txt");
        string zip = Path.Combine(temp, msgId + ".zip");

        await DownloadJsonAsync(msgId, json);
        await DownloadZipAsync(msgId, zip);

        if (message.Files.Count > 0)
        {
            foreach (var file in message.Files)
            {
                if (SkipFile(temp, file))
                    continue;

                string path = Path.Combine(temp, file.Name);

                if (!await DownloadFileAsync(msgId, file, path))
                {
                    Logger.TimeLine($"Ошибка загрузки {file.Name.PathQuoted()} из '{msgId}'!");
                    continue;
                }

                if (file.Encrypted)
                {
                    string decrypted = Path.ChangeExtension(path, null);
                    await DecryptFileAsync(path, decrypted);

                    if (decrypted.EndsWith(".sig", StringComparison.Ordinal))
                    {
                        string src = Path.ChangeExtension(decrypted, null);
                        await DesigFileAsync(decrypted, src);
                    }
                }
            }
        }

        if (_ok)
        {
            var msgInfo = new MessageInfo(message, temp);
            Console.WriteLine(msgInfo.PathName);
            Logger.TimeLine($"Save {msgInfo.PathName}");
            await File.AppendAllTextAsync(info, msgInfo.Description);

            string dir = Path.Combine(
                Path.GetFullPath(DownloadPath),
                message.Type,
                message.TaskName,
                msgInfo.Date[0..7]); // 2024-10-04 => 2024-10

            string save = Path.Combine(dir, msgInfo.PathName);

            Directory.CreateDirectory(dir);

            if (Directory.Exists(save))
                Directory.Delete(save, true);

            Directory.Move(temp, save);

            if (Delete)
            {
                await Program.RestAPI.DeleteMessageAsync(message.Id);
            }
        }
        else
        {
            Logger.TimeLine($"Error with message {msgId}");
        }
    }

    public static async Task StoreMessageAsync(Message message)
    {
        _ok = true;

        string dir = Path.Combine(
            Path.GetFullPath(DownloadPath),
            message.Type,
            message.TaskName,
            message.CreationDate.ToString("yyyy-MM"));

        if (!Directory.CreateDirectory(dir).Exists)
            throw new DirectoryNotFoundException($"Не удалось создать директорию {dir.PathQuoted()}.");

        string msgId = message.Id!;
        string json = Path.Combine(dir, msgId + ".json");
        string zip = Path.Combine(dir, msgId + ".zip");

        await DownloadJsonAsync(msgId, json);
        await DownloadZipAsync(msgId, zip);

        if (!_ok)
        {
            Logger.TimeLine($"Error with message {msgId}");
        }
    }

    private static bool SkipFile(string dir, MessageFile file)
    {
        if (file.SignedFile != null) // file.Name.EndsWith(".sig", StringComparison.Ordinal)
            return true;

        string path = Path.Combine(dir, file.Name);
        string decrypted = Path.ChangeExtension(path, null);
        string src = Path.ChangeExtension(decrypted, null);

        if (file.Encrypted && !Overwrite &&
            (File.Exists(path) || File.Exists(decrypted) || File.Exists(src)))
            return true;

        return false;
    }

    private static async Task DownloadJsonAsync(string messageId, string path)
    {
        try
        {
            _ok = await Program.RestAPI.DownloadMessageJsonAsync(messageId, path, Overwrite);
        }
        catch
        {
            _ok = false; // see Trace.log for errors
        }
    }

    private static async Task DownloadZipAsync(string messageId, string path)
    {
        try
        {
            _ok = await Program.RestAPI.DownloadMessageZipAsync(messageId, path, Overwrite);
        }
        catch
        {
            _ok = false;
        }
    }

    private static async Task<bool> DownloadFileAsync(string msgId, MessageFile file, string path)
    {
        try
        {
            _ok = await Program.RestAPI.DownloadMessageFileAsync(msgId, file.Id!, path, Overwrite);
        }
        catch
        {
            _ok = false;
        }

        return _ok;
    }

    private static async Task DecryptFileAsync(string path, string decrypted)
    {
        if (await _crypto!.DecryptFileAsync(path, decrypted))
            File.Delete(path);
    }

    private static async Task DesigFileAsync(string path, string src)
    {
        //await CryptoPro.VerifyFileAsync(path, src);
        //await PKCS7.CleanSignAsync(path, src);
        await ASN1.CleanSignAsync(path, src);
        //File.Delete(path);
    }

    private static async Task DeleteMessageAsync(Message message)
    {
        try
        {
            await Program.RestAPI.DeleteMessageAsync(message.Id!);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Сообщение не было удалено: " + ex.Message);
        }
    }
}
