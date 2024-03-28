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
/// Типизированный файл, описывающий адресата получателя и общую информацию
/// о сообщении. В случае, если сообщение является ДСП (для служебного
/// пользования/содержит ИОД и ПД), то файл должны быть зашифрован на
/// открытый ключ внешнего абонента и ЕПВВ.
/// </summary>
public class FormRequest
{
    /// <summary>
    /// Тип сообщения
    /// (Вид обращения)
    /// </summary>
    public string MessageType { get; set; } = "Обращение (запрос)";

    /// <summary>
    /// Адресат: первого уровня
    /// из Справочника адресатов 1-го уровня
    /// (1-уровень. Тип участника информационного обмена)
    /// </summary>
    public string SubjectLevel1 { get; set; } = null!;

    /// <summary>
    /// Адресат: второго уровня
    /// из Справочника адресатов 2-го уровня
    /// (2-уровень. Тематический вопрос)
    /// </summary>
    public string SubjectLevel2 { get; set; } = null!;

    /// <summary>
    /// Адресат: третьего уровня
    /// из Справочника адресатов 3-го уровня
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
    /// из Справочника адресатов 2-го или 3-го уровня
    /// (в зависимости от того, что является конечным выбором)
    /// </summary>
    public string? Folder { get; set; }

    /// <summary>
    /// Ограничение доступа к пакету документов:
    /// - Шифрование пакета (1 - ДСП);
    /// - Стандартная передача пакета (0 - не ДСП)
    /// </summary>
    public bool DocFlag { get; set; } = false; // Encrypted = 1

    #region опционально
    /// <summary>
    /// Рубрика
    /// </summary>
    public string? DocRubric { get; set; }

    /// <summary>
    /// Рубрика: Название рубрики
    /// </summary>
    public string? RubricName { get; set; }

    /// <summary>
    /// Рубрика: Код рубрики
    /// </summary>
    public string? DocRubricCode { get; set; }
    #endregion опционально

    /// <summary>
    /// Сопроводительное письмо.
    /// Любой текст, который хочет написать внешний абонент.
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
    /// Подписант: Замещение.
    /// Признак замещения:
    /// «Нет», если Подписант не является заместителем.
    /// «Да», если Подписант является заместителем.
    /// В этом случае одним из прикладываемых файлов должен быть приказ
    /// об исполнении обязанностей.
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

    #region опционально
    /// <summary>
    /// Том архива приложения к электронному документу: Номер тома
    /// </summary>
    public int? DocOutVolumeNumber { get; set; }

    /// <summary>
    /// Том архива приложения к электронному документу: Всего томов
    /// </summary>
    public int? DocOutVolumeAll { get; set; }
    #endregion опционально
}

/*
<?xml version="1.0" encoding="UTF-8"?>
<mf:Form_Request
xmlns:mfb="urn:cbr-ru:e-forms-mf-sadd-base:v0.1"
xmlns:mf="urn:cbr-ru:e-forms-mf-sadd-request:v0.1"
xmlns:xs="http://www.w3.org/2001/XMLSchema"
xmlns:ai="urn:cbr-ru:e-forms-app-info:v1.0"
xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
xsi:schemaLocation="urn:cbr-ru:e-forms-mf-sadd-request:v0.1 main.xsd"
message_type="Обращение (запрос)">
	<mf:subject level_1="Территориальное учреждение"
                level_2="Волго-Вятское ГУ Банка России"
                Addressee="Отделение по Ульяновской обл. Волго-Вятского ГУ Банка России"/>
	<mf:organization>Отделение по Ульяновской обл. Волго-Вятского ГУ Банка России</mf:organization>
	<mf:department>73_АДМИНИСТРАТИВНЫЙ ОТДЕЛ</mf:department>
	<mf:folder>22_lk</mf:folder>
	<mf:doc_flag>0</mf:doc_flag>
	<mf:doc_text>test</mf:doc_text>
	<mf:org_official name="test" post="test" replace="Нет"/>
	<mf:doc_writer name="test" post="test" phone="1234"/>
	<mf:doc_out Number="test" Date="2018-08-20"/>
</mf:Form_Request>
*/
