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

using System.Text;

using Microsoft.Extensions.Logging;

namespace Diev.Extensions.Loggers;

public class SystemdFormatter
{
    public string Format(
        string category,
        LogLevel level,
        EventId eventId,
        string message,
        Exception exception)
    {
        var sb = new StringBuilder();

        //sb.Append($"{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss.fffZ} ");
        sb.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} ");

        string s = level switch
        {
            LogLevel.Trace => "Trce",
            LogLevel.Debug => "Dbug",
            LogLevel.Information => "Info",
            LogLevel.Warning => "Warn",
            LogLevel.Error => "Fail",
            LogLevel.Critical => "Crit",
            _ => "None"
        };

        //sb.Append($"{level.ToString().ToUpper()} ");
        sb.Append($"{s} ");

        if (!string.IsNullOrEmpty(category))
            sb.Append($"[{category}] ");

        if (eventId.Id != 0)
            sb.Append($"({eventId.Id}) ");

        sb.Append(message);

        if (exception != null)
        {
            sb.AppendLine();
            sb.AppendLine("--- EXCEPTION ---");
            sb.AppendLine($"Type: {exception.GetType().FullName}");
            sb.AppendLine($"Message: {exception.Message}");
            sb.AppendLine($"Source: {exception.Source}");
            sb.AppendLine("StackTrace:");
            sb.AppendLine(exception.StackTrace);

            if (exception.InnerException != null)
            {
                sb.AppendLine("--- INNER EXCEPTION ---");
                sb.Append(SystemdFormatter.FormatInnerException(exception.InnerException));
            }

            if (exception.Data?.Count > 0)
            {
                sb.AppendLine("--- DATA ---");
                foreach (var key in exception.Data.Keys)
                {
                    sb.AppendLine($"{key}: {exception.Data[key]}");
                }
            }
        }

        return sb.ToString();
    }

    private static string FormatInnerException(Exception ex)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Type: {ex.GetType().FullName}");
        sb.AppendLine($"Message: {ex.Message}");
        sb.AppendLine("StackTrace:");
        sb.AppendLine(ex.StackTrace);
        return sb.ToString();
    }
}
