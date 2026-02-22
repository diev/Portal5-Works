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

using Diev.Extensions.Tools;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Diev.Extensions.Smtp;

public class SmtpService(
    ILogger<SmtpService> logger,
    IOptions<SmtpSettings> options
    ) : ISmtpService
{
    //private ConcurrentQueue<MailMessage> _queue = new();
    private readonly SmtpSettings settings = options.Value;

    public void SendMessage(string[]? emails, string? subj, string? body, string[]? files = null)
    {
        if (emails is null || emails.Length == 0)
            return;

        SendMessageAsync(emails, subj, body, files).Wait();
    }

    public async Task SendMessageAsync(string[]? emails, string? subj, string? body, string[]? files = null)
    {
        if (emails is null || emails.Length == 0)
            return;

        try
        {
            using var mail = CreateMessage(emails, subj, body, files);

            if (mail.To.Count > 0)
            {
                logger.LogDebug("Sending mail '{Subject}'...", subj);

                using SmtpClient client = new(settings.Host, settings.Port)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(settings.UserName, settings.Password),
                    EnableSsl = settings.UseTls
                };

                await client.SendMailAsync(mail).ConfigureAwait(false); //send silently fails!
                await Task.Delay(1000); //time to send mail before closing the app...
                //_queue.Enqueue(mail);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Sending mail failed");
        }
    }

    private MailMessage CreateMessage(string[] emails, string? subj, string? body, string[]? files = null)
    {
        subj ??= App.Name;

        if (subj.Contains('\n')) //TODO other wrong chars
        {
            subj = subj.Replace('\n', ' ');
        }

        MailMessage mail = new()
        {
            From = new(settings.UserName!, settings.DisplayName, Encoding.UTF8),
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

        if (files is null)
        {
            return mail;
        }

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

        return mail;
    }
}
