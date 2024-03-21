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
/// Состояние обработки сообщения outbox.
/// </summary>
public record ReceiptOutStatus
{
    /// <summary>
    /// Загружено: Сообщение прошло первоначальную проверку.
    /// В рамках 5361-У, квитанцией о загрузке считается эта квитанция.
    /// </summary>
    public static string Delivered => "delivered";

    /// <summary>
    /// Ошибка: При обработке сообщения возникла ошибка.
    /// </summary>
    public static string Error => "error";

    /// <summary>
    /// Принято в обработку: Сообщение передано во внутреннюю систему ЦБ.
    /// </summary>
    public static string Processing => "processing";

    /// <summary>
    /// Зарегистрировано: Сообщение зарегистрировано.
    /// В рамках 5361-У, квитанцией о регистрации считается эта квитанция.
    /// </summary>
    public static string Registered => "registered";

    /// <summary>
    /// Отклонено: Сообщение успешно дошло до получателя, но было отклонено.
    /// </summary>
    public static string Rejected => "rejected";

    /// <summary>
    /// Доставлено: Сообщение успешно размещено в ЛК.
    /// Сообщение передано роутером во внутреннюю систему Банка России,
    /// от которой не ожидается ответ о регистрации.
    /// </summary>
    public static string Success => "success";

    public static string[] Values => ["delivered", "error", "processing", "registered", "rejected", "success"];
}
