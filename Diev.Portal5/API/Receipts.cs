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
/// Квитанции, полученные в ответ на сообщение.
/// </summary>
public class Receipts
{
    /// <summary>
    /// Уникальный идентификатор квитанции.
    /// Устанавливается сервером в ответном сообщении.
    /// Example: "6e16a6ad-018f-4136-a8c6-b088010899bc"
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Время получения квитанции.
    /// </summary>
    public string ReceiveTime { get; set; }

    /// <summary>
    /// Время из самой квитанции.
    /// </summary>
    public string StatusTime { get; set; }

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
    /// new        Новое: Только для входящих сообщений.Сообщение в данном статусе ещё не почтено Пользователем УИО.
    /// read       Прочитано: Только для входящих сообщений.Сообщение в данном статусе почтено Пользователем УИО.
    /// replied    Отправлен ответ: Только для входящих сообщений.На сообщение в данном статусе направлен ответ.
    /// success    Доставлено: Сообщение успешно размещено в ЛК/Сообщение передано роутером во внутреннюю систему
    ///            Банка России, от которой не ожидается ответ о регистрации.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Дополнительная информация из квитанции.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Файлы, включенные в квитанцию.
    /// </summary>
    public Files Files { get; set; }
}

/*
[
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
*/
