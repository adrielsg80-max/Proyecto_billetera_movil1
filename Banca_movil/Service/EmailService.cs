using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Banca_movil.Models.Setting;

namespace Banca_movil.Service
{
    public class EmailService(IOptions<SmtpSettings> smtpOptions) : IEmailService
    {
        private readonly SmtpSettings _smtp = smtpOptions.Value;

        public void SendPasswordResetEmail(string toEmail, string body)
        {
            using var client = new SmtpClient(_smtp.Host, _smtp.Port)
            {
                Credentials = new NetworkCredential(_smtp.User, _smtp.Password),
                EnableSsl = _smtp.EnableSsl
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_smtp.User, "Soporte de Autenticación"),
                Subject = "Token de Confirmación / Recuperación",
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(toEmail);
            client.Send(mail);
        }
    }
}
