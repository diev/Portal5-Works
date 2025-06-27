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
/// 3.1.6.1. Справочник задач.<br/>
/// GET https://portal5.cbr.ru/back/rapi2/tasks<br/>
/// 200 OK
/// </summary>
public record Tasks
(
    /// <summary>
    /// Код задачи (по справочнику задач в формате "Zadacha_*",
    /// где Zadacha_ - неизменная часть,
    /// * - число/набор символов определяющий порядковый номер/обозначение задачи),
    /// используется для идентификации задачи.
    /// Example:
    /// </summary>
    string Code,

    /// <summary>
    /// Наименование задачи.
    /// Example:
    /// </summary>
    string Name,

    /// <summary>
    /// Направление обмена.
    /// Может принимать значения:
    /// - inbox - входящее в ЛК;
    /// - outbox - исходящее из ЛК;
    /// - bidirectional - двунаправленное между ЛК.
    /// </summary>
    string? Direction,

    /// <summary>
    /// Признак возможности отправки связанных сообщений.
    /// </summary>
    bool? AllowLinkedMessages,

    /// <summary>
    /// Признак возможности отправки сообщений через Aspera.
    /// </summary>
    bool? AllowAspera,

    /// <summary>
    /// Текстовое описание задачи, может быть не заполнено.
    /// Example:
    /// </summary>
    string? Description
);

public static class MockTasks
{
    /// <summary>
    /// tasks
    /// </summary>
    /// <returns></returns>
    public static string Text() =>
        """
        [
            {
                "Code": "Zadacha_29",
                "Name": "Отчетность субъектов рынка ценных бумаг и товарного рынка",
                "Description": null,
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_55",
                "Name": "Ответ на запрос ЦИК",
                "Description": "Ответ #FEATURE:Sender@FullName#",
                "AllowAspera": false,
                "Direction": "bidirectional",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_58",
                "Name": "Прикладная квитанция УФР в ответ на запрос ЦИК",
                "Description": "Квитанция #FEATURE:Sender@FullName#",
                "AllowAspera": false,
                "Direction": "bidirectional",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_1-31",
                "Name": "Активы КО",
                "Description": null,
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_204",
                "Name": "Получение ЭОП/Уведомлений",
                "Description": "",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "GroupTask_22",
                "Name": "Федеральные статистические наблюдения, утвержденные Указанием Банка России от 16.02.2023 № 6363-У",
                "Description": "старое наименование Федеральные статистические наблюдения, утвержденные Указанием Банка России от 25.11.2019 № 5328-У",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_161",
                "Name": "Получение ЭОП",
                "Description": "В рамках задачи направляется ЭС с ЭОП из Банка России в адрес УИО",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_162",
                "Name": "Уведомление о результате контроля ЭОП",
                "Description": "В рамках задачи направляется ответное Уведомление от УИО в Банк России на ЭС с ЭОП ",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_94",
                "Name": "Расчёт и регулирование размера обязательных резервов",
                "Description": null,
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_1-33",
                "Name": "Массовая рассылка в соответствии со 115-ФЗ (639-П).",
                "Description": null,
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_1-33-2",
                "Name": "Массовая рассылка в соответствии со 115-ФЗ (639-П) Ответ",
                "Description": null,
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_515",
                "Name": "Информация о результатах приема/обработки ЭС",
                "Description": "Передача из ПС ОД ОПЕРУ-1 ЭС ED263 в ЛК КО с выполнением контролей",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_48",
                "Name": "Запрос Анкетирование",
                "Description": "Взаимообмен по потоку Анкетирование. Отправка шаблонов анкет из БР.",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_78",
                "Name": "Извещения и уведомления по депозитным и кредитным операциям",
                "Description": null,
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_3-1",
                "Name": "Запрос, предписание. Ответ на запрос НФО. Квитанции из САДД",
                "Description": null,
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_63",
                "Name": "Ответ по потоку Агентства страхования вкладов (АСВ)",
                "Description": "",
                "AllowAspera": false,
                "Direction": "bidirectional",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_51",
                "Name": "Нева. Поток по инициативе НЕВЫ",
                "Description": null,
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_113",
                "Name": "Представление формы 0409310",
                "Description": "Поток представления отчетности по форме 0409310 в ППК МПСО (ПП Дельта ИЭС1-114, ИЭС2-123)",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_120",
                "Name": "Отправить XBRL-CSV",
                "Description": "Передача отчетности в формате XBRL-CSV",
                "AllowAspera": true,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_260",
                "Name": "Отчетность 0409701",
                "Description": "Представление отчетности КО по форме 0409701 в Банк России",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_86",
                "Name": "Сообщение из системы Банка России. Ответ на сообщение УИО.",
                "Description": null,
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_179",
                "Name": "Направление информации в реестр эмиссионных ценных бумаг",
                "Description": "Передача форм в РУФР",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_77",
                "Name": "Процедуры допуска",
                "Description": "Блок 625-П",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_147",
                "Name": "Представление отчетов, указанных в ч. 7 ст. 12 Закона № 173-ФЗ",
                "Description": "Поток представления отчетности о движении денежных средств на ин. счетах",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_100",
                "Name": "Выгрузка данных из ППК КГР в ЛК",
                "Description": "В рамках данной задачи направляются данные из ЦБ в ГБО ФНС России",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_76",
                "Name": "Предоставление информации о договорах займа",
                "Description": null,
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_85",
                "Name": "Унифицированный обмен",
                "Description": null,
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_140",
                "Name": "Извещение о результатах контроля информации о ВПОДК и их результатах (ИЭС2)",
                "Description": "Ответ на предоставление информации о результатах контроля информации о ВПОДК и их результатах (ИЭС2)",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_1-32",
                "Name": "Получение отчетности из Клиентского ПО через SOAP-сервис",
                "Description": null,
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_97",
                "Name": "Извещение о результатах контроля информации о ВПОДК и их результатах (ИЭС1)",
                "Description": "Ответ на предоставление информации о результатах контроля информации о ВПОДК и их результатах (ИЭС1)",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_74",
                "Name": "Представление информации о ВПОДК и их результатах",
                "Description": "Представление информации о ВПОДК и их результатах",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_123",
                "Name": "Извещение о результатах контроля представления формы 0409310 (ИЭС2)",
                "Description": "Извещение о результатах контроля представления формы 0409310 (ИЭС2) (ПП Дельта 113, ИЭС1-114)",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_106",
                "Name": "Представление заявлений Операторами платежных систем",
                "Description": "В рамках данной задачи направляются завления Операторами платежных систем",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_157",
                "Name": "Представление отчетности банковских холдингов в Банк России",
                "Description": "(ПП Дельта ИЭС1-158, ИЭС2-160)",
                "AllowAspera": true,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_155",
                "Name": "Представление отчетности КО в Банк России",
                "Description": "(ПП Дельта ИЭС1-156, ИЭС2-159)",
                "AllowAspera": true,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_104",
                "Name": "Представление отчетности субъектов НПС в Банк России",
                "Description": "Представление отчетности и иной информации субъектов национальной платежной системы, не являющихся кредитными организациями, в Банк России (ПП Дельта ИЭС1-107, ИЭС2-133)",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_114",
                "Name": "Извещение о результатах контроля представления формы 0409310 (ИЭС1)",
                "Description": "Извещение о результатах контроля представления формы 0409310 (ИЭС1) (ПП Дельта 113, ИЭС2-123)",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_263",
                "Name": "Извещение о результатах контроля отчетности КО по форме 0409701 (ИЭС1)",
                "Description": "",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_1-28",
                "Name": "Отправить XBRL",
                "Description": null,
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_261",
                "Name": "Представление данных АС ПСД ТУ/ЕИС ЦФО ",
                "Description": "Представление данных АС ПСД ТУ/ЕИС ЦФО ",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_514",
                "Name": "Возврат депозитов КО",
                "Description": "Передача из ЛК КО ЭС ED262 в ПС ОД ОПЕРУ-1",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_49",
                "Name": "Ответ на запрос Анкетирование. Ответная квитанция подсистемы Анкетирование Клиенту на присланные ранее данные",
                "Description": "Взаимообмен по потоку Анкетирование. Ответ от УФР и ответная квитанция с protocl.html",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_222",
                "Name": "Запрос информации о платежах КО",
                "Description": "Направление запроса из Приложения Поток \"Платежи КО\" в сторону ВП ЕПВВ",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_218",
                "Name": "Изменение параметров исполнения обязательств по кредитам Банка России по инициативе КО (поток ЭДО ДКО)",
                "Description": "Передача из ЛК КО ЭС ED260 в ПС ОД ОПЕРУ-1",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_1-40-2",
                "Name": "Активы КО",
                "Description": null,
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_40",
                "Name": "ЭДО ДКО",
                "Description": "при отправке ЭС от КО в адрес АС ДКО через ЛК и при отправке ответных ЭС от АС ДКО",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_221",
                "Name": "Данные о платежах в ответах КО",
                "Description": "Передача ответного сообщения с данными о платежах из ЕПВВ в Поток \"Платежи КО\"",
                "AllowAspera": true,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_42",
                "Name": "ЭДО ДКО (обратный поток)",
                "Description": "при иницировании ЭС от АС ДКО",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_20",
                "Name": "Активы КО (обратный поток)",
                "Description": null,
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_1-40-1",
                "Name": "Представление первичных документов",
                "Description": null,
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_38",
                "Name": "Представление первичных документов",
                "Description": null,
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_60",
                "Name": "Прототип Реестра залогов",
                "Description": null,
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_150",
                "Name": "Представление информации по запросу в структурированном (сжатом) виде",
                "Description": "Поток представления иной информации в ФПС Отчетность",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_81",
                "Name": "Ответ на сбор файлов номеров банкнот",
                "Description": null,
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_139",
                "Name": "Представление файлов с номерами банкнот в Банк России",
                "Description": "В рамках данной задачи направляются файлы с номерами банкнот в Банк России",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_130",
                "Name": "Получение информации об уровне риска ЮЛ/ИП",
                "Description": "Получение информации об уровне риска ЮЛ/ИП",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_137",
                "Name": "Ежедневное информирование Банка России о составе и объеме клиентской базы (ФПС \"Отчетность\")",
                "Description": "Ежедневное информирование Банка России о составе и объеме клиентской базы (передача через СДС)",
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_2-1",
                "Name": "Ответ на запрос, предписание. Запрос в Банк России. Квитанции из ВП ЕПВВ",
                "Description": null,
                "AllowAspera": false,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_212",
                "Name": "Передача сообщений внешнего абонента с формой Эмитенты",
                "Description": "Передача документов в САДД БР (РВЭЦБ)",
                "AllowAspera": true,
                "Direction": "outbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_200",
                "Name": "Передача запроса от АС ВХД в ГИС МТ и получение ответа",
                "Description": "Передача запроса от АС ВХД в ГИС МТ и получение ответа",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_192",
                "Name": "Передача \"Заявки РГ\" от КЗ ИД (КРГ) в ВП ЕПВВ",
                "Description": "Передача \"Заявки РГ\" от КЗ ИД (КРГ) в ВП ЕПВВ. Используется в Поток ЭДО РГ. Передача \"Заявки РГ\" в \"Интерфейс КРГ\" в ЛК.",
                "AllowAspera": false,
                "Direction": "inbox",
                "AllowLinkedMessages": false
            },
            {
                "Code": "Zadacha_44",
                "Name": "Передача отчетности 1-ИЦБ, 1-АРЕНДА, 1-ПОЕЗДКИ, 1-РОУМИНГ, 1,2,3-ТРАНСПОРТ, 1-МЕД, 1-СТР",
                "Description": "Взаимообмен по потоку Анкетирование. Ответ от УФР и ответная квитанция с protocl.html",
                "AllowAspera": false,
                "Direction": MessageType.Outbox,
                "AllowLinkedMessages": false
            }
        ]
        """;

    /// <summary>
    /// tasks?direction=d
    /// </summary>
    /// <param name="direction">0, 1, 2</param>
    /// <returns></returns>
    /// <remarks>
    /// 0 - inbox<br/>
    /// 1 - outbox<br/>
    /// 2 - bidirectional
    /// </remarks>
    public static string Text(int direction) => direction switch
    {
        // inbox
        0 => """
            [
                {
                    "Code": "Zadacha_204",
                    "Name": "Получение ЭОП/Уведомлений",
                    "Description": "",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_161",
                    "Name": "Получение ЭОП",
                    "Description": "В рамках задачи направляется ЭС с ЭОП из Банка России в адрес УИО",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_94",
                    "Name": "Расчёт и регулирование размера обязательных резервов",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_1-33",
                    "Name": "Массовая рассылка в соответствии со 115-ФЗ (639-П).",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_515",
                    "Name": "Информация о результатах приема/обработки ЭС",
                    "Description": "Передача из ПС ОД ОПЕРУ-1 ЭС ED263 в ЛК КО с выполнением контролей",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_48",
                    "Name": "Запрос Анкетирование",
                    "Description": "Взаимообмен по потоку Анкетирование. Отправка шаблонов анкет из БР.",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_78",
                    "Name": "Извещения и уведомления по депозитным и кредитным операциям",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_3-1",
                    "Name": "Запрос, предписание. Ответ на запрос НФО. Квитанции из САДД",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_51",
                    "Name": "Нева. Поток по инициативе НЕВЫ",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_86",
                    "Name": "Сообщение из системы Банка России. Ответ на сообщение УИО.",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_100",
                    "Name": "Выгрузка данных из ППК КГР в ЛК",
                    "Description": "В рамках данной задачи направляются данные из ЦБ в ГБО ФНС России",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_140",
                    "Name": "Извещение о результатах контроля информации о ВПОДК и их результатах (ИЭС2)",
                    "Description": "Ответ на предоставление информации о результатах контроля информации о ВПОДК и их результатах (ИЭС2)",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_97",
                    "Name": "Извещение о результатах контроля информации о ВПОДК и их результатах (ИЭС1)",
                    "Description": "Ответ на предоставление информации о результатах контроля информации о ВПОДК и их результатах (ИЭС1)",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_123",
                    "Name": "Извещение о результатах контроля представления формы 0409310 (ИЭС2)",
                    "Description": "Извещение о результатах контроля представления формы 0409310 (ИЭС2) (ПП Дельта 113, ИЭС1-114)",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_114",
                    "Name": "Извещение о результатах контроля представления формы 0409310 (ИЭС1)",
                    "Description": "Извещение о результатах контроля представления формы 0409310 (ИЭС1) (ПП Дельта 113, ИЭС2-123)",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_263",
                    "Name": "Извещение о результатах контроля отчетности КО по форме 0409701 (ИЭС1)",
                    "Description": "",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_222",
                    "Name": "Запрос информации о платежах КО",
                    "Description": "Направление запроса из Приложения Поток \"Платежи КО\" в сторону ВП ЕПВВ",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_20",
                    "Name": "Активы КО (обратный поток)",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_81",
                    "Name": "Ответ на сбор файлов номеров банкнот",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_130",
                    "Name": "Получение информации об уровне риска ЮЛ/ИП",
                    "Description": "Получение информации об уровне риска ЮЛ/ИП",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_200",
                    "Name": "Передача запроса от АС ВХД в ГИС МТ и получение ответа",
                    "Description": "Передача запроса от АС ВХД в ГИС МТ и получение ответа",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_192",
                    "Name": "Передача \"Заявки РГ\" от КЗ ИД (КРГ) в ВП ЕПВВ",
                    "Description": "Передача \"Заявки РГ\" от КЗ ИД (КРГ) в ВП ЕПВВ. Используется в Поток ЭДО РГ. Передача \"Заявки РГ\" в \"Интерфейс КРГ\" в ЛК.",
                    "AllowAspera": false,
                    "Direction": "inbox",
                    "AllowLinkedMessages": false
                }
            ]
            """,

        // outbox
        1 => """
            [
                {
                    "Code": "Zadacha_29",
                    "Name": "Отчетность субъектов рынка ценных бумаг и товарного рынка",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_1-31",
                    "Name": "Активы КО",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "GroupTask_22",
                    "Name": "Федеральные статистические наблюдения, утвержденные Указанием Банка России от 16.02.2023 № 6363-У",
                    "Description": "старое наименование Федеральные статистические наблюдения, утвержденные Указанием Банка России от 25.11.2019 № 5328-У",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_162",
                    "Name": "Уведомление о результате контроля ЭОП",
                    "Description": "В рамках задачи направляется ответное Уведомление от УИО в Банк России на ЭС с ЭОП ",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_1-33-2",
                    "Name": "Массовая рассылка в соответствии со 115-ФЗ (639-П) Ответ",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_113",
                    "Name": "Представление формы 0409310",
                    "Description": "Поток представления отчетности по форме 0409310 в ППК МПСО (ПП Дельта ИЭС1-114, ИЭС2-123)",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_120",
                    "Name": "Отправить XBRL-CSV",
                    "Description": "Передача отчетности в формате XBRL-CSV",
                    "AllowAspera": true,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_260",
                    "Name": "Отчетность 0409701",
                    "Description": "Представление отчетности КО по форме 0409701 в Банк России",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_179",
                    "Name": "Направление информации в реестр эмиссионных ценных бумаг",
                    "Description": "Передача форм в РУФР",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_77",
                    "Name": "Процедуры допуска",
                    "Description": "Блок 625-П",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_147",
                    "Name": "Представление отчетов, указанных в ч. 7 ст. 12 Закона № 173-ФЗ",
                    "Description": "Поток представления отчетности о движении денежных средств на ин. счетах",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_76",
                    "Name": "Предоставление информации о договорах займа",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_85",
                    "Name": "Унифицированный обмен",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_1-32",
                    "Name": "Получение отчетности из Клиентского ПО через SOAP-сервис",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_74",
                    "Name": "Представление информации о ВПОДК и их результатах",
                    "Description": "Представление информации о ВПОДК и их результатах",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_106",
                    "Name": "Представление заявлений Операторами платежных систем",
                    "Description": "В рамках данной задачи направляются завления Операторами платежных систем",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_157",
                    "Name": "Представление отчетности банковских холдингов в Банк России",
                    "Description": "(ПП Дельта ИЭС1-158, ИЭС2-160)",
                    "AllowAspera": true,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_155",
                    "Name": "Представление отчетности КО в Банк России",
                    "Description": "(ПП Дельта ИЭС1-156, ИЭС2-159)",
                    "AllowAspera": true,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_104",
                    "Name": "Представление отчетности субъектов НПС в Банк России",
                    "Description": "Представление отчетности и иной информации субъектов национальной платежной системы, не являющихся кредитными организациями, в Банк России (ПП Дельта ИЭС1-107, ИЭС2-133)",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_1-28",
                    "Name": "Отправить XBRL",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_261",
                    "Name": "Представление данных АС ПСД ТУ/ЕИС ЦФО ",
                    "Description": "Представление данных АС ПСД ТУ/ЕИС ЦФО ",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_514",
                    "Name": "Возврат депозитов КО",
                    "Description": "Передача из ЛК КО ЭС ED262 в ПС ОД ОПЕРУ-1",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_49",
                    "Name": "Ответ на запрос Анкетирование. Ответная квитанция подсистемы Анкетирование Клиенту на присланные ранее данные",
                    "Description": "Взаимообмен по потоку Анкетирование. Ответ от УФР и ответная квитанция с protocl.html",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_218",
                    "Name": "Изменение параметров исполнения обязательств по кредитам Банка России по инициативе КО (поток ЭДО ДКО)",
                    "Description": "Передача из ЛК КО ЭС ED260 в ПС ОД ОПЕРУ-1",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_1-40-2",
                    "Name": "Активы КО",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_40",
                    "Name": "ЭДО ДКО",
                    "Description": "при отправке ЭС от КО в адрес АС ДКО через ЛК и при отправке ответных ЭС от АС ДКО",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_221",
                    "Name": "Данные о платежах в ответах КО",
                    "Description": "Передача ответного сообщения с данными о платежах из ЕПВВ в Поток \"Платежи КО\"",
                    "AllowAspera": true,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_42",
                    "Name": "ЭДО ДКО (обратный поток)",
                    "Description": "при иницировании ЭС от АС ДКО",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_1-40-1",
                    "Name": "Представление первичных документов",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_38",
                    "Name": "Представление первичных документов",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_60",
                    "Name": "Прототип Реестра залогов",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_150",
                    "Name": "Представление информации по запросу в структурированном (сжатом) виде",
                    "Description": "Поток представления иной информации в ФПС Отчетность",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_139",
                    "Name": "Представление файлов с номерами банкнот в Банк России",
                    "Description": "В рамках данной задачи направляются файлы с номерами банкнот в Банк России",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_137",
                    "Name": "Ежедневное информирование Банка России о составе и объеме клиентской базы (ФПС \"Отчетность\")",
                    "Description": "Ежедневное информирование Банка России о составе и объеме клиентской базы (передача через СДС)",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_2-1",
                    "Name": "Ответ на запрос, предписание. Запрос в Банк России. Квитанции из ВП ЕПВВ",
                    "Description": null,
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_212",
                    "Name": "Передача сообщений внешнего абонента с формой Эмитенты",
                    "Description": "Передача документов в САДД БР (РВЭЦБ)",
                    "AllowAspera": true,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_44",
                    "Name": "Передача отчетности 1-ИЦБ, 1-АРЕНДА, 1-ПОЕЗДКИ, 1-РОУМИНГ, 1,2,3-ТРАНСПОРТ, 1-МЕД, 1-СТР",
                    "Description": "Взаимообмен по потоку Анкетирование. Ответ от УФР и ответная квитанция с protocl.html",
                    "AllowAspera": false,
                    "Direction": "outbox",
                    "AllowLinkedMessages": false
                }
            ]
            """,

        // bidirectional
        2 => """
            [
                {
                    "Code": "Zadacha_55",
                    "Name": "Ответ на запрос ЦИК",
                    "Description": "Ответ #FEATURE:Sender@FullName#",
                    "AllowAspera": false,
                    "Direction": "bidirectional",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_58",
                    "Name": "Прикладная квитанция УФР в ответ на запрос ЦИК",
                    "Description": "Квитанция #FEATURE:Sender@FullName#",
                    "AllowAspera": false,
                    "Direction": "bidirectional",
                    "AllowLinkedMessages": false
                },
                {
                    "Code": "Zadacha_63",
                    "Name": "Ответ по потоку Агентства страхования вкладов (АСВ)",
                    "Description": "",
                    "AllowAspera": false,
                    "Direction": "bidirectional",
                    "AllowLinkedMessages": false
                }
            ]
            """,

        // error
        _ => """
            {
                "HTTPStatus": 400,
                "ErrorCode": "DERECTION_INCORRECT",
                "ErrorMessage": "Неверное значение параметра direction (допустимо 0,1,2)",
                "MoreInfo": null
            }
            """
    };
}
