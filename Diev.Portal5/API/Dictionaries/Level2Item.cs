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

namespace Diev.Portal5.API.Dictionaries;

/// <summary>
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries/25338cfb-5713-4634-bc53-a81129483752 (level 2)
/// </summary>
public record Level2Item
(
    /// <summary>
    /// Example: "147"
    /// </summary>
    string? Code,

    /// <summary>
    /// Example: "Департамент статистики"
    /// </summary>
    string? Subjects2,

    /// <summary>
    /// Example: "ЦА"
    /// </summary>
    string? TypeIE,

    /// <summary>
    /// Example: ""
    /// </summary>
    string? TypeIE2,

    /// <summary>
    /// Example: "Департамент статистики"
    /// </summary>
    string? Addresse,

    /// <summary>
    /// Example: "48_lk"
    /// </summary>
    string? DirSDS,

    /// <summary>
    /// Example: "Банк России"
    /// </summary>
    string? Organization,

    /// <summary>
    /// Example: "6fc60350-fa90-450e-9fea-1b0703501d6a"
    /// </summary>
    string? Id
);

/*
Header:
EPVV-Total: 59
EPVV-TotalPages: 1
EPVV-CurrentPage: 1
EPVV-PerCurrentPage: 59

Body:
{
    "Items": [
        {
            "Code": "147",
            "Subjects2": "Департамент статистики",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент статистики",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "8f272a70-7ad0-4ccd-8042-b09600c475b8"
        },
        {
            "Code": "103",
            "Subjects2": "Департамент банковского регулирования и аналитики",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент банковского регулирования и аналитики",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "5a1808bf-ea82-426d-834a-b09600c475b8"
        },
        {
            "Code": "301",
            "Subjects2": "Набиуллина Э.С.",
            "TypeIE": "РУ",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "56a9c459-6b7b-4a79-8379-b09600c475b8"
        },
        {
            "Code": "129",
            "Subjects2": "Служба анализа рисков",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Служба анализа рисков",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "ed8fe820-975a-4698-8617-b09600c475b8"
        },
        {
            "Code": "102",
            "Subjects2": "Департамент информационной безопасности",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент информационной безопасности",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "872eb993-08a1-49a1-87ee-b09600c475b8"
        },
        {
            "Code": "126",
            "Subjects2": "Департамент финансового оздоровления",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент финансового оздоровления",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "c6877b34-2504-4b84-896c-b09600c475b8"
        },
        {
            "Code": "320",
            "Subjects2": "Габуния Ф.Г.",
            "TypeIE": "РУ",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "01996a43-7501-4ecf-8b41-b09600c475b8"
        },
        {
            "Code": "110",
            "Subjects2": "Департамент корпоративных отношений",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент корпоративных отношений",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "f10bbbc4-667b-4290-8b9e-b09600c475b8"
        },
        {
            "Code": "139",
            "Subjects2": "Финансовый департамент",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Финансовый департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "1bbe7955-3d41-428d-8be0-b09600c475b8"
        },
        {
            "Code": "207",
            "Subjects2": "Дальневосточное ГУ Банка России",
            "TypeIE": "ТУ",
            "TypeIE2": "ДГУ",
            "Addressee": "",
            "DirSDS": "",
            "Organization": "",
            "Id": "c2cf2484-bd86-4e10-8d30-b09600c475b8"
        },
        {
            "Code": "135",
            "Subjects2": "Департамент кадровой политики",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент кадровой политики",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "4bb8e086-fb61-4f7e-8f72-b09600c475b8"
        },
        {
            "Code": "205",
            "Subjects2": "Уральское ГУ Банка России",
            "TypeIE": "ТУ",
            "TypeIE2": "УГУ",
            "Addressee": "",
            "DirSDS": "",
            "Organization": "",
            "Id": "072c2cc2-f97b-4084-8f8f-b09600c475b8"
        },
        {
            "Code": "323",
            "Subjects2": "Заботкин А.Б.",
            "TypeIE": "РУ",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "777b66e6-0855-40ea-90d7-b09600c475b8"
        },
        {
            "Code": "141",
            "Subjects2": "Департамент проектов и процессов",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент проектов и процессов",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "1226d606-615a-40c4-916b-b09600c475b8"
        },
        {
            "Code": "114",
            "Subjects2": "Департамент наличного денежного обращения",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент наличного денежного обращения",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "cb05298e-23fa-4deb-91ee-b09600c475b8"
        },
        {
            "Code": "317",
            "Subjects2": "Департамент инфраструктуры финансового рынка",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент инфраструктуры финансового рынка",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "680e12bd-2b71-449b-9218-b09600c475b8"
        },
        {
            "Code": "134",
            "Subjects2": "Департамент безопасности Банка России",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент безопасности Банка России",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "261f4cf8-781b-420b-923b-b09600c475b8"
        },
        {
            "Code": "101",
            "Subjects2": "Главная инспекция Банка России",
            "TypeIE": "ЦА",
            "TypeIE2": "ГИБР",
            "Addressee": "",
            "DirSDS": "",
            "Organization": "",
            "Id": "b5b0e5fa-28f2-4e46-92c2-b09600c475b8"
        },
        {
            "Code": "326",
            "Subjects2": "Кружалов А.В.",
            "TypeIE": "РУ",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "1fd7cbb3-3996-4668-9432-b09600c475b8"
        },
        {
            "Code": "138",
            "Subjects2": "Департамент информационных технологий",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент информационных технологий",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "e682e1d7-da89-47f3-96f8-b09600c475b8"
        },
        {
            "Code": "321",
            "Subjects2": "Белов С.В.",
            "TypeIE": "РУ",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "7a237dbc-1bfb-48d4-9839-b09600c475b8"
        },
        {
            "Code": "206",
            "Subjects2": "Сибирское ГУ Банка России",
            "TypeIE": "ТУ",
            "TypeIE2": "СГУ",
            "Addressee": "",
            "DirSDS": "",
            "Organization": "",
            "Id": "2210120f-7e77-4047-9a1f-b09600c475b8"
        },
        {
            "Code": "128",
            "Subjects2": "Операционный департамент",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Операционный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "ad16f5cc-9f34-4d00-9aef-b09600c475b8"
        },
        {
            "Code": "314",
            "Subjects2": "Юдаева К.В.",
            "TypeIE": "РУ",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "da5647b7-9e6b-4327-9d10-b09600c475b8"
        },
        {
            "Code": "312",
            "Subjects2": "Чистюхин В.В.",
            "TypeIE": "РУ",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "ec2490d5-db3e-4ae7-9ebc-b09600c475b8"
        },
        {
            "Code": "309",
            "Subjects2": "Скоробогатова О.Н.",
            "TypeIE": "РУ",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "a996afad-cf73-4249-a321-b09600c475b8"
        },
        {
            "Code": "315",
            "Subjects2": "Банк России",
            "TypeIE": "АД",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "86d6d396-e44f-451c-a4cc-b09600c475b8"
        },
        {
            "Code": "318",
            "Subjects2": "Департамент инвестиционных финансовых посредников",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент инвестиционных финансовых посредников",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "be9bb124-94dd-4d74-a5e3-b09600c475b8"
        },
        {
            "Code": "133",
            "Subjects2": "Департамент регулирования бухгалтерского учета",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент регулирования бухгалтерского учета",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "0490fb0b-00bd-4e31-a6d0-b09600c475b8"
        },
        {
            "Code": "142",
            "Subjects2": "Департамент закупок Банка России",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент закупок Банка России",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "eabf7696-5dac-47ea-a833-b09600c475b8"
        },
        {
            "Code": "112",
            "Subjects2": "Департамент небанковского кредитования",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент небанковского кредитования",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "8a09b33c-45a5-4ae9-a9cc-b09600c475b8"
        },
        {
            "Code": "203",
            "Subjects2": "Южное ГУ Банка России",
            "TypeIE": "ТУ",
            "TypeIE2": "ЮГУ",
            "Addressee": "",
            "DirSDS": "",
            "Organization": "",
            "Id": "9016b95e-ea93-49b8-aa9f-b09600c475b8"
        },
        {
            "Code": "115",
            "Subjects2": "Департамент национальной платежной системы",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент национальной платежной системы",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "ea021218-0963-4d09-ad7f-b09600c475b8"
        },
        {
            "Code": "108",
            "Subjects2": "Департамент исследований и прогнозирования",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент исследований и прогнозирования",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "39a697e7-4e91-4a71-af35-b09600c475b8"
        },
        {
            "Code": "131",
            "Subjects2": "Служба текущего банковского надзора",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Служба текущего банковского надзора",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "f4a3eff7-c7a0-4907-b09e-b09600c475b8"
        },
        {
            "Code": "127",
            "Subjects2": "Департамент финансовой стабильности",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент финансовой стабильности",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "f3e984f3-250b-49f1-b112-b09600c475b8"
        },
        {
            "Code": "143",
            "Subjects2": "Департамент по связям с общественностью",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент по связям с общественностью",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "75316561-b484-4b88-b129-b09600c475b8"
        },
        {
            "Code": "306",
            "Subjects2": "Полякова О.В.",
            "TypeIE": "РУ",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "0cc81a13-b0ae-4e3a-b1fe-b09600c475b8"
        },
        {
            "Code": "117",
            "Subjects2": "Департамент операций на финансовых рынках",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент операций на финансовых рынках",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "1da49d40-3ec6-4938-b273-b09600c475b8"
        },
        {
            "Code": "322",
            "Subjects2": "Департамент организации международных расчетов",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент организации международных расчетов",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "cb2befae-2601-4e84-b2eb-b09600c475b8"
        },
        {
            "Code": "105",
            "Subjects2": "Департамент денежно-кредитной политики",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент денежно-кредитной политики",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "614cdf0a-5ea3-44b5-b31c-b09600c475b8"
        },
        {
            "Code": "137",
            "Subjects2": "Департамент финансовых технологий",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент финансовых технологий",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "4baebec9-9a80-413f-b39f-b09600c475b8"
        },
        {
            "Code": "125",
            "Subjects2": "Департамент финансового мониторинга и валютного контроля",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент финансового мониторинга и валютного контроля",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "39aab5b5-0ee8-4106-b45e-b09600c475b8"
        },
        {
            "Code": "146",
            "Subjects2": "Департамент управления данными",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент управления данными",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "8bff41be-77ef-469f-b4d9-b09600c475b8"
        },
        {
            "Code": "311",
            "Subjects2": "Тулин Д.В.",
            "TypeIE": "РУ",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "fd8401a2-f839-4e5d-b508-b09600c475b8"
        },
        {
            "Code": "145",
            "Subjects2": "Университет Банка России",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Университет Банка России",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "8c3989aa-2af9-4b3e-b58a-b09600c475b8"
        },
        {
            "Code": "204",
            "Subjects2": "Волго-Вятское ГУ Банка России",
            "TypeIE": "ТУ",
            "TypeIE2": "ВВГУ",
            "Addressee": "",
            "DirSDS": "",
            "Organization": "",
            "Id": "2878b367-6f36-4fc1-b6f3-b09600c475b8"
        },
        {
            "Code": "121",
            "Subjects2": "Департамент стратегического развития финансового рынка",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент стратегического развития финансового рынка",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "bd968809-a09e-4792-b77b-b09600c475b8"
        },
        {
            "Code": "106",
            "Subjects2": "Департамент допуска и прекращения деятельности финансовых организаций",
            "TypeIE": "ЦА",
            "TypeIE2": "ДДПДФО",
            "Addressee": "",
            "DirSDS": "",
            "Organization": "",
            "Id": "49c68bbe-0b10-40dc-b79f-b09600c475b8"
        },
        {
            "Code": "319",
            "Subjects2": "Департамент полевых учреждений",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент полевых учреждений",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "9abf7287-b9c9-4f75-b9d8-b09600c475b8"
        },
        {
            "Code": "202",
            "Subjects2": "Северо-Западное ГУ Банка России",
            "TypeIE": "ТУ",
            "TypeIE2": "СЗГУ",
            "Addressee": "",
            "DirSDS": "",
            "Organization": "",
            "Id": "1f924581-0b2c-4624-baca-b09600c475b8"
        },
        {
            "Code": "144",
            "Subjects2": "Департамент недвижимости",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент недвижимости",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "87e09dff-b0eb-4419-bad8-b09600c475b8"
        },
        {
            "Code": "124",
            "Subjects2": "Департамент страхового рынка",
            "TypeIE": "ЦА",
            "TypeIE2": "ДСР",
            "Addressee": "",
            "DirSDS": "",
            "Organization": "",
            "Id": "a0c4beab-2e20-4a29-bb24-b09600c475b8"
        },
        {
            "Code": "132",
            "Subjects2": "Департамент бухгалтерского учета и отчетности",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент бухгалтерского учета и отчетности",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "03fc737e-9ce6-48ec-bbb7-b09600c475b8"
        },
        {
            "Code": "130",
            "Subjects2": "Служба по защите прав потребителей и обеспечению доступности финансовых услуг",
            "TypeIE": "ЦА",
            "TypeIE2": "СЗППОДФУ",
            "Addressee": "",
            "DirSDS": "",
            "Organization": "",
            "Id": "c37b47f1-9790-48a1-bcd0-b09600c475b8"
        },
        {
            "Code": "324",
            "Subjects2": "Зубарев Г.А.",
            "TypeIE": "РУ",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "6d8b3441-bac5-4457-bce5-b09600c475b8"
        },
        {
            "Code": "201",
            "Subjects2": "ГУ Банка России по Центральному федеральному округу (Московский регион)",
            "TypeIE": "ТУ",
            "TypeIE2": "ЦФО",
            "Addressee": "",
            "DirSDS": "",
            "Organization": "",
            "Id": "d19d3034-a6fb-483e-bde4-b09600c475b8"
        },
        {
            "Code": "325",
            "Subjects2": "Казарин В.С.",
            "TypeIE": "РУ",
            "TypeIE2": "",
            "Addressee": "Административный департамент",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "e7e835f3-bff3-42e6-bed9-b09600c475b8"
        },
        {
            "Code": "113",
            "Subjects2": "Департамент надзора за системно значимыми кредитными организациями",
            "TypeIE": "ЦА",
            "TypeIE2": "",
            "Addressee": "Департамент надзора за системно значимыми кредитными организациями",
            "DirSDS": "48_lk",
            "Organization": "Банк России",
            "Id": "fc66480b-83d4-4614-bf44-b09600c475b8"
        }
    ],
    "PaginationInfo": {
        "TotalRecords": 59,
        "TotalPages": 1,
        "CurrentPage": 1,
        "PerCurrentPage": 59,
        "PerNextPage": null,
        "MaxPerPage": 100
    }
}
*/
