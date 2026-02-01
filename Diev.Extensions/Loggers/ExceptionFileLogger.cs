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

using Microsoft.Extensions.Logging;

namespace Diev.Extensions.Loggers;

public class ExceptionFileLogger(string logPath, ExceptionFileFormatter logFormatter) : ILogger
{
    private static readonly SemaphoreSlim _writeLock = new(1, 1); // Для потокобезопасности

    /// <summary>
    /// Метод возвращает объект IDisposable, который представляет некоторую область видимости для логгера.
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="state"></param>
    /// <returns></returns>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public void Dispose() { }

    public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Error;

    public async void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (logLevel < LogLevel.Error || exception == null)
            return;

        try
        {
            string logMessage = ExceptionFileFormatter.Format(exception);

            // Потокобезопасная асинхронная запись
            await _writeLock.WaitAsync();
            try
            {
                await File.AppendAllTextAsync(
                    logPath,
                    logMessage + Environment.NewLine,
                    Encoding.UTF8
                );
            }
            finally
            {
                _writeLock.Release();
            }
        }
        catch (Exception ex)
        {
            // Логгируем ошибку в консоль (не бросаем исключение из Log!)
            Console.Error.WriteLine($"Log write failed: {ex.Message}");
        }
    }
}
