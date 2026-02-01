#region License
/*
Copyright 2024-2025 Dmitrii Evdokimov
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
/// Класс работы с утилитой командной строки для вычисления и проверки хэша.
/// </summary>
public class Hash : CryptoBase
{
    public Hash(
        ILogger<Hash> logger,
        IOptions<CryptoSettings> options,
        IExecService exec
        ) : base()
    {
        base.logger = logger;
        base.exec = exec;
        settings = options.Value;

        settings.Util ??= @"C:\Program Files\Crypto Pro\CSP\cpverify.exe";

        //TODO Flags:
        /*
           -logfile<name>: Log into file instead of stdout/stderr
           -sleep<milliseconds>: Sleep before proceed
           -wnd: Display MessageBox
           -errwnd: Display MessageBox only on error
        */

        settings.CalcHash ??= "-mk {0} -alg GR3411_2012_256";
        settings.VerifyHash ??= "{0} -alg GR3411_2012_256 {1}";
    }

    // unused
    protected override string AppendCert(string cert) => string.Empty;

    // unused
    protected override string AppendPIN(string pin) => string.Empty;
}
