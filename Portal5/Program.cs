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

using Diev.Extensions.CredentialManager;
using Diev.Extensions.Loggers;
using Diev.Portal5.Tools;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Portal5;

public static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // required for 1251

        var builder = Host.CreateApplicationBuilder(args);
        var config = builder.Configuration;

        builder.Logging
            .AddConfiguration(config.GetSection("Logging"))
            .AddProvider(new SystemdLoggerProvider("logs/{0:yyyy-MM}/{0:yyyyMMdd-HHmm}.log"))
            .AddProvider(new ExceptionLoggerProvider("logs/errors.log"));

        builder.Services
            .AddCredentialManager()
            .AddPortal5()
            //AddSingleton<IDataService>(sp => new DataService())
            .AddTransient<MessagesPageForm>();

        using var host = builder.Build();
        var form = host.Services.GetRequiredService<MessagesPageForm>();
        Application.Run(form);
    }
}
