using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace LinkedMink.Net.Message
{
    public class EmailService : IEmailService
    {
        public EmailService(IOptionsSnapshot<EmailOptions> options, ILogger<EmailService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task SendAdministratorEmailAsync(string subject, string message)
        {
            MimeMessage email = new MimeMessage();

            email.From.Add(new MailboxAddress(_options.FromAddress.Name, _options.FromAddress.Address));
            email.To.Add(new MailboxAddress(_options.AdministratorAddress.Name, _options.AdministratorAddress.Address));
            email.Subject = subject;
            email.Body = new TextPart("plain") { Text = message };

            await SendEmailAsync(email);
        }

        public async Task SendEmailAsync(string toAddress, string subject, string message)
        {
            MimeMessage email = new MimeMessage();

            email.From.Add(new MailboxAddress(_options.FromAddress.Name, _options.FromAddress.Address));
            email.To.Add(new MailboxAddress(string.Empty, toAddress));
            email.Subject = subject;
            email.Body = new TextPart("plain") { Text = message };

            await SendEmailAsync(email);
        }

        public async Task SendEmailAsync(string toAddress, string subject, string html, string alternative)
        {
            MimeMessage email = new MimeMessage();

            email.From.Add(new MailboxAddress(_options.FromAddress.Name, _options.FromAddress.Address));
            email.To.Add(new MailboxAddress(string.Empty, toAddress));
            email.Subject = subject;

            MultipartAlternative body = new MultipartAlternative
            {
                new TextPart("html") {Text = html},
                new TextPart("plain") {Text = alternative}
            };
            email.Body = body;

            await SendEmailAsync(email);
        }

        private async Task SendEmailAsync(MimeMessage message)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.Timeout = _options.TimeoutMilliseconds;
                client.LocalDomain = _options.ClientDomain;

                _logger.LogDebug($"Connecting: {_options.MailServer.Address}:{_options.MailServer.Port}");

                await client.ConnectAsync(
                    _options.MailServer.Address,
                    _options.MailServer.Port,
                    _options.MailServer.Authentication);

                if (_options.MailServer.Authentication != SecureSocketOptions.None)
                {
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_options.MailServer.Username, _options.MailServer.Password);
                }

                await client.SendAsync(message);
            }
        }

        private readonly EmailOptions _options;
        private readonly ILogger _logger;
    }
}
