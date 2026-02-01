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

using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

using Diev.Extensions.CredentialManager;
using Diev.Extensions.Loggers;
using Diev.Portal5.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Polly;

namespace Diev.Portal5.Tools;

public static class Portal5Extensions
{
    /// <summary>
    /// Добавляет сервисы Portal5 в DI-контейнер с настройками из конфигурации.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration
    /// (опционально, если не передан — берётся из services)</param>
    /// <param name="sectionName">Имя секции в конфигурации
    /// (по умолчанию "ApiSettings")</param>
    /// <returns>IServiceCollection для цепочки вызовов</returns>
    public static IServiceCollection AddPortal5(
        this IServiceCollection services,
        IConfiguration? configuration = null,
        string sectionName = "Api")
    {
        // Если конфигурация не передана — получаем из провайдера услуг
        var config = configuration
            ?? services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        // Регистрируем настройки как IOptions<...Settings>
        var settings = config.GetSection(sectionName);
        services.Configure<ApiSettings>(settings)

        // Регистрируем сервисы
            .AddSingleton<IApiService, ApiService>()
            .AddSingleton<IPathService, PathService>()
            .AddSingleton<IPortalService, PortalService>()

        // Настраиваем HttpClient
            .AddTransient<HttpLogger>()
            .AddHttpClient(Options.DefaultName, client =>
            {
                var app = Assembly.GetEntryAssembly()?.GetName();
                string userAgent = app?.Name ?? "RestClient";
                string version = app?.Version?.ToString() ?? "1.0";

                client.DefaultRequestHeaders.UserAgent.Add(
                    new ProductInfoHeaderValue(userAgent, version));

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // appsettings.json
                // "Portal5 https://portal5.cbr.ru/ [user pass]"
                // "Portal5test https://portal5test.cbr.ru/ [user pass]"
                var target = settings["TargetName"] ?? "Portal5 *";
                var cred = CredentialService.StaticRead(target!);

                var host = cred.TargetName.Contains(' ')
                    ? cred.TargetName.Split(' ')[1] // Windows Credential Manager
                    : cred.TargetName; // open text

                client.BaseAddress = new Uri(host.TrimEnd('/') + "/back/rapi2/");

                var authToken = settings["AuthToken"];

                if (string.IsNullOrEmpty(authToken))
                {
                    var username = cred.UserName;
                    var password = cred.Password;

                    authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ':' + password));
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
                client.Timeout = TimeSpan.FromMinutes(1);
            })

            // Logging
            .AddHttpMessageHandler<HttpLogger>()

            // Polly
            .AddTransientHttpErrorPolicy(policy => // 408, 500+
                policy.WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDuration => TimeSpan.FromSeconds(10))
            )
            .AddTransientHttpErrorPolicy(policy =>
                policy.CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromMinutes(2))
            )
            ;

        return services;
    }
}
