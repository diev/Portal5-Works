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

using Diev.Extensions.Exec;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Diev.Extensions.Crypto;

/// <summary>
/// Класс работы с утилитой командной строки СКЗИ "КриптоПРО CSP".
/// </summary>
public class CryptCP : CryptoBase
{
    public CryptCP(
        ILogger<CryptCP> logger,
        IOptions<CryptoSettings> options,
        IExecService exec
        ) : base()
    {
        base.logger = logger;
        base.exec = exec;
        settings = options.Value;

        settings.Util ??= @"cryptcp.x64.exe";

        settings.Sign ??= @"-sign ""{0}"" ""{1}"" -thumbprint {2} -nochain -der -attached -addchain";
        settings.SignDetached ??= @"-sign ""{0}"" ""{1}"" -thumbprint {2} -nochain -der -detached -addchain";
        settings.Verify ??= @"-verify ""{0}"" ""{1}"" -nochain -attached";
        settings.VerifyDetached ??= @"-verify ""{0}"" ""{1}"" -nochain -detached";
        settings.Encrypt ??= @"-encr ""{0}"" ""{1}"" -thumbprint {2} -nochain -der";
        settings.Decrypt ??= @"-decr ""{0}"" ""{1}"" -thumbprint {2} -nochain";

        //TODO
        settings.CalcHash ??= @"-hash -hex ""{0}"""; //TODO: {1} = {0}.hsh
        settings.VerifyHash ??= @"-vhash -hex ""{0}"""; //TODO: {1} = {0}.hsh
    }

    protected override string AppendCert(string cert) => " -thumbprint " + cert;

    protected override string AppendPIN(string pin) => " -pin " + pin;
}
