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

using Diev.Portal5.API.Messages;

namespace Diev.Portal5.Tools;

public static class MessageExtensions
{
    public static bool Inbox(this Message message)
        => message.Type.Equals(MessageType.Inbox, StringComparison.Ordinal);

    public static bool Outbox(this Message message)
        => message.Type.Equals(MessageType.Outbox, StringComparison.Ordinal);

    public static bool Registered(this Message message)
        => message.Status.Equals(MessageOutStatus.Registered, StringComparison.Ordinal);

    public static bool Success(this Message message)
        => message.Status.Equals(MessageOutStatus.Success, StringComparison.Ordinal);
}
