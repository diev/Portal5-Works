#region License
/*
Copyright 2022-2024 Dmitrii Evdokimov
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
    /// В ответ на сообщение, если есть.
    /// </summary>
    public string? CorrId { get; set; }

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
    /// Краткая справка к сообщению.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Название папки с распакованным сообщением.
    /// </summary>
    public string? PathName { get; set; }

    /// <summary>
    /// Формирует информацию о сообщении.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="outbox">Ящик исходящих или входящих.</param>
    /// <param name="path">Путь к файлам xml.</param>
    /// <returns></returns>
    public MessageInfo(Message message, bool outbox, string path)
    {
        Id = message.Id;
        CorrId = message.CorrelationId;

        string? date = null;
        string? number = null;
        string title = $"{message.Title} {message.Text} {message.RegNumber}";

        if (outbox)
        {
            if (File.Exists(path))
            {
                try
                {
                    XmlDocument doc = new();
                    doc.Load(path);

                    var node = doc.GetElementsByTagName("mf:doc_out")[0];
                    date = node?.Attributes?[nameof(Date)]?.Value;
                    number = node?.Attributes?[nameof(Number)]?.Value;
                    title = doc.GetElementsByTagName("mf:doc_text")[0]?.InnerText ??
                        (CorrId is null ? "Запрос" : "Ответ");
                }
                catch { }
            }
        }
        else // inbox
        {
            if (File.Exists(path))
            {
                try
                {
                    XmlDocument doc = new();
                    doc.Load(path);

                    var node = doc.GetElementsByTagName("RegNumer")[0];
                    date = node?.Attributes?["regdate"]?.Value;
                    number = node?.InnerText;
                    title = doc.GetElementsByTagName("document")[0]?
                        .Attributes?["annotation"]?.Value ?? "Письмо";
                }
                catch { }
            }
        }

        var so = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

        Date = date ?? message.CreationDate.ToString("yyyy-MM-dd");
        Number = number ?? message.RegNumber ?? Id[0..8];
        Subject = string.Join(' ', title.Split(' ', so));

        number = string.Join('-', Number.Split(' ', so));
        title = $"{Date}-{number} {Subject}";
        title = string.Join('-', title.Split(Path.GetInvalidFileNameChars(), so));

        PathName = title.Length > 64 ? title[..64].Trim() : title;

        StringBuilder info = new();
        info
            .Append("Исх. : ").AppendLine($"{Date} N {Number}")
            .Append("Тема : ").AppendLine(Subject)
            .AppendLine()
            .Append("Ид   : ").AppendLine(Id)
            .Append("На   : ").AppendLine(CorrId)
            .Append("Папка: ").AppendLine(PathName);

        Description = info.ToString();
    }

    /// <summary>
    /// Формирует информацию о неполученном сообщении.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="error">Причина неполучения.</param>
    public MessageInfo(Message message, string error)
    {
        Id = message.Id;
        CorrId = message.CorrelationId;

        string? date = null;
        string? number = null;
        string title = $"{message.Title} {message.Text} {message.RegNumber}";

        var so = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

        Date = date ?? message.CreationDate.ToString("yyyy-MM-dd");
        Number = number ?? message.RegNumber ?? Id[0..8];
        Subject = string.Join(' ', title.Split(' ', so));

        StringBuilder info = new();
        info
            .Append("Исх. : ").AppendLine($"{Date} N {Number}")
            .Append("Тема : ").AppendLine(Subject)
            .AppendLine()
            .Append("Ид   : ").AppendLine(Id)
            .Append("На   : ").AppendLine(CorrId)
            .Append("Ошибка: ").AppendLine(error);

        Description = info.ToString();
    }
}
