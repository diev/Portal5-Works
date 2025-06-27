#region License
/*
Copyright 2022-2025 Dmitrii Evdokimov
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
using Diev.Extensions.Crypto;
using Diev.Extensions.Info;
using Diev.Extensions.LogFile;
using Diev.Extensions.Smtp;
using Diev.Extensions.Tools;
using Diev.Portal5;
using Diev.Portal5.API.Messages;

using Microsoft.Extensions.Configuration;

namespace CryptoBot;

internal static class Program
{
    public static int ExitCode { get; set; } = 0;
    public static IConfiguration Config { get; } = null!;
    public static ICrypto Crypto { get; } = null!;
    public static RestAPI RestAPI { get; } = null!;
    public static Smtp Smtp { get; }

    //config
    public static string TargetName { get; }
    public static string UtilName { get; }
    public static string CryptoName { get; }
    public static string SmtpName { get; }
    public static string? Subscribers { get; }
    public static bool Debug { get; }

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

        Debug = bool.Parse(config[nameof(Debug)] ?? "false");

        Crypto = UtilName switch
        {
            nameof(CryptCP) => new CryptCP(CryptoName),
            _ => new CspTest(CryptoName),
        };

        Smtp = new(SmtpName);
        Logger.Reset();

        var portal5 = CredentialManager.ReadCredential(TargetName);
        RestAPI = new(portal5, true, Debug);
    }

    internal static async Task<int> Main(string[] args)
    {
        Console.WriteLine(App.Title);

        var rootCommand = new RootCommand(App.Description);
        
        #region filter
        var idOption = new Option<Guid?>(
            "--id", "Идентификатор одного сообщения (guid)");

        var taskOption = new Option<string?>(
            "--zadacha", "Номер задачи XX[,XX, ...]");
        taskOption.AddAlias(
            "-z");

        var beforeOption = new Option<uint?>(
            "--before", "Ранее скольки дней назад (7 - раньше недели назад)");
        beforeOption.AddAlias(
            "-b");

        var daysOption = new Option<uint?>(
            "--days", "Сколько последних дней (7 - на этой неделе)");
        daysOption.AddAlias(
            "-d");

        var dayOption = new Option<uint?>(
            "--day", "Какой день назад конкретно (1 - вчера)");
        dayOption.AddAlias(
            "-n");

        var minDateTimeOption = new Option<DateTime?>(
            "--min-date", "С какой даты (yyyy-mm-dd[Thh:mm:ssZ])");
        minDateTimeOption.AddAlias(
            "-f");

        var maxDateTimeOption = new Option<DateTime?>(
            "--max-date", "До какой даты (время по умолчанию 00:00!)");
        maxDateTimeOption.AddAlias(
            "-t");

        var minSizeOption = new Option<uint?>(
            "--min-size", "От какого размера (байты)");
        var maxSizeOption = new Option<uint?>(
            "--max-size", "До какого размера (байты)");

        var inboxOption = new Option<bool>(
            "--inbox", "Входящие сообщения только");

        var outboxOption = new Option<bool>(
            "--outbox", "Исходящие сообщения только");

        var statusOption = new Option<string?>(
            "--status", "Статус сообщений");
        var status2 = string.Join(' ', MessageInStatus.Values) + ' ' +
            string.Join(' ', MessageOutStatus.Values);
        statusOption.FromAmong(status2.Split(' '));

        var pageOption = new Option<uint?>(
            "--page", "Номер страницы (по 100 сообщений)");
        #endregion filter

        #region tasks
        var lkiCommand = new Command(
            "lki", "ЛК: Входящие")
        {
            daysOption
        };
        rootCommand.Add(lkiCommand);
        lkiCommand.SetHandler(LKI.RunAsync, daysOption);

        var lkoCommand = new Command(
            "lko", "ЛК: Исходящие")
        {
            daysOption
        };
        rootCommand.Add(lkoCommand);
        lkoCommand.SetHandler(LKO.RunAsync, daysOption);

        var z130Command = new Command(
            "z130", "ЗСК: Получение информации об уровне риска ЮЛ/ИП")
        {
            daysOption
        };
        rootCommand.Add(z130Command);
        z130Command.SetHandler(Zadacha130.RunAsync, daysOption);

        var z137Command = new Command(
            "z137", "ЗСК: Ежедневное информирование Банка России о составе и объеме клиентской базы")
        {
            daysOption
        };
        rootCommand.Add(z137Command);
        z137Command.SetHandler(Zadacha137.RunAsync, daysOption);

        var z222Command = new Command(
            "z222", "ZBR: Получение Запроса информации о платежах КО")
        {
            daysOption
        };
        rootCommand.Add(z222Command);
        z222Command.SetHandler(Zadacha222.RunAsync, daysOption);

        var reqOption = new Option<Guid>(
            "--req", "Идентификатор Запроса (guid)");

        var z221Command = new Command(
            "z221", "ZBR: Ответ на Запрос информации о платежах КО")
        {
            reqOption
        };
        rootCommand.Add(z221Command);
        z221Command.SetHandler(Zadacha221.RunAsync, reqOption);
        #endregion tasks

        #region filter commands
        var loadCommand = new Command(
            "load", "API: Загрузить сообщения по <id> или по фильтру")
        {
            idOption,
            // or
            taskOption,
            beforeOption, daysOption, dayOption,
            minDateTimeOption, maxDateTimeOption,
            minSizeOption, maxSizeOption,
            inboxOption, outboxOption,
            statusOption,
            pageOption
        };

        var cleanCommand = new Command(
            "clean", "API: Удалить сообщения по <id> или по фильтру")
        {
            idOption,
            // or
            taskOption,
            beforeOption, daysOption, dayOption,
            minDateTimeOption, maxDateTimeOption,
            minSizeOption, maxSizeOption,
            inboxOption, outboxOption,
            statusOption,
            pageOption
        };
        #endregion filter commands

        #region messages
        rootCommand.AddCommand(loadCommand);
        rootCommand.AddCommand(cleanCommand);

        loadCommand.SetHandler(Loader.RunAsync,
            idOption,
            // or
            new MessagesFilterBinder(
                taskOption,
                beforeOption, daysOption, dayOption,
                minDateTimeOption, maxDateTimeOption,
                minSizeOption, maxSizeOption,
                inboxOption, outboxOption,
                statusOption,
                pageOption));

        cleanCommand.SetHandler(Cleaner.RunAsync,
            idOption,
            // or
            new MessagesFilterBinder(
                taskOption,
                beforeOption, daysOption, dayOption,
                minDateTimeOption, maxDateTimeOption,
                minSizeOption, maxSizeOption,
                inboxOption, outboxOption,
                statusOption,
                pageOption));
        #endregion messages

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

    public static async Task SendAsync(string subject, string body, string? subscribers, string[]? files = null)
    {
        await Program.Smtp.SendMessageAsync(
            ((subscribers is null) || (subscribers.Length == 0)) ? Subscribers : subscribers,
            subject,
            body,
            files);
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
