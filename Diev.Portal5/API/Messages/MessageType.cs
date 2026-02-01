#region License
/*
Copyright 2022-2026 Dmitrii Evdokimov
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
/// Тип сообщения:<br/>
/// outbox - исходящее<br/>
/// inbox - входящее
/// </summary>
public record MessageType
{
    public static string Inbox => "inbox";

    public static string Outbox => "outbox";

    [JsonIgnore]
    public static string[] Values => [Inbox, Outbox];
}
