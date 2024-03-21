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

using System.Diagnostics.CodeAnalysis;

using Diev.Extensions.Credentials;
using Diev.Portal5;
using Diev.Portal5.API.Tools;

using Microsoft.Extensions.Configuration;

namespace CryptoBot;

internal class Worker
{
    private readonly IConfiguration _config;
    private string _tracelog = "trace.log";

    public RestAPI RestAPI;

    public bool Trace { get; set; } = true;
    public string TraceLog
    {
        get => _tracelog;
        set
        {
            if (value.Contains('%'))
            {
                value = Environment.ExpandEnvironmentVariables(value);
            }

            if (value.Contains("{0}") || value.Contains("{0:"))
            {
                value = string.Format(value, DateTime.Now);
            }

            _tracelog = value;
        }
    }

    public bool MessagesClean { get; set; }
    public bool BulkLoad { get; set; }
    public bool Zadacha130 { get; set; }
    public bool Zadacha137 { get; set; }

    public string[]? Subscribers { get; set; }

    [RequiresUnreferencedCode(
        "Calls Microsoft.Extensions.Configuration.ConfigurationBinder.Bind(String, Object)")]
    public Worker(IConfiguration config)
    {
        _config = config;
        _config.Bind(nameof(Worker), this);

        string filter = "Portal5 *";
        var portal5 = CredentialManager.ReadCredential(filter);
        RestAPI = new(portal5, Trace, TraceLog);
    }

    public async Task Run()
    {
        if (MessagesClean)
        {
            var clean = new MessagesClean(RestAPI, _config);
            await clean.Run(); //OK
            return;
        }

        if (BulkLoad)
        {
            var load = new BulkLoad(RestAPI, _config);

            var filter = new MessagesFilter() // inbox
            {
                MinDateTime = DateTime.Now.AddDays(-3),
                Task = "Zadacha_3-1"
            };
            await load.Run(filter);

            filter = new MessagesFilter() // outbox
            {
                MinDateTime = DateTime.Now.AddDays(-3),
                Task = "Zadacha_2-1"
            };
            await load.Run(filter);

            return;
        }

        if (Zadacha130)
        {
            var task130 = new Zadacha130(RestAPI, _config);
            await task130.Run();
        }

        if (Zadacha137)
        {
            var task137 = new Zadacha137(RestAPI, _config);
            await task137.Run();
        }

        //await Program.Smtp.SendMessageAsync("Task OK", "Job completed.");
    }
}
