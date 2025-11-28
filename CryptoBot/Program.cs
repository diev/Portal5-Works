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
using System.Text;

using CryptoBot.Helpers;
using CryptoBot.Tasks;

using Diev.Extensions.Credentials;
using Diev.Extensions.Crypto;
using Diev.Extensions.Info;
using Diev.Extensions.LogFile;
using Diev.Extensions.Smtp;
using Diev.Extensions.Tools;
using Diev.Portal5;
using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Tools;
using Diev.Portal5.Exceptions;

using Microsoft.Extensions.Configuration;

namespace CryptoBot;

internal class Program
{
    private static string _task = nameof(Program);

    public static IConfiguration Config { get; } = null!;
    public static ICrypto Crypto { get; } = null!;
    public static RestAPI RestAPI { get; } = null!;

    //config
    public static string TargetName { get; }
    public static string UtilName { get; }
    public static string CryptoName { get; }
    public static string? EncryptTo { get; }
    public static string[] MyOld { get; }
    //public static string[] DoverXml { get; }
    public static string[] Subscribers { get; }
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
        EncryptTo = config[nameof(EncryptTo)];

        MyOld = JsonSection.MyOld(config);

        // МЧД
        //DoverXml = JsonSection.DoverXml(config);

        Subscribers = JsonSection.Subscribers(config);

        Debug = bool.Parse(config[nameof(Debug)] ?? "false");

        Crypto = UtilName switch
        {
            nameof(CryptCP) => new CryptCP(MyOld, CryptoName),
            _ => new CspTest(MyOld,CryptoName),
        };

        if (Debug)
        {
            Logger.AutoFlush = true;
            Logger.LogToConsole = true;
        }

        Logger.Reset();

        var portal5 = CredentialManager.ReadCredential(TargetName);
        RestAPI = new(portal5, true, Debug);
    }

    internal static async Task<int> Main(string[] args)
    {
        try
        {
            return await RunApplicationAsync(args);
        }
        catch (NoMessagesException ex)
        {
            return Done(_task, ex.Message, Subscribers);
        }
        catch (Portal5Exception ex)
        {
            return FailAPI(_task, ex, Subscribers);
        }
        catch (TaskException ex)
        {
            return FailTask(_task, ex, Subscribers);
        }
        catch (Exception ex)
        {
            return Fail(_task, ex, Subscribers);
        }
        finally
        {
            Logger.Flush();
        }
    }

    private static async Task<int> RunApplicationAsync(string[] args)
    {
        Console.WriteLine(App.Title);

        RootCommand rootCommand = new(App.Description);

        #region filter
        Option<Guid?> idOption = new("--id")
        {
            Description = "Идентификатор одного сообщения (guid)"
        };

        Option<string?> taskOption = new("--zadacha", "-z")
        {
            Description = "Номер задачи XX[, XX, ...]"
        };

        Option<uint?> beforeOption = new("--before", "-b")
        {
            Description = "Ранее скольки дней назад (7 - раньше недели назад)"
        };

        Option<uint?> daysOption = new("--days", "-d")
        {
            Description = "Сколько последних дней (7 - на этой неделе)"
        };

        Option<uint?> dayOption = new("--day", "-n")
        {
            Description = "Какой день назад конкретно (1 - вчера)"
        };

        Option<DateTime?> minDateTimeOption = new("--min-date", "-f")
        {
            Description = "С какой даты (yyyy-mm-dd[Thh:mm:ssZ])"
        };

        Option<DateTime?> maxDateTimeOption = new("--max-date", "-t")
        {
            Description = "До какой даты (время по умолчанию 00:00!)"
        };

        Option<uint?> minSizeOption = new("--min-size")
        {
            Description = "От какого размера (байты)"
        };

        Option<uint?> maxSizeOption = new("--max-size")
        {
            Description = "До какого размера (байты)"
        };

        Option<bool> inboxOption = new("--inbox")
        {
            Description = "Входящие сообщения только"
        };

        Option<bool> outboxOption = new("--outbox")
        {
            Description = "Исходящие сообщения только"
        };

        Option<string?> statusOption = new("--status")
        {
            Description = "Статус сообщений"
        };
        List<string> status = [.. MessageInStatus.Values];
        status.AddRange(MessageOutStatus.Values);
        statusOption.AcceptOnlyFromAmong([.. status]);

        Option<uint?> pageOption = new("--page")
        {
            Description = "Номер страницы (по 100 сообщений)"
        };
        #endregion filter

        #region tasks
        Command z130Command = new("z130",
            "ЗСК: Получение информации об уровне риска ЮЛ/ИП")
            {
                daysOption
            };
        rootCommand.Add(z130Command);
        z130Command.SetAction(async p =>
        {
            _task = nameof(Zadacha130);
            var config = Config.GetSection(_task);

            Zadacha130 lk = new(
                downloadPath: config["DownloadPath"] ?? ".",
                subscribers: JsonSection.Subscribers(config));

            return await lk.RunAsync(p.GetValue(daysOption));
        });

        Command z137Command = new("z137",
            "ЗСК: Ежедневное информирование Банка России о составе и объеме клиентской базы")
            {
                daysOption
            };
        rootCommand.Add(z137Command);
        z137Command.SetAction(async p =>
        {
            _task = nameof(Zadacha137);
            var config = Config.GetSection(_task);

            Zadacha137 lk = new(
                uploadPath: Path.GetFullPath(config["UploadPath"] ?? "."),
                zip: config["Zip"]
                    ?? "KYCCL_7831001422_3194_{0:yyyyMMdd}_000001.zip",
                xsd: config["Xsd"], // "ClientFileXML.xsd"
                subscribers: JsonSection.Subscribers(config));

            return await lk.RunAsync(p.GetValue(daysOption));
        });

        Command z222Command = new("z222",
            "ZBR: Получение Запроса информации о платежах КО")
            {
                daysOption
            };
        rootCommand.Add(z222Command);
        z222Command.SetAction(async p =>
        {
            _task = nameof(Zadacha222);
            var config = Config.GetSection(_task);

            Zadacha222 lk = new(
                downloadPath: config["DownloadPath"] ?? ".",
                subscribers: JsonSection.Subscribers(config));

            return await lk.RunAsync(p.GetValue(daysOption));
        });

        Option<Guid> reqOption = new("--req")
        {
            Description = "Идентификатор Запроса (guid)"
        };

        Command z221Command = new("z221",
            "ZBR: Ответ на Запрос информации о платежах КО")
            {
                reqOption
            };
        rootCommand.Add(z221Command);
        z221Command.SetAction(async p =>
        {
            _task = nameof(Zadacha221);
            var config = Config.GetSection(_task);

            string uploadPath = Path.GetFullPath(config["UploadPath"] ?? ".");
            string archivePath = Directory.CreateDirectory(
                Path.Combine(uploadPath, "BAK", DateTime.Now.ToString("yyyyMMdd"))
                ).FullName;
            string zip = config["Zip"]
                ?? "AFN_4030702_0000000_{0:yyyyMMdd}_{1:D5}.zip";

            Zadacha221 lk = new(uploadPath, archivePath, zip,
                subscribers: JsonSection.Subscribers(config));

            return await lk.RunAsync(p.GetValue(reqOption));
        });
        #endregion tasks

        #region filter commands
        Option<bool> lkOption = new("--lk")
        {
            Description = "Разослать уведомления о Вх. и Исх."
        };

        Command loadCommand = new("load",
            "API: Загрузить сообщения по <id> или по фильтру")
            {
                idOption,
                // or
                taskOption,
                beforeOption, daysOption, dayOption,
                minDateTimeOption, maxDateTimeOption,
                minSizeOption, maxSizeOption,
                inboxOption, outboxOption,
                statusOption,
                pageOption,

                lkOption
            };

        Command cleanCommand = new("clean",
            "API: Удалить сообщения по <id> или по фильтру")
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
        rootCommand.Add(loadCommand);
        rootCommand.Add(cleanCommand);

        loadCommand.SetAction(async p =>
        {
            _task = nameof(Loader);
            var config = Config.GetSection(_task);

            Loader lk = new(
                zipPath: Path.GetFullPath(config["ZipPath"] ?? "."),
                docPath: Path.GetFullPath(config["DocPath"] ?? "."),
                docPath2: config["DocPath2"] is null
                    ? null
                    : Path.GetFullPath(config["DocPath2"]!),
                exclude: JsonSection.Values(config, "Exclude"),

                //subscribers: JsonSection.Subscribers(config),
                //iSubscribers: JsonSection.Values(config, "iSubscribers"),
                //oSubscribers: JsonSection.Values(config, "oSubscribers"));
                subscribers: config.GetSection("Subscribers"));

            return await lk.RunAsync(
                p.GetValue(idOption),
            // or
            new MessagesFilter(
                p.GetValue(taskOption),
                p.GetValue(beforeOption), p.GetValue(daysOption), p.GetValue(dayOption),
                p.GetValue(minDateTimeOption), p.GetValue(maxDateTimeOption),
                p.GetValue(minSizeOption), p.GetValue(maxSizeOption),
                p.GetValue(inboxOption), p.GetValue(outboxOption),
                p.GetValue(statusOption),
                p.GetValue(pageOption)),

            p.GetValue(lkOption));
        });

        cleanCommand.SetAction(async p =>
        {
            _task = nameof(Cleaner);
            Cleaner lk = new();

            return await lk.RunAsync(
                p.GetValue(idOption),
            // or
            new MessagesFilter(
                p.GetValue(taskOption),
                p.GetValue(beforeOption), p.GetValue(daysOption), p.GetValue(dayOption),
                p.GetValue(minDateTimeOption), p.GetValue(maxDateTimeOption),
                p.GetValue(minSizeOption), p.GetValue(maxSizeOption),
                p.GetValue(inboxOption), p.GetValue(outboxOption),
                p.GetValue(statusOption),
                p.GetValue(pageOption)));
        });
        #endregion messages

        #region parse
        var parser = rootCommand.Parse(args);
        return await parser.InvokeAsync();
        #endregion parse
    }

    #region notifications
    public static void Send(string subject, string body, string[]? subscribers = null, string[]? files = null)
    {
        Mailer.SendMessage(
            subscribers ?? Subscribers,
            subject,
            body,
            files);
    }

    public static int Done(string task, string message, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(message);

        Send($"Portal5.{task}: OK",
            message,
            subscribers ?? Subscribers,
            files);

        return 0;
    }

    public static int Fail(string task, string message, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(message);

        Send($"Portal5.{task}: {message}",
            $"FAIL: {message}",
            subscribers ?? Subscribers,
            files);

        return 1;
    }

    public static int Fail(string task, Exception ex, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(ex.Message);
        Logger.LastError(ex);

        Send($"Portal5.{task}: {ex.Message}",
            $"ERROR: {ex}",
            subscribers ?? Subscribers,
            files);

        return 1;
    }

    public static int FailTask(string task, Exception ex, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(ex.Message);
        Logger.LastError(ex);

        Send($"Portal5.{task}: {ex.Message}",
            $"ERROR TASK: {ex}",
            subscribers ?? Subscribers,
            files);

        return 2;
    }

    public static int FailAPI(string task, Exception ex, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(ex.Message);
        Logger.LastError(ex);

        Send($"Portal5.{task}: {ex.Message}",
            $"ERROR API: {ex}",
            subscribers ?? Subscribers,
            files);

        return 3;
    }
    #endregion notifications
}
