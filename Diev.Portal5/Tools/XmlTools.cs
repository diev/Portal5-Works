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

using System.Xml;
using System.Xml.Serialization;

using Diev.Portal5.API.Forms;
using Diev.Portal5.API.Messages;

namespace Diev.Portal5.Tools;

public static class XmlTools
{
    /// <summary>
    /// Читает из указанного файла "form.xml" FormRequest.
    /// </summary>
    /// <param name="path">Путь файла "form.xml".</param>
    /// <returns>FormRequest</returns>
    public static FormRequest? GetFormRequest(string path)
    {
        var serializer = new XmlSerializer(typeof(FormRequest));
        using var reader = new FileStream(path, FileMode.Open);
        var form = (FormRequest?)serializer.Deserialize(reader);
        return form;
    }

    /// <summary>
    /// Читает из указанного файла "form.xml" FormResponse.
    /// </summary>
    /// <param name="path">Путь файла "form.xml".</param>
    /// <returns>FormResponse</returns>
    public static FormResponse? GetFormResponse(string path)
    {
        var serializer = new XmlSerializer(typeof(FormResponse));
        using var reader = new FileStream(path, FileMode.Open);
        var form = (FormResponse?)serializer.Deserialize(reader);
        return form;
    }

    /// <summary>
    /// Читает из указанного файла "form.xml" идентификатор в виде yyyy-MM-dd-Number.
    /// </summary>
    /// <param name="path">Путь файла "form.xml".</param>
    /// <returns>Идентификатор в виде yyyy-MM-dd-Number.</returns>
    public static string GetFormId(string path)
    {
        // if (message.Type.Equals(MessageType.Outbox) && File.Exists("form.xml"))
        // parse <mf:doc_out Number="44-3-1" Date="2024-03-13"/>
        // => "2024-03-13-44-3-1"

        XmlDocument doc = new();
        doc.Load(path);

        /*
        var root = doc.DocumentElement!;
        var formtype = root.LocalName;
        var pr = root.Prefix;
        var ns = root.NamespaceURI;
        XmlNamespaceManager nsmgr = new(doc.NameTable);
        nsmgr.AddNamespace(pr, ns);
        var node = root.SelectSingleNode($"//{pr}:doc_out", nsmgr);
        */

        var node = doc.GetElementsByTagName("mf:doc_out")[0];
        var number = node?.Attributes?["Number"]?.Value;
        var date = node?.Attributes?["Date"]?.Value;

        return date + '-' + number;
    }

    /// <summary>
    /// Читает из указанного файла "passport.xml" идентификатор в виде yyyy-MM-dd-Number.
    /// </summary>
    /// <param name="path">Путь файла "passport.xml".</param>
    /// <returns>Идентификатор в виде yyyy-MM-dd-Number.</returns>
    public static string GetPassportId(string path)
    {
        XmlDocument doc = new();
        doc.Load(path);

        /*
        var root = doc.DocumentElement!;
        var pr = "n1"; // root.Prefix; // string.Empty
        var ns = "urn:cbr-ru"; // root.NamespaceURI;
        XmlNamespaceManager nsmgr = new(doc.NameTable);
        nsmgr.AddNamespace(pr, ns);
        var node = root.SelectSingleNode($"//OutNumber/RegNumber", nsmgr)
            ?? root.SelectSingleNode($"//document/RegNumer", nsmgr);
        */

        var node = doc.GetElementsByTagName("RegNumer")[0];
        var number = node?.InnerText;
        var date = node?.Attributes?["regdate"]?.Value;

        return date + '-' + number;
    }

    /// <summary>
    /// Создает имя папки для хранения сообщения.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="path">Путь к файлам xml.</param>
    /// <returns></returns>
    public static string GetMessageTitle(Message message, string path)
    {
        string? date = null;
        string? number = null;
        string? subj = null;

        if (message.Type.Equals(MessageType.Outbox, StringComparison.OrdinalIgnoreCase))
        {
            string form = Path.Combine(path, "form.xml");

            if (File.Exists(form))
            {
                XmlDocument doc = new();
                doc.Load(form);

                var node = doc.GetElementsByTagName("mf:doc_out")[0];
                date = node?.Attributes?["Date"]?.Value;
                number = node?.Attributes?["Number"]?.Value;

                // <Form_Request>: "Обращение (запрос)  в Банк России"
                // <Form_Response>: "Ответ на запрос/предписание (требование)"
                subj = $"{(message.CorrelationId is null ? "Запрос" : "Ответ")} {message.Text}";
            }
        }
        else // if (message.Type.Equals(MessageType.Inbox, StringComparison.Ordinal))
        {
            string passport = Path.Combine(path, "passport.xml");

            if (File.Exists(passport))
            {
                XmlDocument doc = new();
                doc.Load(passport);

                var node = doc.GetElementsByTagName("RegNumer")[0];
                date = node?.Attributes?["regdate"]?.Value;
                number = node?.InnerText;

                subj = message.Text;
            }
        }

        date ??= $"{message.CreationDate:yyyy-MM-dd}";
        number ??= message.Id[0..8];
        subj ??= $"{message.Title} {message.Text} {message.RegNumber}" ?? "Письмо";

        string title = $"{date}-{number} {subj}";
 
        title = string.Join(' ', title.Split(' ',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

        title = string.Join('-', title.Split(Path.GetInvalidFileNameChars(),
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

        if (title.Length > 64)
        {
            title = title[..64].Trim();
        }

        return title;
    }
}
