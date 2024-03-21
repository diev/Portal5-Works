#region License
/*
Copyright 2024 Dmitrii Evdokimov
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

using static Diev.Extensions.Exec.Exec;

namespace Diev.Extensions.Crypto;

/// <summary>
/// Класс работы с утилитой командной строки для вычисления и проверки хэша.
/// </summary>
public class Hash
{
    /// <summary>
    /// Исполняемый файл командной строки.
    /// </summary>
    public string Exe { get; set; } = @"C:\Program Files\Crypto Pro\CSP\cpverify.exe";

    //TODO Flags:
    /*
       -logfile<name>: Log into file instead of stdout/stderr
       -sleep<milliseconds>: Sleep before proceed
       -wnd: Display MessageBox
       -errwnd: Display MessageBox only on error
    */

    /// <summary>
    /// Запускать ли программу видимой.
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// Команда подписи.
    /// {0} - исходный файл;
    /// Результат (хэш) будет на следующей строке.
    /// </summary>
    public string CalcCommand { get; set; } = "-mk {0} -alg GR3411_2012_256";

    /// <summary>
    /// Команда проверки хэша.
    /// {0} - исходный файл;
    /// {1} - строка хэша.
    /// Если {1} не указан, то значение будет взято из файла {0}.hsh
    /// </summary>
    public string VerifyCommand { get; set; } = "{0} -alg GR3411_2012_256 {1}";

    /// <summary>
    /// Вычислить хэш файла.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <returns>Строка хэша.</returns>
    /// <exception cref="ApplicationException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<string> CalcFileAsync(string file)
    {
        string cmdline = string.Format(CalcCommand, file);

        string output = await StartWithOutputAsync(Exe, cmdline, Visible);
        // => A36D628486A17D934BE027C9CAF79B27D7CD9E4E49469D97312B40AD6228D26F

        return output;
    }

    /// <summary>
    /// Проверить хэш файла.
    /// </summary>
    /// <param name="file">Имя исходного файла.</param>
    /// <param name="hash">Имя исходного файла.</param>
    /// <returns>Результат проверки хэша.</returns>
    /// <exception cref="ApplicationException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<bool> VerifyFileAsync(string file, string hash)
    {
        string cmdline = string.Format(VerifyCommand, file, hash);
        int exit = await StartAsync(Exe, cmdline, Visible);

        //TODO Check if it is true
        return exit == 0;
    }
}
