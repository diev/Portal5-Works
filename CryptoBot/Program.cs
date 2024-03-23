#region License
/*
Copyright 2022-2024 Dmitrii Evdokimov
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

using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Text;

using CryptoBot.Tasks;

using Diev.Extensions.Credentials;
using Diev.Extensions.Info;
using Diev.Extensions.Log;
using Diev.Extensions.Smtp;
using Diev.Portal5;
using Diev.Portal5.API.Tools;

using Microsoft.Extensions.Configuration;

namespace CryptoBot;

internal static class Program
{
    public static int ExitCode { get; set; } = 0;
    public static IConfiguration Config { get; } = null!;
    public static RestAPI RestAPI { get; } = null!;
    public static Smtp Smtp { get; } = new();

    //config
    public static string TargetName { get; }
    public static string? Subscribers { get; }

    static Program()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // required for 1251

        string appsettings = Path.ChangeExtension(Environment.ProcessPath!, ".config.json");
        string comsettings = Path.Combine(App.CompanyData, Path.GetFileName(appsettings));

        if (File.Exists(comsettings))
        {
            Console.WriteLine(@$"ВНИМАНИЕ: Настройки в файле ""{appsettings}""");
            Console.WriteLine(@$"могут изменяться настройками в файле ""{comsettings}""!");
        }

        Config = new ConfigurationBuilder()
            .AddJsonFile(appsettings, true, false) // optional app path\{appsettings}
            .AddJsonFile(comsettings, true, false) // optional C:\ProgramData\{company}\{appsettings}
            .Build();

        var config = Config.GetSection(nameof(Program));
        TargetName = config[nameof(TargetName)] ?? "Portal5test *";
        Subscribers = config[nameof(Subscribers)];

        Logger.Reset();

        var portal5 = CredentialManager.ReadCredential(TargetName);
        RestAPI = new(portal5, true);
    }

    internal static async Task<int> Main(string[] args)
    {
        Console.WriteLine(App.Title);

        // https://dev.to/karenpayneoregon/c-net-tools-withsystemcommandline-2nc2

        var taskOption = new Option<string>("--zadacha")
        {
            Description = "Номер задачи XX ('Zadacha_XX')",
            IsRequired = true
        }
        .FromAmong("0", "130", "137", "2-1", "3-1");
        taskOption.AddAlias("-z");

        RootCommand rootCommand = new(App.Description)
        {
            taskOption
        };

        rootCommand.SetHandler(TaskCommand, taskOption);
        var commandLineBuilder = new CommandLineBuilder(rootCommand);

        commandLineBuilder.AddMiddleware(async (context, next) =>
        {
            await next(context);
        });

        commandLineBuilder.UseDefaults();
        Parser parser = commandLineBuilder.Build();
        await parser.InvokeAsync(args);

        Logger.Flush();
        return ExitCode;
    }

    internal static void TaskCommand(string task)
    {
        switch (task)
        {
            case "0":
                {
                    MessagesClean.RunAsync().Wait();
                    return;
                }

            case "130":
                {
                    Zadacha130.RunAsync().Wait();
                    return;
                }

            case "137":
                {
                    Zadacha137.RunAsync().Wait();
                    return;
                }

            case "2-1":
                {
                    var outbox = new BulkLoad();
                    var filter = new MessagesFilter()
                    {
                        MinDateTime = DateTime.Now.AddDays(-3),
                        Task = "Zadacha_2-1"
                    };
                    outbox.RunAsync(filter).Wait();
                    return;
                }

            case "3-1":
                {
                    var inbox = new BulkLoad();
                    var filter = new MessagesFilter()
                    {
                        MinDateTime = DateTime.Now.AddDays(-3),
                        Task = "Zadacha_3-1"
                    };
                    inbox.RunAsync(filter).Wait();
                    return;
                }
        }
    }

    public static async Task SendDoneAsync(string task, string body, string? subscribers, string[]? files = null)
    {
        await Smtp.SendMessageAsync(
            ((subscribers is null) || (subscribers.Length == 0)) ? Subscribers : subscribers,
            $"Portal5.{task}: OK",
            body,
            files);
    }

    public static async Task SendFailAsync(string task, string error, string? subscribers, string[]? files = null)
    {
        await Smtp.SendMessageAsync(
            ((subscribers is null) || (subscribers.Length == 0)) ? Subscribers : subscribers,
            $"Portal5.{task}: {error}",
            $"ERROR: {error}",
            files);
    }
}
