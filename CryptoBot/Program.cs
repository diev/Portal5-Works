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

using Diev.Extensions;
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

            Console.WriteLine($"ВНИМАНИЕ: Настройки в файле {appsettings.PathQuoted()}");

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
                Console.WriteLine($"ВНИМАНИЕ: Настройки в файле {appsettings.PathQuoted()}");
                Console.WriteLine($"могут изменяться настройками в файле {comsettings.PathQuoted()}!");
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
        var msgIdArgument = new Argument<string?>("id", "Идентификатор одного сообщения (guid или url/guid)")
        {
            Arity = ArgumentArity.ZeroOrOne
        };

        var taskOption = new Option<string?>("--zadacha", "Номер задачи XX ('Zadacha_XX')");
        taskOption.AddAlias("-z");

        var daysOption = new Option<int?>("--days", "Сколько конкретно дней назад [0]");
        daysOption.AddAlias("-d");

        var minDateOption = new Option<string?>("--min-date", "С какой даты (yyyy-mm-dd)");
        minDateOption.AddAlias("-f");

        var maxDateOption = new Option<string?>("--max-date", "По какую дату (yyyy-mm-dd)");
        maxDateOption.AddAlias("-t");

        var minSizeOption = new Option<int?>("--min-size", "От какого размера (байты)");
        var maxSizeOption = new Option<int?>("--max-size", "До какого размера (байты)");

        var inboxOption = new Option<bool>("--inbox", "Входящие сообщения только");

        var outboxOption = new Option<bool>("--outbox", "Исходящие сообщения только");

        var statusOption = new Option<string?>("--status", "Статус сообщений");
        var status2 = string.Join(' ', MessageInStatus.Values) + ' ' +
            string.Join(' ', MessageOutStatus.Values);
        statusOption.FromAmong(status2.Split(' '));

        var pageOption = new Option<int?>("--page", "Номер страницы по 100 сообщений");

        var loadCommand = new Command("load", "Загрузить одно сообщение или все по фильтру (см. help)")
        {
            msgIdArgument,
            taskOption,
            daysOption,
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
            daysOption, minDateOption, maxDateOption,
            minSizeOption, maxSizeOption,
            inboxOption, outboxOption,
            statusOption, pageOption));
        #endregion load

        #region tasks
        var z130Command = new Command("z130", "Получение информации об уровне риска ЮЛ/ИП");
        rootCommand.Add(z130Command);
        z130Command.SetHandler(Zadacha130.RunAsync);

        var z137Argument = new Argument<int?>("days", "Сколько дней назад может быть файл [0]")
        {
            Arity = ArgumentArity.ZeroOrOne
        };

        var z137Command = new Command("z137", "Ежедневное информирование Банка России о составе и объеме клиентской базы")
        {
            z137Argument
        };
        rootCommand.Add(z137Command);
        z137Command.SetHandler(Zadacha137.RunAsync, z137Argument);
        #endregion tasks

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
            })
            .Build();

        await parser.InvokeAsync(args);
        #endregion parse

        Logger.Flush();
        return ExitCode;
    }

    #region Helpers
    public static string GetTempPath(string path) 
    {
        //TODO Directory.CreateTempSubdirectory(prefix);

        string temp = Path.Combine(path, "TEMP");

        if (Directory.Exists(temp))
            Directory.Delete(temp, true);

        if (Directory.Exists(temp))
            throw new Exception($"Не удалось удалить старую директорию {temp.PathQuoted()}.");

        if (!Directory.CreateDirectory(temp).Exists)
            throw new DirectoryNotFoundException($"Не удалось создать новую директорию {temp.PathQuoted()}.");

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
