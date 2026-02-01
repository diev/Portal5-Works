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

namespace Diev.Extensions.Loggers;

public class ExceptionFileFormatter
{
    public static string Format(Exception exception)
    {
        if (exception is null)
            return string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine("=== EXCEPTION DUMP ===");
        sb.AppendLine($"Type: {exception.GetType().FullName}");
        sb.AppendLine($"Message: {exception.Message}");
        sb.AppendLine($"Source: {exception.Source}");
        sb.AppendLine($"TargetSite: {exception.TargetSite}");
        sb.AppendLine($"StackTrace:");
        sb.AppendLine(exception.StackTrace);

        // Рекурсивно добавляем InnerException
        if (exception.InnerException != null)
        {
            sb.AppendLine("--- INNER EXCEPTION ---");
            sb.Append(ExceptionFileFormatter.Format(exception.InnerException));
        }

        // Добавляем Data, если есть
        if (exception.Data?.Count > 0)
        {
            sb.AppendLine("--- DATA ---");
            foreach (var key in exception.Data.Keys)
            {
                sb.AppendLine($"{key}: {exception.Data[key]}");
            }
        }

        sb.AppendLine("=== END OF DUMP ===");
        return sb.ToString();
    }
}
