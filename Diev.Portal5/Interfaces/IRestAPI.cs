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

using Diev.Portal5.API;

namespace Diev.Portal5.Interfaces;

/// <summary>
/// ES_transmission_24 (30.09.2023 Описание внешнего взаимодействия. Технические условия внешнего обмена).docx
/// </summary>
public interface IRestAPI
{
    /* 3.1.3 Отправка сообщений */

    // 3.1.3.1 Для создания нового сообщения используется метод POST
    public Task<Message> PostMessageRequestAsync(Message message);
    // 3.1.3.2 Для создания сессии отправки HTTP используется метод POST
    public Task<UploadInstruction> PostUploadRequestAsync(string messageId, string fileId);
    // 3.1.3.3 Для отправки файла по HTTP используется метод PUT
    public Task UploadFileAsync(string path, long size, string url);
    // 3.1.3.4 Для подтверждения отправки сообщения используется метод POST
    public Task PostMessageAsync(string messageId);


    /* 3.1.4 Получение УИО сообщений, квитанций, файлов и информации. */

    // 3.1.4.1 Для получения всех сообщений с учетом необязательного фильтра(не более 100 сообщений за один запрос) используется метод GET.
    public Task<MessagePages> GetMessagePagesAsync(MessageFilter filter, int page = 1);
    // 3.1.4.2 Для получения данных о конкретном сообщении используется метод GET
    public Task<Message> GetMessage(string messageId);
    // 3.1.4.3 Для скачивания конкретного сообщения используется метод GET
    public Task DownloadMessageAsync(string messageId, string path, bool overwrite = false);
    // 3.1.4.4 Для получения данных о конкретном файле используется метод GET
    public Task<MessageFile> GetMessageFileInfo(string messageId, string fileId);
    // 3.1.4.5 Для скачивания конкретного файла из конкретного сообщения используется метод GET
    public Task DownloadMessageFileAsync(string messageId, string fileId, string path, bool overwrite = false);
    // 3.1.4.6 Для получения данных о квитанциях на сообщение используется метод GET
    public Task<Receipts> GetReceiptInfo(string messageId);
    // 3.1.4.7 Для получения данных о квитанции на сообщение используется метод GET
    public Task<Receipts> GetReceiptInfo(string messageId, string receiptId);
    // 3.1.4.8 Для получения данных о файле квитанции на сообщение используется метод GET
    public Task<MessageFile> GetReceiptFileInfo(string messageId, string receiptId, string fileId);
    // 3.1.4.9 Для скачивания файла квитанции на сообщение используется метод GET
    public Task DownloadReceiptFileAsync(string messageId, string receiptId, string fileId, string path, bool overwrite = false);


    /* 3.1.5 Удаление сообщений */

    // 3.1.5.1 Для удаления конкретного сообщения используется метод DELETE
    public Task DeleteMessageAsync(string messageId);
    // 3.1.5.2 Для удаления конкретного файла или отмены сессии отправки используется метод DELETE
    public Task DeleteMessageFileAsync(string messageId, string fileId);


    /* 3.1.6 Получение справочной информации */

    // 3.1.6.1 Для получения справочника задач используется метод GET
    public Task<Tasks> GetTasksAsync(int? direction = null);
    // 3.1.6.2 Для получения информации о своём профиле используется метод GET
    public Task<Tasks> GetProfileInfoAsync();
    // 3.1.6.3 Для получения информации о квоте профиля используется метод GET
    public Task<Quota> GetQuotaAsync();
    // 3.1.6.4 Для получения информации о технических оповещениях используется метод GET
    public Task<List<Notification>> GetNotificationsAsync();
    // 3.1.6.5 Для получения списка справочников используется метод GET
    public Task<Levels> GetLevelsAsync(int page = 1);
    // 3.1.6.6 Для получения записей конкретного справочника 1, но не более 100 записей за один запрос, используется метод GET
    public Task<Levels1> GetLevel1Async(int page = 1, string guid = "238d0426-6f57-4c0f-8983-1d1addf8c47a");
    // 3.1.6.6 Для получения записей конкретного справочника 2, но не более 100 записей за один запрос, используется метод GET
    public Task<Levels2> GetLevel2Async(int page = 1, string guid = "25338cfb-5713-4634-bc53-a81129483752");
    // 3.1.6.6 Для получения записей конкретного справочника 3, но не более 100 записей за один запрос, используется метод GET
    public Task<Levels3> GetLevel3Async(int page = 1, string guid = "64529d5a-b1d9-453c-96f3-f380ea577314");
    // 3.1.6.7 Для скачивания конкретного справочника в виде файла используется метод GET
    public Task DownloadLevelFileAsync(string guid, string path, bool overwrite = false);


    /* 3.1.7 Взаимодействие с ХМЧД */

    // Описанные в п.п.3.1.7.1 - 3.1.7.16 методы доступны только в версии v2.


    /* 3.2 Взаимодействие с использованием сервиса REST-УТА (СПО УТА). */
}
