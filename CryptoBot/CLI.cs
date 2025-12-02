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

using CryptoBot.Helpers;
using CryptoBot.Tasks;

using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Tools;

namespace CryptoBot;

internal static class CLI
{
    public static Command CleanCommand
    {
        get
        {
            Command cleanCommand = new("clean",
                "API: Удалить сообщения по <id> или по фильтру")
            {
                IdOption,
                // or
                TaskOption,
                BeforeOption, DaysOption, DayOption,
                MinDateTimeOption, MaxDateTimeOption,
                MinSizeOption, MaxSizeOption,
                InboxOption, OutboxOption,
                StatusOption,
                PageOption
            };

            cleanCommand.SetAction(async p =>
            {
                Program.TaskName = nameof(Cleaner);
                Cleaner lk = new();

                return await lk.RunAsync(
                    p.GetValue(IdOption),
                // or
                new MessagesFilter(
                    p.GetValue(TaskOption),
                    p.GetValue(BeforeOption), p.GetValue(DaysOption), p.GetValue(DayOption),
                    p.GetValue(MinDateTimeOption), p.GetValue(MaxDateTimeOption),
                    p.GetValue(MinSizeOption), p.GetValue(MaxSizeOption),
                    p.GetValue(InboxOption), p.GetValue(OutboxOption),
                    p.GetValue(StatusOption),
                    p.GetValue(PageOption)));
            });

            return cleanCommand;
        }
    }

    public static Command LoadCommand
    {
        get
        {
            Command loadCommand = new("load",
                "API: Загрузить сообщения по <id> или по фильтру")
            {
                IdOption,
                // or
                TaskOption,
                BeforeOption, DaysOption, DayOption,
                MinDateTimeOption, MaxDateTimeOption,
                MinSizeOption, MaxSizeOption,
                InboxOption, OutboxOption,
                StatusOption,
                PageOption,

                LkOption
            };

            loadCommand.SetAction(async p =>
            {
                Program.TaskName = nameof(Loader);
                var config = Program.JsonConfig.GetSection(nameof(Loader));

                Loader lk = new(
                    zipPath: Path.GetFullPath(config["ZipPath"] ?? "."),
                    docPath: Path.GetFullPath(config["DocPath"] ?? "."),
                    docPath2: config["DocPath2"] is null
                        ? null
                        : Path.GetFullPath(config["DocPath2"]!),
                    exclude: JsonSection.Values(config, "Exclude"),
                    subscribers: config.GetSection("Subscribers"));

                return await lk.RunAsync(
                    p.GetValue(IdOption),
                // or
                new MessagesFilter(
                    p.GetValue(TaskOption),
                    p.GetValue(BeforeOption), p.GetValue(DaysOption), p.GetValue(DayOption),
                    p.GetValue(MinDateTimeOption), p.GetValue(MaxDateTimeOption),
                    p.GetValue(MinSizeOption), p.GetValue(MaxSizeOption),
                    p.GetValue(InboxOption), p.GetValue(OutboxOption),
                    p.GetValue(StatusOption),
                    p.GetValue(PageOption)),

                p.GetValue(LkOption));
            });

            return loadCommand;
        }
    }

    public static Command Z130Command
    {
        get
        {
            Command z130Command = new("z130",
                "ЗСК: Получение информации об уровне риска ЮЛ/ИП")
            {
                DaysOption
            };

            z130Command.SetAction(async p =>
            {
                Program.TaskName = "Zadacha_130";
                var config = Program.JsonConfig.GetSection(Program.TaskName);

                Zadacha130 lk = new(
                    downloadPath: config["DownloadPath"] ?? ".",
                    subscribers: JsonSection.Subscribers(config));

                return await lk.RunAsync(p.GetValue(DaysOption));
            });

            return z130Command;
        }
    }

    public static Command Z137Command
    {
        get
        {
            Command z137Command = new("z137",
                "ЗСК: Ежедневное информирование Банка России о составе и объеме клиентской базы")
            {
                DaysOption
            };

            z137Command.SetAction(async p =>
            {
                Program.TaskName = "Zadacha_137";
                var config = Program.JsonConfig.GetSection(Program.TaskName);

                Zadacha137 lk = new(
                    uploadPath: Path.GetFullPath(config["UploadPath"] ?? "."),
                    zip: config["Zip"]
                        ?? "KYCCL_7831001422_3194_{0:yyyyMMdd}_000001.zip",
                    xsd: config["Xsd"], // "ClientFileXML.xsd"
                    subscribers: JsonSection.Subscribers(config));

                return await lk.RunAsync(p.GetValue(DaysOption));
            });

            return z137Command;
        }
    }

    public static Command Z221Command
    {
        get
        {
            Option<Guid> reqOption = new("--req")
            {
                Description = "Идентификатор Запроса (guid)"
            };

            Command z221Command = new("z221",
                "ZBR: Ответ на Запрос информации о платежах КО")
            {
                reqOption
            };

            z221Command.SetAction(async p =>
            {
                Program.TaskName = "Zadacha_221";
                var config = Program.JsonConfig.GetSection(Program.TaskName);

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

            return z221Command;
        }
    }

    public static Command Z222Command
    {
        get
        {
            Command z222Command = new("z222",
                "ZBR: Получение Запроса информации о платежах КО")
            {
                DaysOption
            };

            z222Command.SetAction(async p =>
            {
                Program.TaskName = "Zadacha_222";
                var config = Program.JsonConfig.GetSection(Program.TaskName);

                Zadacha222 lk = new(
                    downloadPath: config["DownloadPath"] ?? ".",
                    subscribers: JsonSection.Subscribers(config));

                return await lk.RunAsync(p.GetValue(DaysOption));
            });

            return z222Command;
        }
    }

    #region Options
    private static Option<bool> LkOption => new("--lk")
    {
        Description = "Разослать уведомления о Вх. и Исх."
    };

    private static Option<Guid?> IdOption => new("--id")
    {
        Description = "Идентификатор одного сообщения (guid)"
    };

    private static Option<uint?> BeforeOption => new("--before", "-b")
    {
        Description = "Ранее скольки дней назад (7 - раньше недели назад)"
    };

    private static Option<uint?> DaysOption => new("--days", "-d")
    {
        Description = "Сколько последних дней (7 - на этой неделе)"
    };

    private static Option<uint?> DayOption => new("--day", "-n")
    {
        Description = "Какой день назад конкретно (1 - вчера)"
    };

    private static Option<DateTime?> MinDateTimeOption => new("--min-date", "-f")
    {
        Description = "С какой даты (yyyy-mm-dd[Thh:mm:ssZ])"
    };

    private static Option<DateTime?> MaxDateTimeOption => new("--max-date", "-t")
    {
        Description = "До какой даты (время по умолчанию 00:00!)"
    };

    private static Option<uint?> MinSizeOption => new("--min-size")
    {
        Description = "От какого размера (байты)"
    };

    private static Option<uint?> MaxSizeOption => new("--max-size")
    {
        Description = "До какого размера (байты)"
    };

    private static Option<bool> InboxOption => new("--inbox")
    {
        Description = "Входящие сообщения только"
    };

    private static Option<bool> OutboxOption => new("--outbox")
    {
        Description = "Исходящие сообщения только"
    };

    private static Option<string?> TaskOption => new("--zadacha", "-z")
    {
        Description = "Номер задачи XX[, XX, ...]"
    };

    private static Option<string?> StatusOption
    {
        get
        {
            Option<string?> statusOption = new("--status")
            {
                Description = "Статус сообщений"
            };

            List<string> status = [.. MessageInStatus.Values];
            status.AddRange(MessageOutStatus.Values);
            statusOption.AcceptOnlyFromAmong([.. status]);

            return statusOption;
        }
    }

    private static Option<uint?> PageOption => new("--page")
    {
        Description = "Номер страницы (по 100 сообщений)"
    };

    #endregion Options
}
