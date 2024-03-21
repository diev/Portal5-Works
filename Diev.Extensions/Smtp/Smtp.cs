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

using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

using Diev.Extensions.Credentials;
using Diev.Extensions.Info;

namespace Diev.Extensions.Smtp;

public class Smtp : IDisposable
{
    private readonly SmtpClient? _client;
    //private ConcurrentQueue<MailMessage> _queue = new();

    public string UserName { get; }
    public string DisplayName { get; set; } = $"{App.Name} {Environment.MachineName}";
    public string[]? Subscribers { get; set; }
    public string[]? SubscribersFail { get; set; }

    /// <summary>
    /// Create from Windows credentials manager
    /// </summary>
    public Smtp()
    {
        string host;
        int port;
        bool useTls;

        string filter = "SMTP *";
        var cred = CredentialManager.ReadCredential(filter);
        string name = cred.TargetName;

        try
        {
            var p = name.Split(' ');

            host = p[1];
            port = p.Length > 2 ? int.Parse(p[2]) : 25;

            useTls = name.EndsWith("tls", StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            throw new Exception($"Windows Credential Manager '{name}' has wrong format.", ex);
        }

        UserName = cred.UserName
            ?? throw new Exception($"Windows Credential Manager '{name}' has no UserName.");
        string pass = cred.Password
            ?? throw new Exception($"Windows Credential Manager '{name}' has no Password.");

        _client = new(host, port)
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(UserName, pass),
            EnableSsl = useTls
        };
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && _client != null)
        {
            _client.Dispose();
        }
    }

    public async Task SendMessageAsync(string subj, string body, string[]? files = null) =>
        await SendMessageAsync(Subscribers, subj, body, files);

    public async Task SendFailMessageAsync(string subj, string body, string[]? files = null) =>
        await SendMessageAsync(SubscribersFail, subj, body, files);

    public async Task SendMessageAsync(string[]? emails, string subj, string body, string[]? files = null)
    //public void SendMessage(IEnumerable<string> emails, string subj, string  body, IEnumerable<string> files)
    {
        if (emails is null || emails.Length == 0 || _client is null)
            return;

        try
        {
            using MailMessage mail = new()
            {
                From = new(UserName, DisplayName, Encoding.UTF8),
                Subject = subj,
                Body = $"{body}{Environment.NewLine}--{Environment.NewLine}{App.Title}"
            };

            foreach (var email in emails)
            {
                mail.To.Add(email);
            }

            if (files != null)
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

            await _client.SendMailAsync(mail);
            //_queue.Enqueue(mail);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Sending mail '{subj}' failed.");
            Console.WriteLine(ex.ToString());
        }
    }
}
