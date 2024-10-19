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
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using System.Text;

using CryptoBot.Tasks;

using Diev.Extensions.Credentials;
using Diev.Extensions.Info;
using Diev.Extensions.LogFile;
using Diev.Extensions.Smtp;
using Diev.Portal5;
using Diev.Portal5.API.Messages;

using Microsoft.Extensions.Configuration;

namespace CryptoBot;

internal static class Program
{
    public static int ExitCode { get; set; } = 0;
    public static IConfiguration Config { get; } = null!;
    public static RestAPI RestAPI { get; } = null!;
    public static Smtp Smtp { get; }

    //config
    public static string TargetName { get; }
    public static string UtilName { get; }
    public static string CryptoName { get; }
    public static string SmtpName { get; }
    public static string? Subscribers { get; }

    static Program()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // required for 1251

        if (Environment.ProcessPath is null) // Linux?
        {
            string appsettings = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            Console.WriteLine(@$"ВНИМАНИЕ: Настройки в файле ""{appsettings}""");

            Config = new ConfigurationBuilder()
                .AddJsonFile(appsettings, true, false) // optional curdir
                .Build();
        }
        else // Windows
        {
            string appsettings = Path.ChangeExtension(Environment.ProcessPath, ".config.json");
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
        }

        var config = Config.GetSection(nameof(Program));
        TargetName = config[nameof(TargetName)] ?? "Portal5test *";
        UtilName = config[nameof(UtilName)] ?? "CspTest";
        CryptoName = config[nameof(CryptoName)] ?? "CryptoPro My";
        SmtpName = config[nameof(SmtpName)] ?? "SMTP *";
        Subscribers = config[nameof(Subscribers)];

        Smtp = new(SmtpName);
        Logger.Reset();

        var portal5 = CredentialManager.ReadCredential(TargetName);
        RestAPI = new(portal5, true);
    }

    internal static async Task<int> Main(string[] args)
    {
        Console.WriteLine(App.Title);

        var rootCommand = new RootCommand(App.Description);

        #region clean
        var cleanCommand = new Command("clean", "Очистить лишнее из отчетности старее 30 дней");
        rootCommand.Add(cleanCommand);
        cleanCommand.SetHandler(MessagesClean.RunAsync);
        #endregion

        #region load
        var msgIdArgument = new Argument<string?>("id", "Идентификатор одного сообщения (guid)")
        {
            Arity = ArgumentArity.ZeroOrOne
        };

        var taskOption = new Option<string?>("--zadacha", "Номер задачи XX ('Zadacha_XX')");
        taskOption.AddAlias("-z");

        var todayOption = new Option<bool>("--today", "Текущий день только");
        todayOption.AddAlias("-d");
        //todayOption.SetDefaultValue(false);

        var minDateOption = new Option<string?>("--min-date", "С какой даты (yyyy-mm-dd)");
        minDateOption.AddAlias("-f");

        var maxDateOption = new Option<string?>("--max-date", "По какую дату (yyyy-mm-dd)");
        maxDateOption.AddAlias("-t");

        var minSizeOption = new Option<int?>("--min-size", "От какого размера (байты)");
        var maxSizeOption = new Option<int?>("--max-size", "До какого размера (байты)");

        var inboxOption = new Option<bool>("--inbox", "Входящие сообщения только");
        //inboxOption.SetDefaultValue(false);

        var outboxOption = new Option<bool>("--outbox", "Исходящие сообщения только");
        //outboxOption.SetDefaultValue(false);

        var statusOption = new Option<string?>("--status", "Статус сообщений");
        var status2 = string.Join(' ', MessageInStatus.Values) + ' ' +
            string.Join(' ', MessageOutStatus.Values);
        statusOption.FromAmong(status2.Split(' '));

        var pageOption = new Option<int?>("--page", "Номер страницы по 100 сообщений");
        //pageOption.SetDefaultValue(1);

        var loadCommand = new Command("load", "Загрузить одно сообщение или все по фильтру")
        {
            msgIdArgument,
            taskOption,
            todayOption,
            minDateOption,
            maxDateOption,
            minSizeOption,
            maxSizeOption,
            inboxOption,
            outboxOption,
            statusOption,
            pageOption
        };
        rootCommand.Add(loadCommand);
        loadCommand.SetHandler(MessagesLoad.RunAsync, msgIdArgument,
            // or
            new MessagesFilterBinder(taskOption,
            todayOption, minDateOption, maxDateOption,
            minSizeOption, maxSizeOption,
            inboxOption, outboxOption,
            statusOption, pageOption));
        #endregion load

        #region run
        var runArgument = new Argument<string>("xx", "Номер задачи XX ('Zadacha_XX')")
        {
            Arity = ArgumentArity.ExactlyOne
        }
        .FromAmong("130", "137");

        var runCommand = new Command("z", "Запустить задачу XX")
        {
            runArgument
        };
        rootCommand.Add(runCommand);
        runCommand.SetHandler(RunCommandAsync, runArgument);
        #endregion run

        #region parse
        var parser = new CommandLineBuilder(rootCommand)
            .UseDefaults()
            .UseHelp(ctx =>
            {
                ctx.HelpBuilder.CustomizeSymbol(statusOption,
                    $"--{statusOption.Name} <{statusOption.Name}>",
                    statusOption.Description +
                    Environment.NewLine +
                    "для inbox: " + string.Join(", ", MessageInStatus.Values) + 
                    Environment.NewLine +
                    "для outbox: " + string.Join(", ", MessageOutStatus.Values));

                ctx.HelpBuilder.CustomizeSymbol(runArgument,
                    $"<{runArgument.Name}>",
                    runArgument.Description +
                    Environment.NewLine +
                    "130: Получение информации об уровне риска ЮЛ/ИП" +
                    Environment.NewLine +
                    "137: Ежедневное информирование Банка России о составе и объеме клиентской базы");
            })
            .Build();

        await parser.InvokeAsync(args);
        #endregion parse

        Logger.Flush();
        return ExitCode;
    }

    internal static async Task RunCommandAsync(string zadacha)
    {
        switch (zadacha)
        {
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

    public static async Task SendFailAsync(string task, string message, string? subscribers, string[]? files = null)
    {
        await Program.Smtp.SendMessageAsync(
            ((subscribers is null) || (subscribers.Length == 0)) ? Subscribers : subscribers,
            $"Portal5.{task}: {message}",
            $"FAIL: {message}",
            files);
    }

    public static async Task SendFailAsync(string task, Exception ex, string? subscribers, string[]? files = null)
    {
        await Program.Smtp.SendMessageAsync(
            ((subscribers is null) || (subscribers.Length == 0)) ? Subscribers : subscribers,
            $"Portal5.{task}: {ex.Message}",
            $"ERROR: {ex}",
            files);
    }

    #endregion Helpers
}
