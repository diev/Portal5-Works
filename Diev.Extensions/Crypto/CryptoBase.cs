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

using System.Text;

using Diev.Extensions.Exec;
using Diev.Extensions.Tools;

using Microsoft.Extensions.Logging;

namespace Diev.Extensions.Crypto;

public abstract class CryptoBase : ICryptoService
{
    protected ILogger logger = null!;
    protected CryptoSettings settings = null!;
    protected IExecService exec = null!;

    protected abstract string AppendPIN(string pin);
    protected abstract string AppendCert(string cert);

    /// <summary>
    /// Расшифровать файл.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя расшифрованного файла.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> DecryptFileAsync(string file, string resultFile)
    {
        //TODO file.Size == 0

        if (string.IsNullOrEmpty(file))
            throw new ArgumentNullException(nameof(file));

        if (!File.Exists(file))
            throw new FileNotFoundException("Not found", file);

        if (settings.Decrypt is null)
            throw new InvalidCastException("Crypto Settings have no 'Decrypt'");

        if (settings.Util is null)
            throw new InvalidCastException("Crypto Settings have no 'Util'");

        StringBuilder cmd = new();
        cmd.AppendFormat(settings.Decrypt, file, resultFile, settings.My);

        if (!string.IsNullOrEmpty(settings.PIN))
            cmd.Append(AppendPIN(settings.PIN));

        logger.LogInformation("Decrypt {File}", Path.GetFileName(file).PathQuoted());

        (int ExitCode, string Output, string Error) =
            await exec.StartWithOutputAsync(settings.Util, cmd.ToString(), settings.Visible);

        logger.LogDebug("{Output}", Output);

        if (File.Exists(resultFile))
            return true;

        logger.LogError("Decrypt error {ExitCode}", ExitCode);

        if (!string.IsNullOrEmpty(Error))
            logger.LogDebug("{Error}", Error);

        foreach (var old in settings.MyOld)
        {
            cmd.Clear().AppendFormat(settings.Decrypt, file, resultFile, old);

            if (!string.IsNullOrEmpty(settings.PIN))
                cmd.Append(AppendPIN(settings.PIN));

            logger.LogInformation("Try decrypt with {Old}", old);

            (ExitCode, Output, Error) =
                await exec.StartWithOutputAsync(settings.Util, cmd.ToString(), settings.Visible);

            logger.LogDebug("{Output}", Output);

            if (File.Exists(resultFile))
                return true;

            logger.LogError("Decrypt error {ExitCode}", ExitCode);
            logger.LogDebug("{Error}", Error);
        }

        //throw new FileNotFoundException("Decrypted file not created", resultFile);
        return false;
    }

    /// <summary>
    /// Зашифровать файл.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя зашифрованного файла.</param>
    /// <param name="to">Список отпечатков сертификатов получателей файла, куда будет добавлен и свой.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> EncryptFileAsync(string file, string resultFile, string[] to)
    {
        // Файлы более 256k (или все) должны шифроваться потоковым методом (-stream)
        // Файлы должны шифроваться по ГОСТ Р 34.12-2015 Кузнечик (-1215gh)

        if (string.IsNullOrEmpty(file))
            throw new ArgumentNullException(nameof(file));

        if (!File.Exists(file))
            throw new FileNotFoundException("Not found", file);

        if (settings.Encrypt is null)
            throw new InvalidCastException("Crypto Settings have no 'Encrypt'");

        if (settings.Util is null)
            throw new InvalidCastException("Crypto Settings have no 'Util'");

        StringBuilder cmd = new();
        cmd.AppendFormat(settings.Encrypt, file, resultFile, settings.My);

        foreach (var cert in to)
            cmd.Append(AppendCert(cert));

        logger.LogInformation("Encrypt {File}", file.PathQuoted());

        var (ExitCode, Output, Error) =
            await exec.StartWithOutputAsync(settings.Util, cmd.ToString(), settings.Visible);

        logger.LogDebug("{Output}", Output);

        if (File.Exists(resultFile))
            return true;

        logger.LogError("Encrypt error {ExitCode}", ExitCode);

        if (!string.IsNullOrEmpty(Error))
            logger.LogDebug("{Error}", Error);

        //throw new FileNotFoundException("Encrypted file not created", resultFile);
        return false;
    }

    /// <summary>
    /// Подписать файл отсоединенной подписью.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя подписанного файла.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> SignDetachedFileAsync(string file, string resultFile)
    {
        if (string.IsNullOrEmpty(file))
            throw new ArgumentNullException(nameof(file));

        if (!File.Exists(file))
            throw new FileNotFoundException("Not found", file);

        if (settings.SignDetached is null)
            throw new InvalidCastException("Crypto Settings have no 'SignDetached'");

        if (settings.Util is null)
            throw new InvalidCastException("Crypto Settings have no 'Util'");

        StringBuilder cmd = new();
        cmd.AppendFormat(settings.SignDetached, file, resultFile, settings.My);

        if (!string.IsNullOrEmpty(settings.PIN))
            cmd.Append(AppendPIN(settings.PIN));

        logger.LogInformation("Sign detached {File}", file.PathQuoted());

        var (ExitCode, Output, Error) =
            await exec.StartWithOutputAsync(settings.Util, cmd.ToString(), settings.Visible);

        logger.LogDebug("{Output}", Output);

        if (File.Exists(resultFile))
            return true;

        logger.LogError("Sign detached error {ExitCode}", ExitCode);

        if (!string.IsNullOrEmpty(Error))
            logger.LogDebug("{Error}", Error);

        //throw new FileNotFoundException("Detached sign file not created", resultFile);
        return false;
    }

    /// <summary>
    /// Подписать файл.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя подписанного файла.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> SignFileAsync(string file, string resultFile)
    {
        if (string.IsNullOrEmpty(file))
            throw new ArgumentNullException(nameof(file));

        if (!File.Exists(file))
            throw new FileNotFoundException("Not found", file);

        if (settings.Sign is null)
            throw new InvalidCastException("Crypto Settings have no 'Sign'");

        if (settings.Util is null)
            throw new InvalidCastException("Crypto Settings have no 'Util'");

        StringBuilder cmd = new();
        cmd.AppendFormat(settings.Sign, file, resultFile, settings.My);

        if (!string.IsNullOrEmpty(settings.PIN))
            cmd.Append(AppendPIN(settings.PIN));

        logger.LogInformation("Sign {File}", file.PathQuoted());

        var (ExitCode, Output, Error) =
            await exec.StartWithOutputAsync(settings.Util, cmd.ToString(), settings.Visible);

        logger.LogDebug("{Output}", Output);

        if (File.Exists(resultFile))
            return true;

        logger.LogError("Sign error {ExitCode}", ExitCode);

        if (!string.IsNullOrEmpty(Error))
            logger.LogDebug("{Error}", Error);

        //throw new FileNotFoundException("Signed file not created", resultFile);
        return false;
    }

    /// <summary>
    /// Проверить отдельную подпись файла.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="signFile">Имя файла отдельной подписи.</param>
    /// <returns>Результат проверки подписи.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> VerifyDetachedFileAsync(string file, string signFile)
    {
        if (string.IsNullOrEmpty(file))
            throw new ArgumentNullException(nameof(file));

        if (string.IsNullOrEmpty(signFile))
            throw new ArgumentNullException(nameof(signFile));

        if (!File.Exists(file))
            throw new FileNotFoundException("Not found", file);

        if (!File.Exists(signFile))
            throw new FileNotFoundException("Not found", signFile);

        if (settings.VerifyDetached is null)
            throw new InvalidCastException("Crypto Settings have no 'VerifyDetached'");

        if (settings.Util is null)
            throw new InvalidCastException("Crypto Settings have no 'Util'");

        string cmdline = string.Format(settings.VerifyDetached, file, signFile, settings.My);

        logger.LogInformation("Verify detached {File}", file.PathQuoted());

        var (ExitCode, Output, Error) =
            await exec.StartWithOutputAsync(settings.Util, cmdline, settings.Visible);

        logger.LogDebug("{Output}", Output);

        if (Error.Length > 0)
        {
            logger.LogError("Verify detached error {ExitCode}", ExitCode);

            if (!string.IsNullOrEmpty(Error))
                logger.LogDebug("{Error}", Error);
        }

        return ExitCode == 0;
    }

    /// <summary>
    /// Проверить и снять подпись с файла.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="resultFile">Имя файла без подписи.</param>
    /// <returns>Результат проверки подписи.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> VerifyFileAsync(string file, string resultFile)
    {
        if (string.IsNullOrEmpty(file))
            throw new ArgumentNullException(nameof(file));

        if (!File.Exists(file))
            throw new FileNotFoundException("Not found", file);

        if (settings.Verify is null)
            throw new InvalidCastException("Crypto Settings have no 'Verify'");

        if (settings.Util is null)
            throw new InvalidCastException("Crypto Settings have no 'Util'");

        string cmdline = string.Format(settings.Verify, file, resultFile, settings.My);

        logger.LogInformation("Verify {File}", file.PathQuoted());

        var (ExitCode, Output, Error) =
            await exec.StartWithOutputAsync(settings.Util, cmdline, settings.Visible);

        logger.LogDebug("{Output}", Output);

        if (File.Exists(resultFile))
            return ExitCode == 0;

        logger.LogError("Verify error {ExitCode}", ExitCode);

        if (!string.IsNullOrEmpty(Error))
            logger.LogDebug("{Error}", Error);

        //throw new FileNotFoundException("Unsigned file not created", resultFile);
        return false;
    }

    /// <summary>
    /// Вычислить хэш файла.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <returns>Строка хэша.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<string> CalcFileHashAsync(string file)
    {
        if (string.IsNullOrEmpty(file))
            throw new ArgumentNullException(nameof(file));

        if (!File.Exists(file))
            throw new FileNotFoundException("Not found", file);

        if (settings.CalcHash is null)
            throw new InvalidCastException("Crypto Settings have no 'CalcHash'");

        if (settings.Util is null)
            throw new InvalidCastException("Crypto Settings have no 'Util'");

        string cmdline = string.Format(settings.CalcHash, file);

        logger.LogInformation("Calc hash of {File}", file.PathQuoted());

        var (_, Output, _) =
            await exec.StartWithOutputAsync(settings.Util, cmdline, settings.Visible);
        // => A36D628486A17D934BE027C9CAF79B27D7CD9E4E49469D97312B40AD6228D26F

        logger.LogDebug("{Output}", Output);

        return Output;
    }

    /// <summary>
    /// Проверить хэш файла.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="hash">Строка хэша. Если не указана,
    /// то значение будет взято из файла с именем исходного файла.hsh</param>
    /// <returns>Результат проверки хэша.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> VerifyFileHashAsync(string file, string hash)
    {
        if (string.IsNullOrEmpty(file))
            throw new ArgumentNullException(nameof(file));

        if (string.IsNullOrEmpty(hash))
            throw new ArgumentNullException(nameof(hash));

        if (!File.Exists(file))
            throw new FileNotFoundException("Not found", file);

        if (settings.VerifyHash is null)
            throw new InvalidCastException("Crypto Settings have no 'VerifyHash'");

        if (settings.Util is null)
            throw new InvalidCastException("Crypto Settings have no 'Util'");

        string cmdline = string.Format(settings.VerifyHash, file, hash);

        logger.LogInformation("Verify hash of {File}", file.PathQuoted());

        var (ExitCode, Output, Error) =
            await exec.StartWithOutputAsync(settings.Util, cmdline, settings.Visible);

        logger.LogDebug("{Output}", Output);

        //TODO Check if it is true
        return ExitCode == 0;
    }
}
