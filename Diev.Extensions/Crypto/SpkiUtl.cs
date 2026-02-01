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
/// Класс работы с утилитой командной строки СКАД "Сигнатура".
/// </summary>
public class SpkiUtl : CryptoBase
{
    public SpkiUtl(
        ILogger<SpkiUtl> logger,
        IOptions<CryptoSettings> options,
        IExecService exec
        ) : base()
    {
        base.logger = logger;
        base.exec = exec;
        settings = options.Value;

        settings.Util ??= @"C:\Program Files\MDPREI\spki\spki1utl.exe";

        settings.Sign ??= @"-sign -data ""{0}"" -out ""{1}""";
        settings.SignDetached ??= @"-sign -data ""{0}"" -out ""{1}"" -detached";
        settings.Verify ??= @"-verify -in ""{0}"" -out ""{1}"" -delete 1";
        settings.VerifyDetached ??= @"-verify -in ""{0}"" -signature ""{1}"" -detached"; //TODO
        settings.Encrypt ??= @"-encrypt -stream -1215gh -1215mac -in ""{0}"" -out ""{1}"" -reckeyid {2}";
        settings.Decrypt ??= @"-decrypt -in ""{0}"" -out ""{1}""";
    }

    protected override string AppendCert(string cert) => " -reckeyid " + cert;

    protected override string AppendPIN(string pin) => " -password " + pin;
}
