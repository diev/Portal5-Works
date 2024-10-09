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

using Diev.Extensions.Crypto;
using Diev.Extensions.LogFile;
using Diev.Portal5;
using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Tools;

namespace CryptoBot.Tasks;

internal static class BulkLoad
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

    static BulkLoad()
    {
        var config = Program.Config.GetSection(nameof(BulkLoad));

        DownloadPath = Path.GetFullPath(config[nameof(DownloadPath)] ?? ".");
        Overwrite = bool.Parse(config[nameof(Overwrite)] ?? "false");
        Decrypt = bool.Parse(config[nameof(Decrypt)] ?? "false");
        Delete = bool.Parse(config[nameof(Delete)] ?? "false");

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

    public static async Task RunAsync(MessagesFilter filter, int page = 1)
    {
        try
        {
            //GET: */messages?Type=inbox&Status=read&Page=2
            // получить прочитанные Входящие

            //GET: */messages?Task=Zadacha_2-1&Status=registered
            // получить все документы, отправленные участником и зарегистрированные Банком России

            var messagesPage = await Program.RestAPI.GetMessagesPageAsync(filter, page)
                ?? throw new Exception("Не получено сообщений.");

            if (messagesPage.Messages.Count == 0)
                throw new Exception("Получен пустой список сообщений.");

            Console.WriteLine($"Messages: {messagesPage.Pages.TotalRecords}");

            while (true)
            {
                Console.WriteLine($"--- Page {page} ---");

                foreach (var message in messagesPage!.Messages)
                {
                    if (SkipZadacha(message.TaskName!))
                        continue;

                    string temp = Program.GetTempPath(DownloadPath);
                    _ok = true;

                    string msgId = message.Id!;
                    string json = Path.Combine(temp, "message.json");
                    string info = Path.Combine(temp, "info.txt");
                    string zip = Path.Combine(temp, msgId + ".zip");

                    await DownloadJson(msgId, json);
                    await DownloadZip(msgId, zip);

                    if (message.Files.Count > 0)
                    {
                        foreach (var file in message.Files)
                        {
                            if (SkipFile(temp, file))
                                continue;

                            string path = Path.Combine(temp, file.Name);

                            if (!await DownloadFile(msgId, file, path))
                            {
                                Logger.TimeLine($@"Ошибка загрузки ""{file.Name}"" из '{msgId}'!");
                                continue;
                            }
                            
                            if (Decrypt && file.Encrypted)
                            {
                                string decrypted = Path.ChangeExtension(path, null);
                                await DecryptFile(path, decrypted);

                                if (decrypted.EndsWith(".sig", StringComparison.Ordinal))
                                {
                                    string src = Path.ChangeExtension(decrypted, null);
                                    await DesigFile(decrypted, src);
                                }
                            }
                        }
                    }

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

                if (messagesPage.Pages.CurrentPage == messagesPage.Pages.TotalPages)
                    break;

                messagesPage = await Program.RestAPI.GetMessagesPageAsync(filter, ++page);
            }

            Console.WriteLine("--- Page end ---");
        }
        catch (Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);
        }
    }

    private static bool SkipZadacha(string Zadacha)
    {
        string[] tasks = [
            // inbox
            "Zadacha_97",  // Извещение о результатах контроля информации о ВПОДК и их результатах (ИЭС1)
            "Zadacha_107", // Извещение о результатах контроля отчетности субъектов НПС (ИЭС1)
            "Zadacha_114", // Извещение о результатах контроля представления формы 0409310 (ИЭС1)
            "Zadacha_123", // Извещение о результатах контроля представления формы 0409310 (ИЭС2)
            "Zadacha_130", // Получение информации об уровне риска ЮЛ/ИП
            "Zadacha_133", // Извещение о результатах контроля отчетности субъектов НПС (ИЭС2)
            "Zadacha_140", // Извещение о результатах контроля информации о ВПОДК и их результатах (ИЭС2)
            "Zadacha_156", // Извещение о результатах контроля представления формы 0409601 отчет ко(нко) (ИЭС1) и др.
            "Zadacha_159", // Извещение о результатах контроля представления формы 0409601 отчет ко(нко) (ИЭС2) и др.
            // outbox
            "Zadacha_155"  // Представление отчетности КО в Банк России
            ];

        if (tasks.Contains(Zadacha))
            return true;

        return false;
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

    private static async Task DownloadJson(string messageId, string path)
    {
        try
        {
            await Program.RestAPI.DownloadMessageJsonAsync(messageId, path, Overwrite);
        }
        catch
        {
            _ok = false; // see Trace.log for errors
        }
    }

    private static async Task DownloadZip(string messageId, string path)
    {
        try
        {
            await Program.RestAPI.DownloadMessageZipAsync(messageId, path, Overwrite);
        }
        catch
        {
            _ok = false;
        }
    }

    private static async Task<bool> DownloadFile(string msgId, MessageFile file, string path)
    {
        try
        {
            await Program.RestAPI.DownloadMessageFileAsync(msgId, file.Id!, path, Overwrite);
            return true;
        }
        catch
        {
            _ok = false;
            return false;
        }
    }

    private static async Task DecryptFile(string path, string decrypted)
    {
        if (await _crypto!.DecryptFileAsync(path, decrypted))
            File.Delete(path);
    }

    private static async Task DesigFile(string path, string src)
    {
        //await CryptoPro.VerifyFileAsync(path, src);
        //await PKCS7.CleanSignAsync(path, src);
        await ASN1.CleanSignAsync(path, src);
        //File.Delete(path);
    }

    private static async Task DeleteMessage(Message message)
    {
        try
        {
            await Program.RestAPI.DeleteMessageAsync(message.Id!);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Неудача удаления сообщения: " + ex.Message);
        }
    }
}
