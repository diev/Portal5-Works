using System.Net;
using System.Net.Mail;

namespace Diev.Extensions.Smtp;

public class DefaultSmtpClient
{
    private readonly SmtpClient _smtpClient;
    private readonly SmtpSettings _settings;

    /// <summary>
    /// Публичный конструктор — для DI (ASP.NET Core и др.)
    /// </summary>
    /// <param name="settings"></param>
    public DefaultSmtpClient(SmtpSettings settings)
    {
        _smtpClient = new(settings.Host, settings.Port)
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(settings.UserName, settings.Password),
            EnableSsl = settings.UseTls
        };

        _settings = settings;
    }

    /// <summary>
    /// Статический фабричный метод — для ручного создания из настроек.
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static DefaultSmtpClient Create(SmtpSettings settings)
    {
        return new DefaultSmtpClient(settings); // Использует публичный конструктор
    }

    /// <summary>
    /// Статический фабричный метод — для создания из параметров (без явного создания SmtpSettings).
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="enableSsl"></param>
    /// <param name="display"></param>
    /// <returns></returns>
    public static DefaultSmtpClient Create(
            string host,
            int port,
            string username,
            string password,
            bool enableSsl,
            string display)
    {
        var settings = new SmtpSettings
        {
            Host = host,
            Port = port,
            UserName = username,
            Password = password,
            UseTls = enableSsl,
            DisplayName = display
        };

        return new DefaultSmtpClient(settings); // Использует публичный конструктор
    }

    public SmtpClient Client => _smtpClient;

    public SmtpSettings Settings => _settings;
}
