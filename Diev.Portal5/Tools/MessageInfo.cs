#region License
/*
Copyright 2022-2026 Dmitrii Evdokimov
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

using Diev.Portal5.API.Messages;

namespace Diev.Portal5.Tools;

/// <summary>
/// Информация о сообщении.
/// </summary>
public class MessageInfo
{
    /// <summary>
    /// Сообщение.
    /// </summary>
    public Message Message { get; }

    /// <summary>
    /// Дата сообщения.
    /// Если получена из CreatedDate, то сконвертирована в местное время.
    /// </summary>
    public string Date { get; }

    /// <summary>
    /// Номер сообщения.
    /// </summary>
    public string Number { get; }

    /// <summary>
    /// Тема сообщения.
    /// </summary>
    public string Subject { get; }

    /// <summary>
    /// Направление (inbox или outbox).
    /// </summary>
    public string Type => Message.Type;

    /// <summary>
    /// Название папки с распакованным сообщением.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Полный путь папки с распакованным сообщением.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Примечания, ошибки, исходный документ.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Сообщение.
    /// </summary>
    public Message? CorrMessage { get; }

    /// <summary>
    /// Формирует информацию о сообщении по файлу описания.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="path">Файл описания сообщения:<br/>
    /// passport.xml (inbox)<br/>
    /// form.xml (outbox)</param>
    /// <returns></returns>
    public MessageInfo(Message message, string? path = null)
    {
        Message = message;

        string? date = null;
        string? number = null;
        string title = $"{message.Title} {message.Text} {message.RegNumber}";

        if (path != null && File.Exists(path))
        {
            try
            {
                XmlDocument doc = new();
                doc.Load(path);
                var root = doc.DocumentElement!.LocalName;

                if (root.Equals("passport", StringComparison.Ordinal))
                {
                    // passport.xml // inbox
                    var node = doc.GetElementsByTagName("RegNumer")[0];
                    date = node?.Attributes?["regdate"]?.Value;
                    number = node?.InnerText;
                    title = doc.GetElementsByTagName("document")[0]?
                        .Attributes?["annotation"]?.Value ?? "Письмо";
                }
                else
                {
                    // form.xml // outbox
                    var node = doc.GetElementsByTagName("mf:doc_out")[0];
                    date = node?.Attributes?[nameof(Date)]?.Value;
                    number = node?.Attributes?[nameof(Number)]?.Value;
                    title = doc.GetElementsByTagName("mf:doc_text")[0]?.InnerText ??
                        (message.CorrelationId is null ? "Запрос" : "Ответ");
                }
            }
            catch { }
        }

        Date = date ?? message.CreationDate.ToLocalTime().ToString("yyyy-MM-dd");
        Number = number ?? message.RegNumber ?? message.Id[0..8];

        var so = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
        Subject = string.Join(' ', title.Split(' ', so));

        number = string.Join('-', Number.Split(' ', so));
        title = $"{Date}-{number} {Subject}";
        title = string.Join('-', title.Split(Path.GetInvalidFileNameChars(), so));

        Name = title.Length > 64 ? title[..64].Trim() : title;

        if (path is not null)
        {
            FullName = Path.Combine(Path.GetDirectoryName(path)!, Name);
        }
    }

    /// <summary>
    /// Краткая справка к сообщению.
    /// </summary>
    public override string ToString()
    {
        StringBuilder info = new();

        // Исходящий от 20.11.2025 N 218-2  //1e561dfa-3354-4211-adc5-b39b00ec830a

        info.Append(TType(Message.Type))
            .Append($" от {TDate(Date)}");

        if (Message.Registered && Message.UpdatedDate is not null)
        {
            if (Message.RegNumber is null || !Message.RegNumber.StartsWith(Number))
            {
                info.Append($" N {Number}");
            }

            info.AppendLine($"  //{Message.Id}  //{Message.TaskName}");

            // Зарегистрирован 20.11.2025 17:55 N 662460

            DateTime dt = ((DateTime)Message.UpdatedDate!).ToLocalTime();
            info.AppendLine($"Зарегистрирован {dt:dd.MM.yyyy HH:mm} N {Message.RegNumber}");
        }
        else
        {
            if (!Message.Id.StartsWith(Number))
            {
                info.Append($" N {Number}");
            }

            info.AppendLine($"  //{Message.Id}  //{Message.TaskName}");
        }

        // Titles
        info.AppendLine();

        // Ответ на запрос/предписание (требование)/перенаправленное обращение заявителя

        if (!string.IsNullOrEmpty(Message.Title))
            info.AppendLine(Message.Title);

        // О неиспользованном лимите кредитования

        if (!string.IsNullOrEmpty(Message.Text))
            info.AppendLine(Message.Text);

        // Content
        info.AppendLine()

            // Вложения в папке "2025-11-20-218-2 О неиспользованном лимите кредитования":

            .AppendLine(@$"Вложения в папке ""{Name}"":");

        List<string> list = [];

        foreach (var file in Message.Files)
        {
            if (file.SignedFile is not null)
                continue;

            if (Message.Inbox && file.Name.StartsWith("passport.xml", StringComparison.OrdinalIgnoreCase))
                continue;

            if (Message.Outbox && file.Name.StartsWith("form.xml", StringComparison.OrdinalIgnoreCase))
                continue;

            if (file.Name.Equals("info.txt", StringComparison.OrdinalIgnoreCase))
                continue;

            // - ВизуализацияЭД.PDF.enc - 100.12 KB

            list.Add($"{file.Name} - {FormatFileSize(file.Size)}");
        }

        list.Sort();

        foreach (var item in list)
        {
            info.AppendLine($"- {item}");
        }

        if (Notes is not null)
        {
            info.AppendLine()
                .AppendLine("--")
                .Append(Notes);
        }

        return info.ToString();
    }

    private static string TType(string? type) => type switch
    {
        "inbox" => "Входящий",
        "outbox" => "Исходящий",
        _ => "?",
    };

    private static string TDate(string? date) =>
        DateTime.TryParse(date, out DateTime dt)
            ? dt.ToString("dd.MM.yyyy")
            : "?";

    private static string TDate(DateTime? date) =>
        date is DateTime dt
            ? dt.ToString("dd.MM.yyyy")
            : "?";

    private static string FormatFileSize(long bytes)
    {
        double size = bytes;
        string[] units = ["B", "KB", "MB", "GB"];
        int i = 0;

        while (size >= 1024 && i < units.Length - 1)
        {
            size /= 1024;
            i++;
        }

        //return $"{size:n2} {units[i]}";
        return $"{size:n0} {units[i]}";
    }
}
