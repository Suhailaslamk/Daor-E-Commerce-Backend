using Daor_E_Commerce.Application.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Daor_E_Commerce.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string toEmail, string subject, string body)
        {
            var smtp = new SmtpClient
            {
                Host = _config["Email:Smtp"],
                Port = int.Parse(_config["Email:Port"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    _config["Email:Username"],
                    _config["Email:Password"]
                )
            };

            var mail = new MailMessage(
                from: _config["Email:From"],
                to: toEmail,
                subject,
                body
            );

            await smtp.SendMailAsync(mail);
        }
    }
}
