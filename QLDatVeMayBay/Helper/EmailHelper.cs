using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace QLDatVeMayBay.Helper
{
    public static class EmailHelper
    {
        public static async Task SendEmailAsync(IConfiguration configuration, string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(configuration["EmailSettings:SenderName"], configuration["EmailSettings:SenderEmail"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(configuration["EmailSettings:SmtpServer"], int.Parse(configuration["EmailSettings:Port"]), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(configuration["EmailSettings:Username"], configuration["EmailSettings:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
