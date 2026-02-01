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

using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

using Diev.Extensions.Crypto;
using Diev.Extensions.Tools;
using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Messages.Create;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Interfaces;

using Microsoft.Extensions.Logging;

namespace Diev.Portal5.Tools;

/// <summary>
/// REST API of Portal5.
/// </summary>
public class PortalService(
    ILogger<PortalService> logger,
    IPathService paths,
    IApiService api,
    ICryptoService cryptor,
    HttpClient httpClient
    ) : IPortalService
{
    /// <summary>
    /// 3.1.3 Отправка пакета файлов из папки сообщения комплексно по задачам
    /// </summary>
    /// <param name="task"></param>
    /// <param name="title"></param>
    /// <param name="path"></param>
    /// <returns>Идентификатор нового сообщения (Message.Id)</returns>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<ApiResult<string>> UploadDirectoryAsync(string task, string? title, string path)
    {
        logger.LogInformation("Upload directory {Path}", path.PathQuoted());

        List<DraftMessageFile> draftFiles = [];

        foreach (var sig in new DirectoryInfo(path).EnumerateFiles("*.sig")) //TODO *.*
        {
            var enc = new FileInfo(Path.ChangeExtension(sig.FullName, "enc"));
            var src = new FileInfo(Path.ChangeExtension(sig.FullName, null));

            //string type = "Document";

            //if (src.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase)
            //    && (src.Name.StartsWith("DOVER_CBR_", StringComparison.OrdinalIgnoreCase)
            //        || src.Name.StartsWith("ON_EMCHD_", StringComparison.OrdinalIgnoreCase)))
            //{
            //    type = "PowerOfAttorney"; // МЧД
            //}

            if (enc.Exists) // Encrypted (ДСП)
            {
                draftFiles.Add(new DraftMessageFile
                {
                    Name = enc.Name,
                    Encrypted = true,
                    //FileType = type,
                    Size = enc.Length
                });

                draftFiles.Add(new DraftMessageFile
                {
                    Name = sig.Name,
                    //FileType = "Sign",
                    SignedFile = enc.Name,
                    Size = sig.Length
                });
            }
            else if (src.Exists) // Открытая информация
            {
                draftFiles.Add(new DraftMessageFile
                {
                    Name = src.Name,
                    //FileType = type,
                    Size = src.Length
                });

                draftFiles.Add(new DraftMessageFile
                {
                    Name = sig.Name,
                    //FileType = "Sign",
                    SignedFile = src.Name,
                    Size = sig.Length
                });
            }
            else
            {
                //throw new FileNotFoundException("Файл не найден", src.Name);
                return ApiResult<string>.CreateError($"Файл не найден {src.Name.PathQuoted()}");
            }
        }

        var draft = new DraftMessage
        {
            Task = task,
            Title = title,
            Files = [.. draftFiles]
        };

        // Step 1 - post request for new message
        logger.LogDebug("1. Post request for a new message");

        var draftResult = await api.PostMessageRequestAsync(draft);

        if (!draftResult.OK)
        {
            return ApiResult<string>.CreateError(draftResult.Error);
        }

        var message = draftResult.Data!;
        string msgId = message.Id;
        int sent = 0;

        // Upload files
        foreach (var messageFile in message.Files)
        {
            // Step 2 - post request for new file
            logger.LogDebug("2.{Sent}. Post request for a new file", sent + 1);

            var uploadSessionResult = await api.PostUploadRequestAsync(msgId, messageFile.Id);

            if (!uploadSessionResult.OK)
                break;

            var uploadSession = uploadSessionResult.Data!;
            var expiration = uploadSession.ExpirationDateTime.ToLocalTime();

            // Step3 - upload new file
            logger.LogDebug("3.{Sent}. Upload new file {File}", sent + 1, messageFile.Name.PathQuoted());

            string file = Path.Combine(path, messageFile.Name);
            var uploadFileResult = await api.UploadFileAsync(file, messageFile.Size, uploadSession.UploadUrl);

            if (!uploadFileResult.OK)
                break;

            sent++;

            if (DateTime.Now > expiration)
                break;
        }

        // Step 4 - post ready message
        logger.LogDebug("4. Post ready message");

        if (sent == message.Files.Length)
        {
            var postResult = await api.PostMessageAsync(msgId);

            if (postResult.OK)
            {
                return ApiResult<string>.CreateOK(msgId);
            }
        }

        // Try to clean expired uploaded files
        logger.LogDebug("Try to clean expired uploaded files");

        foreach (var file in message.Files)
        {
            sent--;

            try
            {
                await api.DeleteMessageFileAsync(msgId, file.Id);
            }
            catch { }

            if (sent == 0)
                break;
        }

        return ApiResult<string>.CreateError("Upload directory fail");
    }

    /// <summary>
    /// 3.1.3 Отправка сообщений комплексно по задачам
    /// </summary>
    /// <param name="task"></param>
    /// <param name="title"></param>
    /// <param name="path"></param>
    /// <param name="corrId"></param>
    /// <returns>Идентификатор нового сообщения (Message.Id)</returns>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<ApiResult<string>> UploadEncFileAsync(string task, string? title, string path, Guid? corrId = null)
    {
        logger.LogInformation("Upload file {File}", path.PathQuoted());

        var enc = new FileInfo(path);
        List<DraftMessageFile> draftFiles = [];

        draftFiles.Add(new DraftMessageFile
        {
            Name = enc.Name,
            Encrypted = true,
            Size = enc.Length
        });

        var draft = new DraftMessage
        {
            Task = task,
            Title = title,
            Files = [.. draftFiles]
        };

        if (corrId is not null)
            draft.CorrelationId = corrId.ToString();

        // Step 1 - post request for new message
        logger.LogDebug("1. Post request for a new message");

        var draftResult = await api.PostMessageRequestAsync(draft);

        if (!draftResult.OK)
        {
            return ApiResult<string>.CreateError(draftResult.Error);
        }

        var message = draftResult.Data!;
        string msgId = message.Id;
        var messageFile = message.Files[0];

        // Step 2 - post request for new file
        logger.LogDebug("2. Post request for a new file");

        var uploadSessionResult = await api.PostUploadRequestAsync(msgId, messageFile.Id);

        if (!uploadSessionResult.OK)
        {
            return ApiResult<string>.CreateError(uploadSessionResult.Error);
        }

        var uploadSession = uploadSessionResult.Data!;
        var expiration = uploadSession.ExpirationDateTime.ToLocalTime();

        // Step3 - upload new file
        logger.LogDebug("3. Upload new file {File}", messageFile.Name.PathQuoted());

        string file = Path.Combine(path, messageFile.Name);
        var uploadFileResult = await api.UploadFileAsync(file, messageFile.Size, uploadSession.UploadUrl);

        if (!uploadFileResult.OK)
        {
            return ApiResult<string>.CreateError(uploadFileResult.Error);
        }

        if (DateTime.Now > expiration)
        {
            return ApiResult<string>.CreateError("Time to upload expired");
        }

        // Step 4 - post ready message
        logger.LogDebug("4. Post ready message");

        var postresult = await api.PostMessageAsync(msgId);

        if (postresult.OK)
        {
            return ApiResult<string>.CreateOK(msgId);
        }

        // Try to clean expired uploaded file
        logger.LogDebug("Try to clean expired uploaded file");

        try
        {
            await api.DeleteMessageFileAsync(msgId, messageFile.Id);
        }
        catch { }

        return ApiResult<string>.CreateError("Upload file fail");
    }

    /// <summary>
    /// 3.1.4.1 Для получения всех сообщений с учетом фильтра используется метод GET.
    /// GET: */messages
    /// Осторожно: эта функция загружает в память ВСЕ сообщения - 
    /// без разбиения на страницы по 100 сообщений.
    /// Бонус: в фильтре можно указать несколько задач через запятую.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Все сообщения с учетом фильтра.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<ApiResult<Message[]>> GetMessagesAsync(MessagesFilter filter)
    {
        logger.LogInformation("Get messages");

        if (filter.Task is null || !filter.Task.Contains(','))
        {
            var messages = await GetMessagesCoreAsync(filter);
            return ApiResult<Message[]>.CreateOK(messages.Data);
        }

        List<Message> rangeMessages = [];
        var tasks = filter.Task!.Split(',');

        foreach (var task in tasks)
        {
            filter.Task = task;
            filter.Page = null;

            var rangeResult = await GetMessagesCoreAsync(filter);

            if (!rangeResult.OK)
            {
                return ApiResult<Message[]>.CreateError("Range is null");
            }

            var range = rangeResult.Data!;

            if (range.Length > 0)
            {
                rangeMessages.AddRange(range);
            }
        }

        return ApiResult<Message[]>.CreateOK([.. rangeMessages]);
    }

    private async Task<ApiResult<Message[]>> GetMessagesCoreAsync(MessagesFilter filter)
    {
        List<Message> messages = [];

        // Get first page of 100
        var messagesPageResult = await api.GetMessagesPageAsync(filter);

        if (!messagesPageResult.OK)
        {
            ApiResult<Message[]>.CreateError(messagesPageResult.Error);
        }

        var messagesPage = messagesPageResult.Data!;

        while (messagesPage.Messages.Length > 0)
        {
            // Do action
            messages.AddRange(messagesPage.Messages);

            // Exit if last page
            if (messagesPage.Pages.CurrentPage == messagesPage.Pages.TotalPages)
                break;

            // Get next page of 100
            filter.Page = (uint)messagesPage.Pages.CurrentPage + 1;
            messagesPageResult = await api.GetMessagesPageAsync(filter);

            if (!messagesPageResult.OK)
            {
                ApiResult<Message[]>.CreateError(messagesPageResult.Error);
            }

            messagesPage = messagesPageResult.Data!;
        }

        return ApiResult<Message[]>.CreateOK([.. messages]);
    }

    /// <summary>
    /// Сохранение JSON информации о конкретном сообщении в файл.
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<ApiResult<bool>> DownloadMessageJsonAsync(string msgId, string path, bool overwrite = false)
    {
        logger.LogInformation("Download {File}", path.PathQuoted());

        if (SkipExisting(path, overwrite))
            return ApiResult<bool>.CreateOK(true);

        string url = httpClient.BaseAddress + $"messages/{msgId}";
        using var response = await httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            using var file = File.OpenWrite(path);
            await stream.CopyToAsync(file);
            await stream.FlushAsync();
        }

        return ApiResult<bool>.CreateOK(File.Exists(path));
    }

    /// <summary>
    /// Сохранение JSON информации о конкретном сообщении в файл.
    /// </summary>
    /// <param name="msgId">Уникальный идентификатор сообщения в формате UUID [4].</param>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<ApiResult<bool>> AppendMessageJsonAsync(string msgId, FileStream file)
    {
        string url = httpClient.BaseAddress + $"messages/{msgId}";
        using var response = await httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            await stream.CopyToAsync(file);
            await stream.FlushAsync();

            return ApiResult<bool>.CreateOK(true);
        }

        return ApiResult<bool>.CreateError(response);
    }

    /// <summary>
    /// 3.1.5 Удаление сообщений с учетом фильтра.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>True если все удалены или false если какие-то (или все) могли остаться.</returns>
    public async Task<ApiResult<bool>> DeleteMessagesAsync(MessagesFilter filter)
    {
        var messagesResult = await GetMessagesAsync(filter);

        if (!messagesResult.OK)
        {
            return ApiResult<bool>.CreateError(messagesResult.Error);
        }

        var messages = messagesResult.Data!;
        var ok = true;

        foreach (var message in messages)
        {
            try
            {
                var deleteResult = await api.DeleteMessageAsync(message.Id);

                if (!deleteResult.OK)
                {
                    ok = false;
                }
            }
            catch
            {
                ok = false;
            }
        }

        return ok
            ? ApiResult<bool>.CreateOK(ok)
            : ApiResult<bool>.CreateError("Deleted files not all");
    }

    public async Task<ApiResult<bool>> SaveMessageJsonAsync(Message message, string path)
    {
        using var stream = File.OpenWrite(path);
        await JsonSerializer.SerializeAsync(stream, message, JsonHelper.JsonOptions);

        return File.Exists(path)
            ? ApiResult<bool>.CreateOK(true)
            : ApiResult<bool>.CreateError("Message.json file not saved");
    }

    public async Task<ApiResult<bool>> SaveMessageZipAsync(string id, string path)
    {
        var result = await api.DownloadMessageZipAsync(id, path);
        return result.OK
            ? ApiResult<bool>.CreateOK(true)
            : ApiResult<bool>.CreateError("Message.zip file not saved");
    }

    /// <summary>
    /// Получение регистрации электронного сообщения.
    /// </summary>
    /// <param name="msgId">Идентификатор сообщения.</param>
    /// <param name="minutes">Время ожидания в минутах, прежде чем прекратить попытки.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ApiResult<Message>> CheckStatusAsync(string msgId, int minutes)
    {
        var end = DateTime.Now.AddMinutes(minutes);
        Message? message = null;

        while (DateTime.Now < end)
        {
            var result = await api.GetMessageAsync(msgId);

            if (result.OK)
            {
                message = result.Data!;

                if (message.Status == MessageOutStatus.Registered)
                    return ApiResult<Message>.CreateOK(message); // OK

                if (message.Status == MessageOutStatus.Error ||
                    message.Status == MessageOutStatus.Rejected)
                {
                    if (message.Receipts is not null)
                    {
                        foreach (var receipt in message.Receipts)
                        {
                            if (receipt.Status == ReceiptOutStatus.Error &&
                                receipt.Message is not null)
                                // Найден сертификат не содержащий ИНН=XX3100XXXX
                                // и ОГРН=XX2780000XXXX
                                // (отсутствует доверенность или доверенность
                                // не предоставляет полномочия по работе с задачей
                                // или доверенность не действительна)..
                                // Ошибки при проверке сертификатов:
                                // Ошибка проверки правил в сертификате:
                                // [код статуса = 0, описание статуса = OK,
                                // время = Wed Oct 15 21:10:54 MSK 2025,
                                // имя издателя = CN=ООО "Сертум-Про",
                                // O=ООО "Сертум-Про",
                                // street=ул. Малопрудная, стр. 5, офис 715,
                                // L=Екатеринбург,ST=66 Свердловская область,C=RU,
                                // INNLE=XX7324XXXX,OGRN=XX1667300XXXX,
                                // Email=XX@XXrtum.ru,
                                // имя владельца = CN=XXXXXX XXXXXX XXXXXXXXX,
                                // SN=XXXXXX,GN=XXXXXX XXXXXXXXX,C=RU,
                                // SNILS=XX35060XXXX,INN=XX162378XXXX,
                                // Email=XXXXXXXXX@XXbank.ru,
                                // e-mail = XXXXXXXXX@XXbank.ru,
                                // серийный номер сертификата = XX9531C50052B36DB94170B954FXXXXXXX,
                                // отпечаток = XXXXe750adb368e56dd796ff078a603bad812914342720357301f669XXXXXXXX]
                                // Не найдено доверенностей выданных для ИНН XX162378XXXX
                                return ApiResult<Message>.CreateError(
                                    "Получена ошибка: " + receipt.Message);

                            if (receipt.Status == ReceiptOutStatus.Rejected &&
                                receipt.Message is not null)
                                return ApiResult<Message>.CreateError(
                                    "Получен отказ: " + receipt.Message);
                        }
                    }

                    return ApiResult<Message>.CreateError(
                        "Получены ошибка или отказ, но нет квитанции");
                }
            }

            Thread.Sleep(30000);
        }

        if (message is not null)
        {
            return ApiResult<Message>.CreateError(
                $"За {minutes} минут статус лишь '{message.Status}'");
            // sent, delivered, [-error], processing, [-registered]
        }

        return ApiResult<Message>.CreateError(
            $"За {minutes} минут так ничего и не получено по отправке");
    }

    public async Task<ApiResult<string>> DownloadLastEncryptedFileAsync(MessagesFilter filter, string path)
    {
        // url = "back/rapi2/messages/8a3306a7-2025-4726-8d7c-ae3200aacaf0/files/948b6d20-c122-417c-9b92-2c3a14ec8de3/download";
        // path = "KGR_20220204_132116_request_128779.zip";

        var messagesResult = await GetMessagesAsync(filter);

        if (!messagesResult.OK)
            return ApiResult<string>.CreateError("Не получено никаких сообщений");

        var messages = messagesResult.Data!;
        int count = messages.Length;

        if (count == 0)
            return ApiResult<string>.CreateError("Получено ноль сообщений");

        string? lastName = null;
        string? fileId = null;

        var message = messages[count - 1];
        string msgId = message.Id;

        foreach (var msgFile in message.Files)
        {
            if (msgFile.Encrypted)
            {
                if (msgFile.Name.Equals("form.xml.enc", StringComparison.Ordinal))
                    continue;

                lastName = msgFile.Name; // "KYC_yyyyMMdd.xml.zip.enc";
                fileId = msgFile.Id;
                break;
            }
        }

        if (fileId is null)
            return ApiResult<string>.CreateError(
                $"Не получен Id файла из сообщения '{msgId}'");

        if (!Directory.CreateDirectory(path).Exists)
            return ApiResult<string>.CreateError(
                $"Не удалось создать папку {path.PathQuoted()}");

        string file = Path.Combine(path, lastName!);
        var result = await api.DownloadMessageFileAsync(msgId, fileId, file);

        if (result.OK)
            return ApiResult<string>.CreateOK(file);

        return ApiResult<string>.CreateError(
            $"Не удалось получить файл {lastName?.PathQuoted()}");
    }

    public async Task<ApiResult<MessageInfo>> DecryptMessageZipAsync(Message message, string zip, string path)
    {
        string temp = CreateTempDir();
#if NET10_0_OR_GREATER
        await ZipFile.ExtractToDirectoryAsync(zip, temp, true);
#else
        await AsyncZipFile.ExtractToDirectoryAsync(zip, temp);
#endif

        var source = Directory.GetDirectories(temp)[0];
        string xml = Path.Combine(source, message.Outbox ? "form.xml" : "passport.xml");
        string enc = xml + ".enc";

        if (!File.Exists(xml) && File.Exists(enc))
        {
            await cryptor.DecryptFileAsync(enc, xml);
        }

        var msgInfo = new MessageInfo(message, xml);
        var corrId = message.CorrelationId;

        if (!string.IsNullOrEmpty(corrId))
        {
            var corrMessage = await api.GetMessageAsync(corrId);

            if (corrMessage.OK)
            {
                var corrInfo = new MessageInfo(corrMessage.Data!);
                msgInfo.Notes = "На " + corrInfo.ToString();
            }
        }

        string docs = paths.GetDocStore(message, msgInfo, path);
        await ExtractFilesToDirectoryAsync(message, source, docs);
        msgInfo.FullName = docs;
        string infoText = msgInfo.ToString();
        await File.WriteAllTextAsync(Path.Combine(docs, "info.txt"), infoText);

        Directory.Delete(temp, true);

        return ApiResult<MessageInfo>.CreateOK(msgInfo);
    }

    public async Task<ApiResult<MessageInfo>> DecryptMessageFilesAsync(Message message, string path)
    {
        string temp = CreateTempDir();
        StringBuilder notes = new();

        foreach (var file in message.Files)
        {
            string name = Path.Combine(temp, file.Name);
            var result = await api.DownloadMessageFileAsync(message.Id, file.Id, name, true);

            if (!result.OK)
            {
                string filename = file.Name.PathQuoted();
                logger.LogError("Файл вложения {File} не скачать", filename);
                notes.AppendLine($"- Файл вложения {filename} не скачать");
            }
        }

        string xml = Path.Combine(temp, message.Outbox ? "form.xml" : "passport.xml");
        string enc = xml + ".enc";

        if (!File.Exists(xml) && File.Exists(enc))
        {
            await cryptor.DecryptFileAsync(enc, xml);
        }

        var msgInfo = new MessageInfo(message, xml);
        var corrId = message.CorrelationId;

        if (!string.IsNullOrEmpty(corrId))
        {
            var corrMessage = await api.GetMessageAsync(corrId);

            if (corrMessage.OK)
            {
                var corrInfo = new MessageInfo(corrMessage.Data!);
                notes.Append("На " + corrInfo.ToString());
            }
        }

        string docs = paths.GetDocStore(message, msgInfo, path);
        await ExtractFilesToDirectoryAsync(message, temp, docs);
        msgInfo.FullName = docs;
        msgInfo.Notes = notes.ToString();
        string infoText = msgInfo.ToString();
        await File.WriteAllTextAsync(Path.Combine(docs, "info.txt"), infoText);

        Directory.Delete(temp, true);

        return ApiResult<MessageInfo>.CreateOK(msgInfo);
    }

    public async Task<ApiResult<MessageInfo>> GetMessageInfoAsync(string msgId)
    {
        var result = await api.GetMessageAsync(msgId);

        if (!result.OK)
            return ApiResult<MessageInfo>.CreateError("Сообщение не получено");

        var message = result.Data!;

        foreach (var file in message.Files)
        {
            if (file.Name.StartsWith("passport.xml", StringComparison.OrdinalIgnoreCase)
                || file.Name.StartsWith("form.xml", StringComparison.OrdinalIgnoreCase))
            {
                string xml = GetTempName();

                if (file.Encrypted)
                {
                    string enc = GetTempName();
                    await api.DownloadMessageFileAsync(msgId, file.Id, enc);
                    await cryptor.DecryptFileAsync(enc, xml);
                    File.Delete(enc);
                }
                else
                {
                    await api.DownloadMessageFileAsync(msgId, file.Id, xml);
                }

                var msgInfo = new MessageInfo(message, xml);
                File.Delete(xml);

                return ApiResult<MessageInfo>.CreateOK(msgInfo);
            }
        }

        return ApiResult<MessageInfo>.CreateOK(new MessageInfo(message));
    }

    public async Task ExtractFilesToDirectoryAsync(Message message, string source, string path)
    {
        //if (!Directory.CreateDirectory(path).Exists)
        //    throw new TaskException($"Невозможно создать директорию {path.PathQuoted()}");

        foreach (var file in message.Files)
        {
            //if (SkipFile(source, file)) //TODO
            //    continue;

            string src = Path.Combine(source, file.Name);
            string dst = Path.Combine(path, file.Name);
            string filename = file.Name.PathQuoted();

            if (!File.Exists(src))
            {
                logger.LogError("В пакете не хватает файла {File}", filename);

                try
                {
                    await api.DownloadMessageFileAsync(message.Id, file.Id!, src);
                }
                catch (Exception ex) // 400 - Общая ошибка запроса
                {
                    logger.LogError(ex, "Файл {File} реально не скачать", filename);
                    continue;
                }
            }

            if (!File.Exists(src))
            {
                logger.LogWarning("Файл {File} реально не скачать - пропускаем!", filename);
                continue;
            }

            if (file.Encrypted)
            {
                //logger.LogInformation("Расшифровка {File}", filename);
                await ExtractFileToDirectoryAsync(src, path);
                continue;
            }

            if (file.SignedFile is not null)
                continue;

            if (!File.Exists(dst))
            {
                //logger.LogDebug("Перенос {File}", filename);
                await AsyncFile.MoveToDirectoryAsync(src, path, true);
            }
        }
    }

    public async Task ExtractFileToDirectoryAsync(string src, string path)
    {
        if (src.EndsWith(".enc", StringComparison.OrdinalIgnoreCase))
        {
            src = await DecryptAsync(src);
        }

        if (src.EndsWith(".sig", StringComparison.OrdinalIgnoreCase))
        {
            src = await UnsignAsync(src);
        }

        await AsyncFile.MoveToDirectoryAsync(src, path, true);
    }

    /// <summary>
    /// Расшифровка файла .enc.
    /// </summary>
    /// <param name="enc">Исходный файл .enc.</param>
    /// <returns>Расшифрованный файл без .enc или исходный.</returns>
    public async Task<string> DecryptAsync(string enc)
    {
        string orig = Path.ChangeExtension(enc, null);

        if (await cryptor.DecryptFileAsync(enc, orig))
        {
            File.Delete(enc);
            return orig;
        }

        return enc;
    }

    /// <summary>
    /// Снятие подписи с файла .sig.
    /// </summary>
    /// <param name="sig">Исходный файл .sig.</param>
    /// <returns>Файл со снятой подписью без .sig или исходный.</returns>
    public async Task<string> UnsignAsync(string sig)
    {
        string orig = Path.ChangeExtension(sig, null);

        //await CryptoPro.VerifyFileAsync(sig, path+);
        //await PKCS7.CleanSignAsync(sig, path+);
        if (await ASN1.CleanSignAsync(sig, orig))
        {
            File.Delete(sig);
            return orig;
        }

        return sig;
    }

    public string GetTempPath(string path)
    {
        //TODO Directory.CreateTempSubdirectory(prefix);

        string temp = Path.Combine(path, "TEMP");

        if (Directory.Exists(temp))
            Directory.Delete(temp, true);

        if (Directory.Exists(temp))
            throw new Exception($"Не удалось удалить старую директорию {temp.PathQuoted()}");

        if (!Directory.CreateDirectory(temp).Exists)
            throw new DirectoryNotFoundException($"Не удалось создать новую директорию {temp.PathQuoted()}");

        return temp;
    }

    public string GetTempName() =>
        Path.Combine(Path.GetTempPath(), nameof(Portal5) + Path.GetRandomFileName());

    public string CreateTempDir() =>
        Directory.CreateTempSubdirectory(nameof(Portal5)).FullName;

    public string MakeUrl(string path) =>
        "[InternetShortcut]" + Environment.NewLine +
        "URL=file://localhost/C" + path[2..].Replace('\\', '/').Replace(" ", "%20");

    /// <summary>
    /// Создание файлов подписи и зашифрованного.
    /// </summary>
    /// <param name="path">Исходный файл.</param>
    /// <param name="encryptTo">Отпечатки сертификатов получателей.</param>
    /// <param name="temp">Временная папка с создаваемыми для отправки файлами.</param>
    /// <returns></returns>
    public async Task<ApiResult<bool>> SignAndEncryptToDirectoryAsync(string path, string[] encryptTo, string store) //TODO ///
    {
        string file = Path.GetFileName(path);
        string sig = Path.Combine(store, file + ".sig");
        string enc = Path.Combine(store, file + ".enc");
        string filename = path.PathQuoted();

        if (!await cryptor.SignDetachedFileAsync(path, sig))
            return ApiResult<bool>.CreateError(
                $"Подписать файл {filename} не удалось");

        if (!await cryptor.EncryptFileAsync(path, enc, encryptTo))
            return ApiResult<bool>.CreateError(
                $"Зашифровать файл {filename} не удалось");

        return ApiResult<bool>.CreateOK(true);
    }

    //private static bool SkipFile(string dir, MessageFile file)
    //{
    //    if (file.SignedFile is not null) // file.Name.EndsWith(".sig", StringComparison.Ordinal)
    //        return true;

    //    string path = Path.Combine(dir, file.Name);
    //    string decrypted = Path.ChangeExtension(path, null);
    //    string src = Path.ChangeExtension(decrypted, null);

    //    return file.Encrypted &&
    //        (File.Exists(path) || File.Exists(decrypted) || File.Exists(src));
    //}

    private static bool SkipExisting(string path, bool overwrite)
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
}
