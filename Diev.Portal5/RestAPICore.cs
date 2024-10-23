#region License
/*
Copyright 2022-2024 Dmitrii Evdokimov
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
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using Diev.Extensions.Credentials;
using Diev.Extensions.Http;
using Diev.Extensions.LogFile;
using Diev.Portal5.API.Dictionaries;
using Diev.Portal5.API.Errors;
using Diev.Portal5.API.Info;
using Diev.Portal5.API.Interfaces;
using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Messages.Create;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Exceptions;

namespace Diev.Portal5;

public class RestAPICore : IRestAPICore
{
    internal string Api { get; set; } = null!;

    public bool SkipExceptions { get; set; }

    public JsonSerializerOptions JsonOptions { get; } = 
        new() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

    /// <summary>
    /// REST API Core of Portal5.
    /// </summary>
    /// <param name="cred">Windows Credential Manager credential.</param>
    /// <param name="trace">Trace HTTP.</param>
    public RestAPICore(Credential cred, bool trace)
    {
        PollyClient.Login(cred, trace);
        SetApi(cred.TargetName.Split(' ')[1]);
    }

    private void SetApi(string host)
    {
        Api = (host.EndsWith('/') ? host : host + '/') + "back/rapi2/";
    }

    #region 3.1.3

    /* 3.1.3 Отправка сообщений */

    /// <summary>
    /// 3.1.3.1 Для создания нового сообщения используется метод POST
    /// POST: */messages
    /// </summary>
    /// <param name="message">Черновик нового сообщения.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<Message?> PostMessageRequestAsync(DraftMessage message) // draft
    {
        string url = Api + "messages";
        using var json = JsonContent.Create(message);
        using var response = await PollyClient.PostAsJsonAsync(url, json);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var content = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Message>(content, JsonOptions);
            }
            catch (Exception e)
            {
                DoException("Новое сообщение на сервере не создано. " + e.Message);
                return null;
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 401 – Unauthorized
        // HTTP 406 – Not Acceptable
        // HTTP 413 – Message size too large
        // HTTP 422 – Unprocessable Entity
        await DoExceptionAsync(response);
        return null;
    }

    /// <summary>
    /// 3.1.3.2 Для создания сессии отправки HTTP используется метод POST
    /// POST: */messages/{msgId}/files/{fileId}/createUploadSession
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4], полученный в качестве ответа при вызове метода из 3.5.1.</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4], полученный в качестве ответа при вызове метода из 3.5.1.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<UploadSession?> PostUploadRequestAsync(string msgId, string fileId)
    {
        string url = Api + $"messages/{msgId}/files/{fileId}/createUploadSession";
        using var response = await PollyClient.PostAsync(url);

        if (response.StatusCode == HttpStatusCode.OK) // 200 Ok
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<UploadSession>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException("Сессия загрузки не создана. " + e.Message);
                return null;
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 404 – Not found
        // HTTP 405 – Invalid input
        await DoExceptionAsync(response);
        return null;
    }

    /// <summary>
    /// 3.1.3.3 Для отправки файла по HTTP используется метод PUT
    /// PUT: */messages/{msgId}/files/{fileId}
    /// </summary>
    /// <param name="path"></param>
    /// <param name="size"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> UploadFileAsync(string path, long size, string url)
    {
        int chunkSize = PollyClient.ChunkSize;

        if (size <= chunkSize)
        {
            //var bytes = File.ReadAllBytes(filename);
            //using var content = new ByteArrayContent(bytes);

            using var file = File.OpenRead(path);
            using var content = new StreamContent(file);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Headers.ContentLength = size;
            content.Headers.ContentRange = new ContentRangeHeaderValue(0, size - 1, size); // $"bytes {from}-{to}/{size}" // 0-127/128

            using var response = await PollyClient.PutAsync(url, content);

            if (response.StatusCode == HttpStatusCode.Created) // 201 Created
            {
                // ignored
                //using var reply = response.Content.ReadAsStream();
                //var messageFile = await JsonSerializer.DeserializeAsync<MessageFile>(reply, JsonOptions);

                return true;
            }

            // HTTP 400 – Bad Request
            // HTTP 404 – Not found
            // HTTP 405 – Invalid input
            // HTTP 411 – Length Required
            await DoExceptionAsync(response);
            return false;
        }
        else
        {
            var bytes = new byte[chunkSize]; //TODO stream
            long from = 0;
            long to = chunkSize - 1;
            long remaining = size;

            using var stream = MemoryMappedFile.CreateFromFile(path);
            using var reader = stream.CreateViewAccessor();

            while (remaining > chunkSize)
            {
                reader.ReadArray(from, bytes, 0, chunkSize);

                using var content = new ByteArrayContent(bytes);
                //content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //content.Headers.ContentLength = chunkSize;
                //content.Headers.ContentRange = new ContentRangeHeaderValue(from, to, size); // $"bytes {from}-{to}/{size}" // 0-127/128

                //using var response = await PollyClient.PutAsync(url, content);

                using var response = await PollyClient.PutPartialAsync(url, content, chunkSize, from, to, size);

                if (response.StatusCode == HttpStatusCode.Accepted)  // 202 Accepted
                {
                    //{"NextExpectedRange":["4096-8191","8192-8713"],"ExpirationDateTime":"2023-11-29T09:38:35Z"}

                    using var reply = response.Content.ReadAsStream();
                    var acceptedRange = await JsonSerializer.DeserializeAsync<AcceptedRange>(reply, JsonOptions);
                    var range = acceptedRange!.NextExpectedRange[0].Split('-');
                    from = long.Parse(range[0]);
                    to = long.Parse(range[1]);
                    remaining -= chunkSize;
                    continue;
                }

                // HTTP 400 – Bad Request
                // HTTP 404 – Not found
                // HTTP 405 – Invalid input
                // HTTP 411 – Length Required
                await DoExceptionAsync(response);
                return false;
            }

            // remaining
            {
                reader.ReadArray(from, bytes, 0, (int)remaining);

                using var content = new ByteArrayContent(bytes, 0, (int)remaining);
                //content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                //content.Headers.ContentLength = remaining;
                //content.Headers.ContentRange = new ContentRangeHeaderValue(from, to, size);

                //using var response = await PollyClient.PutAsync(url, content);

                using var response = await PollyClient.PutPartialAsync(url, content, remaining, from, to, size);

                if (response.StatusCode == HttpStatusCode.Created) // 201 Created
                {
                    // ignored
                    //using var reply = response.Content.ReadAsStream();
                    //var file = await JsonSerializer.DeserializeAsync<MessageFile>(reply, JsonOptions);

                    return true;
                }

                // HTTP 400 – Bad Request
                // HTTP 404 – Not found
                // HTTP 405 – Invalid input
                // HTTP 411 – Length Required
                await DoExceptionAsync(response);
                return false;
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
    /// POST: */messages/{msgId}
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> PostMessageAsync(string msgId)
    {
        string url = Api + $"messages/{msgId}";
        using var response = await PollyClient.PostAsync(url);

        if (response.StatusCode == HttpStatusCode.OK) // 200 Ok
        {
            return true; // no response stream here to check more
        }

        // HTTP 404 – Not found
        // HTTP 406 – Not Acceptable
        // HTTP 422 – Unprocessable Entity
        await DoExceptionAsync(response);
        return false;
    }

    #endregion 3.1.3
    #region 3.1.4

    /* 3.1.4 Получение УИО сообщений, квитанций, файлов и информации. */

    /// <summary>
    /// 3.1.4.1 Для получения всех сообщений с учетом необязательного фильтра (не более 100 сообщений за один запрос) используется метод GET.
    /// GET: */messages
    /// </summary>
    /// <param name="filter"></param>
    /// (если не задан, то вернутся первые 100 сообщений).
    /// Допустимые значения: n > 0 (положительные целые числа, больше 0).
    /// Если запрос страницы не указан, возвращается первая страница сообщений.
    /// Если n за границами диапазона страниц, то вернется пустой массив сообщений.
    /// В случае некорректного номера страницы – ошибка.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<MessagesPage?> GetMessagesPageAsync(MessagesFilter filter)
    {
        return await GetMessagesPageAsync(filter.GetQuery());
    }

    /// <summary>
    /// 3.1.4.1 Для получения всех сообщений с учетом необязательного фильтра (не более 100 сообщений за один запрос) используется метод GET.
    /// GET: */messages
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Все сообщения с учетом необязательного фильтра (не более 100 сообщений за один запрос).</returns>
    /// <exception cref="Exception"></exception>
    public async Task<MessagesPage?> GetMessagesPageAsync(string filter)
    {
        string url = Api + "messages" + filter;
        //using var request = new HttpRequestMessage(HttpMethod.Get, url);
        //request.Content = JsonContent.Create(filter, null, JsonOptions); //null?

        //using var response = await HttpClient.SendAsync(request);

        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK) // 200 Ok
        {
            var messages = await JsonSerializer.DeserializeAsync<IReadOnlyList<Message>>(response.Content.ReadAsStream(), JsonOptions);

            if (messages is null)
            {
                //TODO throw new NoMessagesException();
                return new([], new(
                    0,
                    0,
                    0,
                    0,
                    0,
                    100));
            }

            var pages = new Pagination //TODO extract by return
            (
                GetValue("EPVV-Total"),          // 0, 1, <=100, >100
                GetValue("EPVV-TotalPages"),     // 0, 1, 1    , n
                GetValue("EPVV-CurrentPage"),    // -, 1, 1    , 1
                GetValue("EPVV-PerCurrentPage"), // -, 1, <=100, 100
                GetValue("EPVV-PerNextPage"),    // -, -, -    , <100|100
                100 // hardcoded const
            );

            return new(messages, pages);
        }

        // HTTP 400 – Bad Request
        // HTTP 401 - Unauthorized: ACCOUNT_NOT_FOUND (Аккаунт не найден) - недокументированный ответ
        await DoExceptionAsync(response);
        return null;

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
    /// GET: */messages/{msgId}
    /// </summary>
    /// <param name="msgId"></param>
    /// <returns></returns>
    public async Task<Message?> GetMessageAsync(string msgId)
    {
        string url = Api + $"messages/{msgId}";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Message>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException($"Сообщение '{msgId}' не получено. " + e.Message);
                return null;
            }
        }

        // HTTP 404 – Not found
        await DoExceptionAsync(response);
        return null;
    }

    /// <summary>
    /// 3.1.4.3 Для скачивания конкретного сообщения используется метод GET
    /// GET: */ messages/{msgId}/download
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task<bool> DownloadMessageZipAsync(string msgId, string path, bool overwrite = false)
    {
        string url = Api + $"messages/{msgId}/download";
        await DownloadFileAsync(url, path, overwrite);

        return File.Exists(path);
    }

    /// <summary>
    /// 3.1.4.4 Для получения данных о конкретном файле используется метод GET
    /// GET: */messages/{msgId}/files/{fileId}
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4].</param>
    /// <returns></returns>
    public async Task<MessageFile?> GetMessageFileAsync(string msgId, string fileId)
    {
        string url = Api + $"messages/{msgId}/files/{fileId}";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<MessageFile>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException($"Информация о файле '{fileId}' для сообщения '{msgId}' не получена. " + e.Message);
                return null;
            }
        }

        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        await DoExceptionAsync(response);
        return null;
    }

    /// <summary>
    /// 3.1.4.5 Для скачивания конкретного файла из конкретного сообщения используется метод GET
    /// GET: */messages/{msgId}/files/{fileId}/download
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4].</param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task<bool> DownloadMessageFileAsync(string msgId, string fileId, string path, bool overwrite = false)
    {
        //TODO Header: string Range - Запрашиваемый диапазон байтов (необязательное поле).

        string url = Api + $"messages/{msgId}/files/{fileId}/download";
        await DownloadFileAsync(url, path, overwrite);

        //TODO
        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        // throw new Exception(await response.Content.ReadAsStringAsync());
        return File.Exists(path);
    }

    /// <summary>
    /// 3.1.4.6 Для получения данных о квитанциях на сообщение используется метод GET
    /// GET: */messages/{msgId}/receipts
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <returns></returns>
    public async Task<IReadOnlyList<MessageReceipt>?> GetMessageReceiptsAsync(string msgId)
    {
        string url = Api + $"messages/{msgId}/receipts";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IReadOnlyList<MessageReceipt>>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException($"Квитанции для сообщения '{msgId}' не получены." + e.Message);
                return null;
            }
        }

        // HTTP 404 – Not found
        await DoExceptionAsync(response);
        return null;
    }

    /// <summary>
    /// 3.1.4.7 Для получения данных о квитанции на сообщение используется метод GET
    /// GET: */messages/{msgId}/receipts/{rcptId}
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="rcptId">Уникальный идентификатор квитанции в формате UUID [4].</param>
    /// <returns></returns>
    public async Task<MessageReceipt?> GetMessageReceiptAsync(string msgId, string rcptId)
    {
        string url = Api + $"messages/{msgId}/receipts/{rcptId}";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<MessageReceipt>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException($"Информация о квитанции '{rcptId}' для сообщения '{msgId}' не получена. " + e.Message);
                return null;
            }
        }

        // HTTP 404 – Not found
        await DoExceptionAsync(response);
        return null;
    }

    /// <summary>
    /// 3.1.4.8 Для получения данных о файле квитанции на сообщение используется метод GET
    /// GET: */messages/{msgId}/receipts/{rcptId}/files/{fileId}
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="rcptId">Уникальный идентификатор квитанции в формате UUID [4].</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4].</param>
    /// <returns></returns>
    public async Task<MessageFile?> GetMessageReceiptFileAsync(string msgId, string rcptId, string fileId)
    {
        string url = Api + $"messages/{msgId}/receipts/{rcptId}/files/{fileId}";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<MessageFile>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException($"Информация о файле '{fileId}' квитанции '{rcptId}' для сообщения '{msgId}' не получена. " + e.Message);
                return null;
            }
        }

        // HTTP 404 – Not found
        await DoExceptionAsync(response);
        return null;
    }

    /// <summary>
    /// 3.1.4.9 Для скачивания файла квитанции на сообщение используется метод GET
    /// GET: */messages/{msgId}/receipts/{rcptId}/files/{fileId}/download
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="rcptId">Уникальный идентификатор квитанции в формате UUID [4].</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4].</param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task<bool> DownloadMessageReceiptFileAsync(string msgId, string rcptId, string fileId, string path, bool overwrite = false)
    {
        //TODO Header: string Range - Запрашиваемый диапазон байтов (необязательное поле).

        string url = Api + $"messages/{msgId}/receipts/{rcptId}/files/{fileId}/download";
        await DownloadFileAsync(url, path, overwrite);

        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        // throw new Exception(await response.Content.ReadAsStringAsync());
        return File.Exists(path);
    }

    #endregion 3.1.4
    #region 3.1.5

    /* 3.1.5 Удаление сообщений */

    /// <summary>
    /// 3.1.5.1 Для удаления конкретного сообщения используется метод DELETE
    /// DELETE: */messages/{msgId}
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    public async Task<bool> DeleteMessageAsync(string msgId)
    {
        string url = Api + $"messages/{msgId}";
        using var response = await PollyClient.DeleteAsync(url); // 200 Ok или 404 – Сообщение не найдено

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return true;
        }

        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        await DoExceptionAsync(response);
        return false;
    }

    /// <summary>
    /// 3.1.5.2 Для удаления конкретного файла или отмены сессии отправки используется метод DELETE
    /// DELETE: */messages/{msgId}/files/{fileId}
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4].</param>
    public async Task<bool> DeleteMessageFileAsync(string msgId, string fileId)
    {
        string url = Api + $"messages/{msgId}/files/{fileId}";
        using var response = await PollyClient.DeleteAsync(url); // 200 Ok или 404 – Сообщение не найдено

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return true;
        }

        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        await DoExceptionAsync(response);
        return false;
    }

    #endregion 3.1.5
    #region 3.1.6

    /* 3.1.6 Получение справочной информации */

    /// <summary>
    /// 3.1.6.1 Для получения справочника задач используется метод GET
    /// GET: */tasks
    /// </summary>
    /// <param name="direction">Направление обмена.
    /// Допустимые значения: 0/1/2.
    /// 0 - входящие (БР -> ЛК);
    /// 1 - исходящие (ЛК -> БР);
    /// 2 - двунаправленные (ЛК -> ЛК).
    /// Если параметр не указан, возвращается все задачи.
    /// В случае некорректного указания параметра – ошибка.</param>
    /// <returns></returns>
    public async Task<Tasks?> GetTasksAsync(int? direction = null)
    {
        string url = Api + "tasks";

        if (direction != null)
        {
            url += $"?direction={direction}";
        }

        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Tasks>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException("Справочник задач не получен. " + e.Message);
                return null;
            }
        }

        // HTTP 400 – Bad Request
        await DoExceptionAsync(response);
        return null;
    }

    /// <summary>
    /// 3.1.6.2 Для получения информации о своём профиле используется метод GET
    /// GET: */profile
    /// </summary>
    /// <returns></returns>
    public async Task<Profile?> GetProfileAsync()
    {
        string url = Api + "profile";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Profile>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException("Информация о профиле не получена. " + e.Message);
                return null;
            }
        }

        // ?
        await DoExceptionAsync(response);
        return null;
    }

    /// <summary>
    /// 3.1.6.3 Для получения информации о квоте профиля используется метод GET
    /// GET: */profile/quota
    /// </summary>
    /// <returns></returns>
    public async Task<Quota?> GetQuotaAsync()
    {
        string url = Api + "profile/quota";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Quota>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException("Информация о квоте профиля не получена. " + e.Message);
                return null;
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        await DoExceptionAsync(response);
        return null;
    }

    /// <summary>
    /// 3.1.6.4 Для получения информации о технических оповещениях используется метод GET
    /// GET: */notifications
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyList<Notification>?> GetNotificationsAsync()
    {
        string url = Api + "notifications";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IReadOnlyList<Notification>>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException("Информация о технических оповещениях не получена. " + e.Message);
                return null;
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        await DoExceptionAsync(response);
        return null;
    }

    /// <summary>
    /// 3.1.6.5 Для получения списка справочников используется метод GET
    /// GET: */dictionaries
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<DictItemsPage?> GetLevelsPageAsync(int page = 1)
    {
        string url = Api + $"dictionaries/?page={page}";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<DictItemsPage>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException("Список справочнков не получен." + e.Message);
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        await DoExceptionAsync(response);
        return null;
    }

    /*
     * Примечание: Запрещается запрашивать справочник, предварительно не убедившись в том,
     * что он был обновлён (при запросе списка справочников методом, описанным в пункте 3.8.5,
     * значение поля Date изменилось на актуальную дату).
     */

    /// <summary>
    /// 3.1.6.6 Для получения записей конкретного справочника 1
    /// (Справочник Тематики 1 уровня),
    /// но не более 100 записей за один запрос, используется метод GET
    /// GET: */dictionaries/{dictId}
    /// </summary>
    /// <param name="page"></param>
    /// <param name="dictId">Справочник Тематики 1 уровня</param>
    /// <returns></returns>
    public async Task<Level1ItemsPage?> GetLevels1PageAsync(int page = 1, string dictId = "238d0426-6f57-4c0f-8983-1d1addf8c47a")
    {
        string url = Api + $"dictionaries/{dictId}?page={page}";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Level1ItemsPage>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException("Список записей уровня 1 не получен." + e.Message);
                return null;
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        await DoExceptionAsync(response);
        return null;
    }

    /*
     * Примечание: Запрещается запрашивать справочник, предварительно не убедившись в том,
     * что он был обновлён (при запросе списка справочников методом, описанным в пункте 3.8.5,
     * значение поля Date изменилось на актуальную дату).
     */

    /// <summary>
    /// 3.1.6.6 Для получения записей конкретного справочника 2
    /// (Справочник адресатов 2 уровня),
    /// но не более 100 записей за один запрос, используется метод GET
    /// GET: */dictionaries/{dictId}
    /// </summary>
    /// <param name="page"></param>
    /// <param name="dictId">Справочник адресатов 2 уровня</param>
    /// <returns></returns>
    public async Task<Level2ItemsPage?> GetLevels2PageAsync(int page = 1, string dictId = "25338cfb-5713-4634-bc53-a81129483752")
    {
        string url = Api + $"dictionaries/{dictId}?page={page}";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Level2ItemsPage>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException("Список записей уровня 2 не получен. " + e.Message);
                return null;
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        await DoExceptionAsync(response);
        return null;
    }

    /*
     * Примечание: Запрещается запрашивать справочник, предварительно не убедившись в том,
     * что он был обновлён (при запросе списка справочников методом, описанным в пункте 3.8.5,
     * значение поля Date изменилось на актуальную дату).
     */

    /// <summary>
    /// 3.1.6.6 Для получения записей конкретного справочника 3
    /// (Справочник адресатов 3 уровня),
    /// но не более 100 записей за один запрос, используется метод GET
    /// GET: */dictionaries/{dictId}
    /// </summary>
    /// <param name="page"></param>
    /// <param name="dictId">Справочник адресатов 3 уровня</param>
    /// <returns></returns>
    public async Task<Level3ItemsPage?> GetLevels3PageAsync(int page = 1, string dictId = "64529d5a-b1d9-453c-96f3-f380ea577314")
    {
        string url = Api + $"dictionaries/{dictId}?page={page}";
        using var response = await PollyClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<Level3ItemsPage>(stream, JsonOptions);
            }
            catch (Exception e)
            {
                DoException("Список записей уровня 3 не получен. " + e.Message);
                return null;
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        await DoExceptionAsync(response);
        return null;
    }

    /// <summary>
    /// 3.1.6.7 Для скачивания конкретного справочника в виде файла используется метод GET
    /// GET: */dictionaries/{dictId}/download  
    /// </summary>
    /// <param name="dictId">Уникальный идентификатор справочника в формате UUID [4].</param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task<bool> DownloadLevelsFileAsync(string dictId, string path, bool overwrite = false)
    {
        //TODO В случае успешного ответа возвращается двоичный поток вида application/octet-stream,
        //содержащий zip-архив с двумя файлами в формате xml. Один файл содержит описание структуры
        //справочника, второй - данные запрошенного справочника.
        //В файле данных возвращаются все записи справочника со статусом не равным «удален».
        //Xsd-схемы xml-файлов справочников определены в Приложении И документа [5].

        string url = Api + $"dictionaries/{dictId}/download";
        //path = Path.Combine(path, $"{id}.zip");

        await DownloadFileAsync(url, path, overwrite);

        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        // throw new Exception(await response.Content.ReadAsStringAsync());
        return File.Exists(path);
    }

    #endregion 3.1.6
    #region 3.1.7

    /* 3.1.7 Взаимодействие с ХМЧД */

    // Описанные в п.п.3.1.7.1 - 3.1.7.16 методы доступны только в версии v2.

    // 3.1.7.1 Для создания нового запроса в ХМЧД используется метод POST
    // 3.1.7.2 Для создания сессии отправки HTTP используется метод POST
    // 3.1.7.3 Для отправки файла по HTTP используется метод PUT
    // 3.1.7.4 Для отправки запроса в ХМЧД используется метод POST
    // 3.1.7.5 Для проверки возможности удаления запроса в Истории запросов используется метод POST
    // 3.1.7.6 Для удаления запроса в Истории запросов используется метод DELETE
    // 3.1.7.7 Для получения запросов из Истории запросов используется метод GET
    // 3.1.7.8 Для получения списка загруженных МЧД в ХМЧД используется метод GET
    // 3.1.7.9 Для получения квитанций запроса в ХМЧД используется метод GET
    // 3.1.7.10 Для получения информации о результате регистрации МЧД в ХМЧД используется метод GET
    // 3.1.7.11 Для получения информации о запросе в ХМЧД используется метод GET
    // 3.1.7.12 Для скачивания файла запроса в ХМЧД (с квитанциями) используется метод GET
    // 3.1.7.13 Для удаления квитанций запроса в ХМЧД используется метод DELETE
    // 3.1.7.14 Для простановки признака скачивания МЧД из ХМЧД используется метод PUT
    // 3.1.7.15 Для удаления конкретного файла или отмены сессии отправки используется метод DELETE
    // 3.1.7.16 Для скачивания файла запроса (без квитанций) используется метод GET

    #endregion 3.1.7
    #region 3.2

    /* 3.2 Взаимодействие с использованием сервиса REST-УТА (СПО УТА). */

    // Инициатором электронного обмена может быть как КО, так и Банк России.
    // Прием информации от КО должен осуществляться Порталом "Биврёст"
    // с использованием REST-сервиса. В качестве транспортного адаптера
    // при этом должно использоваться специальное программное обеспечение
    // файлового взаимодействия Банка России (СПО УТА).

    #endregion 3.2
    #region Common Helpers

    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="filename"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task<bool> DownloadFileAsync(string url, string path, bool overwrite = false)
    {
        if (SkipExisting(path, overwrite))
            return true;

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

        //using var stream = await PollyClient.GetStreamAsync(url);
        //using var file = File.OpenWrite(filename);
        //await stream.CopyToAsync(file);

        int chunkSize = PollyClient.ChunkSize;

        //using var request = new HttpRequestMessage(HttpMethod.Get, url);
        //request.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //request.Headers.Range = new RangeHeaderValue(0, chunkSize - 1); // ask to chunk

        //using var response = await PollyClient.SendAsync(request,
        //    HttpCompletionOption.ResponseHeadersRead);

        using var response = await PollyClient.GetPartialAsync(url, 0, chunkSize - 1);

        // 200 Ok (для разового получения)

        if (response.StatusCode == HttpStatusCode.OK) // 200
        {
            using var file = File.OpenWrite(path);
            await response.Content.CopyToAsync(file);
            await file.FlushAsync();

            //using var file = File.OpenWrite(path);
            //await response.Content.CopyToAsync(file);

            return true;
        }

        // 206 Partial content (для получения определённого диапазона)

        if (response.StatusCode == HttpStatusCode.PartialContent) // first 206
        {
            var range = response.Content.Headers.ContentRange;

            if (range == null || range.Length == null)
            {
                DoException("В ответе нет Range или Range.Length");
                return false;
            }

            long remaining = (long)range.Length;

            using var file = File.OpenWrite(path);
            await response.Content.CopyToAsync(file);
            response.Content.Dispose();
            remaining -= chunkSize;

            while (remaining > 0)
            {
                //using var partRequest = new HttpRequestMessage(HttpMethod.Get, url);
                //partRequest.Headers.Range = new RangeHeaderValue(
                //    file.Position,
                //    file.Position + Math.Min(chunkSize, remaining) - 1);

                //using var partResponse = await PollyClient.SendAsync(partRequest,
                //    HttpCompletionOption.ResponseHeadersRead);

                using var partResponse = await PollyClient.GetPartialAsync(url,
                    file.Position,
                    file.Position + Math.Min(chunkSize, remaining) - 1);

                if (partResponse.StatusCode != HttpStatusCode.PartialContent) // no next 206
                {
                    // HTTP 403 – Forbidden
                    // HTTP 404 – Not found
                    // HTTP 410 – Gone
                    // HTTP 416 – Range Not Satisfiable
                    await DoExceptionAsync(response);
                    return false;
                }

                await partResponse.Content.CopyToAsync(file);
                await file.FlushAsync();
                remaining -= chunkSize;
            }

            //var range = response.Content.Headers.ContentRange ?? throw new Exception("В ответе нет Range");
            //long size = range.Length ?? throw new Exception("В ответе нет Range.Length");

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

            //    using var partResponse = await PollyClient.SendAsync(partRequest);

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

            return true;
        }

        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        await DoExceptionAsync(response);
        return false;

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
          "ErrorCode": "FILE_TEMPORARY_NOT_AVAILABLE",
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

        {
          "HTTPStatus": 404,
          "ErrorCode": "FILE_TEMPORARY_NOT_AVAILABLE",
          "ErrorMessage": "Файл сообщения временно недоступен",
          "MoreInfo": {
            "MissedFiles": [{
              "Id": "22109af0-f6a4-4d14-87f9-afca0128a2c0",
              "FileName": "KYCCL_7831001422_3194_20230319_000001.zip.enc"
            },{
              "Id": "6f4953a2-b9d6-4b0b-8f3a-afca0128a2c3",
              "FileName": "KYCCL_7831001422_3194_20230319_000001.zip.sig"
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

    protected static bool SkipExisting(string path, bool overwrite)
    {
        if (File.Exists(path))
        {
            if (overwrite)
            {
                File.Delete(path);
                return false;
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    private async Task DoExceptionAsync(HttpResponseMessage response)
    {
        // HTTP 400 – Bad Request
        // HTTP 401 – Unauthorized
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        // HTTP 405 – Invalid input
        // HTTP 406 – Not Acceptable
        // HTTP 410 – Gone
        // HTTP 411 – Length Required
        // HTTP 413 – Message size too large
        // HTTP 416 – Range Not Satisfiable
        // HTTP 422 – Unprocessable Entity
        int code = (int)response.StatusCode;
        string message = await response.Content.ReadAsStringAsync();

        if (message.StartsWith('{'))
        {
            try
            {
                var json = JsonSerializer.Deserialize<Error4XX>(message);

                if (json != null)
                    message = json.ErrorMessage;
            }
            catch { }
        }

        DoException($"{code} - {message}");
    }

    private void DoException(string message)
    {
        Logger.TimeLine(message);

        if (SkipExceptions)
            return;

        throw new Portal5Exception(message);
    }
    #endregion Common Helpers
}
