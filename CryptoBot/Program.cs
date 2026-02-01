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

using CryptoBot.Services;
using CryptoBot.Tasks.Clean;
using CryptoBot.Tasks.Load;
using CryptoBot.Tasks.Z130;
using CryptoBot.Tasks.Z137;
using CryptoBot.Tasks.Z221;
using CryptoBot.Tasks.Z222;

using Diev.Extensions.CredentialManager;
using Diev.Extensions.Crypto;
using Diev.Extensions.Exec;
using Diev.Extensions.Loggers;
using Diev.Extensions.Smtp;
using Diev.Portal5.Exceptions;
using Diev.Portal5.Tools;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CryptoBot;

internal class Program(
    ILogger<Program> logger,
    IOptions<ProgramSettings> options,
    ICliService cli,
    INotifyService notifier
    )
{
    public static string TaskName { get; set; } = nameof(Program);

    static Program()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // required for 1251
    }

    internal static async Task<int> Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        var config = builder.Configuration;

        builder.Logging
            .AddConfiguration(config.GetSection("Logging"))
            //.AddFile("logs/{0:yyyy-MM}/{0:yyyyMMdd-HHmm}.log")
            .AddProvider(new SystemdLoggerProvider("logs/{0:yyyy-MM}/{0:yyyyMMdd-HHmm}.log"))
            .AddProvider(new ExceptionLoggerProvider("logs/errors.log"));

        builder.Services
            .AddSingleton<Program>().Configure<ProgramSettings>(config.GetSection(nameof(Program)))
            // Services
            .AddTransient<ICliService, CliService>()
            .AddSingleton<INotifyService, NotifyService>()
            // Extensions
            .AddCredentialManager()
            .AddSmtpClient()
            .AddExec()
            .AddCrypto()
            .AddPortal5()
            // Tasks
            .AddTransient<ICleaner, Cleaner>()
            .AddTransient<ILoader, Loader>().Configure<LoaderSettings>(config.GetSection(nameof(Loader)))
            .AddTransient<IZadacha130, Zadacha130>().Configure<Zadacha130Settings>(config.GetSection(nameof(Zadacha130)))
            .AddTransient<IZadacha137, Zadacha137>().Configure<Zadacha137Settings>(config.GetSection(nameof(Zadacha137)))
            .AddTransient<IZadacha221, Zadacha221>().Configure<Zadacha221Settings>(config.GetSection(nameof(Zadacha221)))
            .AddTransient<IZadacha222, Zadacha222>().Configure<Zadacha222Settings>(config.GetSection(nameof(Zadacha222)));

        using var host = builder.Build();
        var program = host.Services.GetRequiredService<Program>();
        return await program.RunAsync(args);
    }

    private async Task<int> RunAsync(string[] args)
    {
        var subscribers = options.Value.Subscribers; //.ToArray();
        int exit = 0;

        try
        {
            logger.LogInformation("Start with: {Args}", string.Join(' ', args));
            logger.LogDebug("LogLevel: {Level}", LogLevel.Debug);

            var parser = cli.Parse(args);
            exit = await parser.InvokeAsync();
        }
        catch (NoMessagesException ex)
        {
            logger.LogInformation("No Messages");
            exit = await notifier.DoneAsync(TaskName, ex.Message, subscribers);
        }
        catch (Portal5Exception ex)
        {
            logger.LogError(ex, "Portal5 Operation failed");
            exit = await notifier.FailAPIAsync(TaskName, ex, subscribers);
        }
        catch (TaskException ex)
        {
            logger.LogError(ex, "Task Operation failed");
            exit = await notifier.FailTaskAsync(TaskName, ex, subscribers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Operation failed");
            exit = await notifier.FailAsync(TaskName, ex, subscribers);
        }
        finally
        {
            logger.LogInformation("ErrorCode: {Code}", exit);
        }

        return exit;
    }
}
