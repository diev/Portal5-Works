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

namespace Diev.Portal5.API.Forms;

/// <summary>
/// Сопроводительная форма XML к пересылаемым сообщениям.
/// </summary>
public class FormResponse
{
    /// <summary>
    /// Тип сообщения
    /// (Вид ответа)
    /// </summary>
    public string MessageType { get; set; } = "Ответ на предписание (требование) Банка России";

    /// <summary>
    /// Адресат: первого уровня
    /// (1-уровень. Тип участника информационного обмена)
    /// </summary>
    public string SubjectLevel1 { get; set; } = null!;

    /// <summary>
    /// Адресат: второго уровня
    /// (2-уровень. Тематический вопрос)
    /// </summary>
    public string SubjectLevel2 { get; set; } = null!;

    /// <summary>
    /// Адресат: третьего уровня
    /// (Адресат по запросам от КО)
    /// </summary>
    public string? SubjectAddresse { get; set; }

    /// <summary>
    /// Организация
    /// </summary>
    public string Organization { get; set; }

    /// <summary>
    /// Структурное подразделение Банка России
    /// </summary>
    public string Departament { get; set; }

    /// <summary>
    /// Указатель на папку СДС
    /// </summary>
    public string? Folder { get; set; }

    /// <summary>
    /// Ограничение доступа к пакету документов:
    /// - Шифрование пакета;
    /// - Стандартная передача пакета
    /// </summary>
    public bool DocFlag { get; set; } = false; // Encrypted = 1

    /// <summary>
    /// Сопроводительное письмо
    /// </summary>
    public string DocText { get; set; } = "Предоставление информации";

    /// <summary>
    /// Подписант: ФИО
    /// </summary>
    public string OrgOfficialName { get; set; } = null!;

    /// <summary>
    /// Подписант: Должность
    /// </summary>
    public string OrgOfficialPost { get; set; } = "Председатель Правления";

    /// <summary>
    /// Подписант: Замещение
    /// </summary>
    public bool OrgOfficialReplace { get; set; } = false;

    /// <summary>
    /// Исполнитель: ФИО
    /// </summary>
    public string DocWriterName { get; set; } = null!;

    /// <summary>
    /// Исполнитель: Должность
    /// </summary>
    public string DocWriterPost { get; set; } = null!;

    /// <summary>
    /// Исполнитель: Контактный номер телефона
    /// </summary>
    public string DocWriterPhone { get; set; } = "8 812";

    /// <summary>
    /// Исходящий документ: Номер
    /// </summary>
    public string DocOutNumber { get; set; } = null!;

    /// <summary>
    /// Исходящий документ: Дата
    /// </summary>
    public DateTime DocOutDate { get; set; }

    /// <summary>
    /// Отчётность, направленная в соответствии с запросом/предписанием:
    /// Входящий номер, присвоенный пакету отчётности
    /// </summary>
    public string ReportingNumber { get; set; } = null!;

    /// <summary>
    /// Отчётность, направленная в соответствии с запросом/предписанием:
    /// Дата отправки пакета
    /// </summary>
    public DateTime ReportingDate { get; set; }
}

/*
<?xml version="1.0" encoding="UTF-8"?>
<mf:Form_Response
xmlns:xs="http://www.w3.org/2001/XMLSchema"
xmlns:mf="urn:cbr-ru:e-forms-mf-sadd-response:v0.4"
xmlns:mfb="urn:cbr-ru:e-forms-mf-sadd-base:v0.1"
xmlns:ai="urn:cbr-ru:e-forms-app-info:v1.0"
xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
xsi:schemaLocation="urn:cbr-ru:e-forms-mf-sadd-response:v0.4 main.xsd"
message_type="Ответ на предписание (требование) Банка России">
	<mf:subject level_1="Банк России"
                level_2="Банк России"
                Addressee="Выбор не требуется"
                organization="Банк России"
                department="Административный департамент"
                folder="48_lk"/>
	<mf:subject level_1="Банк России"
                level_2="Департамент управления данными"
                Addressee="Выбор не требуется"
                organization="Банк России"
                department="Административный департамент"
                folder="48_lk"/>
	<mf:subject level_1="Центральный аппарат"
                level_2="Тестовое сообщение"
                Addressee="Выбор не требуется"
                organization="Банк России"
                department="Тестовое сообщение"
                folder="48_lk"/>
	<mf:doc_flag>0</mf:doc_flag>
	<mf:doc_text>Сопроводительное письмо</mf:doc_text>
	<mf:org_official name="Подписант - ФИО" post="Подписант – Должность" replace="Нет"/>
	<mf:doc_writer name="Исполнитель - ФИО" post="Исполнитель - Должность" phone="1111111"/>
	<mf:doc_out Number="123" Date="2019-01-31"/>
    <mf:reporting Number="456" Date="2019-01-31"/>
</mf:Form_Response>
*/
