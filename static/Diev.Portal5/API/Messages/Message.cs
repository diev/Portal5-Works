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

using System.Text.Json.Serialization;

using Diev.Portal5.API.Info;

namespace Diev.Portal5.API.Messages;

/// <summary>
/// Сообщение.<br/>
/// 200 OK<br/>
/// Headers:<br/>
/// Content-Type: application/json; charset=utf-8<br/>
/// EPVV-Total: 323<br/>
/// EPVV-TotalPages: 4<br/>
/// EPVV-CurrentPage: 1<br/>
/// EPVV-PerCurrentPage: 100<br/>
/// EPVV-PerNextPage: 100<br/>
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
    /// Уникальный идентификатор сообщения.<br/>
    /// Устанавливается сервером в ответном сообщении.<br/>
    /// Example: "6e16a6ad-018f-4136-a8c6-b088010899bc"
    /// </summary>
    string Id,

    /// <summary>
    /// Идентификатор корреляции сообщения.<br/>
    /// Example: null<br/>
    /// Example: "1f6158a2-a7a1-4e14-aace-af7a00f65145"
    /// </summary>
    string? CorrelationId,

    /// <summary>
    /// Идентификатор группы сообщений.<br/>
    /// Example: null<br/>
    /// Example: "a4e5902c-e961-47a3-9670-bd717bcc1749"
    /// </summary>
    string? GroupId,

    /// <summary>
    /// Тип сообщения исходящее (значение: outbox) или входящее (значение: inbox).<br/>
    /// Example: "inbox"  // нам входящие<br/>
    /// Example: "outbox" // наши исходящие
    /// </summary>
    string Type,

    /// <summary>
    /// Название сообщения (subject).<br/>
    /// Example: null<br/>
    /// Example: "N 20-2-1/1 от 10/01/2023 (20) Письма Деп-та денежно-кредитной политики"<br/>
    /// Example: "Ответ на запрос/предписание (требование)"<br/>
    /// Example: "Получение информации об уровне риска ЮЛ/ИП"<br/>
    /// Example: "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)"
    /// </summary>
    string? Title,

    /// <summary>
    /// Текст сообщения (body).<br/>
    /// Example: null<br/>
    /// Example: ""<br/>
    /// Example: "предоставление запрошенной информации"<br/>
    /// Example: "О данных для расчета размера обязательных резервов"
    /// </summary>
    string? Text,

    /// <summary>
    /// Дата создания сообщения (ГОСТ ISO 8601-2001 по маске «yyyy-MM-dd'T'HH:mm:ss'Z'») в UTC.<br/>
    /// Example: "2023-09-25T16:03:31Z"
    /// </summary>
    DateTime CreationDate,

    /// <summary>
    /// Дата последнего изменения статуса сообщения (ГОСТ ISO 8601-2001 по маске «yyyy-MM-dd'T'HH:mm:ss'Z'») в UTC.<br/>
    /// Example: null<br/>
    /// Example: "2023-01-09T13:18:19Z"
    /// </summary>
    DateTime? UpdatedDate,

    /// <summary>
    /// Статус сообщения (возможные значения и их описание находится в п.2.4).<br/>
    /// <br/>
    /// draft      Черновик: Сообщение с данным статусом создано, но ещё не отправлено.<br/>
    /// sent       Отправлено: Сообщение получено сервером.<br/>
    /// delivered  Загружено: Сообщение прошло первоначальную проверку.<br/>
    /// error      Ошибка: При обработке сообщения возникла ошибка.<br/>
    /// processing Принято в обработку: Сообщение передано во внутреннюю систему Банка России.<br/>
    /// registered Зарегистрировано: Сообщение зарегистрировано.<br/>
    /// rejected   Отклонено: Сообщение успешно дошло до получателя, но было отклонено.<br/>
    /// new        Новое: Только для входящих сообщений. Сообщение в данном статусе ещё не прочтено Пользователем УИО.<br/>
    /// read       Прочитано: Только для входящих сообщений. Сообщение в данном статусе прочтено Пользователем УИО.<br/>
    /// replied    Отправлен ответ: Только для входящих сообщений. На сообщение в данном статусе направлен ответ.<br/>
    /// success    Доставлено: Сообщение успешно размещено в ЛК/Сообщение передано роутером во внутреннюю систему<br/>
    ///            Банка России, от которой не ожидается ответ о регистрации.
    /// </summary>
    string Status,

    /// <summary>
    /// Наименование задачи.<br/>
    /// Example: "Zadacha_2-1"<br/>
    /// Example: "Zadacha_3-1"<br/>
    /// Example: "Zadacha_130"<br/>
    /// Example: "Zadacha_137"<br/>
    /// Example: "GroupTask_22"
    /// </summary>
    string TaskName,

    /// <summary>
    /// Регистрационный номер.<br/>
    /// Example: null<br/>
    /// Example: "20-2-1/1"
    /// </summary>
    string? RegNumber,

    /// <summary>
    /// Общий размер сообщения в байтах.<br/>
    /// Example: 3241554
    /// </summary>
    long? TotalSize,

    /// <summary>
    /// Отправитель сообщения (необязательное поле, только для сообщений, отправляемых другими Пользователями).<br/>
    /// Example: null<br/>
    /// Example: {<br/>
    /// "Inn": "7831001422",<br/>
    /// "Ogrn": "1027800000095",<br/>
    /// "Bik": "044030702",<br/>
    /// "RegNum": "3194",<br/>
    /// "DivisionCode": "0000"<br/>
    /// }
    /// </summary>
    Sender? Sender,

    /// <summary>
    /// Получатели сообщения (необязательно, указывается для потоков адресной рассылки).<br/>
    /// Example: null<br/>
    /// Example: [{<br/>
    /// "Inn": "7831001422",<br/>
    /// "Ogrn": "1027800000095",<br/>
    /// "Bik": "044030702",<br/>
    /// "RegNum": "3194",<br/>
    /// "DivisionCode": "0000"<br/>
    /// }, ...]
    /// </summary>
    IReadOnlyList<Receiver>? Receivers,

    /// <summary>
    /// Файлы включенные в сообщение.<br/>
    /// Example: [{<br/>
    /// "Id":"d55cdbbb-e41f-4a2a-8967-78e2a6e15701",<br/>
    /// "Name":"KYC_20230925.xml.zip.enc",<br/>
    /// "Description":null,<br/>
    /// "Encrypted":true,<br/>
    /// "SignedFile":null,<br/>
    /// "Size":3238155,<br/>
    /// "RepositoryInfo": [...]<br/>
    /// }, ...]
    /// </summary>
    IReadOnlyList<MessageFile> Files,

    /// <summary>
    /// Квитанции, полученные в ответ на сообщение.<br/>
    /// Example: []
    /// </summary>
    IReadOnlyList<MessageReceipt>? Receipts
)
{
    [JsonIgnore]
    public bool Inbox => Type.Equals(MessageType.Inbox, StringComparison.Ordinal);
    [JsonIgnore]
    public bool Outbox => Type.Equals(MessageType.Outbox, StringComparison.Ordinal);

    [JsonIgnore]
    public bool Registered => Status.Equals(MessageOutStatus.Registered, StringComparison.Ordinal);
    [JsonIgnore]
    public bool Success => Status.Equals(MessageOutStatus.Success, StringComparison.Ordinal);
}

/* GET https://portal5.cbr.ru/back/rapi2/messages
Header:
EPVV-Total: 3649
EPVV-TotalPages: 37
EPVV-CurrentPage: 1
EPVV-PerCurrentPage: 100
EPVV-PerNextPage: 100
*/

#region Mock
//public static class MockMessage
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="direction"></param>
//    /// <returns></returns>
//    /// <remarks>
//    /// 0 - inbox<br/>
//    /// 1 - outbox<br/>
//    /// 2 - bidirectional
//    /// </remarks>
//    public static string Text(int direction) => direction == 0 ?
//        """
//        {
//            "Id": "e72472be-3e25-4535-b874-3e28d382304a",
//            "CorrelationId": null,
//            "GroupId": null,
//            "Type": "inbox",
//            "Title": "Запрос ЦИК",
//            "Text": null,
//            "CreationDate": "2021-06-08T13:21:32Z",
//            "UpdatedDate": null,
//            "Status": "read",
//            "TaskName": "Zadacha_54",
//            "RegNumber": null,
//            "TotalSize": 772677,
//            "Sender": {
//                "Inn": "7710168307",
//                "Ogrn": "1037739236578",
//                "Bik": null,
//                "RegNum": null,
//                "DivisionCode": null
//            },
//            "Files": [
//                {
//                    "Id": "788c6442-8737-4d7c-bb12-10a4bc93564a",
//                    "Name": "F1027700466640_080621_Z_0021.xml.sig.enc",
//                    "Description": null,
//                    "Encrypted": true,
//                    "SignedFile": null,
//                    "Size": 769929,
//                    "RepositoryInfo": [
//                        {
//                            "RepositoryType": "http",
//                            "Host": "https://portal5.cbr.ru",
//                            "Port": 81,
//                            "Path": "back/rapi2/messages/e72472be-3e25-4535-b874-3e28d382304a/files/788c6442-8737-4d7c-bb12-10a4bc93564a/download"
//                        }
//                    ]
//                },
//                {
//                    "Id": "5adee2dc-ba0c-453f-b4a0-5fa3cf929823",
//                    "Name": "F1027700466640_080621_Z_0021.xml.sig.sig",
//                    "Description": null,
//                    "Encrypted": false,
//                    "SignedFile": "788c6442-8737-4d7c-bb12-10a4bc93564a",
//                    "Size": 2748,
//                    "RepositoryInfo": [
//                        {
//                            "RepositoryType": "http",
//                            "Host": "https://portal5.cbr.ru",
//                            "Port": 81,
//                            "Path": "back/rapi2/messages/e72472be-3e25-4535-b874-3e28d382304a/files/5adee2dc-ba0c-453f-b4a0-5fa3cf929823/download"
//                        }
//                    ]
//                }
//            ],
//            "Receipts": []
//        }
//        """
//        :
//        """
//        {
//            "Id": "4825614f-e641-498b-8670-af7d0128a289",
//            "CorrelationId": null,
//            "GroupId": null,
//            "Type": "outbox",
//            "Title": "Ежедневное информирование Банка России о составе и объеме клиентской базы (REST)",
//            "Text": null,
//            "CreationDate": "2023-01-01T18:00:00Z",
//            "UpdatedDate": "2023-01-01T18:03:18Z",
//            "Status": "registered",
//            "TaskName": "Zadacha_137",
//            "RegNumber": "17_001_001_00564752",
//            "TotalSize": 13369,
//            "Sender": {
//                "Inn": "7831001422",
//                "Ogrn": "1027800000095",
//                "Bik": "044030702",
//                "RegNum": "3194",
//                "DivisionCode": "0000"
//            },
//            "Files": [
//                {
//                    "Id": "719182ec-8d0f-4419-a4e2-af7d0128a285",
//                    "Name": "KYCCL_7831001422_3194_20230101_000001.zip.enc",
//                    "Description": null,
//                    "Encrypted": true,
//                    "SignedFile": null,
//                    "Size": 9497,
//                    "RepositoryInfo": [
//                        {
//                            "RepositoryType": "http",
//                            "Host": "https://portal5.cbr.ru",
//                            "Port": 81,
//                            "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/files/719182ec-8d0f-4419-a4e2-af7d0128a285/download"
//                        }
//                    ]
//                },
//                {
//                    "Id": "6548c720-2de4-442f-8edf-af7d0128a287",
//                    "Name": "KYCCL_7831001422_3194_20230101_000001.zip.sig",
//                    "Description": null,
//                    "Encrypted": false,
//                    "SignedFile": "719182ec-8d0f-4419-a4e2-af7d0128a285",
//                    "Size": 3872,
//                    "RepositoryInfo": [
//                        {
//                            "RepositoryType": "http",
//                            "Host": "https://portal5.cbr.ru",
//                            "Port": 81,
//                            "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/files/6548c720-2de4-442f-8edf-af7d0128a287/download"
//                        }
//                    ]
//                }
//            ],
//            "Receipts": [
//                {
//                    "Id": "2438f1b9-159d-41a1-8309-af7d0128a3b0",
//                    "ReceiveTime": "2023-01-01T18:00:01Z",
//                    "StatusTime": "2023-01-01T18:00:01Z",
//                    "Status": "sent",
//                    "Message": null,
//                    "Files": []
//                },
//                {
//                    "Id": "91595e36-555b-42c7-ad71-af7d0128b528",
//                    "ReceiveTime": "2023-01-01T18:00:16Z",
//                    "StatusTime": "2023-01-01T18:00:12Z",
//                    "Status": "delivered",
//                    "Message": null,
//                    "Files": [
//                        {
//                            "Id": "32138df1-2afc-41f4-ad89-09f30c51b41f",
//                            "Name": "ESODReceipt.xml",
//                            "Description": null,
//                            "Encrypted": false,
//                            "SignedFile": null,
//                            "Size": 1023,
//                            "RepositoryInfo": [
//                                {
//                                    "RepositoryType": "http",
//                                    "Host": "https://portal5.cbr.ru",
//                                    "Port": 81,
//                                    "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/91595e36-555b-42c7-ad71-af7d0128b528/files/32138df1-2afc-41f4-ad89-09f30c51b41f/download"
//                                }
//                            ]
//                        },
//                        {
//                            "Id": "2bfcf7b2-ed5c-43f0-9fc2-27a68f03fdbd",
//                            "Name": "status.xml.sig",
//                            "Description": null,
//                            "Encrypted": false,
//                            "SignedFile": "21d3ec52-a0d1-4aa0-aee5-754b2c269433",
//                            "Size": 3261,
//                            "RepositoryInfo": [
//                                {
//                                    "RepositoryType": "http",
//                                    "Host": "https://portal5.cbr.ru",
//                                    "Port": 81,
//                                    "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/91595e36-555b-42c7-ad71-af7d0128b528/files/2bfcf7b2-ed5c-43f0-9fc2-27a68f03fdbd/download"
//                                }
//                            ]
//                        },
//                        {
//                            "Id": "21d3ec52-a0d1-4aa0-aee5-754b2c269433",
//                            "Name": "status.xml",
//                            "Description": null,
//                            "Encrypted": false,
//                            "SignedFile": null,
//                            "Size": 319,
//                            "RepositoryInfo": [
//                                {
//                                    "RepositoryType": "http",
//                                    "Host": "https://portal5.cbr.ru",
//                                    "Port": 81,
//                                    "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/91595e36-555b-42c7-ad71-af7d0128b528/files/21d3ec52-a0d1-4aa0-aee5-754b2c269433/download"
//                                }
//                            ]
//                        },
//                        {
//                            "Id": "15734f6f-2912-4e15-b609-9804d1a18ad2",
//                            "Name": "ESODReceipt.xml.sig",
//                            "Description": null,
//                            "Encrypted": false,
//                            "SignedFile": "32138df1-2afc-41f4-ad89-09f30c51b41f",
//                            "Size": 3261,
//                            "RepositoryInfo": [
//                                {
//                                    "RepositoryType": "http",
//                                    "Host": "https://portal5.cbr.ru",
//                                    "Port": 81,
//                                    "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/91595e36-555b-42c7-ad71-af7d0128b528/files/15734f6f-2912-4e15-b609-9804d1a18ad2/download"
//                                }
//                            ]
//                        }
//                    ]
//                },
//                {
//                    "Id": "3dcda650-f9c9-4534-b78f-af7d0128bd98",
//                    "ReceiveTime": "2023-01-01T18:00:23Z",
//                    "StatusTime": "2023-01-01T18:00:16Z",
//                    "Status": "processing",
//                    "Message": null,
//                    "Files": [
//                        {
//                            "Id": "79e13183-a736-4575-8e2a-5559e137ffae",
//                            "Name": "ESODReceipt.xml",
//                            "Description": null,
//                            "Encrypted": false,
//                            "SignedFile": null,
//                            "Size": 1024,
//                            "RepositoryInfo": [
//                                {
//                                    "RepositoryType": "http",
//                                    "Host": "https://portal5.cbr.ru",
//                                    "Port": 81,
//                                    "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/3dcda650-f9c9-4534-b78f-af7d0128bd98/files/79e13183-a736-4575-8e2a-5559e137ffae/download"
//                                }
//                            ]
//                        },
//                        {
//                            "Id": "6464c56f-b0f7-4efb-9523-885c3e7b8f0b",
//                            "Name": "status.xml",
//                            "Description": null,
//                            "Encrypted": false,
//                            "SignedFile": null,
//                            "Size": 320,
//                            "RepositoryInfo": [
//                                {
//                                    "RepositoryType": "http",
//                                    "Host": "https://portal5.cbr.ru",
//                                    "Port": 81,
//                                    "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/3dcda650-f9c9-4534-b78f-af7d0128bd98/files/6464c56f-b0f7-4efb-9523-885c3e7b8f0b/download"
//                                }
//                            ]
//                        },
//                        {
//                            "Id": "60412d72-df08-4820-b179-af5a2ee4bbeb",
//                            "Name": "status.xml.sig",
//                            "Description": null,
//                            "Encrypted": false,
//                            "SignedFile": "6464c56f-b0f7-4efb-9523-885c3e7b8f0b",
//                            "Size": 3261,
//                            "RepositoryInfo": [
//                                {
//                                    "RepositoryType": "http",
//                                    "Host": "https://portal5.cbr.ru",
//                                    "Port": 81,
//                                    "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/3dcda650-f9c9-4534-b78f-af7d0128bd98/files/60412d72-df08-4820-b179-af5a2ee4bbeb/download"
//                                }
//                            ]
//                        },
//                        {
//                            "Id": "82bd7cb5-0307-4d2b-b735-d4ed49e2b0dd",
//                            "Name": "ESODReceipt.xml.sig",
//                            "Description": null,
//                            "Encrypted": false,
//                            "SignedFile": "79e13183-a736-4575-8e2a-5559e137ffae",
//                            "Size": 3261,
//                            "RepositoryInfo": [
//                                {
//                                    "RepositoryType": "http",
//                                    "Host": "https://portal5.cbr.ru",
//                                    "Port": 81,
//                                    "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/3dcda650-f9c9-4534-b78f-af7d0128bd98/files/82bd7cb5-0307-4d2b-b735-d4ed49e2b0dd/download"
//                                }
//                            ]
//                        }
//                    ]
//                },
//                {
//                    "Id": "6f4ca825-3968-4e20-a6ee-af7d01298a2f",
//                    "ReceiveTime": "2023-01-01T18:03:18Z",
//                    "StatusTime": "2023-01-01T18:02:27Z",
//                    "Status": "registered",
//                    "Message": null,
//                    "Files": [
//                        {
//                            "Id": "cf0869cd-7e34-464f-847c-2b2dfab3ab39",
//                            "Name": "status.xml",
//                            "Description": null,
//                            "Encrypted": false,
//                            "SignedFile": null,
//                            "Size": 378,
//                            "RepositoryInfo": [
//                                {
//                                    "RepositoryType": "http",
//                                    "Host": "https://portal5.cbr.ru",
//                                    "Port": 81,
//                                    "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/6f4ca825-3968-4e20-a6ee-af7d01298a2f/files/cf0869cd-7e34-464f-847c-2b2dfab3ab39/download"
//                                }
//                            ]
//                        },
//                        {
//                            "Id": "59f25e7f-85bf-4ec2-9c24-69328ce84201",
//                            "Name": "status.xml.sig",
//                            "Description": null,
//                            "Encrypted": false,
//                            "SignedFile": "cf0869cd-7e34-464f-847c-2b2dfab3ab39",
//                            "Size": 3261,
//                            "RepositoryInfo": [
//                                {
//                                    "RepositoryType": "http",
//                                    "Host": "https://portal5.cbr.ru",
//                                    "Port": 81,
//                                    "Path": "back/rapi2/messages/4825614f-e641-498b-8670-af7d0128a289/receipts/6f4ca825-3968-4e20-a6ee-af7d01298a2f/files/59f25e7f-85bf-4ec2-9c24-69328ce84201/download"
//                                }
//                            ]
//                        }
//                    ]
//                }
//            ]
//        }
//        """;
//}
#endregion
