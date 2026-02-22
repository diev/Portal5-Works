# Portal5 REST API (static)

Интерфейс построен на основании документации по ссылке  
<https://www.cbr.ru/lk_uio/guide/rest_api/>  
30.09.2023 Описание внешнего взаимодействия. Технические условия внешнего обмена. Версия 2.4  
(ES_transmission_24.docx)


## 3.1.3. Отправка сообщений

    // 3.1.3.1. Для создания нового сообщения используется метод POST
    public Task<Message?> PostMessageRequestAsync(DraftMessage message);

    // 3.1.3.2. Для создания сессии отправки HTTP используется метод POST
    public Task<UploadSession?> PostUploadRequestAsync(string msgId, string fileId);

    // 3.1.3.3. Для отправки файла по HTTP используется метод PUT
    public Task<bool> UploadFileAsync(string path, long size, string url);

    // 3.1.3.4. Для подтверждения отправки сообщения используется метод POST
    public Task<bool> PostMessageAsync(string msgId);


## 3.1.4. Получение УИО сообщений, квитанций, файлов и информации

    // 3.1.4.1. Для получения всех сообщений с учетом необязательного фильтра (не более 100 сообщений за один запрос) используется метод GET.
    public Task<MessagesPage?> GetMessagesPageAsync(MessagesFilter filter);

    // 3.1.4.2. Для получения данных о конкретном сообщении используется метод GET
    public Task<Message?> GetMessageAsync(string msgId);

    // 3.1.4.3. Для скачивания конкретного сообщения используется метод GET
    public Task<bool> DownloadMessageZipAsync(string msgId, string path, bool overwrite = false);

    // 3.1.4.4. Для получения данных о конкретном файле используется метод GET
    public Task<MessageFile?> GetMessageFileAsync(string msgId, string fileId);

    // 3.1.4.5. Для скачивания конкретного файла из конкретного сообщения используется метод GET
    public Task<bool> DownloadMessageFileAsync(string msgId, string fileId, string path, bool overwrite = false);

    // 3.1.4.6. Для получения данных о квитанциях на сообщение используется метод GET
    public Task<IReadOnlyList<MessageReceipt>?> GetMessageReceiptsAsync(string msgId);

    // 3.1.4.7. Для получения данных о квитанции на сообщение используется метод GET
    public Task<MessageReceipt?> GetMessageReceiptAsync(string msgId, string rcptId);

    // 3.1.4.8. Для получения данных о файле квитанции на сообщение используется метод GET
    public Task<MessageFile?> GetMessageReceiptFileAsync(string msgId, string rcptId, string fileId);

    // 3.1.4.9. Для скачивания файла квитанции на сообщение используется метод GET
    public Task<bool> DownloadMessageReceiptFileAsync(string msgId, string rcptId, string fileId, string path, bool overwrite = false);


## 3.1.5. Удаление сообщений

    // 3.1.5.1. Для удаления конкретного сообщения используется метод DELETE
    public Task<bool> DeleteMessageAsync(string msgId);

    // 3.1.5.2. Для удаления конкретного файла или отмены сессии отправки используется метод DELETE
    public Task<bool> DeleteMessageFileAsync(string msgId, string fileId);


## 3.1.6. Получение справочной информации

    // 3.1.6.1. Для получения справочника задач используется метод GET
    public Task<Tasks?> GetTasksAsync(int? direction = null);

    // 3.1.6.2. Для получения информации о своём профиле используется метод GET
    public Task<Profile?> GetProfileAsync();

    // 3.1.6.3. Для получения информации о квоте профиля используется метод GET
    public Task<Quota?> GetQuotaAsync();

    // 3.1.6.4. Для получения информации о технических оповещениях используется метод GET
    public Task<IReadOnlyList<Notification>?> GetNotificationsAsync();

    // 3.1.6.5. Для получения списка справочников используется метод GET
    public Task<DictItems?> GetLevelsPageAsync();

    // 3.1.6.6. Для получения записей конкретного справочника 1, но не более 100 записей за один запрос, используется метод GET
    public Task<Level1ItemsPage?> GetLevels1PageAsync(int page = 1, string dictId = "238d0426-6f57-4c0f-8983-1d1addf8c47a");

    // 3.1.6.6. Для получения записей конкретного справочника 2, но не более 100 записей за один запрос, используется метод GET
    public Task<Level2ItemsPage?> GetLevels2PageAsync(int page = 1, string dictId = "25338cfb-5713-4634-bc53-a81129483752");

    // 3.1.6.6. Для получения записей конкретного справочника 3, но не более 100 записей за один запрос, используется метод GET
    public Task<Level3ItemsPage?> GetLevels3PageAsync(int page = 1, string dictId = "64529d5a-b1d9-453c-96f3-f380ea577314");

    // 3.1.6.7. Для скачивания конкретного справочника в виде файла используется метод GET
    public Task<bool> DownloadLevelsFileAsync(string dictId, string path, bool overwrite = false);


## 3.1.7. Взаимодействие с ХМЧД

Описанные в п.п.3.1.7.1 - 3.1.7.16 методы доступны только в версии v2.

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


## 3.2. Взаимодействие с использованием сервиса REST-УТА (СПО УТА)

Инициатором электронного обмена может быть как КО, так и Банк России.
Прием информации от КО должен осуществляться Порталом "Биврёст"
с использованием REST-сервиса. В качестве транспортного адаптера
при этом должно использоваться специальное программное обеспечение
файлового взаимодействия Банка России (СПО УТА).

Если вам нужна функциональность СПО УТА, то посмотрите проект
<https://github.com/diev/SVK-Transport-hta>


## License / Лицензия

Licensed under the [Apache License, Version 2.0](LICENSE).  
Вы можете использовать эти материалы под свою ответственность.


[![Telegram](https://img.shields.io/badge/t.me-dievdo-blue?logo=telegram)](https://t.me/dievdo)
