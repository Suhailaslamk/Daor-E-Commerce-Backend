
using Daor_E_Commerce.Application.Interfaces.IServices;
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
            var host = _config["Email:Smtp"];
            var port = _config["Email:Port"];
            var username = _config["Email:Username"];
            var password = _config["Email:Password"];
            var from = _config["Email:From"];
            var fromEmail = _config["EmailSettings:FromEmail"];
            var appName = _config["EmailSettings:AppName"];

            if (string.IsNullOrEmpty(host) ||
                string.IsNullOrEmpty(port) ||
                string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(from))
            {
                throw new Exception("Email configuration is missing");
            }

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail, appName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            var smtp = new SmtpClient(host)
            {
                Port = int.Parse(port),
                EnableSsl = true,
                Credentials = new NetworkCredential(username, password)
            };

            var mail = new MailMessage(from, toEmail, subject, body);

            try
            {
                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception("SMTP ERROR: " + ex.Message);
            }
            //await smtp.SendMailAsync(message);
        }
    }
}
