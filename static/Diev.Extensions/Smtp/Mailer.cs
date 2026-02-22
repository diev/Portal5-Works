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

using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
//using System.Text.RegularExpressions;

using Diev.Extensions.Credentials;
using Diev.Extensions.Info;
using Diev.Extensions.LogFile;

namespace Diev.Extensions.Smtp;

public static class Mailer
{
    private static readonly char[] _separator = [' ', ',', ';'];
    //private ConcurrentQueue<MailMessage> _queue = new();

    public static string Filter { get; set; } = "SMTP *";
    public static string Host { get; set; } = "127.0.0.1";
    public static int Port { get; set; } = 25;
    public static bool UseTls { get; set; } = true;
    public static string UserName { get; set; } = Environment.UserName;
    public static string Password { get; set; } = string.Empty;
    public static string DisplayName { get; set; } = $"{App.Name} {Environment.MachineName}";

    /// <summary>
    /// Create from Windows credentials manager or a text string.
    /// </summary>
    static Mailer()
    {
        ReadCredential(Filter);
    }

    public static void ReadCredential(string filter)
    {
        Filter = filter;
        var cred = CredentialManager.ReadCredential(Filter);
        string name = cred.TargetName;

        try
        {
            var p = name.Split();

            Host = p[1];
            Port = p.Length > 2 ? int.Parse(p[2]) : 25;

            UseTls = name.EndsWith("tls", StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            throw new Exception($"Windows Credential Manager '{name}' has wrong format.", ex);
        }

        UserName = cred.UserName
            ?? throw new Exception($"Windows Credential Manager '{name}' has no UserName.");
        Password = cred.Password
            ?? throw new Exception($"Windows Credential Manager '{name}' has no Password.");
    }

    public static void SendMessage(object? emails, string subj, string body, string[]? files = null)
    {
        SendMessageAsync(emails, subj, body, files).Wait();
    }

    public static async Task SendMessageAsync(object? emails, string subj, string body, string[]? files = null)
    {
        if (emails is null)
            return;

        string[] to;

        if (emails is string emails1)
        {
            to = emails1.Split(_separator,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }
        else if (emails is string[] emails2)
        {
            to = emails2;
        }
        else
            return;

        if (to.Length == 0)
            return;

        try
        {
            if (subj.Contains('\n')) //TODO CleanSubject()
            {
                subj = subj.Replace('\n', ' ');
            }

            using var mail = CreateMessage(to, subj, body, files);

            if (mail.To.Count > 0)
            {
                Logger.TimeLine($"Sending mail '{subj}'...");
                using var client = CreateClient();
                await client.SendMailAsync(mail); //send silently fails!
                await Task.Delay(1000); //time to send mail before closing the app...
                //_queue.Enqueue(mail);
            }
        }
        catch (Exception ex)
        {
            Logger.TimeLine("Sending mail failed.");
            Logger.LastError(ex);
        }
    }

    //private static string CleanSubject(string subject)
    //{
    //    try
    //    {
    //        return Regex.Replace(subject, @"[^\w\.@\-=\\/%]",
    //            string.Empty, RegexOptions.None, TimeSpan.FromSeconds(2));
    //    }
    //    catch (RegexMatchTimeoutException)
    //    {
    //        return string.Empty;
    //    }
    //}

    private static SmtpClient CreateClient()
    {
        SmtpClient client = new(Host, Port)
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(UserName, Password),
            EnableSsl = UseTls
        };

        return client;
    }

    private static MailMessage CreateMessage(string[] emails, string subj, string body, string[]? files = null)
    {
        MailMessage mail = new()
        {
            From = new(UserName, DisplayName, Encoding.UTF8),
            Subject = subj,
            Body = $"{body}{Environment.NewLine}--{Environment.NewLine}{App.Title}"
        };

        foreach (var email in emails)
        {
            if (!string.IsNullOrWhiteSpace(email) && email.Contains('@') && email.Contains('.'))
            {
                mail.To.Add(new MailAddress(email));
            }
        }

        if (files is not null)
        {
            foreach (var file in files)
            {
                FileInfo fi = new(file);

                if (fi.Exists)
                {
                    Attachment attachment = new(fi.FullName);
                    ContentDisposition disposition = attachment.ContentDisposition!;

                    disposition.CreationDate = fi.CreationTime;
                    disposition.ModificationDate = fi.LastWriteTime;
                    disposition.ReadDate = fi.LastAccessTime;

                    mail.Attachments.Add(attachment);
                }
            }
        }

        return mail;
    }
}
