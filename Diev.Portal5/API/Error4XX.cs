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
/// В случае ошибок REST методов из класса HTTP 4XX, в теле ответа передается JSON-объект с описанием ошибки вида.
/// </summary>
public class Error4XX
{
    /// <summary>
    /// HTTP статус класса 4xx согласно Hypertext Transfer Protocol (HTTP) Status Code Registry.
    /// </summary>
    public int HTTPStatus { get; set; }

    /// <summary>
    /// Внутренний код ошибки Портала. Служит клиенту для автоматизированной обработки ошибок. 
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// Расшифровка ошибки. Служит для человеко-читаемой обработки ошибок.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Объект с дополнительно информацией к ошибке, по-умолчанию пустой.
    /// </summary>
    public object? MoreInfo { get; set; }
}

/*
{
    "httpStatus": 401,
    "errorCode": "ACCOUNT_NOT_FOUND",
    "errorMessage": "Аккаунт не найден",
    "moreInfo": null
}
*/
