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

using Microsoft.Extensions.DependencyInjection;

namespace Diev.Extensions.CredentialManager;

public static class CredentialServiceExtensions
{
    /// <summary>
    /// Добавляет DefaultCrypto в DI-контейнер с настройками из конфигурации.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration
    /// (опционально, если не передан — берётся из services)</param>
    /// <param name="sectionName">Имя секции в конфигурации
    /// (по умолчанию "CredentialService")</param>
    /// <returns>IServiceCollection для цепочки вызовов</returns>
    public static IServiceCollection AddCredentialManager(
        this IServiceCollection services)
    {
        services.AddSingleton<ICredentialService, CredentialService>();
        return services;
    }
}
