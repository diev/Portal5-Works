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

using System.Diagnostics;
using System.Text;

using Microsoft.Extensions.Logging;

namespace Diev.Extensions.Loggers;

public class HttpLogger(
    ILogger<HttpLogger> logger
    ) : DelegatingHandler
{
    private const int Len = 77;

    private readonly bool trace = logger.IsEnabled(LogLevel.Trace);
    private readonly bool debug = logger.IsEnabled(LogLevel.Debug);

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (trace)
        {
            await TraceLogRequestAsync(request)
                .ConfigureAwait(false);

            var response = await base.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            await TraceLogResponseAsync(response)
                .ConfigureAwait(false);

            return response;
        }

        if (debug)
        {
            await DebugLogRequestAsync(request)
                .ConfigureAwait(false);

            var response = await base.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);
            
            await DebugLogResponseAsync(response)
                .ConfigureAwait(false);
            
            return response;
        }

        return await base.SendAsync(request, cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task TraceLogRequestAsync(HttpRequestMessage request)
    {
        var logBuilder = new StringBuilder();
        logBuilder.AppendLine($"HTTP REQUEST: {request.Method} {request.RequestUri}");

        foreach (var header in request.Headers)
        {
            if (header.Key.Equals("Authorization", StringComparison.Ordinal))
            {
                logBuilder.AppendLine($"{header.Key}: ****");
            }
            else
            {
                logBuilder.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
        }

        if (request.Content != null)
        {
            await using var stream = new MemoryStream();
            await request.Content.CopyToAsync(stream)
                .ConfigureAwait(false);
            var len = stream.Length;

            if (len > 0)
            {
                logBuilder.AppendLine($"Content: {len} bytes sending");
                var bytes = stream.ToArray();

                if (bytes[0] == '[' || bytes[0] == '{')
                {
                    string s = Encoding.UTF8.GetString(bytes);
                    logBuilder.AppendLine(s);
                }
            }
        }

        logger.LogTrace("{Request}", logBuilder.ToString());
    }

    private async Task DebugLogRequestAsync(HttpRequestMessage request)
    {
        var logBuilder = new StringBuilder();
        logBuilder.AppendLine($"HTTP REQUEST: {request.Method} {request.RequestUri}");

        //foreach (var header in request.Headers)
        //{
        //    if (header.Key.Equals("Authorization", StringComparison.Ordinal))
        //    {
        //        logBuilder.AppendLine($"{header.Key}: ****");
        //    }
        //    else
        //    {
        //        logBuilder.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
        //    }
        //}

        if (request.Content != null)
        {
            await using var stream = new MemoryStream();
            await request.Content.CopyToAsync(stream)
                .ConfigureAwait(false);
            var len = stream.Length;

            if (len > 0)
            {
                logBuilder.AppendLine($"Content: {len} bytes sending");
                var count = Math.Min(len, Len);
                var bytes = new byte[count];
                stream.Position = 0;
                await stream.ReadExactlyAsync(bytes)
                    .ConfigureAwait(false);

                if (bytes[0] == '[' || bytes[0] == '{')
                {
                    string s = Encoding.UTF8.GetString(bytes);
                    logBuilder.AppendLine(len > Len ? s + "..." : s);
                }
            }
        }

        logger.LogDebug("{Request}", logBuilder.ToString());
    }

    private async Task TraceLogResponseAsync(HttpResponseMessage response)
    {
        var logBuilder = new StringBuilder();
        logBuilder.AppendLine($"HTTP RESPONSE: {(int)response.StatusCode} {response.ReasonPhrase}");

        foreach (var header in response.Headers)
        {
            logBuilder.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
        }

        if (response.Content != null)
        {
            var type = response.Content.Headers.ContentType;
            await using var stream = new MemoryStream();
            await response.Content.CopyToAsync(stream)
                .ConfigureAwait(false);

            var bytes = stream.ToArray();
            var len = bytes.Length;

            if (len > 0)
            {
                logBuilder.AppendLine($"Content: {len} bytes received");

                if (bytes[0] == '[' || bytes[0] == '{')
                {
                    string s = Encoding.UTF8.GetString(bytes);
                    logBuilder.AppendLine(s);
                }
            }

            // return back the original response!
            response.Content = new ByteArrayContent(bytes);
            response.Content.Headers.ContentType = type;
        }

        logger.LogTrace("{Response}", logBuilder.ToString());
    }

    private async Task DebugLogResponseAsync(HttpResponseMessage response)
    {
        var logBuilder = new StringBuilder();
        logBuilder.AppendLine($"HTTP RESPONSE: {(int)response.StatusCode} {response.ReasonPhrase}");

        foreach (var header in response.Headers)
        {
            if (header.Key.StartsWith("EPVV-", StringComparison.Ordinal))
            {
                logBuilder.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
        }

        if (response.Content != null)
        {
            var type = response.Content.Headers.ContentType;
            await using var stream = new MemoryStream();
            await response.Content.CopyToAsync(stream)
                .ConfigureAwait(false);

            var bytes = stream.ToArray();
            var len = bytes.Length;

            if (len > 0)
            {
                logBuilder.AppendLine($"Content: {len} bytes received");

                if (bytes[0] == '[' || bytes[0] == '{')
                {
                    var count = Math.Min(len, Len);
                    string s = Encoding.UTF8.GetString(bytes[..count]);
                    logBuilder.AppendLine(len > Len ? s + "..." : s);
                }
            }

            // return back the original response!
            response.Content = new ByteArrayContent(bytes);
            response.Content.Headers.ContentType = type;
        }

        logger.LogDebug("{Response}", logBuilder.ToString());
    }
}
