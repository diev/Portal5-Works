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

using Diev.Portal5.API.Dictionaries;
using Diev.Portal5.API.Info;
using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Messages.Create;
using Diev.Portal5.API.Tools;

namespace Diev.Portal5.API.Interfaces;

/// <summary>
/// https://www.cbr.ru/lk_uio/guide/rest_api/ <br/>
/// 30.09.2023 Описание внешнего взаимодействия. Технические условия внешнего обмена. Версия 2.4 <br/>
/// ES_transmission_24.docx
/// </summary>
public interface IRestAPICore
{
    /* 3.1.3. Отправка сообщений */

    // 3.1.3.1. Для создания нового сообщения используется метод POST
    Task<ApiResult<Message>> PostMessageRequestAsync(DraftMessage message);
    // 3.1.3.2. Для создания сессии отправки HTTP используется метод POST
    Task<ApiResult<UploadSession>> PostUploadRequestAsync(string msgId, string fileId);
    // 3.1.3.3. Для отправки файла по HTTP используется метод PUT
    Task<ApiResult<bool>> UploadFileAsync(string path, long size, string url);
    // 3.1.3.4. Для подтверждения отправки сообщения используется метод POST
    Task<ApiResult<bool>> PostMessageAsync(string msgId);


    /* 3.1.4. Получение УИО сообщений, квитанций, файлов и информации. */

    // 3.1.4.1. Для получения всех сообщений с учетом необязательного фильтра(не более 100 сообщений за один запрос) используется метод GET.
    Task<ApiResult<MessagesPage>> GetMessagesPageAsync(MessagesFilter filter);
    // 3.1.4.2. Для получения данных о конкретном сообщении используется метод GET
    Task<ApiResult<Message>> GetMessageAsync(string msgId);
    // 3.1.4.3. Для скачивания конкретного сообщения используется метод GET
    Task<ApiResult<bool>> DownloadMessageZipAsync(string msgId, string path, bool overwrite = false);
    // 3.1.4.4. Для получения данных о конкретном файле используется метод GET
    Task<ApiResult<MessageFile>> GetMessageFileAsync(string msgId, string fileId);
    // 3.1.4.5. Для скачивания конкретного файла из конкретного сообщения используется метод GET
    Task<ApiResult<bool>> DownloadMessageFileAsync(string msgId, string fileId, string path, bool overwrite = false);
    // 3.1.4.6. Для получения данных о квитанциях на сообщение используется метод GET
    Task<ApiResult<IReadOnlyList<MessageReceipt>>> GetMessageReceiptsAsync(string msgId);
    // 3.1.4.7. Для получения данных о квитанции на сообщение используется метод GET
    Task<ApiResult<MessageReceipt>> GetMessageReceiptAsync(string msgId, string rcptId);
    // 3.1.4.8. Для получения данных о файле квитанции на сообщение используется метод GET
    Task<ApiResult<MessageFile>> GetMessageReceiptFileAsync(string msgId, string rcptId, string fileId);
    // 3.1.4.9. Для скачивания файла квитанции на сообщение используется метод GET
    Task<ApiResult<bool>> DownloadMessageReceiptFileAsync(string msgId, string rcptId, string fileId, string path, bool overwrite = false);


    /* 3.1.5. Удаление сообщений */

    // 3.1.5.1. Для удаления конкретного сообщения используется метод DELETE
    Task<ApiResult<bool>> DeleteMessageAsync(string msgId);
    // 3.1.5.2. Для удаления конкретного файла или отмены сессии отправки используется метод DELETE
    Task<ApiResult<bool>> DeleteMessageFileAsync(string msgId, string fileId);


    /* 3.1.6. Получение справочной информации */

    // 3.1.6.1. Для получения справочника задач используется метод GET
    Task<ApiResult<Tasks>> GetTasksAsync(int? direction = null);
    // 3.1.6.2. Для получения информации о своём профиле используется метод GET
    Task<ApiResult<Profile>> GetProfileAsync();
    // 3.1.6.3. Для получения информации о квоте профиля используется метод GET
    Task<ApiResult<Quota>> GetQuotaAsync();
    // 3.1.6.4. Для получения информации о технических оповещениях используется метод GET
    Task<ApiResult<IReadOnlyList<Notification>>> GetNotificationsAsync();
    // 3.1.6.5. Для получения списка справочников используется метод GET
    Task<ApiResult<DictItems>> GetLevelsPageAsync();
    // 3.1.6.6. Для получения записей конкретного справочника 1, но не более 100 записей за один запрос, используется метод GET
    Task<ApiResult<Level1ItemsPage>> GetLevels1PageAsync(int page = 1, string dictId = "238d0426-6f57-4c0f-8983-1d1addf8c47a");
    // 3.1.6.6. Для получения записей конкретного справочника 2, но не более 100 записей за один запрос, используется метод GET
    Task<ApiResult<Level2ItemsPage>> GetLevels2PageAsync(int page = 1, string dictId = "25338cfb-5713-4634-bc53-a81129483752");
    // 3.1.6.6. Для получения записей конкретного справочника 3, но не более 100 записей за один запрос, используется метод GET
    Task<ApiResult<Level3ItemsPage>> GetLevels3PageAsync(int page = 1, string dictId = "64529d5a-b1d9-453c-96f3-f380ea577314");
    // 3.1.6.7. Для скачивания конкретного справочника в виде файла используется метод GET
    Task<ApiResult<bool>> DownloadLevelsFileAsync(string dictId, string path, bool overwrite = false);


    /* 3.1.7. Взаимодействие с ХМЧД */

    //Описанные в п.п.3.1.7.1 - 3.1.7.16 методы доступны только в версии v2.

    // 3.1.7.1. Для создания нового запроса в ХМЧД используется метод POST
    // 3.1.7.2. Для создания сессии отправки HTTP используется метод POST
    // 3.1.7.3. Для отправки файла по HTTP используется метод PUT
    // 3.1.7.4. Для отправки запроса в ХМЧД используется метод POST
    // 3.1.7.5. Для проверки возможности удаления запроса в Истории запросов используется метод POST
    // 3.1.7.6. Для удаления запроса в Истории запросов используется метод DELETE
    // 3.1.7.7. Для получения запросов из Истории запросов используется метод GET
    // 3.1.7.8. Для получения списка загруженных МЧД в ХМЧД используется метод GET
    // 3.1.7.9. Для получения квитанций запроса в ХМЧД используется метод GET
    // 3.1.7.10. Для получения информации о результате регистрации МЧД в ХМЧД используется метод GET
    // 3.1.7.11. Для получения информации о запросе в ХМЧД используется метод GET
    // 3.1.7.12. Для скачивания файла запроса в ХМЧД (с квитанциями) используется метод GET
    // 3.1.7.13. Для удаления квитанций запроса в ХМЧД используется метод DELETE
    // 3.1.7.14. Для простановки признака скачивания МЧД из ХМЧД используется метод PUT
    // 3.1.7.15. Для удаления конкретного файла или отмены сессии отправки используется метод DELETE
    // 3.1.7.16. Для скачивания файла запроса (без квитанций) используется метод GET


    /* 3.2. Взаимодействие с использованием сервиса REST-УТА (СПО УТА). */

    // Инициатором электронного обмена может быть как КО, так и Банк России.
    // Прием информации от КО должен осуществляться Порталом "Биврёст"
    // с использованием REST-сервиса. В качестве транспортного адаптера
    // при этом должно использоваться специальное программное обеспечение
    // файлового взаимодействия Банка России (СПО УТА).
}
