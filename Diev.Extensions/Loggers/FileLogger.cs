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

using Microsoft.Extensions.Logging;

namespace Diev.Extensions.Loggers;

public sealed class FileLogger(string path) : ILogger, IDisposable
{
    //NET 9+ private static readonly Lock _lock = new();
    private static readonly object _lock = new();

    /// <summary>
    /// Метод возвращает объект IDisposable, который представляет некоторую область видимости для логгера.
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="state"></param>
    /// <returns></returns>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public void Dispose()
    {
        
    }

    /// <summary>
    /// Доступен ли логгер для использования.
    /// </summary>
    /// <param name="logLevel"></param>
    /// <returns></returns>
    public bool IsEnabled(LogLevel logLevel)
    {
        //return logLevel == LogLevel.Trace;
        return true;
    }

    /// <summary>
    /// Метод предназначен для выполнения логгирования.
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="logLevel">Уровень детализации текущего сообщения.</param>
    /// <param name="eventId">Идентификатор события.</param>
    /// <param name="state">Некоторый объект состояния, который хранит сообщение.</param>
    /// <param name="exception">Информация об исключении.</param>
    /// <param name="formatter">Функция форматирования, которая с помощью двух предыдущих
    /// параметов позволяет получить собственно сообщение для логгирования.</param>
    public void Log<TState>(LogLevel logLevel,
        EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        lock (_lock)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.AppendAllText(path, formatter(state, exception) + Environment.NewLine);
        }
    }
}
