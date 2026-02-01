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

using Diev.Extensions.CredentialManager;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diev.Extensions.Crypto;

public static class CryptoServiceExtensions
{
    /// <summary>
    /// Добавляет DefaultCrypto в DI-контейнер с настройками из конфигурации.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration
    /// (опционально, если не передан — берётся из services)</param>
    /// <param name="sectionName">Имя секции в конфигурации
    /// (по умолчанию "Crypto")</param>
    /// <returns>IServiceCollection для цепочки вызовов</returns>
    public static IServiceCollection AddCrypto(
        this IServiceCollection services,
        IConfiguration? configuration = null,
        string sectionName = "Crypto")
    {
        // Если конфигурация не передана — получаем из провайдера услуг
        var config = configuration
            ?? services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        // Регистрируем настройки как IOptions<CryptoSettings>
        services.Configure<CryptoSettings>(config.GetSection(sectionName));

        // Применяем дополнительные настройки
        services.PostConfigure<CryptoSettings>(settings =>
        {
            if (string.IsNullOrEmpty(settings.My) || settings.My.Length < 40)
            {
                ReadCredentialManager(ref settings);
            }
        });

        // Регистрируем Crypto
        //services.AddKeyedSingleton<ICryptoService, CspTest>(nameof(CspTest));
        //services.AddKeyedSingleton<ICryptoService, CryptCP>(nameof(CryptCP));

        //TODO switch if file exists
        services.AddSingleton<ICryptoService, CryptCP>();

        return services;
    }

    private static void ReadCredentialManager(ref CryptoSettings settings)
    {
        string filter = settings.My ?? "CryptoPro My";

        try
        {
            var cred = new CredentialService().Read(filter);
            string name = cred.TargetName;

            settings.My = cred.UserName
                ?? throw new Exception($"Windows Credential Manager '{name}' has no UserName");
            settings.PIN = cred.Password;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Windows Credential Manager '{filter}' has wrong format", ex);
        }
    }
}
