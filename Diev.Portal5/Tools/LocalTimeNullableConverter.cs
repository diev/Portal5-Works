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

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Diev.Portal5.Tools;

public class LocalTimeNullableConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.String:
                {
                    var dt = reader.GetDateTime(); // .NET сам поймёт "Z" как UTC
                    return dt.ToLocalTime();       // сразу в локальное
                }

            default:
                throw new JsonException($"Неподдерживаемый тип токена для DateTime?: {reader.TokenType}");
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            // При записи обратно в JSON лучше хранить в UTC (с "Z")
            writer.WriteStringValue(value.Value.ToUniversalTime());
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
