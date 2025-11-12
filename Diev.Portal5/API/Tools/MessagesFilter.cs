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

using System.Text;

using Diev.Portal5.API.Messages;

namespace Diev.Portal5.API.Tools;

/// <summary>
/// Критерии запроса списка сообщений.
/// </summary>
public class MessagesFilter
{
    /// <summary>
    /// Наименование задачи<br/>
    /// (если параметр будет указан, то будут возвращены только сообщения полученные/отправленные в рамках указанной задачи).
    /// </summary>
    public string? Task { get; set; }

    /// <summary>
    /// Минимально возможная дата создания сообщения (ГОСТ ISO 8601-2001 по маске «yyyy-MM-dd'T'HH:mm:ss'Z'»)<br/>
    /// (если параметр будет указан, то будут возвращены только сообщения полученные/отправленные позднее указанной даты).
    /// Можно и по маске «yyyy-MM-dd», но помнить, что время в зоне Z (UTC) и надо переводить из локального!
    /// </summary>
    public DateTime? MinDateTime { get; set; }

    /// <summary>
    /// Максимально возможная дата создания сообщения (ГОСТ ISO 8601-2001 по маске «yyyy-MM-dd'T'HH:mm:ss'Z'»)<br/>
    /// (если параметр будет указан, то будут возвращены только сообщения полученные/отправленные ранее указанной даты).
    /// Можно и по маске «yyyy-MM-dd», но помнить, что время в зоне Z (UTC) и надо переводить из локального!
    /// </summary>
    public DateTime? MaxDateTime { get; set; }

    /// <summary>
    /// Минимально возможный размер сообщения в байтах<br/>
    /// (если параметр будет указан, то будут возвращены только сообщения больше указанного размера).
    /// </summary>
    public uint? MinSize { get; set; }

    /// <summary>
    /// Максимально возможный размер сообщения в байтах<br/>
    /// (если параметр будет указан, то будут возвращены только сообщения больше указанного размера).
    /// </summary>
    public uint? MaxSize { get; set; }

    /// <summary>
    /// Тип сообщения исходящее (значение: outbox), входящее (значение: inbox).<br/>
    /// (если параметр будет указан, то будут возвращены только сообщения соответствующего типа).
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Статус сообщения:<br/>
    /// - Черновик (значение: draft),<br/>
    /// - Отправлено (значение: sent),<br/>
    /// - Загружено (значение: delivered),<br/>
    /// - Ошибка (значение: error),<br/>
    /// - Принято в обработку (значение: processing),<br/>
    /// - Зарегистрировано (значение: registered),<br/>
    /// - Отклонено (значение: rejected),<br/>
    /// - Новое (значение: new),<br/>
    /// - Прочитано (значение: read),<br/>
    /// - Отправлен ответ (значение: replied),<br/>
    /// - Доставлено (значение: success)<br/>
    /// (если параметр будет указан, то будут возвращены только сообщения с соответствующим статусом).
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Номер страницы списка сообщений в разбивке по 100 сообщений (если не задан, то вернутся первые 100 сообщений).<br/>
    /// Пример: GET: */messages?page={n}, где {n} – номер страницы содержащей 100 сообщений (n-я сотня сообщений).<br/>
    /// Допустимые значения: n > 0 (положительные целые числа, больше 0).<br/>
    /// Если запрос страницы не указан, возвращается первая страница сообщений.<br/>
    /// Если n за границами диапазона страниц, то вернется пустой массив сообщений.<br/>
    /// В случае некорректного номера страницы – ошибка. 
    /// </summary>
    public uint? Page { get; set; }

    public MessagesFilter()
    { }

    public MessagesFilter(string? task,
        uint? before, uint? days, uint? day, DateTime? minDateTime, DateTime? maxDateTime,
        uint? minSize, uint? maxSize,
        bool inbox, bool outbox,
        string? status, uint? page)
    {
        Task = task;

        var today = DateTime.Today; // next 00:00

        DateTime? day1 = day is null
            ? null
            : today.AddDays((double)-day);

        DateTime? from = days is null
            ? minDateTime
            : today.AddDays((double)-days);

        DateTime? to = before is null
            ? maxDateTime
            : today.AddDays((double)-before);

        MinDateTime = day is null
            ? from
            : day1;
        MaxDateTime = day is null
            ? to
            : day1!.Value.AddDays(1);

        MinSize = minSize;
        MaxSize = maxSize;

        Type = inbox == outbox
            ? null
            : inbox
                ? MessageType.Inbox
                : MessageType.Outbox;

        Status = status;

        Page = page;
    }

    public string GetQuery()
    {
        var query = new StringBuilder();

        if (Task is not null)
        {
            if (Task.StartsWith("Zadacha_"))
            {
                query.Append("&Task=").Append(Task);
            }
            else
            {
                query.Append("&Task=Zadacha_").Append(Task);
            }
        }

        if (MinDateTime is not null)
            query.Append("&MinDateTime=")
                .AppendFormat("{0:yyyy-MM-dd'T'HH:mm:ss'Z'}",
                    ((DateTime)MinDateTime).ToUniversalTime());

        if (MaxDateTime is not null)
            query.Append("&MaxDateTime=")
                .AppendFormat("{0:yyyy-MM-dd'T'HH:mm:ss'Z'}",
                    ((DateTime)MaxDateTime).ToUniversalTime());

        if (MinSize is not null)
            query.Append("&MinSize=").Append(MinSize);

        if (MaxSize is not null)
            query.Append("&MaxSize=").Append(MaxSize);

        if (Type is not null)
            query.Append("&Type=").Append(Type);

        if (Status is not null)
            query.Append("&Status=").Append(Status);

        if (Page is not null && Page > 1)
        {
            query.Append("&Page=").Append(Page);
        }

        if (query.Length == 0)
        {
            return string.Empty;
        }

        return '?' + query.ToString()[1..];
    }

    public bool IsEmpty() =>
        !GetQuery().Contains('=');
}
