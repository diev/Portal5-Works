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

namespace Diev.Extensions.Xml;

public static class XsdChecker
{
    /// <summary>
    /// Проверка файла XML по схеме XSD.
    /// </summary>
    /// <param name="xml">Файл XML.</param>
    /// <param name="xsd">Файл XSD.</param>
    /// <returns>Ничего не возвращает, если успешно, или вызывает исключение.</returns>
    /// <exception cref="FileNotFoundException"></exception>
    public static async Task CheckAsync(string xml, string xsd)
    {
        XmlReaderSettings xmlReaderSettings = new()
        {
            Async = true
        };
        xmlReaderSettings.Schemas.Add(null, xsd);
        xmlReaderSettings.ValidationType = ValidationType.Schema;
        xmlReaderSettings.ValidationEventHandler += XmlReaderSettings_ValidationEventHandler;

        using var form = XmlReader.Create(xml, xmlReaderSettings);
        while (await form.ReadAsync()) { }
    }

    /// <summary>
    /// Обработчик ошибок проверки схемы XSD.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="Exception"></exception>
    private static void XmlReaderSettings_ValidationEventHandler(object? sender, System.Xml.Schema.ValidationEventArgs e)
    {
        if (e.Severity == System.Xml.Schema.XmlSeverityType.Warning)
        {
            throw new Exception("Warning: " + e.Message);
        }
        else if (e.Severity == System.Xml.Schema.XmlSeverityType.Error)
        {
            throw new Exception(e.Message);
        }
    }
}
