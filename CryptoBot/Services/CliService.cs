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

using System.CommandLine;

using CryptoBot.Tasks.Clean;
using CryptoBot.Tasks.Load;
using CryptoBot.Tasks.Z130;
using CryptoBot.Tasks.Z137;
using CryptoBot.Tasks.Z221;
using CryptoBot.Tasks.Z222;

using Diev.Extensions.Tools;
using Diev.Portal5.API.Messages;
using Diev.Portal5.API.Tools;

namespace CryptoBot.Services;

internal class CliService : ICliService
{
    private readonly RootCommand root;

    public CliService(
        ICleaner cleaner,
        ILoader loader,
        IZadacha130 zadacha130,
        IZadacha137 zadacha137,
        IZadacha221 zadacha221,
        IZadacha222 zadacha222
        )
    {
        #region Options
        Option<bool> lkOption = new("--lk")
        {
            Description = "Разослать уведомления о Вх. и Исх."
        };

        Option<Guid?> idOption = new("--id")
        {
            Description = "Идентификатор одного сообщения (guid)"
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

        Option<string?> taskOption = new("--zadacha", "-z")
        {
            Description = "Номер задачи XX[, XX, ...]"
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
        #endregion Options

        #region Commands
        Command clean = new("clean",
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

        clean.SetAction(async p =>
        {
            Program.TaskName = nameof(Cleaner);

            return await cleaner.RunAsync(
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

        Command load = new("load",
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
            //
            lkOption
        };

        load.SetAction(async p =>
        {
            Program.TaskName = nameof(Loader);

            return await loader.RunAsync(
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
                //
                p.GetValue(lkOption));
        });

        Command z130 = new("z130",
            "ЗСК: Получение информации об уровне риска ЮЛ/ИП")
        {
            daysOption
        };

        z130.SetAction(async p =>
        {
            Program.TaskName = "Zadacha_130";
            return await zadacha130.RunAsync(p.GetValue(daysOption));
        });

        Command z137 = new("z137",
            "ЗСК: Ежедневное информирование Банка России о составе и объеме клиентской базы")
        {
            daysOption
        };

        z137.SetAction(async p =>
        {
            Program.TaskName = "Zadacha_137";
            return await zadacha137.RunAsync(p.GetValue(daysOption));
        });

        Option<Guid> reqOption = new("--req")
        {
            Description = "Идентификатор Запроса (guid)"
        };

        Command z221 = new("z221",
            "ZBR: Ответ на Запрос информации о платежах КО")
        {
            reqOption
        };

        z221.SetAction(async p =>
        {
            Program.TaskName = "Zadacha_221";
            return await zadacha221.RunAsync(p.GetValue(reqOption));
        });

        Command z222 = new("z222",
            "ZBR: Получение Запроса информации о платежах КО")
        {
            daysOption
        };

        z222.SetAction(async p =>
        {
            Program.TaskName = "Zadacha_222";
            return await zadacha222.RunAsync(p.GetValue(daysOption));
        });
        #endregion Commands

        root = new(App.Description)
        {
            clean,
            load,
            z130,
            z137,
            z221,
            z222
        };
    }

    public ParseResult Parse(string[] args)
    {
        var result = root.Parse(args);

        if (result.Errors.Count > 0)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Message);
            }
        }

        return result;
    }
}
