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
    public static int Done(string? task, string message, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(message);

        Send($"Portal5.{task ?? Program.TaskName}: OK",
            message,
            subscribers ?? Program.Config.Subscribers,
            files);

        return 0;
    }

    public static int Fail(string? task, string message, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(message);

        Send($"Portal5.{task ?? Program.TaskName}: {message}",
            $"FAIL: {message}",
            subscribers ?? Program.Config.Subscribers,
            files);

        return 1;
    }

    public static int Fail(string? task, Exception ex, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(ex.Message);
        Logger.LastError(ex);

        Send($"Portal5.{task ?? Program.TaskName}: {ex.Message}",
            $"ERROR: {ex}",
            subscribers ?? Program.Config.Subscribers,
            files);

        return 1;
    }

    public static int FailAPI(string? task, Exception ex, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(ex.Message);
        Logger.LastError(ex);

        Send($"Portal5.{task ?? Program.TaskName}: {ex.Message}",
            $"ERROR API: {ex}",
            subscribers ?? Program.Config.Subscribers,
            files);

        return 3;
    }

    public static int FailTask(string? task, Exception ex, string[]? subscribers = null, string[]? files = null)
    {
        Logger.TimeLine(ex.Message);
        Logger.LastError(ex);

        Send($"Portal5.{task ?? Program.TaskName}: {ex.Message}",
            $"ERROR TASK: {ex}",
            subscribers ?? Program.Config.Subscribers,
            files);

        return 2;
    }

    public static void Send(string subject, string body, string[]? subscribers = null, string[]? files = null)
    {
        Mailer.SendMessage(
            subscribers ?? Program.Config.Subscribers,
            subject,
            body,
            files);
    }
}
