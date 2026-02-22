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

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using Diev.Extensions.LogFile;
using Diev.Portal5.API.Errors;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Diev.Portal5;

public record ApiResult<T>
(
    bool OK,
    T? Data = default,
    ApiError? Error = null
)
{
    public static JsonSerializerOptions JsonOptions { get; } =
        new() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

    public static ApiResult<T> CreateOK(T? data) =>
        new(true, data);

    public static ApiResult<T> CreateError(ApiError? error)
    {
        Logger.TimeLine($"HTTP {error?.HTTPStatus}, {error?.ErrorCode}, {error?.ErrorMessage}");
        return new(false, default, error);
    }

    public static ApiResult<T> CreateError(string message)
    {
        var error = new ApiError(0, "API Error", message);
        Logger.TimeLine($"HTTP 0, {error?.ErrorCode}, {error?.ErrorMessage}");
        return new(false, default, error);
    }

    public static ApiResult<T> CreateError(HttpResponseMessage response)
    {
        using var content = response.Content.ReadAsStream(); //TODO Async
        var error = JsonSerializer.Deserialize<ApiError>(content, JsonOptions); //TODO 404, etc.
        Logger.TimeLine($"HTTP {error?.HTTPStatus}, {error?.ErrorCode}, {error?.ErrorMessage}");
        return new(false, default, error);
    }

    public static ApiResult<T> CreateExceptionError(Exception ex, string? message = null)
    {
        string messageEx = message is null
            ? ex.Message
            : message + " " + ex.Message;

        var error = ex switch
        {
            HttpRequestException => new ApiError(0, "Network Error", messageEx),
            JsonException => new ApiError(0, "JSON Error", messageEx),
            TaskCanceledException => new ApiError(0, "Timeout", "Request timed out"),
            _ => new ApiError(0, "Unexpected Error", messageEx)
        };

        Logger.TimeLine($"HTTP {error.HTTPStatus}, {error.ErrorCode}, {error.ErrorMessage}");
        return new ApiResult<T>(false, default, error);
    }
}
