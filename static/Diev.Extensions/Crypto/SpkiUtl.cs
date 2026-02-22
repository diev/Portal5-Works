#region License
/*
Copyright 2022-2025 Dmitrii Evdokimov
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

using System.Runtime.InteropServices;
using System.Text;

using Diev.Extensions.LogFile;

using Diev.Extensions.Tools;

using static Diev.Extensions.Exec.Exec;

namespace Diev.Extensions.Crypto;

/// <summary>
/// Класс работы с утилитой командной строки СКАД "Сигнатура".
/// </summary>
public class SpkiUtl : ICrypto
{
    /// <summary>
    /// Исполняемый файл командной строки.
    /// </summary>
    public string Exe { get; set; }

    /// <summary>
    /// Запускать ли программу видимой.
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// Отпечаток своего сертификата.
    /// </summary>
    public string My { get; set; }

    /// <summary>
    /// Архив отпечатков своего сертификата.
    /// </summary>
    public string[]? MyOld { get; set; }

    /// <summary>
    /// Пароль своего сертификата.
    /// </summary>
    public string? PIN { get; set; }

    /// <summary>
    /// Команда подписи.
    /// {0} - исходный файл;
    /// {1} - подписанный файл.
    /// </summary>
    public string SignCommand { get; set; }

    /// <summary>
    /// Команда отсоединенной подписи.
    /// {0} - исходный файл;
    /// {1} - файл отсоединенной подписи.
    /// </summary>
    public string SignDetachedCommand { get; set; }

    /// <summary>
    /// Команда проверки и снятия подписи.
    /// {0} - исходный файл;
    /// {1} - чистый файл.
    /// </summary>
    public string VerifyCommand { get; set; }

    /// <summary>
    /// Команда проверки отсоединенной подписи.
    /// {0} - исходный файл;
    /// {1} - файл отсоединенной подписи.
    /// </summary>
    public string VerifyDetachedCommand { get; set; }

    /// <summary>
    /// Команда шифрования.
    /// {0} - исходный файл;
    /// {1} - зашифрованный файл;
    /// {2} - номер сертификата получателя.
    /// </summary>
    public string EncryptCommand { get; set; }

    /// <summary>
    /// Команда расшифрования.
    /// {0} - исходный файл;
    /// {1} - расшифрованный файл.
    /// </summary>
    public string DecryptCommand { get; set; }

    public SpkiUtl()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new InvalidOperationException("Операции с SpkiUtil доступны только в Windows.");

        //var cred = CredentialManager.ReadCredential(filter);
        //My = cred.UserName
        My = string.Empty;
        //    ?? throw new Exception($"Windows Credential Manager '{filter}' has no UserName.");
        //PIN = cred.Password;
        PIN = null;

        Exe = @"C:\Program Files\MDPREI\spki\spki1utl.exe";
        SignCommand = "-sign -data {0} -out {1}";
        SignDetachedCommand = "-sign -data {0} -out {1} -detached";
        VerifyCommand = "-verify -in {0} -out {1} -delete 1";
        VerifyDetachedCommand = "-verify -in {0} -signature {1} -detached"; //TODO
        EncryptCommand = "-encrypt -stream -1215gh -1215mac -in {0} -out {1} -reckeyid {2}";
        DecryptCommand = "-decrypt -in {0} -out {1}";
    }

    /// <summary>
    /// Создание отсоединенной электронной подписи с помощью утилиты командной строки.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Двоичный файл создаваемой электронной подписи (p7d).</param>
    /// <returns>Создан ли файл электронной подписи.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> SignDetachedFileAsync(string file, string resultFile)
    {
        if (string.IsNullOrEmpty(file))
            throw new ArgumentNullException(nameof(file));

        if (!File.Exists(file))
            throw new FileNotFoundException("Not found", file);

        StringBuilder cmd = new();
        cmd.AppendFormat(SignDetachedCommand, file, resultFile, My);

        if (PIN is not null)
            cmd.Append(" -password ").Append(PIN);

        var (ExitCode, Output, Error) = await StartWithOutputAsync(Exe, cmd, Visible);
        Logger.TimeLine($"Sign detached {file.PathQuoted()}:{Environment.NewLine}{Output}");

        if (File.Exists(resultFile))
            return true;

        Logger.Line($"Error {ExitCode}:{Environment.NewLine}{Error}");

        //throw new FileNotFoundException("Detached sign file not created.", resultFile);
        return false;
    }

    public Task<bool> DecryptFileAsync(string file, string resultFile)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EncryptFileAsync(string file, string resultFile, string? to = null)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SignFileAsync(string file, string resultFile)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyDetachedFileAsync(string file, string signFile)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyFileAsync(string file, string resultFile)
    {
        throw new NotImplementedException();
    }
}
