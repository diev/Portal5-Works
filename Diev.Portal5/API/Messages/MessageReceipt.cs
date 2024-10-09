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

namespace Diev.Portal5.API.Messages;

/// <summary>
/// Квитанции, полученные в ответ на сообщение.
/// </summary>
public record MessageReceipt
(
    /// <summary>
    /// Уникальный идентификатор квитанции.
    /// Устанавливается сервером в ответном сообщении.
    /// Example: "6e16a6ad-018f-4136-a8c6-b088010899bc"
    /// </summary>
    string Id,

    /// <summary>
    /// Время получения квитанции.
    /// </summary>
    string ReceiveTime,

    /// <summary>
    /// Время из самой квитанции.
    /// </summary>
    string StatusTime,

    /// <summary>
    /// Состояние обработки сообщения (возможные значения и их описание находится в п.2.4):
    /// 
    /// delivered  Загружено: Сообщение прошло первоначальную проверку.
    ///            В рамках 5361-У, квитанцией о загрузке считается эта квитанция.
    /// error      Ошибка: При обработке сообщения возникла ошибка.
    /// processing Принято в обработку: Сообщение передано во внутреннюю систему ЦБ.
    /// registered Зарегистрировано: Сообщение зарегистрировано.
    ///            В рамках 5361-У, квитанцией о регистрации считается эта квитанция.
    /// rejected   Отклонено: Сообщение успешно дошло до получателя, но было отклонено.
    /// new        Новое: Только для входящих сообщений. Сообщение в данном статусе ещё не почтено Пользователем УИО.
    /// read       Прочитано: Только для входящих сообщений. Сообщение в данном статусе почтено Пользователем УИО.
    /// replied    Отправлен ответ: Только для входящих сообщений. На сообщение в данном статусе направлен ответ.
    /// success    Доставлено: Сообщение успешно размещено в ЛК/Сообщение передано роутером во внутреннюю систему
    ///            Банка России, от которой не ожидается ответ о регистрации.
    /// </summary>
    string Status,

    /// <summary>
    /// Дополнительная информация из квитанции.
    /// </summary>
    string? Message,

    /// <summary>
    /// Файлы, включенные в квитанцию.
    /// </summary>
    IReadOnlyList<MessageFile> Files
);

/*
[
    {
        "Id": "9b11c093-e87e-4e7c-820a-b0dd012b809d",
        "ReceiveTime": "2023-12-19T18:10:27Z",
        "StatusTime": "2023-12-19T18:10:27Z",
        "Status": "sent",
        "Message": null,
        "Files": []
    },
    {
        "Id": "684ec863-b79a-4246-906e-b0dd012b8d76",
        "ReceiveTime": "2023-12-19T18:10:38Z",
        "StatusTime": "2023-12-19T18:10:34Z",
        "Status": "delivered",
        "Message": null,
        "Files": [
            {
                "Id": "0bf423b2-a971-46f7-ac40-23d1aa7eb426",
                "Name": "status.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 314,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/684ec863-b79a-4246-906e-b0dd012b8d76/files/0bf423b2-a971-46f7-ac40-23d1aa7eb426/download"
                    }
                ]
            },
            {
                "Id": "4e3daee4-7cf6-443b-a8e2-73028a64489f",
                "Name": "ESODReceipt.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "74b0a0d5-3255-431f-bad8-8ac885a5eaf3",
                "Size": 3399,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/684ec863-b79a-4246-906e-b0dd012b8d76/files/4e3daee4-7cf6-443b-a8e2-73028a64489f/download"
                    }
                ]
            },
            {
                "Id": "74b0a0d5-3255-431f-bad8-8ac885a5eaf3",
                "Name": "ESODReceipt.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 1002,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/684ec863-b79a-4246-906e-b0dd012b8d76/files/74b0a0d5-3255-431f-bad8-8ac885a5eaf3/download"
                    }
                ]
            },
            {
                "Id": "b8d7dc71-790a-4e83-91f1-c3a757dd90ac",
                "Name": "status.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "0bf423b2-a971-46f7-ac40-23d1aa7eb426",
                "Size": 3399,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/684ec863-b79a-4246-906e-b0dd012b8d76/files/b8d7dc71-790a-4e83-91f1-c3a757dd90ac/download"
                    }
                ]
            }
        ]
    },
    {
        "Id": "77b3d6e0-2e82-4267-8a55-b0dd012b91d4",
        "ReceiveTime": "2023-12-19T18:10:41Z",
        "StatusTime": "2023-12-19T18:10:37Z",
        "Status": "processing",
        "Message": null,
        "Files": [
            {
                "Id": "315c3faf-bbfd-49f9-b6f3-06612d03c3c7",
                "Name": "ESODReceipt.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 1003,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/77b3d6e0-2e82-4267-8a55-b0dd012b91d4/files/315c3faf-bbfd-49f9-b6f3-06612d03c3c7/download"
                    }
                ]
            },
            {
                "Id": "9f78bc59-41cf-4532-904e-265daa82fb57",
                "Name": "ESODReceipt.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "315c3faf-bbfd-49f9-b6f3-06612d03c3c7",
                "Size": 3399,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/77b3d6e0-2e82-4267-8a55-b0dd012b91d4/files/9f78bc59-41cf-4532-904e-265daa82fb57/download"
                    }
                ]
            },
            {
                "Id": "eb0fc523-8e3d-496d-83a9-c6b7ec2649a6",
                "Name": "status.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "37dadc78-9ef6-4463-a1a7-d5a48144aec1",
                "Size": 3399,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/77b3d6e0-2e82-4267-8a55-b0dd012b91d4/files/eb0fc523-8e3d-496d-83a9-c6b7ec2649a6/download"
                    }
                ]
            },
            {
                "Id": "37dadc78-9ef6-4463-a1a7-d5a48144aec1",
                "Name": "status.xml",
                "Description": null,
                "Encrypted": false,
                "SignedFile": null,
                "Size": 315,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/77b3d6e0-2e82-4267-8a55-b0dd012b91d4/files/37dadc78-9ef6-4463-a1a7-d5a48144aec1/download"
                    }
                ]
            }
        ]
    },
    {
        "Id": "54a8fde1-9f7d-413e-8a68-b0dd012bc8ee",
        "ReceiveTime": "2023-12-19T18:11:29Z",
        "StatusTime": "2023-12-19T18:11:18Z",
        "Status": "registered",
        "Message": null,
        "Files": [
            {
                "Id": "7983548c-cedb-4df4-8268-8e3d06869b65",
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
                        "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/54a8fde1-9f7d-413e-8a68-b0dd012bc8ee/files/7983548c-cedb-4df4-8268-8e3d06869b65/download"
                    }
                ]
            },
            {
                "Id": "24111f98-0749-42df-818d-e176f7ca8c6c",
                "Name": "status.xml.sig",
                "Description": null,
                "Encrypted": false,
                "SignedFile": "7983548c-cedb-4df4-8268-8e3d06869b65",
                "Size": 3399,
                "RepositoryInfo": [
                    {
                        "RepositoryType": "http",
                        "Host": "https://portal5.cbr.ru",
                        "Port": 81,
                        "Path": "back/rapi2/messages/4f2ba6c4-0d9a-4ad6-834b-b0dd012b7f6e/receipts/54a8fde1-9f7d-413e-8a68-b0dd012bc8ee/files/24111f98-0749-42df-818d-e176f7ca8c6c/download"
                    }
                ]
            }
        ]
    }
]
*/
