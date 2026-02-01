#region License
/*
Copyright 2025 Dmitrii Evdokimov
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

using System.Runtime.Versioning;

using Microsoft.Extensions.Logging;

namespace Diev.Extensions.Loggers;

/// <summary>
/// https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
/// </summary>
/// <param name="path"></param>
[UnsupportedOSPlatform("browser")]
[ProviderAlias("File")]
public class FileLoggerProvider : ILoggerProvider
{
    private readonly SystemdFormatter _formatter;
    private readonly string _file; // сгенерированное имя файла

    public FileLoggerProvider(string path)
    {
        _formatter = new SystemdFormatter();

        //_basePath = Path.Combine(basePath, $"{DateTime.Now:yyyy-MM}");
        //_logFileName = $"{DateTime.Now:yyMMdd-HHmm}.log";

        string s = Environment.ExpandEnvironmentVariables(path);

        while (s.Contains('{'))
        {
            s = string.Format(s, DateTime.Now);
        }

        _file = s;
        var dir = Path.GetDirectoryName(s);

        if (!string.IsNullOrEmpty(dir))
        {
            Directory.CreateDirectory(dir); //TODO {0} => DateTime
        }
    }

    public ILogger CreateLogger(string categoryName) =>
        new SystemdFileLogger(_file, _formatter, categoryName);

    //public ILogger CreateLogger(string categoryName)
    //{
    //    string s = Environment.ExpandEnvironmentVariables(path);

    //    while (s.Contains('{'))
    //    {
    //        s = string.Format(s, DateTime.Now);
    //    }

    //    return new FileLogger(s);
    //}

    public void Dispose() {}
}
