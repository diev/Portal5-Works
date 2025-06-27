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

using System.Text;
using System.Xml;

using Diev.Portal5.API.Messages;

namespace Diev.Portal5;

/// <summary>
/// Информация о сообщении.
/// </summary>
public class MessageInfo
{
    /// <summary>
    /// Идентификатор сообшения.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Дата сообщения.
    /// </summary>
    public string Date { get; set; }
    
    /// <summary>
    /// Номер сообщения.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Тема сообщения.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Направление.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Название папки с распакованным сообщением.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Полный путь папки с распакованным сообщением.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// В ответ на сообщение, если есть.
    /// </summary>
    public string? CorrId { get; set; }

    /// <summary>
    /// Дата сообщения.
    /// </summary>
    public string? CorrDate { get; set; }

    /// <summary>
    /// Номер сообщения.
    /// </summary>
    public string? CorrNumber { get; set; }

    /// <summary>
    /// Тема сообщения.
    /// </summary>
    public string? CorrSubject { get; set; }

    /// <summary>
    /// Направление.
    /// </summary>
    public string? CorrType { get; set; }

    /// <summary>
    /// Название папки с распакованным сообщением.
    /// </summary>
    public string? CorrName { get; set; }

    /// <summary>
    /// Полный путь папки с распакованным сообщением.
    /// </summary>
    public string? CorrFullName { get; set; }

    /// <summary>
    /// Формирует информацию о сообщении.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <returns></returns>
    public MessageInfo(Message message)
    {
        var so = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

        Id = message.Id;
        CorrId = message.CorrelationId;
        Type = message.Type;

        string? date = null;
        string? number = null;
        string title = $"{message.Title} {message.Text} {message.RegNumber}";

        Date = date ?? message.CreationDate.ToString("yyyy-MM-dd");
        Number = number ?? message.RegNumber ?? Id[0..8];
        Subject = string.Join(' ', title.Split(' ', so));
    }

    /// <summary>
    /// Формирует информацию о сообщении по файлу описания.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="path">Файл описания сообщения:<br/>
    /// passport.xml (inbox)<br/>
    /// form.xml (outbox)</param>
    /// <returns></returns>
    public MessageInfo(Message message, string path)
    {
        var so = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

        Id = message.Id;
        CorrId = message.CorrelationId;
        Type = message.Type;

        string? date = null;
        string? number = null;
        string title = $"{message.Title} {message.Text} {message.RegNumber}";

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
                    (CorrId is null ? "Запрос" : "Ответ");
            }
        }
        catch { }

        Date = date ?? message.CreationDate.ToString("yyyy-MM-dd");
        Number = number ?? message.RegNumber ?? Id[0..8];
        Subject = string.Join(' ', title.Split(' ', so));

        number = string.Join('-', Number.Split(' ', so));
        title = $"{Date}-{number} {Subject}";
        title = string.Join('-', title.Split(Path.GetInvalidFileNameChars(), so));

        Name = title.Length > 64 ? title[..64].Trim() : title;
        FullName = Path.Combine(Path.GetDirectoryName(path)!, Name);
    }

    /// <summary>
    /// Краткая справка к сообщению.
    /// </summary>
    public override string ToString()
    {
        StringBuilder info = new();
        info.AppendLine($"{TType(Type)} от {TDate(Date)} N {Number}")
            .AppendLine(Subject)
            .AppendLine("--")
            .AppendLine(Id)
            .AppendLine(Name);

        if (!string.IsNullOrEmpty(CorrId))
        {
            info.AppendLine()
                .AppendLine($"на {TType(CorrType)} от {TDate(CorrDate)} N {CorrNumber}")
                .AppendLine(CorrSubject)
                .AppendLine("--")
                .AppendLine(CorrId)
                .AppendLine(CorrName);
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
        date is null
            ? "?"
            : DateTime.Parse(date).ToString("dd.MM.yyyy");
}
