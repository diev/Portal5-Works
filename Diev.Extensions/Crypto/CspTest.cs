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

using System.Runtime.InteropServices;
using System.Text;

using Diev.Extensions.Credentials;
using Diev.Extensions.LogFile;
using Diev.Extensions.Tools;
using static Diev.Extensions.Exec.Exec;

namespace Diev.Extensions.Crypto;

/// <summary>
/// Класс работы с тестовой утилитой командной строки СКЗИ "КриптоПРО CSP".
/// </summary>
public class CspTest : ICrypto
{
    private static readonly char[] _separator = [' ', ',', ';'];

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
    private string? PIN { get; set; }

    /// <summary>
    /// Команда подписи.
    /// {0} - исходный файл;
    /// {1} - подписанный файл;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    public string SignCommand { get; set; }

    /// <summary>
    /// Команда отсоединенной подписи.
    /// {0} - исходный файл;
    /// {1} - файл отсоединенной подписи;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    public string SignDetachedCommand { get; set; }

    /// <summary>
    /// Команда проверки и снятия подписи.
    /// {0} - исходный файл;
    /// {1} - чистый файл;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    public string VerifyCommand { get; set; }

    /// <summary>
    /// Команда проверки отсоединенной подписи.
    /// {0} - исходный файл;
    /// {1} - файл отсоединенной подписи;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    public string VerifyDetachedCommand { get; set; }

    /// <summary>
    /// Команда шифрования.
    /// {0} - исходный файл;
    /// {1} - зашифрованный файл;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    public string EncryptCommand { get; set; }

    /// <summary>
    /// Команда расшифрования.
    /// {0} - исходный файл;
    /// {1} - расшифрованный файл;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    public string DecryptCommand { get; set; }

    public CspTest(string filter = "CryptoPro My")
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) //TODO Linux
            throw new InvalidOperationException("Операции с КриптоПро доступны только в Windows.");

        var cred = CredentialManager.ReadCredential(filter);
        My = cred.UserName
            ?? throw new Exception($"Windows Credential Manager '{filter}' has no UserName.");
        PIN = cred.Password;

        Exe = @"C:\Program Files\Crypto Pro\CSP\csptest.exe";
        SignCommand = @"-sfsign -sign -silent -in ""{0}"" -out ""{1}"" -my {2} -add -addsigtime";
        SignDetachedCommand = @"-sfsign -sign -silent -in ""{0}"" -out ""{1}"" -my {2} -add -addsigtime -detached";
        VerifyCommand = @"-sfsign -verify -silent -in ""{0}"" -out ""{1}"" -my {2}";
        VerifyDetachedCommand = @"-sfsign -verify -silent -in ""{0}"" -signature ""{1}"" -my {2} -detached";
        EncryptCommand = @"-sfenc -encrypt -silent -in ""{0}"" -out ""{1}"" -cert {2}"; // -stream -1215gh
        DecryptCommand = @"-sfenc -decrypt -silent -in ""{0}"" -out ""{1}"" -my {2}";
    }

    /// <summary>
    /// Подписать файл.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя подписанного файла.</param>
    /// <exception cref="Exception"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> SignFileAsync(string file, string resultFile)
    {
        StringBuilder cmd = new();
        cmd.AppendFormat(SignCommand, file, resultFile, My);

        if (PIN is not null)
            cmd.Append(" -password ").Append(PIN);
        var (ExitCode, Output, Error) = await StartWithOutputAsync(Exe, cmd, Visible);
        Logger.TimeLine($"Sign {file.PathQuoted()}:{Environment.NewLine}{Output}");

        if (File.Exists(resultFile))
            return true;

        Logger.Line($"Error {ExitCode}:{Environment.NewLine}{Error}");

        //throw new FileNotFoundException("Signed file not created.", resultFile);
        return false;
    }

    /// <summary>
    /// Подписать файл отсоединенной подписью.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя подписанного файла.</param>
    /// <exception cref="Exception"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> SignDetachedFileAsync(string file, string resultFile)
    {
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

    /// <summary>
    /// Проверить и снять подпись с файла.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя файла без подписи.</param>
    /// <returns>Результат проверки подписи.</returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> VerifyFileAsync(string file, string resultFile)
    {
        string cmdline = string.Format(VerifyCommand, file, resultFile, My);
        var (ExitCode, Output, Error) = await StartWithOutputAsync(Exe, cmdline, Visible);
        Logger.TimeLine($"Verify {file.PathQuoted()}:{Environment.NewLine}{Output}");

        if (File.Exists(resultFile))
            return ExitCode == 0;

        Logger.Line($"Error {ExitCode}:{Environment.NewLine}{Error}");

        //throw new FileNotFoundException("Unsigned file not created.", resultFile);
        return false;
    }

    /// <summary>
    /// Проверить отдельную подпись файла.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="signFile">Имя файла отдельной подписи.</param>
    /// <returns>Результат проверки подписи.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> VerifyDetachedFileAsync(string file, string signFile)
    {
        string cmdline = string.Format(VerifyCommand, file, signFile, My);
        var (ExitCode, Output, Error) = await StartWithOutputAsync(Exe, cmdline, Visible);
        Logger.TimeLine($"Verify detached {file.PathQuoted()}:{Environment.NewLine}{Output}");

        if (Error.Length > 0)
        {
            Logger.Line($"Error {ExitCode}:{Environment.NewLine}{Error}");
        }

        return ExitCode == 0;
    }

    /// <summary>
    /// Зашифровать файл.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя зашифрованного файла.</param>
    /// <param name="to">Список отпечатков сертификатов получателей файла, куда будет добавлен и свой.</param>
    /// <exception cref="Exception"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> EncryptFileAsync(string file, string resultFile, string? to = null)
    {
        // Файлы более 256k (или все) должны шифроваться потоковым методом (-stream)
        // Файлы должны шифроваться по ГОСТ Р 34.12-2015 Кузнечик (-1215gh)

        StringBuilder cmd = new();
        cmd.AppendFormat(EncryptCommand, file, resultFile, My);

        if (to is not null)
        {
            foreach (var cert in to.Split(_separator,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                cmd.Append(" -cert ").Append(cert);
            }
        }

        var (ExitCode, Output, Error) = await StartWithOutputAsync(Exe, cmd, Visible);
        Logger.TimeLine($"Encrypt {file.PathQuoted()}:{Environment.NewLine}{Output}");

        if (File.Exists(resultFile))
            return true;

        Logger.Line($"Error {ExitCode}:{Environment.NewLine}{Error}");

        //throw new FileNotFoundException("Encrypted file not created.", resultFile);
        return false;
    }

    /// <summary>
    /// Расшифровать файл.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя расшифрованного файла.</param>
    /// <exception cref="Exception"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> DecryptFileAsync(string file, string resultFile)
    {
        //TODO file.Size == 0

        StringBuilder cmd = new();
        cmd.AppendFormat(DecryptCommand, file, resultFile, My);

        if (PIN is not null)
            cmd.Append(" -password ").Append(PIN);

        (int ExitCode, string Output, string Error) = await StartWithOutputAsync(Exe, cmd, Visible);
        Logger.TimeLine($"Decrypt {file.PathQuoted()}:{Environment.NewLine}{Output}");

        if (File.Exists(resultFile))
            return true;

        //Logger.Line(@$"Fail: ""{Exe}"" {cmd}");
        Logger.Line($"Error {ExitCode}:{Environment.NewLine}{Error}");

        if (MyOld is null)
            return false;

        foreach (var old in MyOld)
        {
            cmd.Clear().AppendFormat(DecryptCommand, file, resultFile, old);

            if (PIN is not null)
                cmd.Append(" -password ").Append(PIN);

            (ExitCode, Output, Error) = await StartWithOutputAsync(Exe, cmd, Visible);
            Logger.Line($"Try decrypt with {old}{Environment.NewLine}{Output}");

            if (File.Exists(resultFile))
                return true;

            //Logger.Line(@$"Fail: ""{Exe}"" {cmd}");
            Logger.Line($"Error {ExitCode}:{Environment.NewLine}{Error}");
        }

        //throw new FileNotFoundException("Decrypted file not created.", resultFile);
        return false;
    }
}
