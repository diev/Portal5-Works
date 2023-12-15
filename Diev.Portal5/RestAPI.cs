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
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using Diev.Portal5.API;
using Diev.Portal5.Interfaces;

namespace Diev.Portal5;

public class RestAPI : IRestAPI
{
    internal HttpClient HttpClient { get; private set; }

    internal string Api { get; private set; }

    public bool Test { get; set; }
    public string Host { get; set; } = "https://portal5.cbr.ru/";
    public string BaseUrl { get; set; } = "back/rapi2/";
    public string Username { get; set; }
    public string Password { get; set; }
    public string TestHost { get; set; } = "https://portal5test.cbr.ru/";
    public string TestBaseUrl { get; set; } = "back/rapi2/";
    public string TestUsername { get; set; }
    public string TestPassword { get; set; }

    public bool UseProxy { get; set; }
    public string ProxyAddress { get; set; }

    // int.MaxValue;
    //0x10000 = 64K = 65536
    //0x40000 = 256K = 262144
    //1000000 = ~1M

    public int ChunkSize { get; set; }

    public bool Overwrite { get; set; }

    public JsonSerializerOptions JsonOptions { get; set; }

    public RestAPI()
    {
        JsonOptions = new JsonSerializerOptions()
        {
            //AllowTrailingCommas = true,
            //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)//, //< -----Вот эта строка Вам поможет с кодировкой
            //WriteIndented = true
        };
    }

    public void Initialize()
    {
        var app = Assembly.GetExecutingAssembly().GetName();

        if (Test)
        {
            Api = TestHost + TestBaseUrl;

            HttpClientHandler handler = new()
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(TestUsername, TestPassword)
            };

            if (UseProxy && ProxyAddress != null)
            {
                handler.Proxy = new WebProxy(new Uri(ProxyAddress)); // or null;
            }

            HttpClient = new(handler, true)
            {
                BaseAddress = new Uri(TestHost),
                Timeout = TimeSpan.FromMinutes(3)
            };

            HttpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(
                    app.Name ?? "TestClient",
                    app.Version?.ToString() ?? "1.0"));
        }
        else
        {
            Api = Host + BaseUrl;

            HttpClientHandler handler = new()
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Username, Password)
            };

            if (UseProxy && ProxyAddress != null)
            {
                handler.Proxy = new WebProxy(new Uri(ProxyAddress)); // or null;
            }

            HttpClient = new(handler, true)
            {
                BaseAddress = new Uri(Host),
                Timeout = TimeSpan.FromMinutes(3)
            };

            HttpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(
                    app.Name ?? "Client",
                    app.Version?.ToString() ?? "1.0"));
        }

        //var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password}"));
        //HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        /*
        // Check connection
        //using var request = new HttpRequestMessage(HttpMethod.Head, "/");
        using var request = new HttpRequestMessage(HttpMethod.Head, HttpClient.BaseAddress"); //TODO
        using var response = await HttpClient.SendAsync(request);
        //return response.EnsureSuccessStatusCode().Content.Headers.ContentLength > 0;
        response.EnsureSuccessStatusCode();
        // 200 OK
        */

        HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (ChunkSize == 0)
        {
            ChunkSize = int.MaxValue;
        }
    }

    #region 3.1.3

    // 3.1.3 Отправка сообщений

    /// <summary>
    /// 3.1.3.1 Для создания нового сообщения используется метод POST
    /// </summary>
    /// <param name="message">Черновик нового сообщения.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<Message> PostMessageRequestAsync(Message message) // draft
    {
        string url = Api + "messages";
        using var response = await HttpClient.PostAsJsonAsync(url, message, JsonOptions);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var content = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Message>(content, JsonOptions);
        }

        // HTTP 400 – Bad Request
        // HTTP 401 – Unauthorized
        // HTTP 406 – Not Acceptable
        // HTTP 413 – Message size too large
        // HTTP 422 – Unprocessable Entity
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /// <summary>
    /// 3.1.3.2 Для создания сессии отправки HTTP используется метод POST
    /// </summary>
    /// <param name="messageId">Уникальный идентификатор сообщения в формате UUID [4], полученный в качестве ответа при вызове метода из 3.5.1.</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4], полученный в качестве ответа при вызове метода из 3.5.1.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<UploadInstruction> PostUploadRequestAsync(string messageId, string fileId)
    {
        string url = Api + $"messages/{messageId}/files/{fileId}/createUploadSession";
        using var response = await HttpClient.PostAsync(url, null);

        if (response.StatusCode == HttpStatusCode.OK) // 200 Ok
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<UploadInstruction>(stream, JsonOptions) ?? new();
        }

        // HTTP 400 – Bad Request
        // HTTP 404 – Not found
        // HTTP 405 – Invalid input
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /// <summary>
    /// 3.1.3.3 Для отправки файла по HTTP используется метод PUT
    /// </summary>
    /// <param name="path"></param>
    /// <param name="size"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task UploadFileAsync(string path, long size, string url)
    {
        if (size <= ChunkSize)
        {
            //var bytes = File.ReadAllBytes(filename);
            //using var content = new ByteArrayContent(bytes);

            using var file = File.OpenRead(path);
            using var content = new StreamContent(file);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Headers.ContentLength = size;
            content.Headers.ContentRange = new ContentRangeHeaderValue(0, size - 1, size); // $"bytes {from}-{to}/{size}" // 0-127/128

            using var response = await HttpClient.PutAsync(url, content);

            if (response.StatusCode == HttpStatusCode.Created) // 201 Created
            {
                // ignored
                //using var reply = response.Content.ReadAsStream();
                //var messageFile = await JsonSerializer.DeserializeAsync<MessageFile>(reply, JsonOptions);

                return;
            }

            // HTTP 400 – Bad Request
            // HTTP 404 – Not found
            // HTTP 405 – Invalid input
            // HTTP 411 – Length Required
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
        else
        {
            var bytes = new byte[ChunkSize]; //TODO stream
            long from = 0;
            long to = ChunkSize - 1;
            long remaining = size;

            using var stream = MemoryMappedFile.CreateFromFile(path);
            using var reader = stream.CreateViewAccessor();

            while (remaining > ChunkSize)
            {
                reader.ReadArray(from, bytes, 0, ChunkSize);

                using var content = new ByteArrayContent(bytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Headers.ContentLength = ChunkSize;
                content.Headers.ContentRange = new ContentRangeHeaderValue(from, to, size); // $"bytes {from}-{to}/{size}" // 0-127/128

                using var response = await HttpClient.PutAsync(url, content);

                if (response.StatusCode == HttpStatusCode.Accepted)  // 202 Accepted
                {
                    //{"NextExpectedRange":["4096-8191","8192-8713"],"ExpirationDateTime":"2023-11-29T09:38:35Z"}

                    using var reply = response.Content.ReadAsStream();
                    var acceptedRange = await JsonSerializer.DeserializeAsync<AcceptedRange>(reply, JsonOptions);
                    var range = acceptedRange.NextExpectedRange[0].Split('-');
                    from = long.Parse(range[0]);
                    to = long.Parse(range[1]);
                    remaining -= ChunkSize;
                    continue;
                }

                // HTTP 400 – Bad Request
                // HTTP 404 – Not found
                // HTTP 405 – Invalid input
                // HTTP 411 – Length Required
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            // remaining
            {
                reader.ReadArray(from, bytes, 0, (int)remaining);

                using var content = new ByteArrayContent(bytes, 0, (int)remaining);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Headers.ContentLength = remaining;
                content.Headers.ContentRange = new ContentRangeHeaderValue(from, to, size);

                using var response = await HttpClient.PutAsync(url, content);

                if (response.StatusCode == HttpStatusCode.Created) // 201 Created
                {
                    // ignored
                    //using var reply = response.Content.ReadAsStream();
                    //var file = await JsonSerializer.DeserializeAsync<MessageFile>(reply, JsonOptions);

                    return;
                }

                // HTTP 400 – Bad Request
                // HTTP 404 – Not found
                // HTTP 405 – Invalid input
                // HTTP 411 – Length Required
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }
    }

    //private async Task CreateCompanyWithStream()
    //{
    //    var companyForCreation = new CompanyForCreationDto
    //    {
    //        Name = "Eagle IT Ltd.",
    //        Country = "USA",
    //        Address = "Eagle IT Street 289"
    //    };

    //    var ms = new MemoryStream();
    //    await JsonSerializer.SerializeAsync(ms, companyForCreation);
    //    ms.Seek(0, SeekOrigin.Begin);

    //    var request = new HttpRequestMessage(HttpMethod.Post, "companies");
    //    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    //    using var requestContent = new StreamContent(ms);
    //    request.Content = requestContent;
    //    requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

    //    using var response = await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
    //    response.EnsureSuccessStatusCode();

    //    var content = await response.Content.ReadAsStreamAsync();
    //    var createdCompany = await JsonSerializer.DeserializeAsync<CompanyDto>(content, JsonOptions);
    //}

    /// <summary>
    /// 3.1.3.4 Для подтверждения отправки сообщения используется метод POST
    /// </summary>
    /// <param name="messageId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task PostMessageAsync(string messageId)
    {
        string url = Api + $"messages/{messageId}";
        using var response = await HttpClient.PostAsync(url, null);

        if (response.StatusCode == HttpStatusCode.OK) // 200 Ok
        {
            return;
        }

        // HTTP 404 – Not found
        // HTTP 406 – Not Acceptable
        // HTTP 422 – Unprocessable Entity
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    #endregion 3.1.3
    #region 3.1.4

    // 3.1.4 Получение УИО сообщений, квитанций, файлов и информации.

    /// <summary>
    /// 3.1.4.1 Для получения всех сообщений с учетом необязательного фильтра (не более 100 сообщений за один запрос) используется метод GET.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<MessagePages> GetMessagePagesAsync(MessageFilter filter, int page = 1)
    {
        string url = Api + "messages" + filter.GetQuery(page);

        //using var request = new HttpRequestMessage(HttpMethod.Get, url);
        //request.Content = JsonContent.Create(filter, null, JsonOptions); //null?

        //using var response = await HttpClient.SendAsync(request);

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK) // 200 Ok
        {
            var messages = await JsonSerializer.DeserializeAsync<List<Message>>(response.Content.ReadAsStream(), JsonOptions) ?? [];

            var pages = new PaginationInfo //TODO extract by return
            {
                TotalRecords = GetValue("EPVV-Total"),
                TotalPages = GetValue("EPVV-TotalPages"),
                CurrentPage = GetValue("EPVV-CurrentPage"),
                PerCurrentPage = GetValue("EPVV-PerCurrentPage"),
                PerNextPage = GetValue("EPVV-PerNextPage"),
                MaxPerPage = 100 // hard const
            };

            return new MessagePages
            {
                Messages = messages,
                PaginationInfo = pages
            };
        }

        // HTTP 400 – Bad Request
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);

        int GetValue(string key)
        {
            if (response.Headers.NonValidated.TryGetValues(key, out var values))
            {
                return int.Parse(values.First());
            }

            return 0; //TODO null?
        }
    }

    /// <summary>
    /// 3.1.4.2 Для получения данных о конкретном сообщении используется метод GET
    /// </summary>
    /// <param name="messageId"></param>
    /// <returns></returns>
    public async Task<Message> GetMessage(string messageId)
    {
        string url = Api + $"messages/{messageId}";
        //return await HttpClient.GetFromJsonAsync<Message>(url, JsonOptions) ?? new(); // 200 Ok или 404 – Сообщение не найдено

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Message>(stream, JsonOptions) ?? new();
        }

        // HTTP 404 – Not found
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /// <summary>
    /// 3.1.4.3 Для скачивания конкретного сообщения используется метод GET
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task DownloadMessageAsync(string messageId, string path, bool overwrite = false)
    {
        string url = Api + $"messages/{messageId}/download";
        
        await DownloadFileAsync(url, path, overwrite);
    }

    /// <summary>
    /// 3.1.4.4 Для получения данных о конкретном файле используется метод GET
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="fileId"></param>
    /// <returns></returns>
    public async Task<MessageFile> GetMessageFileInfo(string messageId, string fileId)
    {
        string url = Api + $"messages/{messageId}/files/{fileId}";
        //return await HttpClient.GetFromJsonAsync<MessageFile>(url, JsonOptions) ?? new(); // 200 Ok (или 404 – Сообщение не найдено?)

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<MessageFile>(stream, JsonOptions) ?? new();
        }

        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /// <summary>
    /// 3.1.4.5 Для скачивания конкретного файла из конкретного сообщения используется метод GET
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="fileId"></param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task DownloadMessageFileAsync(string messageId, string fileId, string path, bool overwrite = false)
    {
        //TODO Header: string Range - Запрашиваемый диапазон байтов (необязательное поле).

        string url = Api + $"messages/{messageId}/files/{fileId}/download";

        await DownloadFileAsync(url, path, overwrite);

        //TODO
        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        //var error = await response.Content.ReadAsStringAsync();
        //throw new Exception(error);
    }

    /// <summary>
    /// 3.1.4.6 Для получения данных о квитанциях на сообщение используется метод GET
    /// </summary>
    /// <param name="messageId"></param>
    /// <returns></returns>
    public async Task<Receipts> GetReceiptInfo(string messageId)
    {
        string url = Api + $"messages/{messageId}/receipts";
        //return await HttpClient.GetFromJsonAsync<Receipts>(url, JsonOptions) ?? new(); // 200 Ok или 404 – Сообщение не найдено

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Receipts>(stream, JsonOptions) ?? new();
        }

        // HTTP 404 – Not found
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /// <summary>
    /// 3.1.4.7 Для получения данных о квитанции на сообщение используется метод GET
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="receiptId"></param>
    /// <returns></returns>
    public async Task<Receipts> GetReceiptInfo(string messageId, string receiptId)
    {
        string url = Api + $"messages/{messageId}/receipts/{receiptId}";
        //return await HttpClient.GetFromJsonAsync<Receipts>(url, JsonOptions) ?? new(); // 200 Ok или 404 – Сообщение не найдено

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Receipts>(stream, JsonOptions) ?? new();
        }

        // HTTP 404 – Not found
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /// <summary>
    /// 3.1.4.8 Для получения данных о файле квитанции на сообщение используется метод GET
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="receiptId"></param>
    /// <param name="fileId"></param>
    /// <returns></returns>
    public async Task<MessageFile> GetReceiptFileInfo(string messageId, string receiptId, string fileId)
    {
        string url = Api + $"messages/{messageId}/receipts/{receiptId}/files/{fileId}";
        //return await HttpClient.GetFromJsonAsync<MessageFile>(url, JsonOptions) ?? new(); // 200 Ok или 404 – Сообщение не найдено

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<MessageFile>(stream, JsonOptions) ?? new();
        }

        // HTTP 404 – Not found
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /// <summary>
    /// 3.1.4.9 Для скачивания файла квитанции на сообщение используется метод GET
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="receiptId"></param>
    /// <param name="fileId"></param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task DownloadReceiptFileAsync(string messageId, string receiptId, string fileId, string path, bool overwrite = false)
    {
        //TODO Header: string Range - Запрашиваемый диапазон байтов (необязательное поле).

        string url = Api + $"messages/{messageId}/receipts/{receiptId}/files/{fileId}/download";

        await DownloadFileAsync(url, path, overwrite);

        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        //var error = await response.Content.ReadAsStringAsync();
        //throw new Exception(error);
    }

    #endregion 3.1.4
    #region 3.1.5

    // 3.1.5 Удаление сообщений

    /// <summary>
    /// 3.1.5.1 Для удаления конкретного сообщения используется метод DELETE
    /// </summary>
    /// <param name="messageId"></param>
    public async Task DeleteMessageAsync(string messageId)
    {
        string url = Api + $"messages/{messageId}";
        using var response = await HttpClient.DeleteAsync(url); // 200 Ok или 404 – Сообщение не найдено

        if (response.StatusCode != HttpStatusCode.OK)
        {
            // HTTP 403 – Forbidden
            // HTTP 404 – Not found
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }

    /// <summary>
    /// 3.1.5.2 Для удаления конкретного файла или отмены сессии отправки используется метод DELETE
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="fileId"></param>
    public async Task DeleteMessageFileAsync(string messageId, string fileId)
    {
        string url = Api + $"messages/{messageId}/files/{fileId}";
        using var response = await HttpClient.DeleteAsync(url); // 200 Ok или 404 – Сообщение не найдено

        if (response.StatusCode != HttpStatusCode.OK)
        {
            // HTTP 403 – Forbidden
            // HTTP 404 – Not found
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }

    #endregion 3.1.5
    #region 3.1.6

    // 3.1.6 Получение справочной информации

    /// <summary>
    /// 3.1.6.1 Для получения справочника задач используется метод GET
    /// </summary>
    /// <param name="direction">Направление обмена.
    /// Допустимые значения: 0/1/2.
    /// 0 - входящие (БР -> ЛК);
    /// 1 - исходящие (ЛК -> БР);
    /// 2 - двунаправленные (ЛК -> ЛК).
    /// Если параметр не указан, возвращается все задачи.
    /// В случае некорректного указания параметра – ошибка.</param>
    /// <returns></returns>
    public async Task<Tasks> GetTasksAsync(int? direction = null)
    {
        string url = Api + "tasks";

        if (direction != null)
        {
            url += $"?direction={direction}";
        }

        //return await HttpClient.GetFromJsonAsync<Tasks>(url) ?? new(); // 200 Ok

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Tasks>(stream, JsonOptions) ?? new();
        }

        // HTTP 400 – Bad Request
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /// <summary>
    /// 3.1.6.2 Для получения информации о своём профиле используется метод GET
    /// </summary>
    /// <returns></returns>
    public async Task<Tasks> GetProfileInfoAsync()
    {
        string url = Api + "profile";
        //return await HttpClient.GetFromJsonAsync<Tasks>(url) ?? new(); // 200 Ok

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Tasks>(stream, JsonOptions) ?? new();
        }

        // ?
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /// <summary>
    /// 3.1.6.3 Для получения информации о квоте профиля используется метод GET
    /// </summary>
    /// <returns></returns>
    public async Task<Quota> GetQuotaAsync()
    {
        string url = Api + "profile/quota";
        //return await HttpClient.GetFromJsonAsync<Quota>(url) ?? new();

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Quota>(stream, JsonOptions) ?? new();
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /// <summary>
    /// 3.1.6.4 Для получения информации о технических оповещениях используется метод GET
    /// </summary>
    /// <returns></returns>
    public async Task<List<Notification>> GetNotificationsAsync()
    {
        string url = Api + "notifications";
        //return await HttpClient.GetFromJsonAsync<List<Notification>>(url) ?? new(); // 200 Ok

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<Notification>>(stream, JsonOptions) ?? [];
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /// <summary>
    /// 3.1.6.5 Для получения списка справочников используется метод GET
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<Levels> GetLevelsAsync(int page = 1)
    {
        string url = Api + $"dictionaries/?page={page}";
        //return await HttpClient.GetFromJsonAsync<Levels>(url) ?? new();

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Levels>(stream, JsonOptions) ?? new();
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /*
     * Примечание: Запрещается запрашивать справочник, предварительно не убедившись в том,
     * что он был обновлён (при запросе списка справочников методом, описанным в пункте 3.8.5,
     * значение поля Date изменилось на актуальную дату).
     */

    /// <summary>
    /// 3.1.6.6 Для получения записей конкретного справочника 1,
    /// но не более 100 записей за один запрос, используется метод GET
    /// </summary>
    /// <param name="page"></param>
    /// <param name="guid"></param>
    /// <returns></returns>
    public async Task<Levels1> GetLevel1Async(int page = 1, string guid = "238d0426-6f57-4c0f-8983-1d1addf8c47a")
    {
        string url = Api + $"dictionaries/{guid}?page={page}";
        //return await HttpClient.GetFromJsonAsync<Levels1>(url) ?? new();

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Levels1>(stream, JsonOptions) ?? new();
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /*
     * Примечание: Запрещается запрашивать справочник, предварительно не убедившись в том,
     * что он был обновлён (при запросе списка справочников методом, описанным в пункте 3.8.5,
     * значение поля Date изменилось на актуальную дату).
     */

    /// <summary>
    /// 3.1.6.6 Для получения записей конкретного справочника 2,
    /// но не более 100 записей за один запрос, используется метод GET
    /// </summary>
    /// <param name="page"></param>
    /// <param name="guid"></param>
    /// <returns></returns>
    public async Task<Levels2> GetLevel2Async(int page = 1, string guid = "25338cfb-5713-4634-bc53-a81129483752")
    {
        string url = Api + $"dictionaries/{guid}?page={page}";
        //return await HttpClient.GetFromJsonAsync<Levels2>(url) ?? new();

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Levels2>(stream, JsonOptions) ?? new();
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /*
     * Примечание: Запрещается запрашивать справочник, предварительно не убедившись в том,
     * что он был обновлён (при запросе списка справочников методом, описанным в пункте 3.8.5,
     * значение поля Date изменилось на актуальную дату).
     */

    /// <summary>
    /// 3.1.6.6 Для получения записей конкретного справочника 3,
    /// но не более 100 записей за один запрос, используется метод GET
    /// </summary>
    /// <param name="page"></param>
    /// <param name="guid"></param>
    /// <returns></returns>
    public async Task<Levels3> GetLevel3Async(int page = 1, string guid = "64529d5a-b1d9-453c-96f3-f380ea577314")
    {
        string url = Api + $"dictionaries/{guid}?page={page}";
        //return await HttpClient.GetFromJsonAsync<Levels3>(url) ?? new();

        using var response = await HttpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Levels3>(stream, JsonOptions) ?? new();
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);
    }

    /// <summary>
    /// 3.1.6.7 Для скачивания конкретного справочника в виде файла используется метод GET
    /// </summary>
    /// <param name="guid">Уникальный идентификатор справочника в формате UUID [4].</param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task DownloadLevelFileAsync(string guid, string path, bool overwrite = false)
    {
        //TODO В случае успешного ответа возвращается двоичный поток вида application/octet-stream,
        //содержащий zip-архив с двумя файлами в формате xml. Один файл содержит описание структуры
        //справочника, второй - данные запрошенного справочника.
        //В файле данных возвращаются все записи справочника со статусом не равным «удален».
        //Xsd-схемы xml-файлов справочников определены в Приложении И документа [5].

        string url = Api + $"dictionaries/{guid}/download";
        //path = Path.Combine(path, $"{guid}.zip");

        await DownloadFileAsync(url, path, overwrite);

        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        //var error = await response.Content.ReadAsStringAsync();
        //throw new Exception(error);
    }

    #endregion 3.1.6
    #region 3.1.7

    // 3.1.7 Взаимодействие с ХМЧД
    // Описанные в п.п.3.1.7.1 - 3.1.7.16 методы доступны только в версии v2.

    #endregion 3.1.7
    #region Common Helpers

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="filename"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task DownloadFileAsync(string url, string path, bool overwrite = false)
    {
        if (File.Exists(path))
        {
            if (overwrite)
            {
                File.Delete(path);
            }
            else
            {
                return;
            }
        }

        //TODO Header: string Range - Запрашиваемый диапазон байтов (необязательное поле).

        /*
        Request:

        - Range – запрашиваемый диапазон байтов(необязательное поле).
        В случае указания имеет вид: Range: bytes = { диапазон байт},
        где диапазон байт от 0 до Size-1.
        Указание множественных диапазонов не поддерживается.

        Например:
        - Range: bytes=1024-4095, что означает будет скачан диапазон с первого по четвертый килобайты;
        - Range: bytes=4096-, означает будет скачан диапазон с четвертого килобайта до конца файла;
        - Range: bytes=-4096, означает будут скачаны последние четыре килобайта файла.

        Response:

        HTTP 200 – OК (для полного получения);
        HEADER
        - Accept-Ranges: bytes – Заголовок информирует клиента о том,
        что он может запрашивать у сервера фрагменты, указывая их смещения от начала файла в байтах;
        - Content-Length: {полный размер загружаемого сообщения};
        или

        HTTP 206 – Partial content (для получения определённого диапазона, если был указан Range);
        HEADER
        - Accept-Ranges: bytes;
        - Content-Range: bytes {начало фрагмента}-{конец фрагмента}/{полный размер сообщения},
        например: Content-Range: bytes 1024-4095/8192, означает, что был предоставлен фрагмент
        с первого по четвертый килобайты из сообщения в 8 килобайт;
        - Content-Length: {размер тела сообщения}, то есть передаваемого фрагмента, например:
        Content-Length: 1024, означает, что размер фрагмента один килобайт.

        BODY – запрашиваемое сообщение целиком или указанный в Range диапазон байт.
        */

        //using var stream = await HttpClient.GetStreamAsync(url);
        //using var file = File.OpenWrite(filename);
        //await stream.CopyToAsync(file);

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        //request.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        request.Headers.Range = new RangeHeaderValue(0, ChunkSize - 1); // ask to chunk

        using var response = await HttpClient.SendAsync(request,
            HttpCompletionOption.ResponseHeadersRead);

        // 200 Ok (для разового получения)

        if (response.StatusCode == HttpStatusCode.OK) // 200
        {
            using var file = File.OpenWrite(path);
            await response.Content.CopyToAsync(file);

            //using var file = File.OpenWrite(path);
            //await response.Content.CopyToAsync(file);

            return;
        }

        // 206 Partial content (для получения определённого диапазона)

        if (response.StatusCode == HttpStatusCode.PartialContent) // first 206
        {
            var range = response.Content.Headers.ContentRange
                ?? throw new ApplicationException("В ответе нет Range");
            long remaining = range.Length
                ?? throw new ApplicationException("В ответе нет Range.Length");

            using var file = File.OpenWrite(path);
            await response.Content.CopyToAsync(file);
            response.Content.Dispose();
            remaining -= ChunkSize;

            while (remaining > 0)
            {
                using var partRequest = new HttpRequestMessage(HttpMethod.Get, url);
                partRequest.Headers.Range = new RangeHeaderValue(
                    file.Position,
                    file.Position + Math.Min(ChunkSize, remaining) - 1);

                using var partResponse = await HttpClient.SendAsync(partRequest,
                    HttpCompletionOption.ResponseHeadersRead);

                if (partResponse.StatusCode != HttpStatusCode.PartialContent) // no next 206
                {
                    // HTTP 403 – Forbidden
                    // HTTP 404 – Not found
                    // HTTP 410 – Gone
                    // HTTP 416 – Range Not Satisfiable
                    var err = await partResponse.Content.ReadAsStringAsync();
                    throw new Exception(err);
                }

                await partResponse.Content.CopyToAsync(file);
                remaining -= ChunkSize;
            }

            //var range = response.Content.Headers.ContentRange ?? throw new ApplicationException("В ответе нет Range");
            //long size = range.Length ?? throw new ApplicationException("В ответе нет Range.Length");

            //using var stream = MemoryMappedFile.CreateFromFile(path, FileMode.Create, null, size);
            //using var writer = stream.CreateViewAccessor();
            //long streamPosition = 0;
            //long remaining = size;

            //var bytes = await response.Content.ReadAsByteArrayAsync();
            //int length = bytes.Length;
            //writer.WriteArray(streamPosition, bytes, 0, length);

            //long remaining = size - length;

            //while (remaining > 0)
            //{
            //    streamPosition += length;
            //    long to = streamPosition + Math.Min(ChunkSize, remaining) - 1;

            //    using var partRequest = new HttpRequestMessage(HttpMethod.Get, url);
            //    partRequest.Headers.Range = new RangeHeaderValue(streamPosition, to);

            //    using var partResponse = await HttpClient.SendAsync(partRequest);

            //    if (partResponse.StatusCode != HttpStatusCode.PartialContent) // 206
            //    {
            //        // HTTP 403 – Forbidden
            //        // HTTP 404 – Not found
            //        // HTTP 410 – Gone
            //        // HTTP 416 – Range Not Satisfiable
            //        var err = await partResponse.Content.ReadAsStringAsync();
            //        throw new Exception(err);
            //    }

            //    bytes = await partResponse.Content.ReadAsByteArrayAsync();
            //    length = bytes.Length;
            //    writer.WriteArray(streamPosition, bytes, 0, length);

            //    remaining -= length;
            //}

            return;
        }

        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception(error);


        /*
        HTTP 404 – Not found

        {
          "HTTPStatus": 404,
          "ErrorCode": "MESSAGE_NOT_FOUND",
          "ErrorMessage": "Невозможно найти сообщение с указанным id",
          "MoreInfo: {}
        }

        {
          "HTTPStatus": 404,
          "ErrorCode":  "FILE_TEMPORARY_NOT_AVAILABLE",
          "ErrorMessage": "Файлы сообщения временно недоступны",
          "MoreInfo": {
            "MissedFiles": [{
              "Id": "string",
              "FileName": "string",
              "RepositoriInfo":
                "RepositoryInfo": {...}
            }]
          }
        }

        HTTP 410 – Gone
        {
          "HTTPStatus": 410,
          "ErrorCode": "FILE_PERMANENTLY_NOT_AVAILABLE",
          "ErrorMessage": "Файлы сообщения более недоступны или задача не предусматривает их хранения",
          "MoreInfo": {
            "MissedFiles": [{
              "Id": "string",
              "FileName": "string".
            }]
          } 
        }

        HTTP 416 – Range Not Satisfiable

        {
          "HTTPStatus": 416,
          "ErrorCode":  "INCORRECT_BYTE_RANGE",
          "ErrorMessage": "В запросе не верно указан диапазон байт",
          "MoreInfo: {}
        }
        */
    }

    #endregion Common Helpers
}
