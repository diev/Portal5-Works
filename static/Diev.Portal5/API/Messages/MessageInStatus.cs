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

using System.Text.Json.Serialization;

namespace Diev.Portal5.API.Messages;

/// <summary>
/// Статус сообщения inbox.
/// </summary>
public record MessageInStatus
{
    /// <summary>
    /// Новое:<br/>
    /// Сообщение в данном статусе еще не прочитано участником
    /// информационного обмена.
    /// </summary>
    public static string New => "new";

    /// <summary>
    /// Прочитано:<br/>
    /// Сообщение в данном статусе прочтено пользователем.
    /// </summary>
    public static string Read => "read";

    /// <summary>
    /// Отправлен ответ:<br/>
    /// На сообщение в данном статусе направлен ответ.
    /// </summary>
    public static string Replied => "replied";

    [JsonIgnore]
    public static string[] Values => [New, Read, Replied];
}
