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

using Diev.Extensions.Crypto;
using Diev.Extensions.LogFile;
using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Tools;

namespace CryptoBot.Tasks;

internal static class BulkLoad
{
    private static readonly EnumerationOptions _enumOptions = new();
    private static bool _ok;

    public static string DownloadPath { get; set; }
    public static bool Overwrite { get; set; }
    public static bool Decrypt { get; set; }
    public static bool Delete { get; set; }

    static BulkLoad()
    {
        var config = Program.Config.GetSection(nameof(BulkLoad));

        DownloadPath = Path.GetFullPath(config[nameof(DownloadPath)] ?? ".");
        Overwrite = bool.Parse(config[nameof(Overwrite)] ?? "false");
        Decrypt = bool.Parse(config[nameof(Decrypt)] ?? "false");
        Delete = bool.Parse(config[nameof(Delete)] ?? "false");
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

                foreach (var message in messagesPage.Messages)
                {
                    if (SkipZadacha(message.TaskName!))
                        continue;

                    _ok = true;
                    string title = MakeTitle(message);

                    string dir = Path.Combine(
                        Path.GetFullPath(DownloadPath),
                        message.Type!,
                        message.TaskName!,
                        $"{message.CreationDate:yyyy-MM}",
                        $"{message.CreationDate:yyyy-MM-dd}-{message.Id[0..8]} {title}");

                    //TODO if (message.Type.Equals("outbox") && File.Exists("form.xml")
                    // parse <mf:doc_out Number="44-3-1" Date="2024-03-13"/>
                    // for => "2024-03-13-44-3-1" !!!

                    Directory.CreateDirectory(dir);

                    string messageId = message.Id!;
                    string json = Path.Combine(dir, "message.json");
                    string info = Path.Combine(dir, "info.txt");
                    string zip = Path.Combine(dir, messageId + ".zip");

                    await DownloadJson(messageId, json);

                    if (message.Files.Count > 0)
                    {
                        await DownloadZip(messageId, zip);
                        await MakeInfo(message, info);

                        foreach (var file in message.Files)
                        {
                            if (SkipFile(dir, file))
                                continue;

                            string path = Path.Combine(dir, file.Name);

                            if (await DownloadFile(messageId, file, path))
                            {
                                string decrypted = Path.ChangeExtension(path, null);

                                if (Decrypt && file.Encrypted)
                                {
                                    await DecryptFile(path, decrypted);
                                    string src = Path.ChangeExtension(decrypted, null);

                                    if (decrypted.EndsWith(".sig", StringComparison.Ordinal))
                                    {
                                        await DesigFile(decrypted, src);
                                    }
                                }
                            }
                        }
                    }

                    if (Delete && _ok)
                    {
                        await DeleteMessage(message);
                    }
                }

                if (messagesPage.Pages.CurrentPage == messagesPage.Pages.TotalPages)
                    break;

                Thread.Sleep(4000); // anti DDoS

                messagesPage = await Program.RestAPI.GetMessagesPageAsync(filter, ++page); //TODO gaps when Delete!
            }

            Console.WriteLine("--- Page end ---");
        }
        catch (Exception ex)
        {
            Logger.TimeLine(ex.Message);
            Logger.LastError(ex);
        }
    }

    private static string MakeTitle(Message message)
    {
        string title = $"{message.Title} {message.Text}".Trim();

        title = string.Join(' ', title.Split(' ',
            StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries));

        title = string.Join('_', title.Split(Path.GetInvalidFileNameChars(),
            StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries));

        if (title.Length > 0)
        {
            if (char.IsLower(title[0]))
            {
                char[] a = title.ToCharArray();
                a[0] = char.ToUpper(a[0]);
                title = new string(a);
            }

            if (title.Length > 64)
            {
                title = title[..64].Trim(); // + "_"; // "..."
            }
        }
        else
        {
            title = "описания нет";
        }

        return title;
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
        //if (file.Name.StartsWith("form.xml.", StringComparison.Ordinal))
        //    return true;

        if (file.Name.Equals("passport.xml", StringComparison.Ordinal))
            return true;

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

    private static async Task MakeInfo(Message message, string path)
    {
        StringBuilder info = new();
        info.AppendLine(message.Title);
        info.AppendLine(message.Text);
        info.AppendLine(message.TaskName);
        info.AppendLine(message.Type);
        info.AppendLine($"{message.CreationDate:yyyy-MM-dd}");
        info.AppendLine(message.Id);
        info.AppendLine($"на {message.CorrelationId}");
        await File.WriteAllTextAsync(path, info.ToString());
    }

    private static async Task<bool> DownloadFile(string messageId, MessageFile file, string path)
    {
        try
        {
            await Program.RestAPI.DownloadMessageFileAsync(messageId, file.Id!, path, Overwrite);
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
        CryptoPro crypto = new();

        if (await crypto.DecryptFileAsync(path, decrypted))
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
            Console.WriteLine($"Неудача удаления сообщения: {ex.Message}");
        }
    }
}
