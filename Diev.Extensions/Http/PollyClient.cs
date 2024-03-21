#region License
/*
Copyright 2023-2024 Dmitrii Evdokimov
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
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using Diev.Extensions.Credentials;

namespace Diev.Extensions.Http;

// https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
public static class PollyClient
{
    private static HttpClient _httpClient = null!;
    private static int _chunkSize = 1048576; // 1Mb
    private static DateTime _ddosAllowedTime = DateTime.Now;

    public static int Retries { get; set; } = 100;
    public static int RetryTimeout { get; set; } = 2000; // retry * RetryTimeout
    public static int DdosTimeout { get; set; } = 1000;

    public static int ChunkSize 
    {
        get => _chunkSize;
        set => _chunkSize = value == 0 ? int.MaxValue : value;
    }

    //static PollyClient()
    //{
    //    JsonOptions = new JsonSerializerOptions()
    //    {
    //        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    //    };
    //}

    public static void Login(Credential cred, bool trace, string tracelog)
    {
        //var app = Assembly.GetExecutingAssembly().GetName();
        var app = Assembly.GetEntryAssembly()?.GetName();

        //if (Test)
        //{
        //    Api = TestHost + TestBaseUrl;

        //    HttpClientHandler handler = new()
        //    {
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(TestUsername, TestPassword)
        //    };

        //    if (UseProxy && ProxyAddress != null)
        //    {
        //        handler.Proxy = new WebProxy(new Uri(ProxyAddress)); // or null;
        //    }

        //    _httpClient = new(handler, true)
        //    {
        //        BaseAddress = new Uri(TestHost),
        //        Timeout = TimeSpan.FromMinutes(3)
        //    };

        //    _httpClient.DefaultRequestHeaders.UserAgent.Add(
        //        new ProductInfoHeaderValue(
        //            app.Name ?? "TestClient",
        //            app.Version?.ToString() ?? "1.0"));
        //}
        //else
        //{
        //    Api = Host + BaseUrl;

        //    HttpClientHandler handler = new()
        //    {
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(Username, Password)
        //    };

        //    if (UseProxy && ProxyAddress != null)
        //    {
        //        handler.Proxy = new WebProxy(new Uri(ProxyAddress)); // or null;
        //    }

        //    _httpClient = new(handler, true)
        //    {
        //        BaseAddress = new Uri(Host),
        //        Timeout = TimeSpan.FromMinutes(3)
        //    };

        //    _httpClient.DefaultRequestHeaders.UserAgent.Add(
        //        new ProductInfoHeaderValue(
        //            app.Name ?? "Client",
        //            app.Version?.ToString() ?? "1.0"));
        //}

        HttpClientHandler handler = new()
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(cred.UserName, cred.Password)
        };

        string host = cred.TargetName.Split(' ')[1];
        _httpClient = new(new LoggingHandler(handler, trace, tracelog), true)
        {
            BaseAddress = new Uri(host),
            Timeout = TimeSpan.FromMinutes(3)
        };

        _httpClient.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue(
                app?.Name ?? "Client",
                app?.Version?.ToString() ?? "1.0"));

        //var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        /*
        // Check connection
        //using var request = new HttpRequestMessage(HttpMethod.Head, "/");
        using var request = new HttpRequestMessage(HttpMethod.Head, HttpClient.BaseAddress"); //TODO
        using var response = await HttpClient.SendAsync(request);
        //return response.EnsureSuccessStatusCode().Content.Headers.ContentLength > 0;
        response.EnsureSuccessStatusCode();
        // 200 OK
        */

        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public static async Task<HttpResponseMessage> GetAsync(string url)
        => await ExecuteAsync(HttpMethod.Get, url);

    public static async Task<HttpResponseMessage> GetFromJasonAsync(string url, JsonContent content)
        => await ExecuteAsync(HttpMethod.Get, url, content);

    public static async Task<HttpResponseMessage> PostAsync(string url, HttpContent? content = null)
        => await ExecuteAsync(HttpMethod.Post, url, content);

    public static async Task<HttpResponseMessage> PostAsJasonAsync(string url, JsonContent content)
        => await ExecuteAsync(HttpMethod.Post, url, content);

    public static async Task<HttpResponseMessage> PutAsync(string url, HttpContent? content = null)
        => await ExecuteAsync(HttpMethod.Put, url, content);

    public static async Task<HttpResponseMessage> PutAsJasonAsync(string url, JsonContent content)
        => await ExecuteAsync(HttpMethod.Put, url, content);

    public static async Task<HttpResponseMessage> PatchAsync(string url, HttpContent content)
        => await ExecuteAsync(HttpMethod.Patch, url, content);

    public static async Task<HttpResponseMessage> DeleteAsync(string url)
        => await ExecuteAsync(HttpMethod.Delete, url);

    public static async Task<HttpResponseMessage> HeadAsync(string url)
        => await ExecuteAsync(HttpMethod.Head, url);

    public static async Task<HttpResponseMessage> OptionsAsync(string url)
        => await ExecuteAsync(HttpMethod.Options, url);

    public static async Task<HttpResponseMessage> PutPartialAsync(string url, HttpContent content,
        long contentLength, long from, long to, long size)
    {
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Headers.ContentLength = contentLength;
        content.Headers.ContentRange = new ContentRangeHeaderValue(from, to, size);

        return await ExecuteAsync(HttpMethod.Put, url, content);
    }

    public static async Task<HttpResponseMessage> GetPartialAsync(string url, long from, long to)
    {
        int retry = 0;

        while (true)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Range = new RangeHeaderValue(from, to);

                var response = await ExecuteDdosAsync(request);

                if (!RetryRequired(response.StatusCode) || (retry > Retries))
                {
                    return response;
                }
            }

            Thread.Sleep(++retry * RetryTimeout);
        }
    }

    public static async Task<HttpResponseMessage> ExecuteAsync(HttpMethod method,
        string url, HttpContent? content = null)
    {
        int retry = 0;

        while (true)
        {
            using (var request = new HttpRequestMessage(method, url))
            {
                if (content != null)
                {
                    request.Content = content;
                }

                var response = await ExecuteDdosAsync(request);

                if (!RetryRequired(response.StatusCode) || (retry > Retries))
                {
                    return response;
                }
            }

            Thread.Sleep(++retry * RetryTimeout);
        }
    }

    private static async Task<HttpResponseMessage> ExecuteDdosAsync(HttpRequestMessage request)
    {
        if (DateTime.Now < _ddosAllowedTime)
        {
            Thread.Sleep(_ddosAllowedTime - DateTime.Now);
        }

        var response = await _httpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead);

        _ddosAllowedTime = DateTime.Now.AddMilliseconds(DdosTimeout);

        return response;
    }

    private static bool RetryRequired(HttpStatusCode code)
    {
        /*
        100 	HttpStatusCode.Continue
        101 	HttpStatusCode.SwitchingProtocols
        102 	HttpStatusCode.Processing
        103 	HttpStatusCode.EarlyHints

        200 	HttpStatusCode.OK
        201 	HttpStatusCode.Created
        202 	HttpStatusCode.Accepted
        203 	HttpStatusCode.NonAuthoritativeInformation
        204 	HttpStatusCode.NoContent
        205 	HttpStatusCode.ResetContent
        206 	HttpStatusCode.PartialContent
        207 	HttpStatusCode.MultiStatus
        208 	HttpStatusCode.AlreadyReported
        226 	HttpStatusCode.IMUsed

        300 	HttpStatusCode.MultipleChoices or HttpStatusCode.Ambiguous
        301 	HttpStatusCode.MovedPermanently or HttpStatusCode.Moved
        302 	HttpStatusCode.Found or HttpStatusCode.Redirect
        303 	HttpStatusCode.SeeOther or HttpStatusCode.RedirectMethod
        304 	HttpStatusCode.NotModified
        305 	HttpStatusCode.UseProxy
        306 	HttpStatusCode.Unused
        307 	HttpStatusCode.TemporaryRedirect or HttpStatusCode.RedirectKeepVerb
        308 	HttpStatusCode.PermanentRedirect

        400 	HttpStatusCode.BadRequest
        401 	HttpStatusCode.Unauthorized
        402 	HttpStatusCode.PaymentRequired
        403 	HttpStatusCode.Forbidden
        404 	HttpStatusCode.NotFound
        405 	HttpStatusCode.MethodNotAllowed
        406 	HttpStatusCode.NotAcceptable
        407 	HttpStatusCode.ProxyAuthenticationRequired
        408 	HttpStatusCode.RequestTimeout
        409 	HttpStatusCode.Conflict
        410 	HttpStatusCode.Gone
        411 	HttpStatusCode.LengthRequired
        412 	HttpStatusCode.PreconditionFailed
        413 	HttpStatusCode.RequestEntityTooLarge
        414 	HttpStatusCode.RequestUriTooLong
        415 	HttpStatusCode.UnsupportedMediaType
        416 	HttpStatusCode.RequestedRangeNotSatisfiable
        417 	HttpStatusCode.ExpectationFailed
        418 	I'm a teapot
        421 	HttpStatusCode.MisdirectedRequest
        422 	HttpStatusCode.UnprocessableEntity
        423 	HttpStatusCode.Locked
        424 	HttpStatusCode.FailedDependency
        426 	HttpStatusCode.UpgradeRequired
        428 	HttpStatusCode.PreconditionRequired
        429 	HttpStatusCode.TooManyRequests
        431 	HttpStatusCode.RequestHeaderFieldsTooLarge
        451 	HttpStatusCode.UnavailableForLegalReasons

        500     HttpStatusCode.InternalServerError
        501     HttpStatusCode.NotImplemented
        502     HttpStatusCode.BadGateway
        503     HttpStatusCode.ServiceUnavailable
        504     HttpStatusCode.GatewayTimeout
        505     HttpStatusCode.HttpVersionNotSupported
        506     HttpStatusCode.VariantAlsoNegotiates
        507     HttpStatusCode.InsufficientStorage
        508     HttpStatusCode.LoopDetected
        510     HttpStatusCode.NotExtended
        511     HttpStatusCode.NetworkAuthenticationRequired
        */

        return (code >= HttpStatusCode.InternalServerError)
            || (code == HttpStatusCode.RequestTimeout)
            || (code == HttpStatusCode.TooManyRequests);
    }
}
