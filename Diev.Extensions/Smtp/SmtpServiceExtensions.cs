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

using System.Reflection;

using Diev.Extensions.CredentialManager;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diev.Extensions.Smtp;

public static class SmtpServiceExtensions
{
    /// <summary>
    /// Добавляет DefaultSmtpClient в DI-контейнер с настройками из конфигурации.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration
    /// (опционально, если не передан — берётся из services)</param>
    /// <param name="sectionName">Имя секции в конфигурации
    /// (по умолчанию "Smtp")</param>
    /// <returns>IServiceCollection для цепочки вызовов</returns>
    public static IServiceCollection AddSmtpClient(
        this IServiceCollection services,
        IConfiguration? configuration = null,
        string sectionName = "Smtp")
    {
        // Если конфигурация не передана — получаем из провайдера услуг
        var config = configuration
            ?? services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        // Регистрируем настройки как IOptions<SmtpSettings>
        services.Configure<SmtpSettings>(config.GetSection(sectionName));

        // Применяем дополнительные настройки
        services.PostConfigure<SmtpSettings>(settings =>
        {
            if (string.IsNullOrEmpty(settings.Password))
            {
                ReadCredentialManager(ref settings);
            }

            settings.UserName ??= Environment.UserName;
            settings.DisplayName ??= string.Join(' ',
                Assembly.GetEntryAssembly()?.GetName().Name ?? settings.UserName,
                Environment.MachineName);
        });

        // Регистрируем SmtpClient с использованием настроек
        //services.AddSingleton(sp =>
        //{
        //    var settings = sp.GetRequiredService<IOptions<SmtpSettings>>().Value;
        //    return new DefaultSmtpClient(settings);
        //});

        services.AddSingleton<ISmtpService, SmtpService>();

        return services;
    }

    private static void ReadCredentialManager(ref SmtpSettings settings)
    {
        var cred = new CredentialService().Read(settings.Host);
        string name = cred.TargetName;

        try
        {
            var p = name.Split();

            settings.Host = p[1];
            settings.Port = p.Length > 2 ? int.Parse(p[2]) : 25;

            settings.UseTls = name.EndsWith("tls", StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Windows Credential Manager '{name}' has wrong format", ex);
        }

        settings.UserName = cred.UserName
            ?? throw new InvalidOperationException(
                $"Windows Credential Manager '{name}' has no UserName");
        settings.Password = cred.Password
            ?? throw new InvalidOperationException(
                $"Windows Credential Manager '{name}' has no Password");
    }
}
