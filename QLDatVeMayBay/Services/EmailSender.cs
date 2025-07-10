using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using System.Text;

namespace QLDatVeMayBay.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<EmailSettings> emailSettings, ILogger<EmailSender> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = message };

                using var client = new SmtpClient();

                // Thêm logging để debug
                _logger.LogInformation($"Connecting to SMTP server: {_emailSettings.SmtpServer}:{_emailSettings.Port}");

                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);

                _logger.LogInformation($"Authenticating with username: {_emailSettings.Username}");

                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);

                _logger.LogInformation($"Sending email to: {email}");

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Email sent successfully to: {email}");
            }
            catch (AuthenticationException ex)
            {
                _logger.LogError(ex, $"Authentication failed when sending email to {email}");
                throw new InvalidOperationException("Lỗi xác thực email. Vui lòng kiểm tra cấu hình App Password.", ex);
            }
            catch (SmtpCommandException ex)
            {
                _logger.LogError(ex, $"SMTP command failed when sending email to {email}");
                throw new InvalidOperationException($"Lỗi SMTP: {ex.Message}", ex);
            }
            catch (SmtpProtocolException ex)
            {
                _logger.LogError(ex, $"SMTP protocol error when sending email to {email}");
                throw new InvalidOperationException($"Lỗi giao thức SMTP: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"General error when sending email to {email}");
                throw new InvalidOperationException($"Lỗi gửi email: {ex.Message}", ex);
            }
        }

        public async Task SendVerificationEmailAsync(string email, string verificationCode, string userName)
        {
            var subject = "Xác nhận đăng ký tài khoản - Hệ thống đặt vé máy bay";
            var message = GenerateVerificationEmailHtml(verificationCode, userName);
            await SendEmailAsync(email, subject, message);
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetToken, string userName)
        {
            var subject = "Đặt lại mật khẩu - Hệ thống đặt vé máy bay";
            var message = GeneratePasswordResetEmailHtml(resetToken, userName);
            await SendEmailAsync(email, subject, message);
        }

        private string GenerateVerificationEmailHtml(string verificationCode, string userName)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #007bff; color: white; padding: 20px; text-align: center; }}
                    .content {{ padding: 20px; background-color: #f8f9fa; }}
                    .code {{ background-color: #007bff; color: white; padding: 15px; text-align: center; 
                            font-size: 24px; font-weight: bold; margin: 20px 0; border-radius: 5px; }}
                    .footer {{ background-color: #6c757d; color: white; padding: 15px; text-align: center; font-size: 12px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>🛫 Hệ thống đặt vé máy bay</h1>
                    </div>
                    <div class='content'>
                        <h2>Xin chào {userName}!</h2>
                        <p>Cảm ơn bạn đã đăng ký tài khoản tại hệ thống đặt vé máy bay của chúng tôi.</p>
                        <p>Để hoàn tất quá trình đăng ký, vui lòng nhập mã xác nhận sau:</p>
                        <div class='code'>{verificationCode}</div>
                        <p><strong>Lưu ý:</strong></p>
                        <ul>
                            <li>Mã xác nhận có hiệu lực trong <strong>15 phút</strong></li>
                            <li>Nếu bạn không thực hiện đăng ký này, vui lòng bỏ qua email này</li>
                            <li>Không chia sẻ mã xác nhận với bất kỳ ai</li>
                        </ul>
                        <p>Nếu bạn cần hỗ trợ, vui lòng liên hệ với chúng tôi.</p>
                    </div>
                    <div class='footer'>
                        <p>© 2024 Hệ thống đặt vé máy bay. Tất cả quyền được bảo lưu.</p>
                        <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                    </div>
                </div>
            </body>
            </html>";
        }

        private string GeneratePasswordResetEmailHtml(string resetToken, string userName)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #dc3545; color: white; padding: 20px; text-align: center; }}
                    .content {{ padding: 20px; background-color: #f8f9fa; }}
                    .button {{ background-color: #dc3545; color: white; padding: 12px 24px; 
                              text-decoration: none; border-radius: 5px; display: inline-block; margin: 20px 0; }}
                    .footer {{ background-color: #6c757d; color: white; padding: 15px; text-align: center; font-size: 12px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>🔒 Đặt lại mật khẩu</h1>
                    </div>
                    <div class='content'>
                        <h2>Xin chào {userName}!</h2>
                        <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.</p>
                        <p>Để đặt lại mật khẩu, vui lòng nhấp vào nút bên dưới:</p>
                        <a href='#' class='button'>Đặt lại mật khẩu</a>
                        <p><strong>Lưu ý:</strong></p>
                        <ul>
                            <li>Link đặt lại mật khẩu có hiệu lực trong <strong>30 phút</strong></li>
                            <li>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này</li>
                            <li>Để bảo mật tài khoản, vui lòng không chia sẻ link này với ai</li>
                        </ul>
                    </div>
                    <div class='footer'>
                        <p>© 2024 Hệ thống đặt vé máy bay. Tất cả quyền được bảo lưu.</p>
                        <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}