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

namespace Diev.Portal5.API.Messages;

/// <summary>
/// Сообщение.
/// 200 OK
/// Headers:
/// Content-Type: application/json; charset=utf-8
/// EPVV-Total: 323
/// EPVV-TotalPages: 4
/// EPVV-CurrentPage: 1
/// EPVV-PerCurrentPage: 100
/// EPVV-PerNextPage: 100
/// [MaxPerPage: 100]
/// </summary>
/// <example>
/// GET https://portal5.cbr.ru/back/rapi2/messages?Task=Zadacha_2-1&Status=registered
/// </example>
/// <example>
/// GET https://portal5.cbr.ru/back/rapi2/messages?Task=Zadacha_3-1&Status=new
/// </example>
/// <example>
/// GET https://portal5.cbr.ru/back/rapi2/messages?Task=Zadacha_3-1&page=1
/// </example>
/// <example>
/// GET https://portal5.cbr.ru/back/rapi2/messages?Task=Zadacha_130&MinDateTime=2023-11-01T00:00:00Z&MaxDateTime=2023-11-07T23:59:59Z
/// </example>
public record Message
(
    /// <summary>
    /// Уникальный идентификатор сообщения.
    /// Устанавливается сервером в ответном сообщении.
    /// Example: "6e16a6ad-018f-4136-a8c6-b088010899bc"
    /// </summary>
    string Id,

    /// <summary>
    /// Идентификатор корреляции сообщения.
    /// Example: null
    /// Example: "1f6158a2-a7a1-4e14-aace-af7a00f65145"
    /// </summary>
    string? CorrelationId,

    /// <summary>
    /// Идентификатор группы сообщений.
    /// Example: null
    /// Example: "a4e5902c-e961-47a3-9670-bd717bcc1749"
    /// </summary>
    string? GroupId,

    /// <summary>
    /// Тип сообщения исходящее (значение: outbox) или входящее (значение: inbox).
    /// Example: "inbox"  // нам входящие
    /// Example: "outbox" // наши исходящие
    /// </summary>
    string Type,

    /// <summary>
    /// Название сообщения (subject).
    /// Example: null
    /// Example: "N 20-2-1/1 от 10/01/2023 (20) Письма Деп-та денежно-кредитной политики"
    /// Example: "Ответ на запрос/предписание (требование)"
    /// Example: "Получение информации об уровне риска ЮЛ/ИП"
    /// Example: "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)"
    /// </summary>
    string? Title,

    /// <summary>
    /// Текст сообщения (body).
    /// Example: null
    /// Example: ""
    /// Example: "предоставление запрошенной информации"
    /// Example: "О данных для расчета размера обязательных резервов"
    /// </summary>
    string? Text,

    /// <summary>
    /// Дата создания сообщения (ГОСТ ISO 8601-2001 по маске «yyyy-MM-dd’T’HH:mm:ss’Z’»).
    /// Example: "2023-09-25T16:03:31Z"
    /// </summary>
    DateTime CreationDate,

    /// <summary>
    /// Дата последнего изменения статуса сообщения (ГОСТ ISO 8601-2001 по маске «yyyy-MM-dd’T’HH:mm:ss’Z’»).
    /// Example: null
    /// Example: "2023-01-09T13:18:19Z"
    /// </summary>
    DateTime? UpdatedDate,

    /// <summary>
    /// Статус сообщения (возможные значения и их описание находится в п.2.4).
    /// 
    /// draft      Черновик: Сообщение с данным статусом создано, но ещё не отправлено.
    /// sent       Отправлено: Сообщение получено сервером.
    /// delivered  Загружено: Сообщение прошло первоначальную проверку.
    /// error      Ошибка: При обработке сообщения возникла ошибка.
    /// processing Принято в обработку: Сообщение передано во внутреннюю систему Банка России.
    /// registered Зарегистрировано: Сообщение зарегистрировано.
    /// rejected   Отклонено: Сообщение успешно дошло до получателя, но было отклонено.
    /// new        Новое: Только для входящих сообщений.Сообщение в данном статусе ещё не почтено Пользователем УИО.
    /// read       Прочитано: Только для входящих сообщений.Сообщение в данном статусе почтено Пользователем УИО.
    /// replied    Отправлен ответ: Только для входящих сообщений.На сообщение в данном статусе направлен ответ.
    /// success    Доставлено: Сообщение успешно размещено в ЛК/Сообщение передано роутером во внутреннюю систему Банка России, от которой не ожидается ответ о регистрации.
    /// </summary>
    string Status,

    /// <summary>
    /// Наименование задачи.
    /// Example: "Zadacha_2-1"
    /// Example: "Zadacha_3-1"
    /// Example: "Zadacha_130"
    /// Example: "Zadacha_137"
    /// Example: "GroupTask_22"
    /// </summary>
    string TaskName,

    /// <summary>
    /// Регистрационный номер.
    /// Example: null
    /// Example: "20-2-1/1"
    /// </summary>
    string? RegNumber,

    /// <summary>
    /// Общий размер сообщения в байтах.
    /// Example: 3241554
    /// </summary>
    long? TotalSize,

    /// <summary>
    /// Отправитель сообщения (необязательное поле, только для сообщений, отправляемых другими Пользователями).
    /// Example: null
    /// Example: {
    /// "Inn": "7831001422",
    /// "Ogrn": "1027800000095",
    /// "Bik": "044030702",
    /// "RegNum": "3194",
    /// "DivisionCode": "0000"
    /// }
    /// </summary>
    Sender? Sender,

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
    IReadOnlyList<Receiver>? Receivers,

    /// <summary>
    /// Файлы включенные в сообщение.
    /// Example: [{
    /// "Id":"d55cdbbb-e41f-4a2a-8967-78e2a6e15701",
    /// "Name":"KYC_20230925.xml.zip.enc",
    /// "Description":null,
    /// "Encrypted":true,
    /// "SignedFile":null,
    /// "Size":3238155,
    /// "RepositoryInfo": [...]
    /// }, ...]
    /// </summary>
    IReadOnlyList<MessageFile> Files,

    /// <summary>
    /// Квитанции, полученные в ответ на сообщение.
    /// Example: []
    /// </summary>
    IReadOnlyList<MessageReceipt>? Receipts
);

/* GET https://portal5.cbr.ru/back/rapi2/messages
Header:
EPVV-Total: 3649
EPVV-TotalPages: 37
EPVV-CurrentPage: 1
EPVV-PerCurrentPage: 100
EPVV-PerNextPage: 100

Body:
[
    {
        "Id": "e72472be-3e25-4535-b874-3e28d382304a",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-06-08T13:21:32Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 772677,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "788c6442-8737-4d7c-bb12-10a4bc93564a",
                "Name": "F1027700466640_080621_Z_0021.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 769929,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e72472be-3e25-4535-b874-3e28d382304a/files/788c6442-8737-4d7c-bb12-10a4bc93564a/download"
                    }
                ]
            },
            {
                "Id": "5adee2dc-ba0c-453f-b4a0-5fa3cf929823",
                "Name": "F1027700466640_080621_Z_0021.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "788c6442-8737-4d7c-bb12-10a4bc93564a",
                "Size": 2748,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e72472be-3e25-4535-b874-3e28d382304a/files/5adee2dc-ba0c-453f-b4a0-5fa3cf929823/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "202cd3c3-c6eb-40a9-8c8c-0415c029b493",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-06-15T12:35:38Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 784253,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "f2f71524-3635-4e2b-a1cb-694f3fa93674",
                "Name": "F1027700466640_150621_Z_0022.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 781505,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/202cd3c3-c6eb-40a9-8c8c-0415c029b493/files/f2f71524-3635-4e2b-a1cb-694f3fa93674/download"
                    }
                ]
            },
            {
                "Id": "46429408-e91d-45e7-9e48-83b59c7e4a6f",
                "Name": "F1027700466640_150621_Z_0022.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "f2f71524-3635-4e2b-a1cb-694f3fa93674",
                "Size": 2748,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/202cd3c3-c6eb-40a9-8c8c-0415c029b493/files/46429408-e91d-45e7-9e48-83b59c7e4a6f/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "7ff4e9fc-ddf7-4dd1-a0aa-0e0d982d2118",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-06-22T11:59:47Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 843969,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "b35e778f-1f1c-481f-8eff-05153b498f53",
                "Name": "F1027700466640_220621_Z_0023.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 841221,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7ff4e9fc-ddf7-4dd1-a0aa-0e0d982d2118/files/b35e778f-1f1c-481f-8eff-05153b498f53/download"
                    }
                ]
            },
            {
                "Id": "38ef8085-5e48-45b9-a583-f32a3dc602cd",
                "Name": "F1027700466640_220621_Z_0023.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "b35e778f-1f1c-481f-8eff-05153b498f53",
                "Size": 2748,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7ff4e9fc-ddf7-4dd1-a0aa-0e0d982d2118/files/38ef8085-5e48-45b9-a583-f32a3dc602cd/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "07595e97-6378-4ef9-86aa-c056e094afa5",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-06-29T11:58:50Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 1413028,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "4c4a39ac-36a6-4c88-8750-0585e4782c1a",
                "Name": "F1027700466640_290621_Z_0024.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 1410280,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/07595e97-6378-4ef9-86aa-c056e094afa5/files/4c4a39ac-36a6-4c88-8750-0585e4782c1a/download"
                    }
                ]
            },
            {
                "Id": "16803f10-bf11-46d5-ba11-b99944d8b49f",
                "Name": "F1027700466640_290621_Z_0024.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "4c4a39ac-36a6-4c88-8750-0585e4782c1a",
                "Size": 2748,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/07595e97-6378-4ef9-86aa-c056e094afa5/files/16803f10-bf11-46d5-ba11-b99944d8b49f/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "c149addd-16c5-47aa-995f-4ecbbb9c243f",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-07-06T12:14:49Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 2645346,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "91314ecc-48c0-43f2-b944-144789c12efe",
                "Name": "F1027700466640_060721_Z_0025.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2642598,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/c149addd-16c5-47aa-995f-4ecbbb9c243f/files/91314ecc-48c0-43f2-b944-144789c12efe/download"
                    }
                ]
            },
            {
                "Id": "73b3a82a-16a0-44c9-84d3-2776fde304c3",
                "Name": "F1027700466640_060721_Z_0025.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "91314ecc-48c0-43f2-b944-144789c12efe",
                "Size": 2748,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/c149addd-16c5-47aa-995f-4ecbbb9c243f/files/73b3a82a-16a0-44c9-84d3-2776fde304c3/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "7cafcfb4-a731-4c83-861d-08865332da36",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-07-13T12:41:18Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 10205967,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "c3332565-bd48-47df-ad87-402ff4b0bdb0",
                "Name": "F1027700466640_130721_Z_0026.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "63a1c9a9-e92a-4a7b-b3ad-724b7dcaa2be",
                "Size": 2748,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7cafcfb4-a731-4c83-861d-08865332da36/files/c3332565-bd48-47df-ad87-402ff4b0bdb0/download"
                    }
                ]
            },
            {
                "Id": "63a1c9a9-e92a-4a7b-b3ad-724b7dcaa2be",
                "Name": "F1027700466640_130721_Z_0026.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 10203219,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7cafcfb4-a731-4c83-861d-08865332da36/files/63a1c9a9-e92a-4a7b-b3ad-724b7dcaa2be/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "3123dc54-9a5c-4b0c-ad58-472753de04d5",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-07-21T11:43:59Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 21225987,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "62b8c6f2-2ddb-4d79-92fd-01cc63ed6a1b",
                "Name": "F1027700466640_200721_Z_0027.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 21223239,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/3123dc54-9a5c-4b0c-ad58-472753de04d5/files/62b8c6f2-2ddb-4d79-92fd-01cc63ed6a1b/download"
                    }
                ]
            },
            {
                "Id": "99228ad1-1304-4fd3-898f-e42a6d131032",
                "Name": "F1027700466640_200721_Z_0027.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "62b8c6f2-2ddb-4d79-92fd-01cc63ed6a1b",
                "Size": 2748,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/3123dc54-9a5c-4b0c-ad58-472753de04d5/files/99228ad1-1304-4fd3-898f-e42a6d131032/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "5b6c35f1-1d1e-4d93-b217-8900dee57585",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-07-27T13:52:02Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 15543683,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "6458e77b-2311-4651-a45d-8b5d9e82e0d1",
                "Name": "F1027700466640_270721_Z_0028.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 15540935,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/5b6c35f1-1d1e-4d93-b217-8900dee57585/files/6458e77b-2311-4651-a45d-8b5d9e82e0d1/download"
                    }
                ]
            },
            {
                "Id": "2478489f-6b6a-450f-9d31-f74f2b057332",
                "Name": "F1027700466640_270721_Z_0028.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "6458e77b-2311-4651-a45d-8b5d9e82e0d1",
                "Size": 2748,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/5b6c35f1-1d1e-4d93-b217-8900dee57585/files/2478489f-6b6a-450f-9d31-f74f2b057332/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "884a1ada-5c0d-4e72-95e6-62551c062135",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-08-03T14:37:03Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 9493952,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "81fa01a0-03be-49c2-8ea2-224e4e71e06b",
                "Name": "F1027700466640_030821_Z_0029.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "1fea2944-2647-4448-9285-294b1877e7f2",
                "Size": 2748,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/884a1ada-5c0d-4e72-95e6-62551c062135/files/81fa01a0-03be-49c2-8ea2-224e4e71e06b/download"
                    }
                ]
            },
            {
                "Id": "1fea2944-2647-4448-9285-294b1877e7f2",
                "Name": "F1027700466640_030821_Z_0029.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9491204,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/884a1ada-5c0d-4e72-95e6-62551c062135/files/1fea2944-2647-4448-9285-294b1877e7f2/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "85421579-0b68-4efc-9988-0bf62805d166",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-08-10T13:51:11Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 6604854,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "6a1d2a79-4d38-45fc-8f1f-020a0c5ca54c",
                "Name": "F1027700466640_100821_Z_0030.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "94fa3ea6-8342-4cd3-96ca-605ad1610b94",
                "Size": 2748,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/85421579-0b68-4efc-9988-0bf62805d166/files/6a1d2a79-4d38-45fc-8f1f-020a0c5ca54c/download"
                    }
                ]
            },
            {
                "Id": "94fa3ea6-8342-4cd3-96ca-605ad1610b94",
                "Name": "F1027700466640_100821_Z_0030.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 6602106,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/85421579-0b68-4efc-9988-0bf62805d166/files/94fa3ea6-8342-4cd3-96ca-605ad1610b94/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "39126e3c-4d23-4f58-9399-d7d26edf5cab",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-08-17T12:37:52Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 1065370,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "bba6918e-fee5-4a3a-baa5-5a103dbd9afb",
                "Name": "F1027700466640_170821_Z_0031.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 1062622,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/39126e3c-4d23-4f58-9399-d7d26edf5cab/files/bba6918e-fee5-4a3a-baa5-5a103dbd9afb/download"
                    }
                ]
            },
            {
                "Id": "b4cb4bd0-ce0d-4947-b29b-e9e76920ec81",
                "Name": "F1027700466640_170821_Z_0031.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "bba6918e-fee5-4a3a-baa5-5a103dbd9afb",
                "Size": 2748,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/39126e3c-4d23-4f58-9399-d7d26edf5cab/files/b4cb4bd0-ce0d-4947-b29b-e9e76920ec81/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "e0a416a7-604f-4247-bcef-2a6a539bea80",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-08-24T15:15:43Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 880129,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "aa8bee90-fd2c-417c-8f59-389a90be2ebe",
                "Name": "F1027700466640_240821_Z_0032.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 877379,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e0a416a7-604f-4247-bcef-2a6a539bea80/files/aa8bee90-fd2c-417c-8f59-389a90be2ebe/download"
                    }
                ]
            },
            {
                "Id": "6b50c3e1-3a38-4cf5-ba2f-5e33fdc9fe40",
                "Name": "F1027700466640_240821_Z_0032.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "aa8bee90-fd2c-417c-8f59-389a90be2ebe",
                "Size": 2750,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e0a416a7-604f-4247-bcef-2a6a539bea80/files/6b50c3e1-3a38-4cf5-ba2f-5e33fdc9fe40/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "f730b29a-5cac-4299-98ec-0e5c3df14c25",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-08-26T07:57:11Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 846165,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "e9e154ec-0a9c-4fd1-815f-ef7d75b08010",
                "Name": "F1027700466640_260821_P_0034.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 843415,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/f730b29a-5cac-4299-98ec-0e5c3df14c25/files/e9e154ec-0a9c-4fd1-815f-ef7d75b08010/download"
                    }
                ]
            },
            {
                "Id": "a98103cb-c905-4959-b255-efc276e330fe",
                "Name": "F1027700466640_260821_P_0034.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "e9e154ec-0a9c-4fd1-815f-ef7d75b08010",
                "Size": 2750,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/f730b29a-5cac-4299-98ec-0e5c3df14c25/files/a98103cb-c905-4959-b255-efc276e330fe/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "00a03d25-0da1-4802-9cdf-801e52fb26fb",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-08-26T14:39:28Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 850629,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "eb14f590-dae1-4e6d-b176-50a6482214ad",
                "Name": "F1027700466640_260821_P_0035.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 847879,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/00a03d25-0da1-4802-9cdf-801e52fb26fb/files/eb14f590-dae1-4e6d-b176-50a6482214ad/download"
                    }
                ]
            },
            {
                "Id": "2604ad55-c351-44c1-a3fa-ab2c90c23095",
                "Name": "F1027700466640_260821_P_0035.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "eb14f590-dae1-4e6d-b176-50a6482214ad",
                "Size": 2750,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/00a03d25-0da1-4802-9cdf-801e52fb26fb/files/2604ad55-c351-44c1-a3fa-ab2c90c23095/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "320914c1-1988-4d98-84a8-3db024cf5f73",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": null,
        "CreationDate": "2021-08-31T14:32:05Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 902087,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "f89fbd39-6cae-41b0-8266-91d9321bb7f3",
                "Name": "F1027700466640_310821_Z_0036.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 899337,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/320914c1-1988-4d98-84a8-3db024cf5f73/files/f89fbd39-6cae-41b0-8266-91d9321bb7f3/download"
                    }
                ]
            },
            {
                "Id": "d7f5de2f-6d7b-4cdd-9124-f21b8e5396ed",
                "Name": "F1027700466640_310821_Z_0036.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "f89fbd39-6cae-41b0-8266-91d9321bb7f3",
                "Size": 2750,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/320914c1-1988-4d98-84a8-3db024cf5f73/files/d7f5de2f-6d7b-4cdd-9124-f21b8e5396ed/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "cfec6dda-2b6d-4465-8e3a-aebb00d5e024",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": "",
        "CreationDate": "2022-06-21T14:07:13Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 1028011,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "d425a00b-6455-4629-b6e8-67ae7652a1a0",
                "Name": "F1027700466640_210622_Z_0025.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "8cb771e7-1056-477e-b178-cfac5841729a",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/cfec6dda-2b6d-4465-8e3a-aebb00d5e024/files/d425a00b-6455-4629-b6e8-67ae7652a1a0/download"
                    }
                ]
            },
            {
                "Id": "8cb771e7-1056-477e-b178-cfac5841729a",
                "Name": "F1027700466640_210622_Z_0025.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 1024750,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/cfec6dda-2b6d-4465-8e3a-aebb00d5e024/files/8cb771e7-1056-477e-b178-cfac5841729a/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "1e668084-2e77-4da2-99c9-af3200eae654",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК",
        "Text": "",
        "CreationDate": "2022-10-18T14:15:19Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_54",
        "RegNumber": null,
        "TotalSize": 903265,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "2a78b138-1537-4937-9f4c-5831f59eb67e",
                "Name": "F1027700466640_181022_Z_0045.xml.sig.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 900004,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1e668084-2e77-4da2-99c9-af3200eae654/files/2a78b138-1537-4937-9f4c-5831f59eb67e/download"
                    }
                ]
            },
            {
                "Id": "89fb7264-895f-416d-8c9d-b47de08e6522",
                "Name": "F1027700466640_181022_Z_0045.xml.sig.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "2a78b138-1537-4937-9f4c-5831f59eb67e",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1e668084-2e77-4da2-99c9-af3200eae654/files/89fb7264-895f-416d-8c9d-b47de08e6522/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "4825614f-e641-498b-8670-af7d0128a289",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-01T18:00:00Z",
        "UpdatedDate": "2023-01-01T18:03:18Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00564752",
        "TotalSize": 13369,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "719182ec-8d0f-4419-a4e2-af7d0128a285",
                "Name": "KYCCL_7831001422_3194_20230101_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9497,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/files/719182ec-8d0f-4419-a4e2-af7d0128a285/download"
                    }
                ]
            },
            {
                "Id": "6548c720-2de4-442f-8edf-af7d0128a287",
                "Name": "KYCCL_7831001422_3194_20230101_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "719182ec-8d0f-4419-a4e2-af7d0128a285",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/files/6548c720-2de4-442f-8edf-af7d0128a287/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "2438f1b9-159d-41a1-8309-af7d0128a3b0",
                "ReceiveTime": "2023-01-01T18:00:01Z",
                "StatusTime": "2023-01-01T18:00:01Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "91595e36-555b-42c7-ad71-af7d0128b528",
                "ReceiveTime": "2023-01-01T18:00:16Z",
                "StatusTime": "2023-01-01T18:00:12Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "32138df1-2afc-41f4-ad89-09f30c51b41f",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/91595e36-555b-42c7-ad71-af7d0128b528/files/32138df1-2afc-41f4-ad89-09f30c51b41f/download"
                            }
                        ]
                    },
                    {
                        "Id": "2bfcf7b2-ed5c-43f0-9fc2-27a68f03fdbd",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "21d3ec52-a0d1-4aa0-aee5-754b2c269433",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/91595e36-555b-42c7-ad71-af7d0128b528/files/2bfcf7b2-ed5c-43f0-9fc2-27a68f03fdbd/download"
                            }
                        ]
                    },
                    {
                        "Id": "21d3ec52-a0d1-4aa0-aee5-754b2c269433",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/91595e36-555b-42c7-ad71-af7d0128b528/files/21d3ec52-a0d1-4aa0-aee5-754b2c269433/download"
                            }
                        ]
                    },
                    {
                        "Id": "15734f6f-2912-4e15-b609-9804d1a18ad2",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "32138df1-2afc-41f4-ad89-09f30c51b41f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/91595e36-555b-42c7-ad71-af7d0128b528/files/15734f6f-2912-4e15-b609-9804d1a18ad2/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "3dcda650-f9c9-4534-b78f-af7d0128bd98",
                "ReceiveTime": "2023-01-01T18:00:23Z",
                "StatusTime": "2023-01-01T18:00:16Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "79e13183-a736-4575-8e2a-5559e137ffae",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/3dcda650-f9c9-4534-b78f-af7d0128bd98/files/79e13183-a736-4575-8e2a-5559e137ffae/download"
                            }
                        ]
                    },
                    {
                        "Id": "6464c56f-b0f7-4efb-9523-885c3e7b8f0b",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/3dcda650-f9c9-4534-b78f-af7d0128bd98/files/6464c56f-b0f7-4efb-9523-885c3e7b8f0b/download"
                            }
                        ]
                    },
                    {
                        "Id": "60412d72-df08-4820-b179-af5a2ee4bbeb",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "6464c56f-b0f7-4efb-9523-885c3e7b8f0b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/3dcda650-f9c9-4534-b78f-af7d0128bd98/files/60412d72-df08-4820-b179-af5a2ee4bbeb/download"
                            }
                        ]
                    },
                    {
                        "Id": "82bd7cb5-0307-4d2b-b735-d4ed49e2b0dd",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "79e13183-a736-4575-8e2a-5559e137ffae",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/3dcda650-f9c9-4534-b78f-af7d0128bd98/files/82bd7cb5-0307-4d2b-b735-d4ed49e2b0dd/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "6f4ca825-3968-4e20-a6ee-af7d01298a2f",
                "ReceiveTime": "2023-01-01T18:03:18Z",
                "StatusTime": "2023-01-01T18:02:27Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "cf0869cd-7e34-464f-847c-2b2dfab3ab39",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/6f4ca825-3968-4e20-a6ee-af7d01298a2f/files/cf0869cd-7e34-464f-847c-2b2dfab3ab39/download"
                            }
                        ]
                    },
                    {
                        "Id": "59f25e7f-85bf-4ec2-9c24-69328ce84201",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "cf0869cd-7e34-464f-847c-2b2dfab3ab39",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/6f4ca825-3968-4e20-a6ee-af7d01298a2f/files/59f25e7f-85bf-4ec2-9c24-69328ce84201/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "7568ff65-0bf1-4f10-b3a4-af7e0128a0b2",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-02T17:59:59Z",
        "UpdatedDate": "2023-01-02T18:04:30Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00564874",
        "TotalSize": 13370,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "53c03d24-52fb-4f5b-8565-af7e0128a0ae",
                "Name": "KYCCL_7831001422_3194_20230102_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9498,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7568ff65-0bf1-4f10-b3a4-af7e0128a0b2/files/53c03d24-52fb-4f5b-8565-af7e0128a0ae/download"
                    }
                ]
            },
            {
                "Id": "bf8d3db6-f067-47dd-be46-af7e0128a0b0",
                "Name": "KYCCL_7831001422_3194_20230102_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "53c03d24-52fb-4f5b-8565-af7e0128a0ae",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7568ff65-0bf1-4f10-b3a4-af7e0128a0b2/files/bf8d3db6-f067-47dd-be46-af7e0128a0b0/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "16692145-4193-455d-bb7f-af7e0128a203",
                "ReceiveTime": "2023-01-02T18:00:00Z",
                "StatusTime": "2023-01-02T18:00:00Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "7f746cf6-6393-412b-8aac-af7e0128b35f",
                "ReceiveTime": "2023-01-02T18:00:15Z",
                "StatusTime": "2023-01-02T18:00:11Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "e1282eb5-bb87-4741-8d19-1a339503342e",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "7c36e138-1cd9-4507-927b-2c8c5e406bf8",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7568ff65-0bf1-4f10-b3a4-af7e0128a0b2/receipts/7f746cf6-6393-412b-8aac-af7e0128b35f/files/e1282eb5-bb87-4741-8d19-1a339503342e/download"
                            }
                        ]
                    },
                    {
                        "Id": "7c36e138-1cd9-4507-927b-2c8c5e406bf8",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7568ff65-0bf1-4f10-b3a4-af7e0128a0b2/receipts/7f746cf6-6393-412b-8aac-af7e0128b35f/files/7c36e138-1cd9-4507-927b-2c8c5e406bf8/download"
                            }
                        ]
                    },
                    {
                        "Id": "506bbf0c-be5b-4102-a059-809a987b670d",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7568ff65-0bf1-4f10-b3a4-af7e0128a0b2/receipts/7f746cf6-6393-412b-8aac-af7e0128b35f/files/506bbf0c-be5b-4102-a059-809a987b670d/download"
                            }
                        ]
                    },
                    {
                        "Id": "a20b7419-95ee-4ab4-bfa0-f966f45f6e9a",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "506bbf0c-be5b-4102-a059-809a987b670d",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7568ff65-0bf1-4f10-b3a4-af7e0128a0b2/receipts/7f746cf6-6393-412b-8aac-af7e0128b35f/files/a20b7419-95ee-4ab4-bfa0-f966f45f6e9a/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "e0d2d15c-cbe9-4331-949a-af7e0128bb34",
                "ReceiveTime": "2023-01-02T18:00:21Z",
                "StatusTime": "2023-01-02T18:00:14Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "d4510afe-e48f-467a-8457-60d872b3cff3",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "60ef869b-966b-4626-a0ca-e3198953c26f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7568ff65-0bf1-4f10-b3a4-af7e0128a0b2/receipts/e0d2d15c-cbe9-4331-949a-af7e0128bb34/files/d4510afe-e48f-467a-8457-60d872b3cff3/download"
                            }
                        ]
                    },
                    {
                        "Id": "9bfe1cff-5480-4ad0-b19d-8f7b1dd46b33",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7568ff65-0bf1-4f10-b3a4-af7e0128a0b2/receipts/e0d2d15c-cbe9-4331-949a-af7e0128bb34/files/9bfe1cff-5480-4ad0-b19d-8f7b1dd46b33/download"
                            }
                        ]
                    },
                    {
                        "Id": "60ef869b-966b-4626-a0ca-e3198953c26f",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7568ff65-0bf1-4f10-b3a4-af7e0128a0b2/receipts/e0d2d15c-cbe9-4331-949a-af7e0128bb34/files/60ef869b-966b-4626-a0ca-e3198953c26f/download"
                            }
                        ]
                    },
                    {
                        "Id": "3162452c-af33-4ca5-8cf6-ee81267554aa",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "9bfe1cff-5480-4ad0-b19d-8f7b1dd46b33",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7568ff65-0bf1-4f10-b3a4-af7e0128a0b2/receipts/e0d2d15c-cbe9-4331-949a-af7e0128bb34/files/3162452c-af33-4ca5-8cf6-ee81267554aa/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "f41b820f-e9aa-41a0-9e2b-af7e0129de55",
                "ReceiveTime": "2023-01-02T18:04:30Z",
                "StatusTime": "2023-01-02T18:03:13Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "f0528233-d975-4710-94da-ea4891d735aa",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "372743d7-bd1c-497f-9be1-f40a93bbb328",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7568ff65-0bf1-4f10-b3a4-af7e0128a0b2/receipts/f41b820f-e9aa-41a0-9e2b-af7e0129de55/files/f0528233-d975-4710-94da-ea4891d735aa/download"
                            }
                        ]
                    },
                    {
                        "Id": "372743d7-bd1c-497f-9be1-f40a93bbb328",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7568ff65-0bf1-4f10-b3a4-af7e0128a0b2/receipts/f41b820f-e9aa-41a0-9e2b-af7e0129de55/files/372743d7-bd1c-497f-9be1-f40a93bbb328/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "7e057478-d5f6-4a5f-a9ab-af7f01289fc9",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-03T17:59:58Z",
        "UpdatedDate": "2023-01-03T18:02:58Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00564974",
        "TotalSize": 13370,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "5b51abaa-ab29-4e4d-a3c1-af7f01289fc5",
                "Name": "KYCCL_7831001422_3194_20230103_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9498,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7e057478-d5f6-4a5f-a9ab-af7f01289fc9/files/5b51abaa-ab29-4e4d-a3c1-af7f01289fc5/download"
                    }
                ]
            },
            {
                "Id": "b6ab1ca6-a63a-458b-8b9b-af7f01289fc7",
                "Name": "KYCCL_7831001422_3194_20230103_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "5b51abaa-ab29-4e4d-a3c1-af7f01289fc5",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7e057478-d5f6-4a5f-a9ab-af7f01289fc9/files/b6ab1ca6-a63a-458b-8b9b-af7f01289fc7/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "aa666403-3c8d-4e12-ad65-af7f0128a11b",
                "ReceiveTime": "2023-01-03T17:59:59Z",
                "StatusTime": "2023-01-03T17:59:59Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "431fb001-a685-4b02-affb-af7f0128b1fc",
                "ReceiveTime": "2023-01-03T18:00:14Z",
                "StatusTime": "2023-01-03T18:00:10Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "9f6ae13c-93e4-43e5-8d74-0ec22e1f0ff9",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "ce9a7a59-ffc1-4bbb-a878-d22cdb55bb34",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7e057478-d5f6-4a5f-a9ab-af7f01289fc9/receipts/431fb001-a685-4b02-affb-af7f0128b1fc/files/9f6ae13c-93e4-43e5-8d74-0ec22e1f0ff9/download"
                            }
                        ]
                    },
                    {
                        "Id": "e3da0b63-f6fb-42de-9426-2451165c5418",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7e057478-d5f6-4a5f-a9ab-af7f01289fc9/receipts/431fb001-a685-4b02-affb-af7f0128b1fc/files/e3da0b63-f6fb-42de-9426-2451165c5418/download"
                            }
                        ]
                    },
                    {
                        "Id": "405e220d-400f-4d54-9cd4-aa33583ff4ea",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "e3da0b63-f6fb-42de-9426-2451165c5418",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7e057478-d5f6-4a5f-a9ab-af7f01289fc9/receipts/431fb001-a685-4b02-affb-af7f0128b1fc/files/405e220d-400f-4d54-9cd4-aa33583ff4ea/download"
                            }
                        ]
                    },
                    {
                        "Id": "ce9a7a59-ffc1-4bbb-a878-d22cdb55bb34",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7e057478-d5f6-4a5f-a9ab-af7f01289fc9/receipts/431fb001-a685-4b02-affb-af7f0128b1fc/files/ce9a7a59-ffc1-4bbb-a878-d22cdb55bb34/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "86095df3-4b7a-48c2-aa04-af7f0128ba75",
                "ReceiveTime": "2023-01-03T18:00:21Z",
                "StatusTime": "2023-01-03T18:00:13Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "7251b33f-314b-49cc-9394-3cff46e65f26",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "fba322a3-7b4b-45e9-a622-d801456ce586",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7e057478-d5f6-4a5f-a9ab-af7f01289fc9/receipts/86095df3-4b7a-48c2-aa04-af7f0128ba75/files/7251b33f-314b-49cc-9394-3cff46e65f26/download"
                            }
                        ]
                    },
                    {
                        "Id": "c44582d7-c2ae-4eea-9afe-72e1df84cc31",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7e057478-d5f6-4a5f-a9ab-af7f01289fc9/receipts/86095df3-4b7a-48c2-aa04-af7f0128ba75/files/c44582d7-c2ae-4eea-9afe-72e1df84cc31/download"
                            }
                        ]
                    },
                    {
                        "Id": "fba322a3-7b4b-45e9-a622-d801456ce586",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7e057478-d5f6-4a5f-a9ab-af7f01289fc9/receipts/86095df3-4b7a-48c2-aa04-af7f0128ba75/files/fba322a3-7b4b-45e9-a622-d801456ce586/download"
                            }
                        ]
                    },
                    {
                        "Id": "2cd6306a-52e4-4b75-8031-ee79d3a40ab7",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "c44582d7-c2ae-4eea-9afe-72e1df84cc31",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7e057478-d5f6-4a5f-a9ab-af7f01289fc9/receipts/86095df3-4b7a-48c2-aa04-af7f0128ba75/files/2cd6306a-52e4-4b75-8031-ee79d3a40ab7/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "d77e0a23-aad8-460e-9d2d-af7f012972c2",
                "ReceiveTime": "2023-01-03T18:02:58Z",
                "StatusTime": "2023-01-03T18:02:12Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "a5d40e9f-df0a-4cca-9b8b-73058fa3d84f",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7e057478-d5f6-4a5f-a9ab-af7f01289fc9/receipts/d77e0a23-aad8-460e-9d2d-af7f012972c2/files/a5d40e9f-df0a-4cca-9b8b-73058fa3d84f/download"
                            }
                        ]
                    },
                    {
                        "Id": "e4ac3f5c-f1e2-49f2-aeef-79988eb4e019",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "a5d40e9f-df0a-4cca-9b8b-73058fa3d84f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7e057478-d5f6-4a5f-a9ab-af7f01289fc9/receipts/d77e0a23-aad8-460e-9d2d-af7f012972c2/files/e4ac3f5c-f1e2-49f2-aeef-79988eb4e019/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "134cf201-77f0-4878-a101-af8001289deb",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-04T17:59:56Z",
        "UpdatedDate": "2023-01-04T18:02:46Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00564993",
        "TotalSize": 13368,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "75064130-580c-47ec-83cb-af8001289de7",
                "Name": "KYCCL_7831001422_3194_20230104_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9496,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/134cf201-77f0-4878-a101-af8001289deb/files/75064130-580c-47ec-83cb-af8001289de7/download"
                    }
                ]
            },
            {
                "Id": "f27dd6cd-1afd-4496-a400-af8001289de9",
                "Name": "KYCCL_7831001422_3194_20230104_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "75064130-580c-47ec-83cb-af8001289de7",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/134cf201-77f0-4878-a101-af8001289deb/files/f27dd6cd-1afd-4496-a400-af8001289de9/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "d68a9211-2b37-4e68-8630-af8001289fae",
                "ReceiveTime": "2023-01-04T17:59:58Z",
                "StatusTime": "2023-01-04T17:59:58Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "a9b4d541-b342-40d9-bc1b-af800128b06f",
                "ReceiveTime": "2023-01-04T18:00:12Z",
                "StatusTime": "2023-01-04T18:00:08Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "95b14598-566b-474d-ad40-5182f1b78e31",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "b7d6da2b-417c-4cc1-9698-a0cf51ce3f8c",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/134cf201-77f0-4878-a101-af8001289deb/receipts/a9b4d541-b342-40d9-bc1b-af800128b06f/files/95b14598-566b-474d-ad40-5182f1b78e31/download"
                            }
                        ]
                    },
                    {
                        "Id": "b7d6da2b-417c-4cc1-9698-a0cf51ce3f8c",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/134cf201-77f0-4878-a101-af8001289deb/receipts/a9b4d541-b342-40d9-bc1b-af800128b06f/files/b7d6da2b-417c-4cc1-9698-a0cf51ce3f8c/download"
                            }
                        ]
                    },
                    {
                        "Id": "f13f075e-a1c4-4a16-9e7a-a1c320d03efa",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/134cf201-77f0-4878-a101-af8001289deb/receipts/a9b4d541-b342-40d9-bc1b-af800128b06f/files/f13f075e-a1c4-4a16-9e7a-a1c320d03efa/download"
                            }
                        ]
                    },
                    {
                        "Id": "7a5f149d-43f1-49c6-b8bf-d813c590b033",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f13f075e-a1c4-4a16-9e7a-a1c320d03efa",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/134cf201-77f0-4878-a101-af8001289deb/receipts/a9b4d541-b342-40d9-bc1b-af800128b06f/files/7a5f149d-43f1-49c6-b8bf-d813c590b033/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "15526202-66c1-4d11-ae4d-af800128b81b",
                "ReceiveTime": "2023-01-04T18:00:19Z",
                "StatusTime": "2023-01-04T18:00:12Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "4b9c479f-799c-4b8e-b6ce-056a38e86eef",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/134cf201-77f0-4878-a101-af8001289deb/receipts/15526202-66c1-4d11-ae4d-af800128b81b/files/4b9c479f-799c-4b8e-b6ce-056a38e86eef/download"
                            }
                        ]
                    },
                    {
                        "Id": "f491955d-b744-436a-937a-229228956dc8",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/134cf201-77f0-4878-a101-af8001289deb/receipts/15526202-66c1-4d11-ae4d-af800128b81b/files/f491955d-b744-436a-937a-229228956dc8/download"
                            }
                        ]
                    },
                    {
                        "Id": "34645462-ed69-4720-afdd-978c2f1cbaa1",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "4b9c479f-799c-4b8e-b6ce-056a38e86eef",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/134cf201-77f0-4878-a101-af8001289deb/receipts/15526202-66c1-4d11-ae4d-af800128b81b/files/34645462-ed69-4720-afdd-978c2f1cbaa1/download"
                            }
                        ]
                    },
                    {
                        "Id": "23980024-306f-49be-8397-e413c9dfc788",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f491955d-b744-436a-937a-229228956dc8",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/134cf201-77f0-4878-a101-af8001289deb/receipts/15526202-66c1-4d11-ae4d-af800128b81b/files/23980024-306f-49be-8397-e413c9dfc788/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "20ef47a6-7924-4b49-abdc-af800129642e",
                "ReceiveTime": "2023-01-04T18:02:46Z",
                "StatusTime": "2023-01-04T18:02:01Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "4b950a0b-bb7c-4696-b0c3-780c1790818c",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "bdda323e-74ba-47b5-b1a7-c6129bd75983",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/134cf201-77f0-4878-a101-af8001289deb/receipts/20ef47a6-7924-4b49-abdc-af800129642e/files/4b950a0b-bb7c-4696-b0c3-780c1790818c/download"
                            }
                        ]
                    },
                    {
                        "Id": "bdda323e-74ba-47b5-b1a7-c6129bd75983",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/134cf201-77f0-4878-a101-af8001289deb/receipts/20ef47a6-7924-4b49-abdc-af800129642e/files/bdda323e-74ba-47b5-b1a7-c6129bd75983/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "c4a3a876-f317-4355-8b9a-af8101289d1c",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-05T17:59:56Z",
        "UpdatedDate": "2023-01-05T18:03:52Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00565067",
        "TotalSize": 13368,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "3b6ff7dd-8bb5-4f01-9321-af8101289d18",
                "Name": "KYCCL_7831001422_3194_20230105_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9496,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/c4a3a876-f317-4355-8b9a-af8101289d1c/files/3b6ff7dd-8bb5-4f01-9321-af8101289d18/download"
                    }
                ]
            },
            {
                "Id": "caf67885-e600-4b6e-9447-af8101289d1a",
                "Name": "KYCCL_7831001422_3194_20230105_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "3b6ff7dd-8bb5-4f01-9321-af8101289d18",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/c4a3a876-f317-4355-8b9a-af8101289d1c/files/caf67885-e600-4b6e-9447-af8101289d1a/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "2cd44730-423e-4f33-90f1-af8101289e8c",
                "ReceiveTime": "2023-01-05T17:59:57Z",
                "StatusTime": "2023-01-05T17:59:57Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "b3fe45fd-297d-4670-aa44-af810128aed7",
                "ReceiveTime": "2023-01-05T18:00:11Z",
                "StatusTime": "2023-01-05T18:00:07Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "e50658d8-139d-4d1d-ba85-00452ce82865",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "866e8aa4-1f9e-4c6c-a240-217236433878",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/c4a3a876-f317-4355-8b9a-af8101289d1c/receipts/b3fe45fd-297d-4670-aa44-af810128aed7/files/e50658d8-139d-4d1d-ba85-00452ce82865/download"
                            }
                        ]
                    },
                    {
                        "Id": "866e8aa4-1f9e-4c6c-a240-217236433878",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/c4a3a876-f317-4355-8b9a-af8101289d1c/receipts/b3fe45fd-297d-4670-aa44-af810128aed7/files/866e8aa4-1f9e-4c6c-a240-217236433878/download"
                            }
                        ]
                    },
                    {
                        "Id": "de6fdad2-ea5c-4b84-af46-39a2074a13ab",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/c4a3a876-f317-4355-8b9a-af8101289d1c/receipts/b3fe45fd-297d-4670-aa44-af810128aed7/files/de6fdad2-ea5c-4b84-af46-39a2074a13ab/download"
                            }
                        ]
                    },
                    {
                        "Id": "2c5ad80b-c94e-478c-923f-d8fe4b052f4e",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "de6fdad2-ea5c-4b84-af46-39a2074a13ab",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/c4a3a876-f317-4355-8b9a-af8101289d1c/receipts/b3fe45fd-297d-4670-aa44-af810128aed7/files/2c5ad80b-c94e-478c-923f-d8fe4b052f4e/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "2491d067-1dc4-4821-b096-af810128b76d",
                "ReceiveTime": "2023-01-05T18:00:18Z",
                "StatusTime": "2023-01-05T18:00:11Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "6331adb8-4807-4970-8cb3-097d54a4d2c8",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "247e7da3-5d70-4990-aebb-4b63cd1cc13c",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/c4a3a876-f317-4355-8b9a-af8101289d1c/receipts/2491d067-1dc4-4821-b096-af810128b76d/files/6331adb8-4807-4970-8cb3-097d54a4d2c8/download"
                            }
                        ]
                    },
                    {
                        "Id": "247e7da3-5d70-4990-aebb-4b63cd1cc13c",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/c4a3a876-f317-4355-8b9a-af8101289d1c/receipts/2491d067-1dc4-4821-b096-af810128b76d/files/247e7da3-5d70-4990-aebb-4b63cd1cc13c/download"
                            }
                        ]
                    },
                    {
                        "Id": "2da43ad3-c1b2-434d-88dc-90e2c9004ebd",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "681aca97-f9d0-4ca9-a1ed-ceeb0f2937f8",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/c4a3a876-f317-4355-8b9a-af8101289d1c/receipts/2491d067-1dc4-4821-b096-af810128b76d/files/2da43ad3-c1b2-434d-88dc-90e2c9004ebd/download"
                            }
                        ]
                    },
                    {
                        "Id": "681aca97-f9d0-4ca9-a1ed-ceeb0f2937f8",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/c4a3a876-f317-4355-8b9a-af8101289d1c/receipts/2491d067-1dc4-4821-b096-af810128b76d/files/681aca97-f9d0-4ca9-a1ed-ceeb0f2937f8/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "43f30f71-c1e4-471c-a0fc-af810129b173",
                "ReceiveTime": "2023-01-05T18:03:52Z",
                "StatusTime": "2023-01-05T18:02:12Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "b36a637e-3a97-4b32-8150-3d5debeedd57",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f07d9f06-9daf-41ce-83b5-4a7bf44642f0",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/c4a3a876-f317-4355-8b9a-af8101289d1c/receipts/43f30f71-c1e4-471c-a0fc-af810129b173/files/b36a637e-3a97-4b32-8150-3d5debeedd57/download"
                            }
                        ]
                    },
                    {
                        "Id": "f07d9f06-9daf-41ce-83b5-4a7bf44642f0",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/c4a3a876-f317-4355-8b9a-af8101289d1c/receipts/43f30f71-c1e4-471c-a0fc-af810129b173/files/f07d9f06-9daf-41ce-83b5-4a7bf44642f0/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "32bad0ce-a2d9-4a78-ad5f-af8201289b69",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-06T17:59:54Z",
        "UpdatedDate": "2023-01-06T18:03:03Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00565150",
        "TotalSize": 13370,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "91d26079-2af4-467c-a536-af8201289b65",
                "Name": "KYCCL_7831001422_3194_20230106_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9498,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/32bad0ce-a2d9-4a78-ad5f-af8201289b69/files/91d26079-2af4-467c-a536-af8201289b65/download"
                    }
                ]
            },
            {
                "Id": "2f26ca98-a71c-4f23-bcc9-af8201289b67",
                "Name": "KYCCL_7831001422_3194_20230106_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "91d26079-2af4-467c-a536-af8201289b65",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/32bad0ce-a2d9-4a78-ad5f-af8201289b69/files/2f26ca98-a71c-4f23-bcc9-af8201289b67/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "0cdadc27-a67a-4041-bd3f-af8201289ceb",
                "ReceiveTime": "2023-01-06T17:59:56Z",
                "StatusTime": "2023-01-06T17:59:56Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "4920a991-d426-452e-82bb-af820128aeb7",
                "ReceiveTime": "2023-01-06T18:00:11Z",
                "StatusTime": "2023-01-06T18:00:07Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "4689ce03-3835-4cd9-b557-233db646380d",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/32bad0ce-a2d9-4a78-ad5f-af8201289b69/receipts/4920a991-d426-452e-82bb-af820128aeb7/files/4689ce03-3835-4cd9-b557-233db646380d/download"
                            }
                        ]
                    },
                    {
                        "Id": "128fdf45-9d92-4e18-b167-3eb8e13816dc",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "4689ce03-3835-4cd9-b557-233db646380d",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/32bad0ce-a2d9-4a78-ad5f-af8201289b69/receipts/4920a991-d426-452e-82bb-af820128aeb7/files/128fdf45-9d92-4e18-b167-3eb8e13816dc/download"
                            }
                        ]
                    },
                    {
                        "Id": "148807c1-473e-4f03-a007-569dc98bd66e",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "140fca86-d416-4e4c-a768-956c38b2542b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/32bad0ce-a2d9-4a78-ad5f-af8201289b69/receipts/4920a991-d426-452e-82bb-af820128aeb7/files/148807c1-473e-4f03-a007-569dc98bd66e/download"
                            }
                        ]
                    },
                    {
                        "Id": "140fca86-d416-4e4c-a768-956c38b2542b",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/32bad0ce-a2d9-4a78-ad5f-af8201289b69/receipts/4920a991-d426-452e-82bb-af820128aeb7/files/140fca86-d416-4e4c-a768-956c38b2542b/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "abaf9c96-d7d8-4b7c-b41f-af820128b75c",
                "ReceiveTime": "2023-01-06T18:00:18Z",
                "StatusTime": "2023-01-06T18:00:10Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "f218d8d7-744e-40ec-9af9-6b9fdeb3d66f",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/32bad0ce-a2d9-4a78-ad5f-af8201289b69/receipts/abaf9c96-d7d8-4b7c-b41f-af820128b75c/files/f218d8d7-744e-40ec-9af9-6b9fdeb3d66f/download"
                            }
                        ]
                    },
                    {
                        "Id": "62f8c9dc-4717-44e4-9ace-9763239b3dfd",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f218d8d7-744e-40ec-9af9-6b9fdeb3d66f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/32bad0ce-a2d9-4a78-ad5f-af8201289b69/receipts/abaf9c96-d7d8-4b7c-b41f-af820128b75c/files/62f8c9dc-4717-44e4-9ace-9763239b3dfd/download"
                            }
                        ]
                    },
                    {
                        "Id": "8302a85d-8e6f-4ce4-a58b-e3fdeb4ea34d",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f8901590-437d-49c2-9a8f-efd3efb0fee1",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/32bad0ce-a2d9-4a78-ad5f-af8201289b69/receipts/abaf9c96-d7d8-4b7c-b41f-af820128b75c/files/8302a85d-8e6f-4ce4-a58b-e3fdeb4ea34d/download"
                            }
                        ]
                    },
                    {
                        "Id": "f8901590-437d-49c2-9a8f-efd3efb0fee1",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/32bad0ce-a2d9-4a78-ad5f-af8201289b69/receipts/abaf9c96-d7d8-4b7c-b41f-af820128b75c/files/f8901590-437d-49c2-9a8f-efd3efb0fee1/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "079fbdb9-281a-4b10-a26f-af820129790e",
                "ReceiveTime": "2023-01-06T18:03:03Z",
                "StatusTime": "2023-01-06T18:02:07Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "c3d0a222-c6b0-4dc4-b212-2b91dc52e6f6",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "962789ae-c7a8-4199-a31c-5a7b5f466e13",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/32bad0ce-a2d9-4a78-ad5f-af8201289b69/receipts/079fbdb9-281a-4b10-a26f-af820129790e/files/c3d0a222-c6b0-4dc4-b212-2b91dc52e6f6/download"
                            }
                        ]
                    },
                    {
                        "Id": "962789ae-c7a8-4199-a31c-5a7b5f466e13",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/32bad0ce-a2d9-4a78-ad5f-af8201289b69/receipts/079fbdb9-281a-4b10-a26f-af820129790e/files/962789ae-c7a8-4199-a31c-5a7b5f466e13/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "db10bc0f-5c71-4728-b639-af830128a256",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-07T18:00:00Z",
        "UpdatedDate": "2023-01-07T18:03:39Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00565218",
        "TotalSize": 13368,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "4c50cfa3-5323-4a76-a780-af830128a252",
                "Name": "KYCCL_7831001422_3194_20230107_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9496,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/db10bc0f-5c71-4728-b639-af830128a256/files/4c50cfa3-5323-4a76-a780-af830128a252/download"
                    }
                ]
            },
            {
                "Id": "95e4bed4-a395-4c97-952d-af830128a254",
                "Name": "KYCCL_7831001422_3194_20230107_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "4c50cfa3-5323-4a76-a780-af830128a252",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/db10bc0f-5c71-4728-b639-af830128a256/files/95e4bed4-a395-4c97-952d-af830128a254/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "1511c7c9-4411-4198-b78e-af830128a453",
                "ReceiveTime": "2023-01-07T18:00:02Z",
                "StatusTime": "2023-01-07T18:00:02Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "73ac8810-068d-492b-96f0-af830128b534",
                "ReceiveTime": "2023-01-07T18:00:16Z",
                "StatusTime": "2023-01-07T18:00:12Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "65c7154b-2688-4953-a467-036b818e6b67",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/db10bc0f-5c71-4728-b639-af830128a256/receipts/73ac8810-068d-492b-96f0-af830128b534/files/65c7154b-2688-4953-a467-036b818e6b67/download"
                            }
                        ]
                    },
                    {
                        "Id": "a6f8d8d4-a8fc-4cee-83f6-3ff63b072ca9",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "79c38a0b-1010-49c4-90a0-b76f7a8cb206",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/db10bc0f-5c71-4728-b639-af830128a256/receipts/73ac8810-068d-492b-96f0-af830128b534/files/a6f8d8d4-a8fc-4cee-83f6-3ff63b072ca9/download"
                            }
                        ]
                    },
                    {
                        "Id": "8d0175c9-8404-4000-82e5-525b47887a90",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "65c7154b-2688-4953-a467-036b818e6b67",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/db10bc0f-5c71-4728-b639-af830128a256/receipts/73ac8810-068d-492b-96f0-af830128b534/files/8d0175c9-8404-4000-82e5-525b47887a90/download"
                            }
                        ]
                    },
                    {
                        "Id": "79c38a0b-1010-49c4-90a0-b76f7a8cb206",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/db10bc0f-5c71-4728-b639-af830128a256/receipts/73ac8810-068d-492b-96f0-af830128b534/files/79c38a0b-1010-49c4-90a0-b76f7a8cb206/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "0b5570df-450d-410d-b631-af830128bda4",
                "ReceiveTime": "2023-01-07T18:00:24Z",
                "StatusTime": "2023-01-07T18:00:16Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "ab8f1112-15d7-45b3-b150-7a26f7bef34c",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "0aec991e-a990-4521-98ce-a895d02ca38b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/db10bc0f-5c71-4728-b639-af830128a256/receipts/0b5570df-450d-410d-b631-af830128bda4/files/ab8f1112-15d7-45b3-b150-7a26f7bef34c/download"
                            }
                        ]
                    },
                    {
                        "Id": "0aec991e-a990-4521-98ce-a895d02ca38b",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/db10bc0f-5c71-4728-b639-af830128a256/receipts/0b5570df-450d-410d-b631-af830128bda4/files/0aec991e-a990-4521-98ce-a895d02ca38b/download"
                            }
                        ]
                    },
                    {
                        "Id": "48493951-819b-4598-8845-cb094b8e1f7a",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "33b130ae-f654-4422-a5ba-e761ce0a6ccd",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/db10bc0f-5c71-4728-b639-af830128a256/receipts/0b5570df-450d-410d-b631-af830128bda4/files/48493951-819b-4598-8845-cb094b8e1f7a/download"
                            }
                        ]
                    },
                    {
                        "Id": "33b130ae-f654-4422-a5ba-e761ce0a6ccd",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/db10bc0f-5c71-4728-b639-af830128a256/receipts/0b5570df-450d-410d-b631-af830128bda4/files/33b130ae-f654-4422-a5ba-e761ce0a6ccd/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "4e9cfcfc-6e7e-49c4-a221-af830129a2c1",
                "ReceiveTime": "2023-01-07T18:03:39Z",
                "StatusTime": "2023-01-07T18:03:00Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "b3572d6c-0a29-4255-936d-f6a427eb2fec",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "cb8489c5-dcee-4d89-ac71-fff2a575e33e",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/db10bc0f-5c71-4728-b639-af830128a256/receipts/4e9cfcfc-6e7e-49c4-a221-af830129a2c1/files/b3572d6c-0a29-4255-936d-f6a427eb2fec/download"
                            }
                        ]
                    },
                    {
                        "Id": "cb8489c5-dcee-4d89-ac71-fff2a575e33e",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/db10bc0f-5c71-4728-b639-af830128a256/receipts/4e9cfcfc-6e7e-49c4-a221-af830129a2c1/files/cb8489c5-dcee-4d89-ac71-fff2a575e33e/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "05d3df74-9a31-48ad-aa78-af840128a181",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-08T17:59:59Z",
        "UpdatedDate": "2023-01-08T18:03:12Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00565279",
        "TotalSize": 13368,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "6e759b45-9b0e-4323-9070-af840128a17c",
                "Name": "KYCCL_7831001422_3194_20230108_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9496,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/05d3df74-9a31-48ad-aa78-af840128a181/files/6e759b45-9b0e-4323-9070-af840128a17c/download"
                    }
                ]
            },
            {
                "Id": "b756acd3-2651-4d9d-829a-af840128a17f",
                "Name": "KYCCL_7831001422_3194_20230108_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "6e759b45-9b0e-4323-9070-af840128a17c",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/05d3df74-9a31-48ad-aa78-af840128a181/files/b756acd3-2651-4d9d-829a-af840128a17f/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "d2697a83-3112-4f8f-bad9-af840128af21",
                "ReceiveTime": "2023-01-08T18:00:11Z",
                "StatusTime": "2023-01-08T18:00:11Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "36d5a8e7-5827-4d8f-9f88-af840128c527",
                "ReceiveTime": "2023-01-08T18:00:30Z",
                "StatusTime": "2023-01-08T18:00:22Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "48c74976-8d39-4bd7-aed6-1f316a94a797",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "1f821610-dc05-4bd4-b846-4aab945324d9",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/05d3df74-9a31-48ad-aa78-af840128a181/receipts/36d5a8e7-5827-4d8f-9f88-af840128c527/files/48c74976-8d39-4bd7-aed6-1f316a94a797/download"
                            }
                        ]
                    },
                    {
                        "Id": "1f821610-dc05-4bd4-b846-4aab945324d9",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/05d3df74-9a31-48ad-aa78-af840128a181/receipts/36d5a8e7-5827-4d8f-9f88-af840128c527/files/1f821610-dc05-4bd4-b846-4aab945324d9/download"
                            }
                        ]
                    },
                    {
                        "Id": "99fa93db-5848-4c3f-a31c-8b9db23490d9",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/05d3df74-9a31-48ad-aa78-af840128a181/receipts/36d5a8e7-5827-4d8f-9f88-af840128c527/files/99fa93db-5848-4c3f-a31c-8b9db23490d9/download"
                            }
                        ]
                    },
                    {
                        "Id": "68776ac8-7aea-4d28-9bc5-bb92fda40ba1",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "99fa93db-5848-4c3f-a31c-8b9db23490d9",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/05d3df74-9a31-48ad-aa78-af840128a181/receipts/36d5a8e7-5827-4d8f-9f88-af840128c527/files/68776ac8-7aea-4d28-9bc5-bb92fda40ba1/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "75354d33-c045-4f85-80aa-af840128cdde",
                "ReceiveTime": "2023-01-08T18:00:37Z",
                "StatusTime": "2023-01-08T18:00:30Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "d6632df5-ce5a-4bc9-88aa-058b54fd6a05",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "896d0e9e-c114-4a4e-9e7c-797b1e3664b7",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/05d3df74-9a31-48ad-aa78-af840128a181/receipts/75354d33-c045-4f85-80aa-af840128cdde/files/d6632df5-ce5a-4bc9-88aa-058b54fd6a05/download"
                            }
                        ]
                    },
                    {
                        "Id": "896d0e9e-c114-4a4e-9e7c-797b1e3664b7",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/05d3df74-9a31-48ad-aa78-af840128a181/receipts/75354d33-c045-4f85-80aa-af840128cdde/files/896d0e9e-c114-4a4e-9e7c-797b1e3664b7/download"
                            }
                        ]
                    },
                    {
                        "Id": "c5d91804-fac9-49fc-8487-83259fc0ce2d",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "d0f2c2d2-ebee-48e4-b75f-f09d699ecb02",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/05d3df74-9a31-48ad-aa78-af840128a181/receipts/75354d33-c045-4f85-80aa-af840128cdde/files/c5d91804-fac9-49fc-8487-83259fc0ce2d/download"
                            }
                        ]
                    },
                    {
                        "Id": "d0f2c2d2-ebee-48e4-b75f-f09d699ecb02",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/05d3df74-9a31-48ad-aa78-af840128a181/receipts/75354d33-c045-4f85-80aa-af840128cdde/files/d0f2c2d2-ebee-48e4-b75f-f09d699ecb02/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "ad4c7700-46c0-48ed-9be1-af8401298328",
                "ReceiveTime": "2023-01-08T18:03:12Z",
                "StatusTime": "2023-01-08T18:02:28Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "a2712ae2-3889-4527-aa1f-38b3d01196dc",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/05d3df74-9a31-48ad-aa78-af840128a181/receipts/ad4c7700-46c0-48ed-9be1-af8401298328/files/a2712ae2-3889-4527-aa1f-38b3d01196dc/download"
                            }
                        ]
                    },
                    {
                        "Id": "2b7eb75a-47b2-4984-b1ef-8f90e3bf8490",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "a2712ae2-3889-4527-aa1f-38b3d01196dc",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/05d3df74-9a31-48ad-aa78-af840128a181/receipts/ad4c7700-46c0-48ed-9be1-af8401298328/files/2b7eb75a-47b2-4984-b1ef-8f90e3bf8490/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "0b2b879a-ae21-4d46-a4c5-af8500d7dc52",
        "CorrelationId": "1f6158a2-a7a1-4e14-aace-af7a00f65145",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "предоставление запрошенной информации",
        "CreationDate": "2023-01-09T13:05:55Z",
        "UpdatedDate": "2023-01-09T13:18:19Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2086",
        "TotalSize": 14855079,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "7747c9d1-37c8-4b90-b4a4-af8500d7dc8a",
                "Name": "2023-01-09-1-3.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 14783077,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/files/7747c9d1-37c8-4b90-b4a4-af8500d7dc8a/download"
                    }
                ]
            },
            {
                "Id": "058d1343-8a40-4b2f-a1f7-af8500d7dc9a",
                "Name": "2023-01-09-1-3.docx.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 51873,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/files/058d1343-8a40-4b2f-a1f7-af8500d7dc9a/download"
                    }
                ]
            },
            {
                "Id": "ad865b23-fcd3-45d1-a6c2-af8500d7dc9e",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2006,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/files/ad865b23-fcd3-45d1-a6c2-af8500d7dc9e/download"
                    }
                ]
            },
            {
                "Id": "3fbffd34-dc38-4011-89e2-af8500d82b76",
                "Name": "2023-01-09-1-3.docx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "058d1343-8a40-4b2f-a1f7-af8500d7dc9a",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/files/3fbffd34-dc38-4011-89e2-af8500d82b76/download"
                    }
                ]
            },
            {
                "Id": "98999af0-c555-4d42-86e5-af8500d87b5c",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "ad865b23-fcd3-45d1-a6c2-af8500d7dc9e",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/files/98999af0-c555-4d42-86e5-af8500d87b5c/download"
                    }
                ]
            },
            {
                "Id": "83bea222-4099-4369-a4f0-af8500d8e6f6",
                "Name": "2023-01-09-1-3.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "7747c9d1-37c8-4b90-b4a4-af8500d7dc8a",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/files/83bea222-4099-4369-a4f0-af8500d8e6f6/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "c40d2a53-39e4-4f70-9356-af8500d8e96c",
                "ReceiveTime": "2023-01-09T13:09:45Z",
                "StatusTime": "2023-01-09T13:09:45Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "5abd4cb0-083f-495e-b69e-af8500d9029e",
                "ReceiveTime": "2023-01-09T13:10:06Z",
                "StatusTime": "2023-01-09T13:10:02Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "992b69af-7989-4029-84bb-190f62be3582",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "6279c8c5-0b98-4ebb-8684-f3512ffe72ed",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/receipts/5abd4cb0-083f-495e-b69e-af8500d9029e/files/992b69af-7989-4029-84bb-190f62be3582/download"
                            }
                        ]
                    },
                    {
                        "Id": "31860f99-dbef-4508-86c3-23f592b0925e",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/receipts/5abd4cb0-083f-495e-b69e-af8500d9029e/files/31860f99-dbef-4508-86c3-23f592b0925e/download"
                            }
                        ]
                    },
                    {
                        "Id": "e5eec349-2306-4e70-8f1a-c202ee681443",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "31860f99-dbef-4508-86c3-23f592b0925e",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/receipts/5abd4cb0-083f-495e-b69e-af8500d9029e/files/e5eec349-2306-4e70-8f1a-c202ee681443/download"
                            }
                        ]
                    },
                    {
                        "Id": "6279c8c5-0b98-4ebb-8684-f3512ffe72ed",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/receipts/5abd4cb0-083f-495e-b69e-af8500d9029e/files/6279c8c5-0b98-4ebb-8684-f3512ffe72ed/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "0fcc939a-1ec7-44eb-aa32-af8500d9097a",
                "ReceiveTime": "2023-01-09T13:10:12Z",
                "StatusTime": "2023-01-09T13:10:04Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "c329676c-2f64-4358-9892-43f1ae6d84ce",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "d7431cbd-18ae-4faa-a593-4ad5ba179136",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/receipts/0fcc939a-1ec7-44eb-aa32-af8500d9097a/files/c329676c-2f64-4358-9892-43f1ae6d84ce/download"
                            }
                        ]
                    },
                    {
                        "Id": "d7431cbd-18ae-4faa-a593-4ad5ba179136",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/receipts/0fcc939a-1ec7-44eb-aa32-af8500d9097a/files/d7431cbd-18ae-4faa-a593-4ad5ba179136/download"
                            }
                        ]
                    },
                    {
                        "Id": "22cb4e72-4899-4917-8685-9cd90dadda4a",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "18e71f62-f5fb-4515-917d-d33a2d7f5ac7",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/receipts/0fcc939a-1ec7-44eb-aa32-af8500d9097a/files/22cb4e72-4899-4917-8685-9cd90dadda4a/download"
                            }
                        ]
                    },
                    {
                        "Id": "18e71f62-f5fb-4515-917d-d33a2d7f5ac7",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/receipts/0fcc939a-1ec7-44eb-aa32-af8500d9097a/files/18e71f62-f5fb-4515-917d-d33a2d7f5ac7/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "3cc1fa7d-8d26-46b4-98cd-af8500db4499",
                "ReceiveTime": "2023-01-09T13:18:19Z",
                "StatusTime": "2023-01-09T13:18:10Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "d1db900e-067c-437d-bd1b-5646ce8f9acd",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 353,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/receipts/3cc1fa7d-8d26-46b4-98cd-af8500db4499/files/d1db900e-067c-437d-bd1b-5646ce8f9acd/download"
                            }
                        ]
                    },
                    {
                        "Id": "360ec920-e6d1-4a6f-8c55-9887ac252893",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 741,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/receipts/3cc1fa7d-8d26-46b4-98cd-af8500db4499/files/360ec920-e6d1-4a6f-8c55-9887ac252893/download"
                            }
                        ]
                    },
                    {
                        "Id": "65cc6c11-c71a-41cb-8577-b6131e3be18b",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "360ec920-e6d1-4a6f-8c55-9887ac252893",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/receipts/3cc1fa7d-8d26-46b4-98cd-af8500db4499/files/65cc6c11-c71a-41cb-8577-b6131e3be18b/download"
                            }
                        ]
                    },
                    {
                        "Id": "527b08b1-933a-4438-acd4-cdf686a14fba",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "d1db900e-067c-437d-bd1b-5646ce8f9acd",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0b2b879a-ae21-4d46-a4c5-af8500d7dc52/receipts/3cc1fa7d-8d26-46b4-98cd-af8500db4499/files/527b08b1-933a-4438-acd4-cdf686a14fba/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "57ddb83f-9909-4fe9-b4ef-af8500def49a",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Обращение (запрос)  в Банк России",
        "Text": "предоставление запрошенной информации",
        "CreationDate": "2023-01-09T13:31:45Z",
        "UpdatedDate": "2023-01-09T13:54:12Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2-484",
        "TotalSize": 14855479,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "dd8b53ca-6181-4035-9c86-af8500def4c6",
                "Name": "2023-01-09-1-3-1.docx.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 51904,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/files/dd8b53ca-6181-4035-9c86-af8500def4c6/download"
                    }
                ]
            },
            {
                "Id": "35f4e9d2-a268-4f35-8b40-af8500def4dd",
                "Name": "2023-01-09-1-3-1.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 14783077,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/files/35f4e9d2-a268-4f35-8b40-af8500def4dd/download"
                    }
                ]
            },
            {
                "Id": "d47b3aa0-4ac3-403e-bf41-af8500def4e0",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2375,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/files/d47b3aa0-4ac3-403e-bf41-af8500def4e0/download"
                    }
                ]
            },
            {
                "Id": "814a114b-c912-4ddb-91c2-af8500df3e56",
                "Name": "2023-01-09-1-3-1.docx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "dd8b53ca-6181-4035-9c86-af8500def4c6",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/files/814a114b-c912-4ddb-91c2-af8500df3e56/download"
                    }
                ]
            },
            {
                "Id": "ecd8f7f3-ef77-45d6-afb8-af8500df8da3",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "d47b3aa0-4ac3-403e-bf41-af8500def4e0",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/files/ecd8f7f3-ef77-45d6-afb8-af8500df8da3/download"
                    }
                ]
            },
            {
                "Id": "54da0661-1c7a-43bf-a3fb-af8500dff973",
                "Name": "2023-01-09-1-3-1.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "35f4e9d2-a268-4f35-8b40-af8500def4dd",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/files/54da0661-1c7a-43bf-a3fb-af8500dff973/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "2c090e9b-8486-4fc8-872c-af8500dffc68",
                "ReceiveTime": "2023-01-09T13:35:30Z",
                "StatusTime": "2023-01-09T13:35:30Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "2f75cd48-a2b7-44e1-a698-af8500e01023",
                "ReceiveTime": "2023-01-09T13:35:47Z",
                "StatusTime": "2023-01-09T13:35:46Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "212abc5e-1fc1-45a9-8a1d-149c740bf74d",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "86608456-c229-43f3-84a1-7b39fbbecb12",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/receipts/2f75cd48-a2b7-44e1-a698-af8500e01023/files/212abc5e-1fc1-45a9-8a1d-149c740bf74d/download"
                            }
                        ]
                    },
                    {
                        "Id": "2cab3ccf-c29d-4044-a647-2399f162395c",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "e49ad271-d06d-43b9-a510-c521c05d7ae5",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/receipts/2f75cd48-a2b7-44e1-a698-af8500e01023/files/2cab3ccf-c29d-4044-a647-2399f162395c/download"
                            }
                        ]
                    },
                    {
                        "Id": "86608456-c229-43f3-84a1-7b39fbbecb12",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/receipts/2f75cd48-a2b7-44e1-a698-af8500e01023/files/86608456-c229-43f3-84a1-7b39fbbecb12/download"
                            }
                        ]
                    },
                    {
                        "Id": "e49ad271-d06d-43b9-a510-c521c05d7ae5",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/receipts/2f75cd48-a2b7-44e1-a698-af8500e01023/files/e49ad271-d06d-43b9-a510-c521c05d7ae5/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "5e30feb5-34ed-46d7-8f1a-af8500e01729",
                "ReceiveTime": "2023-01-09T13:35:53Z",
                "StatusTime": "2023-01-09T13:35:47Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "8ab85550-fdb6-4be4-8580-50f9e2b69c4d",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "a5df2308-1499-472b-a607-8c45354d2fa7",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/receipts/5e30feb5-34ed-46d7-8f1a-af8500e01729/files/8ab85550-fdb6-4be4-8580-50f9e2b69c4d/download"
                            }
                        ]
                    },
                    {
                        "Id": "a5df2308-1499-472b-a607-8c45354d2fa7",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/receipts/5e30feb5-34ed-46d7-8f1a-af8500e01729/files/a5df2308-1499-472b-a607-8c45354d2fa7/download"
                            }
                        ]
                    },
                    {
                        "Id": "ec709a7a-33de-4fc7-86c8-c9cd2971105f",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "00d05452-b1aa-427a-8af4-f2234158e036",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/receipts/5e30feb5-34ed-46d7-8f1a-af8500e01729/files/ec709a7a-33de-4fc7-86c8-c9cd2971105f/download"
                            }
                        ]
                    },
                    {
                        "Id": "00d05452-b1aa-427a-8af4-f2234158e036",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/receipts/5e30feb5-34ed-46d7-8f1a-af8500e01729/files/00d05452-b1aa-427a-8af4-f2234158e036/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "9609ebeb-4280-4c9e-b521-af8500e51ef8",
                "ReceiveTime": "2023-01-09T13:54:12Z",
                "StatusTime": "2023-01-09T13:37:29Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "db72488b-691a-405f-bd7c-9de3e706d740",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/receipts/9609ebeb-4280-4c9e-b521-af8500e51ef8/files/db72488b-691a-405f-bd7c-9de3e706d740/download"
                            }
                        ]
                    },
                    {
                        "Id": "1672cc8d-f0c3-4e70-bd14-a4283725b24e",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "cd812df0-3c37-4a3e-a05a-f3790f1c0e6e",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/receipts/9609ebeb-4280-4c9e-b521-af8500e51ef8/files/1672cc8d-f0c3-4e70-bd14-a4283725b24e/download"
                            }
                        ]
                    },
                    {
                        "Id": "3e0a5782-0305-404e-b9ce-e5dba086a042",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "db72488b-691a-405f-bd7c-9de3e706d740",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/receipts/9609ebeb-4280-4c9e-b521-af8500e51ef8/files/3e0a5782-0305-404e-b9ce-e5dba086a042/download"
                            }
                        ]
                    },
                    {
                        "Id": "cd812df0-3c37-4a3e-a05a-f3790f1c0e6e",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 779,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/57ddb83f-9909-4fe9-b4ef-af8500def49a/receipts/9609ebeb-4280-4c9e-b521-af8500e51ef8/files/cd812df0-3c37-4a3e-a05a-f3790f1c0e6e/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "32b3026c-1f60-4a16-96ba-af8500e8aa6b",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК в организацию. Ответ ЦИК в организацию.",
        "Text": "",
        "CreationDate": "2023-01-09T14:07:06Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_56",
        "RegNumber": null,
        "TotalSize": 3916,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "8c2df224-1390-4806-9ddc-21baef268c9d",
                "Name": "F1027700466640_090123_151212_K_0056_2000_K1027800000095.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "4a426ca9-1d40-4d5a-90be-af8500e8878f",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/32b3026c-1f60-4a16-96ba-af8500e8aa6b/files/8c2df224-1390-4806-9ddc-21baef268c9d/download"
                    }
                ]
            },
            {
                "Id": "4a426ca9-1d40-4d5a-90be-af8500e8878f",
                "Name": "F1027700466640_090123_151212_K_0056_2000_K1027800000095.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 655,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/32b3026c-1f60-4a16-96ba-af8500e8aa6b/files/4a426ca9-1d40-4d5a-90be-af8500e8878f/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "e35ff65d-3450-4cb3-a2d8-af8500ebfd74",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Обращение (запрос)  в Банк России",
        "Text": "в дополнение к нашему 3-1-1 от 09.01.2023",
        "CreationDate": "2023-01-09T14:19:12Z",
        "UpdatedDate": "2023-01-09T14:27:36Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2-570",
        "TotalSize": 20557523,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "18618896-0239-4ff4-a5c5-af8500ebfda7",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2366,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/files/18618896-0239-4ff4-a5c5-af8500ebfda7/download"
                    }
                ]
            },
            {
                "Id": "d0250af8-1683-45f0-b713-af8500ebfdad",
                "Name": "2023-01-09-1-3-1-1 Кредиты.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 20543075,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/files/d0250af8-1683-45f0-b713-af8500ebfdad/download"
                    }
                ]
            },
            {
                "Id": "17dfec7f-d6d6-4dd8-afb2-af8500ec42e1",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "18618896-0239-4ff4-a5c5-af8500ebfda7",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/files/17dfec7f-d6d6-4dd8-afb2-af8500ec42e1/download"
                    }
                ]
            },
            {
                "Id": "4114ea77-664c-414a-84f0-af8500ecb834",
                "Name": "2023-01-09-1-3-1-1 Кредиты.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "d0250af8-1683-45f0-b713-af8500ebfdad",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/files/4114ea77-664c-414a-84f0-af8500ecb834/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "be0dab1c-879a-4fdd-bdff-af8500ecbae9",
                "ReceiveTime": "2023-01-09T14:21:54Z",
                "StatusTime": "2023-01-09T14:21:54Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "5a8e8da3-f231-4a51-8245-af8500ecd0c3",
                "ReceiveTime": "2023-01-09T14:22:13Z",
                "StatusTime": "2023-01-09T14:22:10Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "897599b0-6f54-4d2d-a274-2f84c629b3eb",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "5277d4c0-61d0-440c-ab08-e0abda2ab424",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/receipts/5a8e8da3-f231-4a51-8245-af8500ecd0c3/files/897599b0-6f54-4d2d-a274-2f84c629b3eb/download"
                            }
                        ]
                    },
                    {
                        "Id": "e962b519-084f-4f00-ae97-7cd0dea5804b",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "94380b41-5415-4cfb-9a51-d85f56ef1a87",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/receipts/5a8e8da3-f231-4a51-8245-af8500ecd0c3/files/e962b519-084f-4f00-ae97-7cd0dea5804b/download"
                            }
                        ]
                    },
                    {
                        "Id": "94380b41-5415-4cfb-9a51-d85f56ef1a87",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/receipts/5a8e8da3-f231-4a51-8245-af8500ecd0c3/files/94380b41-5415-4cfb-9a51-d85f56ef1a87/download"
                            }
                        ]
                    },
                    {
                        "Id": "5277d4c0-61d0-440c-ab08-e0abda2ab424",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/receipts/5a8e8da3-f231-4a51-8245-af8500ecd0c3/files/5277d4c0-61d0-440c-ab08-e0abda2ab424/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "8f247b4d-a24b-4eb6-acd2-af8500ecd7ea",
                "ReceiveTime": "2023-01-09T14:22:19Z",
                "StatusTime": "2023-01-09T14:22:12Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "7bc0c63f-8b11-4dd0-a68a-5f714fd6519a",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/receipts/8f247b4d-a24b-4eb6-acd2-af8500ecd7ea/files/7bc0c63f-8b11-4dd0-a68a-5f714fd6519a/download"
                            }
                        ]
                    },
                    {
                        "Id": "931dcd20-134b-41e2-9297-97b8bd39cd13",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "aaa2fa3e-99b8-4b85-9f54-e83964f72dcd",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/receipts/8f247b4d-a24b-4eb6-acd2-af8500ecd7ea/files/931dcd20-134b-41e2-9297-97b8bd39cd13/download"
                            }
                        ]
                    },
                    {
                        "Id": "6e0679ad-8b7a-4477-8e75-d7a983c01e12",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "7bc0c63f-8b11-4dd0-a68a-5f714fd6519a",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/receipts/8f247b4d-a24b-4eb6-acd2-af8500ecd7ea/files/6e0679ad-8b7a-4477-8e75-d7a983c01e12/download"
                            }
                        ]
                    },
                    {
                        "Id": "aaa2fa3e-99b8-4b85-9f54-e83964f72dcd",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/receipts/8f247b4d-a24b-4eb6-acd2-af8500ecd7ea/files/aaa2fa3e-99b8-4b85-9f54-e83964f72dcd/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "064ed11e-3ef8-44b1-9f14-af8500ee4bd6",
                "ReceiveTime": "2023-01-09T14:27:36Z",
                "StatusTime": "2023-01-09T14:27:28Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "c2ce8023-81b8-42ac-a2f3-2915f4758585",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "c6eadb84-d83d-477c-ab85-fd0a8b414e33",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/receipts/064ed11e-3ef8-44b1-9f14-af8500ee4bd6/files/c2ce8023-81b8-42ac-a2f3-2915f4758585/download"
                            }
                        ]
                    },
                    {
                        "Id": "e6dd2f64-391a-4a58-84ee-97f473ff1898",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "a8b8ef9f-33ad-427a-964b-b202aca0cbd9",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/receipts/064ed11e-3ef8-44b1-9f14-af8500ee4bd6/files/e6dd2f64-391a-4a58-84ee-97f473ff1898/download"
                            }
                        ]
                    },
                    {
                        "Id": "a8b8ef9f-33ad-427a-964b-b202aca0cbd9",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/receipts/064ed11e-3ef8-44b1-9f14-af8500ee4bd6/files/a8b8ef9f-33ad-427a-964b-b202aca0cbd9/download"
                            }
                        ]
                    },
                    {
                        "Id": "c6eadb84-d83d-477c-ab85-fd0a8b414e33",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 779,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e35ff65d-3450-4cb3-a2d8-af8500ebfd74/receipts/064ed11e-3ef8-44b1-9f14-af8500ee4bd6/files/c6eadb84-d83d-477c-ab85-fd0a8b414e33/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "97817251-9226-4a2c-bbb8-af8500ed33f6",
        "CorrelationId": "1f6158a2-a7a1-4e14-aace-af7a00f65145",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "в дополнение к нашему 1-3 от 09.01.2023",
        "CreationDate": "2023-01-09T14:23:37Z",
        "UpdatedDate": "2023-01-09T14:52:19Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2688",
        "TotalSize": 20557155,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "dbd2ba9a-fb2a-4409-b3ec-af8500ed3434",
                "Name": "2023-01-09-1-3-1-1 Кредиты.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 20543075,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/files/dbd2ba9a-fb2a-4409-b3ec-af8500ed3434/download"
                    }
                ]
            },
            {
                "Id": "0bfb905b-2f1b-4b1c-a9a8-af8500ed3447",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 1998,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/files/0bfb905b-2f1b-4b1c-a9a8-af8500ed3447/download"
                    }
                ]
            },
            {
                "Id": "02b05753-e662-4a98-9c3b-af8500ed81bc",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "0bfb905b-2f1b-4b1c-a9a8-af8500ed3447",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/files/02b05753-e662-4a98-9c3b-af8500ed81bc/download"
                    }
                ]
            },
            {
                "Id": "1f78840c-76ac-42b0-b4e7-af8500edf74e",
                "Name": "2023-01-09-1-3-1-1 Кредиты.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "dbd2ba9a-fb2a-4409-b3ec-af8500ed3434",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/files/1f78840c-76ac-42b0-b4e7-af8500edf74e/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "feef0aac-ed1e-4ec8-8c96-af8500edfa4c",
                "ReceiveTime": "2023-01-09T14:26:27Z",
                "StatusTime": "2023-01-09T14:26:27Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "e32e78f0-d286-492b-a744-af8500ee11f5",
                "ReceiveTime": "2023-01-09T14:26:47Z",
                "StatusTime": "2023-01-09T14:26:43Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "99a55120-e140-476b-887c-3478dc864553",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/receipts/e32e78f0-d286-492b-a744-af8500ee11f5/files/99a55120-e140-476b-887c-3478dc864553/download"
                            }
                        ]
                    },
                    {
                        "Id": "6856e8c1-102c-4e61-9b21-50c886402cfe",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "b172d19f-8070-4ad5-a2dd-dca8b02a3a32",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/receipts/e32e78f0-d286-492b-a744-af8500ee11f5/files/6856e8c1-102c-4e61-9b21-50c886402cfe/download"
                            }
                        ]
                    },
                    {
                        "Id": "edd2bd30-15ed-4b54-ae02-c1cff228bef6",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "99a55120-e140-476b-887c-3478dc864553",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/receipts/e32e78f0-d286-492b-a744-af8500ee11f5/files/edd2bd30-15ed-4b54-ae02-c1cff228bef6/download"
                            }
                        ]
                    },
                    {
                        "Id": "b172d19f-8070-4ad5-a2dd-dca8b02a3a32",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/receipts/e32e78f0-d286-492b-a744-af8500ee11f5/files/b172d19f-8070-4ad5-a2dd-dca8b02a3a32/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "e675cf81-17c6-41dd-a782-af8500ee1c2d",
                "ReceiveTime": "2023-01-09T14:26:55Z",
                "StatusTime": "2023-01-09T14:26:46Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "3ed649e7-ed64-4798-974d-02df26d445f1",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "b47d27fc-c252-4b42-a53b-cf758d52ee80",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/receipts/e675cf81-17c6-41dd-a782-af8500ee1c2d/files/3ed649e7-ed64-4798-974d-02df26d445f1/download"
                            }
                        ]
                    },
                    {
                        "Id": "af551420-79ab-44d0-8847-27e1c9ba47dc",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/receipts/e675cf81-17c6-41dd-a782-af8500ee1c2d/files/af551420-79ab-44d0-8847-27e1c9ba47dc/download"
                            }
                        ]
                    },
                    {
                        "Id": "64f55476-c9e8-4a64-8753-bcea8fb0232a",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "af551420-79ab-44d0-8847-27e1c9ba47dc",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/receipts/e675cf81-17c6-41dd-a782-af8500ee1c2d/files/64f55476-c9e8-4a64-8753-bcea8fb0232a/download"
                            }
                        ]
                    },
                    {
                        "Id": "b47d27fc-c252-4b42-a53b-cf758d52ee80",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/receipts/e675cf81-17c6-41dd-a782-af8500ee1c2d/files/b47d27fc-c252-4b42-a53b-cf758d52ee80/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "0ef3d06c-c3d5-4e26-b9ee-af8500f5162b",
                "ReceiveTime": "2023-01-09T14:52:19Z",
                "StatusTime": "2023-01-09T14:49:57Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "dee401c5-e9a7-4f11-b9e5-12374f10b0b0",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 353,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/receipts/0ef3d06c-c3d5-4e26-b9ee-af8500f5162b/files/dee401c5-e9a7-4f11-b9e5-12374f10b0b0/download"
                            }
                        ]
                    },
                    {
                        "Id": "f64d424d-bdc5-46b9-b140-2aa72fac2ffd",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "b657e738-07a5-4dd9-83ee-f3e79fb9dc0c",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/receipts/0ef3d06c-c3d5-4e26-b9ee-af8500f5162b/files/f64d424d-bdc5-46b9-b140-2aa72fac2ffd/download"
                            }
                        ]
                    },
                    {
                        "Id": "c783ea2b-2f23-4d15-b647-300005ba0dc8",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "dee401c5-e9a7-4f11-b9e5-12374f10b0b0",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/receipts/0ef3d06c-c3d5-4e26-b9ee-af8500f5162b/files/c783ea2b-2f23-4d15-b647-300005ba0dc8/download"
                            }
                        ]
                    },
                    {
                        "Id": "b657e738-07a5-4dd9-83ee-f3e79fb9dc0c",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 741,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/97817251-9226-4a2c-bbb8-af8500ed33f6/receipts/0ef3d06c-c3d5-4e26-b9ee-af8500f5162b/files/b657e738-07a5-4dd9-83ee-f3e79fb9dc0c/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "733eb92a-ffd9-4f0d-a4d8-af850108b750",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-01-09T16:03:56Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 2182812,
        "Sender": null,
        "Files": [
            {
                "Id": "bacf2e10-c9b3-4eaa-ad48-4e8c41a5323b",
                "Name": "KYC_20230109.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2179551,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/733eb92a-ffd9-4f0d-a4d8-af850108b750/files/bacf2e10-c9b3-4eaa-ad48-4e8c41a5323b/download"
                    }
                ]
            },
            {
                "Id": "8a96f206-77f2-4f6f-bce0-df43e3d31e78",
                "Name": "KYC_20230109.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "bacf2e10-c9b3-4eaa-ad48-4e8c41a5323b",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/733eb92a-ffd9-4f0d-a4d8-af850108b750/files/8a96f206-77f2-4f6f-bce0-df43e3d31e78/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "36d7c6b0-ada6-4936-ae58-af850128a00b",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-09T17:59:58Z",
        "UpdatedDate": "2023-01-09T18:02:55Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00566171",
        "TotalSize": 13368,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "e88c20d4-d06f-478b-bfd3-af850128a007",
                "Name": "KYCCL_7831001422_3194_20230109_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9496,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/36d7c6b0-ada6-4936-ae58-af850128a00b/files/e88c20d4-d06f-478b-bfd3-af850128a007/download"
                    }
                ]
            },
            {
                "Id": "717d02c7-e8bb-440c-9bec-af850128a009",
                "Name": "KYCCL_7831001422_3194_20230109_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "e88c20d4-d06f-478b-bfd3-af850128a007",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/36d7c6b0-ada6-4936-ae58-af850128a00b/files/717d02c7-e8bb-440c-9bec-af850128a009/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "aa179c1c-f23f-4e3b-9b60-af850128a16b",
                "ReceiveTime": "2023-01-09T17:59:59Z",
                "StatusTime": "2023-01-09T17:59:59Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "431aa217-a835-4114-b2ef-af850128b100",
                "ReceiveTime": "2023-01-09T18:00:13Z",
                "StatusTime": "2023-01-09T18:00:09Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "fbfcc773-4ee0-4691-8a04-551ff27d4060",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "c85fd07e-ac9a-46cd-830e-cc801e03307d",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/36d7c6b0-ada6-4936-ae58-af850128a00b/receipts/431aa217-a835-4114-b2ef-af850128b100/files/fbfcc773-4ee0-4691-8a04-551ff27d4060/download"
                            }
                        ]
                    },
                    {
                        "Id": "e09f43a5-fcaf-4014-a825-76a3c7c7e0ff",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/36d7c6b0-ada6-4936-ae58-af850128a00b/receipts/431aa217-a835-4114-b2ef-af850128b100/files/e09f43a5-fcaf-4014-a825-76a3c7c7e0ff/download"
                            }
                        ]
                    },
                    {
                        "Id": "5c7f9dde-9ba1-4a9c-bfe2-9299a1231d0c",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "e09f43a5-fcaf-4014-a825-76a3c7c7e0ff",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/36d7c6b0-ada6-4936-ae58-af850128a00b/receipts/431aa217-a835-4114-b2ef-af850128b100/files/5c7f9dde-9ba1-4a9c-bfe2-9299a1231d0c/download"
                            }
                        ]
                    },
                    {
                        "Id": "c85fd07e-ac9a-46cd-830e-cc801e03307d",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/36d7c6b0-ada6-4936-ae58-af850128a00b/receipts/431aa217-a835-4114-b2ef-af850128b100/files/c85fd07e-ac9a-46cd-830e-cc801e03307d/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "b0d82183-ccc0-4896-a39f-af850128b9a6",
                "ReceiveTime": "2023-01-09T18:00:20Z",
                "StatusTime": "2023-01-09T18:00:13Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "6a84bb6f-d131-41d5-8a24-342dd3fa6db4",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/36d7c6b0-ada6-4936-ae58-af850128a00b/receipts/b0d82183-ccc0-4896-a39f-af850128b9a6/files/6a84bb6f-d131-41d5-8a24-342dd3fa6db4/download"
                            }
                        ]
                    },
                    {
                        "Id": "95031051-508c-4ce6-8d9e-7ecbf654c31a",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "dd4bb681-1beb-4c13-b088-fef96bf166f2",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/36d7c6b0-ada6-4936-ae58-af850128a00b/receipts/b0d82183-ccc0-4896-a39f-af850128b9a6/files/95031051-508c-4ce6-8d9e-7ecbf654c31a/download"
                            }
                        ]
                    },
                    {
                        "Id": "b33205c5-d61a-46a0-be64-bb700e62ffd3",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "6a84bb6f-d131-41d5-8a24-342dd3fa6db4",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/36d7c6b0-ada6-4936-ae58-af850128a00b/receipts/b0d82183-ccc0-4896-a39f-af850128b9a6/files/b33205c5-d61a-46a0-be64-bb700e62ffd3/download"
                            }
                        ]
                    },
                    {
                        "Id": "dd4bb681-1beb-4c13-b088-fef96bf166f2",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/36d7c6b0-ada6-4936-ae58-af850128a00b/receipts/b0d82183-ccc0-4896-a39f-af850128b9a6/files/dd4bb681-1beb-4c13-b088-fef96bf166f2/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "39a3a5c6-af77-468e-a7a7-af8501296f72",
                "ReceiveTime": "2023-01-09T18:02:55Z",
                "StatusTime": "2023-01-09T18:02:07Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "5da5214d-82e9-46fd-89bc-0e0c141cf1ac",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/36d7c6b0-ada6-4936-ae58-af850128a00b/receipts/39a3a5c6-af77-468e-a7a7-af8501296f72/files/5da5214d-82e9-46fd-89bc-0e0c141cf1ac/download"
                            }
                        ]
                    },
                    {
                        "Id": "b03da480-75b9-4bfc-bbe2-4d59d4f2719c",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "5da5214d-82e9-46fd-89bc-0e0c141cf1ac",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/36d7c6b0-ada6-4936-ae58-af850128a00b/receipts/39a3a5c6-af77-468e-a7a7-af8501296f72/files/b03da480-75b9-4bfc-bbe2-4d59d4f2719c/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "4969c89f-4c9e-46d9-81b4-af86006f91b9",
        "CorrelationId": null,
        "GroupId": "a4e5902c-e961-47a3-9670-bd717bcc1749",
        "Type": "inbox",
        "Title": "№ 20-2-1/1 от 10/01/2023 (20) Письма Деп-та денежно-кредитной политики",
        "Text": "О данных для расчета размера обязательных резервов",
        "CreationDate": "2023-01-10T06:46:16Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "20-2-1/1",
        "TotalSize": 498117,
        "Sender": null,
        "Files": [
            {
                "Id": "16626017-8606-48de-b23e-15b22b77f1b6",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "cf54b779-0303-401a-b7e2-327a43937a16",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4969c89f-4c9e-46d9-81b4-af86006f91b9/files/16626017-8606-48de-b23e-15b22b77f1b6/download"
                    }
                ]
            },
            {
                "Id": "cf54b779-0303-401a-b7e2-327a43937a16",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "612079589.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 114194,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4969c89f-4c9e-46d9-81b4-af86006f91b9/files/cf54b779-0303-401a-b7e2-327a43937a16/download"
                    }
                ]
            },
            {
                "Id": "79219bdc-f574-4fd3-92f1-c40a7be9e1aa",
                "Name": "ЭД_Письмо.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "87f1dbb7-8df4-4896-b1f0-de282b83c6a9",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4969c89f-4c9e-46d9-81b4-af86006f91b9/files/79219bdc-f574-4fd3-92f1-c40a7be9e1aa/download"
                    }
                ]
            },
            {
                "Id": "a9a08e23-bc40-428e-a090-dd42d9d3a7a1",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 268719,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4969c89f-4c9e-46d9-81b4-af86006f91b9/files/a9a08e23-bc40-428e-a090-dd42d9d3a7a1/download"
                    }
                ]
            },
            {
                "Id": "87f1dbb7-8df4-4896-b1f0-de282b83c6a9",
                "Name": "ЭД_Письмо.pdf",
                "Description": "612078576.pdf",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 108682,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4969c89f-4c9e-46d9-81b4-af86006f91b9/files/87f1dbb7-8df4-4896-b1f0-de282b83c6a9/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "38a9fd73-b9b0-4775-bd1d-af8600b0f1c6",
        "CorrelationId": "ca302a97-d12c-4397-b0fd-af7300ccb86c",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "Сообщение об ознакомлении, возражения на Акт проверки",
        "CreationDate": "2023-01-10T10:44:14Z",
        "UpdatedDate": "2023-01-10T10:54:14Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2-913",
        "TotalSize": 1501621,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "f2538646-9175-43ce-8a95-af8600b0f220",
                "Name": "2023-01-10-2-3 сообщение об ознакомлении.pdf.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 91318,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/files/f2538646-9175-43ce-8a95-af8600b0f220/download"
                    }
                ]
            },
            {
                "Id": "555c94be-b0c5-4067-b64c-af8600b0f224",
                "Name": "2022-01-10-2-3 возражения.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 1390147,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/files/555c94be-b0c5-4067-b64c-af8600b0f224/download"
                    }
                ]
            },
            {
                "Id": "b5ea744c-b969-4705-b3cd-af8600b0f251",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2033,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/files/b5ea744c-b969-4705-b3cd-af8600b0f251/download"
                    }
                ]
            },
            {
                "Id": "8a8af800-8256-4815-ac17-af8600b13db7",
                "Name": "2023-01-10-2-3 сообщение об ознакомлении.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "f2538646-9175-43ce-8a95-af8600b0f220",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/files/8a8af800-8256-4815-ac17-af8600b13db7/download"
                    }
                ]
            },
            {
                "Id": "7db52921-76c9-4d35-bc0e-af8600b18851",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "b5ea744c-b969-4705-b3cd-af8600b0f251",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/files/7db52921-76c9-4d35-bc0e-af8600b18851/download"
                    }
                ]
            },
            {
                "Id": "82317938-5997-4239-a33e-af8600b1d3b3",
                "Name": "2022-01-10-2-3 возражения.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "555c94be-b0c5-4067-b64c-af8600b0f224",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/files/82317938-5997-4239-a33e-af8600b1d3b3/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "88b85c3c-653c-4d8a-bea7-af8600b1d4fb",
                "ReceiveTime": "2023-01-10T10:47:27Z",
                "StatusTime": "2023-01-10T10:47:27Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "7ef29308-e26a-46fa-a4a3-af8600b1e7a3",
                "ReceiveTime": "2023-01-10T10:47:43Z",
                "StatusTime": "2023-01-10T10:47:40Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "02f54f6e-ddd5-4801-baa4-28dd2ede1000",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/receipts/7ef29308-e26a-46fa-a4a3-af8600b1e7a3/files/02f54f6e-ddd5-4801-baa4-28dd2ede1000/download"
                            }
                        ]
                    },
                    {
                        "Id": "0b6aa654-db26-4568-bec6-7eb82084be47",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "02f54f6e-ddd5-4801-baa4-28dd2ede1000",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/receipts/7ef29308-e26a-46fa-a4a3-af8600b1e7a3/files/0b6aa654-db26-4568-bec6-7eb82084be47/download"
                            }
                        ]
                    },
                    {
                        "Id": "ea79b1df-4a4f-44fb-9970-8359f741c5da",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/receipts/7ef29308-e26a-46fa-a4a3-af8600b1e7a3/files/ea79b1df-4a4f-44fb-9970-8359f741c5da/download"
                            }
                        ]
                    },
                    {
                        "Id": "1a5248b0-3455-4c92-8fcf-fa70a3d68020",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "ea79b1df-4a4f-44fb-9970-8359f741c5da",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/receipts/7ef29308-e26a-46fa-a4a3-af8600b1e7a3/files/1a5248b0-3455-4c92-8fcf-fa70a3d68020/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "92d457c5-68c9-4a0f-80f1-af8600b1f208",
                "ReceiveTime": "2023-01-10T10:47:52Z",
                "StatusTime": "2023-01-10T10:47:43Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "d49c8174-ac22-4167-83ae-27c940075e24",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/receipts/92d457c5-68c9-4a0f-80f1-af8600b1f208/files/d49c8174-ac22-4167-83ae-27c940075e24/download"
                            }
                        ]
                    },
                    {
                        "Id": "b6816d33-aebb-468a-936c-5900efc94296",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "d49c8174-ac22-4167-83ae-27c940075e24",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/receipts/92d457c5-68c9-4a0f-80f1-af8600b1f208/files/b6816d33-aebb-468a-936c-5900efc94296/download"
                            }
                        ]
                    },
                    {
                        "Id": "5fe274ed-1208-4881-a5d2-74bbed443ce2",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "31a4333a-801b-476c-a872-c8a66719a6ad",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/receipts/92d457c5-68c9-4a0f-80f1-af8600b1f208/files/5fe274ed-1208-4881-a5d2-74bbed443ce2/download"
                            }
                        ]
                    },
                    {
                        "Id": "31a4333a-801b-476c-a872-c8a66719a6ad",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/receipts/92d457c5-68c9-4a0f-80f1-af8600b1f208/files/31a4333a-801b-476c-a872-c8a66719a6ad/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "f818d8c1-a01b-458a-af52-af8600b3b100",
                "ReceiveTime": "2023-01-10T10:54:14Z",
                "StatusTime": "2023-01-10T10:54:02Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "5b893c45-baef-448d-8b4a-0ecb0b78e5e7",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/receipts/f818d8c1-a01b-458a-af52-af8600b3b100/files/5b893c45-baef-448d-8b4a-0ecb0b78e5e7/download"
                            }
                        ]
                    },
                    {
                        "Id": "711f7837-041d-4633-81d8-7c2ed1dfb1f2",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 779,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/receipts/f818d8c1-a01b-458a-af52-af8600b3b100/files/711f7837-041d-4633-81d8-7c2ed1dfb1f2/download"
                            }
                        ]
                    },
                    {
                        "Id": "d627f2ad-503d-406b-8e2a-c1f92a85f006",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "711f7837-041d-4633-81d8-7c2ed1dfb1f2",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/receipts/f818d8c1-a01b-458a-af52-af8600b3b100/files/d627f2ad-503d-406b-8e2a-c1f92a85f006/download"
                            }
                        ]
                    },
                    {
                        "Id": "aff7e76c-fa1e-4c63-86e0-dafdfc934b80",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "5b893c45-baef-448d-8b4a-0ecb0b78e5e7",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/38a9fd73-b9b0-4775-bd1d-af8600b0f1c6/receipts/f818d8c1-a01b-458a-af52-af8600b3b100/files/aff7e76c-fa1e-4c63-86e0-dafdfc934b80/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "064b6883-b2e8-4fdb-b353-af8600ba1db5",
        "CorrelationId": "f30ed7d8-ef94-49d5-844c-af8600a59512",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос ЦИК: Уточнение запроса (Квитанция)",
        "Text": null,
        "CreationDate": "2023-01-10T11:17:37Z",
        "UpdatedDate": "2023-01-10T11:18:05Z",
        "Status": "success",
        "TaskName": "Zadacha_58",
        "RegNumber": null,
        "TotalSize": 622,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "42709522-a00c-4e4e-850e-af8600ba1dfb",
                "Name": "K1027800000095_100123_141455_K_0001_1000_F1027700466640.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 622,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/files/42709522-a00c-4e4e-850e-af8600ba1dfb/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "045e4398-9076-4628-a8c3-af8600ba1f0a",
                "ReceiveTime": "2023-01-10T11:17:38Z",
                "StatusTime": "2023-01-10T11:17:38Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "cf50af94-e7b9-4b8d-a381-af8600ba3412",
                "ReceiveTime": "2023-01-10T11:17:56Z",
                "StatusTime": "2023-01-10T11:17:52Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "49148f0d-419c-4d99-9e65-04cfad8fe7d1",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "3b04e670-7ac6-4ec1-9aca-525df5dd1a1c",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/receipts/cf50af94-e7b9-4b8d-a381-af8600ba3412/files/49148f0d-419c-4d99-9e65-04cfad8fe7d1/download"
                            }
                        ]
                    },
                    {
                        "Id": "567ca2fa-4434-4657-b407-1031822cc29d",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 956,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/receipts/cf50af94-e7b9-4b8d-a381-af8600ba3412/files/567ca2fa-4434-4657-b407-1031822cc29d/download"
                            }
                        ]
                    },
                    {
                        "Id": "3b926534-174f-4923-bcf2-4ab021cc1f33",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "567ca2fa-4434-4657-b407-1031822cc29d",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/receipts/cf50af94-e7b9-4b8d-a381-af8600ba3412/files/3b926534-174f-4923-bcf2-4ab021cc1f33/download"
                            }
                        ]
                    },
                    {
                        "Id": "3b04e670-7ac6-4ec1-9aca-525df5dd1a1c",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/receipts/cf50af94-e7b9-4b8d-a381-af8600ba3412/files/3b04e670-7ac6-4ec1-9aca-525df5dd1a1c/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "2cd3150b-663b-4920-984e-af8600ba3cbb",
                "ReceiveTime": "2023-01-10T11:18:04Z",
                "StatusTime": "2023-01-10T11:17:56Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "8d3e242b-ed74-4056-829e-105bc12a5747",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 957,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/receipts/2cd3150b-663b-4920-984e-af8600ba3cbb/files/8d3e242b-ed74-4056-829e-105bc12a5747/download"
                            }
                        ]
                    },
                    {
                        "Id": "d9f85e19-61b9-480d-a5fa-6caa83144b8f",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "940d8dd1-b27b-4e4a-87c0-da02f51069ed",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/receipts/2cd3150b-663b-4920-984e-af8600ba3cbb/files/d9f85e19-61b9-480d-a5fa-6caa83144b8f/download"
                            }
                        ]
                    },
                    {
                        "Id": "3621a4a2-faa3-46de-8682-c531997e4eb3",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "8d3e242b-ed74-4056-829e-105bc12a5747",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/receipts/2cd3150b-663b-4920-984e-af8600ba3cbb/files/3621a4a2-faa3-46de-8682-c531997e4eb3/download"
                            }
                        ]
                    },
                    {
                        "Id": "940d8dd1-b27b-4e4a-87c0-da02f51069ed",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/receipts/2cd3150b-663b-4920-984e-af8600ba3cbb/files/940d8dd1-b27b-4e4a-87c0-da02f51069ed/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "46088264-7518-4138-9b22-af8600ba3eb6",
                "ReceiveTime": "2023-01-10T11:18:05Z",
                "StatusTime": "2023-01-10T11:18:05Z",
                "Status": "success",
                "Message": null,
                "Files": [
                    {
                        "Id": "7d7ab269-e393-4b24-a20a-0e0dbb1fd695",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 141,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/receipts/46088264-7518-4138-9b22-af8600ba3eb6/files/7d7ab269-e393-4b24-a20a-0e0dbb1fd695/download"
                            }
                        ]
                    },
                    {
                        "Id": "e492a8d8-aa1e-45bf-8b30-1f3e87045e1e",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "cf76269d-bb0c-4a44-845d-a44b8caaba9c",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/receipts/46088264-7518-4138-9b22-af8600ba3eb6/files/e492a8d8-aa1e-45bf-8b30-1f3e87045e1e/download"
                            }
                        ]
                    },
                    {
                        "Id": "77e0e0e2-fd58-4b10-9806-2abe55e337d3",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "7d7ab269-e393-4b24-a20a-0e0dbb1fd695",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/receipts/46088264-7518-4138-9b22-af8600ba3eb6/files/77e0e0e2-fd58-4b10-9806-2abe55e337d3/download"
                            }
                        ]
                    },
                    {
                        "Id": "cf76269d-bb0c-4a44-845d-a44b8caaba9c",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1058,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/064b6883-b2e8-4fdb-b353-af8600ba1db5/receipts/46088264-7518-4138-9b22-af8600ba3eb6/files/cf76269d-bb0c-4a44-845d-a44b8caaba9c/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "fcf05205-f09a-4df5-9606-af8600f81dc5",
        "CorrelationId": "68e4acb7-8362-4f87-8e2a-af7400eb78da",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "сообщение об ознакомлении с актом проверки",
        "CreationDate": "2023-01-10T15:03:21Z",
        "UpdatedDate": "2023-01-11T05:32:12Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2-1361",
        "TotalSize": 103818,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "bee0ad5c-1681-41ae-88d8-af8600f81e83",
                "Name": "2023-01-10-2-9 сообщение об ознакомлении.pdf.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 89723,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/files/bee0ad5c-1681-41ae-88d8-af8600f81e83/download"
                    }
                ]
            },
            {
                "Id": "8aeb8cd5-2278-4d1a-b382-af8600f81e91",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2013,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/files/8aeb8cd5-2278-4d1a-b382-af8600f81e91/download"
                    }
                ]
            },
            {
                "Id": "132e9265-05a4-405d-b81a-af8600f86c71",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "8aeb8cd5-2278-4d1a-b382-af8600f81e91",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/files/132e9265-05a4-405d-b81a-af8600f86c71/download"
                    }
                ]
            },
            {
                "Id": "da0e85b6-c7f3-4f51-9a72-af8600f8b6fa",
                "Name": "2023-01-10-2-9 сообщение об ознакомлении.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "bee0ad5c-1681-41ae-88d8-af8600f81e83",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/files/da0e85b6-c7f3-4f51-9a72-af8600f8b6fa/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "e14603d6-5723-452b-9736-af8600f92a9f",
                "ReceiveTime": "2023-01-10T15:07:11Z",
                "StatusTime": "2023-01-10T15:07:11Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "d59969d9-4d7e-46df-b205-af8600f933a9",
                "ReceiveTime": "2023-01-10T15:07:18Z",
                "StatusTime": "2023-01-10T15:07:17Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "1b007b6a-85f9-4716-8185-037fd9141c61",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/receipts/d59969d9-4d7e-46df-b205-af8600f933a9/files/1b007b6a-85f9-4716-8185-037fd9141c61/download"
                            }
                        ]
                    },
                    {
                        "Id": "4f541e2a-0dad-4d72-87ef-6d289bccf6cd",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "1b007b6a-85f9-4716-8185-037fd9141c61",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/receipts/d59969d9-4d7e-46df-b205-af8600f933a9/files/4f541e2a-0dad-4d72-87ef-6d289bccf6cd/download"
                            }
                        ]
                    },
                    {
                        "Id": "d828a21f-a21e-4b08-a27f-c4776b4ff300",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "ac964264-d03a-4f65-a6f8-e62fcc570f8c",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/receipts/d59969d9-4d7e-46df-b205-af8600f933a9/files/d828a21f-a21e-4b08-a27f-c4776b4ff300/download"
                            }
                        ]
                    },
                    {
                        "Id": "ac964264-d03a-4f65-a6f8-e62fcc570f8c",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/receipts/d59969d9-4d7e-46df-b205-af8600f933a9/files/ac964264-d03a-4f65-a6f8-e62fcc570f8c/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "062e39dc-aee2-4171-bbe8-af8600f93986",
                "ReceiveTime": "2023-01-10T15:07:23Z",
                "StatusTime": "2023-01-10T15:07:18Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "0e8dcce4-e489-4603-98d1-076f9db25fe1",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/receipts/062e39dc-aee2-4171-bbe8-af8600f93986/files/0e8dcce4-e489-4603-98d1-076f9db25fe1/download"
                            }
                        ]
                    },
                    {
                        "Id": "3e4b7b7f-9690-48f2-9fe1-170c81adc682",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/receipts/062e39dc-aee2-4171-bbe8-af8600f93986/files/3e4b7b7f-9690-48f2-9fe1-170c81adc682/download"
                            }
                        ]
                    },
                    {
                        "Id": "9b39ce66-5fca-4084-bead-2acb8be9fb44",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "0e8dcce4-e489-4603-98d1-076f9db25fe1",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/receipts/062e39dc-aee2-4171-bbe8-af8600f93986/files/9b39ce66-5fca-4084-bead-2acb8be9fb44/download"
                            }
                        ]
                    },
                    {
                        "Id": "47449097-4a51-4549-8dae-c0e8e083cfa8",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "3e4b7b7f-9690-48f2-9fe1-170c81adc682",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/receipts/062e39dc-aee2-4171-bbe8-af8600f93986/files/47449097-4a51-4549-8dae-c0e8e083cfa8/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "80445ff9-fcad-44af-8264-af87005b3e28",
                "ReceiveTime": "2023-01-11T05:32:12Z",
                "StatusTime": "2023-01-11T05:32:02Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "542c56b8-1cbb-45bc-86ef-3458cf1a0a88",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 355,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/receipts/80445ff9-fcad-44af-8264-af87005b3e28/files/542c56b8-1cbb-45bc-86ef-3458cf1a0a88/download"
                            }
                        ]
                    },
                    {
                        "Id": "80eb87bc-3c99-4aff-a36f-7c0952883fff",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "542c56b8-1cbb-45bc-86ef-3458cf1a0a88",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/receipts/80445ff9-fcad-44af-8264-af87005b3e28/files/80eb87bc-3c99-4aff-a36f-7c0952883fff/download"
                            }
                        ]
                    },
                    {
                        "Id": "6c78c936-89e8-48d8-b750-b9b5a0f69b1e",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "8ab8f157-59bf-401b-b524-f302970e8274",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/receipts/80445ff9-fcad-44af-8264-af87005b3e28/files/6c78c936-89e8-48d8-b750-b9b5a0f69b1e/download"
                            }
                        ]
                    },
                    {
                        "Id": "8ab8f157-59bf-401b-b524-f302970e8274",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 780,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fcf05205-f09a-4df5-9606-af8600f81dc5/receipts/80445ff9-fcad-44af-8264-af87005b3e28/files/8ab8f157-59bf-401b-b524-f302970e8274/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "29c96bec-54e2-4006-965a-af8601138602",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-01-10T16:43:17Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 2192811,
        "Sender": null,
        "Files": [
            {
                "Id": "055fc3db-40ee-43e6-be93-0a5c43bbf59e",
                "Name": "KYC_20230110.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "94577149-cc1b-457f-aa96-b9be9ed37657",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/29c96bec-54e2-4006-965a-af8601138602/files/055fc3db-40ee-43e6-be93-0a5c43bbf59e/download"
                    }
                ]
            },
            {
                "Id": "94577149-cc1b-457f-aa96-b9be9ed37657",
                "Name": "KYC_20230110.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2189550,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/29c96bec-54e2-4006-965a-af8601138602/files/94577149-cc1b-457f-aa96-b9be9ed37657/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "e1525bf9-ee7e-468b-9b9b-af8601289ec4",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-10T17:59:57Z",
        "UpdatedDate": "2023-01-10T18:04:45Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00567129",
        "TotalSize": 13371,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "a3baf068-cb96-4190-b62b-af8601289ec0",
                "Name": "KYCCL_7831001422_3194_20230110_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9499,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e1525bf9-ee7e-468b-9b9b-af8601289ec4/files/a3baf068-cb96-4190-b62b-af8601289ec0/download"
                    }
                ]
            },
            {
                "Id": "07962bbf-f2f3-4d19-a01a-af8601289ec2",
                "Name": "KYCCL_7831001422_3194_20230110_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "a3baf068-cb96-4190-b62b-af8601289ec0",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e1525bf9-ee7e-468b-9b9b-af8601289ec4/files/07962bbf-f2f3-4d19-a01a-af8601289ec2/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "ddf4ad61-db5c-4055-8111-af860128a032",
                "ReceiveTime": "2023-01-10T17:59:58Z",
                "StatusTime": "2023-01-10T17:59:58Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "3b15cebd-005c-48b3-be4b-af860128b1f5",
                "ReceiveTime": "2023-01-10T18:00:14Z",
                "StatusTime": "2023-01-10T18:00:10Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "bb0c66b2-9b7d-45a6-a9ae-2c0da04a4c12",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e1525bf9-ee7e-468b-9b9b-af8601289ec4/receipts/3b15cebd-005c-48b3-be4b-af860128b1f5/files/bb0c66b2-9b7d-45a6-a9ae-2c0da04a4c12/download"
                            }
                        ]
                    },
                    {
                        "Id": "c4f53ad0-5e2b-4c60-b474-7079dbbc0ba5",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e1525bf9-ee7e-468b-9b9b-af8601289ec4/receipts/3b15cebd-005c-48b3-be4b-af860128b1f5/files/c4f53ad0-5e2b-4c60-b474-7079dbbc0ba5/download"
                            }
                        ]
                    },
                    {
                        "Id": "7ea50130-c86c-4baa-8f34-9e3ae1f3dbb5",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "c4f53ad0-5e2b-4c60-b474-7079dbbc0ba5",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e1525bf9-ee7e-468b-9b9b-af8601289ec4/receipts/3b15cebd-005c-48b3-be4b-af860128b1f5/files/7ea50130-c86c-4baa-8f34-9e3ae1f3dbb5/download"
                            }
                        ]
                    },
                    {
                        "Id": "9d3ac9ae-2810-4959-be53-e2b3f8557b3b",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "bb0c66b2-9b7d-45a6-a9ae-2c0da04a4c12",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e1525bf9-ee7e-468b-9b9b-af8601289ec4/receipts/3b15cebd-005c-48b3-be4b-af860128b1f5/files/9d3ac9ae-2810-4959-be53-e2b3f8557b3b/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "82b27acb-7ad3-484a-91e6-af860128ba50",
                "ReceiveTime": "2023-01-10T18:00:21Z",
                "StatusTime": "2023-01-10T18:00:13Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "3d2a282b-d7c6-4a3e-ae82-28266134f628",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "87865984-ba09-405e-b7bd-936f87855029",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e1525bf9-ee7e-468b-9b9b-af8601289ec4/receipts/82b27acb-7ad3-484a-91e6-af860128ba50/files/3d2a282b-d7c6-4a3e-ae82-28266134f628/download"
                            }
                        ]
                    },
                    {
                        "Id": "fbe24a13-87fa-4b59-b5d7-310aad15ddeb",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "a489372e-b085-4bd3-a39b-4d536360384f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e1525bf9-ee7e-468b-9b9b-af8601289ec4/receipts/82b27acb-7ad3-484a-91e6-af860128ba50/files/fbe24a13-87fa-4b59-b5d7-310aad15ddeb/download"
                            }
                        ]
                    },
                    {
                        "Id": "a489372e-b085-4bd3-a39b-4d536360384f",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e1525bf9-ee7e-468b-9b9b-af8601289ec4/receipts/82b27acb-7ad3-484a-91e6-af860128ba50/files/a489372e-b085-4bd3-a39b-4d536360384f/download"
                            }
                        ]
                    },
                    {
                        "Id": "87865984-ba09-405e-b7bd-936f87855029",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e1525bf9-ee7e-468b-9b9b-af8601289ec4/receipts/82b27acb-7ad3-484a-91e6-af860128ba50/files/87865984-ba09-405e-b7bd-936f87855029/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "b83a42f9-5aba-4076-8bdd-af860129efee",
                "ReceiveTime": "2023-01-10T18:04:45Z",
                "StatusTime": "2023-01-10T18:03:33Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "528c0998-19bc-42d3-af73-774c5f5cda20",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e1525bf9-ee7e-468b-9b9b-af8601289ec4/receipts/b83a42f9-5aba-4076-8bdd-af860129efee/files/528c0998-19bc-42d3-af73-774c5f5cda20/download"
                            }
                        ]
                    },
                    {
                        "Id": "e67a3333-8959-4bbf-8652-79f3de1079d5",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "528c0998-19bc-42d3-af73-774c5f5cda20",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e1525bf9-ee7e-468b-9b9b-af8601289ec4/receipts/b83a42f9-5aba-4076-8bdd-af860129efee/files/e67a3333-8959-4bbf-8652-79f3de1079d5/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "fa9d71cd-e135-47a6-bf5e-af8700aff276",
        "CorrelationId": "1f6158a2-a7a1-4e14-aace-af7a00f65145",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "в дополнение к нашему исх. 1-3 от 09.01.2023",
        "CreationDate": "2023-01-11T10:40:36Z",
        "UpdatedDate": "2023-01-11T10:53:31Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "7251",
        "TotalSize": 957036,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "0e3be0ac-f493-431b-8b56-af8700aff348",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2003,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/files/0e3be0ac-f493-431b-8b56-af8700aff348/download"
                    }
                ]
            },
            {
                "Id": "474b2e7c-e554-45a2-b3e5-af8700aff356",
                "Name": "2023-01-09-1-3-2 ОСВ.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 942951,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/files/474b2e7c-e554-45a2-b3e5-af8700aff356/download"
                    }
                ]
            },
            {
                "Id": "52bbc066-fe2d-4369-af46-af8700b041b4",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "0e3be0ac-f493-431b-8b56-af8700aff348",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/files/52bbc066-fe2d-4369-af46-af8700b041b4/download"
                    }
                ]
            },
            {
                "Id": "aa5d9e13-9225-47d1-bf61-af8700b08c05",
                "Name": "2023-01-09-1-3-2 ОСВ.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "474b2e7c-e554-45a2-b3e5-af8700aff356",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/files/aa5d9e13-9225-47d1-bf61-af8700b08c05/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "294ba0f2-3c07-43b0-8493-af8700b08dc3",
                "ReceiveTime": "2023-01-11T10:42:48Z",
                "StatusTime": "2023-01-11T10:42:48Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "099e3352-594b-4350-8e22-af8700b0a1c2",
                "ReceiveTime": "2023-01-11T10:43:05Z",
                "StatusTime": "2023-01-11T10:43:01Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "36f3da41-8832-4e31-b746-40d612b64ef3",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/receipts/099e3352-594b-4350-8e22-af8700b0a1c2/files/36f3da41-8832-4e31-b746-40d612b64ef3/download"
                            }
                        ]
                    },
                    {
                        "Id": "5fe534f4-e5c7-48c6-93b0-571bbf541508",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "36f3da41-8832-4e31-b746-40d612b64ef3",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/receipts/099e3352-594b-4350-8e22-af8700b0a1c2/files/5fe534f4-e5c7-48c6-93b0-571bbf541508/download"
                            }
                        ]
                    },
                    {
                        "Id": "aa98a33b-35d3-4622-8e36-6966f774db92",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/receipts/099e3352-594b-4350-8e22-af8700b0a1c2/files/aa98a33b-35d3-4622-8e36-6966f774db92/download"
                            }
                        ]
                    },
                    {
                        "Id": "899daa81-c7c7-4f67-bb79-8d3f2a0b3843",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "aa98a33b-35d3-4622-8e36-6966f774db92",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/receipts/099e3352-594b-4350-8e22-af8700b0a1c2/files/899daa81-c7c7-4f67-bb79-8d3f2a0b3843/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "92dd69c5-f1d8-4bfc-9b29-af8700b0aa40",
                "ReceiveTime": "2023-01-11T10:43:13Z",
                "StatusTime": "2023-01-11T10:43:05Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "66d39fe6-621b-4502-b8ee-6f9c60992edf",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/receipts/92dd69c5-f1d8-4bfc-9b29-af8700b0aa40/files/66d39fe6-621b-4502-b8ee-6f9c60992edf/download"
                            }
                        ]
                    },
                    {
                        "Id": "1bdf6b7b-e343-4128-a5f9-a8d42a27649f",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/receipts/92dd69c5-f1d8-4bfc-9b29-af8700b0aa40/files/1bdf6b7b-e343-4128-a5f9-a8d42a27649f/download"
                            }
                        ]
                    },
                    {
                        "Id": "bf9643a2-181c-4b3d-91ea-dc6d38c4f0bd",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "66d39fe6-621b-4502-b8ee-6f9c60992edf",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/receipts/92dd69c5-f1d8-4bfc-9b29-af8700b0aa40/files/bf9643a2-181c-4b3d-91ea-dc6d38c4f0bd/download"
                            }
                        ]
                    },
                    {
                        "Id": "e238080e-d5fa-46eb-a48d-e31ef98b37f6",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "1bdf6b7b-e343-4128-a5f9-a8d42a27649f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/receipts/92dd69c5-f1d8-4bfc-9b29-af8700b0aa40/files/e238080e-d5fa-46eb-a48d-e31ef98b37f6/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "305ad062-0462-4c1b-8285-af8700b37f5c",
                "ReceiveTime": "2023-01-11T10:53:31Z",
                "StatusTime": "2023-01-11T10:50:02Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "e6f084e0-f2e9-4974-b6e2-5906b4e6e949",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "60acbe15-536e-48a7-999f-c98216934209",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/receipts/305ad062-0462-4c1b-8285-af8700b37f5c/files/e6f084e0-f2e9-4974-b6e2-5906b4e6e949/download"
                            }
                        ]
                    },
                    {
                        "Id": "7e4afc61-7a2b-4d51-ba68-68cb596d366b",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 741,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/receipts/305ad062-0462-4c1b-8285-af8700b37f5c/files/7e4afc61-7a2b-4d51-ba68-68cb596d366b/download"
                            }
                        ]
                    },
                    {
                        "Id": "60acbe15-536e-48a7-999f-c98216934209",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 353,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/receipts/305ad062-0462-4c1b-8285-af8700b37f5c/files/60acbe15-536e-48a7-999f-c98216934209/download"
                            }
                        ]
                    },
                    {
                        "Id": "d4614f50-7e08-4a76-8fc6-d4c9a13c4716",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "7e4afc61-7a2b-4d51-ba68-68cb596d366b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/fa9d71cd-e135-47a6-bf5e-af8700aff276/receipts/305ad062-0462-4c1b-8285-af8700b37f5c/files/d4614f50-7e08-4a76-8fc6-d4c9a13c4716/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "bb8b34c2-32c2-46fc-85cc-af8700b1678e",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Обращение (запрос)  в Банк России",
        "Text": "в дополнение к нашему исх. 1-3-1 от 09.01.2023",
        "CreationDate": "2023-01-11T10:45:54Z",
        "UpdatedDate": "2023-01-11T10:56:22Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2-1758",
        "TotalSize": 957405,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "037ee7e9-e06a-4710-89c9-af8700b167b4",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2372,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/files/037ee7e9-e06a-4710-89c9-af8700b167b4/download"
                    }
                ]
            },
            {
                "Id": "d2583b7e-f410-4b83-aec7-af8700b167c8",
                "Name": "2023-01-09-1-3-3 ОСВ.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 942951,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/files/d2583b7e-f410-4b83-aec7-af8700b167c8/download"
                    }
                ]
            },
            {
                "Id": "354d7265-4020-4b97-ae67-af8700b1b50a",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "037ee7e9-e06a-4710-89c9-af8700b167b4",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/files/354d7265-4020-4b97-ae67-af8700b1b50a/download"
                    }
                ]
            },
            {
                "Id": "e02a3aec-f630-49be-8dc3-af8700b20087",
                "Name": "2023-01-09-1-3-3 ОСВ.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "d2583b7e-f410-4b83-aec7-af8700b167c8",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/files/e02a3aec-f630-49be-8dc3-af8700b20087/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "493f63c7-d2c2-4ba6-b545-af8700b2037e",
                "ReceiveTime": "2023-01-11T10:48:07Z",
                "StatusTime": "2023-01-11T10:48:07Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "ad3a208b-a969-465d-a1b9-af8700b216b6",
                "ReceiveTime": "2023-01-11T10:48:24Z",
                "StatusTime": "2023-01-11T10:48:20Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "e6e5407e-cc77-49f0-874d-3aee9fee900b",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "5b2fa7b3-9d35-4594-903f-949397ef6788",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/receipts/ad3a208b-a969-465d-a1b9-af8700b216b6/files/e6e5407e-cc77-49f0-874d-3aee9fee900b/download"
                            }
                        ]
                    },
                    {
                        "Id": "6b711d6d-48f0-468b-860d-579b2ff38790",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "4844451a-017b-46cb-bdaa-efb556d8db5b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/receipts/ad3a208b-a969-465d-a1b9-af8700b216b6/files/6b711d6d-48f0-468b-860d-579b2ff38790/download"
                            }
                        ]
                    },
                    {
                        "Id": "5b2fa7b3-9d35-4594-903f-949397ef6788",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/receipts/ad3a208b-a969-465d-a1b9-af8700b216b6/files/5b2fa7b3-9d35-4594-903f-949397ef6788/download"
                            }
                        ]
                    },
                    {
                        "Id": "4844451a-017b-46cb-bdaa-efb556d8db5b",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/receipts/ad3a208b-a969-465d-a1b9-af8700b216b6/files/4844451a-017b-46cb-bdaa-efb556d8db5b/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "eb7825cb-e2ee-4ec5-be57-af8700b22104",
                "ReceiveTime": "2023-01-11T10:48:32Z",
                "StatusTime": "2023-01-11T10:48:23Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "62eead6a-6967-4bd0-8f89-2920ab0156cd",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/receipts/eb7825cb-e2ee-4ec5-be57-af8700b22104/files/62eead6a-6967-4bd0-8f89-2920ab0156cd/download"
                            }
                        ]
                    },
                    {
                        "Id": "237c1b76-cbe1-4148-8e21-6bcb26ac0ef8",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/receipts/eb7825cb-e2ee-4ec5-be57-af8700b22104/files/237c1b76-cbe1-4148-8e21-6bcb26ac0ef8/download"
                            }
                        ]
                    },
                    {
                        "Id": "b4b2e1ac-d5f3-4921-896d-6f4a4e6f82ae",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "237c1b76-cbe1-4148-8e21-6bcb26ac0ef8",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/receipts/eb7825cb-e2ee-4ec5-be57-af8700b22104/files/b4b2e1ac-d5f3-4921-896d-6f4a4e6f82ae/download"
                            }
                        ]
                    },
                    {
                        "Id": "ba4f2534-717c-47ca-8da3-e6dca2f71fed",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "62eead6a-6967-4bd0-8f89-2920ab0156cd",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/receipts/eb7825cb-e2ee-4ec5-be57-af8700b22104/files/ba4f2534-717c-47ca-8da3-e6dca2f71fed/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "87d4e0ba-1400-4c75-92fa-af8700b446f3",
                "ReceiveTime": "2023-01-11T10:56:22Z",
                "StatusTime": "2023-01-11T10:56:02Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "eb75567a-9c2a-4f9c-8ff3-1059d9d8660d",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 780,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/receipts/87d4e0ba-1400-4c75-92fa-af8700b446f3/files/eb75567a-9c2a-4f9c-8ff3-1059d9d8660d/download"
                            }
                        ]
                    },
                    {
                        "Id": "e1849a39-75b1-47f6-9e84-6e82f204538a",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 355,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/receipts/87d4e0ba-1400-4c75-92fa-af8700b446f3/files/e1849a39-75b1-47f6-9e84-6e82f204538a/download"
                            }
                        ]
                    },
                    {
                        "Id": "98b1c3e0-5889-4f18-8be5-c36ef011448e",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "eb75567a-9c2a-4f9c-8ff3-1059d9d8660d",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/receipts/87d4e0ba-1400-4c75-92fa-af8700b446f3/files/98b1c3e0-5889-4f18-8be5-c36ef011448e/download"
                            }
                        ]
                    },
                    {
                        "Id": "5ae310cd-26a6-4411-ac16-fd300de19bff",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "e1849a39-75b1-47f6-9e84-6e82f204538a",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bb8b34c2-32c2-46fc-85cc-af8700b1678e/receipts/87d4e0ba-1400-4c75-92fa-af8700b446f3/files/5ae310cd-26a6-4411-ac16-fd300de19bff/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "6599d20e-60c0-4ac6-8329-af8700b9a270",
        "CorrelationId": null,
        "GroupId": "29390e17-d59c-4010-9db0-481950e84cd7",
        "Type": "inbox",
        "Title": "№ 20-2-1/6 от 11/01/2023 (20) Письма Деп-та денежно-кредитной политики",
        "Text": "Об информации, размещенной на официальном сайте Банка России",
        "CreationDate": "2023-01-11T11:16:00Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "20-2-1/6",
        "TotalSize": 501092,
        "Sender": null,
        "Files": [
            {
                "Id": "ac01f454-4ea9-4c47-bcf6-1f7a7ed3d2e3",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "282e60b1-8c4d-41cd-8582-d672c56d0185",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6599d20e-60c0-4ac6-8329-af8700b9a270/files/ac01f454-4ea9-4c47-bcf6-1f7a7ed3d2e3/download"
                    }
                ]
            },
            {
                "Id": "d944d501-ab5a-41e2-be01-25e883c51ef9",
                "Name": "ЭД_Письмо.pdf",
                "Description": "612613485.pdf",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 110523,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6599d20e-60c0-4ac6-8329-af8700b9a270/files/d944d501-ab5a-41e2-be01-25e883c51ef9/download"
                    }
                ]
            },
            {
                "Id": "1a445343-8b96-4eb3-a891-8e3b8cfe0839",
                "Name": "ЭД_Письмо.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "d944d501-ab5a-41e2-be01-25e883c51ef9",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6599d20e-60c0-4ac6-8329-af8700b9a270/files/1a445343-8b96-4eb3-a891-8e3b8cfe0839/download"
                    }
                ]
            },
            {
                "Id": "ffe306ed-ad48-4e4e-a464-ab331dfef687",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 268864,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6599d20e-60c0-4ac6-8329-af8700b9a270/files/ffe306ed-ad48-4e4e-a464-ab331dfef687/download"
                    }
                ]
            },
            {
                "Id": "282e60b1-8c4d-41cd-8582-d672c56d0185",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "612614711.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 115183,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6599d20e-60c0-4ac6-8329-af8700b9a270/files/282e60b1-8c4d-41cd-8582-d672c56d0185/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "2f716831-22ef-4bad-820d-af8700c7cd8b",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК в организацию. Ответ ЦИК в организацию.",
        "Text": "",
        "CreationDate": "2023-01-11T12:07:27Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_56",
        "RegNumber": null,
        "TotalSize": 3916,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "8e3c723f-388b-4760-8871-af8700c6a91f",
                "Name": "F1027700466640_110123_142241_K_0001_2000_K1027800000095.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 655,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/2f716831-22ef-4bad-820d-af8700c7cd8b/files/8e3c723f-388b-4760-8871-af8700c6a91f/download"
                    }
                ]
            },
            {
                "Id": "03d93c2e-bc61-4585-91b5-f985d6856c0e",
                "Name": "F1027700466640_110123_142241_K_0001_2000_K1027800000095.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "8e3c723f-388b-4760-8871-af8700c6a91f",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/2f716831-22ef-4bad-820d-af8700c7cd8b/files/03d93c2e-bc61-4585-91b5-f985d6856c0e/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "054c7940-53b6-4c55-bb52-af8700d49cd5",
        "CorrelationId": "24050721-2871-4320-b6bf-af8500965f62",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "предоставление запрошенной информации",
        "CreationDate": "2023-01-11T12:54:05Z",
        "UpdatedDate": "2023-01-11T13:01:08Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "7805",
        "TotalSize": 1804048,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "24f6706f-1b24-431c-be58-af8700d49d5b",
                "Name": "01-01-2023 ОД1.pdf",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 457430,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/files/24f6706f-1b24-431c-be58-af8700d49d5b/download"
                    }
                ]
            },
            {
                "Id": "c8aea7fe-c2aa-4d96-b427-af8700d49d62",
                "Name": "01-01-2023 ОД.pdf",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 887966,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/files/c8aea7fe-c2aa-4d96-b427-af8700d49d62/download"
                    }
                ]
            },
            {
                "Id": "dd882dcf-50d8-4315-9320-af8700d49d80",
                "Name": "01-01-2023 ОД сопровод.pdf",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 433597,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/files/dd882dcf-50d8-4315-9320-af8700d49d80/download"
                    }
                ]
            },
            {
                "Id": "6854c679-ff79-4996-9e09-af8700d49d82",
                "Name": "form.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 891,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/files/6854c679-ff79-4996-9e09-af8700d49d82/download"
                    }
                ]
            },
            {
                "Id": "cc4aee65-1fa7-49d2-ba47-af8700d4eb4f",
                "Name": "01-01-2023 ОД.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "c8aea7fe-c2aa-4d96-b427-af8700d49d62",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/files/cc4aee65-1fa7-49d2-ba47-af8700d4eb4f/download"
                    }
                ]
            },
            {
                "Id": "7766f569-2732-43f6-813d-af8700d53674",
                "Name": "01-01-2023 ОД1.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "24f6706f-1b24-431c-be58-af8700d49d5b",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/files/7766f569-2732-43f6-813d-af8700d53674/download"
                    }
                ]
            },
            {
                "Id": "1de87d1c-4830-4c09-882e-af8700d5824d",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "6854c679-ff79-4996-9e09-af8700d49d82",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/files/1de87d1c-4830-4c09-882e-af8700d5824d/download"
                    }
                ]
            },
            {
                "Id": "0ba07811-ccdb-4ffc-b6bc-af8700d5ccbb",
                "Name": "01-01-2023 ОД сопровод.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "dd882dcf-50d8-4315-9320-af8700d49d80",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/files/0ba07811-ccdb-4ffc-b6bc-af8700d5ccbb/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "9cb306ad-4376-44f6-a1a2-af8700d5d035",
                "ReceiveTime": "2023-01-11T12:58:28Z",
                "StatusTime": "2023-01-11T12:58:28Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "7a0f6827-01aa-4572-8612-af8700d5dd63",
                "ReceiveTime": "2023-01-11T12:58:39Z",
                "StatusTime": "2023-01-11T12:58:35Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "7ec9b551-80b6-4306-a3e9-0d98c23d2582",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/receipts/7a0f6827-01aa-4572-8612-af8700d5dd63/files/7ec9b551-80b6-4306-a3e9-0d98c23d2582/download"
                            }
                        ]
                    },
                    {
                        "Id": "03d0faf7-f6eb-411b-b29c-aa452424a81d",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "7ec9b551-80b6-4306-a3e9-0d98c23d2582",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/receipts/7a0f6827-01aa-4572-8612-af8700d5dd63/files/03d0faf7-f6eb-411b-b29c-aa452424a81d/download"
                            }
                        ]
                    },
                    {
                        "Id": "44cb5a19-7983-4d08-bfa8-bd560ab5e95d",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/receipts/7a0f6827-01aa-4572-8612-af8700d5dd63/files/44cb5a19-7983-4d08-bfa8-bd560ab5e95d/download"
                            }
                        ]
                    },
                    {
                        "Id": "c80f9f79-c7fd-4b7d-9c70-c341514aea65",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "44cb5a19-7983-4d08-bfa8-bd560ab5e95d",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/receipts/7a0f6827-01aa-4572-8612-af8700d5dd63/files/c80f9f79-c7fd-4b7d-9c70-c341514aea65/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "c75dbe75-b64a-4ffc-955e-af8700d5e3f7",
                "ReceiveTime": "2023-01-11T12:58:45Z",
                "StatusTime": "2023-01-11T12:58:38Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "c311fecb-66f4-4fbe-a78f-1d7eff155c74",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/receipts/c75dbe75-b64a-4ffc-955e-af8700d5e3f7/files/c311fecb-66f4-4fbe-a78f-1d7eff155c74/download"
                            }
                        ]
                    },
                    {
                        "Id": "3ca54594-5539-4e22-9525-6a3055b1b9d3",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/receipts/c75dbe75-b64a-4ffc-955e-af8700d5e3f7/files/3ca54594-5539-4e22-9525-6a3055b1b9d3/download"
                            }
                        ]
                    },
                    {
                        "Id": "4d5be27c-172a-4ce1-8e8c-7851c6ac29b2",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "3ca54594-5539-4e22-9525-6a3055b1b9d3",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/receipts/c75dbe75-b64a-4ffc-955e-af8700d5e3f7/files/4d5be27c-172a-4ce1-8e8c-7851c6ac29b2/download"
                            }
                        ]
                    },
                    {
                        "Id": "1ce2194b-dd5f-4fdb-bfdf-a8f18c2887f7",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "c311fecb-66f4-4fbe-a78f-1d7eff155c74",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/receipts/c75dbe75-b64a-4ffc-955e-af8700d5e3f7/files/1ce2194b-dd5f-4fdb-bfdf-a8f18c2887f7/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "7c24ad83-e9b3-423f-a9ab-af8700d68bc5",
                "ReceiveTime": "2023-01-11T13:01:08Z",
                "StatusTime": "2023-01-11T13:01:01Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "c307cc73-209f-4280-8845-54a12ea21bd8",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "0fee246d-f504-4eb8-9b3b-6a7aee4c5ae0",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/receipts/7c24ad83-e9b3-423f-a9ab-af8700d68bc5/files/c307cc73-209f-4280-8845-54a12ea21bd8/download"
                            }
                        ]
                    },
                    {
                        "Id": "0fee246d-f504-4eb8-9b3b-6a7aee4c5ae0",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 353,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/receipts/7c24ad83-e9b3-423f-a9ab-af8700d68bc5/files/0fee246d-f504-4eb8-9b3b-6a7aee4c5ae0/download"
                            }
                        ]
                    },
                    {
                        "Id": "91b5cce3-7e9a-4c00-a485-d8e61203bab4",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 741,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/receipts/7c24ad83-e9b3-423f-a9ab-af8700d68bc5/files/91b5cce3-7e9a-4c00-a485-d8e61203bab4/download"
                            }
                        ]
                    },
                    {
                        "Id": "3c54977a-d3f6-433b-a1b8-fd394e1e87de",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "91b5cce3-7e9a-4c00-a485-d8e61203bab4",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/054c7940-53b6-4c55-bb52-af8700d49cd5/receipts/7c24ad83-e9b3-423f-a9ab-af8700d68bc5/files/3c54977a-d3f6-433b-a1b8-fd394e1e87de/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "72430a75-121d-42eb-8b71-af8700d7fcab",
        "CorrelationId": "2b08b11c-2ff5-472a-a409-af85008744ad",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "предоставление запрошенной информации",
        "CreationDate": "2023-01-11T13:06:23Z",
        "UpdatedDate": "2023-01-11T13:13:12Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2-1922",
        "TotalSize": 1237659,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "388ba92a-a88b-4c0f-8ebc-af8700d7fcdb",
                "Name": "01-01-2023 ЦБ.pdf",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 763577,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/files/388ba92a-a88b-4c0f-8ebc-af8700d7fcdb/download"
                    }
                ]
            },
            {
                "Id": "26a4c954-14de-4615-9715-af8700d7fcf4",
                "Name": "01-01-2023 ЦБ сопровод.pdf",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 455068,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/files/26a4c954-14de-4615-9715-af8700d7fcf4/download"
                    }
                ]
            },
            {
                "Id": "8a175ff1-66f2-4e39-8a19-af8700d7fcf7",
                "Name": "form.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 891,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/files/8a175ff1-66f2-4e39-8a19-af8700d7fcf7/download"
                    }
                ]
            },
            {
                "Id": "b0a62e01-bff3-4353-b5cf-af8700d84842",
                "Name": "01-01-2023 ЦБ сопровод.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "26a4c954-14de-4615-9715-af8700d7fcf4",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/files/b0a62e01-bff3-4353-b5cf-af8700d84842/download"
                    }
                ]
            },
            {
                "Id": "4eea081b-347e-4740-b1a4-af8700d8939c",
                "Name": "01-01-2023 ЦБ.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "388ba92a-a88b-4c0f-8ebc-af8700d7fcdb",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/files/4eea081b-347e-4740-b1a4-af8700d8939c/download"
                    }
                ]
            },
            {
                "Id": "d8d25d25-5193-4061-ac4a-af8700d8de31",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "8a175ff1-66f2-4e39-8a19-af8700d7fcf7",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/files/d8d25d25-5193-4061-ac4a-af8700d8de31/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "0b6aacb5-5524-42f5-b128-af8700d8e16c",
                "ReceiveTime": "2023-01-11T13:09:38Z",
                "StatusTime": "2023-01-11T13:09:38Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "bd0b225a-9e2f-4ce8-849f-af8700d8f244",
                "ReceiveTime": "2023-01-11T13:09:52Z",
                "StatusTime": "2023-01-11T13:09:49Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "09cdee29-211c-4637-b6fe-010aee5b0171",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/receipts/bd0b225a-9e2f-4ce8-849f-af8700d8f244/files/09cdee29-211c-4637-b6fe-010aee5b0171/download"
                            }
                        ]
                    },
                    {
                        "Id": "f97b7ece-fcce-4a30-9c57-0c63016d1537",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "1de31c2f-d55b-45b4-b0ef-e143996e6152",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/receipts/bd0b225a-9e2f-4ce8-849f-af8700d8f244/files/f97b7ece-fcce-4a30-9c57-0c63016d1537/download"
                            }
                        ]
                    },
                    {
                        "Id": "2a1a967c-7514-4ec3-936c-7a14953ca42c",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "09cdee29-211c-4637-b6fe-010aee5b0171",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/receipts/bd0b225a-9e2f-4ce8-849f-af8700d8f244/files/2a1a967c-7514-4ec3-936c-7a14953ca42c/download"
                            }
                        ]
                    },
                    {
                        "Id": "1de31c2f-d55b-45b4-b0ef-e143996e6152",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/receipts/bd0b225a-9e2f-4ce8-849f-af8700d8f244/files/1de31c2f-d55b-45b4-b0ef-e143996e6152/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "7106dd81-dd05-4afb-bbf9-af8700d8fa82",
                "ReceiveTime": "2023-01-11T13:09:59Z",
                "StatusTime": "2023-01-11T13:09:52Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "9e9220fe-2716-460a-9cbe-120ba4019203",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/receipts/7106dd81-dd05-4afb-bbf9-af8700d8fa82/files/9e9220fe-2716-460a-9cbe-120ba4019203/download"
                            }
                        ]
                    },
                    {
                        "Id": "c53b8ebb-424f-49df-bf17-275804c045a0",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/receipts/7106dd81-dd05-4afb-bbf9-af8700d8fa82/files/c53b8ebb-424f-49df-bf17-275804c045a0/download"
                            }
                        ]
                    },
                    {
                        "Id": "ad4fb67c-26dd-4adb-9167-54bdbae12604",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "9e9220fe-2716-460a-9cbe-120ba4019203",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/receipts/7106dd81-dd05-4afb-bbf9-af8700d8fa82/files/ad4fb67c-26dd-4adb-9167-54bdbae12604/download"
                            }
                        ]
                    },
                    {
                        "Id": "6ba83d53-f527-4dc5-8de1-5982fc5ea0ee",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "c53b8ebb-424f-49df-bf17-275804c045a0",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/receipts/7106dd81-dd05-4afb-bbf9-af8700d8fa82/files/6ba83d53-f527-4dc5-8de1-5982fc5ea0ee/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "1b6e65f6-5345-454a-82ce-af8700d9dc82",
                "ReceiveTime": "2023-01-11T13:13:12Z",
                "StatusTime": "2023-01-11T13:13:02Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "79ba07d8-d0b2-444f-9d2c-03f671e05314",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "01b3e4ed-8fd5-4501-80cc-c0e9e8ee8dcc",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/receipts/1b6e65f6-5345-454a-82ce-af8700d9dc82/files/79ba07d8-d0b2-444f-9d2c-03f671e05314/download"
                            }
                        ]
                    },
                    {
                        "Id": "b48b4a91-1080-4a70-a56b-62f59084d6e3",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 780,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/receipts/1b6e65f6-5345-454a-82ce-af8700d9dc82/files/b48b4a91-1080-4a70-a56b-62f59084d6e3/download"
                            }
                        ]
                    },
                    {
                        "Id": "797b4f37-1961-4b77-a515-66be8ce12e3d",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "b48b4a91-1080-4a70-a56b-62f59084d6e3",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/receipts/1b6e65f6-5345-454a-82ce-af8700d9dc82/files/797b4f37-1961-4b77-a515-66be8ce12e3d/download"
                            }
                        ]
                    },
                    {
                        "Id": "01b3e4ed-8fd5-4501-80cc-c0e9e8ee8dcc",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 355,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/72430a75-121d-42eb-8b71-af8700d7fcab/receipts/1b6e65f6-5345-454a-82ce-af8700d9dc82/files/01b3e4ed-8fd5-4501-80cc-c0e9e8ee8dcc/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "d335b29a-1f97-4c33-9d28-af870110b82d",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-01-11T16:33:05Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 2202825,
        "Sender": null,
        "Files": [
            {
                "Id": "fe40520d-f300-41d0-a9e5-19b30191ac42",
                "Name": "KYC_20230111.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "5c1bb97a-0584-411c-803a-793c4d2aff03",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/d335b29a-1f97-4c33-9d28-af870110b82d/files/fe40520d-f300-41d0-a9e5-19b30191ac42/download"
                    }
                ]
            },
            {
                "Id": "5c1bb97a-0584-411c-803a-793c4d2aff03",
                "Name": "KYC_20230111.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2199564,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/d335b29a-1f97-4c33-9d28-af870110b82d/files/5c1bb97a-0584-411c-803a-793c4d2aff03/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "b9381f81-03e3-46e1-9836-af8701289d85",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-11T17:59:56Z",
        "UpdatedDate": "2023-01-11T18:04:04Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00568011",
        "TotalSize": 13372,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "09bf4c7b-fb88-46c6-a883-af8701289d44",
                "Name": "KYCCL_7831001422_3194_20230111_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9500,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b9381f81-03e3-46e1-9836-af8701289d85/files/09bf4c7b-fb88-46c6-a883-af8701289d44/download"
                    }
                ]
            },
            {
                "Id": "fec6cbf2-baf1-4cf6-9121-af8701289d61",
                "Name": "KYCCL_7831001422_3194_20230111_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "09bf4c7b-fb88-46c6-a883-af8701289d44",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b9381f81-03e3-46e1-9836-af8701289d85/files/fec6cbf2-baf1-4cf6-9121-af8701289d61/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "960d92d3-03e3-4769-94b7-af8701289ec4",
                "ReceiveTime": "2023-01-11T17:59:57Z",
                "StatusTime": "2023-01-11T17:59:57Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "a4be7e70-accf-400a-a40f-af870128b027",
                "ReceiveTime": "2023-01-11T18:00:12Z",
                "StatusTime": "2023-01-11T18:00:08Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "900c59e2-c185-4bf6-a909-4c9bfe961a91",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b9381f81-03e3-46e1-9836-af8701289d85/receipts/a4be7e70-accf-400a-a40f-af870128b027/files/900c59e2-c185-4bf6-a909-4c9bfe961a91/download"
                            }
                        ]
                    },
                    {
                        "Id": "cb011c64-d165-4a9e-9b95-906a7e877962",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b9381f81-03e3-46e1-9836-af8701289d85/receipts/a4be7e70-accf-400a-a40f-af870128b027/files/cb011c64-d165-4a9e-9b95-906a7e877962/download"
                            }
                        ]
                    },
                    {
                        "Id": "ad1c6fe8-298a-4e24-b955-931cdd38a92d",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "cb011c64-d165-4a9e-9b95-906a7e877962",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b9381f81-03e3-46e1-9836-af8701289d85/receipts/a4be7e70-accf-400a-a40f-af870128b027/files/ad1c6fe8-298a-4e24-b955-931cdd38a92d/download"
                            }
                        ]
                    },
                    {
                        "Id": "a13578c3-3bd8-4024-931a-e25a2954f2b3",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "900c59e2-c185-4bf6-a909-4c9bfe961a91",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b9381f81-03e3-46e1-9836-af8701289d85/receipts/a4be7e70-accf-400a-a40f-af870128b027/files/a13578c3-3bd8-4024-931a-e25a2954f2b3/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "a12a6bca-6d4b-4478-bc25-af870128b8d2",
                "ReceiveTime": "2023-01-11T18:00:19Z",
                "StatusTime": "2023-01-11T18:00:12Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "fdc54468-b37a-4e62-8913-093d28df1321",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b9381f81-03e3-46e1-9836-af8701289d85/receipts/a12a6bca-6d4b-4478-bc25-af870128b8d2/files/fdc54468-b37a-4e62-8913-093d28df1321/download"
                            }
                        ]
                    },
                    {
                        "Id": "5666acf3-3f58-4fa6-bcad-1c6962d1cea4",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "3e2cdd02-2611-433f-9c5a-3b03acc581d5",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b9381f81-03e3-46e1-9836-af8701289d85/receipts/a12a6bca-6d4b-4478-bc25-af870128b8d2/files/5666acf3-3f58-4fa6-bcad-1c6962d1cea4/download"
                            }
                        ]
                    },
                    {
                        "Id": "59e94f2c-e040-4b94-907d-246da5557865",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "fdc54468-b37a-4e62-8913-093d28df1321",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b9381f81-03e3-46e1-9836-af8701289d85/receipts/a12a6bca-6d4b-4478-bc25-af870128b8d2/files/59e94f2c-e040-4b94-907d-246da5557865/download"
                            }
                        ]
                    },
                    {
                        "Id": "3e2cdd02-2611-433f-9c5a-3b03acc581d5",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b9381f81-03e3-46e1-9836-af8701289d85/receipts/a12a6bca-6d4b-4478-bc25-af870128b8d2/files/3e2cdd02-2611-433f-9c5a-3b03acc581d5/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "9a997b79-8b5a-41bc-bb98-af870129bfb2",
                "ReceiveTime": "2023-01-11T18:04:04Z",
                "StatusTime": "2023-01-11T18:02:22Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "ca7021d4-976f-4662-b312-3db545f915a7",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b9381f81-03e3-46e1-9836-af8701289d85/receipts/9a997b79-8b5a-41bc-bb98-af870129bfb2/files/ca7021d4-976f-4662-b312-3db545f915a7/download"
                            }
                        ]
                    },
                    {
                        "Id": "e5315150-f8d2-4890-8c63-a466e3f5fcae",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "ca7021d4-976f-4662-b312-3db545f915a7",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b9381f81-03e3-46e1-9836-af8701289d85/receipts/9a997b79-8b5a-41bc-bb98-af870129bfb2/files/e5315150-f8d2-4890-8c63-a466e3f5fcae/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "1b5f3c60-e5ff-4630-bb23-af880093ff68",
        "CorrelationId": null,
        "GroupId": "0ed8e567-5ee7-40e3-9160-8599f9c77e8d",
        "Type": "inbox",
        "Title": "№ ИН-017-56/2 от 12/01/2023 Информационные письма Банка России",
        "Text": "Информационное письмо о реализации кредитными организациями подпункта 7.1 пункта 7 Положения Банка России № 683-П",
        "CreationDate": "2023-01-12T08:58:59Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "ИН-017-56/2",
        "TotalSize": 508656,
        "Sender": null,
        "Files": [
            {
                "Id": "5c6dd6d9-09b4-4155-892a-30a8986ef6a4",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "43211373-f3b5-4085-bad2-50d73bcd4ac6",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1b5f3c60-e5ff-4630-bb23-af880093ff68/files/5c6dd6d9-09b4-4155-892a-30a8986ef6a4/download"
                    }
                ]
            },
            {
                "Id": "43211373-f3b5-4085-bad2-50d73bcd4ac6",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "612919944.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 114903,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1b5f3c60-e5ff-4630-bb23-af880093ff68/files/43211373-f3b5-4085-bad2-50d73bcd4ac6/download"
                    }
                ]
            },
            {
                "Id": "c0ce1d9f-474f-4526-895a-71cb1b1209f9",
                "Name": "ЭД_2_IN23.pdf",
                "Description": "612915659.pdf",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 105355,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1b5f3c60-e5ff-4630-bb23-af880093ff68/files/c0ce1d9f-474f-4526-895a-71cb1b1209f9/download"
                    }
                ]
            },
            {
                "Id": "625ecd5b-60d3-4ded-b3b3-789677f0826a",
                "Name": "ЭД_2_IN23.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "c0ce1d9f-474f-4526-895a-71cb1b1209f9",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1b5f3c60-e5ff-4630-bb23-af880093ff68/files/625ecd5b-60d3-4ded-b3b3-789677f0826a/download"
                    }
                ]
            },
            {
                "Id": "9491fa0a-39a2-4742-b641-a5b7d7b95b5c",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 281876,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1b5f3c60-e5ff-4630-bb23-af880093ff68/files/9491fa0a-39a2-4742-b641-a5b7d7b95b5c/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "094ab399-877b-48a6-8d11-af8800d9e7d5",
        "CorrelationId": "353e28f1-1fbb-4e62-8724-af73005acc12",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "предоставление запрошенной информации",
        "CreationDate": "2023-01-12T13:13:22Z",
        "UpdatedDate": "2023-01-12T13:23:00Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "10620",
        "TotalSize": 1451617,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "70984524-aeaa-4610-a56a-af8800d9e7ee",
                "Name": "2023-01-12-4-2.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 1370033,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/files/70984524-aeaa-4610-a56a-af8800d9e7ee/download"
                    }
                ]
            },
            {
                "Id": "e217d973-a108-47e6-874a-af8800d9e87b",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2007,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/files/e217d973-a108-47e6-874a-af8800d9e87b/download"
                    }
                ]
            },
            {
                "Id": "884378f5-5155-4d0e-9d07-af8800d9e87c",
                "Name": "2023-01-12-4-2.docx.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 61454,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/files/884378f5-5155-4d0e-9d07-af8800d9e87c/download"
                    }
                ]
            },
            {
                "Id": "ae4d1c7e-e372-4d50-bcbc-af8800da378d",
                "Name": "2023-01-12-4-2.docx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "884378f5-5155-4d0e-9d07-af8800d9e87c",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/files/ae4d1c7e-e372-4d50-bcbc-af8800da378d/download"
                    }
                ]
            },
            {
                "Id": "91592d25-a257-4e5f-aab1-af8800da823f",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "e217d973-a108-47e6-874a-af8800d9e87b",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/files/91592d25-a257-4e5f-aab1-af8800da823f/download"
                    }
                ]
            },
            {
                "Id": "d723ff1b-29c1-444f-9f26-af8800dace96",
                "Name": "2023-01-12-4-2.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "70984524-aeaa-4610-a56a-af8800d9e7ee",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/files/d723ff1b-29c1-444f-9f26-af8800dace96/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "5146daa9-6628-413b-b1ee-af8800dad050",
                "ReceiveTime": "2023-01-12T13:16:40Z",
                "StatusTime": "2023-01-12T13:16:40Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "c9312f5d-49d6-4845-a186-af8800dae541",
                "ReceiveTime": "2023-01-12T13:16:58Z",
                "StatusTime": "2023-01-12T13:16:54Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "8754141d-24c6-4396-81d5-7085530cee09",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "e4a6deea-b37d-4105-ab0e-7288614a1ab1",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/receipts/c9312f5d-49d6-4845-a186-af8800dae541/files/8754141d-24c6-4396-81d5-7085530cee09/download"
                            }
                        ]
                    },
                    {
                        "Id": "e4a6deea-b37d-4105-ab0e-7288614a1ab1",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/receipts/c9312f5d-49d6-4845-a186-af8800dae541/files/e4a6deea-b37d-4105-ab0e-7288614a1ab1/download"
                            }
                        ]
                    },
                    {
                        "Id": "c6b891f3-9243-410b-9c7a-7348c878d4d3",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/receipts/c9312f5d-49d6-4845-a186-af8800dae541/files/c6b891f3-9243-410b-9c7a-7348c878d4d3/download"
                            }
                        ]
                    },
                    {
                        "Id": "cc22a03f-d30e-48f7-bafe-b84c6f624246",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "c6b891f3-9243-410b-9c7a-7348c878d4d3",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/receipts/c9312f5d-49d6-4845-a186-af8800dae541/files/cc22a03f-d30e-48f7-bafe-b84c6f624246/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "2378f5be-0ab1-4293-98f3-af8800daef47",
                "ReceiveTime": "2023-01-12T13:17:07Z",
                "StatusTime": "2023-01-12T13:16:58Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "bfdc0b0b-c74b-41db-aaa0-5c60d405931e",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/receipts/2378f5be-0ab1-4293-98f3-af8800daef47/files/bfdc0b0b-c74b-41db-aaa0-5c60d405931e/download"
                            }
                        ]
                    },
                    {
                        "Id": "ac392e80-b11e-4393-bf7b-9b433d036fc2",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "bfdc0b0b-c74b-41db-aaa0-5c60d405931e",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/receipts/2378f5be-0ab1-4293-98f3-af8800daef47/files/ac392e80-b11e-4393-bf7b-9b433d036fc2/download"
                            }
                        ]
                    },
                    {
                        "Id": "95681106-fb8a-4d00-97e5-e1f396b89fbf",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/receipts/2378f5be-0ab1-4293-98f3-af8800daef47/files/95681106-fb8a-4d00-97e5-e1f396b89fbf/download"
                            }
                        ]
                    },
                    {
                        "Id": "50cf19c5-c8e6-49db-8719-eaecf3fe069a",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "95681106-fb8a-4d00-97e5-e1f396b89fbf",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/receipts/2378f5be-0ab1-4293-98f3-af8800daef47/files/50cf19c5-c8e6-49db-8719-eaecf3fe069a/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "21e1179f-2daf-41fa-8ad6-af8800dc8d16",
                "ReceiveTime": "2023-01-12T13:23:00Z",
                "StatusTime": "2023-01-12T13:21:03Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "dd0b1688-82b6-423d-bf11-209d40b33f85",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "05ad0f86-e174-413e-b4d8-ef81bc06a3f2",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/receipts/21e1179f-2daf-41fa-8ad6-af8800dc8d16/files/dd0b1688-82b6-423d-bf11-209d40b33f85/download"
                            }
                        ]
                    },
                    {
                        "Id": "af0c8c26-1c24-4a7f-a38c-88b2d0d1588c",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "6a1f7af0-fcbf-4803-a3d2-f8c7cca8e83f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/receipts/21e1179f-2daf-41fa-8ad6-af8800dc8d16/files/af0c8c26-1c24-4a7f-a38c-88b2d0d1588c/download"
                            }
                        ]
                    },
                    {
                        "Id": "05ad0f86-e174-413e-b4d8-ef81bc06a3f2",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 742,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/receipts/21e1179f-2daf-41fa-8ad6-af8800dc8d16/files/05ad0f86-e174-413e-b4d8-ef81bc06a3f2/download"
                            }
                        ]
                    },
                    {
                        "Id": "6a1f7af0-fcbf-4803-a3d2-f8c7cca8e83f",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/094ab399-877b-48a6-8d11-af8800d9e7d5/receipts/21e1179f-2daf-41fa-8ad6-af8800dc8d16/files/6a1f7af0-fcbf-4803-a3d2-f8c7cca8e83f/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "e74822d4-e5ef-4959-aabb-af880113a96d",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-01-12T16:43:47Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 2214519,
        "Sender": null,
        "Files": [
            {
                "Id": "82cbaa0b-8feb-422c-a69a-e1d16a104ee9",
                "Name": "KYC_20230112.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "b4351f91-e968-44bf-b314-fa876a07edde",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e74822d4-e5ef-4959-aabb-af880113a96d/files/82cbaa0b-8feb-422c-a69a-e1d16a104ee9/download"
                    }
                ]
            },
            {
                "Id": "b4351f91-e968-44bf-b314-fa876a07edde",
                "Name": "KYC_20230112.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2211258,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e74822d4-e5ef-4959-aabb-af880113a96d/files/b4351f91-e968-44bf-b314-fa876a07edde/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "8868d490-885e-4233-b530-af8801289be2",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-12T17:59:55Z",
        "UpdatedDate": "2023-01-12T18:04:02Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00568891",
        "TotalSize": 13369,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "56395ce1-6984-462d-8040-af8801289bde",
                "Name": "KYCCL_7831001422_3194_20230112_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9497,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/8868d490-885e-4233-b530-af8801289be2/files/56395ce1-6984-462d-8040-af8801289bde/download"
                    }
                ]
            },
            {
                "Id": "69963b97-cb1d-43e0-8d3e-af8801289be0",
                "Name": "KYCCL_7831001422_3194_20230112_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "56395ce1-6984-462d-8040-af8801289bde",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/8868d490-885e-4233-b530-af8801289be2/files/69963b97-cb1d-43e0-8d3e-af8801289be0/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "1f5abcf8-60df-44eb-bb5b-af8801289e52",
                "ReceiveTime": "2023-01-12T17:59:57Z",
                "StatusTime": "2023-01-12T17:59:57Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "ce2741a1-020b-4cc6-9ed3-af880128af90",
                "ReceiveTime": "2023-01-12T18:00:11Z",
                "StatusTime": "2023-01-12T18:00:08Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "8ba7e6d9-f43f-4b14-954f-740d1f164581",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "60363a97-ec24-4b51-b2a8-853bd79e789f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8868d490-885e-4233-b530-af8801289be2/receipts/ce2741a1-020b-4cc6-9ed3-af880128af90/files/8ba7e6d9-f43f-4b14-954f-740d1f164581/download"
                            }
                        ]
                    },
                    {
                        "Id": "68820745-7b7e-4d3b-80e3-780c5f953a20",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8868d490-885e-4233-b530-af8801289be2/receipts/ce2741a1-020b-4cc6-9ed3-af880128af90/files/68820745-7b7e-4d3b-80e3-780c5f953a20/download"
                            }
                        ]
                    },
                    {
                        "Id": "60363a97-ec24-4b51-b2a8-853bd79e789f",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8868d490-885e-4233-b530-af8801289be2/receipts/ce2741a1-020b-4cc6-9ed3-af880128af90/files/60363a97-ec24-4b51-b2a8-853bd79e789f/download"
                            }
                        ]
                    },
                    {
                        "Id": "86100d8e-f2e1-4cfd-ae31-ad78793f0874",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "68820745-7b7e-4d3b-80e3-780c5f953a20",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8868d490-885e-4233-b530-af8801289be2/receipts/ce2741a1-020b-4cc6-9ed3-af880128af90/files/86100d8e-f2e1-4cfd-ae31-ad78793f0874/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "e77cfb51-dd18-4b74-b5ed-af880128b662",
                "ReceiveTime": "2023-01-12T18:00:17Z",
                "StatusTime": "2023-01-12T18:00:11Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "94279827-dbf3-46bb-9709-5b9966ba17e9",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8868d490-885e-4233-b530-af8801289be2/receipts/e77cfb51-dd18-4b74-b5ed-af880128b662/files/94279827-dbf3-46bb-9709-5b9966ba17e9/download"
                            }
                        ]
                    },
                    {
                        "Id": "6b0fd511-8e77-4ad2-8d1a-764fa2a652a9",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "e03a3f36-2d2c-4106-9489-9d10c5242bdb",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8868d490-885e-4233-b530-af8801289be2/receipts/e77cfb51-dd18-4b74-b5ed-af880128b662/files/6b0fd511-8e77-4ad2-8d1a-764fa2a652a9/download"
                            }
                        ]
                    },
                    {
                        "Id": "e03a3f36-2d2c-4106-9489-9d10c5242bdb",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8868d490-885e-4233-b530-af8801289be2/receipts/e77cfb51-dd18-4b74-b5ed-af880128b662/files/e03a3f36-2d2c-4106-9489-9d10c5242bdb/download"
                            }
                        ]
                    },
                    {
                        "Id": "9ee2ab4c-1daa-4291-940d-b853eeee9c03",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "94279827-dbf3-46bb-9709-5b9966ba17e9",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8868d490-885e-4233-b530-af8801289be2/receipts/e77cfb51-dd18-4b74-b5ed-af880128b662/files/9ee2ab4c-1daa-4291-940d-b853eeee9c03/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "5beb4a9c-eb31-484e-98a1-af880129bd9f",
                "ReceiveTime": "2023-01-12T18:04:02Z",
                "StatusTime": "2023-01-12T18:02:37Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "72425f36-e491-45eb-89f5-77fbc078480f",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "2045c9cb-d504-4adb-ab35-b3659ad95886",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8868d490-885e-4233-b530-af8801289be2/receipts/5beb4a9c-eb31-484e-98a1-af880129bd9f/files/72425f36-e491-45eb-89f5-77fbc078480f/download"
                            }
                        ]
                    },
                    {
                        "Id": "2045c9cb-d504-4adb-ab35-b3659ad95886",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8868d490-885e-4233-b530-af8801289be2/receipts/5beb4a9c-eb31-484e-98a1-af880129bd9f/files/2045c9cb-d504-4adb-ab35-b3659ad95886/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "26e7b0e8-76ce-4804-9982-af89007b5d19",
        "CorrelationId": null,
        "GroupId": "9f469b5b-7195-4b63-b101-262de5c77979",
        "Type": "inbox",
        "Title": "№ ИН-03-23/3 от 13/01/2023 Информационные письма Банка России",
        "Text": "Информационное письмо о применении коэффициента риска 50% в целях расчета нормативов концентрации",
        "CreationDate": "2023-01-13T07:29:15Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "ИН-03-23/3",
        "TotalSize": 463721,
        "Sender": null,
        "Files": [
            {
                "Id": "49ac4c1d-ab93-4c13-9561-135ff7ecc8a1",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "3aecf1b6-4128-457a-a314-756242dd746b",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/26e7b0e8-76ce-4804-9982-af89007b5d19/files/49ac4c1d-ab93-4c13-9561-135ff7ecc8a1/download"
                    }
                ]
            },
            {
                "Id": "6a9aa589-fd1c-4766-b493-28c2d918ba42",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 250110,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/26e7b0e8-76ce-4804-9982-af89007b5d19/files/6a9aa589-fd1c-4766-b493-28c2d918ba42/download"
                    }
                ]
            },
            {
                "Id": "3aecf1b6-4128-457a-a314-756242dd746b",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "613217713.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 108087,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/26e7b0e8-76ce-4804-9982-af89007b5d19/files/3aecf1b6-4128-457a-a314-756242dd746b/download"
                    }
                ]
            },
            {
                "Id": "3a2a5c76-0fc9-4c4d-ae2a-a21d11abbd31",
                "Name": "ЭД_3_IN23.pdf",
                "Description": "613213135.pdf",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 99002,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/26e7b0e8-76ce-4804-9982-af89007b5d19/files/3a2a5c76-0fc9-4c4d-ae2a-a21d11abbd31/download"
                    }
                ]
            },
            {
                "Id": "2e732ac1-09fa-44f6-b62f-d983149ff56e",
                "Name": "ЭД_3_IN23.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "3a2a5c76-0fc9-4c4d-ae2a-a21d11abbd31",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/26e7b0e8-76ce-4804-9982-af89007b5d19/files/2e732ac1-09fa-44f6-b62f-d983149ff56e/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "6e6950f3-744c-434f-b6db-af8a0128a1db",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-14T18:00:00Z",
        "UpdatedDate": "2023-01-14T18:03:40Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00569765",
        "TotalSize": 13369,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "091d38fc-95bc-44c3-a977-af8a0128a1d7",
                "Name": "KYCCL_7831001422_3194_20230114_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9497,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6e6950f3-744c-434f-b6db-af8a0128a1db/files/091d38fc-95bc-44c3-a977-af8a0128a1d7/download"
                    }
                ]
            },
            {
                "Id": "76e64688-115d-4108-ab74-af8a0128a1d9",
                "Name": "KYCCL_7831001422_3194_20230114_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "091d38fc-95bc-44c3-a977-af8a0128a1d7",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6e6950f3-744c-434f-b6db-af8a0128a1db/files/76e64688-115d-4108-ab74-af8a0128a1d9/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "3b0bb1a9-919e-4ff7-b230-af8a0128a313",
                "ReceiveTime": "2023-01-14T18:00:01Z",
                "StatusTime": "2023-01-14T18:00:01Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "b4759365-3462-465c-87eb-af8a0128b27d",
                "ReceiveTime": "2023-01-14T18:00:14Z",
                "StatusTime": "2023-01-14T18:00:10Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "bba3fc52-f5e6-4f49-ac38-004515235e79",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "e7254d5d-0961-431a-a15e-252e60998aa3",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6e6950f3-744c-434f-b6db-af8a0128a1db/receipts/b4759365-3462-465c-87eb-af8a0128b27d/files/bba3fc52-f5e6-4f49-ac38-004515235e79/download"
                            }
                        ]
                    },
                    {
                        "Id": "addda9a9-1a8e-42be-a1a8-1a3b4802f185",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6e6950f3-744c-434f-b6db-af8a0128a1db/receipts/b4759365-3462-465c-87eb-af8a0128b27d/files/addda9a9-1a8e-42be-a1a8-1a3b4802f185/download"
                            }
                        ]
                    },
                    {
                        "Id": "e7254d5d-0961-431a-a15e-252e60998aa3",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6e6950f3-744c-434f-b6db-af8a0128a1db/receipts/b4759365-3462-465c-87eb-af8a0128b27d/files/e7254d5d-0961-431a-a15e-252e60998aa3/download"
                            }
                        ]
                    },
                    {
                        "Id": "5b61b41a-2565-4eac-86c8-6db90919e31f",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "addda9a9-1a8e-42be-a1a8-1a3b4802f185",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6e6950f3-744c-434f-b6db-af8a0128a1db/receipts/b4759365-3462-465c-87eb-af8a0128b27d/files/5b61b41a-2565-4eac-86c8-6db90919e31f/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "d50decdd-d64b-45f7-bf1e-af8a0128bafb",
                "ReceiveTime": "2023-01-14T18:00:21Z",
                "StatusTime": "2023-01-14T18:00:14Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "33b2243b-2d2c-4f7e-b8c4-8727ba1680ec",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "a64b1f51-6cfb-405c-a478-943721fb9002",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6e6950f3-744c-434f-b6db-af8a0128a1db/receipts/d50decdd-d64b-45f7-bf1e-af8a0128bafb/files/33b2243b-2d2c-4f7e-b8c4-8727ba1680ec/download"
                            }
                        ]
                    },
                    {
                        "Id": "5371fba2-87f9-4093-9f0a-942c04a128af",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "0d8555de-92b1-49b1-9d35-a716c01a0b55",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6e6950f3-744c-434f-b6db-af8a0128a1db/receipts/d50decdd-d64b-45f7-bf1e-af8a0128bafb/files/5371fba2-87f9-4093-9f0a-942c04a128af/download"
                            }
                        ]
                    },
                    {
                        "Id": "a64b1f51-6cfb-405c-a478-943721fb9002",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6e6950f3-744c-434f-b6db-af8a0128a1db/receipts/d50decdd-d64b-45f7-bf1e-af8a0128bafb/files/a64b1f51-6cfb-405c-a478-943721fb9002/download"
                            }
                        ]
                    },
                    {
                        "Id": "0d8555de-92b1-49b1-9d35-a716c01a0b55",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6e6950f3-744c-434f-b6db-af8a0128a1db/receipts/d50decdd-d64b-45f7-bf1e-af8a0128bafb/files/0d8555de-92b1-49b1-9d35-a716c01a0b55/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "af56e3cc-6541-4679-9959-af8a0129a43a",
                "ReceiveTime": "2023-01-14T18:03:40Z",
                "StatusTime": "2023-01-14T18:02:58Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "20cfb77d-8a36-4876-a9cb-56285d6a7bea",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f214ff79-7c19-4066-a23d-a2620c3d069b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6e6950f3-744c-434f-b6db-af8a0128a1db/receipts/af56e3cc-6541-4679-9959-af8a0129a43a/files/20cfb77d-8a36-4876-a9cb-56285d6a7bea/download"
                            }
                        ]
                    },
                    {
                        "Id": "f214ff79-7c19-4066-a23d-a2620c3d069b",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6e6950f3-744c-434f-b6db-af8a0128a1db/receipts/af56e3cc-6541-4679-9959-af8a0129a43a/files/f214ff79-7c19-4066-a23d-a2620c3d069b/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "3a1c5d06-7875-4054-8b17-af8b0128a05f",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-15T17:59:59Z",
        "UpdatedDate": "2023-01-15T18:03:24Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00569832",
        "TotalSize": 13369,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "10e24ef9-4173-497b-b3c8-af8b0128a053",
                "Name": "KYCCL_7831001422_3194_20230115_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9497,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/3a1c5d06-7875-4054-8b17-af8b0128a05f/files/10e24ef9-4173-497b-b3c8-af8b0128a053/download"
                    }
                ]
            },
            {
                "Id": "9f73225c-f188-4da9-8839-af8b0128a058",
                "Name": "KYCCL_7831001422_3194_20230115_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "10e24ef9-4173-497b-b3c8-af8b0128a053",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/3a1c5d06-7875-4054-8b17-af8b0128a05f/files/9f73225c-f188-4da9-8839-af8b0128a058/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "6db6a589-6457-4028-b9d8-af8b0128a1bb",
                "ReceiveTime": "2023-01-15T18:00:00Z",
                "StatusTime": "2023-01-15T18:00:00Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "e7ea42be-f405-4b11-af61-af8b0128b236",
                "ReceiveTime": "2023-01-15T18:00:14Z",
                "StatusTime": "2023-01-15T18:00:10Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "d9104430-04b2-4f2f-a6ac-01359cf663d7",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "0e264d88-770b-4431-8169-833e9bf6ef6b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/3a1c5d06-7875-4054-8b17-af8b0128a05f/receipts/e7ea42be-f405-4b11-af61-af8b0128b236/files/d9104430-04b2-4f2f-a6ac-01359cf663d7/download"
                            }
                        ]
                    },
                    {
                        "Id": "f14c45df-1d41-4293-a612-26d18b47512b",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/3a1c5d06-7875-4054-8b17-af8b0128a05f/receipts/e7ea42be-f405-4b11-af61-af8b0128b236/files/f14c45df-1d41-4293-a612-26d18b47512b/download"
                            }
                        ]
                    },
                    {
                        "Id": "0e264d88-770b-4431-8169-833e9bf6ef6b",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/3a1c5d06-7875-4054-8b17-af8b0128a05f/receipts/e7ea42be-f405-4b11-af61-af8b0128b236/files/0e264d88-770b-4431-8169-833e9bf6ef6b/download"
                            }
                        ]
                    },
                    {
                        "Id": "a52a71e2-f341-4511-a89e-bd2f5d74ff94",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f14c45df-1d41-4293-a612-26d18b47512b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/3a1c5d06-7875-4054-8b17-af8b0128a05f/receipts/e7ea42be-f405-4b11-af61-af8b0128b236/files/a52a71e2-f341-4511-a89e-bd2f5d74ff94/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "4cfea1cc-32bf-46b2-a38f-af8b0128bb13",
                "ReceiveTime": "2023-01-15T18:00:21Z",
                "StatusTime": "2023-01-15T18:00:14Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "ad59aa2b-15c8-4d82-b6a5-2023c22b08b9",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "323cb823-b5b8-4ee2-97b5-268e28711170",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/3a1c5d06-7875-4054-8b17-af8b0128a05f/receipts/4cfea1cc-32bf-46b2-a38f-af8b0128bb13/files/ad59aa2b-15c8-4d82-b6a5-2023c22b08b9/download"
                            }
                        ]
                    },
                    {
                        "Id": "323cb823-b5b8-4ee2-97b5-268e28711170",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/3a1c5d06-7875-4054-8b17-af8b0128a05f/receipts/4cfea1cc-32bf-46b2-a38f-af8b0128bb13/files/323cb823-b5b8-4ee2-97b5-268e28711170/download"
                            }
                        ]
                    },
                    {
                        "Id": "cc3c5c8f-cacc-45a9-a6b4-a763f5a14be5",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "6746882f-5429-4104-8494-b0d25fc8d23d",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/3a1c5d06-7875-4054-8b17-af8b0128a05f/receipts/4cfea1cc-32bf-46b2-a38f-af8b0128bb13/files/cc3c5c8f-cacc-45a9-a6b4-a763f5a14be5/download"
                            }
                        ]
                    },
                    {
                        "Id": "6746882f-5429-4104-8494-b0d25fc8d23d",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/3a1c5d06-7875-4054-8b17-af8b0128a05f/receipts/4cfea1cc-32bf-46b2-a38f-af8b0128bb13/files/6746882f-5429-4104-8494-b0d25fc8d23d/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "ebc933c0-a9cf-40ee-9f6b-af8b01299187",
                "ReceiveTime": "2023-01-15T18:03:24Z",
                "StatusTime": "2023-01-15T18:02:27Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "0ab3d341-d728-4517-a82a-076af795bb81",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "6bfbced0-a4b6-45c0-af8d-82e40c70a88e",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/3a1c5d06-7875-4054-8b17-af8b0128a05f/receipts/ebc933c0-a9cf-40ee-9f6b-af8b01299187/files/0ab3d341-d728-4517-a82a-076af795bb81/download"
                            }
                        ]
                    },
                    {
                        "Id": "6bfbced0-a4b6-45c0-af8d-82e40c70a88e",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/3a1c5d06-7875-4054-8b17-af8b0128a05f/receipts/ebc933c0-a9cf-40ee-9f6b-af8b01299187/files/6bfbced0-a4b6-45c0-af8d-82e40c70a88e/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "c7a4085e-bb7a-4444-995c-af8c006a93c5",
        "CorrelationId": null,
        "GroupId": "c1e92048-39bd-4a2f-9543-4b244b6ca7c7",
        "Type": "inbox",
        "Title": "№ 36-11-2-1/352 от 13/01/2023 (36) Письма Службы текущего банковского надзора",
        "Text": "О направлении сведений",
        "CreationDate": "2023-01-16T06:28:02Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "36-11-2-1/352",
        "TotalSize": 260926,
        "Sender": null,
        "Files": [
            {
                "Id": "4cdc20c4-a74d-4629-a120-047b3fb58dae",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "7fe79147-8375-4c6c-b3c4-6d2b806645da",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/c7a4085e-bb7a-4444-995c-af8c006a93c5/files/4cdc20c4-a74d-4629-a120-047b3fb58dae/download"
                    }
                ]
            },
            {
                "Id": "d11b9485-95d5-4043-94ec-3aafaa1afa1e",
                "Name": "ЭД_Письмо.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "8e1b88ed-cb98-46b4-b0c0-98479bcff1e4",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/c7a4085e-bb7a-4444-995c-af8c006a93c5/files/d11b9485-95d5-4043-94ec-3aafaa1afa1e/download"
                    }
                ]
            },
            {
                "Id": "7fe79147-8375-4c6c-b3c4-6d2b806645da",
                "Name": "ВизуализацияЭД.PDF.enc",
                "Description": "613478732.PDF.enc",
                "Encrypted": true,
                "SignedFile": null,
                "Size": 126229,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/c7a4085e-bb7a-4444-995c-af8c006a93c5/files/7fe79147-8375-4c6c-b3c4-6d2b806645da/download"
                    }
                ]
            },
            {
                "Id": "8e1b88ed-cb98-46b4-b0c0-98479bcff1e4",
                "Name": "ЭД_Письмо.pdf.enc",
                "Description": "613478650.pdf.enc",
                "Encrypted": true,
                "SignedFile": null,
                "Size": 116457,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/c7a4085e-bb7a-4444-995c-af8c006a93c5/files/8e1b88ed-cb98-46b4-b0c0-98479bcff1e4/download"
                    }
                ]
            },
            {
                "Id": "971b040e-49f2-42ef-9faf-ba6f5df48e59",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 11718,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/c7a4085e-bb7a-4444-995c-af8c006a93c5/files/971b040e-49f2-42ef-9faf-ba6f5df48e59/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "b0754f17-51c8-4c72-a2ed-af8c00a54760",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Обращение (запрос)  в Банк России",
        "Text": "предоставление запрошенной информации",
        "CreationDate": "2023-01-16T10:01:45Z",
        "UpdatedDate": "2023-01-16T10:15:27Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "15695",
        "TotalSize": 118079,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "c3fca4f7-7c6e-44ab-a2bf-af8c00a547aa",
                "Name": "Приложение №2_по предписанию № 018-34-12822(01012023).xlsx.docx",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 16333,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/files/c3fca4f7-7c6e-44ab-a2bf-af8c00a547aa/download"
                    }
                ]
            },
            {
                "Id": "b4eee8f5-ca2d-476f-9c4c-af8c00a547be",
                "Name": "2023-01-16-6-4.docx",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 60168,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/files/b4eee8f5-ca2d-476f-9c4c-af8c00a547be/download"
                    }
                ]
            },
            {
                "Id": "8de19102-f3e3-4b66-b087-af8c00a547c2",
                "Name": "form.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 1140,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/files/8de19102-f3e3-4b66-b087-af8c00a547c2/download"
                    }
                ]
            },
            {
                "Id": "d0e59a4d-4779-4646-9bc9-af8c00a547ca",
                "Name": "Приложение №1_по предписанию № 018-34-12822(01012023).xlsx.docx",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 16274,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/files/d0e59a4d-4779-4646-9bc9-af8c00a547ca/download"
                    }
                ]
            },
            {
                "Id": "019b3fa0-52ab-4c6b-bca0-af8c00a597b6",
                "Name": "2023-01-16-6-4.docx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "b4eee8f5-ca2d-476f-9c4c-af8c00a547be",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/files/019b3fa0-52ab-4c6b-bca0-af8c00a597b6/download"
                    }
                ]
            },
            {
                "Id": "e494546f-601c-49f4-b51f-af8c00a5e337",
                "Name": "Приложение №2_по предписанию № 018-34-12822(01012023).xlsx.docx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "c3fca4f7-7c6e-44ab-a2bf-af8c00a547aa",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/files/e494546f-601c-49f4-b51f-af8c00a5e337/download"
                    }
                ]
            },
            {
                "Id": "0406381d-02ad-4be6-9e31-af8c00a63014",
                "Name": "Приложение №1_по предписанию № 018-34-12822(01012023).xlsx.docx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "d0e59a4d-4779-4646-9bc9-af8c00a547ca",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/files/0406381d-02ad-4be6-9e31-af8c00a63014/download"
                    }
                ]
            },
            {
                "Id": "59241577-e1e9-4df5-b621-af8c00a67a82",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "8de19102-f3e3-4b66-b087-af8c00a547c2",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/files/59241577-e1e9-4df5-b621-af8c00a67a82/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "21373767-ced3-4469-be18-af8c00a682d0",
                "ReceiveTime": "2023-01-16T10:06:14Z",
                "StatusTime": "2023-01-16T10:06:14Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "7a8a6334-0af5-4a28-8c71-af8c00a695a1",
                "ReceiveTime": "2023-01-16T10:06:30Z",
                "StatusTime": "2023-01-16T10:06:26Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "6fd6daa8-2363-425c-97cc-0d51b0f62186",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/receipts/7a8a6334-0af5-4a28-8c71-af8c00a695a1/files/6fd6daa8-2363-425c-97cc-0d51b0f62186/download"
                            }
                        ]
                    },
                    {
                        "Id": "a3927ec5-5a2e-4738-83fe-61dd5986c3f4",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "6fd6daa8-2363-425c-97cc-0d51b0f62186",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/receipts/7a8a6334-0af5-4a28-8c71-af8c00a695a1/files/a3927ec5-5a2e-4738-83fe-61dd5986c3f4/download"
                            }
                        ]
                    },
                    {
                        "Id": "fb55ead1-7b75-41d8-95de-d778832c2ebd",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/receipts/7a8a6334-0af5-4a28-8c71-af8c00a695a1/files/fb55ead1-7b75-41d8-95de-d778832c2ebd/download"
                            }
                        ]
                    },
                    {
                        "Id": "9264018e-271b-4bb0-8375-e4d1f09ccfda",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "fb55ead1-7b75-41d8-95de-d778832c2ebd",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/receipts/7a8a6334-0af5-4a28-8c71-af8c00a695a1/files/9264018e-271b-4bb0-8375-e4d1f09ccfda/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "198a2f40-542f-4ba5-b860-af8c00a6a055",
                "ReceiveTime": "2023-01-16T10:06:40Z",
                "StatusTime": "2023-01-16T10:06:30Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "9a2eba0b-9f38-4e1d-8644-0405f5dd51b2",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "cf171971-564c-4194-a342-20d151f7e609",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/receipts/198a2f40-542f-4ba5-b860-af8c00a6a055/files/9a2eba0b-9f38-4e1d-8644-0405f5dd51b2/download"
                            }
                        ]
                    },
                    {
                        "Id": "cf171971-564c-4194-a342-20d151f7e609",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/receipts/198a2f40-542f-4ba5-b860-af8c00a6a055/files/cf171971-564c-4194-a342-20d151f7e609/download"
                            }
                        ]
                    },
                    {
                        "Id": "bbb3584d-ee0c-4d4b-a15a-36e51d922d7f",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/receipts/198a2f40-542f-4ba5-b860-af8c00a6a055/files/bbb3584d-ee0c-4d4b-a15a-36e51d922d7f/download"
                            }
                        ]
                    },
                    {
                        "Id": "63a73f84-b421-43d3-a7a9-fb5ec45ccbef",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "bbb3584d-ee0c-4d4b-a15a-36e51d922d7f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/receipts/198a2f40-542f-4ba5-b860-af8c00a6a055/files/63a73f84-b421-43d3-a7a9-fb5ec45ccbef/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "8763e2d2-fe08-42cf-9884-af8c00a909ed",
                "ReceiveTime": "2023-01-16T10:15:27Z",
                "StatusTime": "2023-01-16T10:15:08Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "86eabaff-0fa3-44b6-9a5b-0f6ce74317cb",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 742,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/receipts/8763e2d2-fe08-42cf-9884-af8c00a909ed/files/86eabaff-0fa3-44b6-9a5b-0f6ce74317cb/download"
                            }
                        ]
                    },
                    {
                        "Id": "af093e6e-ed9b-4a3f-b73a-aa43a72752e2",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "a0c1f782-6160-4518-bc91-c8d44167bec4",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/receipts/8763e2d2-fe08-42cf-9884-af8c00a909ed/files/af093e6e-ed9b-4a3f-b73a-aa43a72752e2/download"
                            }
                        ]
                    },
                    {
                        "Id": "a0c1f782-6160-4518-bc91-c8d44167bec4",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/receipts/8763e2d2-fe08-42cf-9884-af8c00a909ed/files/a0c1f782-6160-4518-bc91-c8d44167bec4/download"
                            }
                        ]
                    },
                    {
                        "Id": "e3098826-c740-4e7f-a8fb-d1ac094199de",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "86eabaff-0fa3-44b6-9a5b-0f6ce74317cb",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/b0754f17-51c8-4c72-a2ed-af8c00a54760/receipts/8763e2d2-fe08-42cf-9884-af8c00a909ed/files/e3098826-c740-4e7f-a8fb-d1ac094199de/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "a3ff37c6-aca3-43e5-8c0f-af8c00a7a573",
        "CorrelationId": "4f534ebc-3123-43ba-a80f-af86008e4c2a",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "предоставление запрошенной информации",
        "CreationDate": "2023-01-16T10:10:22Z",
        "UpdatedDate": "2023-01-16T10:16:40Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2-3419",
        "TotalSize": 552070,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "39ea5526-5e47-48c4-ada8-af8c00a7a592",
                "Name": "form.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 884,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/files/39ea5526-5e47-48c4-ada8-af8c00a7a592/download"
                    }
                ]
            },
            {
                "Id": "cf812dce-c901-4a6e-b518-af8c00a7a5b9",
                "Name": "2023-01-16-6-2 Акт уничтожения ключей Сигнатуры 16-O.pdf",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 539104,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/files/cf812dce-c901-4a6e-b518-af8c00a7a5b9/download"
                    }
                ]
            },
            {
                "Id": "807a89c7-0887-4675-ae46-af8c00a7f472",
                "Name": "2023-01-16-6-2 Акт уничтожения ключей Сигнатуры 16-O.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "cf812dce-c901-4a6e-b518-af8c00a7a5b9",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/files/807a89c7-0887-4675-ae46-af8c00a7f472/download"
                    }
                ]
            },
            {
                "Id": "37a41911-7ff6-42ef-95bd-af8c00a83f2e",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "39ea5526-5e47-48c4-ada8-af8c00a7a592",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/files/37a41911-7ff6-42ef-95bd-af8c00a83f2e/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "5ac47044-3643-4994-8134-af8c00a84015",
                "ReceiveTime": "2023-01-16T10:12:34Z",
                "StatusTime": "2023-01-16T10:12:34Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "5b6123d9-de02-4d92-815f-af8c00a8513c",
                "ReceiveTime": "2023-01-16T10:12:49Z",
                "StatusTime": "2023-01-16T10:12:45Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "28136a75-f90b-41ee-8b15-405611901a17",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "92629e22-34c4-42a1-bf28-4f98cd8d6a72",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/receipts/5b6123d9-de02-4d92-815f-af8c00a8513c/files/28136a75-f90b-41ee-8b15-405611901a17/download"
                            }
                        ]
                    },
                    {
                        "Id": "92629e22-34c4-42a1-bf28-4f98cd8d6a72",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/receipts/5b6123d9-de02-4d92-815f-af8c00a8513c/files/92629e22-34c4-42a1-bf28-4f98cd8d6a72/download"
                            }
                        ]
                    },
                    {
                        "Id": "8a5ae8d5-eedd-4028-b162-6db6bbf0241c",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "222f5636-70bc-4fb8-819e-933f9a67a367",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/receipts/5b6123d9-de02-4d92-815f-af8c00a8513c/files/8a5ae8d5-eedd-4028-b162-6db6bbf0241c/download"
                            }
                        ]
                    },
                    {
                        "Id": "222f5636-70bc-4fb8-819e-933f9a67a367",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/receipts/5b6123d9-de02-4d92-815f-af8c00a8513c/files/222f5636-70bc-4fb8-819e-933f9a67a367/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "da05b249-b978-45c9-a1b1-af8c00a85897",
                "ReceiveTime": "2023-01-16T10:12:55Z",
                "StatusTime": "2023-01-16T10:12:49Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "736ac5dc-0c5d-4ef9-b5c2-1d1271ebfd24",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "ff81d72c-8bd8-423b-89d0-da9fec3b761c",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/receipts/da05b249-b978-45c9-a1b1-af8c00a85897/files/736ac5dc-0c5d-4ef9-b5c2-1d1271ebfd24/download"
                            }
                        ]
                    },
                    {
                        "Id": "699ccc1e-e640-4ef3-9e90-4658c476b868",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "07f796d3-c3dc-4c8f-8cc8-e1ac99fcee32",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/receipts/da05b249-b978-45c9-a1b1-af8c00a85897/files/699ccc1e-e640-4ef3-9e90-4658c476b868/download"
                            }
                        ]
                    },
                    {
                        "Id": "ff81d72c-8bd8-423b-89d0-da9fec3b761c",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/receipts/da05b249-b978-45c9-a1b1-af8c00a85897/files/ff81d72c-8bd8-423b-89d0-da9fec3b761c/download"
                            }
                        ]
                    },
                    {
                        "Id": "07f796d3-c3dc-4c8f-8cc8-e1ac99fcee32",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/receipts/da05b249-b978-45c9-a1b1-af8c00a85897/files/07f796d3-c3dc-4c8f-8cc8-e1ac99fcee32/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "dbda92ec-832a-4228-92cf-af8c00a95f9f",
                "ReceiveTime": "2023-01-16T10:16:40Z",
                "StatusTime": "2023-01-16T10:16:31Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "47324f5d-9102-456a-bec3-0fc6d58f3bcb",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 355,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/receipts/dbda92ec-832a-4228-92cf-af8c00a95f9f/files/47324f5d-9102-456a-bec3-0fc6d58f3bcb/download"
                            }
                        ]
                    },
                    {
                        "Id": "2c6e7002-7c51-4d77-ab16-4a7fab9c6e69",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "47324f5d-9102-456a-bec3-0fc6d58f3bcb",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/receipts/dbda92ec-832a-4228-92cf-af8c00a95f9f/files/2c6e7002-7c51-4d77-ab16-4a7fab9c6e69/download"
                            }
                        ]
                    },
                    {
                        "Id": "d7f638de-a28a-4bd4-9b7f-6441a31d0063",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 780,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/receipts/dbda92ec-832a-4228-92cf-af8c00a95f9f/files/d7f638de-a28a-4bd4-9b7f-6441a31d0063/download"
                            }
                        ]
                    },
                    {
                        "Id": "5ed5aa3c-3cf0-4389-82f8-64b95a67020a",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "d7f638de-a28a-4bd4-9b7f-6441a31d0063",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/a3ff37c6-aca3-43e5-8c0f-af8c00a7a573/receipts/dbda92ec-832a-4228-92cf-af8c00a95f9f/files/5ed5aa3c-3cf0-4389-82f8-64b95a67020a/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "be71a876-c551-476c-9699-af8c00ad60cb",
        "CorrelationId": null,
        "GroupId": "2877433d-08e1-4fea-b993-74c7a02df7b1",
        "Type": "inbox",
        "Title": "№ ИН-018-34/4 от 16/01/2023 Информационные письма Банка России",
        "Text": "Информационное письмо о мерах поддержки депозитариев",
        "CreationDate": "2023-01-16T10:31:20Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "ИН-018-34/4",
        "TotalSize": 414802,
        "Sender": null,
        "Files": [
            {
                "Id": "baffa6d0-a51b-4ab8-b696-135c49111bc4",
                "Name": "ЭД_4_IN22.pdf",
                "Description": "613706150.pdf",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 102192,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/be71a876-c551-476c-9699-af8c00ad60cb/files/baffa6d0-a51b-4ab8-b696-135c49111bc4/download"
                    }
                ]
            },
            {
                "Id": "847c1e99-f3d3-4327-80d4-85976551f2dd",
                "Name": "ЭД_4_IN22.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "baffa6d0-a51b-4ab8-b696-135c49111bc4",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/be71a876-c551-476c-9699-af8c00ad60cb/files/847c1e99-f3d3-4327-80d4-85976551f2dd/download"
                    }
                ]
            },
            {
                "Id": "a965ada7-3e67-47e7-8c25-9b9e087cbfb1",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 192019,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/be71a876-c551-476c-9699-af8c00ad60cb/files/a965ada7-3e67-47e7-8c25-9b9e087cbfb1/download"
                    }
                ]
            },
            {
                "Id": "46b3b7ec-e603-4585-814a-d89576330f49",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "613707802.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 114069,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/be71a876-c551-476c-9699-af8c00ad60cb/files/46b3b7ec-e603-4585-814a-d89576330f49/download"
                    }
                ]
            },
            {
                "Id": "3f49f0cf-93ac-4e00-9f5d-dad23e00957b",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "46b3b7ec-e603-4585-814a-d89576330f49",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/be71a876-c551-476c-9699-af8c00ad60cb/files/3f49f0cf-93ac-4e00-9f5d-dad23e00957b/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "965d7476-392c-41dd-87da-af8c00be373a",
        "CorrelationId": null,
        "GroupId": "767cb049-8c56-483f-8a60-cc169affdf9a",
        "Type": "inbox",
        "Title": "№ ТД14-12-3/903 от 16/01/2023 Переписка с внешними организациями",
        "Text": "Об оценке финансового положения владельцев (контролеров) крупного пакета акций (долей) кредитной организации за 2022 год",
        "CreationDate": "2023-01-16T11:32:37Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "ТД14-12-3/903",
        "TotalSize": 605502,
        "Sender": null,
        "Files": [
            {
                "Id": "7fd17638-a2db-44f8-ba67-38eb12e7c1f3",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 147270,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/965d7476-392c-41dd-87da-af8c00be373a/files/7fd17638-a2db-44f8-ba67-38eb12e7c1f3/download"
                    }
                ]
            },
            {
                "Id": "73da3bce-fcd2-4833-b482-633ace3b6a21",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "0375ecae-a5b0-4b61-98a4-d13cb59acec1",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/965d7476-392c-41dd-87da-af8c00be373a/files/73da3bce-fcd2-4833-b482-633ace3b6a21/download"
                    }
                ]
            },
            {
                "Id": "020d75ea-6d21-440f-bbbe-7c6a1059feed",
                "Name": "ЭД_903.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "75ea4fe6-400b-4c52-b560-ee48c384ea2b",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/965d7476-392c-41dd-87da-af8c00be373a/files/020d75ea-6d21-440f-bbbe-7c6a1059feed/download"
                    }
                ]
            },
            {
                "Id": "0375ecae-a5b0-4b61-98a4-d13cb59acec1",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "113419031.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 272575,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/965d7476-392c-41dd-87da-af8c00be373a/files/0375ecae-a5b0-4b61-98a4-d13cb59acec1/download"
                    }
                ]
            },
            {
                "Id": "75ea4fe6-400b-4c52-b560-ee48c384ea2b",
                "Name": "ЭД_903.pdf",
                "Description": "113416645.pdf",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 179135,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/965d7476-392c-41dd-87da-af8c00be373a/files/75ea4fe6-400b-4c52-b560-ee48c384ea2b/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "893a4db3-2e9e-4169-8932-af8c01289f3f",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-16T17:59:58Z",
        "UpdatedDate": "2023-01-16T18:02:28Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00570652",
        "TotalSize": 13373,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "5c4b1b43-a84a-4d09-9462-af8c01289f3b",
                "Name": "KYCCL_7831001422_3194_20230116_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9501,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/893a4db3-2e9e-4169-8932-af8c01289f3f/files/5c4b1b43-a84a-4d09-9462-af8c01289f3b/download"
                    }
                ]
            },
            {
                "Id": "93f77777-105b-4118-9ff3-af8c01289f3d",
                "Name": "KYCCL_7831001422_3194_20230116_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "5c4b1b43-a84a-4d09-9462-af8c01289f3b",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/893a4db3-2e9e-4169-8932-af8c01289f3f/files/93f77777-105b-4118-9ff3-af8c01289f3d/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "bdff8961-9da9-4cfa-ab1b-af8c0128a081",
                "ReceiveTime": "2023-01-16T17:59:59Z",
                "StatusTime": "2023-01-16T17:59:59Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "b089d08b-e5c8-4003-a624-af8c0128b08c",
                "ReceiveTime": "2023-01-16T18:00:12Z",
                "StatusTime": "2023-01-16T18:00:09Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "7a2eae8f-4f26-491f-8315-4be22ca67a16",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/893a4db3-2e9e-4169-8932-af8c01289f3f/receipts/b089d08b-e5c8-4003-a624-af8c0128b08c/files/7a2eae8f-4f26-491f-8315-4be22ca67a16/download"
                            }
                        ]
                    },
                    {
                        "Id": "7c6ea9bc-47e7-4ce2-a2ff-5f9a6fa57013",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "7a2eae8f-4f26-491f-8315-4be22ca67a16",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/893a4db3-2e9e-4169-8932-af8c01289f3f/receipts/b089d08b-e5c8-4003-a624-af8c0128b08c/files/7c6ea9bc-47e7-4ce2-a2ff-5f9a6fa57013/download"
                            }
                        ]
                    },
                    {
                        "Id": "aeccf614-f188-414c-bc0d-91e7716169f7",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "d773bf9b-fb66-406f-aae5-e51ec7e76a80",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/893a4db3-2e9e-4169-8932-af8c01289f3f/receipts/b089d08b-e5c8-4003-a624-af8c0128b08c/files/aeccf614-f188-414c-bc0d-91e7716169f7/download"
                            }
                        ]
                    },
                    {
                        "Id": "d773bf9b-fb66-406f-aae5-e51ec7e76a80",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/893a4db3-2e9e-4169-8932-af8c01289f3f/receipts/b089d08b-e5c8-4003-a624-af8c0128b08c/files/d773bf9b-fb66-406f-aae5-e51ec7e76a80/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "095811b0-ba75-45aa-80b5-af8c0128b7e2",
                "ReceiveTime": "2023-01-16T18:00:19Z",
                "StatusTime": "2023-01-16T18:00:12Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "3c36dde2-55ff-4011-abe7-334ac4c3d58e",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "389c7a1c-9b79-4c7f-9207-c11d4c147391",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/893a4db3-2e9e-4169-8932-af8c01289f3f/receipts/095811b0-ba75-45aa-80b5-af8c0128b7e2/files/3c36dde2-55ff-4011-abe7-334ac4c3d58e/download"
                            }
                        ]
                    },
                    {
                        "Id": "06e8979f-e79b-4488-b376-6a4752315fc3",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/893a4db3-2e9e-4169-8932-af8c01289f3f/receipts/095811b0-ba75-45aa-80b5-af8c0128b7e2/files/06e8979f-e79b-4488-b376-6a4752315fc3/download"
                            }
                        ]
                    },
                    {
                        "Id": "1a8c7e68-48ba-4970-a9ef-6d8dd45606c8",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "06e8979f-e79b-4488-b376-6a4752315fc3",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/893a4db3-2e9e-4169-8932-af8c01289f3f/receipts/095811b0-ba75-45aa-80b5-af8c0128b7e2/files/1a8c7e68-48ba-4970-a9ef-6d8dd45606c8/download"
                            }
                        ]
                    },
                    {
                        "Id": "389c7a1c-9b79-4c7f-9207-c11d4c147391",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/893a4db3-2e9e-4169-8932-af8c01289f3f/receipts/095811b0-ba75-45aa-80b5-af8c0128b7e2/files/389c7a1c-9b79-4c7f-9207-c11d4c147391/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "42a6cf92-fee8-4e4c-a91b-af8c01294ffd",
                "ReceiveTime": "2023-01-16T18:02:28Z",
                "StatusTime": "2023-01-16T18:01:55Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "8049bd7e-97b3-49f6-abbe-8f951d30fce3",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "dc36a478-67a4-42c9-a375-9a653f0893f8",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/893a4db3-2e9e-4169-8932-af8c01289f3f/receipts/42a6cf92-fee8-4e4c-a91b-af8c01294ffd/files/8049bd7e-97b3-49f6-abbe-8f951d30fce3/download"
                            }
                        ]
                    },
                    {
                        "Id": "dc36a478-67a4-42c9-a375-9a653f0893f8",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/893a4db3-2e9e-4169-8932-af8c01289f3f/receipts/42a6cf92-fee8-4e4c-a91b-af8c01294ffd/files/dc36a478-67a4-42c9-a375-9a653f0893f8/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "43c35e37-3208-4070-b254-af8d00ca5302",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Обращение (запрос)  в Банк России",
        "Text": "расчет размера операционного риска в соответствии с требованиями 744-П",
        "CreationDate": "2023-01-17T12:16:38Z",
        "UpdatedDate": "2023-01-17T12:32:20Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "19123",
        "TotalSize": 174143,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "7d97347f-ed63-4c29-a9af-af8d00ca5365",
                "Name": "2023_01_17 7-3.pdf.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 117482,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/files/7d97347f-ed63-4c29-a9af-af8d00ca5365/download"
                    }
                ]
            },
            {
                "Id": "a7cb7b45-323f-462a-8623-af8d00ca5366",
                "Name": "Отчет о расчете ОР_2023.docx.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 36163,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/files/a7cb7b45-323f-462a-8623-af8d00ca5366/download"
                    }
                ]
            },
            {
                "Id": "de9875ea-1051-4d7c-9bc4-af8d00ca5376",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2375,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/files/de9875ea-1051-4d7c-9bc4-af8d00ca5376/download"
                    }
                ]
            },
            {
                "Id": "08624231-0b25-4e7e-91a3-af8d00caa367",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "de9875ea-1051-4d7c-9bc4-af8d00ca5376",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/files/08624231-0b25-4e7e-91a3-af8d00caa367/download"
                    }
                ]
            },
            {
                "Id": "7b1785ad-8fff-4f9a-afdf-af8d00caee78",
                "Name": "2023_01_17 7-3.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "7d97347f-ed63-4c29-a9af-af8d00ca5365",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/files/7b1785ad-8fff-4f9a-afdf-af8d00caee78/download"
                    }
                ]
            },
            {
                "Id": "9e9d099b-d4e9-469a-a8fe-af8d00cb3992",
                "Name": "Отчет о расчете ОР_2023.docx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "a7cb7b45-323f-462a-8623-af8d00ca5366",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/files/9e9d099b-d4e9-469a-a8fe-af8d00cb3992/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "1b50974c-0933-4773-a806-af8d00cb3b8b",
                "ReceiveTime": "2023-01-17T12:19:56Z",
                "StatusTime": "2023-01-17T12:19:56Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "d76d8502-848d-4a13-a9da-af8d00cb4f0e",
                "ReceiveTime": "2023-01-17T12:20:13Z",
                "StatusTime": "2023-01-17T12:20:09Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "4947e884-bc57-4f2c-90cf-1adba928347b",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/receipts/d76d8502-848d-4a13-a9da-af8d00cb4f0e/files/4947e884-bc57-4f2c-90cf-1adba928347b/download"
                            }
                        ]
                    },
                    {
                        "Id": "751bb38d-368f-43ce-84f2-66e09643f3f1",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "0b735537-4f69-489e-b96d-a05395bc790c",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/receipts/d76d8502-848d-4a13-a9da-af8d00cb4f0e/files/751bb38d-368f-43ce-84f2-66e09643f3f1/download"
                            }
                        ]
                    },
                    {
                        "Id": "4882fdd8-62c8-4b45-ac8b-938173f514e7",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "4947e884-bc57-4f2c-90cf-1adba928347b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/receipts/d76d8502-848d-4a13-a9da-af8d00cb4f0e/files/4882fdd8-62c8-4b45-ac8b-938173f514e7/download"
                            }
                        ]
                    },
                    {
                        "Id": "0b735537-4f69-489e-b96d-a05395bc790c",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/receipts/d76d8502-848d-4a13-a9da-af8d00cb4f0e/files/0b735537-4f69-489e-b96d-a05395bc790c/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "6a14c3e5-e48b-4e32-941f-af8d00cb5926",
                "ReceiveTime": "2023-01-17T12:20:22Z",
                "StatusTime": "2023-01-17T12:20:13Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "75f7e564-ae24-488a-912b-1c859b060254",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/receipts/6a14c3e5-e48b-4e32-941f-af8d00cb5926/files/75f7e564-ae24-488a-912b-1c859b060254/download"
                            }
                        ]
                    },
                    {
                        "Id": "431f3b16-b9ff-4ab7-8602-2f07c9b66e98",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "4034939b-3113-4820-b492-6593dd6dd492",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/receipts/6a14c3e5-e48b-4e32-941f-af8d00cb5926/files/431f3b16-b9ff-4ab7-8602-2f07c9b66e98/download"
                            }
                        ]
                    },
                    {
                        "Id": "4034939b-3113-4820-b492-6593dd6dd492",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/receipts/6a14c3e5-e48b-4e32-941f-af8d00cb5926/files/4034939b-3113-4820-b492-6593dd6dd492/download"
                            }
                        ]
                    },
                    {
                        "Id": "484fa1b5-5690-4fee-83f6-85c6915345ed",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "75f7e564-ae24-488a-912b-1c859b060254",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/receipts/6a14c3e5-e48b-4e32-941f-af8d00cb5926/files/484fa1b5-5690-4fee-83f6-85c6915345ed/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "b3d527f7-7f7a-424e-bcc8-af8d00cea326",
                "ReceiveTime": "2023-01-17T12:32:20Z",
                "StatusTime": "2023-01-17T12:32:12Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "0c51ae24-ed0f-49f2-a742-05a7d4e26ecc",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/receipts/b3d527f7-7f7a-424e-bcc8-af8d00cea326/files/0c51ae24-ed0f-49f2-a742-05a7d4e26ecc/download"
                            }
                        ]
                    },
                    {
                        "Id": "2e861a76-8943-425d-b491-11560ca9819b",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 742,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/receipts/b3d527f7-7f7a-424e-bcc8-af8d00cea326/files/2e861a76-8943-425d-b491-11560ca9819b/download"
                            }
                        ]
                    },
                    {
                        "Id": "004fc589-abcf-4e78-9cee-5ab0f5b0422a",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "2e861a76-8943-425d-b491-11560ca9819b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/receipts/b3d527f7-7f7a-424e-bcc8-af8d00cea326/files/004fc589-abcf-4e78-9cee-5ab0f5b0422a/download"
                            }
                        ]
                    },
                    {
                        "Id": "14b2629e-aea7-49f0-b95f-9fcbc3081f59",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "0c51ae24-ed0f-49f2-a742-05a7d4e26ecc",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/43c35e37-3208-4070-b254-af8d00ca5302/receipts/b3d527f7-7f7a-424e-bcc8-af8d00cea326/files/14b2629e-aea7-49f0-b95f-9fcbc3081f59/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "e653ff8b-681c-4e2f-8d1f-af8d00cc1833",
        "CorrelationId": "1f6158a2-a7a1-4e14-aace-af7a00f65145",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "в дополнение к нашему исх. 1-3 от 09.01.2023",
        "CreationDate": "2023-01-17T12:23:05Z",
        "UpdatedDate": "2023-01-17T12:33:20Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "19125",
        "TotalSize": 89706,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "f486b0cf-723e-4a88-983a-af8d00cc18f7",
                "Name": "2023-01-09-1-3-6.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 75621,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/files/f486b0cf-723e-4a88-983a-af8d00cc18f7/download"
                    }
                ]
            },
            {
                "Id": "e7d9bba1-8d9c-40c9-8fde-af8d00cc1909",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2003,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/files/e7d9bba1-8d9c-40c9-8fde-af8d00cc1909/download"
                    }
                ]
            },
            {
                "Id": "58b8a411-2c8e-4ae8-84f3-af8d00cc6595",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "e7d9bba1-8d9c-40c9-8fde-af8d00cc1909",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/files/58b8a411-2c8e-4ae8-84f3-af8d00cc6595/download"
                    }
                ]
            },
            {
                "Id": "238a66fd-288f-4f32-90f7-af8d00ccb0b4",
                "Name": "2023-01-09-1-3-6.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "f486b0cf-723e-4a88-983a-af8d00cc18f7",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/files/238a66fd-288f-4f32-90f7-af8d00ccb0b4/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "0eef2c9c-3bac-4dbf-9733-af8d00ccb303",
                "ReceiveTime": "2023-01-17T12:25:17Z",
                "StatusTime": "2023-01-17T12:25:17Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "3aa983f4-c3d2-4bb0-9cb4-af8d00ccc6f9",
                "ReceiveTime": "2023-01-17T12:25:34Z",
                "StatusTime": "2023-01-17T12:25:30Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "c857a511-7c67-41aa-9b47-39ecde696915",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f1579695-0e91-4c7d-8a9d-3ce03a8a47b8",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/receipts/3aa983f4-c3d2-4bb0-9cb4-af8d00ccc6f9/files/c857a511-7c67-41aa-9b47-39ecde696915/download"
                            }
                        ]
                    },
                    {
                        "Id": "f1579695-0e91-4c7d-8a9d-3ce03a8a47b8",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/receipts/3aa983f4-c3d2-4bb0-9cb4-af8d00ccc6f9/files/f1579695-0e91-4c7d-8a9d-3ce03a8a47b8/download"
                            }
                        ]
                    },
                    {
                        "Id": "c0b6f483-d274-445f-9cdc-676a7ee613bf",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "14db65fb-224e-4ab8-9b1b-f6b3403d0365",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/receipts/3aa983f4-c3d2-4bb0-9cb4-af8d00ccc6f9/files/c0b6f483-d274-445f-9cdc-676a7ee613bf/download"
                            }
                        ]
                    },
                    {
                        "Id": "14db65fb-224e-4ab8-9b1b-f6b3403d0365",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/receipts/3aa983f4-c3d2-4bb0-9cb4-af8d00ccc6f9/files/14db65fb-224e-4ab8-9b1b-f6b3403d0365/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "3aea25a5-484f-49cc-bddc-af8d00ccd153",
                "ReceiveTime": "2023-01-17T12:25:43Z",
                "StatusTime": "2023-01-17T12:25:33Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "2ef18426-bdcc-41fb-809a-089957f40afd",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "ee5a5a27-d611-4857-8525-c6ea33403f38",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/receipts/3aea25a5-484f-49cc-bddc-af8d00ccd153/files/2ef18426-bdcc-41fb-809a-089957f40afd/download"
                            }
                        ]
                    },
                    {
                        "Id": "5a7b8230-ef3e-43e8-b886-790f377d6fd3",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/receipts/3aea25a5-484f-49cc-bddc-af8d00ccd153/files/5a7b8230-ef3e-43e8-b886-790f377d6fd3/download"
                            }
                        ]
                    },
                    {
                        "Id": "83c50835-e8cf-4735-9655-b3c466e69df4",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "5a7b8230-ef3e-43e8-b886-790f377d6fd3",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/receipts/3aea25a5-484f-49cc-bddc-af8d00ccd153/files/83c50835-e8cf-4735-9655-b3c466e69df4/download"
                            }
                        ]
                    },
                    {
                        "Id": "ee5a5a27-d611-4857-8525-c6ea33403f38",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/receipts/3aea25a5-484f-49cc-bddc-af8d00ccd153/files/ee5a5a27-d611-4857-8525-c6ea33403f38/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "3958fe03-125b-4c16-9c40-af8d00cee94a",
                "ReceiveTime": "2023-01-17T12:33:20Z",
                "StatusTime": "2023-01-17T12:33:12Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "375cae21-3264-40e4-b86d-17d4c00ec848",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 742,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/receipts/3958fe03-125b-4c16-9c40-af8d00cee94a/files/375cae21-3264-40e4-b86d-17d4c00ec848/download"
                            }
                        ]
                    },
                    {
                        "Id": "63360fcd-45c4-43a8-bdab-36c995f9526b",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/receipts/3958fe03-125b-4c16-9c40-af8d00cee94a/files/63360fcd-45c4-43a8-bdab-36c995f9526b/download"
                            }
                        ]
                    },
                    {
                        "Id": "8125c140-cfdb-411b-a2e8-5a98b53c7792",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "63360fcd-45c4-43a8-bdab-36c995f9526b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/receipts/3958fe03-125b-4c16-9c40-af8d00cee94a/files/8125c140-cfdb-411b-a2e8-5a98b53c7792/download"
                            }
                        ]
                    },
                    {
                        "Id": "040d66c0-3296-4788-b987-9d975b971300",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "375cae21-3264-40e4-b86d-17d4c00ec848",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/e653ff8b-681c-4e2f-8d1f-af8d00cc1833/receipts/3958fe03-125b-4c16-9c40-af8d00cee94a/files/040d66c0-3296-4788-b987-9d975b971300/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "79f0d630-c7a4-4689-8876-af8d00cedf94",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Обращение (запрос)  в Банк России",
        "Text": "в дополнение к нашему 1-3-1 от 09.01.2023",
        "CreationDate": "2023-01-17T12:33:12Z",
        "UpdatedDate": "2023-01-17T12:39:09Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2-4140",
        "TotalSize": 90081,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "4825efa7-7cd2-446d-9ea7-af8d00cedff7",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2366,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/files/4825efa7-7cd2-446d-9ea7-af8d00cedff7/download"
                    }
                ]
            },
            {
                "Id": "5e970047-3673-4c8d-ae27-af8d00cedff7",
                "Name": "2023-01-09-1-3-6-1.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 75633,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/files/5e970047-3673-4c8d-ae27-af8d00cedff7/download"
                    }
                ]
            },
            {
                "Id": "2fac2d7e-f73d-4879-9ace-af8d00cf2bbf",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "4825efa7-7cd2-446d-9ea7-af8d00cedff7",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/files/2fac2d7e-f73d-4879-9ace-af8d00cf2bbf/download"
                    }
                ]
            },
            {
                "Id": "00f4c570-8b2b-4dd9-8b58-af8d00cf7708",
                "Name": "2023-01-09-1-3-6-1.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "5e970047-3673-4c8d-ae27-af8d00cedff7",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/files/00f4c570-8b2b-4dd9-8b58-af8d00cf7708/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "81aabd2d-d02c-4ebf-998c-af8d00cf9932",
                "ReceiveTime": "2023-01-17T12:35:50Z",
                "StatusTime": "2023-01-17T12:35:50Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "d015534c-381c-4e2c-b653-af8d00cfa3d7",
                "ReceiveTime": "2023-01-17T12:35:59Z",
                "StatusTime": "2023-01-17T12:35:55Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "461d0a56-3427-452f-8906-34998e808f83",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/receipts/d015534c-381c-4e2c-b653-af8d00cfa3d7/files/461d0a56-3427-452f-8906-34998e808f83/download"
                            }
                        ]
                    },
                    {
                        "Id": "1fb80b6f-b936-499d-b2d3-404f481fd192",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/receipts/d015534c-381c-4e2c-b653-af8d00cfa3d7/files/1fb80b6f-b936-499d-b2d3-404f481fd192/download"
                            }
                        ]
                    },
                    {
                        "Id": "0ebaff17-cd8c-452e-934b-d93f55e64888",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "1fb80b6f-b936-499d-b2d3-404f481fd192",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/receipts/d015534c-381c-4e2c-b653-af8d00cfa3d7/files/0ebaff17-cd8c-452e-934b-d93f55e64888/download"
                            }
                        ]
                    },
                    {
                        "Id": "4d73a4c0-d1e4-4ff2-9834-f5a6fe36bbb8",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "461d0a56-3427-452f-8906-34998e808f83",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/receipts/d015534c-381c-4e2c-b653-af8d00cfa3d7/files/4d73a4c0-d1e4-4ff2-9834-f5a6fe36bbb8/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "75f1a428-e122-40a4-8934-af8d00cfa889",
                "ReceiveTime": "2023-01-17T12:36:03Z",
                "StatusTime": "2023-01-17T12:35:57Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "f8bf516a-c2e9-4394-ad14-07ba2fc763b8",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/receipts/75f1a428-e122-40a4-8934-af8d00cfa889/files/f8bf516a-c2e9-4394-ad14-07ba2fc763b8/download"
                            }
                        ]
                    },
                    {
                        "Id": "1e011ba8-b702-4fdd-b9b2-7b7a99c9ec74",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f8bf516a-c2e9-4394-ad14-07ba2fc763b8",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/receipts/75f1a428-e122-40a4-8934-af8d00cfa889/files/1e011ba8-b702-4fdd-b9b2-7b7a99c9ec74/download"
                            }
                        ]
                    },
                    {
                        "Id": "7aa372e0-a601-40ff-acf9-7e39c1a94a37",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/receipts/75f1a428-e122-40a4-8934-af8d00cfa889/files/7aa372e0-a601-40ff-acf9-7e39c1a94a37/download"
                            }
                        ]
                    },
                    {
                        "Id": "24a8ff8e-c5c1-4b71-a8a4-9ef27596938d",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "7aa372e0-a601-40ff-acf9-7e39c1a94a37",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/receipts/75f1a428-e122-40a4-8934-af8d00cfa889/files/24a8ff8e-c5c1-4b71-a8a4-9ef27596938d/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "e6cf643c-8c68-4782-b9f4-af8d00d08296",
                "ReceiveTime": "2023-01-17T12:39:09Z",
                "StatusTime": "2023-01-17T12:39:03Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "a4927112-b2b7-4079-adb3-20c91d789b5a",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 780,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/receipts/e6cf643c-8c68-4782-b9f4-af8d00d08296/files/a4927112-b2b7-4079-adb3-20c91d789b5a/download"
                            }
                        ]
                    },
                    {
                        "Id": "9ef7339f-eff0-4cf5-9068-937eaa73ea90",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "a4927112-b2b7-4079-adb3-20c91d789b5a",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/receipts/e6cf643c-8c68-4782-b9f4-af8d00d08296/files/9ef7339f-eff0-4cf5-9068-937eaa73ea90/download"
                            }
                        ]
                    },
                    {
                        "Id": "36aac6fc-a68d-4d96-9359-b4b1a2650550",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "cd20265b-b60a-488d-a83b-d3df22023526",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/receipts/e6cf643c-8c68-4782-b9f4-af8d00d08296/files/36aac6fc-a68d-4d96-9359-b4b1a2650550/download"
                            }
                        ]
                    },
                    {
                        "Id": "cd20265b-b60a-488d-a83b-d3df22023526",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 355,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/79f0d630-c7a4-4689-8876-af8d00cedf94/receipts/e6cf643c-8c68-4782-b9f4-af8d00d08296/files/cd20265b-b60a-488d-a83b-d3df22023526/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "1b74a994-757c-4aa1-8293-af8d0119b513",
        "CorrelationId": null,
        "GroupId": "0e172963-855a-424f-aaa7-ea72e96657ab",
        "Type": "inbox",
        "Title": "№ 08-42-6/282 от 17/01/2023 Письма за подписью руководства Банка России",
        "Text": "Об отмене мониторинга",
        "CreationDate": "2023-01-17T17:05:42Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "08-42-6/282",
        "TotalSize": 1153206,
        "Sender": null,
        "Files": [
            {
                "Id": "632a69d0-9cf8-40cd-9573-38944d309ffe",
                "Name": "Список рассылки.xlsx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "1ce42e69-da9f-4242-976d-51021ec4f6d4",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1b74a994-757c-4aa1-8293-af8d0119b513/files/632a69d0-9cf8-40cd-9573-38944d309ffe/download"
                    }
                ]
            },
            {
                "Id": "1ce42e69-da9f-4242-976d-51021ec4f6d4",
                "Name": "Список рассылки.xlsx.enc",
                "Description": "614310700.xlsx.enc",
                "Encrypted": true,
                "SignedFile": null,
                "Size": 687552,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1b74a994-757c-4aa1-8293-af8d0119b513/files/1ce42e69-da9f-4242-976d-51021ec4f6d4/download"
                    }
                ]
            },
            {
                "Id": "35a4a541-e42f-4073-b299-9281c21b5647",
                "Name": "ЭД_282.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "582b66ef-1dc3-4101-bada-dad2551aa24f",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1b74a994-757c-4aa1-8293-af8d0119b513/files/35a4a541-e42f-4073-b299-9281c21b5647/download"
                    }
                ]
            },
            {
                "Id": "2d8bb77d-daa6-4c8a-86cc-ba3438e02585",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "614311492.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 99565,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1b74a994-757c-4aa1-8293-af8d0119b513/files/2d8bb77d-daa6-4c8a-86cc-ba3438e02585/download"
                    }
                ]
            },
            {
                "Id": "832ec99b-a216-4579-a9b7-c97a336e337e",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 268521,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1b74a994-757c-4aa1-8293-af8d0119b513/files/832ec99b-a216-4579-a9b7-c97a336e337e/download"
                    }
                ]
            },
            {
                "Id": "080c3629-35ee-4259-9352-d28b5de5a2b8",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "2d8bb77d-daa6-4c8a-86cc-ba3438e02585",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1b74a994-757c-4aa1-8293-af8d0119b513/files/080c3629-35ee-4259-9352-d28b5de5a2b8/download"
                    }
                ]
            },
            {
                "Id": "582b66ef-1dc3-4101-bada-dad2551aa24f",
                "Name": "ЭД_282.pdf",
                "Description": "614310697.pdf",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 87785,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1b74a994-757c-4aa1-8293-af8d0119b513/files/582b66ef-1dc3-4101-bada-dad2551aa24f/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "7ad661cb-53f9-4975-b759-af8d01289d98",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-17T17:59:56Z",
        "UpdatedDate": "2023-01-17T18:03:13Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00571636",
        "TotalSize": 13356,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "3188d89f-b146-4767-bcc0-af8d01289d93",
                "Name": "KYCCL_7831001422_3194_20230117_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9484,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7ad661cb-53f9-4975-b759-af8d01289d98/files/3188d89f-b146-4767-bcc0-af8d01289d93/download"
                    }
                ]
            },
            {
                "Id": "ed7d608b-9a2c-46b3-b8a6-af8d01289d96",
                "Name": "KYCCL_7831001422_3194_20230117_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "3188d89f-b146-4767-bcc0-af8d01289d93",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7ad661cb-53f9-4975-b759-af8d01289d98/files/ed7d608b-9a2c-46b3-b8a6-af8d01289d96/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "28f09b51-ec7d-44be-8304-af8d01289f3f",
                "ReceiveTime": "2023-01-17T17:59:58Z",
                "StatusTime": "2023-01-17T17:59:58Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "58e0c0bf-cb45-4e83-9c06-af8d0128b0eb",
                "ReceiveTime": "2023-01-17T18:00:13Z",
                "StatusTime": "2023-01-17T18:00:09Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "564fb9f5-999b-4c7c-8003-4536b5b44b1d",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ad661cb-53f9-4975-b759-af8d01289d98/receipts/58e0c0bf-cb45-4e83-9c06-af8d0128b0eb/files/564fb9f5-999b-4c7c-8003-4536b5b44b1d/download"
                            }
                        ]
                    },
                    {
                        "Id": "502607db-8578-47e3-9f87-4df20781e596",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f4fced6a-b1fb-4517-9563-9ba5776d0da9",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ad661cb-53f9-4975-b759-af8d01289d98/receipts/58e0c0bf-cb45-4e83-9c06-af8d0128b0eb/files/502607db-8578-47e3-9f87-4df20781e596/download"
                            }
                        ]
                    },
                    {
                        "Id": "4639bbc2-5620-4a7a-a2ca-60d76b1a8f2d",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "564fb9f5-999b-4c7c-8003-4536b5b44b1d",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ad661cb-53f9-4975-b759-af8d01289d98/receipts/58e0c0bf-cb45-4e83-9c06-af8d0128b0eb/files/4639bbc2-5620-4a7a-a2ca-60d76b1a8f2d/download"
                            }
                        ]
                    },
                    {
                        "Id": "f4fced6a-b1fb-4517-9563-9ba5776d0da9",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ad661cb-53f9-4975-b759-af8d01289d98/receipts/58e0c0bf-cb45-4e83-9c06-af8d0128b0eb/files/f4fced6a-b1fb-4517-9563-9ba5776d0da9/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "babb0833-5f85-4036-b9f1-af8d0128b94c",
                "ReceiveTime": "2023-01-17T18:00:20Z",
                "StatusTime": "2023-01-17T18:00:12Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "c629f6f5-c39a-46d0-a5ff-1b0939cb0114",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ad661cb-53f9-4975-b759-af8d01289d98/receipts/babb0833-5f85-4036-b9f1-af8d0128b94c/files/c629f6f5-c39a-46d0-a5ff-1b0939cb0114/download"
                            }
                        ]
                    },
                    {
                        "Id": "d0d542fb-240b-4532-be41-5852ec6176d5",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "34bdccc7-4a8e-4dd5-8b00-7aa3ddac797e",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ad661cb-53f9-4975-b759-af8d01289d98/receipts/babb0833-5f85-4036-b9f1-af8d0128b94c/files/d0d542fb-240b-4532-be41-5852ec6176d5/download"
                            }
                        ]
                    },
                    {
                        "Id": "690f1012-f32d-4479-996e-5d9fc98ab012",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "c629f6f5-c39a-46d0-a5ff-1b0939cb0114",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ad661cb-53f9-4975-b759-af8d01289d98/receipts/babb0833-5f85-4036-b9f1-af8d0128b94c/files/690f1012-f32d-4479-996e-5d9fc98ab012/download"
                            }
                        ]
                    },
                    {
                        "Id": "34bdccc7-4a8e-4dd5-8b00-7aa3ddac797e",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ad661cb-53f9-4975-b759-af8d01289d98/receipts/babb0833-5f85-4036-b9f1-af8d0128b94c/files/34bdccc7-4a8e-4dd5-8b00-7aa3ddac797e/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "fd04f497-a5d8-448e-ae98-af8d012984ab",
                "ReceiveTime": "2023-01-17T18:03:13Z",
                "StatusTime": "2023-01-17T18:02:38Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "280f22dc-8e70-4106-8d00-415049ff025d",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "1f5f1405-642f-4978-b9eb-5d9e0eb06a9c",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ad661cb-53f9-4975-b759-af8d01289d98/receipts/fd04f497-a5d8-448e-ae98-af8d012984ab/files/280f22dc-8e70-4106-8d00-415049ff025d/download"
                            }
                        ]
                    },
                    {
                        "Id": "1f5f1405-642f-4978-b9eb-5d9e0eb06a9c",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ad661cb-53f9-4975-b759-af8d01289d98/receipts/fd04f497-a5d8-448e-ae98-af8d012984ab/files/1f5f1405-642f-4978-b9eb-5d9e0eb06a9c/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "6e296b50-399e-410b-9056-af8e00683781",
        "CorrelationId": null,
        "GroupId": "57200239-942f-4512-bfe1-0ef235155801",
        "Type": "inbox",
        "Title": "№ 34-3-3-1/193 от 17/01/2023 (34) Письма Деп. инфраструктуры финансового рынка",
        "Text": "О назначении кураторов АО «Сити Инвест Банк»",
        "CreationDate": "2023-01-18T06:19:26Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "34-3-3-1/193",
        "TotalSize": 258100,
        "Sender": null,
        "Files": [
            {
                "Id": "ac9814cd-6b68-4010-8db1-96c88081c153",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "614278895.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 120345,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6e296b50-399e-410b-9056-af8e00683781/files/ac9814cd-6b68-4010-8db1-96c88081c153/download"
                    }
                ]
            },
            {
                "Id": "c39e7039-34cb-4e1d-ab92-9bf7b5f520b5",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 20748,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6e296b50-399e-410b-9056-af8e00683781/files/c39e7039-34cb-4e1d-ab92-9bf7b5f520b5/download"
                    }
                ]
            },
            {
                "Id": "c39a43a4-ed63-45d3-810c-abdd0cf6b4ca",
                "Name": "ЭД_Акционерное общество Сити Инвест Банк.pdf",
                "Description": "614278773.pdf",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 110485,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6e296b50-399e-410b-9056-af8e00683781/files/c39a43a4-ed63-45d3-810c-abdd0cf6b4ca/download"
                    }
                ]
            },
            {
                "Id": "88c1de14-3833-4f02-944f-cba46b979333",
                "Name": "ЭД_Акционерное общество Сити Инвест Банк.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "c39a43a4-ed63-45d3-810c-abdd0cf6b4ca",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6e296b50-399e-410b-9056-af8e00683781/files/88c1de14-3833-4f02-944f-cba46b979333/download"
                    }
                ]
            },
            {
                "Id": "0d3782fc-2841-47d1-81cb-f70881dd24d9",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "ac9814cd-6b68-4010-8db1-96c88081c153",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6e296b50-399e-410b-9056-af8e00683781/files/0d3782fc-2841-47d1-81cb-f70881dd24d9/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "7ba6f090-beac-42bb-8633-af8e00d40b00",
        "CorrelationId": "cdd2e3bf-c330-4775-b6eb-af7700605219",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "предоставление запрошенной информации",
        "CreationDate": "2023-01-18T12:52:01Z",
        "UpdatedDate": "2023-01-18T13:00:29Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "21999",
        "TotalSize": 398000,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "d773389b-9fbe-483c-ac71-af8e00d40b6d",
                "Name": "Приложение_выписка.pdf.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 294306,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/files/d773389b-9fbe-483c-ac71-af8e00d40b6d/download"
                    }
                ]
            },
            {
                "Id": "6420a985-cf3d-4b4c-a135-af8e00d40bd3",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2006,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/files/6420a985-cf3d-4b4c-a135-af8e00d40bd3/download"
                    }
                ]
            },
            {
                "Id": "12cc09ab-3aa5-48fd-93f1-af8e00d40bd6",
                "Name": "2023-01-18-8-2.pdf.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 83565,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/files/12cc09ab-3aa5-48fd-93f1-af8e00d40bd6/download"
                    }
                ]
            },
            {
                "Id": "4b6ade46-3822-409f-b5ac-af8e00d466ce",
                "Name": "2023-01-18-8-2.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "12cc09ab-3aa5-48fd-93f1-af8e00d40bd6",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/files/4b6ade46-3822-409f-b5ac-af8e00d466ce/download"
                    }
                ]
            },
            {
                "Id": "5e593cdc-71a7-4a39-9d54-af8e00d4bdc2",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "6420a985-cf3d-4b4c-a135-af8e00d40bd3",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/files/5e593cdc-71a7-4a39-9d54-af8e00d4bdc2/download"
                    }
                ]
            },
            {
                "Id": "81583ef2-127d-4c3f-84a2-af8e00d5147d",
                "Name": "Приложение_выписка.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "d773389b-9fbe-483c-ac71-af8e00d40b6d",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/files/81583ef2-127d-4c3f-84a2-af8e00d5147d/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "80afc8e6-0bb4-456d-bee6-af8e00d515de",
                "ReceiveTime": "2023-01-18T12:55:49Z",
                "StatusTime": "2023-01-18T12:55:49Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "a019569f-4d29-4a11-a85f-af8e00d52ac2",
                "ReceiveTime": "2023-01-18T12:56:07Z",
                "StatusTime": "2023-01-18T12:56:02Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "cf64387e-aa7c-4eee-af22-77cd5736e6ee",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/receipts/a019569f-4d29-4a11-a85f-af8e00d52ac2/files/cf64387e-aa7c-4eee-af22-77cd5736e6ee/download"
                            }
                        ]
                    },
                    {
                        "Id": "36414da5-21f3-4233-b7fb-81b53b485d11",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "cf64387e-aa7c-4eee-af22-77cd5736e6ee",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/receipts/a019569f-4d29-4a11-a85f-af8e00d52ac2/files/36414da5-21f3-4233-b7fb-81b53b485d11/download"
                            }
                        ]
                    },
                    {
                        "Id": "caf04091-1136-495a-9458-c1738df4a688",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/receipts/a019569f-4d29-4a11-a85f-af8e00d52ac2/files/caf04091-1136-495a-9458-c1738df4a688/download"
                            }
                        ]
                    },
                    {
                        "Id": "512bd7d2-28fa-4d3b-8db3-ed1be8625714",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "caf04091-1136-495a-9458-c1738df4a688",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/receipts/a019569f-4d29-4a11-a85f-af8e00d52ac2/files/512bd7d2-28fa-4d3b-8db3-ed1be8625714/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "8ab7120a-ad89-4a71-a89c-af8e00d5306e",
                "ReceiveTime": "2023-01-18T12:56:11Z",
                "StatusTime": "2023-01-18T12:56:06Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "d33f5f53-d992-4a12-aca6-1b1ac86c4c51",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "73740896-b56e-4d96-8072-60d147889d33",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/receipts/8ab7120a-ad89-4a71-a89c-af8e00d5306e/files/d33f5f53-d992-4a12-aca6-1b1ac86c4c51/download"
                            }
                        ]
                    },
                    {
                        "Id": "73740896-b56e-4d96-8072-60d147889d33",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/receipts/8ab7120a-ad89-4a71-a89c-af8e00d5306e/files/73740896-b56e-4d96-8072-60d147889d33/download"
                            }
                        ]
                    },
                    {
                        "Id": "d6c153f8-500f-4cf4-b1cd-698eb240597d",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/receipts/8ab7120a-ad89-4a71-a89c-af8e00d5306e/files/d6c153f8-500f-4cf4-b1cd-698eb240597d/download"
                            }
                        ]
                    },
                    {
                        "Id": "83f916fe-ecc6-4347-b188-a3c6f333e0b7",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "d6c153f8-500f-4cf4-b1cd-698eb240597d",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/receipts/8ab7120a-ad89-4a71-a89c-af8e00d5306e/files/83f916fe-ecc6-4347-b188-a3c6f333e0b7/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "7fd2ce32-a620-4916-9d5e-af8e00d65e5a",
                "ReceiveTime": "2023-01-18T13:00:29Z",
                "StatusTime": "2023-01-18T13:00:13Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "97e9747e-755b-4ff8-b035-12a8573ee988",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 742,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/receipts/7fd2ce32-a620-4916-9d5e-af8e00d65e5a/files/97e9747e-755b-4ff8-b035-12a8573ee988/download"
                            }
                        ]
                    },
                    {
                        "Id": "961c313e-fcbd-4190-a4dc-1eb4a4e460be",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "9090f66d-bdf7-4431-b3f6-a0faa42463d5",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/receipts/7fd2ce32-a620-4916-9d5e-af8e00d65e5a/files/961c313e-fcbd-4190-a4dc-1eb4a4e460be/download"
                            }
                        ]
                    },
                    {
                        "Id": "fc5275fb-af17-4280-b290-55bba0ad684d",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "97e9747e-755b-4ff8-b035-12a8573ee988",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/receipts/7fd2ce32-a620-4916-9d5e-af8e00d65e5a/files/fc5275fb-af17-4280-b290-55bba0ad684d/download"
                            }
                        ]
                    },
                    {
                        "Id": "9090f66d-bdf7-4431-b3f6-a0faa42463d5",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/7ba6f090-beac-42bb-8633-af8e00d40b00/receipts/7fd2ce32-a620-4916-9d5e-af8e00d65e5a/files/9090f66d-bdf7-4431-b3f6-a0faa42463d5/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "faa867dc-3d72-4566-a5fa-af8e00edae1e",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК в организацию. Ответ ЦИК в организацию.",
        "Text": "",
        "CreationDate": "2023-01-18T14:25:22Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_56",
        "RegNumber": null,
        "TotalSize": 3916,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "b2612c41-b438-4c20-bf96-af8e00ed6d2d",
                "Name": "F1027700466640_180123_170113_K_0002_2000_K1027800000095.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 655,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/faa867dc-3d72-4566-a5fa-af8e00edae1e/files/b2612c41-b438-4c20-bf96-af8e00ed6d2d/download"
                    }
                ]
            },
            {
                "Id": "a49e3419-cb98-44c1-a4f0-cac96c05e929",
                "Name": "F1027700466640_180123_170113_K_0002_2000_K1027800000095.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "b2612c41-b438-4c20-bf96-af8e00ed6d2d",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/faa867dc-3d72-4566-a5fa-af8e00edae1e/files/a49e3419-cb98-44c1-a4f0-cac96c05e929/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "2dfe4c9a-0b03-45f1-8d3a-af8e010de40d",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-01-18T16:22:45Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 2253094,
        "Sender": null,
        "Files": [
            {
                "Id": "6051c550-e8b7-4912-b4cb-43055f181eaf",
                "Name": "KYC_20230118.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2249833,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/2dfe4c9a-0b03-45f1-8d3a-af8e010de40d/files/6051c550-e8b7-4912-b4cb-43055f181eaf/download"
                    }
                ]
            },
            {
                "Id": "897bae64-f438-4d01-a079-4864f1a9668e",
                "Name": "KYC_20230118.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "6051c550-e8b7-4912-b4cb-43055f181eaf",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/2dfe4c9a-0b03-45f1-8d3a-af8e010de40d/files/897bae64-f438-4d01-a079-4864f1a9668e/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "bce01b12-9222-4322-a508-af8e01289cf6",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-18T17:59:56Z",
        "UpdatedDate": "2023-01-18T18:02:41Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00572364",
        "TotalSize": 13356,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "020e7677-35b3-4463-b571-af8e01289cf2",
                "Name": "KYCCL_7831001422_3194_20230118_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9484,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/bce01b12-9222-4322-a508-af8e01289cf6/files/020e7677-35b3-4463-b571-af8e01289cf2/download"
                    }
                ]
            },
            {
                "Id": "aa7da675-b628-4faf-a27f-af8e01289cf4",
                "Name": "KYCCL_7831001422_3194_20230118_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "020e7677-35b3-4463-b571-af8e01289cf2",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/bce01b12-9222-4322-a508-af8e01289cf6/files/aa7da675-b628-4faf-a27f-af8e01289cf4/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "6541d63e-aaa2-4ed8-b46c-af8e01289e7d",
                "ReceiveTime": "2023-01-18T17:59:57Z",
                "StatusTime": "2023-01-18T17:59:57Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "48199e07-53ac-47c4-8a8a-af8e0128aa22",
                "ReceiveTime": "2023-01-18T18:00:07Z",
                "StatusTime": "2023-01-18T18:00:05Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "a060c677-caf7-447e-9760-1b496ed0daba",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bce01b12-9222-4322-a508-af8e01289cf6/receipts/48199e07-53ac-47c4-8a8a-af8e0128aa22/files/a060c677-caf7-447e-9760-1b496ed0daba/download"
                            }
                        ]
                    },
                    {
                        "Id": "1a9f4e49-2c04-4fda-b509-6cc329923f5a",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "b49e9c90-5e05-4ceb-99f3-81a97b6006f5",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bce01b12-9222-4322-a508-af8e01289cf6/receipts/48199e07-53ac-47c4-8a8a-af8e0128aa22/files/1a9f4e49-2c04-4fda-b509-6cc329923f5a/download"
                            }
                        ]
                    },
                    {
                        "Id": "b49e9c90-5e05-4ceb-99f3-81a97b6006f5",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bce01b12-9222-4322-a508-af8e01289cf6/receipts/48199e07-53ac-47c4-8a8a-af8e0128aa22/files/b49e9c90-5e05-4ceb-99f3-81a97b6006f5/download"
                            }
                        ]
                    },
                    {
                        "Id": "d3aa641d-4338-4dbd-b62b-d5acc12c63f9",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "a060c677-caf7-447e-9760-1b496ed0daba",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bce01b12-9222-4322-a508-af8e01289cf6/receipts/48199e07-53ac-47c4-8a8a-af8e0128aa22/files/d3aa641d-4338-4dbd-b62b-d5acc12c63f9/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "9e268b8d-d499-4598-822a-af8e0128b348",
                "ReceiveTime": "2023-01-18T18:00:15Z",
                "StatusTime": "2023-01-18T18:00:07Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "b4985046-c3f8-4d63-95de-040524f6c35a",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "e3690145-ebd3-4c0f-a928-cef2b49e67a2",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bce01b12-9222-4322-a508-af8e01289cf6/receipts/9e268b8d-d499-4598-822a-af8e0128b348/files/b4985046-c3f8-4d63-95de-040524f6c35a/download"
                            }
                        ]
                    },
                    {
                        "Id": "6fe1ae4f-a4f9-43c1-9073-63bb4df750b6",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bce01b12-9222-4322-a508-af8e01289cf6/receipts/9e268b8d-d499-4598-822a-af8e0128b348/files/6fe1ae4f-a4f9-43c1-9073-63bb4df750b6/download"
                            }
                        ]
                    },
                    {
                        "Id": "9d07bfb4-82d6-4b69-9ddb-7168e8a940ab",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "6fe1ae4f-a4f9-43c1-9073-63bb4df750b6",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bce01b12-9222-4322-a508-af8e01289cf6/receipts/9e268b8d-d499-4598-822a-af8e0128b348/files/9d07bfb4-82d6-4b69-9ddb-7168e8a940ab/download"
                            }
                        ]
                    },
                    {
                        "Id": "e3690145-ebd3-4c0f-a928-cef2b49e67a2",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bce01b12-9222-4322-a508-af8e01289cf6/receipts/9e268b8d-d499-4598-822a-af8e0128b348/files/e3690145-ebd3-4c0f-a928-cef2b49e67a2/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "d83a8bb1-1722-413a-aa07-af8e01295e9f",
                "ReceiveTime": "2023-01-18T18:02:41Z",
                "StatusTime": "2023-01-18T18:01:58Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "c8d587e8-df30-4fb9-8eff-70036bd9fc01",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "178a4f54-68ee-4b26-973b-b91dcaaf14d9",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bce01b12-9222-4322-a508-af8e01289cf6/receipts/d83a8bb1-1722-413a-aa07-af8e01295e9f/files/c8d587e8-df30-4fb9-8eff-70036bd9fc01/download"
                            }
                        ]
                    },
                    {
                        "Id": "178a4f54-68ee-4b26-973b-b91dcaaf14d9",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/bce01b12-9222-4322-a508-af8e01289cf6/receipts/d83a8bb1-1722-413a-aa07-af8e01295e9f/files/178a4f54-68ee-4b26-973b-b91dcaaf14d9/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "724bbfa5-8260-4529-b8b5-af8f00e08e75",
        "CorrelationId": "1f6158a2-a7a1-4e14-aace-af7a00f65145",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "в дополнение к нашему исх. 1-3 от 09.01.2023",
        "CreationDate": "2023-01-19T13:37:35Z",
        "UpdatedDate": "2023-01-19T13:53:56Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "25312",
        "TotalSize": 843994,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "839d0144-76e3-4d0a-976c-af8f00e08ec7",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2003,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/files/839d0144-76e3-4d0a-976c-af8f00e08ec7/download"
                    }
                ]
            },
            {
                "Id": "aaa648e1-fc35-4b2b-913c-af8f00e08ef3",
                "Name": "2023-01-19-1-3-7.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 829909,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/files/aaa648e1-fc35-4b2b-913c-af8f00e08ef3/download"
                    }
                ]
            },
            {
                "Id": "ec7050d6-900c-4ac0-9f3b-af8f00e0dfd7",
                "Name": "2023-01-19-1-3-7.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "aaa648e1-fc35-4b2b-913c-af8f00e08ef3",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/files/ec7050d6-900c-4ac0-9f3b-af8f00e0dfd7/download"
                    }
                ]
            },
            {
                "Id": "2e182d59-87b4-40ac-83be-af8f00e12d1e",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "839d0144-76e3-4d0a-976c-af8f00e08ec7",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/files/2e182d59-87b4-40ac-83be-af8f00e12d1e/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "ac3e2c42-297d-4c92-8562-af8f00e12e09",
                "ReceiveTime": "2023-01-19T13:39:51Z",
                "StatusTime": "2023-01-19T13:39:51Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "563248be-0a55-4b59-89cd-af8f00e1441c",
                "ReceiveTime": "2023-01-19T13:40:10Z",
                "StatusTime": "2023-01-19T13:40:06Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "d102bc8c-5042-4a28-8720-179818d05c82",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/receipts/563248be-0a55-4b59-89cd-af8f00e1441c/files/d102bc8c-5042-4a28-8720-179818d05c82/download"
                            }
                        ]
                    },
                    {
                        "Id": "defbc61e-6a3e-4352-95a7-2ffe6e42d2ec",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/receipts/563248be-0a55-4b59-89cd-af8f00e1441c/files/defbc61e-6a3e-4352-95a7-2ffe6e42d2ec/download"
                            }
                        ]
                    },
                    {
                        "Id": "0a465262-7073-425d-9396-b8a9fb05dc28",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "d102bc8c-5042-4a28-8720-179818d05c82",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/receipts/563248be-0a55-4b59-89cd-af8f00e1441c/files/0a465262-7073-425d-9396-b8a9fb05dc28/download"
                            }
                        ]
                    },
                    {
                        "Id": "6c288910-f2d5-45ef-abac-c6bdc4c757eb",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "defbc61e-6a3e-4352-95a7-2ffe6e42d2ec",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/receipts/563248be-0a55-4b59-89cd-af8f00e1441c/files/6c288910-f2d5-45ef-abac-c6bdc4c757eb/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "f3d71848-ad92-44c5-aba8-af8f00e14e73",
                "ReceiveTime": "2023-01-19T13:40:18Z",
                "StatusTime": "2023-01-19T13:40:09Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "fcbe2750-9a03-43bd-973e-105a6cb9af85",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/receipts/f3d71848-ad92-44c5-aba8-af8f00e14e73/files/fcbe2750-9a03-43bd-973e-105a6cb9af85/download"
                            }
                        ]
                    },
                    {
                        "Id": "d3376200-a6b7-4df7-967f-6ceaea9be529",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "5816173f-d28b-4083-bf7e-8cd08fcf3428",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/receipts/f3d71848-ad92-44c5-aba8-af8f00e14e73/files/d3376200-a6b7-4df7-967f-6ceaea9be529/download"
                            }
                        ]
                    },
                    {
                        "Id": "6030b884-cc1e-4029-8f16-7cd348a35ce8",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "fcbe2750-9a03-43bd-973e-105a6cb9af85",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/receipts/f3d71848-ad92-44c5-aba8-af8f00e14e73/files/6030b884-cc1e-4029-8f16-7cd348a35ce8/download"
                            }
                        ]
                    },
                    {
                        "Id": "5816173f-d28b-4083-bf7e-8cd08fcf3428",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/receipts/f3d71848-ad92-44c5-aba8-af8f00e14e73/files/5816173f-d28b-4083-bf7e-8cd08fcf3428/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "7fff3145-98e4-405f-aae0-af8f00e50d15",
                "ReceiveTime": "2023-01-19T13:53:56Z",
                "StatusTime": "2023-01-19T13:53:18Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "58cb2eaa-6446-4eeb-beba-1d5cc5c2afe6",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 742,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/receipts/7fff3145-98e4-405f-aae0-af8f00e50d15/files/58cb2eaa-6446-4eeb-beba-1d5cc5c2afe6/download"
                            }
                        ]
                    },
                    {
                        "Id": "c22927a0-c493-4b7a-8d76-a19fba613c72",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/receipts/7fff3145-98e4-405f-aae0-af8f00e50d15/files/c22927a0-c493-4b7a-8d76-a19fba613c72/download"
                            }
                        ]
                    },
                    {
                        "Id": "398e2895-35eb-46c8-9e91-d55a276f96b9",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "c22927a0-c493-4b7a-8d76-a19fba613c72",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/receipts/7fff3145-98e4-405f-aae0-af8f00e50d15/files/398e2895-35eb-46c8-9e91-d55a276f96b9/download"
                            }
                        ]
                    },
                    {
                        "Id": "986bd145-35e8-4547-9115-fe54dc45a4dc",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "58cb2eaa-6446-4eeb-beba-1d5cc5c2afe6",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/724bbfa5-8260-4529-b8b5-af8f00e08e75/receipts/7fff3145-98e4-405f-aae0-af8f00e50d15/files/986bd145-35e8-4547-9115-fe54dc45a4dc/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "3e6740e1-5d94-4a73-981b-af8f00e22be5",
        "CorrelationId": null,
        "GroupId": "93e014f7-8b59-4d7f-bb3f-2afac113bb99",
        "Type": "inbox",
        "Title": "№ Т2-24-2/1453 от 19/01/2023 Переписка с внешними организациями",
        "Text": "о памятной монете",
        "CreationDate": "2023-01-19T13:43:29Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "Т2-24-2/1453",
        "TotalSize": 742834,
        "Sender": null,
        "Files": [
            {
                "Id": "84c45c4d-8610-4ee5-aa7f-13392055cc58",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "113702687.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 383110,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/3e6740e1-5d94-4a73-981b-af8f00e22be5/files/84c45c4d-8610-4ee5-aa7f-13392055cc58/download"
                    }
                ]
            },
            {
                "Id": "372e4062-b19d-4495-a33c-2cff5af011ae",
                "Name": "Список рассылки.xlsx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "705b160d-5287-4421-a5f0-6fbb3d0d7aab",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/3e6740e1-5d94-4a73-981b-af8f00e22be5/files/372e4062-b19d-4495-a33c-2cff5af011ae/download"
                    }
                ]
            },
            {
                "Id": "3df1231c-64dc-4140-9956-59ae8ccbb2cb",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "84c45c4d-8610-4ee5-aa7f-13392055cc58",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/3e6740e1-5d94-4a73-981b-af8f00e22be5/files/3df1231c-64dc-4140-9956-59ae8ccbb2cb/download"
                    }
                ]
            },
            {
                "Id": "705b160d-5287-4421-a5f0-6fbb3d0d7aab",
                "Name": "Список рассылки.xlsx",
                "Description": "113720593.xlsx",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 13693,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/3e6740e1-5d94-4a73-981b-af8f00e22be5/files/705b160d-5287-4421-a5f0-6fbb3d0d7aab/download"
                    }
                ]
            },
            {
                "Id": "012932a1-06bd-4928-af4d-94b62c0ec468",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 77019,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/3e6740e1-5d94-4a73-981b-af8f00e22be5/files/012932a1-06bd-4928-af4d-94b62c0ec468/download"
                    }
                ]
            },
            {
                "Id": "01345f75-30d5-4b2e-9af5-ba936188b192",
                "Name": "ЭД_.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "4961a7d8-b86c-49d0-8a8e-c7a3963951ba",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/3e6740e1-5d94-4a73-981b-af8f00e22be5/files/01345f75-30d5-4b2e-9af5-ba936188b192/download"
                    }
                ]
            },
            {
                "Id": "4961a7d8-b86c-49d0-8a8e-c7a3963951ba",
                "Name": "ЭД_.pdf",
                "Description": "113702591.pdf",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 259229,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/3e6740e1-5d94-4a73-981b-af8f00e22be5/files/4961a7d8-b86c-49d0-8a8e-c7a3963951ba/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "9789f63d-72e8-4149-8f1c-af8f00e460a2",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Обращение (запрос)  в Банк России",
        "Text": "в дополнение к нашему 1-3-1 от 09.01.2023",
        "CreationDate": "2023-01-19T13:51:29Z",
        "UpdatedDate": "2023-01-19T14:01:33Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2-5281",
        "TotalSize": 844357,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "a7325b3c-1f49-44bb-973b-af8f00e4608f",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2366,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/files/a7325b3c-1f49-44bb-973b-af8f00e4608f/download"
                    }
                ]
            },
            {
                "Id": "71ce6f9d-cd29-4cfb-9daf-af8f00e460b0",
                "Name": "2023-01-19-1-3-7-1.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 829909,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/files/71ce6f9d-cd29-4cfb-9daf-af8f00e460b0/download"
                    }
                ]
            },
            {
                "Id": "51d16b31-d3a9-4377-90e1-af8f00e4af30",
                "Name": "2023-01-19-1-3-7-1.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "71ce6f9d-cd29-4cfb-9daf-af8f00e460b0",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/files/51d16b31-d3a9-4377-90e1-af8f00e4af30/download"
                    }
                ]
            },
            {
                "Id": "7ba288dd-2542-4641-8c8f-af8f00e4fa0f",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "a7325b3c-1f49-44bb-973b-af8f00e4608f",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/files/7ba288dd-2542-4641-8c8f-af8f00e4fa0f/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "e8cb603d-466b-4376-8e3e-af8f00e4fb7a",
                "ReceiveTime": "2023-01-19T13:53:41Z",
                "StatusTime": "2023-01-19T13:53:41Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "013f3d79-af66-440c-abfe-af8f00e510b4",
                "ReceiveTime": "2023-01-19T13:54:00Z",
                "StatusTime": "2023-01-19T13:53:56Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "cdb3fc3d-7819-40a5-aa97-005896a7de4b",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/receipts/013f3d79-af66-440c-abfe-af8f00e510b4/files/cdb3fc3d-7819-40a5-aa97-005896a7de4b/download"
                            }
                        ]
                    },
                    {
                        "Id": "23e8743b-42d6-4ca1-8e2e-7c9502d89d53",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "3c28b7ce-ca21-417a-af94-d1e7e7416234",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/receipts/013f3d79-af66-440c-abfe-af8f00e510b4/files/23e8743b-42d6-4ca1-8e2e-7c9502d89d53/download"
                            }
                        ]
                    },
                    {
                        "Id": "857c3e1f-5ab8-42fe-8c09-a750c9a00289",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "cdb3fc3d-7819-40a5-aa97-005896a7de4b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/receipts/013f3d79-af66-440c-abfe-af8f00e510b4/files/857c3e1f-5ab8-42fe-8c09-a750c9a00289/download"
                            }
                        ]
                    },
                    {
                        "Id": "3c28b7ce-ca21-417a-af94-d1e7e7416234",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/receipts/013f3d79-af66-440c-abfe-af8f00e510b4/files/3c28b7ce-ca21-417a-af94-d1e7e7416234/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "2e6513df-b354-4e04-8ab6-af8f00e51763",
                "ReceiveTime": "2023-01-19T13:54:05Z",
                "StatusTime": "2023-01-19T13:53:59Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "b34c43a5-cc9a-4fa0-8200-1fe9d2f9cb78",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/receipts/2e6513df-b354-4e04-8ab6-af8f00e51763/files/b34c43a5-cc9a-4fa0-8200-1fe9d2f9cb78/download"
                            }
                        ]
                    },
                    {
                        "Id": "3691d2b1-d128-4ba5-b475-7f6a41c3ec45",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/receipts/2e6513df-b354-4e04-8ab6-af8f00e51763/files/3691d2b1-d128-4ba5-b475-7f6a41c3ec45/download"
                            }
                        ]
                    },
                    {
                        "Id": "f6650d51-cea8-4edf-ae77-8879b99f7dff",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "3691d2b1-d128-4ba5-b475-7f6a41c3ec45",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/receipts/2e6513df-b354-4e04-8ab6-af8f00e51763/files/f6650d51-cea8-4edf-ae77-8879b99f7dff/download"
                            }
                        ]
                    },
                    {
                        "Id": "920618e7-20a9-4842-8749-d17c7812e598",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "b34c43a5-cc9a-4fa0-8200-1fe9d2f9cb78",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/receipts/2e6513df-b354-4e04-8ab6-af8f00e51763/files/920618e7-20a9-4842-8749-d17c7812e598/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "63744538-41bb-4763-a937-af8f00e72384",
                "ReceiveTime": "2023-01-19T14:01:33Z",
                "StatusTime": "2023-01-19T13:58:05Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "b41339fc-cab8-410c-b0b7-2ba203b0697d",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 780,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/receipts/63744538-41bb-4763-a937-af8f00e72384/files/b41339fc-cab8-410c-b0b7-2ba203b0697d/download"
                            }
                        ]
                    },
                    {
                        "Id": "f0780629-7338-4974-b86d-a32795a0cb7c",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 355,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/receipts/63744538-41bb-4763-a937-af8f00e72384/files/f0780629-7338-4974-b86d-a32795a0cb7c/download"
                            }
                        ]
                    },
                    {
                        "Id": "51e87cee-e7ac-4e46-9bf5-b07abc84287a",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "b41339fc-cab8-410c-b0b7-2ba203b0697d",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/receipts/63744538-41bb-4763-a937-af8f00e72384/files/51e87cee-e7ac-4e46-9bf5-b07abc84287a/download"
                            }
                        ]
                    },
                    {
                        "Id": "5e3085ae-dd42-4124-ab8d-b75326f1c956",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f0780629-7338-4974-b86d-a32795a0cb7c",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9789f63d-72e8-4149-8f1c-af8f00e460a2/receipts/63744538-41bb-4763-a937-af8f00e72384/files/5e3085ae-dd42-4124-ab8d-b75326f1c956/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "1bb5e5d0-2173-4cc1-9798-af8f00f211e6",
        "CorrelationId": "76acafcd-5dde-4db2-885c-af7a00ea874c",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "Сообщение об ознакомлении, возражения",
        "CreationDate": "2023-01-19T14:41:20Z",
        "UpdatedDate": "2023-01-19T14:51:15Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2-5327",
        "TotalSize": 3741618,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "61777ac3-7b4d-454f-8bf9-af8f00f2122f",
                "Name": "2023-01-19-9-1.pdf.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 91427,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/files/61777ac3-7b4d-454f-8bf9-af8f00f2122f/download"
                    }
                ]
            },
            {
                "Id": "2af0db55-dcd7-4120-a711-af8f00f21230",
                "Name": "2023-01-19-9-1.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 3630064,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/files/2af0db55-dcd7-4120-a711-af8f00f21230/download"
                    }
                ]
            },
            {
                "Id": "aeb888da-feaf-4b1e-b281-af8f00f2123e",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2004,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/files/aeb888da-feaf-4b1e-b281-af8f00f2123e/download"
                    }
                ]
            },
            {
                "Id": "72977420-fe7b-4bcf-a379-af8f00f262ff",
                "Name": "2023-01-19-9-1.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "61777ac3-7b4d-454f-8bf9-af8f00f2122f",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/files/72977420-fe7b-4bcf-a379-af8f00f262ff/download"
                    }
                ]
            },
            {
                "Id": "c006df99-c5e1-4b24-9cba-af8f00f2b246",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "aeb888da-feaf-4b1e-b281-af8f00f2123e",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/files/c006df99-c5e1-4b24-9cba-af8f00f2b246/download"
                    }
                ]
            },
            {
                "Id": "456712de-0166-466d-bd7d-af8f00f30226",
                "Name": "2023-01-19-9-1.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "2af0db55-dcd7-4120-a711-af8f00f21230",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/files/456712de-0166-466d-bd7d-af8f00f30226/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "8e626fd8-9dfc-465e-984e-af8f00f303bd",
                "ReceiveTime": "2023-01-19T14:44:47Z",
                "StatusTime": "2023-01-19T14:44:47Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "db5a6a61-5f51-425e-b3d1-af8f00f31489",
                "ReceiveTime": "2023-01-19T14:45:01Z",
                "StatusTime": "2023-01-19T14:44:59Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "75831a67-7601-4863-9fd6-6e7738cba6b4",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/receipts/db5a6a61-5f51-425e-b3d1-af8f00f31489/files/75831a67-7601-4863-9fd6-6e7738cba6b4/download"
                            }
                        ]
                    },
                    {
                        "Id": "37f93358-e55c-4449-b804-c54ccb2b96e9",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "75831a67-7601-4863-9fd6-6e7738cba6b4",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/receipts/db5a6a61-5f51-425e-b3d1-af8f00f31489/files/37f93358-e55c-4449-b804-c54ccb2b96e9/download"
                            }
                        ]
                    },
                    {
                        "Id": "152046de-4857-455e-a7fb-d2d664153854",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "7c6534ec-055c-4387-ad7c-d7d1e8f7ce45",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/receipts/db5a6a61-5f51-425e-b3d1-af8f00f31489/files/152046de-4857-455e-a7fb-d2d664153854/download"
                            }
                        ]
                    },
                    {
                        "Id": "7c6534ec-055c-4387-ad7c-d7d1e8f7ce45",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/receipts/db5a6a61-5f51-425e-b3d1-af8f00f31489/files/7c6534ec-055c-4387-ad7c-d7d1e8f7ce45/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "1e72b483-40a1-4455-aa9a-af8f00f31ee2",
                "ReceiveTime": "2023-01-19T14:45:10Z",
                "StatusTime": "2023-01-19T14:45:01Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "6be68429-f0d6-4495-aeac-02f383cdc297",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "a8cbd756-d8ba-473a-9c3f-8b0afaba9ad9",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/receipts/1e72b483-40a1-4455-aa9a-af8f00f31ee2/files/6be68429-f0d6-4495-aeac-02f383cdc297/download"
                            }
                        ]
                    },
                    {
                        "Id": "a8cbd756-d8ba-473a-9c3f-8b0afaba9ad9",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/receipts/1e72b483-40a1-4455-aa9a-af8f00f31ee2/files/a8cbd756-d8ba-473a-9c3f-8b0afaba9ad9/download"
                            }
                        ]
                    },
                    {
                        "Id": "9a19bb09-9075-47c2-9b2d-c88cf4cbd74e",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/receipts/1e72b483-40a1-4455-aa9a-af8f00f31ee2/files/9a19bb09-9075-47c2-9b2d-c88cf4cbd74e/download"
                            }
                        ]
                    },
                    {
                        "Id": "96ddebb6-f17f-4961-9835-e6672eace470",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "9a19bb09-9075-47c2-9b2d-c88cf4cbd74e",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/receipts/1e72b483-40a1-4455-aa9a-af8f00f31ee2/files/96ddebb6-f17f-4961-9835-e6672eace470/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "c506227c-a58e-4295-ae44-af8f00f4cab9",
                "ReceiveTime": "2023-01-19T14:51:15Z",
                "StatusTime": "2023-01-19T14:51:04Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "f50cb7c9-f1c5-4366-a468-1543210dbc2b",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 780,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/receipts/c506227c-a58e-4295-ae44-af8f00f4cab9/files/f50cb7c9-f1c5-4366-a468-1543210dbc2b/download"
                            }
                        ]
                    },
                    {
                        "Id": "de450d6a-09fa-4c4f-94c9-f5c3bfe93baa",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "60625ec9-439c-43ab-b217-ffbe77b3127d",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/receipts/c506227c-a58e-4295-ae44-af8f00f4cab9/files/de450d6a-09fa-4c4f-94c9-f5c3bfe93baa/download"
                            }
                        ]
                    },
                    {
                        "Id": "baa24ef1-049f-4b71-a644-fdf974bc078a",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f50cb7c9-f1c5-4366-a468-1543210dbc2b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/receipts/c506227c-a58e-4295-ae44-af8f00f4cab9/files/baa24ef1-049f-4b71-a644-fdf974bc078a/download"
                            }
                        ]
                    },
                    {
                        "Id": "60625ec9-439c-43ab-b217-ffbe77b3127d",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 355,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1bb5e5d0-2173-4cc1-9798-af8f00f211e6/receipts/c506227c-a58e-4295-ae44-af8f00f4cab9/files/60625ec9-439c-43ab-b217-ffbe77b3127d/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "6c89c121-1de1-47e5-9e43-af8f0111dc76",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-01-19T16:37:13Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 2265123,
        "Sender": null,
        "Files": [
            {
                "Id": "004d7ab7-0fe3-48a0-b187-1d19c38f4d51",
                "Name": "KYC_20230119.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "cc8efcb8-69cc-4cfa-a022-e99d1524decf",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6c89c121-1de1-47e5-9e43-af8f0111dc76/files/004d7ab7-0fe3-48a0-b187-1d19c38f4d51/download"
                    }
                ]
            },
            {
                "Id": "cc8efcb8-69cc-4cfa-a022-e99d1524decf",
                "Name": "KYC_20230119.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2261862,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6c89c121-1de1-47e5-9e43-af8f0111dc76/files/cc8efcb8-69cc-4cfa-a022-e99d1524decf/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "9a75b169-305d-4c58-82f3-af8f01289be8",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-19T17:59:55Z",
        "UpdatedDate": "2023-01-19T18:03:46Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00573517",
        "TotalSize": 13354,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "6325c062-ca0b-4ff2-82ec-af8f01289be4",
                "Name": "KYCCL_7831001422_3194_20230119_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9482,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9a75b169-305d-4c58-82f3-af8f01289be8/files/6325c062-ca0b-4ff2-82ec-af8f01289be4/download"
                    }
                ]
            },
            {
                "Id": "6e0467f0-243b-4f75-8f33-af8f01289be6",
                "Name": "KYCCL_7831001422_3194_20230119_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "6325c062-ca0b-4ff2-82ec-af8f01289be4",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9a75b169-305d-4c58-82f3-af8f01289be8/files/6e0467f0-243b-4f75-8f33-af8f01289be6/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "3446c051-a6a7-4df8-a702-af8f01289d28",
                "ReceiveTime": "2023-01-19T17:59:56Z",
                "StatusTime": "2023-01-19T17:59:56Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "a45b0454-154d-45e0-a266-af8f0128af06",
                "ReceiveTime": "2023-01-19T18:00:11Z",
                "StatusTime": "2023-01-19T18:00:07Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "84a4626b-1661-45dd-ae58-13c4ca2eaf15",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "3338b213-ec07-48a9-8454-c0bcbf90528a",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9a75b169-305d-4c58-82f3-af8f01289be8/receipts/a45b0454-154d-45e0-a266-af8f0128af06/files/84a4626b-1661-45dd-ae58-13c4ca2eaf15/download"
                            }
                        ]
                    },
                    {
                        "Id": "cf6db67d-435d-49ea-afa5-4c4b819fc3ee",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9a75b169-305d-4c58-82f3-af8f01289be8/receipts/a45b0454-154d-45e0-a266-af8f0128af06/files/cf6db67d-435d-49ea-afa5-4c4b819fc3ee/download"
                            }
                        ]
                    },
                    {
                        "Id": "3338b213-ec07-48a9-8454-c0bcbf90528a",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9a75b169-305d-4c58-82f3-af8f01289be8/receipts/a45b0454-154d-45e0-a266-af8f0128af06/files/3338b213-ec07-48a9-8454-c0bcbf90528a/download"
                            }
                        ]
                    },
                    {
                        "Id": "471eff77-bc57-4aa7-a7e6-e3567fab207c",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "cf6db67d-435d-49ea-afa5-4c4b819fc3ee",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9a75b169-305d-4c58-82f3-af8f01289be8/receipts/a45b0454-154d-45e0-a266-af8f0128af06/files/471eff77-bc57-4aa7-a7e6-e3567fab207c/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "32f6eb1f-475a-4a61-b576-af8f0128b78c",
                "ReceiveTime": "2023-01-19T18:00:18Z",
                "StatusTime": "2023-01-19T18:00:11Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "6cc4e3c5-8f0a-4868-9d8f-04d03b2b584e",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9a75b169-305d-4c58-82f3-af8f01289be8/receipts/32f6eb1f-475a-4a61-b576-af8f0128b78c/files/6cc4e3c5-8f0a-4868-9d8f-04d03b2b584e/download"
                            }
                        ]
                    },
                    {
                        "Id": "2e7e727d-7aeb-46ea-9804-079262c7c3b3",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9a75b169-305d-4c58-82f3-af8f01289be8/receipts/32f6eb1f-475a-4a61-b576-af8f0128b78c/files/2e7e727d-7aeb-46ea-9804-079262c7c3b3/download"
                            }
                        ]
                    },
                    {
                        "Id": "39f6cf93-3cb5-41da-8fb8-1b5765b18312",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "6cc4e3c5-8f0a-4868-9d8f-04d03b2b584e",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9a75b169-305d-4c58-82f3-af8f01289be8/receipts/32f6eb1f-475a-4a61-b576-af8f0128b78c/files/39f6cf93-3cb5-41da-8fb8-1b5765b18312/download"
                            }
                        ]
                    },
                    {
                        "Id": "e43df128-ffde-47c9-862f-9446c1cc233c",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "2e7e727d-7aeb-46ea-9804-079262c7c3b3",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9a75b169-305d-4c58-82f3-af8f01289be8/receipts/32f6eb1f-475a-4a61-b576-af8f0128b78c/files/e43df128-ffde-47c9-862f-9446c1cc233c/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "83555bfb-4fda-4a75-85b2-af8f0129aa79",
                "ReceiveTime": "2023-01-19T18:03:46Z",
                "StatusTime": "2023-01-19T18:02:58Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "2a7c46f4-57af-44e6-867c-058bed68d21e",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9a75b169-305d-4c58-82f3-af8f01289be8/receipts/83555bfb-4fda-4a75-85b2-af8f0129aa79/files/2a7c46f4-57af-44e6-867c-058bed68d21e/download"
                            }
                        ]
                    },
                    {
                        "Id": "7ce79c47-a3cf-455b-aa9e-5738c6d33175",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "2a7c46f4-57af-44e6-867c-058bed68d21e",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/9a75b169-305d-4c58-82f3-af8f01289be8/receipts/83555bfb-4fda-4a75-85b2-af8f0129aa79/files/7ce79c47-a3cf-455b-aa9e-5738c6d33175/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "4cf319d4-ddae-4dd6-b384-af9000dff93c",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Обращение (запрос)  в Банк России",
        "Text": "Сопроводительное письмо к форме 7504 на 01.01.2023",
        "CreationDate": "2023-01-20T13:35:27Z",
        "UpdatedDate": "2023-01-20T13:48:15Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2-5823",
        "TotalSize": 128957,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "5a5ebf0a-939e-48a1-963f-af9000dff9bd",
                "Name": "form.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 1379,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/files/5a5ebf0a-939e-48a1-963f-af9000dff9bd/download"
                    }
                ]
            },
            {
                "Id": "d31cc625-910f-4ec1-aac2-af9000dffa05",
                "Name": "2023-01-20-10-1 сопровод. письмо к ф.7504 на 01012023.pdf",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 115496,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/files/d31cc625-910f-4ec1-aac2-af9000dffa05/download"
                    }
                ]
            },
            {
                "Id": "835f4a4c-39c9-48c5-9ef9-af9000e04a03",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "5a5ebf0a-939e-48a1-963f-af9000dff9bd",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/files/835f4a4c-39c9-48c5-9ef9-af9000e04a03/download"
                    }
                ]
            },
            {
                "Id": "c1ce1ab1-dd45-451e-be59-af9000e094f1",
                "Name": "2023-01-20-10-1 сопровод. письмо к ф.7504 на 01012023.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "d31cc625-910f-4ec1-aac2-af9000dffa05",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/files/c1ce1ab1-dd45-451e-be59-af9000e094f1/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "785da2e0-cd82-488e-8281-af9000e097ab",
                "ReceiveTime": "2023-01-20T13:37:42Z",
                "StatusTime": "2023-01-20T13:37:42Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "f2d0f7cd-a52e-4826-a8f3-af9000e0a8f8",
                "ReceiveTime": "2023-01-20T13:37:57Z",
                "StatusTime": "2023-01-20T13:37:53Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "bdbe61da-b2dd-46d4-a87a-009756cda7fe",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/receipts/f2d0f7cd-a52e-4826-a8f3-af9000e0a8f8/files/bdbe61da-b2dd-46d4-a87a-009756cda7fe/download"
                            }
                        ]
                    },
                    {
                        "Id": "82597a33-3049-4722-b46d-1675da1fde94",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/receipts/f2d0f7cd-a52e-4826-a8f3-af9000e0a8f8/files/82597a33-3049-4722-b46d-1675da1fde94/download"
                            }
                        ]
                    },
                    {
                        "Id": "95eec48e-7919-43ce-909f-9378098c66b6",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "bdbe61da-b2dd-46d4-a87a-009756cda7fe",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/receipts/f2d0f7cd-a52e-4826-a8f3-af9000e0a8f8/files/95eec48e-7919-43ce-909f-9378098c66b6/download"
                            }
                        ]
                    },
                    {
                        "Id": "d1fc1428-74e4-49e9-b471-f0841d61f55c",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "82597a33-3049-4722-b46d-1675da1fde94",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/receipts/f2d0f7cd-a52e-4826-a8f3-af9000e0a8f8/files/d1fc1428-74e4-49e9-b471-f0841d61f55c/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "f678e371-7301-43e5-8bff-af9000e0af51",
                "ReceiveTime": "2023-01-20T13:38:03Z",
                "StatusTime": "2023-01-20T13:37:56Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "449c4318-adde-4d3d-be1a-0ea223e948c1",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/receipts/f678e371-7301-43e5-8bff-af9000e0af51/files/449c4318-adde-4d3d-be1a-0ea223e948c1/download"
                            }
                        ]
                    },
                    {
                        "Id": "a97507e3-203d-48ec-868c-21a65f499272",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "de121598-7552-4579-80a1-42de2b8e0dd7",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/receipts/f678e371-7301-43e5-8bff-af9000e0af51/files/a97507e3-203d-48ec-868c-21a65f499272/download"
                            }
                        ]
                    },
                    {
                        "Id": "de121598-7552-4579-80a1-42de2b8e0dd7",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/receipts/f678e371-7301-43e5-8bff-af9000e0af51/files/de121598-7552-4579-80a1-42de2b8e0dd7/download"
                            }
                        ]
                    },
                    {
                        "Id": "60fa58e4-03ad-4bcc-b76a-4fa51d0a69fe",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "449c4318-adde-4d3d-be1a-0ea223e948c1",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/receipts/f678e371-7301-43e5-8bff-af9000e0af51/files/60fa58e4-03ad-4bcc-b76a-4fa51d0a69fe/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "65c427b3-9c6d-4221-a85b-af9000e37ccb",
                "ReceiveTime": "2023-01-20T13:48:15Z",
                "StatusTime": "2023-01-20T13:48:07Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "4e9473bd-3e64-48c1-8bec-5fddd46a5f1c",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 780,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/receipts/65c427b3-9c6d-4221-a85b-af9000e37ccb/files/4e9473bd-3e64-48c1-8bec-5fddd46a5f1c/download"
                            }
                        ]
                    },
                    {
                        "Id": "bdadc779-f355-48f8-afbd-8d98b0f0f6d3",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "4b8687b5-32ee-49d5-997c-8edf94808c92",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/receipts/65c427b3-9c6d-4221-a85b-af9000e37ccb/files/bdadc779-f355-48f8-afbd-8d98b0f0f6d3/download"
                            }
                        ]
                    },
                    {
                        "Id": "4b8687b5-32ee-49d5-997c-8edf94808c92",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 355,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/receipts/65c427b3-9c6d-4221-a85b-af9000e37ccb/files/4b8687b5-32ee-49d5-997c-8edf94808c92/download"
                            }
                        ]
                    },
                    {
                        "Id": "67b56ee9-7996-4ae0-985f-a70601578583",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "4e9473bd-3e64-48c1-8bec-5fddd46a5f1c",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/4cf319d4-ddae-4dd6-b384-af9000dff93c/receipts/65c427b3-9c6d-4221-a85b-af9000e37ccb/files/67b56ee9-7996-4ae0-985f-a70601578583/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "071c0c98-ce8d-480d-b4f4-af9000e21ba2",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Обращение (запрос)  в Банк России",
        "Text": "О предоставлении копий исполнительных листов",
        "CreationDate": "2023-01-20T13:43:14Z",
        "UpdatedDate": "2023-01-20T13:50:19Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "2-5825",
        "TotalSize": 110360,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "df69883d-6781-4f49-ba35-af9000e21c08",
                "Name": "2023-01-20-9-2 О предоставлении копий документов.pdf",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 96960,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/files/df69883d-6781-4f49-ba35-af9000e21c08/download"
                    }
                ]
            },
            {
                "Id": "c8677782-0e95-40b9-97e0-af9000e21c15",
                "Name": "form.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 1318,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/files/c8677782-0e95-40b9-97e0-af9000e21c15/download"
                    }
                ]
            },
            {
                "Id": "6fb6fedb-937a-4300-bc37-af9000e26876",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "c8677782-0e95-40b9-97e0-af9000e21c15",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/files/6fb6fedb-937a-4300-bc37-af9000e26876/download"
                    }
                ]
            },
            {
                "Id": "e2e6e241-b204-4257-b3d9-af9000e2b375",
                "Name": "2023-01-20-9-2 О предоставлении копий документов.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "df69883d-6781-4f49-ba35-af9000e21c08",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/files/e2e6e241-b204-4257-b3d9-af9000e2b375/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "cc4691e0-3c7a-4700-b9f5-af9000e2b4f1",
                "ReceiveTime": "2023-01-20T13:45:24Z",
                "StatusTime": "2023-01-20T13:45:24Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "43278b1a-1f6b-4331-ad46-af9000e2c7fe",
                "ReceiveTime": "2023-01-20T13:45:41Z",
                "StatusTime": "2023-01-20T13:45:36Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "f074b2a7-3354-4271-8ca4-076680400b3f",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "db628f7d-6461-491d-a4ae-d656e4822bd9",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/receipts/43278b1a-1f6b-4331-ad46-af9000e2c7fe/files/f074b2a7-3354-4271-8ca4-076680400b3f/download"
                            }
                        ]
                    },
                    {
                        "Id": "6dd2e8de-65ab-4aff-9a5c-4ee362b98c89",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "ce4b92b7-cc24-40e5-9c8a-e735a8a0f353",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/receipts/43278b1a-1f6b-4331-ad46-af9000e2c7fe/files/6dd2e8de-65ab-4aff-9a5c-4ee362b98c89/download"
                            }
                        ]
                    },
                    {
                        "Id": "db628f7d-6461-491d-a4ae-d656e4822bd9",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/receipts/43278b1a-1f6b-4331-ad46-af9000e2c7fe/files/db628f7d-6461-491d-a4ae-d656e4822bd9/download"
                            }
                        ]
                    },
                    {
                        "Id": "ce4b92b7-cc24-40e5-9c8a-e735a8a0f353",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/receipts/43278b1a-1f6b-4331-ad46-af9000e2c7fe/files/ce4b92b7-cc24-40e5-9c8a-e735a8a0f353/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "d1c174ea-5b5b-45b0-a01b-af9000e2d352",
                "ReceiveTime": "2023-01-20T13:45:50Z",
                "StatusTime": "2023-01-20T13:45:40Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "d21e1e0e-20b2-482a-b3dc-6713cd33053e",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/receipts/d1c174ea-5b5b-45b0-a01b-af9000e2d352/files/d21e1e0e-20b2-482a-b3dc-6713cd33053e/download"
                            }
                        ]
                    },
                    {
                        "Id": "9501d46f-d391-4d57-bf4b-6c02254143e3",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/receipts/d1c174ea-5b5b-45b0-a01b-af9000e2d352/files/9501d46f-d391-4d57-bf4b-6c02254143e3/download"
                            }
                        ]
                    },
                    {
                        "Id": "59ce51f6-3f2e-4867-be84-861c3ffe02ef",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "d21e1e0e-20b2-482a-b3dc-6713cd33053e",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/receipts/d1c174ea-5b5b-45b0-a01b-af9000e2d352/files/59ce51f6-3f2e-4867-be84-861c3ffe02ef/download"
                            }
                        ]
                    },
                    {
                        "Id": "d1cc8bfe-8895-49b9-8986-8814c279bd20",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "9501d46f-d391-4d57-bf4b-6c02254143e3",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/receipts/d1c174ea-5b5b-45b0-a01b-af9000e2d352/files/d1cc8bfe-8895-49b9-8986-8814c279bd20/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "7f679fe3-7db2-434c-8e91-af9000e40eb4",
                "ReceiveTime": "2023-01-20T13:50:19Z",
                "StatusTime": "2023-01-20T13:50:07Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "2366539c-008d-4655-ae0f-0b842e38e743",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 780,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/receipts/7f679fe3-7db2-434c-8e91-af9000e40eb4/files/2366539c-008d-4655-ae0f-0b842e38e743/download"
                            }
                        ]
                    },
                    {
                        "Id": "bad3fd34-025b-46e9-b846-5ec60017c2f9",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "ba3f334b-0984-49bd-b445-73b420662845",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/receipts/7f679fe3-7db2-434c-8e91-af9000e40eb4/files/bad3fd34-025b-46e9-b846-5ec60017c2f9/download"
                            }
                        ]
                    },
                    {
                        "Id": "ba3f334b-0984-49bd-b445-73b420662845",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 355,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/receipts/7f679fe3-7db2-434c-8e91-af9000e40eb4/files/ba3f334b-0984-49bd-b445-73b420662845/download"
                            }
                        ]
                    },
                    {
                        "Id": "e88bd151-4b23-4303-86be-a0f4a9b6d86e",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "2366539c-008d-4655-ae0f-0b842e38e743",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/071c0c98-ce8d-480d-b4f4-af9000e21ba2/receipts/7f679fe3-7db2-434c-8e91-af9000e40eb4/files/e88bd151-4b23-4303-86be-a0f4a9b6d86e/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "957b9d80-dcb7-4d8a-9f73-af9000f5af11",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Обращение (запрос)  в Банк России",
        "Text": "Предоставление информации",
        "CreationDate": "2023-01-20T14:54:30Z",
        "UpdatedDate": "2023-01-23T06:21:18Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "29385",
        "TotalSize": 1211475,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "029eb35f-31bf-4d8c-9fc7-af9000f5af1e",
                "Name": "Учетная политика на 2023 титул лист.pdf",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 51164,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/files/029eb35f-31bf-4d8c-9fc7-af9000f5af1e/download"
                    }
                ]
            },
            {
                "Id": "81b01f59-b5a7-43da-93cd-af9000f5af44",
                "Name": "УЧЕТНАЯ политика 2023.docx",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 1084312,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/files/81b01f59-b5a7-43da-93cd-af9000f5af44/download"
                    }
                ]
            },
            {
                "Id": "9a6600d4-34d7-4d46-bc84-af9000f5af4f",
                "Name": "Письмо в ЦБ по УП.docx",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 50603,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/files/9a6600d4-34d7-4d46-bc84-af9000f5af4f/download"
                    }
                ]
            },
            {
                "Id": "5413977d-a6b1-4ca9-848d-af9000f5af63",
                "Name": "form.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 1232,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/files/5413977d-a6b1-4ca9-848d-af9000f5af63/download"
                    }
                ]
            },
            {
                "Id": "d01af2f1-d10b-43d8-9bbb-af9000f5fd4d",
                "Name": "Учетная политика на 2023 титул лист.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "029eb35f-31bf-4d8c-9fc7-af9000f5af1e",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/files/d01af2f1-d10b-43d8-9bbb-af9000f5fd4d/download"
                    }
                ]
            },
            {
                "Id": "1be213c0-95d3-4a4a-9e69-af9000f6470c",
                "Name": "Письмо в ЦБ по УП.docx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "9a6600d4-34d7-4d46-bc84-af9000f5af4f",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/files/1be213c0-95d3-4a4a-9e69-af9000f6470c/download"
                    }
                ]
            },
            {
                "Id": "dad8b18f-2444-424b-9f56-af9000f691c3",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "5413977d-a6b1-4ca9-848d-af9000f5af63",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/files/dad8b18f-2444-424b-9f56-af9000f691c3/download"
                    }
                ]
            },
            {
                "Id": "64d8902f-9400-4d16-9840-af9000f6dd1a",
                "Name": "УЧЕТНАЯ политика 2023.docx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "81b01f59-b5a7-43da-93cd-af9000f5af44",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/files/64d8902f-9400-4d16-9840-af9000f6dd1a/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "566b97b9-2bd8-48da-a9ef-af9000f6de90",
                "ReceiveTime": "2023-01-20T14:58:49Z",
                "StatusTime": "2023-01-20T14:58:49Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "baa5a4e4-52d1-4f29-9d1a-af9000f6ef56",
                "ReceiveTime": "2023-01-20T14:59:03Z",
                "StatusTime": "2023-01-20T14:59:00Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "a1da37b9-30e3-4f39-afb4-0bd4d9b640de",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/receipts/baa5a4e4-52d1-4f29-9d1a-af9000f6ef56/files/a1da37b9-30e3-4f39-afb4-0bd4d9b640de/download"
                            }
                        ]
                    },
                    {
                        "Id": "7649f131-6cc6-4c8c-9b40-8bd8802d1ec9",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f50ad347-9b7a-4092-9c15-e11c8fb2938b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/receipts/baa5a4e4-52d1-4f29-9d1a-af9000f6ef56/files/7649f131-6cc6-4c8c-9b40-8bd8802d1ec9/download"
                            }
                        ]
                    },
                    {
                        "Id": "f50ad347-9b7a-4092-9c15-e11c8fb2938b",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/receipts/baa5a4e4-52d1-4f29-9d1a-af9000f6ef56/files/f50ad347-9b7a-4092-9c15-e11c8fb2938b/download"
                            }
                        ]
                    },
                    {
                        "Id": "51d7e076-c9e8-4c37-855c-e42cfae9896a",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "a1da37b9-30e3-4f39-afb4-0bd4d9b640de",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/receipts/baa5a4e4-52d1-4f29-9d1a-af9000f6ef56/files/51d7e076-c9e8-4c37-855c-e42cfae9896a/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "8d275326-64fe-4d1e-8e52-af9000f6f8da",
                "ReceiveTime": "2023-01-20T14:59:11Z",
                "StatusTime": "2023-01-20T14:59:03Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "2a0a33ed-81a0-4803-bfa1-3661ba475570",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/receipts/8d275326-64fe-4d1e-8e52-af9000f6f8da/files/2a0a33ed-81a0-4803-bfa1-3661ba475570/download"
                            }
                        ]
                    },
                    {
                        "Id": "15641ea0-6a98-49e0-9f3f-44db3f0f81ac",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "2a0a33ed-81a0-4803-bfa1-3661ba475570",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/receipts/8d275326-64fe-4d1e-8e52-af9000f6f8da/files/15641ea0-6a98-49e0-9f3f-44db3f0f81ac/download"
                            }
                        ]
                    },
                    {
                        "Id": "d7251776-48df-4b4f-8064-8643df6d788b",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "e9776df9-564d-4226-9b3a-dcf18baa6801",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/receipts/8d275326-64fe-4d1e-8e52-af9000f6f8da/files/d7251776-48df-4b4f-8064-8643df6d788b/download"
                            }
                        ]
                    },
                    {
                        "Id": "e9776df9-564d-4226-9b3a-dcf18baa6801",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/receipts/8d275326-64fe-4d1e-8e52-af9000f6f8da/files/e9776df9-564d-4226-9b3a-dcf18baa6801/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "cc1072cb-b0d3-4071-ba52-af930068bac9",
                "ReceiveTime": "2023-01-23T06:21:18Z",
                "StatusTime": "2023-01-23T06:21:10Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "e76013c0-a491-4ddb-8fa2-304211810324",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "2aa74f6f-18c1-4ed7-9bc2-589a5b83d64b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/receipts/cc1072cb-b0d3-4071-ba52-af930068bac9/files/e76013c0-a491-4ddb-8fa2-304211810324/download"
                            }
                        ]
                    },
                    {
                        "Id": "84479c3e-7f6f-4e05-9411-4edd29afbc0c",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/receipts/cc1072cb-b0d3-4071-ba52-af930068bac9/files/84479c3e-7f6f-4e05-9411-4edd29afbc0c/download"
                            }
                        ]
                    },
                    {
                        "Id": "2aa74f6f-18c1-4ed7-9bc2-589a5b83d64b",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 742,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/receipts/cc1072cb-b0d3-4071-ba52-af930068bac9/files/2aa74f6f-18c1-4ed7-9bc2-589a5b83d64b/download"
                            }
                        ]
                    },
                    {
                        "Id": "a57eb1e3-c7ae-4b8a-9270-a79f14e929f2",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "84479c3e-7f6f-4e05-9411-4edd29afbc0c",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/957b9d80-dcb7-4d8a-9f73-af9000f5af11/receipts/cc1072cb-b0d3-4071-ba52-af930068bac9/files/a57eb1e3-c7ae-4b8a-9270-a79f14e929f2/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "b02bce9d-c3a3-4b41-8290-af9000f9373f",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-01-20T15:08:27Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 2269886,
        "Sender": null,
        "Files": [
            {
                "Id": "185a79ac-264f-4da1-bebc-3b0e80de967d",
                "Name": "KYC_20230120.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "345298cc-5c3e-44e9-8e24-8e819b8c0f02",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b02bce9d-c3a3-4b41-8290-af9000f9373f/files/185a79ac-264f-4da1-bebc-3b0e80de967d/download"
                    }
                ]
            },
            {
                "Id": "345298cc-5c3e-44e9-8e24-8e819b8c0f02",
                "Name": "KYC_20230120.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2266625,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b02bce9d-c3a3-4b41-8290-af9000f9373f/files/345298cc-5c3e-44e9-8e24-8e819b8c0f02/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "8eb445e1-4e5f-4c4b-a762-af9001289aa2",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-20T17:59:54Z",
        "UpdatedDate": "2023-01-20T18:02:54Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00574957",
        "TotalSize": 13352,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "bd033fb4-def4-40e1-904c-af9001289a9d",
                "Name": "KYCCL_7831001422_3194_20230120_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9480,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/8eb445e1-4e5f-4c4b-a762-af9001289aa2/files/bd033fb4-def4-40e1-904c-af9001289a9d/download"
                    }
                ]
            },
            {
                "Id": "21249740-b2a5-4ad5-9477-af9001289aa0",
                "Name": "KYCCL_7831001422_3194_20230120_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "bd033fb4-def4-40e1-904c-af9001289a9d",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/8eb445e1-4e5f-4c4b-a762-af9001289aa2/files/21249740-b2a5-4ad5-9477-af9001289aa0/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "ffc98c08-2d4c-4ee6-9f4e-af9001289d42",
                "ReceiveTime": "2023-01-20T17:59:56Z",
                "StatusTime": "2023-01-20T17:59:56Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "e93653a7-f2ed-4de8-a744-af900128a764",
                "ReceiveTime": "2023-01-20T18:00:05Z",
                "StatusTime": "2023-01-20T18:00:04Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "05c1b72b-9cff-4f48-bb4c-4a51a736348f",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "0434e907-ea03-46c6-9f84-632afe193196",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8eb445e1-4e5f-4c4b-a762-af9001289aa2/receipts/e93653a7-f2ed-4de8-a744-af900128a764/files/05c1b72b-9cff-4f48-bb4c-4a51a736348f/download"
                            }
                        ]
                    },
                    {
                        "Id": "c4c3a895-1aee-4d05-a7ff-5c1080d27c81",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "edd6390e-c1d6-439c-9bd6-dc7ad1666708",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8eb445e1-4e5f-4c4b-a762-af9001289aa2/receipts/e93653a7-f2ed-4de8-a744-af900128a764/files/c4c3a895-1aee-4d05-a7ff-5c1080d27c81/download"
                            }
                        ]
                    },
                    {
                        "Id": "0434e907-ea03-46c6-9f84-632afe193196",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8eb445e1-4e5f-4c4b-a762-af9001289aa2/receipts/e93653a7-f2ed-4de8-a744-af900128a764/files/0434e907-ea03-46c6-9f84-632afe193196/download"
                            }
                        ]
                    },
                    {
                        "Id": "edd6390e-c1d6-439c-9bd6-dc7ad1666708",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8eb445e1-4e5f-4c4b-a762-af9001289aa2/receipts/e93653a7-f2ed-4de8-a744-af900128a764/files/edd6390e-c1d6-439c-9bd6-dc7ad1666708/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "a4f27c6a-f005-4a5a-adb7-af900128a99c",
                "ReceiveTime": "2023-01-20T18:00:06Z",
                "StatusTime": "2023-01-20T18:00:04Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "28277de3-6918-493b-80da-6a1c8422d95a",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8eb445e1-4e5f-4c4b-a762-af9001289aa2/receipts/a4f27c6a-f005-4a5a-adb7-af900128a99c/files/28277de3-6918-493b-80da-6a1c8422d95a/download"
                            }
                        ]
                    },
                    {
                        "Id": "d1859942-06a7-43be-98f0-87682a20f2ec",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "28277de3-6918-493b-80da-6a1c8422d95a",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8eb445e1-4e5f-4c4b-a762-af9001289aa2/receipts/a4f27c6a-f005-4a5a-adb7-af900128a99c/files/d1859942-06a7-43be-98f0-87682a20f2ec/download"
                            }
                        ]
                    },
                    {
                        "Id": "af029c95-03fb-4432-8456-933fd02b4deb",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "cd48f8f6-6039-4208-bfcd-a46231690427",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8eb445e1-4e5f-4c4b-a762-af9001289aa2/receipts/a4f27c6a-f005-4a5a-adb7-af900128a99c/files/af029c95-03fb-4432-8456-933fd02b4deb/download"
                            }
                        ]
                    },
                    {
                        "Id": "cd48f8f6-6039-4208-bfcd-a46231690427",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8eb445e1-4e5f-4c4b-a762-af9001289aa2/receipts/a4f27c6a-f005-4a5a-adb7-af900128a99c/files/cd48f8f6-6039-4208-bfcd-a46231690427/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "ecb1b33d-40e4-4cb6-a92e-af9001296d9b",
                "ReceiveTime": "2023-01-20T18:02:54Z",
                "StatusTime": "2023-01-20T18:02:31Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "a1f690fd-4944-43bd-8ce1-40d25434c71f",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8eb445e1-4e5f-4c4b-a762-af9001289aa2/receipts/ecb1b33d-40e4-4cb6-a92e-af9001296d9b/files/a1f690fd-4944-43bd-8ce1-40d25434c71f/download"
                            }
                        ]
                    },
                    {
                        "Id": "18098538-d9ff-4134-a1d0-fc0921114548",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "a1f690fd-4944-43bd-8ce1-40d25434c71f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/8eb445e1-4e5f-4c4b-a762-af9001289aa2/receipts/ecb1b33d-40e4-4cb6-a92e-af9001296d9b/files/18098538-d9ff-4134-a1d0-fc0921114548/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "1df23ba8-7eae-4471-93b7-af910128a28b",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-21T18:00:00Z",
        "UpdatedDate": "2023-01-21T18:04:12Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00575011",
        "TotalSize": 13357,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "b6a15680-981e-4953-b752-af910128a281",
                "Name": "KYCCL_7831001422_3194_20230121_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9485,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1df23ba8-7eae-4471-93b7-af910128a28b/files/b6a15680-981e-4953-b752-af910128a281/download"
                    }
                ]
            },
            {
                "Id": "532fae4b-007b-40ea-ba51-af910128a283",
                "Name": "KYCCL_7831001422_3194_20230121_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "b6a15680-981e-4953-b752-af910128a281",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1df23ba8-7eae-4471-93b7-af910128a28b/files/532fae4b-007b-40ea-ba51-af910128a283/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "6f0d6053-cbab-43f8-b910-af910128a417",
                "ReceiveTime": "2023-01-21T18:00:02Z",
                "StatusTime": "2023-01-21T18:00:02Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "359e1714-99e0-4811-b03a-af910128b52b",
                "ReceiveTime": "2023-01-21T18:00:16Z",
                "StatusTime": "2023-01-21T18:00:12Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "63038beb-c6a4-4cb5-9109-3c185131dbff",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1df23ba8-7eae-4471-93b7-af910128a28b/receipts/359e1714-99e0-4811-b03a-af910128b52b/files/63038beb-c6a4-4cb5-9109-3c185131dbff/download"
                            }
                        ]
                    },
                    {
                        "Id": "379cfc5b-bf05-40c4-a5a9-9a050ac0ee37",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "741f4789-a676-4936-b49b-cc162c42dacd",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1df23ba8-7eae-4471-93b7-af910128a28b/receipts/359e1714-99e0-4811-b03a-af910128b52b/files/379cfc5b-bf05-40c4-a5a9-9a050ac0ee37/download"
                            }
                        ]
                    },
                    {
                        "Id": "741f4789-a676-4936-b49b-cc162c42dacd",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1df23ba8-7eae-4471-93b7-af910128a28b/receipts/359e1714-99e0-4811-b03a-af910128b52b/files/741f4789-a676-4936-b49b-cc162c42dacd/download"
                            }
                        ]
                    },
                    {
                        "Id": "7cd3ecf1-fa78-4ef7-97c7-e50c5c8329e1",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "63038beb-c6a4-4cb5-9109-3c185131dbff",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1df23ba8-7eae-4471-93b7-af910128a28b/receipts/359e1714-99e0-4811-b03a-af910128b52b/files/7cd3ecf1-fa78-4ef7-97c7-e50c5c8329e1/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "5e5e90ad-44db-4f7c-90bd-af910128bdac",
                "ReceiveTime": "2023-01-21T18:00:24Z",
                "StatusTime": "2023-01-21T18:00:16Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "bfb16b44-3a3b-498f-9bfb-1bda92f9beec",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1df23ba8-7eae-4471-93b7-af910128a28b/receipts/5e5e90ad-44db-4f7c-90bd-af910128bdac/files/bfb16b44-3a3b-498f-9bfb-1bda92f9beec/download"
                            }
                        ]
                    },
                    {
                        "Id": "53a296b6-a919-4257-a589-5a134b316b8e",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "bfb16b44-3a3b-498f-9bfb-1bda92f9beec",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1df23ba8-7eae-4471-93b7-af910128a28b/receipts/5e5e90ad-44db-4f7c-90bd-af910128bdac/files/53a296b6-a919-4257-a589-5a134b316b8e/download"
                            }
                        ]
                    },
                    {
                        "Id": "1b97ea25-ba05-436c-bf08-b7cc75ee9e91",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1df23ba8-7eae-4471-93b7-af910128a28b/receipts/5e5e90ad-44db-4f7c-90bd-af910128bdac/files/1b97ea25-ba05-436c-bf08-b7cc75ee9e91/download"
                            }
                        ]
                    },
                    {
                        "Id": "3ebe45f1-10f5-4ff6-ae80-fe2485d095d6",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "1b97ea25-ba05-436c-bf08-b7cc75ee9e91",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1df23ba8-7eae-4471-93b7-af910128a28b/receipts/5e5e90ad-44db-4f7c-90bd-af910128bdac/files/3ebe45f1-10f5-4ff6-ae80-fe2485d095d6/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "23d6653a-48c6-4d97-ba4a-af910129c926",
                "ReceiveTime": "2023-01-21T18:04:12Z",
                "StatusTime": "2023-01-21T18:02:39Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "b4810295-d068-4565-8f3d-69eed24e3f36",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "c5104494-fe84-4b28-8f38-d466f49ae0e9",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1df23ba8-7eae-4471-93b7-af910128a28b/receipts/23d6653a-48c6-4d97-ba4a-af910129c926/files/b4810295-d068-4565-8f3d-69eed24e3f36/download"
                            }
                        ]
                    },
                    {
                        "Id": "c5104494-fe84-4b28-8f38-d466f49ae0e9",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1df23ba8-7eae-4471-93b7-af910128a28b/receipts/23d6653a-48c6-4d97-ba4a-af910129c926/files/c5104494-fe84-4b28-8f38-d466f49ae0e9/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "89cbb7c5-7422-4239-a0cd-af930112065a",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-01-23T16:37:49Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 2278948,
        "Sender": null,
        "Files": [
            {
                "Id": "4fddad58-f0a9-4d49-aff4-3f115cf41345",
                "Name": "KYC_20230123.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2275687,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/89cbb7c5-7422-4239-a0cd-af930112065a/files/4fddad58-f0a9-4d49-aff4-3f115cf41345/download"
                    }
                ]
            },
            {
                "Id": "d7805d3b-d484-47b5-9f63-d4416c90c990",
                "Name": "KYC_20230123.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "4fddad58-f0a9-4d49-aff4-3f115cf41345",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/89cbb7c5-7422-4239-a0cd-af930112065a/files/d7805d3b-d484-47b5-9f63-d4416c90c990/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "125f8b7c-0c4b-491d-9691-af930128a019",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-23T17:59:58Z",
        "UpdatedDate": "2023-01-23T18:02:58Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00575902",
        "TotalSize": 13319,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "ca35f9bf-5e86-4572-a024-af930128a015",
                "Name": "KYCCL_7831001422_3194_20230123_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9447,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/125f8b7c-0c4b-491d-9691-af930128a019/files/ca35f9bf-5e86-4572-a024-af930128a015/download"
                    }
                ]
            },
            {
                "Id": "dcbf1038-639f-42c1-befd-af930128a017",
                "Name": "KYCCL_7831001422_3194_20230123_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "ca35f9bf-5e86-4572-a024-af930128a015",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/125f8b7c-0c4b-491d-9691-af930128a019/files/dcbf1038-639f-42c1-befd-af930128a017/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "64ee39e9-2006-4514-9151-af930128a1fd",
                "ReceiveTime": "2023-01-23T18:00:00Z",
                "StatusTime": "2023-01-23T18:00:00Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "33ac63cc-9acb-4cab-9052-af930128afee",
                "ReceiveTime": "2023-01-23T18:00:12Z",
                "StatusTime": "2023-01-23T18:00:09Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "6459fd86-2c06-4829-bb3f-602f20910c02",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/125f8b7c-0c4b-491d-9691-af930128a019/receipts/33ac63cc-9acb-4cab-9052-af930128afee/files/6459fd86-2c06-4829-bb3f-602f20910c02/download"
                            }
                        ]
                    },
                    {
                        "Id": "49d48f13-c8ce-4590-9079-7af637be15ce",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "38a57189-fd59-48f2-8cdc-f253349ccecf",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/125f8b7c-0c4b-491d-9691-af930128a019/receipts/33ac63cc-9acb-4cab-9052-af930128afee/files/49d48f13-c8ce-4590-9079-7af637be15ce/download"
                            }
                        ]
                    },
                    {
                        "Id": "c5e2c699-c035-4d5e-93c2-80c1f378a4b1",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "6459fd86-2c06-4829-bb3f-602f20910c02",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/125f8b7c-0c4b-491d-9691-af930128a019/receipts/33ac63cc-9acb-4cab-9052-af930128afee/files/c5e2c699-c035-4d5e-93c2-80c1f378a4b1/download"
                            }
                        ]
                    },
                    {
                        "Id": "38a57189-fd59-48f2-8cdc-f253349ccecf",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/125f8b7c-0c4b-491d-9691-af930128a019/receipts/33ac63cc-9acb-4cab-9052-af930128afee/files/38a57189-fd59-48f2-8cdc-f253349ccecf/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "31da84bd-c2f9-4923-8134-af930128b74a",
                "ReceiveTime": "2023-01-23T18:00:18Z",
                "StatusTime": "2023-01-23T18:00:12Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "1c0e4bb8-20d9-43a8-87f1-033536d60b90",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "12818df6-78fb-46d8-aac0-b6d4485ec039",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/125f8b7c-0c4b-491d-9691-af930128a019/receipts/31da84bd-c2f9-4923-8134-af930128b74a/files/1c0e4bb8-20d9-43a8-87f1-033536d60b90/download"
                            }
                        ]
                    },
                    {
                        "Id": "fbb57317-7a6e-43db-8da7-3ee0070aff8a",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/125f8b7c-0c4b-491d-9691-af930128a019/receipts/31da84bd-c2f9-4923-8134-af930128b74a/files/fbb57317-7a6e-43db-8da7-3ee0070aff8a/download"
                            }
                        ]
                    },
                    {
                        "Id": "12818df6-78fb-46d8-aac0-b6d4485ec039",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/125f8b7c-0c4b-491d-9691-af930128a019/receipts/31da84bd-c2f9-4923-8134-af930128b74a/files/12818df6-78fb-46d8-aac0-b6d4485ec039/download"
                            }
                        ]
                    },
                    {
                        "Id": "df8984e1-35c7-4cc6-b3df-bfbead5963a1",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "fbb57317-7a6e-43db-8da7-3ee0070aff8a",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/125f8b7c-0c4b-491d-9691-af930128a019/receipts/31da84bd-c2f9-4923-8134-af930128b74a/files/df8984e1-35c7-4cc6-b3df-bfbead5963a1/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "1236b1a4-3517-4dd4-b959-af9301297302",
                "ReceiveTime": "2023-01-23T18:02:58Z",
                "StatusTime": "2023-01-23T18:02:29Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "c8649730-1cfb-4c6e-a9f0-279cd0ad385f",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/125f8b7c-0c4b-491d-9691-af930128a019/receipts/1236b1a4-3517-4dd4-b959-af9301297302/files/c8649730-1cfb-4c6e-a9f0-279cd0ad385f/download"
                            }
                        ]
                    },
                    {
                        "Id": "8c13d7a2-64b2-41b6-b230-90c135d39d5f",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "c8649730-1cfb-4c6e-a9f0-279cd0ad385f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/125f8b7c-0c4b-491d-9691-af930128a019/receipts/1236b1a4-3517-4dd4-b959-af9301297302/files/8c13d7a2-64b2-41b6-b230-90c135d39d5f/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "0081d307-e72d-46bd-af00-af94009a40d5",
        "CorrelationId": "353e28f1-1fbb-4e62-8724-af73005acc12",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "предоставление запрошенной информации в рамках совещания",
        "CreationDate": "2023-01-24T09:21:37Z",
        "UpdatedDate": "2023-01-24T09:52:22Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "33363",
        "TotalSize": 152550,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "3e2af383-9637-4224-9b14-af94009a40f7",
                "Name": "2023-01-24-11-2.docx.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 61189,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/files/3e2af383-9637-4224-9b14-af94009a40f7/download"
                    }
                ]
            },
            {
                "Id": "8d16b61b-8a31-4f24-8cc9-af94009a40fc",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2042,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/files/8d16b61b-8a31-4f24-8cc9-af94009a40fc/download"
                    }
                ]
            },
            {
                "Id": "ee716839-1236-4c7e-99b0-af94009a4118",
                "Name": "2023-01-19 Стратегия развития на 2023 год.docx.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 71196,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/files/ee716839-1236-4c7e-99b0-af94009a4118/download"
                    }
                ]
            },
            {
                "Id": "5c5509ef-bee5-475c-96cd-af94009a8ff3",
                "Name": "2023-01-19 Стратегия развития на 2023 год.docx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "ee716839-1236-4c7e-99b0-af94009a4118",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/files/5c5509ef-bee5-475c-96cd-af94009a8ff3/download"
                    }
                ]
            },
            {
                "Id": "00301b3e-a5c2-4d8c-ac7c-af94009ad88d",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "8d16b61b-8a31-4f24-8cc9-af94009a40fc",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/files/00301b3e-a5c2-4d8c-ac7c-af94009ad88d/download"
                    }
                ]
            },
            {
                "Id": "ec697447-8e07-4a1d-9357-af94009b236f",
                "Name": "2023-01-24-11-2.docx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "3e2af383-9637-4224-9b14-af94009a40f7",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/files/ec697447-8e07-4a1d-9357-af94009b236f/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "ca2817c6-7b94-44c4-8ae1-af94009b24a3",
                "ReceiveTime": "2023-01-24T09:24:51Z",
                "StatusTime": "2023-01-24T09:24:51Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "7a066510-07e5-41d6-9a84-af94009b3841",
                "ReceiveTime": "2023-01-24T09:25:08Z",
                "StatusTime": "2023-01-24T09:25:03Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "51fab50a-a426-424d-bcee-793c510b7c81",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/receipts/7a066510-07e5-41d6-9a84-af94009b3841/files/51fab50a-a426-424d-bcee-793c510b7c81/download"
                            }
                        ]
                    },
                    {
                        "Id": "6adb74e1-5ce4-49dd-b1cd-a1d31eefbc9d",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "aec0a434-c2f5-49f6-bc64-d38aa07ae071",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/receipts/7a066510-07e5-41d6-9a84-af94009b3841/files/6adb74e1-5ce4-49dd-b1cd-a1d31eefbc9d/download"
                            }
                        ]
                    },
                    {
                        "Id": "05b76fd0-bd6a-405f-90a2-aea1c365cfdd",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "51fab50a-a426-424d-bcee-793c510b7c81",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/receipts/7a066510-07e5-41d6-9a84-af94009b3841/files/05b76fd0-bd6a-405f-90a2-aea1c365cfdd/download"
                            }
                        ]
                    },
                    {
                        "Id": "aec0a434-c2f5-49f6-bc64-d38aa07ae071",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/receipts/7a066510-07e5-41d6-9a84-af94009b3841/files/aec0a434-c2f5-49f6-bc64-d38aa07ae071/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "c5fd89a9-37fa-4c1b-b5ba-af94009b3f7b",
                "ReceiveTime": "2023-01-24T09:25:14Z",
                "StatusTime": "2023-01-24T09:25:07Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "d3460ebf-01fb-4c6e-add2-15c727c5c1c6",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "3f364d02-0a8c-4dfa-9a57-b2c10f6b90f8",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/receipts/c5fd89a9-37fa-4c1b-b5ba-af94009b3f7b/files/d3460ebf-01fb-4c6e-add2-15c727c5c1c6/download"
                            }
                        ]
                    },
                    {
                        "Id": "3b03dd96-a39a-4ff6-89bc-a0c8c4354800",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "4be7b0cc-271a-499a-a8a7-b662fabec4a3",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/receipts/c5fd89a9-37fa-4c1b-b5ba-af94009b3f7b/files/3b03dd96-a39a-4ff6-89bc-a0c8c4354800/download"
                            }
                        ]
                    },
                    {
                        "Id": "3f364d02-0a8c-4dfa-9a57-b2c10f6b90f8",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/receipts/c5fd89a9-37fa-4c1b-b5ba-af94009b3f7b/files/3f364d02-0a8c-4dfa-9a57-b2c10f6b90f8/download"
                            }
                        ]
                    },
                    {
                        "Id": "4be7b0cc-271a-499a-a8a7-b662fabec4a3",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/receipts/c5fd89a9-37fa-4c1b-b5ba-af94009b3f7b/files/4be7b0cc-271a-499a-a8a7-b662fabec4a3/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "430097c4-5c28-4ef1-bd67-af9400a2b3b6",
                "ReceiveTime": "2023-01-24T09:52:22Z",
                "StatusTime": "2023-01-24T09:52:11Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "de1a4a22-4eee-4274-8885-27d5a10e5ab1",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "9aca9206-89f9-4639-8b5d-70b0f4153eff",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/receipts/430097c4-5c28-4ef1-bd67-af9400a2b3b6/files/de1a4a22-4eee-4274-8885-27d5a10e5ab1/download"
                            }
                        ]
                    },
                    {
                        "Id": "fa6a22e7-b393-4c02-8aed-31f38d79216f",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/receipts/430097c4-5c28-4ef1-bd67-af9400a2b3b6/files/fa6a22e7-b393-4c02-8aed-31f38d79216f/download"
                            }
                        ]
                    },
                    {
                        "Id": "9aca9206-89f9-4639-8b5d-70b0f4153eff",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 742,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/receipts/430097c4-5c28-4ef1-bd67-af9400a2b3b6/files/9aca9206-89f9-4639-8b5d-70b0f4153eff/download"
                            }
                        ]
                    },
                    {
                        "Id": "e676d08a-2215-4bab-a79c-d71440dd2711",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "fa6a22e7-b393-4c02-8aed-31f38d79216f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0081d307-e72d-46bd-af00-af94009a40d5/receipts/430097c4-5c28-4ef1-bd67-af9400a2b3b6/files/e676d08a-2215-4bab-a79c-d71440dd2711/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "09f16222-a7d6-4368-a998-af9400b17d47",
        "CorrelationId": null,
        "GroupId": "b43a8799-3a7b-4857-ada2-c2679f4ebfa4",
        "Type": "inbox",
        "Title": "№ 20-2-1/20 от 24/01/2023 (20) Письма Деп-та денежно-кредитной политики",
        "Text": "О предоставлении данных о резервируемых обязательствах",
        "CreationDate": "2023-01-24T10:46:16Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "20-2-1/20",
        "TotalSize": 676563,
        "Sender": null,
        "Files": [
            {
                "Id": "52ec77ff-2232-4405-87bf-06e5c6563782",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "616037944.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 121204,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/09f16222-a7d6-4368-a998-af9400b17d47/files/52ec77ff-2232-4405-87bf-06e5c6563782/download"
                    }
                ]
            },
            {
                "Id": "e92657ce-bf42-4441-9c4d-16048619fb09",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "52ec77ff-2232-4405-87bf-06e5c6563782",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/09f16222-a7d6-4368-a998-af9400b17d47/files/e92657ce-bf42-4441-9c4d-16048619fb09/download"
                    }
                ]
            },
            {
                "Id": "349b0bf1-76f0-4641-8fad-55af56469f7a",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 245178,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/09f16222-a7d6-4368-a998-af9400b17d47/files/349b0bf1-76f0-4641-8fad-55af56469f7a/download"
                    }
                ]
            },
            {
                "Id": "e56d4b7d-6be7-4cf1-aeaa-55e897bd1307",
                "Name": "ЭП_6360-У.docx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "9c4f9fb8-01f3-4cf3-812b-a83eb7b6816c",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/09f16222-a7d6-4368-a998-af9400b17d47/files/e56d4b7d-6be7-4cf1-aeaa-55e897bd1307/download"
                    }
                ]
            },
            {
                "Id": "e8ae86fd-7997-418b-8b5b-59f162699b0a",
                "Name": "ЭД_Письмо.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "a8f724f4-b6c4-4509-8ae3-f113287b60d9",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/09f16222-a7d6-4368-a998-af9400b17d47/files/e8ae86fd-7997-418b-8b5b-59f162699b0a/download"
                    }
                ]
            },
            {
                "Id": "86812f01-3cfe-4df1-ba36-a408ddbeab5a",
                "Name": "ЭП_Приложение 2.xlsx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "f97a8ceb-165a-4723-b557-e3f9b8b57a6b",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/09f16222-a7d6-4368-a998-af9400b17d47/files/86812f01-3cfe-4df1-ba36-a408ddbeab5a/download"
                    }
                ]
            },
            {
                "Id": "9c4f9fb8-01f3-4cf3-812b-a83eb7b6816c",
                "Name": "ЭП_6360-У.docx",
                "Description": "616037407.docx",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 157300,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/09f16222-a7d6-4368-a998-af9400b17d47/files/9c4f9fb8-01f3-4cf3-812b-a83eb7b6816c/download"
                    }
                ]
            },
            {
                "Id": "23bc958c-d255-49e4-b93f-b9fa33b30482",
                "Name": "ЭП_Приложение 1.xlsx",
                "Description": "616037401.xlsx",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 9812,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/09f16222-a7d6-4368-a998-af9400b17d47/files/23bc958c-d255-49e4-b93f-b9fa33b30482/download"
                    }
                ]
            },
            {
                "Id": "3da2b497-24e8-4724-b966-daa29cbc126c",
                "Name": "ЭП_Приложение 1.xlsx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "23bc958c-d255-49e4-b93f-b9fa33b30482",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/09f16222-a7d6-4368-a998-af9400b17d47/files/3da2b497-24e8-4724-b966-daa29cbc126c/download"
                    }
                ]
            },
            {
                "Id": "f97a8ceb-165a-4723-b557-e3f9b8b57a6b",
                "Name": "ЭП_Приложение 2.xlsx",
                "Description": "616037404.xlsx",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 11176,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/09f16222-a7d6-4368-a998-af9400b17d47/files/f97a8ceb-165a-4723-b557-e3f9b8b57a6b/download"
                    }
                ]
            },
            {
                "Id": "a8f724f4-b6c4-4509-8ae3-f113287b60d9",
                "Name": "ЭД_Письмо.pdf",
                "Description": "616037397.pdf",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 115588,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/09f16222-a7d6-4368-a998-af9400b17d47/files/a8f724f4-b6c4-4509-8ae3-f113287b60d9/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "bc243a4c-84af-4e1d-9acb-af9400df964b",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Расчёт и регулирование размера обязательных резервов",
        "Text": "",
        "CreationDate": "2023-01-24T13:34:03Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_94",
        "RegNumber": null,
        "TotalSize": 45938,
        "Sender": null,
        "Files": [
            {
                "Id": "f7999358-3418-46b2-9e4c-00579b470603",
                "Name": "FOR-3194-0000000603-01012023-19012023163018.xlsx",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 32816,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/bc243a4c-84af-4e1d-9acb-af9400df964b/files/f7999358-3418-46b2-9e4c-00579b470603/download"
                    }
                ]
            },
            {
                "Id": "902c691e-dfbf-48f8-999a-58c2294d784b",
                "Name": "FOR-3194-0000000603-01012023-24012023163225.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "9e847918-4b23-413e-a417-8ee8f3506e40",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/bc243a4c-84af-4e1d-9acb-af9400df964b/files/902c691e-dfbf-48f8-999a-58c2294d784b/download"
                    }
                ]
            },
            {
                "Id": "9e847918-4b23-413e-a417-8ee8f3506e40",
                "Name": "FOR-3194-0000000603-01012023-24012023163225.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 6600,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/bc243a4c-84af-4e1d-9acb-af9400df964b/files/9e847918-4b23-413e-a417-8ee8f3506e40/download"
                    }
                ]
            },
            {
                "Id": "f07b9c48-539a-4e3f-82e2-dac7311676b8",
                "Name": "FOR-3194-0000000603-01012023-19012023163018.xlsx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "f7999358-3418-46b2-9e4c-00579b470603",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/bc243a4c-84af-4e1d-9acb-af9400df964b/files/f07b9c48-539a-4e3f-82e2-dac7311676b8/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "5f706009-237b-4414-80c5-af9400f4fa77",
        "CorrelationId": "f1acc920-feb0-4471-ba57-af9400e49bcc",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос ЦИК: Уточнение запроса (Квитанция)",
        "Text": null,
        "CreationDate": "2023-01-24T14:51:56Z",
        "UpdatedDate": "2023-01-24T14:52:23Z",
        "Status": "success",
        "TaskName": "Zadacha_58",
        "RegNumber": null,
        "TotalSize": 622,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "504d6d52-c0c4-4abd-8834-af9400f4fa4b",
                "Name": "K1027800000095_240123_175013_K_0003_1000_F1027700466640.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 622,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/files/504d6d52-c0c4-4abd-8834-af9400f4fa4b/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "3b4ccd09-9826-40e9-8589-af9400f4fc2d",
                "ReceiveTime": "2023-01-24T14:51:57Z",
                "StatusTime": "2023-01-24T14:51:57Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "76908817-ac1f-4617-a010-af9400f50eff",
                "ReceiveTime": "2023-01-24T14:52:13Z",
                "StatusTime": "2023-01-24T14:52:09Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "37deb7bc-a1d5-45a5-88e1-76c0c97146c3",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "ecf86cdc-2ef3-45a9-a29c-d9c2463f1be0",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/receipts/76908817-ac1f-4617-a010-af9400f50eff/files/37deb7bc-a1d5-45a5-88e1-76c0c97146c3/download"
                            }
                        ]
                    },
                    {
                        "Id": "7d8cd727-4e9c-491b-b8ba-b697ee5dae64",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "87f10931-bd8e-4e96-ab26-cee9672ef6e2",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/receipts/76908817-ac1f-4617-a010-af9400f50eff/files/7d8cd727-4e9c-491b-b8ba-b697ee5dae64/download"
                            }
                        ]
                    },
                    {
                        "Id": "87f10931-bd8e-4e96-ab26-cee9672ef6e2",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/receipts/76908817-ac1f-4617-a010-af9400f50eff/files/87f10931-bd8e-4e96-ab26-cee9672ef6e2/download"
                            }
                        ]
                    },
                    {
                        "Id": "ecf86cdc-2ef3-45a9-a29c-d9c2463f1be0",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 956,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/receipts/76908817-ac1f-4617-a010-af9400f50eff/files/ecf86cdc-2ef3-45a9-a29c-d9c2463f1be0/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "346e15de-f740-491e-a768-af9400f51a75",
                "ReceiveTime": "2023-01-24T14:52:23Z",
                "StatusTime": "2023-01-24T15:01:55Z",
                "Status": "success",
                "Message": null,
                "Files": [
                    {
                        "Id": "1daca171-2567-4f7c-b7d6-59335e37dfef",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1058,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/receipts/346e15de-f740-491e-a768-af9400f51a75/files/1daca171-2567-4f7c-b7d6-59335e37dfef/download"
                            }
                        ]
                    },
                    {
                        "Id": "64276a88-57e7-444c-a20a-59d26ab4c50e",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "1daca171-2567-4f7c-b7d6-59335e37dfef",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/receipts/346e15de-f740-491e-a768-af9400f51a75/files/64276a88-57e7-444c-a20a-59d26ab4c50e/download"
                            }
                        ]
                    },
                    {
                        "Id": "9b6ca41b-d6fa-4eff-9838-b80939ac36e0",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 141,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/receipts/346e15de-f740-491e-a768-af9400f51a75/files/9b6ca41b-d6fa-4eff-9838-b80939ac36e0/download"
                            }
                        ]
                    },
                    {
                        "Id": "a4be9188-60ae-4d75-b191-d0f97afbeaa4",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "9b6ca41b-d6fa-4eff-9838-b80939ac36e0",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/receipts/346e15de-f740-491e-a768-af9400f51a75/files/a4be9188-60ae-4d75-b191-d0f97afbeaa4/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "a2900706-f955-4711-9381-af9400f51a78",
                "ReceiveTime": "2023-01-24T14:52:23Z",
                "StatusTime": "2023-01-24T14:52:13Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "96a9dc00-b93c-4b9e-9c26-1e31430ca6ac",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "3e344b14-c638-4d4b-8540-34af9e5039ae",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/receipts/a2900706-f955-4711-9381-af9400f51a78/files/96a9dc00-b93c-4b9e-9c26-1e31430ca6ac/download"
                            }
                        ]
                    },
                    {
                        "Id": "2f7f0e2d-c0df-46cf-9afd-3080038c532f",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 957,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/receipts/a2900706-f955-4711-9381-af9400f51a78/files/2f7f0e2d-c0df-46cf-9afd-3080038c532f/download"
                            }
                        ]
                    },
                    {
                        "Id": "3e344b14-c638-4d4b-8540-34af9e5039ae",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/receipts/a2900706-f955-4711-9381-af9400f51a78/files/3e344b14-c638-4d4b-8540-34af9e5039ae/download"
                            }
                        ]
                    },
                    {
                        "Id": "1c5f60f7-d872-4d58-9398-a1ced1503b69",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "2f7f0e2d-c0df-46cf-9afd-3080038c532f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/5f706009-237b-4414-80c5-af9400f4fa77/receipts/a2900706-f955-4711-9381-af9400f51a78/files/1c5f60f7-d872-4d58-9398-a1ced1503b69/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "6a95ff62-ad6a-4a8d-b9fd-af940105e528",
        "CorrelationId": null,
        "GroupId": "42ff689b-f781-4737-8be8-f2da67490e34",
        "Type": "inbox",
        "Title": "№ 08-12/498 от 24/01/2023 Письма за подписью руководства Банка России",
        "Text": "О сообщениях в налоговые органы",
        "CreationDate": "2023-01-24T15:53:37Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "08-12/498",
        "TotalSize": 431236,
        "Sender": null,
        "Files": [
            {
                "Id": "559d21fc-da47-40dc-8028-6850bc174478",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "92069655-d0d1-475c-87b7-ebe3274f250b",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6a95ff62-ad6a-4a8d-b9fd-af940105e528/files/559d21fc-da47-40dc-8028-6850bc174478/download"
                    }
                ]
            },
            {
                "Id": "8e97e7f1-e6ef-49c9-929a-91c49a8ddaf2",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 256793,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6a95ff62-ad6a-4a8d-b9fd-af940105e528/files/8e97e7f1-e6ef-49c9-929a-91c49a8ddaf2/download"
                    }
                ]
            },
            {
                "Id": "49530a41-b692-40e0-a1ef-9809efaec16d",
                "Name": "ЭП_приложение.doc",
                "Description": "616288901.doc",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 52736,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6a95ff62-ad6a-4a8d-b9fd-af940105e528/files/49530a41-b692-40e0-a1ef-9809efaec16d/download"
                    }
                ]
            },
            {
                "Id": "92069655-d0d1-475c-87b7-ebe3274f250b",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "616292855.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 115185,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6a95ff62-ad6a-4a8d-b9fd-af940105e528/files/92069655-d0d1-475c-87b7-ebe3274f250b/download"
                    }
                ]
            },
            {
                "Id": "0bd007a5-e519-4b26-83cc-f14746face73",
                "Name": "ЭП_приложение.doc.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "49530a41-b692-40e0-a1ef-9809efaec16d",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6a95ff62-ad6a-4a8d-b9fd-af940105e528/files/0bd007a5-e519-4b26-83cc-f14746face73/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "7dafef6d-3dff-43fc-9a98-af940122833c",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-01-24T17:37:52Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 2298660,
        "Sender": null,
        "Files": [
            {
                "Id": "1d468877-e169-44ea-9101-0e62a9aae8f5",
                "Name": "KYC_20230124.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2295399,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7dafef6d-3dff-43fc-9a98-af940122833c/files/1d468877-e169-44ea-9101-0e62a9aae8f5/download"
                    }
                ]
            },
            {
                "Id": "62c7b73f-7e15-478f-bbce-fef81febde92",
                "Name": "KYC_20230124.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "1d468877-e169-44ea-9101-0e62a9aae8f5",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/7dafef6d-3dff-43fc-9a98-af940122833c/files/62c7b73f-7e15-478f-bbce-fef81febde92/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "394c0829-6c3a-428d-af9e-af9401289ec8",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-24T17:59:57Z",
        "UpdatedDate": "2023-01-24T18:04:30Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00576673",
        "TotalSize": 13210,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "9d3587af-b2b0-4da1-9c06-af9401289ec4",
                "Name": "KYCCL_7831001422_3194_20230124_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9338,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/394c0829-6c3a-428d-af9e-af9401289ec8/files/9d3587af-b2b0-4da1-9c06-af9401289ec4/download"
                    }
                ]
            },
            {
                "Id": "c95a7f2d-4d0a-4867-9641-af9401289ec6",
                "Name": "KYCCL_7831001422_3194_20230124_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "9d3587af-b2b0-4da1-9c06-af9401289ec4",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/394c0829-6c3a-428d-af9e-af9401289ec8/files/c95a7f2d-4d0a-4867-9641-af9401289ec6/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "1530f31c-4de9-4f6d-ad0a-af940128a058",
                "ReceiveTime": "2023-01-24T17:59:59Z",
                "StatusTime": "2023-01-24T17:59:59Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "f4c0a939-3f8f-41da-bee4-af940128afc2",
                "ReceiveTime": "2023-01-24T18:00:12Z",
                "StatusTime": "2023-01-24T18:00:08Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "991fad2e-1d39-4d1f-8777-7a929e99c4af",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/394c0829-6c3a-428d-af9e-af9401289ec8/receipts/f4c0a939-3f8f-41da-bee4-af940128afc2/files/991fad2e-1d39-4d1f-8777-7a929e99c4af/download"
                            }
                        ]
                    },
                    {
                        "Id": "b2c5c05a-7da0-4a5e-b11a-951348946419",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "991fad2e-1d39-4d1f-8777-7a929e99c4af",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/394c0829-6c3a-428d-af9e-af9401289ec8/receipts/f4c0a939-3f8f-41da-bee4-af940128afc2/files/b2c5c05a-7da0-4a5e-b11a-951348946419/download"
                            }
                        ]
                    },
                    {
                        "Id": "863639e5-f5e7-438f-aa6c-ba27023d9c77",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "e9c08e04-b578-413f-8ebb-cb46b8e50767",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/394c0829-6c3a-428d-af9e-af9401289ec8/receipts/f4c0a939-3f8f-41da-bee4-af940128afc2/files/863639e5-f5e7-438f-aa6c-ba27023d9c77/download"
                            }
                        ]
                    },
                    {
                        "Id": "e9c08e04-b578-413f-8ebb-cb46b8e50767",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/394c0829-6c3a-428d-af9e-af9401289ec8/receipts/f4c0a939-3f8f-41da-bee4-af940128afc2/files/e9c08e04-b578-413f-8ebb-cb46b8e50767/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "0caa4db7-2042-4185-83f6-af940128b72a",
                "ReceiveTime": "2023-01-24T18:00:18Z",
                "StatusTime": "2023-01-24T18:00:11Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "255c20af-db26-473a-9a54-424ba9dfb173",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/394c0829-6c3a-428d-af9e-af9401289ec8/receipts/0caa4db7-2042-4185-83f6-af940128b72a/files/255c20af-db26-473a-9a54-424ba9dfb173/download"
                            }
                        ]
                    },
                    {
                        "Id": "9ef212c7-d267-408b-aff5-5e73a2771292",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "32e7be03-5d16-472d-8d6d-692e69373da5",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/394c0829-6c3a-428d-af9e-af9401289ec8/receipts/0caa4db7-2042-4185-83f6-af940128b72a/files/9ef212c7-d267-408b-aff5-5e73a2771292/download"
                            }
                        ]
                    },
                    {
                        "Id": "32e7be03-5d16-472d-8d6d-692e69373da5",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/394c0829-6c3a-428d-af9e-af9401289ec8/receipts/0caa4db7-2042-4185-83f6-af940128b72a/files/32e7be03-5d16-472d-8d6d-692e69373da5/download"
                            }
                        ]
                    },
                    {
                        "Id": "9834a229-f31a-47a2-baaa-c63af45a7c09",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "255c20af-db26-473a-9a54-424ba9dfb173",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/394c0829-6c3a-428d-af9e-af9401289ec8/receipts/0caa4db7-2042-4185-83f6-af940128b72a/files/9834a229-f31a-47a2-baaa-c63af45a7c09/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "8f568752-ddf1-495d-997c-af940129de63",
                "ReceiveTime": "2023-01-24T18:04:30Z",
                "StatusTime": "2023-01-24T18:03:16Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "c051899d-dc59-47e4-977b-7aa677305c9e",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f46a7bf5-c914-4e18-a61d-96e93a5057f1",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/394c0829-6c3a-428d-af9e-af9401289ec8/receipts/8f568752-ddf1-495d-997c-af940129de63/files/c051899d-dc59-47e4-977b-7aa677305c9e/download"
                            }
                        ]
                    },
                    {
                        "Id": "f46a7bf5-c914-4e18-a61d-96e93a5057f1",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/394c0829-6c3a-428d-af9e-af9401289ec8/receipts/8f568752-ddf1-495d-997c-af940129de63/files/f46a7bf5-c914-4e18-a61d-96e93a5057f1/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "1efae7f2-9d44-4e32-9f46-af95007c0ce0",
        "CorrelationId": "1f6158a2-a7a1-4e14-aace-af7a00f65145",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "в дополнение к нашему исх. 1-3 от 09.01.2023",
        "CreationDate": "2023-01-25T07:31:39Z",
        "UpdatedDate": "2023-01-25T07:45:20Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "35465",
        "TotalSize": 58386,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "e8f3fa58-1485-4cc3-be93-af95007c0cdd",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2004,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/files/e8f3fa58-1485-4cc3-be93-af95007c0cdd/download"
                    }
                ]
            },
            {
                "Id": "5f4425fd-96ce-4579-875d-af95007c0cf6",
                "Name": "ОВП за 22-01.xls.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 44300,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/files/5f4425fd-96ce-4579-875d-af95007c0cf6/download"
                    }
                ]
            },
            {
                "Id": "0c169a51-a5d2-4597-a851-af95007c5e08",
                "Name": "ОВП за 22-01.xls.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "5f4425fd-96ce-4579-875d-af95007c0cf6",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/files/0c169a51-a5d2-4597-a851-af95007c5e08/download"
                    }
                ]
            },
            {
                "Id": "7f5fb793-a6ff-4008-a173-af95007ca8d9",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "e8f3fa58-1485-4cc3-be93-af95007c0cdd",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/files/7f5fb793-a6ff-4008-a173-af95007ca8d9/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "f58b1c2b-5b9a-450f-8c6b-af95007caa39",
                "ReceiveTime": "2023-01-25T07:33:53Z",
                "StatusTime": "2023-01-25T07:33:53Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "42088dd5-dad8-47bf-8d0b-af95007cbd85",
                "ReceiveTime": "2023-01-25T07:34:09Z",
                "StatusTime": "2023-01-25T07:34:05Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "fb5efd16-f37a-4dd0-8973-13b6140fa461",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/receipts/42088dd5-dad8-47bf-8d0b-af95007cbd85/files/fb5efd16-f37a-4dd0-8973-13b6140fa461/download"
                            }
                        ]
                    },
                    {
                        "Id": "e6831972-6137-403a-b1f9-8e3d9ed6ed0c",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "11ccec0d-0e40-48bf-bf18-ed768f96f33b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/receipts/42088dd5-dad8-47bf-8d0b-af95007cbd85/files/e6831972-6137-403a-b1f9-8e3d9ed6ed0c/download"
                            }
                        ]
                    },
                    {
                        "Id": "e3274395-a3cd-4171-8af7-a7e5c6fe8c15",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "fb5efd16-f37a-4dd0-8973-13b6140fa461",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/receipts/42088dd5-dad8-47bf-8d0b-af95007cbd85/files/e3274395-a3cd-4171-8af7-a7e5c6fe8c15/download"
                            }
                        ]
                    },
                    {
                        "Id": "11ccec0d-0e40-48bf-bf18-ed768f96f33b",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/receipts/42088dd5-dad8-47bf-8d0b-af95007cbd85/files/11ccec0d-0e40-48bf-bf18-ed768f96f33b/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "2252dfea-2cb0-433e-8047-af95007cc677",
                "ReceiveTime": "2023-01-25T07:34:17Z",
                "StatusTime": "2023-01-25T07:34:09Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "10e4959d-e218-49cf-912d-1458ec4467a7",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "11787c47-05bb-4fd5-8690-2a67b05367bf",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/receipts/2252dfea-2cb0-433e-8047-af95007cc677/files/10e4959d-e218-49cf-912d-1458ec4467a7/download"
                            }
                        ]
                    },
                    {
                        "Id": "11787c47-05bb-4fd5-8690-2a67b05367bf",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/receipts/2252dfea-2cb0-433e-8047-af95007cc677/files/11787c47-05bb-4fd5-8690-2a67b05367bf/download"
                            }
                        ]
                    },
                    {
                        "Id": "4eb17043-4193-4e4c-ae51-6da2add55bdc",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/receipts/2252dfea-2cb0-433e-8047-af95007cc677/files/4eb17043-4193-4e4c-ae51-6da2add55bdc/download"
                            }
                        ]
                    },
                    {
                        "Id": "1ec000ec-0b97-48ef-9d4f-d031e92e8915",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "4eb17043-4193-4e4c-ae51-6da2add55bdc",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/receipts/2252dfea-2cb0-433e-8047-af95007cc677/files/1ec000ec-0b97-48ef-9d4f-d031e92e8915/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "f311dbc6-80b1-4ecd-90bc-af95007fcf94",
                "ReceiveTime": "2023-01-25T07:45:20Z",
                "StatusTime": "2023-01-25T07:45:14Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "b7885201-dcb1-4670-82fc-0649f33357b4",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 742,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/receipts/f311dbc6-80b1-4ecd-90bc-af95007fcf94/files/b7885201-dcb1-4670-82fc-0649f33357b4/download"
                            }
                        ]
                    },
                    {
                        "Id": "fcd09d83-d2ab-4c36-8bb0-646309931fc8",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "74755ad3-3935-4723-9ef0-6e1a3e16bd34",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/receipts/f311dbc6-80b1-4ecd-90bc-af95007fcf94/files/fcd09d83-d2ab-4c36-8bb0-646309931fc8/download"
                            }
                        ]
                    },
                    {
                        "Id": "74755ad3-3935-4723-9ef0-6e1a3e16bd34",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/receipts/f311dbc6-80b1-4ecd-90bc-af95007fcf94/files/74755ad3-3935-4723-9ef0-6e1a3e16bd34/download"
                            }
                        ]
                    },
                    {
                        "Id": "8e337863-e9b5-4352-9984-a5456ca5ebcd",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "b7885201-dcb1-4670-82fc-0649f33357b4",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/1efae7f2-9d44-4e32-9f46-af95007c0ce0/receipts/f311dbc6-80b1-4ecd-90bc-af95007fcf94/files/8e337863-e9b5-4352-9984-a5456ca5ebcd/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "f4cfea62-7e19-44b8-9bba-af9500e0ae90",
        "CorrelationId": "79e55273-579e-492f-a296-af8800a33f0d",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "предоставление запрошенной информации",
        "CreationDate": "2023-01-25T13:38:02Z",
        "UpdatedDate": "2023-01-25T13:56:20Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "36843",
        "TotalSize": 37586846,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "b4828e5b-8597-4d64-bad4-af9500e0aedc",
                "Name": "2023-01-25-12-2.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 36997590,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/files/b4828e5b-8597-4d64-bad4-af9500e0aedc/download"
                    }
                ]
            },
            {
                "Id": "697d46ee-12fd-4925-b37c-af9500e0af05",
                "Name": "Исх 12-2 от 25.01.2023.pdf.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 569095,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/files/697d46ee-12fd-4925-b37c-af9500e0af05/download"
                    }
                ]
            },
            {
                "Id": "2dc53b73-1590-4450-9c49-af9500e0af06",
                "Name": "form.xml.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2038,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/files/2dc53b73-1590-4450-9c49-af9500e0af06/download"
                    }
                ]
            },
            {
                "Id": "44358120-b539-436f-8b6a-af9500e0ffce",
                "Name": "Исх 12-2 от 25.01.2023.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "697d46ee-12fd-4925-b37c-af9500e0af05",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/files/44358120-b539-436f-8b6a-af9500e0ffce/download"
                    }
                ]
            },
            {
                "Id": "27c3ba7f-f887-4610-999c-af9500e14bf1",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "2dc53b73-1590-4450-9c49-af9500e0af06",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/files/27c3ba7f-f887-4610-999c-af9500e14bf1/download"
                    }
                ]
            },
            {
                "Id": "b285b962-7cf5-49c1-a022-af9500e22bcf",
                "Name": "2023-01-25-12-2.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "b4828e5b-8597-4d64-bad4-af9500e0aedc",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/files/b285b962-7cf5-49c1-a022-af9500e22bcf/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "7629eab3-f6dd-4449-a1a6-af9500e230bc",
                "ReceiveTime": "2023-01-25T13:43:32Z",
                "StatusTime": "2023-01-25T13:43:32Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "58ed15c0-b806-47f3-a914-af9500e253b1",
                "ReceiveTime": "2023-01-25T13:44:01Z",
                "StatusTime": "2023-01-25T13:43:50Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "2dcf53dc-0c27-44bd-aba3-425c2a82990d",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f07b6f26-7912-49b8-a7ef-5ec3a330ee22",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/receipts/58ed15c0-b806-47f3-a914-af9500e253b1/files/2dcf53dc-0c27-44bd-aba3-425c2a82990d/download"
                            }
                        ]
                    },
                    {
                        "Id": "f07b6f26-7912-49b8-a7ef-5ec3a330ee22",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/receipts/58ed15c0-b806-47f3-a914-af9500e253b1/files/f07b6f26-7912-49b8-a7ef-5ec3a330ee22/download"
                            }
                        ]
                    },
                    {
                        "Id": "8782d66c-e1ef-46fd-8316-741876972144",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/receipts/58ed15c0-b806-47f3-a914-af9500e253b1/files/8782d66c-e1ef-46fd-8316-741876972144/download"
                            }
                        ]
                    },
                    {
                        "Id": "63145dbf-5f3e-4bfc-b5e1-c1715ecc8595",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "8782d66c-e1ef-46fd-8316-741876972144",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/receipts/58ed15c0-b806-47f3-a914-af9500e253b1/files/63145dbf-5f3e-4bfc-b5e1-c1715ecc8595/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "3be3d4b9-f11e-4ede-82f3-af9500e25970",
                "ReceiveTime": "2023-01-25T13:44:06Z",
                "StatusTime": "2023-01-25T13:43:51Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "eb4cb9ce-716a-4664-bbd0-0aa4641dcf8f",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/receipts/3be3d4b9-f11e-4ede-82f3-af9500e25970/files/eb4cb9ce-716a-4664-bbd0-0aa4641dcf8f/download"
                            }
                        ]
                    },
                    {
                        "Id": "0b620026-d4c2-485d-bcc6-25a5c6c8e444",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/receipts/3be3d4b9-f11e-4ede-82f3-af9500e25970/files/0b620026-d4c2-485d-bcc6-25a5c6c8e444/download"
                            }
                        ]
                    },
                    {
                        "Id": "9aec69bc-8e65-4d23-8369-91e2a3a469ea",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "eb4cb9ce-716a-4664-bbd0-0aa4641dcf8f",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/receipts/3be3d4b9-f11e-4ede-82f3-af9500e25970/files/9aec69bc-8e65-4d23-8369-91e2a3a469ea/download"
                            }
                        ]
                    },
                    {
                        "Id": "be99d4d6-8441-4e4d-b061-caefe48c2ab0",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "0b620026-d4c2-485d-bcc6-25a5c6c8e444",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/receipts/3be3d4b9-f11e-4ede-82f3-af9500e25970/files/be99d4d6-8441-4e4d-b061-caefe48c2ab0/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "18107803-c568-4e00-9f0b-af9500e5b5a8",
                "ReceiveTime": "2023-01-25T13:56:20Z",
                "StatusTime": "2023-01-25T13:56:14Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "f19002da-26ff-4365-88a4-1e66c47b0412",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "591f3b9c-9558-4603-a7e2-aeca142a51d0",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/receipts/18107803-c568-4e00-9f0b-af9500e5b5a8/files/f19002da-26ff-4365-88a4-1e66c47b0412/download"
                            }
                        ]
                    },
                    {
                        "Id": "9e845f6a-3711-41eb-8b54-6ab1147890b0",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/receipts/18107803-c568-4e00-9f0b-af9500e5b5a8/files/9e845f6a-3711-41eb-8b54-6ab1147890b0/download"
                            }
                        ]
                    },
                    {
                        "Id": "66804ef1-d1a4-4288-8446-7f86bd0d8531",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "9e845f6a-3711-41eb-8b54-6ab1147890b0",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/receipts/18107803-c568-4e00-9f0b-af9500e5b5a8/files/66804ef1-d1a4-4288-8446-7f86bd0d8531/download"
                            }
                        ]
                    },
                    {
                        "Id": "591f3b9c-9558-4603-a7e2-aeca142a51d0",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 742,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/f4cfea62-7e19-44b8-9bba-af9500e0ae90/receipts/18107803-c568-4e00-9f0b-af9500e5b5a8/files/591f3b9c-9558-4603-a7e2-aeca142a51d0/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "a921c0fa-3f50-4fde-b182-af9500e2f158",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Запрос ЦИК в организацию. Ответ ЦИК в организацию.",
        "Text": "",
        "CreationDate": "2023-01-25T13:46:16Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_56",
        "RegNumber": null,
        "TotalSize": 3916,
        "Sender": {
            "Inn": "7710168307",
            "Ogrn": "1037739236578",
            "Bik": null,
            "RegNum": null,
            "DivisionCode": null
        },
        "Files": [
            {
                "Id": "820a9ed3-9d1f-4a26-95a9-43dceac29458",
                "Name": "F1027700466640_250123_163258_K_0003_2000_K1027800000095.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "b7807e71-1c8f-4021-836f-af9500e2c7fa",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a921c0fa-3f50-4fde-b182-af9500e2f158/files/820a9ed3-9d1f-4a26-95a9-43dceac29458/download"
                    }
                ]
            },
            {
                "Id": "b7807e71-1c8f-4021-836f-af9500e2c7fa",
                "Name": "F1027700466640_250123_163258_K_0003_2000_K1027800000095.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 655,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a921c0fa-3f50-4fde-b182-af9500e2f158/files/b7807e71-1c8f-4021-836f-af9500e2c7fa/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "d1cbc695-c9d4-42c7-a182-af9500fd8899",
        "CorrelationId": "88eda28e-9fb4-4c8a-b9bb-af8f0060d1da",
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ответ на запрос/предписание (требование)",
        "Text": "о счетах типа \"С\"",
        "CreationDate": "2023-01-25T15:23:05Z",
        "UpdatedDate": "2023-01-25T16:00:25Z",
        "Status": "registered",
        "TaskName": "Zadacha_2-1",
        "RegNumber": "37208",
        "TotalSize": 2248567,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "ecf1bc80-4b99-4809-94a7-af9500fd88a6",
                "Name": "Приложение.pdf",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 1702423,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/files/ecf1bc80-4b99-4809-94a7-af9500fd88a6/download"
                    }
                ]
            },
            {
                "Id": "7c81f1e6-2a52-45f4-9e45-af9500fd88a9",
                "Name": "2023-01-25-12-4.pdf",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 527155,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/files/7c81f1e6-2a52-45f4-9e45-af9500fd88a9/download"
                    }
                ]
            },
            {
                "Id": "76fdfea9-a076-46f8-8b51-af9500fd88ab",
                "Name": "form.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 866,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/files/76fdfea9-a076-46f8-8b51-af9500fd88ab/download"
                    }
                ]
            },
            {
                "Id": "c4c90bc5-ff7f-450b-8d4a-af9500fde390",
                "Name": "2023-01-25-12-4.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "7c81f1e6-2a52-45f4-9e45-af9500fd88a9",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/files/c4c90bc5-ff7f-450b-8d4a-af9500fde390/download"
                    }
                ]
            },
            {
                "Id": "a882b5e2-591a-4d41-be20-af9500fe2d6a",
                "Name": "form.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "76fdfea9-a076-46f8-8b51-af9500fd88ab",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/files/a882b5e2-591a-4d41-be20-af9500fe2d6a/download"
                    }
                ]
            },
            {
                "Id": "30e4fa4d-1564-4652-8711-af9500fe78a4",
                "Name": "Приложение.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "ecf1bc80-4b99-4809-94a7-af9500fd88a6",
                "Size": 6041,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/files/30e4fa4d-1564-4652-8711-af9500fe78a4/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "cd8e2428-63f5-4089-b8bf-af9500fe79bb",
                "ReceiveTime": "2023-01-25T15:26:31Z",
                "StatusTime": "2023-01-25T15:26:31Z",
                "Status": "answer",
                "Message": null,
                "Files": []
            },
            {
                "Id": "6a51bb32-0377-4c25-8fd2-af9500fe8a6d",
                "ReceiveTime": "2023-01-25T15:26:45Z",
                "StatusTime": "2023-01-25T15:26:41Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "deed9cac-14ca-4621-95ce-3d836dd746b5",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/receipts/6a51bb32-0377-4c25-8fd2-af9500fe8a6d/files/deed9cac-14ca-4621-95ce-3d836dd746b5/download"
                            }
                        ]
                    },
                    {
                        "Id": "c0a85a34-d761-4bfb-b37c-68f866ebbfe4",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "b869140f-07c2-4ead-9454-9a709967b772",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/receipts/6a51bb32-0377-4c25-8fd2-af9500fe8a6d/files/c0a85a34-d761-4bfb-b37c-68f866ebbfe4/download"
                            }
                        ]
                    },
                    {
                        "Id": "b869140f-07c2-4ead-9454-9a709967b772",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/receipts/6a51bb32-0377-4c25-8fd2-af9500fe8a6d/files/b869140f-07c2-4ead-9454-9a709967b772/download"
                            }
                        ]
                    },
                    {
                        "Id": "cb25f7dd-bd7b-480f-a1ac-b9c5b0dae26a",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "deed9cac-14ca-4621-95ce-3d836dd746b5",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/receipts/6a51bb32-0377-4c25-8fd2-af9500fe8a6d/files/cb25f7dd-bd7b-480f-a1ac-b9c5b0dae26a/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "d05b2992-ab62-4cd1-a149-af9500fe91fe",
                "ReceiveTime": "2023-01-25T15:26:51Z",
                "StatusTime": "2023-01-25T15:26:45Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "4e8c8c23-39b6-48dc-adef-1afcc58cdb06",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/receipts/d05b2992-ab62-4cd1-a149-af9500fe91fe/files/4e8c8c23-39b6-48dc-adef-1afcc58cdb06/download"
                            }
                        ]
                    },
                    {
                        "Id": "d0b83f22-1dce-4c02-8405-2389e6a6e5c8",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/receipts/d05b2992-ab62-4cd1-a149-af9500fe91fe/files/d0b83f22-1dce-4c02-8405-2389e6a6e5c8/download"
                            }
                        ]
                    },
                    {
                        "Id": "d014dbb7-0f2f-461d-8993-e604ef334ef6",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "4e8c8c23-39b6-48dc-adef-1afcc58cdb06",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/receipts/d05b2992-ab62-4cd1-a149-af9500fe91fe/files/d014dbb7-0f2f-461d-8993-e604ef334ef6/download"
                            }
                        ]
                    },
                    {
                        "Id": "67b059bc-e708-49bf-a82c-f805a7a9c340",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "d0b83f22-1dce-4c02-8405-2389e6a6e5c8",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/receipts/d05b2992-ab62-4cd1-a149-af9500fe91fe/files/67b059bc-e708-49bf-a82c-f805a7a9c340/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "f7aa2998-f09f-4b42-ab42-af950107c992",
                "ReceiveTime": "2023-01-25T16:00:25Z",
                "StatusTime": "2023-01-25T16:00:14Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "2394a0b5-b008-474b-a2be-04f761ab8e72",
                        "Name": "receipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "8e0986ea-4d29-4d0c-b58d-7f4ed010ed23",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/receipts/f7aa2998-f09f-4b42-ab42-af950107c992/files/2394a0b5-b008-474b-a2be-04f761ab8e72/download"
                            }
                        ]
                    },
                    {
                        "Id": "8e0986ea-4d29-4d0c-b58d-7f4ed010ed23",
                        "Name": "receipt.xml",
                        "Description": "receipt.xml",
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 742,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/receipts/f7aa2998-f09f-4b42-ab42-af950107c992/files/8e0986ea-4d29-4d0c-b58d-7f4ed010ed23/download"
                            }
                        ]
                    },
                    {
                        "Id": "4b5714b7-fcb7-4849-8794-d4cc041ca4e6",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 354,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/receipts/f7aa2998-f09f-4b42-ab42-af950107c992/files/4b5714b7-fcb7-4849-8794-d4cc041ca4e6/download"
                            }
                        ]
                    },
                    {
                        "Id": "b7bfb438-f921-4741-b9b7-fe42c37f6409",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "4b5714b7-fcb7-4849-8794-d4cc041ca4e6",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/d1cbc695-c9d4-42c7-a182-af9500fd8899/receipts/f7aa2998-f09f-4b42-ab42-af950107c992/files/b7bfb438-f921-4741-b9b7-fe42c37f6409/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "deaadd0e-9fa9-4fb2-acbf-af95010923e1",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-01-25T16:05:29Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 2306721,
        "Sender": null,
        "Files": [
            {
                "Id": "772d3147-47ec-495a-8203-a768a0201268",
                "Name": "KYC_20230125.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2303460,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/deaadd0e-9fa9-4fb2-acbf-af95010923e1/files/772d3147-47ec-495a-8203-a768a0201268/download"
                    }
                ]
            },
            {
                "Id": "c5736b48-6275-49b0-9816-fca1cc06235d",
                "Name": "KYC_20230125.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "772d3147-47ec-495a-8203-a768a0201268",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/deaadd0e-9fa9-4fb2-acbf-af95010923e1/files/c5736b48-6275-49b0-9816-fca1cc06235d/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "6932a121-7f05-41a5-bb5d-af9501289e5e",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-25T17:59:57Z",
        "UpdatedDate": "2023-01-25T18:04:33Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00577715",
        "TotalSize": 13083,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "9168ccfc-7a7f-457a-a45b-af9501289e59",
                "Name": "KYCCL_7831001422_3194_20230125_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9211,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6932a121-7f05-41a5-bb5d-af9501289e5e/files/9168ccfc-7a7f-457a-a45b-af9501289e59/download"
                    }
                ]
            },
            {
                "Id": "e2897804-73f2-47f6-930c-af9501289e5b",
                "Name": "KYCCL_7831001422_3194_20230125_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "9168ccfc-7a7f-457a-a45b-af9501289e59",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6932a121-7f05-41a5-bb5d-af9501289e5e/files/e2897804-73f2-47f6-930c-af9501289e5b/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "6bbfc3db-d20f-4e8d-a734-af9501289fe8",
                "ReceiveTime": "2023-01-25T17:59:58Z",
                "StatusTime": "2023-01-25T17:59:58Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "bcac2e55-ea83-43cf-92b0-af950128b19a",
                "ReceiveTime": "2023-01-25T18:00:13Z",
                "StatusTime": "2023-01-25T18:00:09Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "fc12bcf2-c0cd-474d-87d0-495b39faebc3",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6932a121-7f05-41a5-bb5d-af9501289e5e/receipts/bcac2e55-ea83-43cf-92b0-af950128b19a/files/fc12bcf2-c0cd-474d-87d0-495b39faebc3/download"
                            }
                        ]
                    },
                    {
                        "Id": "f52c5ff0-089f-4138-9ca5-c6c9c6ba2631",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "fc12bcf2-c0cd-474d-87d0-495b39faebc3",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6932a121-7f05-41a5-bb5d-af9501289e5e/receipts/bcac2e55-ea83-43cf-92b0-af950128b19a/files/f52c5ff0-089f-4138-9ca5-c6c9c6ba2631/download"
                            }
                        ]
                    },
                    {
                        "Id": "616f1bca-5435-456b-965f-f1dd7cdfc703",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "77fbc429-3470-491e-bd5f-fdd452632e9b",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6932a121-7f05-41a5-bb5d-af9501289e5e/receipts/bcac2e55-ea83-43cf-92b0-af950128b19a/files/616f1bca-5435-456b-965f-f1dd7cdfc703/download"
                            }
                        ]
                    },
                    {
                        "Id": "77fbc429-3470-491e-bd5f-fdd452632e9b",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6932a121-7f05-41a5-bb5d-af9501289e5e/receipts/bcac2e55-ea83-43cf-92b0-af950128b19a/files/77fbc429-3470-491e-bd5f-fdd452632e9b/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "ed538390-8b82-4559-a404-af950128b7ef",
                "ReceiveTime": "2023-01-25T18:00:19Z",
                "StatusTime": "2023-01-25T18:00:13Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "de9a7f9f-d8fd-45d5-a189-2e4747b8cd07",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6932a121-7f05-41a5-bb5d-af9501289e5e/receipts/ed538390-8b82-4559-a404-af950128b7ef/files/de9a7f9f-d8fd-45d5-a189-2e4747b8cd07/download"
                            }
                        ]
                    },
                    {
                        "Id": "f60c2681-0d84-4fab-a36c-8cb60f366268",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6932a121-7f05-41a5-bb5d-af9501289e5e/receipts/ed538390-8b82-4559-a404-af950128b7ef/files/f60c2681-0d84-4fab-a36c-8cb60f366268/download"
                            }
                        ]
                    },
                    {
                        "Id": "149d6dd6-8568-4612-babf-a4d707f170d1",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "f60c2681-0d84-4fab-a36c-8cb60f366268",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6932a121-7f05-41a5-bb5d-af9501289e5e/receipts/ed538390-8b82-4559-a404-af950128b7ef/files/149d6dd6-8568-4612-babf-a4d707f170d1/download"
                            }
                        ]
                    },
                    {
                        "Id": "50b6b2b0-882b-4eae-95d9-bbb4baeb3f84",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "de9a7f9f-d8fd-45d5-a189-2e4747b8cd07",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6932a121-7f05-41a5-bb5d-af9501289e5e/receipts/ed538390-8b82-4559-a404-af950128b7ef/files/50b6b2b0-882b-4eae-95d9-bbb4baeb3f84/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "0ec92d26-0561-4e5d-a3d3-af950129e271",
                "ReceiveTime": "2023-01-25T18:04:33Z",
                "StatusTime": "2023-01-25T18:03:07Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "1216f0a0-3d95-4349-83fe-851f203e7561",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "19c2f0ee-7fb2-4973-be65-a0b06aeac4e8",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6932a121-7f05-41a5-bb5d-af9501289e5e/receipts/0ec92d26-0561-4e5d-a3d3-af950129e271/files/1216f0a0-3d95-4349-83fe-851f203e7561/download"
                            }
                        ]
                    },
                    {
                        "Id": "19c2f0ee-7fb2-4973-be65-a0b06aeac4e8",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/6932a121-7f05-41a5-bb5d-af9501289e5e/receipts/0ec92d26-0561-4e5d-a3d3-af950129e271/files/19c2f0ee-7fb2-4973-be65-a0b06aeac4e8/download"
                            }
                        ]
                    }
                ]
            }
        ]
    },
    {
        "Id": "9b1edd92-472b-439e-a6db-af9600cc7ab4",
        "CorrelationId": null,
        "GroupId": "45fd1050-b73e-4209-8299-21cd51508bc1",
        "Type": "inbox",
        "Title": "№ 017-56-3/565 от 26/01/2023 Письма за подписью руководства Банка России",
        "Text": "Об усилении контроля за реализацией требований абзаца 6 подпункта 14.3 пункта 14 Положения Банка России № 802-П.",
        "CreationDate": "2023-01-26T12:24:31Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "017-56-3/565",
        "TotalSize": 462759,
        "Sender": null,
        "Files": [
            {
                "Id": "96168867-08bd-4716-b023-34a69d312b5b",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "8b1ac162-4876-429d-894a-9838c3320aa6",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9b1edd92-472b-439e-a6db-af9600cc7ab4/files/96168867-08bd-4716-b023-34a69d312b5b/download"
                    }
                ]
            },
            {
                "Id": "39579527-69a8-41a0-b35f-6d92e9191210",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 221157,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9b1edd92-472b-439e-a6db-af9600cc7ab4/files/39579527-69a8-41a0-b35f-6d92e9191210/download"
                    }
                ]
            },
            {
                "Id": "1e81ff0e-6f3d-4f84-a790-932625ed70c8",
                "Name": "ЭД_565.pdf",
                "Description": "617196826.pdf",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 113940,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9b1edd92-472b-439e-a6db-af9600cc7ab4/files/1e81ff0e-6f3d-4f84-a790-932625ed70c8/download"
                    }
                ]
            },
            {
                "Id": "8b1ac162-4876-429d-894a-9838c3320aa6",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "617197953.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 121140,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9b1edd92-472b-439e-a6db-af9600cc7ab4/files/8b1ac162-4876-429d-894a-9838c3320aa6/download"
                    }
                ]
            },
            {
                "Id": "14504f97-b96e-485d-981d-cc99eda9bfaf",
                "Name": "ЭД_565.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "1e81ff0e-6f3d-4f84-a790-932625ed70c8",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9b1edd92-472b-439e-a6db-af9600cc7ab4/files/14504f97-b96e-485d-981d-cc99eda9bfaf/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "a650a27a-8a97-4526-80ff-af96010a35be",
        "CorrelationId": null,
        "GroupId": "09da2609-6431-4ee0-9162-9dc53159d10a",
        "Type": "inbox",
        "Title": "№ 018-59/573 от 26/01/2023 Письма за подписью руководства Банка России",
        "Text": "О совершении брокерами сделок купли-продажи принадлежащей их клиентам валюты недружественных государств",
        "CreationDate": "2023-01-26T16:09:18Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_3-1",
        "RegNumber": "018-59/573",
        "TotalSize": 436196,
        "Sender": null,
        "Files": [
            {
                "Id": "bdea8bca-b65a-42a8-97f3-23f161e0121d",
                "Name": "ЭД_573.pdf.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "48347f04-5973-4848-86b7-ceb02ce7b579",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a650a27a-8a97-4526-80ff-af96010a35be/files/bdea8bca-b65a-42a8-97f3-23f161e0121d/download"
                    }
                ]
            },
            {
                "Id": "5180793d-86cb-4ce3-9049-56d141a04b9e",
                "Name": "ВизуализацияЭД.PDF",
                "Description": "617409203.PDF",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 105293,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a650a27a-8a97-4526-80ff-af96010a35be/files/5180793d-86cb-4ce3-9049-56d141a04b9e/download"
                    }
                ]
            },
            {
                "Id": "d0ead063-7426-4037-874b-68b8d1594fab",
                "Name": "Список рассылки.xlsx.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "65bf9672-1a47-419d-820a-a7ed630c561c",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a650a27a-8a97-4526-80ff-af96010a35be/files/d0ead063-7426-4037-874b-68b8d1594fab/download"
                    }
                ]
            },
            {
                "Id": "65bf9672-1a47-419d-820a-a7ed630c561c",
                "Name": "Список рассылки.xlsx",
                "Description": "617407977.xlsx",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 19867,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a650a27a-8a97-4526-80ff-af96010a35be/files/65bf9672-1a47-419d-820a-a7ed630c561c/download"
                    }
                ]
            },
            {
                "Id": "c1dda5c0-2f2b-4662-9d56-bbd7e9821cc9",
                "Name": "passport.xml",
                "Description": "Паспорт РК",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 206798,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a650a27a-8a97-4526-80ff-af96010a35be/files/c1dda5c0-2f2b-4662-9d56-bbd7e9821cc9/download"
                    }
                ]
            },
            {
                "Id": "48347f04-5973-4848-86b7-ceb02ce7b579",
                "Name": "ЭД_573.pdf",
                "Description": "617407974.pdf",
                "Encrypted": false,
                "SignedFile": null,
                "Size": 94455,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a650a27a-8a97-4526-80ff-af96010a35be/files/48347f04-5973-4848-86b7-ceb02ce7b579/download"
                    }
                ]
            },
            {
                "Id": "ba33dff6-8bda-45bf-b4ef-f91d79cad6d4",
                "Name": "ВизуализацияЭД.PDF.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "5180793d-86cb-4ce3-9049-56d141a04b9e",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a650a27a-8a97-4526-80ff-af96010a35be/files/ba33dff6-8bda-45bf-b4ef-f91d79cad6d4/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "cbc7797d-2d02-4735-bca7-af960116f96d",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-01-26T16:55:51Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 2316872,
        "Sender": null,
        "Files": [
            {
                "Id": "fd3bf344-ae88-407f-a5b2-8d8208995c19",
                "Name": "KYC_20230126.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "ce336eed-a8ec-4d9f-80c5-be2e93ac558b",
                "Size": 3261,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/cbc7797d-2d02-4735-bca7-af960116f96d/files/fd3bf344-ae88-407f-a5b2-8d8208995c19/download"
                    }
                ]
            },
            {
                "Id": "ce336eed-a8ec-4d9f-80c5-be2e93ac558b",
                "Name": "KYC_20230126.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 2313611,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/cbc7797d-2d02-4735-bca7-af960116f96d/files/ce336eed-a8ec-4d9f-80c5-be2e93ac558b/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "0d414c11-531e-42ba-9159-af9601289c3e",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "outbox",
        "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
        "Text": null,
        "CreationDate": "2023-01-26T17:59:55Z",
        "UpdatedDate": "2023-01-26T18:04:42Z",
        "Status": "registered",
        "TaskName": "Zadacha_137",
        "RegNumber": "17_001_001_00578667",
        "TotalSize": 13101,
        "Sender": {
            "Inn": "7831001422",
            "Ogrn": "1027800000095",
            "Bik": "044030702",
            "RegNum": "3194",
            "DivisionCode": "0000"
        },
        "Files": [
            {
                "Id": "08deaaeb-7e05-43a4-81ef-af9601289c3a",
                "Name": "KYCCL_7831001422_3194_20230126_000001.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 9229,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0d414c11-531e-42ba-9159-af9601289c3e/files/08deaaeb-7e05-43a4-81ef-af9601289c3a/download"
                    }
                ]
            },
            {
                "Id": "951794fc-ad2c-462a-89d4-af9601289c3c",
                "Name": "KYCCL_7831001422_3194_20230126_000001.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "08deaaeb-7e05-43a4-81ef-af9601289c3a",
                "Size": 3872,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/0d414c11-531e-42ba-9159-af9601289c3e/files/951794fc-ad2c-462a-89d4-af9601289c3c/download"
                    }
                ]
            }
        ],
        "Receipts": [
            {
                "Id": "7f88a1db-172e-4b8b-86c7-af9601289e69",
                "ReceiveTime": "2023-01-26T17:59:57Z",
                "StatusTime": "2023-01-26T17:59:57Z",
                "Status": "sent",
                "Message": null,
                "Files": []
            },
            {
                "Id": "456fd1ff-5d8c-4f18-a83f-af960128af41",
                "ReceiveTime": "2023-01-26T18:00:11Z",
                "StatusTime": "2023-01-26T18:00:07Z",
                "Status": "delivered",
                "Message": null,
                "Files": [
                    {
                        "Id": "ee4ce448-d298-461a-80ec-1e118d6dd364",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "33473cdd-bed7-4f48-be27-71c191f787c1",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0d414c11-531e-42ba-9159-af9601289c3e/receipts/456fd1ff-5d8c-4f18-a83f-af960128af41/files/ee4ce448-d298-461a-80ec-1e118d6dd364/download"
                            }
                        ]
                    },
                    {
                        "Id": "33473cdd-bed7-4f48-be27-71c191f787c1",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1023,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0d414c11-531e-42ba-9159-af9601289c3e/receipts/456fd1ff-5d8c-4f18-a83f-af960128af41/files/33473cdd-bed7-4f48-be27-71c191f787c1/download"
                            }
                        ]
                    },
                    {
                        "Id": "33dd3bc6-b789-4d59-a0b5-a88c70fd2e17",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 319,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0d414c11-531e-42ba-9159-af9601289c3e/receipts/456fd1ff-5d8c-4f18-a83f-af960128af41/files/33dd3bc6-b789-4d59-a0b5-a88c70fd2e17/download"
                            }
                        ]
                    },
                    {
                        "Id": "fce780bd-79a3-43fa-93a4-e238a668ee4c",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "33dd3bc6-b789-4d59-a0b5-a88c70fd2e17",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0d414c11-531e-42ba-9159-af9601289c3e/receipts/456fd1ff-5d8c-4f18-a83f-af960128af41/files/fce780bd-79a3-43fa-93a4-e238a668ee4c/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "93e36e49-5cf9-4e46-af32-af960128b7f9",
                "ReceiveTime": "2023-01-26T18:00:19Z",
                "StatusTime": "2023-01-26T18:00:11Z",
                "Status": "processing",
                "Message": null,
                "Files": [
                    {
                        "Id": "e518d35d-a605-4619-9cdf-1660b5fa94a2",
                        "Name": "ESODReceipt.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "381b984f-50a0-43b2-b2ac-82df90f57235",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0d414c11-531e-42ba-9159-af9601289c3e/receipts/93e36e49-5cf9-4e46-af32-af960128b7f9/files/e518d35d-a605-4619-9cdf-1660b5fa94a2/download"
                            }
                        ]
                    },
                    {
                        "Id": "34dfed10-5279-432a-b8fa-25bbf9177d09",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 320,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0d414c11-531e-42ba-9159-af9601289c3e/receipts/93e36e49-5cf9-4e46-af32-af960128b7f9/files/34dfed10-5279-432a-b8fa-25bbf9177d09/download"
                            }
                        ]
                    },
                    {
                        "Id": "381b984f-50a0-43b2-b2ac-82df90f57235",
                        "Name": "ESODReceipt.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 1024,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0d414c11-531e-42ba-9159-af9601289c3e/receipts/93e36e49-5cf9-4e46-af32-af960128b7f9/files/381b984f-50a0-43b2-b2ac-82df90f57235/download"
                            }
                        ]
                    },
                    {
                        "Id": "4163dcc1-78b6-4ded-8cd1-c56a3b2372cf",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "34dfed10-5279-432a-b8fa-25bbf9177d09",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0d414c11-531e-42ba-9159-af9601289c3e/receipts/93e36e49-5cf9-4e46-af32-af960128b7f9/files/4163dcc1-78b6-4ded-8cd1-c56a3b2372cf/download"
                            }
                        ]
                    }
                ]
            },
            {
                "Id": "c2695b7e-0026-4125-ab53-af960129ec79",
                "ReceiveTime": "2023-01-26T18:04:42Z",
                "StatusTime": "2023-01-26T18:03:19Z",
                "Status": "registered",
                "Message": null,
                "Files": [
                    {
                        "Id": "c2c953e6-615a-4c2a-a6d9-8141f58db32e",
                        "Name": "status.xml.sig",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": "b3a118f3-d309-42db-a289-d58b5ce85f9e",
                        "Size": 3261,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0d414c11-531e-42ba-9159-af9601289c3e/receipts/c2695b7e-0026-4125-ab53-af960129ec79/files/c2c953e6-615a-4c2a-a6d9-8141f58db32e/download"
                            }
                        ]
                    },
                    {
                        "Id": "b3a118f3-d309-42db-a289-d58b5ce85f9e",
                        "Name": "status.xml",
                        "Description": null,
                        "Encrypted": false,
                        "SignedFile": null,
                        "Size": 378,
                        "RepositoryInfo": [
                            {
                                "RepositoryType": "http",
                                "Host": "https://portal5.cbr.ru",
                                "Port": 81,
                                "Path": "back/rapi2/messages/0d414c11-531e-42ba-9159-af9601289c3e/receipts/c2695b7e-0026-4125-ab53-af960129ec79/files/b3a118f3-d309-42db-a289-d58b5ce85f9e/download"
                            }
                        ]
                    }
                ]
            }
        ]
    }
]


/* GET https://portal5.cbr.ru/back/rapi2/messages?Task=Zadacha_130&MinDateTime=2023-11-01T00:00:00Z&MaxDateTime=2023-11-07T23:59:59Z
EPVV-Total: 5
EPVV-TotalPages: 1
EPVV-CurrentPage: 1
EPVV-PerCurrentPage: 5

[
    {
        "Id": "6fbc3cf9-b48c-4a15-ba8c-b0ad002c489c",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-11-01T02:41:23Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 3328262,
        "Sender": null,
        "Files": [
            {
                "Id": "3d9f1174-ad1d-485e-8149-109ae7353688",
                "Name": "KYC_20231031.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 3324863,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6fbc3cf9-b48c-4a15-ba8c-b0ad002c489c/files/3d9f1174-ad1d-485e-8149-109ae7353688/download"
                    }
                ]
            },
            {
                "Id": "d2a087db-55b8-4348-9ec7-5313c935ec41",
                "Name": "KYC_20231031.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "3d9f1174-ad1d-485e-8149-109ae7353688",
                "Size": 3399,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/6fbc3cf9-b48c-4a15-ba8c-b0ad002c489c/files/d2a087db-55b8-4348-9ec7-5313c935ec41/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "a4a9cf94-fe03-4892-9595-b0ad010cc9ea",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-11-01T16:18:47Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 3336630,
        "Sender": null,
        "Files": [
            {
                "Id": "ee9f0f1c-bd2f-4aea-87db-9559c06b83a3",
                "Name": "KYC_20231101.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 3333231,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a4a9cf94-fe03-4892-9595-b0ad010cc9ea/files/ee9f0f1c-bd2f-4aea-87db-9559c06b83a3/download"
                    }
                ]
            },
            {
                "Id": "1d47d72e-47cb-4c25-81c2-bcc4853bf6dc",
                "Name": "KYC_20231101.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "ee9f0f1c-bd2f-4aea-87db-9559c06b83a3",
                "Size": 3399,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/a4a9cf94-fe03-4892-9595-b0ad010cc9ea/files/1d47d72e-47cb-4c25-81c2-bcc4853bf6dc/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "b5725ef7-080c-404c-a310-b0ae01074dd4",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-11-02T15:58:49Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 3347478,
        "Sender": null,
        "Files": [
            {
                "Id": "de6702b7-c1c2-4aad-8caa-68bb6a3cd536",
                "Name": "KYC_20231102.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 3344079,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b5725ef7-080c-404c-a310-b0ae01074dd4/files/de6702b7-c1c2-4aad-8caa-68bb6a3cd536/download"
                    }
                ]
            },
            {
                "Id": "f9ae6fe0-2748-4514-bca2-697f6b81463a",
                "Name": "KYC_20231102.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "de6702b7-c1c2-4aad-8caa-68bb6a3cd536",
                "Size": 3399,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/b5725ef7-080c-404c-a310-b0ae01074dd4/files/f9ae6fe0-2748-4514-bca2-697f6b81463a/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "e75a3d45-4548-4c8c-b53a-b0af00f43081",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-11-03T14:49:13Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 3351468,
        "Sender": null,
        "Files": [
            {
                "Id": "04ed3915-5e14-4aee-adc4-43e28d973b31",
                "Name": "KYC_20231103.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 3348069,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e75a3d45-4548-4c8c-b53a-b0af00f43081/files/04ed3915-5e14-4aee-adc4-43e28d973b31/download"
                    }
                ]
            },
            {
                "Id": "03c64700-5224-4138-81d2-be839e07ea62",
                "Name": "KYC_20231103.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "04ed3915-5e14-4aee-adc4-43e28d973b31",
                "Size": 3399,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/e75a3d45-4548-4c8c-b53a-b0af00f43081/files/03c64700-5224-4138-81d2-be839e07ea62/download"
                    }
                ]
            }
        ],
        "Receipts": []
    },
    {
        "Id": "9f8a4712-9bd3-431d-94b2-b0b3010a229a",
        "CorrelationId": null,
        "GroupId": null,
        "Type": "inbox",
        "Title": "Получение информации об уровне риска ЮЛ/ИП",
        "Text": "",
        "CreationDate": "2023-11-07T16:09:07Z",
        "UpdatedDate": null,
        "Status": "read",
        "TaskName": "Zadacha_130",
        "RegNumber": null,
        "TotalSize": 3273181,
        "Sender": null,
        "Files": [
            {
                "Id": "9cd13e23-7d36-47e4-974e-0791d5dda91a",
                "Name": "KYC_20231107.xml.zip.enc",
                "Description": null,
                "Encrypted": true,
                "SignedFile": null,
                "Size": 3269782,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9f8a4712-9bd3-431d-94b2-b0b3010a229a/files/9cd13e23-7d36-47e4-974e-0791d5dda91a/download"
                    }
                ]
            },
            {
                "Id": "8be3afd7-2b67-452e-bcc5-0ce9ae882ebc",
                "Name": "KYC_20231107.xml.zip.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "9cd13e23-7d36-47e4-974e-0791d5dda91a",
                "Size": 3399,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/9f8a4712-9bd3-431d-94b2-b0b3010a229a/files/8be3afd7-2b67-452e-bcc5-0ce9ae882ebc/download"
                    }
                ]
            }
        ],
        "Receipts": []
    }
]
*/
