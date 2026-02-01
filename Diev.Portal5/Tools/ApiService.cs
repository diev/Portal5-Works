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

using System.IO.MemoryMappedFiles;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using Diev.Portal5.API.Dictionaries;
using Diev.Portal5.API.Errors;
using Diev.Portal5.API.Info;
using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Messages.Create;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Interfaces;

using Microsoft.Extensions.Options;

namespace Diev.Portal5.Tools;

public class ApiService(
    IOptions<ApiSettings> options,
    HttpClient httpClient
    ) : IApiService
{
    #region 3.1.3

    /* 3.1.3 Отправка сообщений */

    /// <summary>
    /// 3.1.3.1 Для создания нового сообщения используется метод POST<br/>
    /// POST: */messages
    /// </summary>
    /// <param name="message">Черновик нового сообщения.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ApiResult<Message>> PostMessageRequestAsync(DraftMessage draft)
    {
        string url = $"{httpClient.BaseAddress}messages";
        using var response = await httpClient.PostAsJsonAsync(url, draft, JsonHelper.JsonOptions)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var message = await response.Content.ReadFromJsonAsync<Message>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return message is null
                    ? ApiResult<Message>.CreateJsonError()
                    : ApiResult<Message>.CreateOK(message);
            }
            catch (Exception e)
            {
                return ApiResult<Message>.CreateExceptionError(e,
                    "Новое сообщение на сервере не создано");
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 401 – Unauthorized
        // HTTP 406 – Not Acceptable
        // HTTP 413 – Message size too large
        // HTTP 422 – Unprocessable Entity
        return ApiResult<Message>.CreateError(response);
    }

    /// <summary>
    /// 3.1.3.2 Для создания сессии отправки HTTP используется метод POST<br/>
    /// POST: */messages/{msgId}/files/{fileId}/createUploadSession
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4], полученный в качестве ответа при вызове метода из 3.5.1.</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4], полученный в качестве ответа при вызове метода из 3.5.1.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ApiResult<UploadSession>> PostUploadRequestAsync(string msgId, string fileId)
    {
        string url = $"{httpClient.BaseAddress}messages/{msgId}/files/{fileId}/createUploadSession";
        using var response = await httpClient.PostAsync(url, null)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK) // 200 Ok
        {
            try
            {
                var session = await response.Content.ReadFromJsonAsync<UploadSession>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return session is null
                    ? ApiResult<UploadSession>.CreateJsonError()
                    : ApiResult<UploadSession>.CreateOK(session);
            }
            catch (Exception e)
            {
                return ApiResult<UploadSession>.CreateExceptionError(e,
                    "Сессия загрузки не создана");
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 404 – Not found
        // HTTP 405 – Invalid input
        return ApiResult<UploadSession>.CreateError(response);
    }

    /// <summary>
    /// 3.1.3.3 Для отправки файла по HTTP используется метод PUT<br/>
    /// PUT: */messages/{msgId}/files/{fileId}
    /// </summary>
    /// <param name="path"></param>
    /// <param name="size"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ApiResult<bool>> UploadFileAsync(string path, long size, string url)
    {
        int chunkSize = options.Value.ChunkSize;

        if (chunkSize == 0 || size <= chunkSize)
        {
            using var file = File.OpenRead(path);
            using var content = new StreamContent(file);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Headers.ContentLength = size;
            content.Headers.ContentRange = new ContentRangeHeaderValue(0, size - 1, size); // $"bytes {from}-{to}/{size}" // 0-127/128

            using var response = await httpClient.PutAsync(url, content)
                .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Created) // 201 Created
            {
                return ApiResult<bool>.CreateOK(true);
            }

            // HTTP 400 – Bad Request
            // HTTP 404 – Not found
            // HTTP 405 – Invalid input
            // HTTP 411 – Length Required
            return ApiResult<bool>.CreateError(response);
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
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Headers.ContentLength = chunkSize;
                content.Headers.ContentRange = new ContentRangeHeaderValue(from, to, size); // $"bytes {from}-{to}/{size}" // 0-127/128

                using var response = await httpClient.PutAsync(url, content)
                    .ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.Accepted)  // 202 Accepted
                {
                    //{"NextExpectedRange":["4096-8191","8192-8713"],"ExpirationDateTime":"2023-11-29T09:38:35Z"}

                    var acceptedRange = await response.Content.ReadFromJsonAsync<AcceptedRange>(JsonHelper.JsonOptions)
                        .ConfigureAwait(false);

                    if (acceptedRange is null)
                        return ApiResult<bool>.CreateJsonError("Ошибка формата JSON AcceptedRange");

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
                return ApiResult<bool>.CreateError(response);
            }

            // remaining
            {
                reader.ReadArray(from, bytes, 0, (int)remaining);

                using var content = new ByteArrayContent(bytes, 0, (int)remaining);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Headers.ContentLength = remaining;
                content.Headers.ContentRange = new ContentRangeHeaderValue(from, to, size);

                using var response = await httpClient.PutAsync(url, content)
                    .ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.Created) // 201 Created
                {
                    return ApiResult<bool>.CreateOK(true);
                }

                // HTTP 400 – Bad Request
                // HTTP 404 – Not found
                // HTTP 405 – Invalid input
                // HTTP 411 – Length Required
                return ApiResult<bool>.CreateError(response);
            }
        }
    }

    /// <summary>
    /// 3.1.3.4 Для подтверждения отправки сообщения используется метод POST<br/>
    /// POST: */messages/{msgId}
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ApiResult<bool>> PostMessageAsync(string msgId)
    {
        string url = $"{httpClient.BaseAddress}messages/{msgId}";
        using var response = await httpClient.PostAsync(url, null)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK) // 200 Ok
        {
            return ApiResult<bool>.CreateOK(true); // no response stream here to check more
        }

        // HTTP 404 – Not found
        // HTTP 406 – Not Acceptable
        // HTTP 422 – Unprocessable Entity
        return ApiResult<bool>.CreateError(response);
    }

    #endregion 3.1.3
    #region 3.1.4

    /* 3.1.4 Получение УИО сообщений, квитанций, файлов и информации. */

    /// <summary>
    /// 3.1.4.1 Для получения всех сообщений с учетом необязательного фильтра
    /// (не более 100 сообщений за один запрос) используется метод GET.<br/>
    /// GET: */messages
    /// </summary>
    /// <param name="filter">Фильтр сообщений.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ApiResult<MessagesPage>> GetMessagesPageAsync(MessagesFilter? filter)
    {
        return await GetMessagesPageAsync(filter?.GetQuery())
            .ConfigureAwait(false);
    }

    /// <summary>
    /// 3.1.4.1 Для получения всех сообщений с учетом необязательного фильтра
    /// (не более 100 сообщений за один запрос) используется метод GET.<br/>
    /// GET: */messages
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Все сообщения с учетом необязательного фильтра (не более 100 сообщений за один запрос).</returns>
    /// <exception cref="Exception"></exception>
    public async Task<ApiResult<MessagesPage>> GetMessagesPageAsync(string? filter)
    {
        string url = $"{httpClient.BaseAddress}messages{filter}";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK) // 200 Ok
        {
            var messages = await response.Content.ReadFromJsonAsync<Message[]>(JsonHelper.JsonOptions)
                .ConfigureAwait(false);

            if (messages is null)
            {
                //TODO throw new NoMessagesException();
                return ApiResult<MessagesPage>.CreateOK(
                    new([], new(0, 0, 0, 0, 0)));
            }

            var pages = new Pagination //TODO extract by return
            (
                GetValue("EPVV-Total"),          // 0, 1, <=100, >100
                GetValue("EPVV-TotalPages"),     // 0, 1, 1    , n
                GetValue("EPVV-CurrentPage"),    // -, 1, 1    , 1
                GetValue("EPVV-PerCurrentPage"), // -, 1, <=100, 100
                GetValue("EPVV-PerNextPage")     // -, -, -    , <100|100
            );

            return ApiResult<MessagesPage>.CreateOK(new(messages, pages));
        }

        // HTTP 400 – Bad Request
        // или
        // HTTP 401 - Unauthorized: ACCOUNT_NOT_FOUND (Аккаунт не найден) - недокументированный ответ
        // HTTP 502 - Bad Gateway - недокументированный ответ с промежуточного шлюза, который возвращает
        // текст своей HTML страницы вместо ожидаемого ответа в JSON с сервера:
        // <!DOCTYPE html>
        // <html lang=en>
        // <meta charset=utf-8>
        // <meta name=viewport content="initial-scale=1, minimum-scale=1, width=device-width">
        // <title>Error 502</title>
        // <style>...</style>
        // <p><b>502 - Bad Gateway.</b>
        // <ins>That’s an error.</ins>
        // <p>Looks like we have got an invalid response from the upstream server.
        // <ins>That’s all we know.</ins>
        if (response.StatusCode == HttpStatusCode.BadGateway) // 502
        {
            var error = new ApiError((int)response.StatusCode,
                "BadGateway", "Ошибка прокси");

            return ApiResult<MessagesPage>.CreateError(error);
        }

        return ApiResult<MessagesPage>.CreateError(response);

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
    /// 3.1.4.2 Для получения данных о конкретном сообщении используется метод GET<br/>
    /// GET: */messages/{msgId}
    /// </summary>
    /// <param name="msgId"></param>
    /// <returns></returns>
    public async Task<ApiResult<Message>> GetMessageAsync(string msgId)
    {
        string url = $"{httpClient.BaseAddress}messages/{msgId}";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var message = await response.Content.ReadFromJsonAsync<Message>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return message is null
                    ? ApiResult<Message>.CreateJsonError()
                    : ApiResult<Message>.CreateOK(message);
            }
            catch (Exception e)
            {
                return ApiResult<Message>.CreateExceptionError(e,
                    $"Сообщение '{msgId}' не получено");
            }
        }

        // HTTP 404 – Not found
        return ApiResult<Message>.CreateError(response);
    }

    /// <summary>
    /// 3.1.4.3 Для скачивания конкретного сообщения используется метод GET<br/>
    /// GET: */ messages/{msgId}/download
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task<ApiResult<bool>> DownloadMessageZipAsync(string msgId, string path, bool overwrite = false)
    {
        string url = $"{httpClient.BaseAddress}messages/{msgId}/download";
        return await DownloadFileAsync(url, path, overwrite)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// 3.1.4.4 Для получения данных о конкретном файле используется метод GET<br/>
    /// GET: */messages/{msgId}/files/{fileId}
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4].</param>
    /// <returns></returns>
    public async Task<ApiResult<MessageFile>> GetMessageFileAsync(string msgId, string fileId)
    {
        string url = $"{httpClient.BaseAddress}messages/{msgId}/files/{fileId}";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var messageFile = await response.Content.ReadFromJsonAsync<MessageFile>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return messageFile is null
                    ? ApiResult<MessageFile>.CreateJsonError()
                    : ApiResult<MessageFile>.CreateOK(messageFile);
            }
            catch (Exception e)
            {
                ApiResult<MessageFile>.CreateExceptionError(e, 
                    $"Информация о файле '{fileId}' для сообщения '{msgId}' не получена");
            }
        }

        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        return ApiResult<MessageFile>.CreateError(response);
    }

    /// <summary>
    /// 3.1.4.5 Для скачивания конкретного файла из конкретного сообщения используется метод GET<br/>
    /// GET: */messages/{msgId}/files/{fileId}/download
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4].</param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task<ApiResult<bool>> DownloadMessageFileAsync(string msgId, string fileId, string path, bool overwrite = false)
    {
        //TODO Header: string Range - Запрашиваемый диапазон байтов (необязательное поле).

        string url = $"{httpClient.BaseAddress}messages/{msgId}/files/{fileId}/download";
        return await DownloadFileAsync(url, path, overwrite)
            .ConfigureAwait(false);

        //TODO
        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        // throw new Exception(await response.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// 3.1.4.6 Для получения данных о квитанциях на сообщение используется метод GET<br/>
    /// GET: */messages/{msgId}/receipts
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <returns></returns>
    public async Task<ApiResult<MessageReceipt[]>> GetMessageReceiptsAsync(string msgId)
    {
        string url = $"{httpClient.BaseAddress}messages/{msgId}/receipts";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var messageReceipts = await response.Content.ReadFromJsonAsync<MessageReceipt[]>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return messageReceipts is null
                    ? ApiResult<MessageReceipt[]>.CreateJsonError()
                    : ApiResult<MessageReceipt[]>.CreateOK(messageReceipts);
            }
            catch (Exception e)
            {
                ApiResult<MessageReceipt[]>.CreateExceptionError(e,
                    $"Квитанции для сообщения '{msgId}' не получены");
            }
        }

        // HTTP 404 – Not found
        return ApiResult<MessageReceipt[]>.CreateError(response);
    }

    /// <summary>
    /// 3.1.4.7 Для получения данных о квитанции на сообщение используется метод GET<br/>
    /// GET: */messages/{msgId}/receipts/{rcptId}
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="rcptId">Уникальный идентификатор квитанции в формате UUID [4].</param>
    /// <returns></returns>
    public async Task<ApiResult<MessageReceipt>> GetMessageReceiptAsync(string msgId, string rcptId)
    {
        string url = $"{httpClient.BaseAddress}messages/{msgId}/receipts/{rcptId}";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var messageReceipt = await response.Content.ReadFromJsonAsync<MessageReceipt>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return messageReceipt is null
                    ? ApiResult<MessageReceipt>.CreateJsonError()
                    : ApiResult<MessageReceipt>.CreateOK(messageReceipt);
            }
            catch (Exception e)
            {
                ApiResult<MessageReceipt>.CreateExceptionError(e,
                    $"Информация о квитанции '{rcptId}' для сообщения '{msgId}' не получена");
            }
        }

        // HTTP 404 – Not found
        return ApiResult<MessageReceipt>.CreateError(response);
    }

    /// <summary>
    /// 3.1.4.8 Для получения данных о файле квитанции на сообщение используется метод GET<br/>
    /// GET: */messages/{msgId}/receipts/{rcptId}/files/{fileId}
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="rcptId">Уникальный идентификатор квитанции в формате UUID [4].</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4].</param>
    /// <returns></returns>
    public async Task<ApiResult<MessageFile>> GetMessageReceiptFileAsync(string msgId, string rcptId, string fileId)
    {
        string url = $"{httpClient.BaseAddress}messages/{msgId}/receipts/{rcptId}/files/{fileId}";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var messageFile = await response.Content.ReadFromJsonAsync<MessageFile>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return messageFile is null
                    ? ApiResult<MessageFile>.CreateJsonError()
                    : ApiResult<MessageFile>.CreateOK(messageFile);
            }
            catch (Exception e)
            {
                return ApiResult<MessageFile>.CreateExceptionError(e,
                    $"Информация о файле '{fileId}' квитанции '{rcptId}' для сообщения '{msgId}' не получена");
            }
        }

        // HTTP 404 – Not found
        return ApiResult<MessageFile>.CreateError(response);
    }

    /// <summary>
    /// 3.1.4.9 Для скачивания файла квитанции на сообщение используется метод GET<br/>
    /// GET: */messages/{msgId}/receipts/{rcptId}/files/{fileId}/download
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="rcptId">Уникальный идентификатор квитанции в формате UUID [4].</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4].</param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task<ApiResult<bool>> DownloadMessageReceiptFileAsync(string msgId, string rcptId, string fileId, string path, bool overwrite = false)
    {
        //TODO Header: string Range - Запрашиваемый диапазон байтов (необязательное поле).

        string url = $"{httpClient.BaseAddress}messages/{msgId}/receipts/{rcptId}/files/{fileId}/download";
        return await DownloadFileAsync(url, path, overwrite)
            .ConfigureAwait(false);

        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        // throw new Exception(await response.Content.ReadAsStringAsync());
    }

    #endregion 3.1.4
    #region 3.1.5

    /* 3.1.5 Удаление сообщений */

    /// <summary>
    /// 3.1.5.1 Для удаления конкретного сообщения используется метод DELETE<br/>
    /// DELETE: */messages/{msgId}
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    public async Task<ApiResult<bool>> DeleteMessageAsync(string msgId)
    {
        string url = $"{httpClient.BaseAddress}messages/{msgId}";
        using var response = await httpClient.DeleteAsync(url)
            .ConfigureAwait(false); // 200 Ok или 404 – Сообщение не найдено

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return ApiResult<bool>.CreateOK(true);
        }

        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        return ApiResult<bool>.CreateError(response);
    }

    /// <summary>
    /// 3.1.5.2 Для удаления конкретного файла или отмены сессии отправки используется метод DELETE<br/>
    /// DELETE: */messages/{msgId}/files/{fileId}
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="fileId">Уникальный идентификатор файла в формате UUID [4].</param>
    public async Task<ApiResult<bool>> DeleteMessageFileAsync(string msgId, string fileId)
    {
        string url = $"{httpClient.BaseAddress}messages/{msgId}/files/{fileId}";
        using var response = await httpClient.DeleteAsync(url)
            .ConfigureAwait(false); // 200 Ok или 404 – Сообщение не найдено

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return ApiResult<bool>.CreateOK(true);
        }

        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        return ApiResult<bool>.CreateError(response);
    }

    #endregion 3.1.5
    #region 3.1.6

    /* 3.1.6 Получение справочной информации */

    /// <summary>
    /// 3.1.6.1 Для получения справочника задач используется метод GET<br/>
    /// GET: */tasks
    /// </summary>
    /// <param name="direction">Направление обмена.
    /// Допустимые значения: 0/1/2.<br/>
    /// 0 - входящие (БР -> ЛК);<br/>
    /// 1 - исходящие (ЛК -> БР);<br/>
    /// 2 - двунаправленные (ЛК -> ЛК).<br/>
    /// Если параметр не указан, возвращается все задачи.<br/>
    /// В случае некорректного указания параметра – ошибка.</param>
    /// <returns></returns>
    public async Task<ApiResult<Tasks>> GetTasksAsync(int? direction = null)
    {
        string url = direction is not null
            ? $"{httpClient.BaseAddress}tasks"
            : $"{httpClient.BaseAddress}tasks?direction={direction}";

        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var tasks = await response.Content.ReadFromJsonAsync<Tasks>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return tasks is null
                    ? ApiResult<Tasks>.CreateJsonError()
                    : ApiResult<Tasks>.CreateOK(tasks);
            }
            catch (Exception e)
            {
                ApiResult<Tasks>.CreateExceptionError(e,
                    "Справочник задач не получен");
            }
        }

        // HTTP 400 – Bad Request
        return ApiResult<Tasks>.CreateError(response);
    }

    /// <summary>
    /// 3.1.6.2 Для получения информации о своём профиле используется метод GET<br/>
    /// GET: */profile
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResult<Profile>> GetProfileAsync()
    {
        string url = $"{httpClient.BaseAddress}profile";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var profile = await response.Content.ReadFromJsonAsync<Profile>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return profile is null
                    ? ApiResult<Profile>.CreateJsonError()
                    : ApiResult<Profile>.CreateOK(profile);
            }
            catch (Exception e)
            {
                ApiResult<Profile>.CreateExceptionError(e,
                    "Информация о профиле не получена");
            }
        }

        // ?
        return ApiResult<Profile>.CreateError(response);
    }

    /// <summary>
    /// 3.1.6.3 Для получения информации о квоте профиля используется метод GET<br/>
    /// GET: */profile/quota
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResult<Quota>> GetQuotaAsync()
    {
        string url = $"{httpClient.BaseAddress}profile/quota";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var quota = await response.Content.ReadFromJsonAsync<Quota>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return quota is null
                    ? ApiResult<Quota>.CreateJsonError()
                    : ApiResult<Quota>.CreateOK(quota);
            }
            catch (Exception e)
            {
                return ApiResult<Quota>.CreateExceptionError(e,
                    "Информация о квоте профиля не получена");
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        return ApiResult<Quota>.CreateError(response);
    }

    /// <summary>
    /// 3.1.6.4 Для получения информации о технических оповещениях используется метод GET<br/>
    /// GET: */notifications
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResult<Notification[]>> GetNotificationsAsync()
    {
        string url = $"{httpClient.BaseAddress}notifications";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var notifications = await response.Content.ReadFromJsonAsync<Notification[]>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return notifications is null
                    ? ApiResult<Notification[]>.CreateJsonError()
                    : ApiResult<Notification[]>.CreateOK(notifications);
            }
            catch (Exception e)
            {
                return ApiResult<Notification[]>.CreateExceptionError(e,
                    "Информация о технических оповещениях не получена");
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        return ApiResult<Notification[]>.CreateError(response);
    }

    /// <summary>
    /// 3.1.6.5 Для получения списка справочников используется метод GET<br/>
    /// GET: */dictionaries
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResult<DictItems>> GetLevelsPageAsync()
    {
        string url = $"{httpClient.BaseAddress}dictionaries";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var items = await response.Content.ReadFromJsonAsync<DictItems>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return items is null
                    ? ApiResult<DictItems>.CreateJsonError()
                    : ApiResult<DictItems>.CreateOK(items);
            }
            catch (Exception e)
            {
                return ApiResult<DictItems>.CreateExceptionError(e,
                    "Список справочников не получен");
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        return ApiResult<DictItems>.CreateError(response);
    }

    /*
     * Примечание: Запрещается запрашивать справочник, предварительно не убедившись в том,
     * что он был обновлён (при запросе списка справочников методом, описанным в пункте 3.8.5,
     * значение поля Date изменилось на актуальную дату).
     */

    /// <summary>
    /// 3.1.6.6 Для получения записей конкретного справочника 1<br/>
    /// (Справочник Тематики 1 уровня),<br/>
    /// но не более 100 записей за один запрос, используется метод GET<br/>
    /// GET: */dictionaries/{dictId}
    /// </summary>
    /// <param name="page"></param>
    /// <param name="dictId">Справочник Тематики 1 уровня</param>
    /// <returns></returns>
    public async Task<ApiResult<Level1ItemsPage>> GetLevels1PageAsync(int page = 1,
        string dictId = "238d0426-6f57-4c0f-8983-1d1addf8c47a")
    {
        string url = $"{httpClient.BaseAddress}dictionaries/{dictId}?page={page}";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var level = await response.Content.ReadFromJsonAsync<Level1ItemsPage>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return level is null
                    ? ApiResult<Level1ItemsPage>.CreateJsonError()
                    : ApiResult<Level1ItemsPage>.CreateOK(level);
            }
            catch (Exception e)
            {
                return ApiResult<Level1ItemsPage>.CreateExceptionError(e,
                    "Список записей уровня 1 не получен");
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        return ApiResult<Level1ItemsPage>.CreateError(response);
    }

    /*
     * Примечание: Запрещается запрашивать справочник, предварительно не убедившись в том,
     * что он был обновлён (при запросе списка справочников методом, описанным в пункте 3.8.5,
     * значение поля Date изменилось на актуальную дату).
     */

    /// <summary>
    /// 3.1.6.6 Для получения записей конкретного справочника 2<br/>
    /// (Справочник адресатов 2 уровня),<br/>
    /// но не более 100 записей за один запрос, используется метод GET<br/>
    /// GET: */dictionaries/{dictId}
    /// </summary>
    /// <param name="page"></param>
    /// <param name="dictId">Справочник адресатов 2 уровня</param>
    /// <returns></returns>
    public async Task<ApiResult<Level2ItemsPage>> GetLevels2PageAsync(int page = 1,
        string dictId = "25338cfb-5713-4634-bc53-a81129483752")
    {
        string url = $"{httpClient.BaseAddress}dictionaries/{dictId}?page={page}";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var level = await response.Content.ReadFromJsonAsync<Level2ItemsPage>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return level is null
                    ? ApiResult<Level2ItemsPage>.CreateJsonError()
                    : ApiResult<Level2ItemsPage>.CreateOK(level);
            }
            catch (Exception e)
            {
                return ApiResult<Level2ItemsPage>.CreateExceptionError(e,
                    "Список записей уровня 2 не получен");
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        return ApiResult<Level2ItemsPage>.CreateError(response);
    }

    /*
     * Примечание: Запрещается запрашивать справочник, предварительно не убедившись в том,
     * что он был обновлён (при запросе списка справочников методом, описанным в пункте 3.8.5,
     * значение поля Date изменилось на актуальную дату).
     */

    /// <summary>
    /// 3.1.6.6 Для получения записей конкретного справочника 3<br/>
    /// (Справочник адресатов 3 уровня),<br/>
    /// но не более 100 записей за один запрос, используется метод GET<br/>
    /// GET: */dictionaries/{dictId}
    /// </summary>
    /// <param name="page"></param>
    /// <param name="dictId">Справочник адресатов 3 уровня</param>
    /// <returns></returns>
    public async Task<ApiResult<Level3ItemsPage>> GetLevels3PageAsync(int page = 1,
        string dictId = "64529d5a-b1d9-453c-96f3-f380ea577314")
    {
        string url = $"{httpClient.BaseAddress}dictionaries/{dictId}?page={page}";
        using var response = await httpClient.GetAsync(url)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            try
            {
                var level = await response.Content.ReadFromJsonAsync<Level3ItemsPage>(JsonHelper.JsonOptions)
                    .ConfigureAwait(false);

                return level is null
                    ? ApiResult<Level3ItemsPage>.CreateJsonError()
                    : ApiResult<Level3ItemsPage>.CreateOK(level);
            }
            catch (Exception e)
            {
                return ApiResult<Level3ItemsPage>.CreateExceptionError(e,
                    "Список записей уровня 3 не получен");
            }
        }

        // HTTP 400 – Bad Request
        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        return ApiResult<Level3ItemsPage>.CreateError(response);
    }

    /// <summary>
    /// 3.1.6.7 Для скачивания конкретного справочника в виде файла используется метод GET<br/>
    /// GET: */dictionaries/{dictId}/download  
    /// </summary>
    /// <param name="dictId">Уникальный идентификатор справочника в формате UUID [4].</param>
    /// <param name="path"></param>
    /// <param name="overwrite"></param>
    /// <returns></returns>
    public async Task<ApiResult<bool>> DownloadLevelsFileAsync(string dictId, string path, bool overwrite = false)
    {
        //TODO В случае успешного ответа возвращается двоичный поток вида application/octet-stream,
        //содержащий zip-архив с двумя файлами в формате xml. Один файл содержит описание структуры
        //справочника, второй - данные запрошенного справочника.
        //В файле данных возвращаются все записи справочника со статусом не равным «удален».
        //Xsd-схемы xml-файлов справочников определены в Приложении И документа [5].

        string url = $"{httpClient.BaseAddress}dictionaries/{dictId}/download";
        //path = Path.Combine(path, $"{id}.zip");

        return await DownloadFileAsync(url, path, overwrite)
            .ConfigureAwait(false);

        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        // throw new Exception(await response.Content.ReadAsStringAsync());
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
    public async Task<ApiResult<bool>> DownloadFileAsync(string url, string path, bool overwrite = false)
    {
        if (SkipExisting(path, overwrite))
            return ApiResult<bool>.CreateOK(true);

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

        //using var stream = await httpClient.GetStreamAsync(url);
        //using var file = File.OpenWrite(filename);
        //await stream.CopyToAsync(file);

        int chunkSize = options.Value.ChunkSize;

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        //request.Headers.Add("Accept", "application/octet-stream"); //??
        //request.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream"); //TODO check Content?

        if (chunkSize > 0)
        {
            request.Headers.Range = new RangeHeaderValue(0, chunkSize - 1); // ask to chunk
        }

        using var response = await httpClient.SendAsync(request)
            .ConfigureAwait(false);
            //HttpCompletionOption.ResponseHeadersRead);

        //using var response = await httpClient.GetPartialAsync(url, 0, chunkSize - 1);

        // 200 Ok (для разового получения)

        if (response.StatusCode == HttpStatusCode.OK) // 200
        {
            using var file = File.OpenWrite(path);
            await response.Content.CopyToAsync(file).ConfigureAwait(false);
            await file.FlushAsync().ConfigureAwait(false);

            if (File.Exists(path))
            {
                return ApiResult<bool>.CreateOK(true);
            }

            var fileError = new ApiError((int)response.StatusCode,
                "Error 404", $"Файл '{path}' из '{url}' не получен");

            return ApiResult<bool>.CreateError(fileError); //TODO exception?
        }

        // 206 Partial content (для получения определённого диапазона)

        if (response.StatusCode == HttpStatusCode.PartialContent) // first 206
        {
            var range = response.Content.Headers.ContentRange;

            if (range is null || range.Length is null)
            {
                var rangeError = new ApiError((int)response.StatusCode,
                    "Range", "В ответе нет Range или Range.Length");

                return ApiResult<bool>.CreateError(rangeError); //TODO exception?
            }

            long remaining = (long)range.Length;

            using var file = File.OpenWrite(path);
            await response.Content.CopyToAsync(file).ConfigureAwait(false);
            response.Content.Dispose();
            remaining -= chunkSize;

            while (remaining > 0)
            {
                using var partRequest = new HttpRequestMessage(HttpMethod.Get, url);
                partRequest.Headers.Range = new RangeHeaderValue(
                    file.Position,
                    file.Position + Math.Min(chunkSize, remaining) - 1);

                using var partResponse = await httpClient.SendAsync(partRequest)
                    .ConfigureAwait(false);
                    //HttpCompletionOption.ResponseHeadersRead);

                //using var partResponse = await httpClient.GetPartialAsync(url,
                //    file.Position,
                //    file.Position + Math.Min(chunkSize, remaining) - 1);

                if (partResponse.StatusCode != HttpStatusCode.PartialContent) // no next 206
                {
                    // HTTP 403 – Forbidden
                    // HTTP 404 – Not found
                    // HTTP 410 – Gone
                    // HTTP 416 – Range Not Satisfiable
                    return ApiResult<bool>.CreateError(response);
                }

                await partResponse.Content.CopyToAsync(file).ConfigureAwait(false);
                await file.FlushAsync().ConfigureAwait(false);
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

            //    using var partResponse = await httpClient.SendAsync(partRequest);

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

            if (File.Exists(path))
            {
                return ApiResult<bool>.CreateOK(true);
            }

            var fileError = new ApiError((int)response.StatusCode,
                "Error 404", $"Файл '{path}' из '{url}' не получен");

            return ApiResult<bool>.CreateError(fileError); //TODO exception?
        }

        // HTTP 403 – Forbidden
        // HTTP 404 – Not found
        // HTTP 410 – Gone
        // HTTP 416 – Range Not Satisfiable
        return ApiResult<bool>.CreateError(response);

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

    //private async Task DoExceptionAsync(HttpResponseMessage response)
    //{
    //    // HTTP 400 – Bad Request
    //    // HTTP 401 – Unauthorized
    //    // HTTP 403 – Forbidden
    //    // HTTP 404 – Not found
    //    // HTTP 405 – Invalid input
    //    // HTTP 406 – Not Acceptable
    //    // HTTP 410 – Gone
    //    // HTTP 411 – Length Required
    //    // HTTP 413 – Message size too large
    //    // HTTP 416 – Range Not Satisfiable
    //    // HTTP 422 – Unprocessable Entity

    //    // или
    //    // HTTP 502 - Bad Gateway - недокументированный ответ с промежуточного шлюза, который возвращает
    //    // текст своей HTML страницы вместо ожидаемого ответа в JSON с сервера:
    //    // <!DOCTYPE html>
    //    // <html lang=en>
    //    // <meta charset=utf-8>
    //    // <meta name=viewport content="initial-scale=1, minimum-scale=1, width=device-width">
    //    // <title>Error 502</title>
    //    // <style>...</style>
    //    // <p><b>502 - Bad Gateway.</b>
    //    // <ins>That’s an error.</ins>
    //    // <p>Looks like we have got an invalid response from the upstream server.
    //    // <ins>That’s all we know.</ins>
    //    int code = (int)response.StatusCode;
    //    string message = await response.Content.ReadAsStringAsync();

    //    if (message.StartsWith('{'))
    //    {
    //        try
    //        {
    //            var json = JsonSerializer.Deserialize<ApiError>(message);

    //            if (json is not null)
    //                message = json.ErrorMessage;
    //        }
    //        catch { }
    //    }
    //    else if (message.StartsWith('<'))
    //    {
    //        message = "See HTML reply"; //TODO parse HTML for <title>
    //    }

    //    DoException($"{code} - {message}");
    //}

    //private void DoException(string message)
    //{
    //    logger.LogError(message);

    //    if (SkipExceptions)
    //        return;

    //    throw new Portal5Exception(message);
    //}
    #endregion Common Helpers
}
