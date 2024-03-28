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
using Diev.Extensions.LogFile;
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
        // https://learn.microsoft.com/en-us/dotnet/standard/commandline/get-started-tutorial

        var taskOption = new Option<string>("--zadacha")
        {
            Description = "Номер задачи XX ('Zadacha_XX')",
            IsRequired = true
        }
        .FromAmong("0", "130", "137", "2-1", "3-1", "54");
        taskOption.AddAlias("-z");

        var fromOption = new Option<string?>("--from")
        {
            Description = "С какой даты (yyyy-mm-dd)"
        };
        fromOption.AddAlias("-f");

        var toOption = new Option<string?>("--to")
        {
            Description = "По какую дату (yyyy-mm-dd)",
        };
        toOption.AddAlias("-t");

        RootCommand rootCommand = new(App.Description)
        {
            taskOption,
            fromOption,
            toOption
        };

        rootCommand.SetHandler(TaskCommandAsync, taskOption, fromOption, toOption);
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

    internal static async Task TaskCommandAsync(string zadacha, string? from, string? to)
    {
        switch (zadacha)
        {
            case "0":
                {
                    await MessagesClean.RunAsync();
                    return;
                }

            case "130":
                {
                    await Zadacha130.RunAsync();
                    return;
                }

            case "137":
                {
                    await Zadacha137.RunAsync();
                    return;
                }

            case "2-1":
                {
                    await BulkLoad.RunAsync(new MessagesFilter()
                    {
                        Task = "Zadacha_" + zadacha,
                        MinDate = from,
                        MaxDate = to
                    });
                    return;
                }

            case "3-1":
                {
                    await BulkLoad.RunAsync(new MessagesFilter()
                    {
                        Task = "Zadacha_" + zadacha,
                        MinDate = from,
                        MaxDate = to
                    });
                    return;
                }

            case "54":
                {
                    await BulkLoad.RunAsync(new MessagesFilter()
                    {
                        Task = "Zadacha_" + zadacha,
                        MinDate = from,
                        MaxDate = to
                    });
                    return;
                }
        }
    }

    #region Helpers
    public static string GetTempPath(string path) 
    {
        //TODO Directory.CreateTempSubdirectory(prefix);

        string temp = Path.Combine(path, "TEMP");

        if (Directory.Exists(temp))
            Directory.Delete(temp, true);

        if (Directory.Exists(temp))
            throw new Exception(@$"Не удалось удалить старую директорию ""{temp}"".");

        Directory.CreateDirectory(temp);

        if (!Directory.Exists(temp))
            throw new Exception(@$"Не удалось создать новую директорию ""{temp}"".");

        return temp;
    }

    public static async Task SendDoneAsync(string task, string body, string? subscribers, string[]? files = null)
    {
        await Program.Smtp.SendMessageAsync(
            ((subscribers is null) || (subscribers.Length == 0)) ? Subscribers : subscribers,
            $"Portal5.{task}: OK",
            body,
            files);
    }

    public static async Task SendFailAsync(string task, Exception ex, string? subscribers, string[]? files = null)
    {
        await Program.Smtp.SendMessageAsync(
            ((subscribers is null) || (subscribers.Length == 0)) ? Subscribers : subscribers,
            $"Portal5.{task}: {RestAPI.GetErrorMessage(ex.Message)}",
            $"ERROR: {ex}",
            files);
    }
    #endregion Helpers
}
