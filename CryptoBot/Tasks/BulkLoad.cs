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
using System.Xml;

using Diev.Extensions.Crypto;
using Diev.Extensions.LogFile;
using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Tools;

namespace CryptoBot.Tasks;

internal static class BulkLoad
{
    private static readonly EnumerationOptions _enumOptions = new();
    private static bool _ok;

    //config
    public static string DownloadPath { get; }
    public static bool Overwrite { get; }
    public static bool Decrypt { get; }

    static BulkLoad()
    {
        var config = Program.Config.GetSection(nameof(BulkLoad));

        DownloadPath = Path.GetFullPath(config[nameof(DownloadPath)] ?? ".");
        Overwrite = bool.Parse(config[nameof(Overwrite)] ?? "false");
        Decrypt = bool.Parse(config[nameof(Decrypt)] ?? "false");
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
                    string? formtype = null;
                    string? datenum = null;
                    _ok = true;

                    //TODO if (message.Type.Equals("outbox") && File.Exists("form.xml")
                    // parse <mf:doc_out Number="44-3-1" Date="2024-03-13"/>
                    // for => "2024-03-13-44-3-1" !!!

                    string msgId = message.Id!;
                    string json = Path.Combine(temp, "message.json");
                    string info = Path.Combine(temp, "info.txt");
                    string zip = Path.Combine(temp, msgId + ".zip");

                    await DownloadJson(msgId, json);

                    if (message.Files.Count > 0)
                    {
                        await DownloadZip(msgId, zip);
                        await MakeInfo(message, info);

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

                        if (message.Type.Equals("outbox", StringComparison.OrdinalIgnoreCase))
                        {
                            string form = Path.Combine(temp, "form.xml");

                            if (File.Exists(form))
                            {
                                try // find <mf:doc_out Number="52-2" Date="2024-03-25"/>
                                {
                                    XmlDocument doc = new();
                                    doc.Load(form);
                                    var root = doc.DocumentElement!;
                                    var pr = root.Prefix;
                                    var ns = root.NamespaceURI;
                                    XmlNamespaceManager nsmgr = new(doc.NameTable);
                                    nsmgr.AddNamespace(pr, ns);
                                    var node = root.SelectSingleNode($"//{pr}:doc_out", nsmgr);
                                    string number = node!.Attributes!["Number"]!.Value;
                                    string date = node!.Attributes!["Date"]!.Value;

                                    formtype = root.LocalName;
                                    datenum = $"{date}-{number}";
                                }
                                catch (Exception ex)
                                {
                                    Logger.LastError(ex);
                                    Logger.TimeLine($@"Файл ""form.xml"" в '{msgId}' имеет ошибку: {ex.Message}");
                                }
                            }
                        }
                        else if (message.Type.Equals("inbox", StringComparison.OrdinalIgnoreCase))
                        {
                            string passport = Path.Combine(temp, "passport.xml");

                            if (File.Exists(passport))
                            {
                                try // find <mf:doc_out Number="52-2" Date="2024-03-25"/>
                                {
                                    XmlDocument doc = new();
                                    doc.Load(passport);
                                    var root = doc.DocumentElement!;
                                    var pr = "n1"; // root.Prefix; // string.Empty
                                    var ns = "urn:cbr-ru"; // root.NamespaceURI;
                                    XmlNamespaceManager nsmgr = new(doc.NameTable);
                                    nsmgr.AddNamespace(pr, ns);
                                    var node = root.SelectSingleNode($"//OutNumber/RegNumber", nsmgr)
                                        ?? root.SelectSingleNode($"//document/RegNumer", nsmgr);
                                    string number = node!.InnerText;
                                    string date = node!.Attributes!["regdate"]!.Value;

                                    formtype = root.LocalName;
                                    datenum = $"{date}-{number}";
                                }
                                catch (Exception ex)
                                {
                                    Logger.LastError(ex);
                                    Logger.TimeLine($@"Файл ""passport.xml"" в '{msgId}' имеет ошибку: {ex.Message}");
                                }
                            }
                        }
                    }

                    string title = MakeTitle(message, formtype);
                    datenum ??= $"{message.CreationDate:yyyy-MM-dd}-{msgId[0..8]}";

                    datenum = string.Join('~', datenum.Split(Path.GetInvalidFileNameChars(),
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

                    string dir = Path.Combine(
                        Path.GetFullPath(DownloadPath),
                        message.Type,
                        message.TaskName,
                        $"{datenum[0..7]}");

                    string save = Path.Combine(dir,
                        $"{datenum} {title}");

                    Directory.CreateDirectory(dir);

                    if (Directory.Exists(save))
                        Directory.Delete(save, true);

                    Directory.Move(temp, save);
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

    private static string MakeTitle(Message message, string? formtype)
    {
        string title;

        if (formtype is null)
        {
            title = $"{message.Title} {message.Text} {message.RegNumber}";
        }
        else if (formtype.Equals("Form_Request", StringComparison.OrdinalIgnoreCase))
        {
            // Обращение (запрос)  в Банк России
            title = $"Запрос {message.Text}".Trim();
        }
        else if (formtype.Equals("Form_Response", StringComparison.OrdinalIgnoreCase))
        {
            // Ответ на запрос/предписание (требование)
            title = $"Ответ {message.Text}".Trim();
        }
        else if (formtype.Equals("passport", StringComparison.OrdinalIgnoreCase))
        {
            title = (message.Text ?? "Письмо").Trim();
        }
        else
        {
            title = $"{message.Title} {message.Text} {message.RegNumber}";
        }

        title = string.Join(' ', title.Split(' ',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

        title = string.Join('_', title.Split(Path.GetInvalidFileNameChars(),
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

        if (title.Length > 0)
        {
            //if (char.IsLower(title[0]))
            //{
            //    char[] a = title.ToCharArray();
            //    a[0] = char.ToUpper(a[0]);
            //    title = new string(a);
            //}

            if (title.Length > 64)
            {
                title = title[..64].Trim(); // + "_"; // "..."
            }
        }
        else
        {
            title = "Письмо";
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

        //if (file.Name.Equals("passport.xml", StringComparison.Ordinal))
        //    return true;

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
            Console.WriteLine("Неудача удаления сообщения: " + ex.Message);
        }
    }
}
