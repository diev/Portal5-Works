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

using Diev.Extensions.LogFile;
using Diev.Extensions.Smtp;

namespace CryptoBot;

internal static class Notifications
{
    public async static Task<int> DoneAsync(string? task, string message, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(message);

        await SendAsync($"Portal5.{task ?? Program.TaskName}: OK",
            message,
            subscribers ?? Program.Config.Subscribers,
            files);

        return 0;
    }

    public async static Task<int> FailAsync(string? task, string message, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(message);

        await SendAsync($"Portal5.{task ?? Program.TaskName}: {message}",
            $"FAIL: {message}",
            subscribers ?? Program.Config.Subscribers,
            files);

        return 1;
    }

    public async static Task<int> FailAsync(string? task, Exception ex, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(ex.Message);
        Logger.LastError(ex);

        await SendAsync($"Portal5.{task ?? Program.TaskName}: {ex.Message}",
            $"ERROR: {ex}",
            subscribers ?? Program.Config.Subscribers,
            files);

        return 1;
    }

    public async static Task<int> FailAPIAsync(string? task, Exception ex, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(ex.Message);
        Logger.LastError(ex);

        await SendAsync($"Portal5.{task ?? Program.TaskName}: {ex.Message}",
            $"ERROR API: {ex}",
            subscribers ?? Program.Config.Subscribers,
            files);

        return 3;
    }

    public async static Task<int> FailTaskAsync(string? task, Exception ex, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(ex.Message);
        Logger.LastError(ex);

        await SendAsync($"Portal5.{task ?? Program.TaskName}: {ex.Message}",
            $"ERROR TASK: {ex}",
            subscribers ?? Program.Config.Subscribers,
            files);

        return 2;
    }

    public async static Task SendAsync(string subject, string body, string[]? subscribers = null, string[]? files = null)
    {
        await Mailer.SendMessageAsync(
            subscribers ?? Program.Config.Subscribers,
            subject,
            body,
            files);
    }
}
