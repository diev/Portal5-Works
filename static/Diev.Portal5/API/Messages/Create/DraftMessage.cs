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

using Diev.Portal5.API.Info;

namespace Diev.Portal5.API.Messages.Create;

/// <summary>
/// Черновик сообщения.
/// Headers:
/// Content-Type: application/json; charset=utf-8
/// </summary>
public class DraftMessage
{
    /// <summary>
    /// Наименование задачи в черновике сообщения (потом меняется на TaskName).
    /// Example: "Zadacha_2-1"
    /// Example: "Zadacha_137"
    /// </summary>
    public string Task { get; set; } = null!;

    /// <summary>
    /// Идентификатор корреляции сообщения
    /// (необязательно, указывается для формирования ответного сообщения для потоков,
    /// поддерживаемых данную функциональность).
    /// Example: null
    /// Example: "1f6158a2-a7a1-4e14-aace-af7a00f65145"
    /// </summary>
    public string? CorrelationId { get; set; }

    /// <summary>
    /// Идентификатор группы сообщений
    /// (необязательно, указывается для формирования ответного сообщения для потоков,
    /// поддерживаемых данную функциональность).
    /// Example: null
    /// Example: "a4e5902c-e961-47a3-9670-bd717bcc1749"
    /// </summary>
    public string? GroupId { get; set; }

    /// <summary>
    /// Название сообщения, отображается в интерфейсе (subject).
    /// Example: null
    /// Example: "Ответ на запрос/предписание (требование)"
    /// Example: "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)"
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Текст сообщения, отображается в интерфейсе (body).
    /// Example: null
    /// Example: ""
    /// Example: "предоставление запрошенной информации"
    /// Example: "О данных для расчета размера обязательных резервов"
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Файлы включенные в сообщение.
    /// Example: [{
    /// "Name":"KYC_20230925.xml.zip.enc",
    /// "Encrypted":true,
    /// "SignedFile":null,
    /// "Size":3238155,
    /// }, ...]
    /// </summary>
    public List<DraftMessageFile> Files { get; set; } = [];

    /// <summary>
    /// Получатели сообщения (необязательно, указывается для потоков адресной рассылки).
    /// Example: null
    /// Example: [{
    /// "Inn": "7831001422",
    /// "Ogrn": "1027800000095",
    /// "Bik": "044030702",
    /// "RegNum": "3194",
    /// "DivisionCode": "0000",
    /// "Activity": ""
    /// }, ...]
    /// </summary>
    public List<Receiver>? Receivers { get; set; }
}
