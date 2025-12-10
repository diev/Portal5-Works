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

using Diev.Extensions.Credentials;
using Diev.Extensions.Crypto;
using Diev.Extensions.Info;
using Diev.Extensions.LogFile;
using Diev.Extensions.Tools;
using Diev.Portal5;
using Diev.Portal5.Exceptions;

using Microsoft.Extensions.Configuration;

namespace CryptoBot;

internal class Program
{
    public static IConfiguration JsonConfig { get; } = null!;
    public static ICrypto Crypto { get; } = null!;
    public static RestAPI RestAPI { get; } = null!;
    public static AppConfig Config { get; } = null!;
    public static string TaskName { get; set; } = nameof(Program);

    static Program()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // required for 1251

        if (Environment.ProcessPath is null) // Linux?
        {
            string appsettings = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            Console.WriteLine($"ВНИМАНИЕ: Настройки в файле {appsettings.PathQuoted()}");

            JsonConfig = new ConfigurationBuilder()
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

            JsonConfig = new ConfigurationBuilder()
                .AddJsonFile(appsettings, true, false) // optional app path\{appsettings}
                .AddJsonFile(comsettings, true, false) // optional C:\ProgramData\{company}\{appsettings}
                .Build();
        }

        var config = JsonConfig.GetSection(nameof(Program));
        Config = new AppConfig(
            TargetName: config["TargetName"] ?? "Portal5test *",
            UtilName: config["UtilName"] ?? "CspTest",
            CryptoName: config["CryptoName"] ?? "CryptoPro My",
            EncryptTo: config["EncryptTo"],
            MyOld: JsonSection.MyOld(config),
            Subscribers: JsonSection.Subscribers(config),
            Debug: bool.Parse(config["Debug"] ?? "false")
        );

        Crypto = Config.UtilName switch
        {
            nameof(CryptCP) => new CryptCP(Config.MyOld, Config.CryptoName),
            _ => new CspTest(Config.MyOld, Config.CryptoName),
        };

        if (Config.Debug)
        {
            Logger.AutoFlush = true;
            Logger.LogToConsole = true;
        }

        Logger.Reset();

        var portal5 = CredentialManager.ReadCredential(Config.TargetName);
        RestAPI = new(portal5, true, Config.Debug);
    }

    internal static async Task<int> Main(string[] args)
    {
        try
        {
            return await RunApplicationAsync(args);
        }
        catch (NoMessagesException ex)
        {
            return await Notifications.DoneAsync(TaskName, ex.Message, Config.Subscribers);
        }
        catch (Portal5Exception ex)
        {
            return await Notifications.FailAPIAsync(TaskName, ex, Config.Subscribers);
        }
        catch (TaskException ex)
        {
            return await Notifications.FailTaskAsync(TaskName, ex, Config.Subscribers);
        }
        catch (Exception ex)
        {
            return await Notifications.FailAsync(TaskName, ex, Config.Subscribers);
        }
        finally
        {
            Logger.Flush();
        }
    }

    private static async Task<int> RunApplicationAsync(string[] args)
    {
        Console.WriteLine(App.Title);

        RootCommand rootCommand = new(App.Description)
        {
            CLI.CleanCommand,
            CLI.LoadCommand,
            CLI.Z130Command,
            CLI.Z137Command,
            CLI.Z221Command,
            CLI.Z222Command
        };

        var parser = rootCommand.Parse(args);
        return await parser.InvokeAsync();
    }
}
