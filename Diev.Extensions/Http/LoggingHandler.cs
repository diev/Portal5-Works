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

using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;

using Diev.Extensions.LogFile;

namespace Diev.Extensions.Http;

internal class LoggingHandler : DelegatingHandler
{
    public bool Json { get; set; } = true;
    public bool Pretty { get; set; } = true;
    public bool Debug { get; set; } = false;
    public JsonSerializerOptions JsonOptions { get; set; }

    public LoggingHandler(HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
        JsonOptions = new JsonSerializerOptions()
        {
            //AllowTrailingCommas = true,
            //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        };
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //if (!DoTrace)
        //{
        //    return await base.SendAsync(request, cancellationToken);
        //}

        Logger.TimeLine($"Request {request}");

        if (request.Content is not null)
        {
            var type = request.Content.Headers.ContentType;

            if (type?.MediaType is not null)
            {
                if (type.MediaType.Contains("json", StringComparison.Ordinal))
                {
                    using MemoryStream stream = new();
                    await request.Content.CopyToAsync(stream, cancellationToken);

                    stream.Seek(0, SeekOrigin.Begin);
                    using var sr = new StreamReader(stream, Encoding.UTF8);
                    string json = await sr.ReadToEndAsync(cancellationToken);

                    if (Json)
                    {
                        Logger.Line(Pretty ? JsonNode.Parse(json)!.ToJsonString(JsonOptions) : json);
                    }

                    Logger.Line($"{stream.Length} bytes sent");
                }
                else
                {
                    Logger.Line($"{request.Content.Headers.ContentLength} bytes sent");
                }
            }

        }

        Logger.Flush(1);

        var response = Debug
            ? new HttpResponseMessage() { StatusCode = HttpStatusCode.ServiceUnavailable } // don't touch the server!
            : await base.SendAsync(request, cancellationToken); // send to the server

        Logger.TimeLine($"Response {response}");

        if (response.Content is not null)
        {
            var type = response.Content.Headers.ContentType;

            if (type?.MediaType is not null)
            {
                if (type.MediaType.Contains("json", StringComparison.Ordinal))
                {
                    //WARNING: DO NOT EDIT THIS CODE BLOCK!!!

                    //var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

                    using MemoryStream stream = new();
                    await response.Content.CopyToAsync(stream, cancellationToken);

                    stream.Seek(0, SeekOrigin.Begin); //MemoryStream only
                    using var sr = new StreamReader(stream, Encoding.UTF8);
                    string json = await sr.ReadToEndAsync(cancellationToken);

                    if (Json)
                    {
                        Logger.Line(Pretty ? JsonNode.Parse(json)!.ToJsonString(JsonOptions) : json);
                    }

                    Logger.Line($"{stream.Length} bytes received");

                    //stream.Seek(0, SeekOrigin.Begin);
                    //using var binaryReader = new BinaryReader(stream);
                    //response.Content = new ByteArrayContent(binaryReader.ReadBytes((int)stream.Length));

                    response.Content = new ByteArrayContent(stream.ToArray());
                    response.Content.Headers.ContentType = type;
                }
                else
                {
                    Logger.Line($"{response.Content.Headers.ContentLength ?? 0} bytes received");
                }
            }

            Logger.Flush(1);
        }

        //using var log = new FileStream(TraceLog, FileMode.Append);
        //using var logger = new StreamWriter(log, Encoding.UTF8);

        //await logger.WriteLineAsync();
        //await logger.WriteLineAsync();
        //await logger.WriteLineAsync($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} Request {request}");
        //await logger.FlushAsync(cancellationToken).ConfigureAwait(false);

        //if (request.Content is not null)
        //{
        //    var type = request.Content.Headers.ContentType;

        //    if (type?.MediaType is not null)
        //    {
        //        if (type.MediaType.Contains("json", StringComparison.Ordinal))
        //        {
        //            using MemoryStream stream = new();
        //            await request.Content.CopyToAsync(stream, cancellationToken);

        //            stream.Seek(0, SeekOrigin.Begin);
        //            using var sr = new StreamReader(stream, Encoding.UTF8);
        //            string json = await sr.ReadToEndAsync(cancellationToken);

        //            if (Json)
        //            {
        //                if (Pretty)
        //                {
        //                    var prettyJson = JsonNode.Parse(json)!.ToJsonString(JsonOptions);
        //                    await logger.WriteLineAsync(prettyJson);
        //                }
        //                else
        //                {
        //                    await logger.WriteLineAsync(json);
        //                }
        //            }

        //            await logger.WriteLineAsync($"{stream.Length} bytes sent");
        //        }
        //        else
        //        {
        //            await logger.WriteLineAsync($"{request.Content.Headers.ContentLength} bytes sent");
        //        }
        //    }

        //    await logger.FlushAsync(cancellationToken).ConfigureAwait(false);
        //}

        //Console.WriteLine($"Request {request}");
        //var response = await base.SendAsync(request, cancellationToken);
        //Console.WriteLine($"{Environment.NewLine}Response {response}");

        //await logger.WriteLineAsync();
        //await logger.WriteLineAsync($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} Response {response}");
        //await logger.FlushAsync(cancellationToken).ConfigureAwait(false);

        //if (response.Content is not null)
        //{
        //    var type = response.Content.Headers.ContentType;

        //    if (type?.MediaType is not null)
        //    {
        //        if (type.MediaType.Contains("json", StringComparison.Ordinal))
        //        {
        //            using MemoryStream stream = new();
        //            await response.Content.CopyToAsync(stream, cancellationToken);
        //            //.ConfigureAwait(false);

        //            stream.Seek(0, SeekOrigin.Begin);
        //            using var sr = new StreamReader(stream, Encoding.UTF8);
        //            string json = await sr.ReadToEndAsync(cancellationToken);

        //            if (Json)
        //            {
        //                if (Pretty)
        //                {
        //                    var prettyJson = JsonNode.Parse(json)!.ToJsonString(JsonOptions);
        //                    await logger.WriteLineAsync(prettyJson);
        //                }
        //                else
        //                {
        //                    await logger.WriteLineAsync(json);
        //                }
        //            }

        //            await logger.WriteLineAsync($"{stream.Length} bytes received");

        //            response.Content = new ByteArrayContent(stream.ToArray());
        //            response.Content.Headers.ContentType = type;
        //        }
        //        else
        //        {
        //            await logger.WriteLineAsync($"{response.Content.Headers.ContentLength ?? 0} bytes received");
        //        }
        //    }

        //    await logger.FlushAsync(cancellationToken).ConfigureAwait(false);
        //}

        return response;
    }
}
