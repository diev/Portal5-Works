#region License
/*
Copyright 2022-2023 Dmitrii Evdokimov
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

using System.IO.MemoryMappedFiles;
using System.Net;
using System.Net.Http.Headers;

using Diev.Portal5.API;

namespace Diev.Portal5;

/*
 * https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
 * https://www.codeproject.com/Questions/5163547/Read-a-large-file-from-disk-in-chunks-to-send-to-a
 * https://www.codingwithcalvin.net/uploading-files-with-httpclient-in-net-6/
 * https://coderoad.ru/566462/%D0%97%D0%B0%D0%B3%D1%80%D1%83%D0%B7%D0%BA%D0%B0-%D1%84%D0%B0%D0%B9%D0%BB%D0%BE%D0%B2-%D1%81-%D0%BF%D0%BE%D0%BC%D0%BE%D1%89%D1%8C%D1%8E-HTTPWebrequest-%D0%BC%D0%BD%D0%BE%D0%B3%D0%BE%D1%8D%D0%BB%D0%B5%D0%BC%D0%B5%D0%BD%D1%82%D0%BD%D1%8B%D0%B5-%D1%84%D0%BE%D1%80%D0%BC%D0%B0%D0%BB%D1%8C%D0%BD%D1%8B%D0%B5-%D0%B4%D0%B0%D0%BD%D0%BD%D1%8B%D0%B5
 * https://makolyte.com/csharp-how-to-send-a-file-with-httpclient/
 * https://brokul.dev/sending-files-and-additional-data-using-httpclient-in-net-core
 * https://learn.microsoft.com/en-us/answers/questions/405068/httpclient-multipart-form-file-upload
 * https://community.monday.com/t/file-upload-using-http-post-c/20294
 * https://www.reddit.com/r/csharp/comments/142juoq/help_request_how_to_add_the_contents_of_a_file_to/?rdt=33394
 * https://zetcode.com/csharp/httpclient/
 * https://briangrinstead.com/blog/multipart-form-post-in-c/
 * https://www.rms.com/technical-blogs/how-you-can-quickly-and-reliably-upload-large-databases-to-data-bridge-using-multi-part-uploads
 * https://community.progress.com/s/article/how-to-send-a-binary-file-with-the-HTTP-client-as-part-of-a-MIME-message
 * https://puresourcecode.com/dotnet/net6/upload-download-files-using-httpclient/
 * https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-7.0
 * https://docs.developers.optimizely.com/customized-commerce/v1.3.0-service-api-developer-guide/docs/chunk-upload-of-large-files
 * https://scatteredcode.net/how-to-upload-large-files/
 * https://www.codeproject.com/Articles/1034347/Upload-Large-Files-to-MVC-WebAPI-using-Partitionin
 * https://www.stevejgordon.co.uk/sending-and-receiving-json-using-httpclient-with-system-net-http-json
 * https://www.stevejgordon.co.uk/using-httpcompletionoption-responseheadersread-to-improve-httpclient-performance-dotnet
 * https://csharp.hotexamples.com/examples/-/MultipartContent/Add/php-multipartcontent-add-method-examples.html
 * https://learn.microsoft.com/en-us/previous-versions/aspnet/jj856095(v=vs.118)
 * https://learn.microsoft.com/en-us/previous-versions/aspnet/mt174882(v=vs.118)
 * https://learn.microsoft.com/en-us/previous-versions/visualstudio/hh137998(v=vs.118)
 * https://metanit.com/sharp/tutorial/6.5.php
 * https://learn.microsoft.com/ru-ru/dotnet/api/system.net.http.bytearraycontent.createcontentreadstreamasync?view=net-7.0
 * 
 * VPN:
 * https://www.pluralsight.com/courses/asp-dot-net-core-6-dot-net-6-creating-background-services
 * https://www.pluralsight.com/courses/dependency-injection-asp-dot-net-core-6
 * https://www.pluralsight.com/courses/asp-dot-net-core-6-configuration-options
*/

public class RestClient : RestAPI
{
    //?
    public async Task<List<Message>> InboxAsync(int page = 1, int rows = 100)
    {
        throw new NotImplementedException();

        // https://learn.microsoft.com/ru-ru/dotnet/fundamentals/networking/http/httpclient

        //public record class Todo(
        //   int? UserId = null,
        //   int? Id = null,
        //   string? Title = null,
        //   bool? Completed = null);

        //var todos = await HttpClient.GetFromJsonAsync<List<Todo>>("todos?userId=1&completed=false");

        //Console.WriteLine("GET https://jsonplaceholder.typicode.com/todos?userId=1&completed=false HTTP/1.1");
        //todos?.ForEach(Console.WriteLine);
        //Console.WriteLine();

        // Expected output:
        //   GET https://jsonplaceholder.typicode.com/todos?userId=1&completed=false HTTP/1.1
        //   Todo { UserId = 1, Id = 1, Title = delectus aut autem, Completed = False }
        //   Todo { UserId = 1, Id = 2, Title = quis ut nam facilis et officia qui, Completed = False }
        //   Todo { UserId = 1, Id = 3, Title = fugiat veniam minus, Completed = False }
        //   Todo { UserId = 1, Id = 5, Title = laboriosam mollitia et enim quasi adipisci quia provident illum, Completed = False }
        //   Todo { UserId = 1, Id = 6, Title = qui ullam ratione quibusdam voluptatem quia omnis, Completed = False }
        //   Todo { UserId = 1, Id = 7, Title = illo expedita consequatur quia in, Completed = False }
        //   Todo { UserId = 1, Id = 9, Title = molestiae perspiciatis ipsa, Completed = False }
        //   Todo { UserId = 1, Id = 13, Title = et doloremque nulla, Completed = False }
        //   Todo { UserId = 1, Id = 18, Title = dolorum est consequatur ea mollitia in culpa, Completed = False }
    }

    /// <summary>
    /// 3.1.3 Отправка сообщений комплексно по задачам
    /// </summary>
    /// <param name="task"></param>
    /// <param name="title"></param>
    /// <param name="path"></param>
    /// <param name="secret"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> UploadDirectoryAsync(string task, string? title, string path, bool secret = false)
    {
        var msg = new Message
        {
            Task = task,
            Title = title,
            Files = []
        };

        //TODO Encode & Sign

        foreach (var sig in new DirectoryInfo(path).EnumerateFiles("*.sig")) //TODO *.*
        {
            var enc = new FileInfo(Path.ChangeExtension(sig.FullName, "enc"));
            var src = new FileInfo(Path.ChangeExtension(sig.FullName, null));

            if (enc.Exists) // Encrypted (ДСП)
            {
                msg.Files.Add(new MessageFile()
                {
                    Name = enc.Name,
                    Encrypted = true,
                    Size = enc.Length
                });

                msg.Files.Add(new MessageFile()
                {
                    Name = sig.Name,
                    SignedFile = enc.Name,
                    Size = sig.Length
                });
            }
            else if (src.Exists) // Открытая информация
            {
                msg.Files.Add(new MessageFile()
                {
                    Name = src.Name,
                    Size = src.Length
                });

                msg.Files.Add(new MessageFile()
                {
                    Name = sig.Name,
                    SignedFile = src.Name,
                    Size = sig.Length
                });
            }
            else
            {
                throw new FileNotFoundException("Файл не найден", src.Name);
            }
        }

        // Step 1
        var message = await PostMessageRequestAsync(msg);

        foreach (var file in message.Files)
        {
            // Step 2
            var uploadInstruction = await PostUploadRequestAsync(message.Id!, file.Id!);

            // Step3
            await UploadFileAsync(Path.Combine(path, file.Name), file.Size, uploadInstruction.UploadUrl);
        }

        // Step 4
        await PostMessageAsync(message.Id!);
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    public async Task DownloadFileAsync(string url, string filename)
    {
        //TODO Header: string Range - Запрашиваемый диапазон байтов (необязательное поле).

        /*Request:
        	Range – запрашиваемый диапазон байтов(необязательное поле).В случае указания имеет вид: Range: bytes = { диапазон байт}, где диапазон байт от 0 до Size-1.Указание множественных диапазонов не поддерживается.  
Например:
        o Range: bytes = 1024 - 4095, что означает будет скачан диапазон с первого по четвертый килобайты;
        o Range: bytes = 4096 - , означает будет скачан диапазон с четвертого килобайта до конца файла;
        o Range: bytes = -4096, означает будут скачаны последние четыре килобайта файла.
        */

        /*Response:
HTTP 200 – OК (для полного получения); 
HEADER
	Accept-Ranges: bytes – Заголовок информирует клиента о том, что он может запрашивать у сервера фрагменты, указывая их смещения от начала файла в байтах;
	Content-Length: {полный размер загружаемого сообщения};
или
HTTP 206 – Partial content (для получения определённого диапазона, если был указан Range);
HEADER
	Accept-Ranges: bytes;
	Content-Range: bytes {начало фрагмента}-{конец фрагмента}/{полный размер сообщения}, например: Content-Range: bytes 1024-4095/8192, означает, что был предоставлен фрагмент с первого по четвертый килобайты из сообщения в 8 килобайт;
	Content-Length: {размер тела сообщения}, то есть передаваемого фрагмента, например: Content-Length: 1024, означает, что размер фрагмента один килобайт.

BODY – запрашиваемое сообщение целиком или указанный в Range диапазон байт.
        
         */

        //using var stream = await HttpClient.GetStreamAsync(url);
        //using var file = File.OpenWrite(filename);
        //await stream.CopyToAsync(file);

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        //request.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        request.Headers.Range = new RangeHeaderValue(0, ChunkSize - 1);

        using var response = await HttpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.OK) // 200
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            using var file = File.OpenWrite(filename);
            await stream.CopyToAsync(file);

            return;
        }

        if (response.StatusCode == HttpStatusCode.PartialContent) // 206
        {
            //long length = response.Content.Headers.ContentLength ?? throw new ApplicationException("");
            var range = response.Content.Headers.ContentRange ?? throw new ApplicationException("В ответе нет Range");
            
            long from = range.From ?? throw new ApplicationException("В ответе нет Range.From");
            long to = range.To ?? throw new ApplicationException("В ответе нет Range.To");
            long size = range.Length ?? throw new ApplicationException("В ответе нет Range.Length");

            using var stream = MemoryMappedFile.CreateFromFile(filename, FileMode.Create, null, size);
            using var writer = stream.CreateViewAccessor();

            var bytes = await response.Content.ReadAsByteArrayAsync();
            int length = bytes.Length;
            writer.WriteArray(from, bytes, 0, length);

            long remaining = size - length;

            while (remaining > 0)
            {
                from += length;
                to = from + Math.Min(ChunkSize, remaining) - 1;

                using var partRequest = new HttpRequestMessage(HttpMethod.Get, url);
                partRequest.Headers.Range = new RangeHeaderValue(from, to);

                using var partResponse = await HttpClient.SendAsync(partRequest);

                if (partResponse.StatusCode != HttpStatusCode.PartialContent) // 206
                {
                    // HTTP 403 – Forbidden
                    // HTTP 404 – Not found
                    // HTTP 410 – Gone
                    // HTTP 416 – Range Not Satisfiable
                    var err = await partResponse.Content.ReadAsStringAsync();
                    throw new Exception(err);
                }

                bytes = await partResponse.Content.ReadAsByteArrayAsync();
                length = bytes.Length;

                range = response.Content.Headers.ContentRange ?? throw new ApplicationException("В ответе нет Range");
                from = range.From ?? throw new ApplicationException("В ответе нет Range.From");
                to = range.To ?? throw new ApplicationException("В ответе нет Range.To");
                writer.WriteArray(from, bytes, 0, length);

                remaining -= length;
            }

            return;
        }

        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    //    public async Task DownloadFileAsync(string url, string filename)
    //    {
    //        //TODO Header: string Range - Запрашиваемый диапазон байтов (необязательное поле).

    //        /*Request:
    //        	Range – запрашиваемый диапазон байтов(необязательное поле).В случае указания имеет вид: Range: bytes = { диапазон байт}, где диапазон байт от 0 до Size-1.Указание множественных диапазонов не поддерживается.  
    //Например:
    //        o Range: bytes = 1024 - 4095, что означает будет скачан диапазон с первого по четвертый килобайты;
    //        o Range: bytes = 4096 - , означает будет скачан диапазон с четвертого килобайта до конца файла;
    //        o Range: bytes = -4096, означает будут скачаны последние четыре килобайта файла.
    //        */

    //        /*Response:
    //HTTP 200 – OК (для полного получения); 
    //HEADER
    //	Accept-Ranges: bytes – Заголовок информирует клиента о том, что он может запрашивать у сервера фрагменты, указывая их смещения от начала файла в байтах;
    //	Content-Length: {полный размер загружаемого сообщения};
    //или
    //HTTP 206 – Partial content (для получения определённого диапазона, если был указан Range);
    //HEADER
    //	Accept-Ranges: bytes;
    //	Content-Range: bytes {начало фрагмента}-{конец фрагмента}/{полный размер сообщения}, например: Content-Range: bytes 1024-4095/8192, означает, что был предоставлен фрагмент с первого по четвертый килобайты из сообщения в 8 килобайт;
    //	Content-Length: {размер тела сообщения}, то есть передаваемого фрагмента, например: Content-Length: 1024, означает, что размер фрагмента один килобайт.

    //BODY – запрашиваемое сообщение целиком или указанный в Range диапазон байт.

    //         */

    //        //using var stream = await HttpClient.GetStreamAsync(url);
    //        //using var file = File.OpenWrite(filename);
    //        //await stream.CopyToAsync(file);

    //        using var response = await HttpClient.GetAsync(url);

    //        if (response.StatusCode == HttpStatusCode.OK) // 200
    //        {
    //            using var stream = await response.Content.ReadAsStreamAsync();
    //            using var file = File.OpenWrite(filename);
    //            await stream.CopyToAsync(file);
    //        }

    //        if (response.StatusCode == HttpStatusCode.PartialContent) // 206
    //        {
    //            using var stream = await response.Content.ReadAsStreamAsync();
    //            using var file = File.OpenWrite(filename);
    //            await stream.CopyToAsync(file);
    //        }

    //        // HTTP 403 – Forbidden
    //        // HTTP 404 – Not found
    //        // HTTP 410 – Gone
    //        // HTTP 416 – Range Not Satisfiable
    //        var error = await response.Content.ReadAsStringAsync();
    //        throw new Exception(error);
    //    }

    /// <summary>
    /// Загрузить все файлы из видимых по фильтру сообщений (максимум 100 сообщений).
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task<bool> DownloadMessageFilesAsync(MessageFilter filter, string path, bool overwrite = false)
    {
        var messagePages = await GetMessagePagesAsync(filter);

        if (messagePages.Messages.Count == 0) return false;

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        foreach (var message in messagePages.Messages)
        {
            foreach (var file in message.Files)
            {
                await DownloadMessageFileAsync(message.Id, file.Id, path, overwrite);
            }
        }

        return true;
    }
}
