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

using System.Net.Http.Json;

using Microsoft.Extensions.Logging;

namespace Diev.Extensions.Http;

public class HttpService(
    ILogger<HttpService> logger,
    HttpClient httpClient) : IHttpService
{
    public async Task<HttpResponseMessage> GetAsync(
        string uri,
        CancellationToken cancellationToken = default)
    {
        //logger.LogDebug("GET {Uri}", uri);
        return await httpClient.GetAsync(uri, cancellationToken);
    }

    public async Task<HttpResponseMessage> PostAsync(
        string uri,
        HttpContent content,
        CancellationToken cancellationToken = default)
    {
        //logger.LogDebug("POST {Uri}", uri);
        return await httpClient.PostAsync(uri, content, cancellationToken);
    }

    public async Task<HttpResponseMessage> PostAsJsonAsync<T>(
        string uri,
        T data,
        CancellationToken cancellationToken = default)
    {
        //logger.LogDebug("POST JSON {Uri}", uri);
        var content = JsonContent.Create(data);
        return await httpClient.PostAsync(uri, content, cancellationToken);
    }

    public async Task<HttpResponseMessage> PutAsync(
        string uri,
        HttpContent content,
        CancellationToken cancellationToken = default)
    {
        //logger.LogDebug("PUT {Uri}", uri);
        return await httpClient.PutAsync(uri, content, cancellationToken);
    }

    public async Task<HttpResponseMessage> PutAsJsonAsync<T>(
        string uri,
        T data,
        CancellationToken cancellationToken = default)
    {
        //logger.LogDebug("PUT JSON {Uri}", uri);
        var content = JsonContent.Create(data);
        return await httpClient.PutAsync(uri, content, cancellationToken);
    }

    public async Task<HttpResponseMessage> DeleteAsync(
        string uri,
        CancellationToken cancellationToken = default)
    {
        //logger.LogDebug("DELETE {Uri}", uri);
        return await httpClient.DeleteAsync(uri, cancellationToken);
    }

    public async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        // Download
        //var range = request.Headers.Range;

        //if (range is null)
        //{
        //    logger.LogDebug("{Method} {Uri}", request.Method, request.RequestUri);
        //}
        //else
        //{
        //    var bytes = range.ToString();
        //    logger.LogDebug("{Method} {Uri} {Range}", request.Method, request.RequestUri, bytes);
        //}

        return await httpClient.SendAsync(request, cancellationToken);
        //var response = await httpClient.SendAsync(request, cancellationToken);
        //logger.LogDebug("Received {StatusCode}", (int)response.StatusCode);
        //return response;
    }
}
