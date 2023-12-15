#region License
/*
Copyright 2022-2023 Dmitrii Evdokimov
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

using Diev.Extensions;
using Diev.Extensions.Smtp;

using Microsoft.Extensions.Configuration;
//using Diev.Extensions.QueueService;

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

namespace CryptoBot;

/*

20:30 AppServer3 ЗСК. Подготовка файлов для отправки в портал5: \scripts\ZSK_BIN\XXI_570_upload.bat

21:00 CryptoBot ЗСК. Отправка перечня клиентов в портал5: /usr/local/zsk_bin/upload.bat
22:00 CryptoBot ЗСК. Получение реестра с портала5: /usr/local/zsk_bin/download.bat

22:15 AppServer3 ЗСК. Обработка полученного файла: \scripts\ZSK_BIN\XXI_570_load.bat
23:30 AppServer3 ЗСК. Формирование отчета по контрагентам из списка Светофор: \scripts\ZSK_BIN\send_report.cmd odb1 XXISCR
 
*/

internal class Program
{
    public static IConfiguration Settings { get; private set; } = null!;
    public static Smtp Smtp = new();

    internal static async Task<int> Main(string[] args)
    {
        Console.WriteLine(App.Title);

#if DEBUG
        await Run(args);
        return 0;
#else
        try
        {
            await Run(args);
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            await Smtp.SendMessageAsync($"Portal5: ERROR: {ex.Message}", ex.ToString());
            return 1;
        }
#endif
    }

    private static async Task Run(string[] args)
    {
        string appsettings = "appsettings.json";
        string curdir = Directory.GetCurrentDirectory();

        string app = Path.Combine(App.Directory, appsettings);
        string cur = Path.Combine(curdir, appsettings);
        string com = Path.Combine(App.CompanyData, appsettings); //C:\ProgramData\{company}\appsettings.json
        string usr = Path.Combine(App.UserData, appsettings); //C:\Users\{username}\AppData\Local\diev\CryptoBot\appsettings.json

        Settings = new ConfigurationBuilder()
            .SetBasePath(curdir)
            .AddJsonFile(app, false, false)
            .AddJsonFile(cur, false, false)
            .AddJsonFile(com, true, false)
            .AddJsonFile(usr, true, false)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

#if DEBUG

        Console.WriteLine($@"Settings:
App: {(File.Exists(app) ? "+" : "-")} {app}
Cur: {(File.Exists(cur) ? "+" : "-")} {cur}
Com: {(File.Exists(com) ? "+" : "-")} {com}
Usr: {(File.Exists(usr) ? "+" : "-")} {usr}
");
#endif

        var worker = new Worker();
        await worker.Run();

        //HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        //builder.Services.AddSingleton<MonitorLoop>();
        //builder.Services.AddHostedService<QueuedHostedService>();
        //builder.Services.AddSingleton<IBackgroundTaskQueue>(_ =>
        //{
        //    if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
        //    {
        //        queueCapacity = 100;
        //    }

        //    return new DefaultBackgroundTaskQueue(queueCapacity);
        //});

        //IHost host = builder.Build();

        //MonitorLoop monitorLoop = host.Services.GetRequiredService<MonitorLoop>()!;
        //monitorLoop.StartMonitorLoop();

        //host.Run();
    }
}
