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

namespace Diev.Portal5.API.Errors;

/// <summary>
/// В случае ошибок REST методов из класса HTTP 4XX, в теле ответа передается JSON-объект с описанием ошибки вида.
/// </summary>
public record Error4XX
(
    /// <summary>
    /// HTTP статус класса 4xx согласно Hypertext Transfer Protocol (HTTP) Status Code Registry.
    /// </summary>
    int HTTPStatus,

    /// <summary>
    /// Внутренний код ошибки Портала. Служит клиенту для автоматизированной обработки ошибок. 
    /// </summary>
    string ErrorCode,

    /// <summary>
    /// Расшифровка ошибки. Служит для человеко-читаемой обработки ошибок.
    /// </summary>
    string ErrorMessage

    /// <summary>
    /// Объект с дополнительно информацией к ошибке, по-умолчанию пустой.
    /// HTTP 404 – Not found
    /// HTTP 410 – Gone
    /// HTTP 413 – Message size too large
    /// </summary>
    //object? MoreInfo
);

/*
"MoreInfo": {
  "MissedFiles": [
    {
      "Id": "string",
      "FileName": "string",
      "RepositoriInfo":
        "RepositoryInfo": {...}
    },
    {
      "Id": "string",
      "FileName": "string"
    }
  ]
}

"MoreInfo": {
  "AccountQuota": "integer",
  "RestOfQuota": "integer",
  "MessageQuota": "integer"
}
*/

/*
HTTP STATUS
    EPVV_ERROR
    TEXT
    Запрос

400 COMMON_ERROR
    Общая ошибка запроса
    GET /messages

400 CONTENT_LENGTH_INCORRECT
    Параметр content-length не соответствует размеру входящих данных
    PUT /messages/{msgId}/files/{fileId}

400 CONTENT_LENGTH_NOT_SET
    Не указан параметр content-length
    PUT /messages/{msgId}/files/{fileId}

400 CONTENT_RANGE_INCORRECT
    Не указан или указан неверно параметр content-range
    PUT /messages/{msgId}/files/{fileId}

400 DATA_ALLREADY_WRITTEN
    Данные уже записаны
    PUT /messages/{msgId}/files/{fileId}

400 FILE_ALLREADY_LOADED
    Файл уже загружен
    PUT  /messages/{msgId}/files/{fileId}
    POST /messages/{msgId}/files/{fileId}/createUploadSession
    POST /poa/CreateUploadSession/{msgId}/files/{fileId}/createUploadSession

400 DATA_RANGE_SAVE_ERROR
    Ошибка сохранения участка данных
    PUT /messages/{msgId}/files/{fileId}

400 DATA_UNREADABLE
    Не удалось прочитать данные, убедитесь что контент задан корректно!
    PUT /messages/{msgId}/files/{fileId}

400 DIRECTION_INCORRECT
    Неверное значение параметра direction (допустимо 0,1,2)
    GET /tasks

400 DICTIONARY_DATA_ERROR
    Ошибка при получении данных справочника {dictid}
    GET /dictionaries/{dictId}/download

400 REQUEST_AUTHOR_NOT_SET
    Не удалось извлечь автора запроса
    DELETE /messages/{msgId}

400 REQUEST_PLAYLOD_INCORRECT
    Неправильное тело запроса
    POST /messages
    POST /poa/CreateMessage

400 INCORRECT_PAGE_NUM
    Произошла ошибка. Некорректное значение страницы: {page}
    GET /dictionaries/{dictId}
    GET /messages

400 INCORRECT_REQUEST_PARAM
    Некорректное значение параметра запроса
    GET  /messages
    PUT  /poa/SetPoaRead
    POST /poa/SendMessage
    GET  /poa/GetRequests
    GET  /poa/GetRequest
    GET  /poa/GetAll
    GET  /poa/GetReceipts
    GET  /poa/GetRegistrationResult

400 TASK_CODE_MUST_BE_SENT
    Должен быть передан параметр Task
    PUT /messages/{msgId}/files/{fileId}

400 FILE_SIZE_NOT_MATCH_DB
    Размер файла не соответствует записи из базы данных
    POST /messages

400 FILES_REQUIRED
    Должен быть указан хотя бы один файл
    POST /messages
    POST /poa/CreateMessage

401 ACCOUNT_NOT_FOUND
    Аккаунт не найден
    POST /messages
    GET  /profile
    GET  /quote

403 DICTIONARY_FORBIDDEN
    Доступ к справочнику запрещен
    GET /dictionaries/{dictId}

403 MESSAGE_DELETE_ERROR
    Удаление сообщений с данным статусом запрещено настройками статусной модели
    DELETE /messages/{msgId}
    DELETE /messages/{msgId}}/files/{fileId}
    DELETE /poa/Delete
    DELETE /poa/DeleteFile/{msgId}/files/{fileId}

404 DICTIONARY_NOT_FOUND
    Справочник не найден
    GET /dictionaries/{dictId}
    GET /dictionaries/{dictId}/download

404 FILE_NOT_FOUND
    Невозможно найти файл с указанным id
    POST /messages/{msgId}/files/{fileId}/createUploadSession
    GET  /messages/{msgId}/files/{fileId}
    PUT  /messages/{msgId}/files/{fileId}
    GET  /messages/{msgId}/files/{fileId}/download
    GET  /messages/{msgId}/receipts/{rcptId}/files/{fileId}
    GET  /messages/{msgId}/receipts/{rcptId}/files/{fileId}/download
    POST /poa/CreateUploadSession/{msgId}/files/{fileId}/createUploadSession
    GET  /poa/DownloadFile/{msgId}/files/{fileId}/download

404 FILE_TEMPORARY_NOT_AVAILABLE
    Файл сообщения временно недоступен
    GET /messages/{msgId}/download (прим.: если для всех файлов в сообщении)
    GET /messages/{msgId}/files/{fileId}/download

404 MESSAGE_NOT_FOUND
    Невозможно найти сообщение с указанным id
    GET    /messages/{msgId}
    DELETE /messages/{msgId}
    POST   /messages/{msgId}
    GET    /messages/{msgId}/download
    POST   /messages/{msgId}/files/{fileId}/createUploadSession
    GET    /messages/{msgId}/files/{fileId}
    PUT    /messages/{msgId}/files/{fileId}
    GET    /messages/{msgId}/files/{fileId}/download
    GET    /messages/{msgId}/receipts
    GET    /messages/{msgId}/receipts/{rcptId}
    GET    /messages/{msgId}/receipts/{rcptId}/files/{fileId}
    GET    /messages/{msgId}/receipts/{rcptId}/files/{fileId}/download
    POST /poa/SendMessage
    GET  /poa/GetRequest
    GET  /poa/DownloadMessage/{msgId}/download
    POST /poa/CreateUploadSession/{msgId}/files/{fileId}/createUploadSession
    GET  /poa/DownloadFile/{msgId}/files/{fileId}/download

404 RECEIPT_NOT_FOUND
    Невозможно найти квитанцию с указанным id
    GET /messages/{msgId}/receipts/{rcptId}
    GET /messages/{msgId}/receipts/{rcptId}/files/{fileId}
    GET /messages/{msgId}/receipts/{rcptId}/files/{fileId}/download
    GET /poa/GetRegistrationResult

405 BASE_REQUEST_ADDRESS_NOT_FOUND
    Не найден базовый адрес запроса
    POST /messages/{msgId}/files/{fileId}/createUploadSession

405 NOT_ALLOWED_FOR_ASPERA_REPO
    Для файла указано RepositoryType = Aspera, он не может быть загружен через HTTP
    POST /messages/{msgId}/files/{fileId}/createUploadSession
    POST /poa/CreateUploadSession/{msgId}/files/{fileId}/createUploadSession

406 MESSAGE_SENT_ERROR
    Сообщение не может быть отправлено
    POST /messages
    POST /messages/{msgId}

406 FILE_SIZE_ERROR
    Размер файла {file.name} должен быть Должен быть в диапазоне от 1 до 9223372036854775807 байт
    POST /messages

406 DUPLICATE_FILE_NAME
    Имена файлов не должны повторяться
    POST /messages

406 FILE_ENCRYPTION_FLAG_MUST_BE_SET
    Для файла {requestfile.name} должен быть указан флаг шифрования.
    POST /messages

406 REQ_FILE_EXTENSION_ERROR
    Файл {requestfile.name} с указанным флагом шифрования должен иметь расширение '.enc'.
    POST /messages

406 SIGN_FILE_EXTENSION_ERROR
    Файл подписи {sigfile.name} должен иметь расширение '.sig'
    POST /messages

406 INCORRECT_RECEIVER
    Получатель должен быть КО
    POST /messages

406 RECEIVER_NOT_SET
    Не определен получатель
    POST /messages

406 SEND_BY_THIS_TASK_NOT_ALLOWED
    Не доступна отправка сообщения по указанной задаче.
    POST /messages

406 SIGN_FILE_NOT_FOUND
    Не найден файл для подписи {sigfile.name}.
    POST /messages

406 INVALID_FILE_EXTENSION
    Недопустимое расширение файла для данной задачи
    POST /messages
    POST /poa/CreateMessage

406 INVALID_FILE_NAME
    Имя файла содержит недопустимые символы: {все недопустимые символы}
    POST /messages
    POST /poa/CreateMessage

406 INVALID_FILE_MASK
    Имя файла {0} не соответствует маске {1}, где 0 - имя файла, 1 - маска
    POST /messages

410 FILE_PERMANENTLY_NOT_AVAILABLE
    Файл сообщения более недоступен или задача не предусматривает его хранения
    GET /messages/{msgId}/download (прим.: если для всех файлов в сообщении)
    GET /messages/{msgId}/files/{fileId}/download

413 ACCOUNT_QUOTA_EXCIDED
    Сообщение не может быть отправлено, так как оставшаяся квота хранения истории обмена ЭС будет превышена.
    POST /messages

413 MESSAGE_QUOTA_EXCIDED
    Сообщение не может быть отправлено, так как размер ЭС превышает квоту
    POST /messages

416 INCORRECT_BYTE_RANGE
    В запросе не верно указан диапазон байт
    GET /messages/{msgId}/download
    GET /messages/{msgId}/files/{fileId}/download
    GET /messages/{msgId}/receipts/{rcptId}/files/{fileId}/download

422 INCORRECT_BODY_PARAM
    Неверные параметры в теле запроса. Проверьте сообщение на соответствие параметрам задачи
    POST /messages
    POST /messages/{msgId}

422 INCORRECT_CORRELATION_ID
    Не найдено сообщение, которое должно соответствовать переданному CorrelationId
    POST /messages
*/


/*
HTTP 400 – Bad Request

{
    "httpStatus": 400,
    "errorCode": "REQUEST_PLAYLOD_INCORRECT",
    "errorMessage": "Неправильное тело запроса",
    "moreInfo": {}
}

{
    "httpStatus": 400,
    "errorCode": "FILE_ALLREADY_LOADED",
    "errorMessage": "Файл уже загружен",
    "moreInfo": {}
}

HTTP 401 – Unauthorized

{
    "httpStatus": 401,
    "errorCode": "ACCOUNT_NOT_FOUND",
    "errorMessage": "Аккаунт не найден",
    "moreInfo": {}
}

HTTP 403 – Forbidden

{
    "httpStatus": 403,
    "errorCode": "MESSAGE_DELETE_ERROR",
    "errorMessage": "Удаление сообщений с данным статусом запрещено настройками статусной модели",
    "moreInfo": {}
}

{
    "httpStatus": 403,
    "errorCode": "DICTIONARY_FORBIDDEN",
    "errorMessage": "Доступ к справочнику запрещен",
    "moreInfo": {}
}

HTTP 404 – Not found

{
  "HTTPStatus": 404,
  "ErrorCode": "MESSAGE_NOT_FOUND",
  "ErrorMessage": "Невозможно найти сообщение с указанным id",
  "MoreInfo: {}
}

{
  "HTTPStatus": 404,
  "ErrorCode": "FILE_NOT_FOUND",
  "ErrorMessage": "Невозможно найти файл с указанным id",
  "MoreInfo: {}
}

{
  "HTTPStatus": 404,
  "ErrorCode": "FILE_TEMPORARY_NOT_AVAILABLE",
  "ErrorMessage": "Файлы сообщения временно недоступны",
  "MoreInfo": {
    "MissedFiles": [{
      "Id": "string",
      "FileName": "string",
      "RepositoriInfo":
        "RepositoryInfo": {...}
    }]
  }
}

{
  "HTTPStatus": 404,
  "ErrorCode": "FILE_TEMPORARY_NOT_AVAILABLE",
  "ErrorMessage": "Файл сообщения временно недоступен",
  "MoreInfo": {
    "MissedFiles": [{
      "Id": "22109af0-f6a4-4d14-87f9-afca0128a2c0",
      "FileName": "KYCCL_7831001422_3194_20230319_000001.zip.enc"
    },{
      "Id": "6f4953a2-b9d6-4b0b-8f3a-afca0128a2c3",
      "FileName": "KYCCL_7831001422_3194_20230319_000001.zip.sig"
    }]
  }
}

HTTP 405 – Invalid input

{
  "HTTPStatus": 405,
  "ErrorCode": "BASE_REQUEST_ADDRESS_NOT_FOUND",
  "ErrorMessage": "Не найден базовый адрес запроса",
  "MoreInfo: {}
}

{
  "HTTPStatus": 405,
  "ErrorCode": "NOT_ALLOWED_FOR_ASPERA_REPO",
  "ErrorMessage": "Для указанного файла указано RepositoryType = Aspera, он не может быть загружен через HTTP",
  "MoreInfo: {}
}

HTTP 406 – Not Acceptable

{
  "HTTPStatus": 406,
  "ErrorCode": "MESSAGE_SENT_ERROR",
  "ErrorMessage": "Сообщение не может быть отправлено",
  "MoreInfo: {}
}

{
  "HTTPStatus": 406,
  "ErrorCode": "FILE_SIZE_ERROR",
  "ErrorMessage": "Размер файла {file.name} должен быть в диапазоне от 1 до 9223372036854775807 байт",
  "MoreInfo: {}
}

{
  "HTTPStatus": 406,
  "ErrorCode": "FILE_ENCRYPTION_FLAG_MUST_BE_SET",
  "ErrorMessage": "Для файла {requestfile.name} должен быть указан флаг шифрования",
  "MoreInfo: {}
}

HTTP 410 – Gone

{
  "HTTPStatus": 410,
  "ErrorCode": "FILE_PERMANENTLY_NOT_AVAILABLE",
  "ErrorMessage": "Файлы сообщения более недоступны или задача не предусматривает их хранения",
  "MoreInfo": {
    "MissedFiles": [{
      "Id": "string",
      "FileName": "string".
    }]
  } 
}

HTTP 411 – Length Required

{
  "HTTPStatus": 411,
  "ErrorCode": "THE REQUEST MUST BE CHUNKED OR HAVE A CONTENT LENGTH",
  "ErrorMessage": "Запрос должен быть разбит на чанки или иметь content-length",
  "MoreInfo: {}
}

HTTP 413 – Message size too large

{
  "HTTPStatus": 413,
  "ErrorCode": "MESSAGE_QUOTA_EXCIDED",
  "ErrorMessage": "Сообщение не может быть отправлено, так как размер ЭС превышает квоту (%размер квоты ЭС в MB%)",
  "MoreInfo": {
    "AccountQuota": "integer",
    "RestOfQuota": "integer",
    "MessageQuota": "integer"
  }
}


HTTP 416 – Range Not Satisfiable

{
  "HTTPStatus": 416,
  "ErrorCode":  "INCORRECT_BYTE_RANGE",
  "ErrorMessage": "В запросе не верно указан диапазон байт",
  "MoreInfo: {}
}

HTTP 422 – Unprocessable Entity

{
  "HTTPStatus": 422,
  "ErrorCode": "INCORRECT_BODY_PARAM",
  "ErrorMessage": "Неверные параметры в теле запроса. Проверьте сообщение на соответствие параметрам задачи",
  "MoreInfo": {}
}
*/
