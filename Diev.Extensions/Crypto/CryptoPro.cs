﻿#region License
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

using System.Text;

using Diev.Extensions.Credentials;

using static Diev.Extensions.Exec.Exec;

namespace Diev.Extensions.Crypto;

/// <summary>
/// Класс работы с утилитой командной строки СКЗИ "КриптоПРО CSP".
/// </summary>
public class CryptoPro
{
    private static readonly char[] _separator = [' ', ',', ';'];

    /// <summary>
    /// Исполняемый файл командной строки.
    /// </summary>
    public string Exe { get; set; } = @"C:\Program Files\Crypto Pro\CSP\csptest.exe";


    /// <summary>
    /// Запускать ли программу видимой.
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// Отпечаток своего сертификата.
    /// </summary>
    public string My { get; }

    /// <summary>
    /// Архив отпечатков своего сертификата.
    /// </summary>
    public string[]? MyOld { get; set; }

    /// <summary>
    /// Пароль своего сертификата.
    /// </summary>
    private string? PIN { get; }

    /// <summary>
    /// Команда подписи.
    /// {0} - исходный файл;
    /// {1} - подписанный файл;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    protected static string SignCommand
        => @"-sfsign -sign -silent -in ""{0}"" -out ""{1}"" -my {2} -add -addsigtime";

    /// <summary>
    /// Команда отсоединенной подписи.
    /// {0} - исходный файл;
    /// {1} - файл отсоединенной подписи;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    protected static string SignDetachedCommand
        => @"-sfsign -sign -silent -in ""{0}"" -out ""{1}"" -my {2} -add -addsigtime -detached";

    /// <summary>
    /// Команда проверки и снятия подписи.
    /// {0} - исходный файл;
    /// {1} - чистый файл;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    protected static string VerifyCommand
        => @"-sfsign -verify -silent -in ""{0}"" -out ""{1}"" -my {2}";

    /// <summary>
    /// Команда проверки отсоединенной подписи.
    /// {0} - исходный файл;
    /// {1} - файл отсоединенной подписи;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    protected static string VerifyDetachedCommand
        => @"-sfsign -verify -silent -in ""{0}"" -signature ""{1}"" -my {2} -detached";

    /// <summary>
    /// Команда шифрования.
    /// {0} - исходный файл;
    /// {1} - зашифрованный файл;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    protected static string EncryptCommand
        => @"-sfenc -encrypt -silent -in ""{0}"" -out ""{1}"" -cert {2}"; // -stream -1215gh

    /// <summary>
    /// Команда расшифрования.
    /// {0} - исходный файл;
    /// {1} - расшифрованный файл;
    /// {2} - отпечаток своего сертификата.
    /// </summary>
    protected static string DecryptCommand
        => @"-sfenc -decrypt -silent -in ""{0}"" -out ""{1}"" -my {2}";

    public CryptoPro()
    {
        string filter = "CryptoPro My";
        var cred = CredentialManager.ReadCredential(filter);
        My = cred.UserName
            ?? throw new Exception($"Windows Credential Manager '{filter}' has no UserName.");
        PIN = cred.Password;
    }

    /// <summary>
    /// Подписать файл.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя подписанного файла.</param>
    /// <exception cref="ApplicationException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> SignFileAsync(string file, string resultFile)
    {
        StringBuilder cmd = new();
        cmd.AppendFormat(SignCommand, file, resultFile, My);

        if (PIN != null)
            cmd.Append(" -password ").Append(PIN);

        await StartAsync(Exe, cmd, Visible);

        if (File.Exists(resultFile))
            return true;

        //throw new FileNotFoundException("Signed file not created.", resultFile);
        return false;
    }

    /// <summary>
    /// Подписать файл отсоединенной подписью.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя подписанного файла.</param>
    /// <exception cref="ApplicationException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> SignDetachedFileAsync(string file, string resultFile)
    {
        StringBuilder cmd = new();
        cmd.AppendFormat(SignDetachedCommand, file, resultFile, My);

        if (PIN != null)
            cmd.Append(" -password ").Append(PIN);

        await StartAsync(Exe, cmd, Visible);

        if (File.Exists(resultFile))
            return true;

        //throw new FileNotFoundException("Detached sign file not created.", resultFile);
        return false;
    }

    /// <summary>
    /// Проверить и снять подпись с файла.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя файла без подписи.</param>
    /// <returns>Результат проверки подписи.</returns>
    /// <exception cref="ApplicationException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> VerifyFileAsync(string file, string resultFile)
    {
        string cmdline = string.Format(VerifyCommand, file, resultFile, My);
        int exit = await StartAsync(Exe, cmdline, Visible);

        if (File.Exists(resultFile))
            return exit == 0;

        //throw new FileNotFoundException("Unsigned file not created.", resultFile);
        return false;
    }

    /// <summary>
    /// Проверить отдельную подпись файла.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="signFile">Имя файла отдельной подписи.</param>
    /// <returns>Результат проверки подписи.</returns>
    /// <exception cref="ApplicationException"></exception>
    public async Task<bool> VerifyDetachedFileAsync(string file, string signFile)
    {
        string cmdline = string.Format(VerifyCommand, file, signFile, My);
        return await StartAsync(Exe, cmdline, Visible) == 0;
    }

    /// <summary>
    /// Зашифровать файл.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя зашифрованного файла.</param>
    /// <param name="to">Список отпечатков сертификатов получателей файла, куда будет добавлен и свой.</param>
    /// <exception cref="ApplicationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> EncryptFileAsync(string file, string resultFile, string? to = null)
    {
        // Файлы более 256k (или все) должны шифроваться потоковым методом (-stream)
        // Файлы должны шифроваться по ГОСТ Р 34.12-2015 Кузнечик (-1215gh)

        StringBuilder cmd = new();
        cmd.AppendFormat(EncryptCommand, file, resultFile, My);

        if (to != null)
        {
            foreach (var cert in to.Split(_separator,
                StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries))
            {
                cmd.Append(" -cert ").Append(cert);
            }
        }

        await StartAsync(Exe, cmd, Visible);

        if (File.Exists(resultFile))
            return true;

        //throw new FileNotFoundException("Encrypted file not created.", resultFile);
        return false;
    }

    /// <summary>
    /// Расшифровать файл.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя расшифрованного файла.</param>
    /// <exception cref="ApplicationException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> DecryptFileAsync(string file, string resultFile)
    {
        //TODO file.Size == 0

        StringBuilder cmd = new();
        cmd.AppendFormat(DecryptCommand, file, resultFile, My);

        if (PIN != null)
            cmd.Append(" -password ").Append(PIN);

        await StartAsync(Exe, cmd, Visible);

        if (File.Exists(resultFile))
            return true;

        if (MyOld is null)
            return false;

        foreach (var old in MyOld)
        {
            cmd.Clear().AppendFormat(DecryptCommand, file, resultFile, old);

            if (PIN != null)
                cmd.Append(" -password ").Append(PIN);

            await StartAsync(Exe, cmd, Visible);

            if (File.Exists(resultFile))
                return true;
        }

        //throw new FileNotFoundException("Decrypted file not created.", resultFile);
        return false;
    }
}
