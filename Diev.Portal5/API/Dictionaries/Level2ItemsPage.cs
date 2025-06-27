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

using Diev.Portal5.API.Tools;

namespace Diev.Portal5.API.Dictionaries;

/// <summary>
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries/25338cfb-5713-4634-bc53-a81129483752 (level 2)<br/>
/// GET https://portal5.cbr.ru/back/rapi2/dictionaries/25338cfb-5713-4634-bc53-a81129483752?page=2<br/>
/// 200 OK
/// </summary>
public record Level2ItemsPage
(
    /// <summary>
    /// Example: "[{...}, ...]"
    /// </summary>
    IReadOnlyList<Level2Item> Items,

    /// <summary>
    /// Example: {
    /// "TotalRecords": 124,
    /// "TotalPages": 2,
    /// "CurrentPage": 1,
    /// "PerCurrentPage": 100,
    /// "PerNextPage": 24,
    /// "MaxPerPage": 100
    /// }
    /// </summary>
    Pagination Pages
);

public static class MockLevel2ItemsPage
{
    /// <summary>
    /// dictionaries/238d0426-6f57-4c0f-8983-1d1addf8c47a
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Header:<br/>
    /// EPVV-Total: 58<br/>
    /// EPVV-TotalPages: 1<br/>
    /// EPVV-CurrentPage: 1<br/>
    /// EPVV-PerCurrentPage: 58
    /// </remarks>
    public static string Page(int page = 1) => page switch
    {
        1 => """
            {
                "Items": [
                    {
                        "Code": "127",
                        "Subjects2": "Департамент финансовой стабильности",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент финансовой стабильности",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "0000a038-0b54-4b53-a9bb-b2ce00a14930"
                    },
                    {
                        "Code": "317",
                        "Subjects2": "Департамент инфраструктуры финансового рынка",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент инфраструктуры финансового рынка",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "00043644-1ff4-41a4-8e97-b2ce00a14930"
                    },
                    {
                        "Code": "325",
                        "Subjects2": "Казарин В.С.",
                        "TypeIE": "РУ",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "02992355-126e-47a7-90bc-b2ce00a14930"
                    },
                    {
                        "Code": "113",
                        "Subjects2": "Департамент надзора за системно значимыми кредитными организациями",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент надзора за системно значимыми кредитными организациями",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "11b9f67c-a0d6-4158-a7c2-b2ce00a14930"
                    },
                    {
                        "Code": "201",
                        "Subjects2": "ГУ Банка России по Центральному федеральному округу (Московский регион)",
                        "TypeIE": "ТУ",
                        "TypeIE2": "ЦФО",
                        "Addressee": "",
                        "DirSDS": "",
                        "Organization": "",
                        "Id": "159c68ae-bf38-4665-8e78-b2ce00a14930"
                    },
                    {
                        "Code": "143",
                        "Subjects2": "Департамент по связям с общественностью",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент по связям с общественностью",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "193e3bbe-eca2-4177-9b4d-b2ce00a14930"
                    },
                    {
                        "Code": "139",
                        "Subjects2": "Финансовый департамент",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Финансовый департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "1e4b1e9f-77b4-40e6-b4af-b2ce00a14930"
                    },
                    {
                        "Code": "112",
                        "Subjects2": "Департамент небанковского кредитования",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент небанковского кредитования",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "1fcf4026-e67d-4265-ba1a-b2ce00a14930"
                    },
                    {
                        "Code": "117",
                        "Subjects2": "Департамент операций на финансовых рынках",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент операций на финансовых рынках",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "3329f67f-d076-4378-bd19-b2ce00a14930"
                    },
                    {
                        "Code": "110",
                        "Subjects2": "Департамент корпоративных отношений",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент корпоративных отношений",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "3619efa6-47d3-4535-b6e4-b2ce00a14930"
                    },
                    {
                        "Code": "138",
                        "Subjects2": "Департамент информационных технологий",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент информационных технологий",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "3669898e-8108-4d26-bc6d-b2ce00a14930"
                    },
                    {
                        "Code": "319",
                        "Subjects2": "Департамент полевых учреждений",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент полевых учреждений",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "36e5cef2-2fe1-4b26-a932-b2ce00a14930"
                    },
                    {
                        "Code": "142",
                        "Subjects2": "Департамент закупок Банка России",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент закупок Банка России",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "3a18c148-d1ed-4894-94c7-b2ce00a14930"
                    },
                    {
                        "Code": "326",
                        "Subjects2": "Кружалов А.В.",
                        "TypeIE": "РУ",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "3a7570ce-0c58-44a1-accc-b2ce00a14930"
                    },
                    {
                        "Code": "126",
                        "Subjects2": "Департамент финансового оздоровления",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент финансового оздоровления",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "3baaa1c9-ec87-4c3b-b0a5-b2ce00a14930"
                    },
                    {
                        "Code": "132",
                        "Subjects2": "Департамент бухгалтерского учета и отчетности",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент бухгалтерского учета и отчетности",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "3d188542-5d84-4102-9c7b-b2ce00a14930"
                    },
                    {
                        "Code": "205",
                        "Subjects2": "Уральское ГУ Банка России",
                        "TypeIE": "ТУ",
                        "TypeIE2": "УГУ",
                        "Addressee": "",
                        "DirSDS": "",
                        "Organization": "",
                        "Id": "4630e6c0-af50-4cf2-bd56-b2ce00a14930"
                    },
                    {
                        "Code": "204",
                        "Subjects2": "Волго-Вятское ГУ Банка России",
                        "TypeIE": "ТУ",
                        "TypeIE2": "ВВГУ",
                        "Addressee": "",
                        "DirSDS": "",
                        "Organization": "",
                        "Id": "48e65b08-3161-4170-b868-b2ce00a14930"
                    },
                    {
                        "Code": "324",
                        "Subjects2": "Зубарев Г.А.",
                        "TypeIE": "РУ",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "48f03748-d785-4c31-a62a-b2ce00a14930"
                    },
                    {
                        "Code": "130",
                        "Subjects2": "Служба по защите прав потребителей и обеспечению доступности финансовых услуг",
                        "TypeIE": "ЦА",
                        "TypeIE2": "СЗППОДФУ",
                        "Addressee": "",
                        "DirSDS": "",
                        "Organization": "",
                        "Id": "4941abe6-5c09-419a-98c3-b2ce00a14930"
                    },
                    {
                        "Code": "137",
                        "Subjects2": "Департамент финансовых технологий",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент финансовых технологий",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "50442bf0-278e-482f-b5a6-b2ce00a14930"
                    },
                    {
                        "Code": "306",
                        "Subjects2": "Полякова О.В.",
                        "TypeIE": "РУ",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "5aff16e2-8af2-42d4-bde0-b2ce00a14930"
                    },
                    {
                        "Code": "323",
                        "Subjects2": "Заботкин А.Б.",
                        "TypeIE": "РУ",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "5cfdc8e4-a021-49b8-9481-b2ce00a14930"
                    },
                    {
                        "Code": "129",
                        "Subjects2": "Служба анализа рисков",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Служба анализа рисков",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "64faa7e0-d3d8-45e9-bbb5-b2ce00a14930"
                    },
                    {
                        "Code": "131",
                        "Subjects2": "Служба текущего банковского надзора",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Служба текущего банковского надзора",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "657139d4-d10e-4802-8b18-b2ce00a14930"
                    },
                    {
                        "Code": "124",
                        "Subjects2": "Департамент страхового рынка",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент страхового рынка",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "6d899179-c714-4a3b-b10d-b2ce00a14930"
                    },
                    {
                        "Code": "101",
                        "Subjects2": "Главная инспекция Банка России",
                        "TypeIE": "ЦА",
                        "TypeIE2": "ГИБР",
                        "Addressee": "",
                        "DirSDS": "",
                        "Organization": "",
                        "Id": "73791b68-de6c-41b6-b89f-b2ce00a14930"
                    },
                    {
                        "Code": "128",
                        "Subjects2": "Операционный департамент",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Операционный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "798797a5-74a0-4b6f-93e0-b2ce00a14930"
                    },
                    {
                        "Code": "312",
                        "Subjects2": "Чистюхин В.В.",
                        "TypeIE": "РУ",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "7a2cfad4-2506-4a88-bdbb-b2ce00a14930"
                    },
                    {
                        "Code": "105",
                        "Subjects2": "Департамент денежно-кредитной политики",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент денежно-кредитной политики",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "7b51dfa7-1074-40a4-a7b1-b2ce00a14930"
                    },
                    {
                        "Code": "102",
                        "Subjects2": "Департамент информационной безопасности",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент информационной безопасности",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "8051b2e4-95c8-4090-bc57-b2ce00a14930"
                    },
                    {
                        "Code": "320",
                        "Subjects2": "Габуния Ф.Г.",
                        "TypeIE": "РУ",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "86c3e724-9a22-4304-9783-b2ce00a14930"
                    },
                    {
                        "Code": "206",
                        "Subjects2": "Сибирское ГУ Банка России",
                        "TypeIE": "ТУ",
                        "TypeIE2": "СГУ",
                        "Addressee": "",
                        "DirSDS": "",
                        "Organization": "",
                        "Id": "8b9355ad-013f-45b8-a4d1-b2ce00a14930"
                    },
                    {
                        "Code": "145",
                        "Subjects2": "Университет Банка России",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Университет Банка России",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "8c2e6f0a-7e33-4828-8269-b2ce00a14930"
                    },
                    {
                        "Code": "301",
                        "Subjects2": "Набиуллина Э.С.",
                        "TypeIE": "РУ",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "8c5c0a16-a7ee-4c90-8735-b2ce00a14930"
                    },
                    {
                        "Code": "318",
                        "Subjects2": "Департамент инвестиционных финансовых посредников",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент инвестиционных финансовых посредников",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "99bd2a4d-a4f1-4d77-a3bd-b2ce00a14930"
                    },
                    {
                        "Code": "135",
                        "Subjects2": "Департамент кадровой политики",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент кадровой политики",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "9fcad132-ac92-4a65-b154-b2ce00a14930"
                    },
                    {
                        "Code": "314",
                        "Subjects2": "Юдаева К.В.",
                        "TypeIE": "РУ",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "a89a09e6-6a77-423e-95f5-b2ce00a14930"
                    },
                    {
                        "Code": "315",
                        "Subjects2": "Банк России",
                        "TypeIE": "АД",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "a9330618-a26e-4fe9-ab13-b2ce00a14930"
                    },
                    {
                        "Code": "328",
                        "Subjects2": "Кахруманова З.Н.",
                        "TypeIE": "РУ",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "b02f8ea3-f8ba-44af-ae4c-b2ce00a14930"
                    },
                    {
                        "Code": "114",
                        "Subjects2": "Департамент наличного денежного обращения",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент наличного денежного обращения",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "b1fcb3d6-8246-406b-9720-b2ce00a14930"
                    },
                    {
                        "Code": "108",
                        "Subjects2": "Департамент исследований и прогнозирования",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент исследований и прогнозирования",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "b8fa34cf-3539-4ba5-9b19-b2ce00a14930"
                    },
                    {
                        "Code": "327",
                        "Subjects2": "Департамент данных, проектов и процессов",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент данных, проектов и процессов",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "c1477fc2-7853-4cea-aada-b2ce00a14930"
                    },
                    {
                        "Code": "207",
                        "Subjects2": "Дальневосточное ГУ Банка России",
                        "TypeIE": "ТУ",
                        "TypeIE2": "ДГУ",
                        "Addressee": "",
                        "DirSDS": "",
                        "Organization": "",
                        "Id": "c4ce11d7-4f54-4188-a99c-b2ce00a14930"
                    },
                    {
                        "Code": "103",
                        "Subjects2": "Департамент банковского регулирования и аналитики",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент банковского регулирования и аналитики",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "c5d28a45-244c-466f-a9d5-b2ce00a14930"
                    },
                    {
                        "Code": "202",
                        "Subjects2": "Северо-Западное ГУ Банка России",
                        "TypeIE": "ТУ",
                        "TypeIE2": "СЗГУ",
                        "Addressee": "",
                        "DirSDS": "",
                        "Organization": "",
                        "Id": "c88f089e-17f1-402a-bf4d-b2ce00a14930"
                    },
                    {
                        "Code": "134",
                        "Subjects2": "Департамент безопасности Банка России",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент безопасности Банка России",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "c8fc5b0e-ec2f-44a6-a40e-b2ce00a14930"
                    },
                    {
                        "Code": "125",
                        "Subjects2": "Служба финансового мониторинга и валютного контроля",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Служба финансового мониторинга и валютного контроля",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "ca0d5655-89dc-4fb2-b52e-b2ce00a14930"
                    },
                    {
                        "Code": "115",
                        "Subjects2": "Департамент национальной платежной системы",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент национальной платежной системы",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "cceec5f6-d53a-498c-8ba4-b2ce00a14930"
                    },
                    {
                        "Code": "322",
                        "Subjects2": "Департамент организации международных расчетов",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент организации международных расчетов",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "d197d8fd-d696-4b7a-95a7-b2ce00a14930"
                    },
                    {
                        "Code": "106",
                        "Subjects2": "Департамент допуска и прекращения деятельности финансовых организаций",
                        "TypeIE": "ЦА",
                        "TypeIE2": "ДДПДФО",
                        "Addressee": "",
                        "DirSDS": "",
                        "Organization": "",
                        "Id": "d2c8b030-3aa1-4163-aace-b2ce00a14930"
                    },
                    {
                        "Code": "147",
                        "Subjects2": "Департамент статистики",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент статистики",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "d662580c-37ad-4ef2-a105-b2ce00a14930"
                    },
                    {
                        "Code": "321",
                        "Subjects2": "Белов С.В.",
                        "TypeIE": "РУ",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "ddb7ebdb-d3a0-4ae6-84bf-b2ce00a14930"
                    },
                    {
                        "Code": "311",
                        "Subjects2": "Тулин Д.В.",
                        "TypeIE": "РУ",
                        "TypeIE2": "",
                        "Addressee": "Административный департамент",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "e37643fe-5827-4a21-a8cb-b2ce00a14930"
                    },
                    {
                        "Code": "121",
                        "Subjects2": "Департамент стратегического развития финансового рынка",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент стратегического развития финансового рынка",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "ea1934b0-b375-4d28-a4ec-b2ce00a14930"
                    },
                    {
                        "Code": "133",
                        "Subjects2": "Департамент регулирования бухгалтерского учета",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент регулирования бухгалтерского учета",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "eaf0ea6f-cb2d-426c-ae32-b2ce00a14930"
                    },
                    {
                        "Code": "203",
                        "Subjects2": "Южное ГУ Банка России",
                        "TypeIE": "ТУ",
                        "TypeIE2": "ЮГУ",
                        "Addressee": "",
                        "DirSDS": "",
                        "Organization": "",
                        "Id": "f0ca2f21-aeac-4ffe-8477-b2ce00a14930"
                    },
                    {
                        "Code": "144",
                        "Subjects2": "Департамент недвижимости",
                        "TypeIE": "ЦА",
                        "TypeIE2": "",
                        "Addressee": "Департамент недвижимости",
                        "DirSDS": "48_lk",
                        "Organization": "Банк России",
                        "Id": "f67e3bd6-6bd9-4029-91fc-b2ce00a14930"
                    }
                ],
                "PaginationInfo": {
                    "TotalRecords": 58,
                    "TotalPages": 1,
                    "CurrentPage": 1,
                    "PerCurrentPage": 58,
                    "PerNextPage": null,
                    "MaxPerPage": 100
                }
            }
            """,

        _ => $$"""
            {
                "HTTPStatus": 400,
                "ErrorCode": "INCORRECT_PAGE_NUM",
                "ErrorMessage": "Произошла ошибка. Некорректное значение страницы: {{page}}",
                "MoreInfo": {
                    "TotalItems": 58,
                    "TotalPages": 1
                }
            }
            """
    };
}
