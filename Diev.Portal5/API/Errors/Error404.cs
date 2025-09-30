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
public record Error404
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
    string ErrorMessage,

    /// <summary>
    /// Объект с дополнительно информацией к ошибке, по-умолчанию пустой.
    /// HTTP 404 – Not found
    /// HTTP 410 – Gone
    /// HTTP 413 – Message size too large
    /// </summary>
    MoreInfo404 MoreInfo
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
