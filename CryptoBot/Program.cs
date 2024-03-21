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

using Diev.Extensions.Credentials;
using Diev.Extensions.Info;
using Diev.Extensions.Smtp;

using Microsoft.Extensions.Configuration;
//using Diev.Extensions.QueueService;

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

namespace CryptoBot;

internal class Program
{
    public string[]? SubsribersFail { get; set; }

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

            try
            {
                Smtp smtp = new()
                {
                    SubscribersFail = Settings.GetValue<string[]?>("Program:SubscribersFail")
                };
                await smtp.SendFailMessageAsync($"Portal5: ERROR: {ex.Message}", ex.ToString());
            }
            catch { }

            return 1;
        }
#endif
    }

    private static async Task Run(string[] args)
    {
        string appsettings = App.Name + ".config.json";
        string curdir = Directory.GetCurrentDirectory();

        string app = Path.Combine(App.Directory, appsettings);

        var config = new ConfigurationBuilder()
            .SetBasePath(curdir)
            .AddJsonFile(app, false, false)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

        var worker = new Worker(config);
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
