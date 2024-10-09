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

public class MessageInfo
{
    public string Id { get; set; }
    public string? CorrId { get; set; }
    public string Date { get; set; }
    public string Number { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public string PathName { get; set; }

    /// <summary>
    /// Формирует информацию о сообщении.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="path">Путь к файлам xml.</param>
    /// <returns></returns>
    public MessageInfo(Message message, string path)
    {
        Id = message.Id;
        CorrId = message.CorrelationId;

        string? date = null;
        string? number = null;
        string text;

        if (message.Type.Equals("outbox", StringComparison.OrdinalIgnoreCase))
        {
            text = CorrId is null ? "Запрос " : "Ответ ";
            string form = Path.Combine(path, "form.xml");

            if (File.Exists(form))
            {
                try
                {
                    XmlDocument doc = new();
                    doc.Load(form);

                    var node = doc.GetElementsByTagName("mf:doc_out")[0];
                    date = node?.Attributes?[nameof(Date)]?.Value;
                    number = node?.Attributes?[nameof(Number)]?.Value;
                    text += doc.GetElementsByTagName("mf:doc_text")[0]?.InnerText;
                }
                catch
                {
                    text = $"{message.Title} {message.Text} {message.RegNumber}";
                }
            }
            else
            {
                text += $"{message.Title} {message.Text} {message.RegNumber}";
            }
        }
        else // if (message.Type.Equals("inbox", StringComparison.OrdinalIgnoreCase))
        {
            string passport = Path.Combine(path, "passport.xml");

            if (File.Exists(passport))
            {
                try
                {
                    XmlDocument doc = new();
                    doc.Load(passport);

                    var node = doc.GetElementsByTagName("RegNumer")[0];
                    date = node?.Attributes?["regdate"]?.Value;
                    number = node?.InnerText;
                    text = doc.GetElementsByTagName("document")[0]?
                        .Attributes?["annotation"]?.Value ?? string.Empty;
                }
                catch
                {
                    text = $"{message.Title} {message.Text} {message.RegNumber}";
                }
            }
            else
            {
                text = $"{message.Title} {message.Text} {message.RegNumber}";
            }
        }

        var so = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

        Date = date ?? message.CreationDate.ToString("yyyy-MM-dd");
        Number = number ?? message.RegNumber ?? Id[0..8];
        Subject = string.Join(' ', text.Split(' ', so));

        number = string.Join('-', Number.Split(' ', so));
        text = $"{Date}-{number} {Subject}";
        text = string.Join('-', text.Split(Path.GetInvalidFileNameChars(), so));

        PathName = text.Length > 64 ? text[..64].Trim() : text;

        StringBuilder info = new();
        info.AppendLine($"{Date} N {Number}");
        info.AppendLine(Subject).AppendLine();
        info.AppendLine(CorrId is null ? Id : $"{Id} на {CorrId}");
        info.AppendLine(PathName);
        Description = info.ToString();
    }
}
