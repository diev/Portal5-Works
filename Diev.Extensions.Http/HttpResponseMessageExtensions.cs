#region License
/*
Copyright 2023 Dmitrii Evdokimov
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

namespace Diev.Extensions.Http;

public static class HttpResponseMessageExtensions
{
    /// <summary>
    /// Эта функция используется для записи сведений о запросе в консоль в следующей форме:
    /// <HTTP Request Method> <Request URI> <HTTP/Version>
    /// </summary>
    /// <code>
    /// response.EnsureSuccessStatusCode().WriteRequestToConsole();
    /// </code>
    /// <example>
    /// GET https://jsonplaceholder.typicode.com/todos/3 HTTP/1.1
    /// </example>
    /// <seealso cref="https://learn.microsoft.com/ru-ru/dotnet/fundamentals/networking/http/httpclient"/>
    /// <param name="response"></param>
    public static void WriteRequestToConsole(this HttpResponseMessage response)
    {
        if (response is null)
        {
            return;
        }

        var request = response.RequestMessage;

        Console.Write($"{request?.Method} ");
        Console.Write($"{request?.RequestUri} ");
        Console.WriteLine($"HTTP/{request?.Version}");
    }

    public static void WriteRequest(this HttpResponseMessage response, out string output)
    {
        if (response is null)
        {
            output = "ERROR";
            return;
        }

        var request = response.RequestMessage;
        output = $"{request?.Method} {request?.RequestUri} HTTP/{request?.Version}";
    }
}
