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

using Diev.Extensions.Smtp;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CryptoBot.Services;

public class NotifyService(
    ILogger<NotifyService> logger,
    ISmtpService mailer,
    IOptions<ProgramSettings> options
    ) : INotifyService
{
    public async Task<int> DoneAsync(string? task, string message, string[]? subscribers = null, string[]? files = null)
    {
        logger.LogInformation("{Message}", message);

        await SendAsync($"Portal5.{task ?? Program.TaskName}: OK",
            message,
            subscribers,
            files);

        return 0;
    }

    public async Task<int> FailAsync(string? task, string message, string[]? subscribers = null, string[]? files = null)
    {
        logger.LogWarning("{Message}", message);

        await SendAsync($"Portal5.{task ?? Program.TaskName}: {message}",
            $"FAIL: {message}",
            subscribers,
            files);

        return 1;
    }

    public async Task<int> FailAsync(string? task, Exception ex, string[]? subscribers = null, string[]? files = null)
    {
        logger.LogError(ex, "Fail");

        await SendAsync($"Portal5.{task ?? Program.TaskName}: {ex.Message}",
            $"ERROR: {ex}",
            subscribers,
            files);

        return 1;
    }

    public async Task<int> FailAPIAsync(string? task, Exception ex, string[]? subscribers = null, string[]? files = null)
    {
        logger.LogError(ex, "Fail API");

        await SendAsync($"Portal5.{task ?? Program.TaskName}: {ex.Message}",
            $"ERROR API: {ex}",
            subscribers,
            files);

        return 3;
    }

    public async Task<int> FailTaskAsync(string? task, Exception ex, string[]? subscribers = null, string[]? files = null)
    {
        logger.LogError(ex, "Fail Task");

        await SendAsync($"Portal5.{task ?? Program.TaskName}: {ex.Message}",
            $"ERROR TASK: {ex}",
            subscribers,
            files);

        return 2;
    }

    public async Task SendAsync(string subject, string body, string[]? subscribers = null, string[]? files = null)
    {
        logger.LogDebug("Send: {Subject}", subject);

        try
        {
            await mailer.SendMessageAsync(
                subscribers,
                subject,
                body,
                files);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail Send");

            await mailer.SendMessageAsync(
                options.Value.Subscribers,
                ex.Message,
                ex.ToString());
        }
    }
}
