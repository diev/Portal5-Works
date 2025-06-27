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

namespace Diev.Portal5.API.Info;

/// <summary>
/// Получатель сообщения (необязательно, указывается для потоков адресной рассылки).
/// </summary>
public record Receiver
(
    /// <summary>
    /// ИНН получателя сообщения.
    /// Example: "7831001422"
    /// </summary>
    string? Inn,

    /// <summary>
    /// ОГРН получателя сообщения.
    /// Example: "1027800000095"
    /// </summary>
    string? Ogrn,

    /// <summary>
    /// БИК получателя сообщения.
    /// Example: "044030702"
    /// </summary>
    string? Bik,

    /// <summary>
    /// Адрес электронной почты получателя сообщения.
    /// </summary>
    string? Email,

    /// <summary>
    /// Регистрационный номер КО - получателя сообщения по КГРКО.
    /// Example: "3194"
    /// </summary>
    string? RegNum,

    /// <summary>
    /// Номер филиала КО - получателя сообщения по КГРКО.
    /// Example: "0000"
    /// </summary>
    string? DivisionCode,

    /// <summary>
    /// Краткое наименование вида деятельности получателя.
    /// </summary>
    string? Activity
);

public static class MockReceiver
{
    public static string Text() =>
        """
        {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        }
        """;
}
