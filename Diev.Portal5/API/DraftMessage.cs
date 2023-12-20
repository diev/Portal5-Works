#region License
/*
Copyright 2022-2023 Dmitrii Evdokimov
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

namespace Diev.Portal5.API;

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
    /// Example: "Zadacha_3-1"
    /// Example: "Zadacha_130"
    /// Example: "Zadacha_137"
    /// </summary>
    public string Task { get; set; } = null!;

    /// <summary>
    /// Название сообщения (subject).
    /// Example: null
    /// Example: "N 20-2-1/1 от 10/01/2023 (20) Письма Деп-та денежно-кредитной политики"
    /// Example: "Ответ на запрос/предписание (требование)"
    /// Example: "Получение информации об уровне риска ЮЛ/ИП"
    /// Example: "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)"
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Текст сообщения (body).
    /// Example: null
    /// Example: ""
    /// Example: "предоставление запрошенной информации"
    /// Example: "О данных для расчета размера обязательных резервов"
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Регистрационный номер.
    /// Example: null
    /// Example: "20-2-1/1"
    /// </summary>
    public string? RegNumber { get; set; }

    /// <summary>
    /// Получатели сообщения (необязательно, указывается для потоков адресной рассылки).
    /// Example: null
    /// Example: [{
    /// "Inn": "7831001422",
    /// "Ogrn": "1027800000095",
    /// "Bik": "044030702",
    /// "RegNum": "3194",
    /// "DivisionCode": "0000"
    /// }, ...]
    /// </summary>
    public List<Sender>? Receivers { get; set; }

    /// <summary>
    /// Файлы включенные в сообщение.
    /// Example: [{
    /// "Id":"d55cdbbb-e41f-4a2a-8967-78e2a6e15701",
    /// "Name":"KYC_20230925.xml.zip.enc",
    /// "Encrypted":true,
    /// "SignedFile":null,
    /// "Size":3238155,
    /// }, ...]
    /// </summary>
    public List<DraftMessageFile> Files { get; set; } = null!;
}
