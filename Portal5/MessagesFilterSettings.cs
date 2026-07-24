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

namespace Portal5;

public class MessagesFilterSettings
{
    public string? Task { get; set; }
    public List<string>? Tasks { get; set; }
    public List<string>? NoTasks { get; set; }
    public uint? Before { get; set; }
    public uint? Days { get; set; }
    public uint? Day { get; set; }
    public DateTime? MinDateTime { get; set; }
    public DateTime? MaxDateTime { get; set; }
    public uint? MinSize { get; set; }
    public uint? MaxSize { get; set; }
    public bool Inbox { get; set; }
    public bool Outbox { get; set; }
    public string? Status { get; set; }
    public uint? Page { get; set; }
}
