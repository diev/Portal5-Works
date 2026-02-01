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

using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Tools;

namespace Diev.Portal5.Interfaces;

public interface IPortalService
{
    Task<ApiResult<bool>> AppendMessageJsonAsync(string msgId, FileStream file);
    Task<ApiResult<Message>> CheckStatusAsync(string msgId, int minutes);
    string CreateTempDir();
    Task<string> DecryptAsync(string enc);
    Task<ApiResult<MessageInfo>> DecryptMessageFilesAsync(Message message, string path);
    Task<ApiResult<MessageInfo>> DecryptMessageZipAsync(Message message, string zip, string path);
    Task<ApiResult<bool>> DeleteMessagesAsync(MessagesFilter filter);
    Task<ApiResult<string>> DownloadLastEncryptedFileAsync(MessagesFilter filter, string path);
    Task<ApiResult<bool>> DownloadMessageJsonAsync(string msgId, string path, bool overwrite = false);
    Task ExtractFilesToDirectoryAsync(Message message, string source, string path);
    Task ExtractFileToDirectoryAsync(string src, string path);
    Task<ApiResult<MessageInfo>> GetMessageInfoAsync(string msgId);
    Task<ApiResult<Message[]>> GetMessagesAsync(MessagesFilter filter);
    string GetTempName();
    string GetTempPath(string path);
    string MakeUrl(string path);
    Task<ApiResult<bool>> SaveMessageJsonAsync(Message message, string path);
    Task<ApiResult<bool>> SaveMessageZipAsync(string id, string path);
    Task<ApiResult<bool>> SignAndEncryptToDirectoryAsync(string path, string[] encryptTo, string store);
    Task<string> UnsignAsync(string sig);
    Task<ApiResult<string>> UploadDirectoryAsync(string task, string? title, string path);
    Task<ApiResult<string>> UploadEncFileAsync(string task, string? title, string path, Guid? corrId = null);
}
