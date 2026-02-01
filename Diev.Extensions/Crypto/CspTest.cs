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
/// Класс работы с тестовой утилитой командной строки СКЗИ "КриптоПРО CSP".
/// </summary>
public class CspTest : CryptoBase
{
    public CspTest(
        ILogger<CspTest> logger,
        IOptions<CryptoSettings> options,
        IExecService exec
        ) : base()
    {
        base.logger = logger;
        base.exec = exec;
        settings = options.Value;

        settings.Util ??= @"C:\Program Files\Crypto Pro\CSP\csptest.exe";

        settings.Sign ??= @"-sfsign -sign -silent -in ""{0}"" -out ""{1}"" -my {2} -add -addsigtime";
        settings.SignDetached ??= @"-sfsign -sign -silent -in ""{0}"" -out ""{1}"" -my {2} -add -addsigtime -detached";
        settings.Verify ??= @"-sfsign -verify -silent -in ""{0}"" -out ""{1}"" -my {2}";
        settings.VerifyDetached ??= @"-sfsign -verify -silent -in ""{0}"" -signature ""{1}"" -my {2} -detached";
        settings.Encrypt ??= @"-sfenc -encrypt -silent -in ""{0}"" -out ""{1}"" -cert {2}"; // -stream -1215gh
        settings.Decrypt ??= @"-sfenc -decrypt -silent -in ""{0}"" -out ""{1}"" -my {2}";

        //if (settings.CalcHash is null) // ??= @"-hash -alg GOST12_256 -in ""{0}"" -out ""{1}"""; //TODO format out
        //    throw new NotImplementedException("Вычисление хэша с помощью утилиты CspTest не реализовано");

        //if (settings.VerifyHash is null)
        //    throw new NotImplementedException("Проверка хэша с помощью утилиты CspTest не реализована");
    }

    protected override string AppendCert(string cert) => " -cert " + cert;

    protected override string AppendPIN(string pin) => " -password " + pin;
}
