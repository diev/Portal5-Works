#region License
/*
Copyright 2024-2025 Dmitrii Evdokimov
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

using Diev.Extensions.Tools;
using Diev.Portal5.API.Messages;
using Diev.Portal5.Interfaces;

namespace Diev.Portal5.Tools;

public class PathService : IPathService
{
    public (string json, string zip) GetZipStore(Message message, string root)
    {
        string download = Path.Combine(
            Path.GetFullPath(root),
            message.Type,
            message.TaskName,
            message.CreationDate.ToString("yyyy-MM"));

        if (!Directory.CreateDirectory(download).Exists)
            throw new DirectoryNotFoundException($"Не удалось создать директорию {download.PathQuoted()}");

        string id = message.Id!;
        string name = $"{message.CreationDate:yyyy-MM-dd}-{id}";
        string path = Path.Combine(download, name);

        string json = path + ".json";
        string zip = path + ".zip";

        return (json, zip);
    }

    public string GetDocStore(Message message, MessageInfo msgInfo, string root)
    {
        //return Path.Combine(
        //    Path.GetFullPath(root),
        //    message.Type,
        //    message.TaskName,
        //    msgInfo.Date[0..7], // 2024-10-04 => 2024-10
        //    msgInfo.PathName!);

        return Path.Combine(
            Path.GetFullPath(root),
            message.Outbox ? "Исх.ЦБ" : "Вх.ЦБ", // "OUT" : "INC",
            msgInfo.Date[0..7], // 2024-10-04 => 2024-10
            msgInfo.Name!);
    }

    //public string GetDocStore2(Message message, MessageInfo msgInfo, string root)
    //{
    //    return Path.Combine(
    //        Path.GetFullPath(root),
    //        message.Outbox ? "Исх.ЦБ" : "Вх.ЦБ",
    //        //msgInfo.Date[0..4], // 2024-10-04 => 2024
    //        msgInfo.Date[0..7], // 2024-10-04 => 2024-10
    //        msgInfo.Name!);
    //}
}
