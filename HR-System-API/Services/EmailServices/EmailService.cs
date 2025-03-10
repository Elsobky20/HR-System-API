using HR_System_API.DTO;
using MailKit.Security;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using System.Text;

namespace HR_System_API.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _configuration;

        public EmailService(IOptions<EmailConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public async Task<string> SendEmailAsync(string Name, string Link, string Email)
        {

            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                // Create an email message based on the provided authentication details
                var mailMessage = CreateEmailMessage(Name,Link,Email);

                // Connect to the SMTP server using the configured settings
                await client.ConnectAsync(_configuration.SmtpServer, _configuration.Port, true);

                // Remove the XOAUTH2 authentication mechanism
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Authenticate with the SMTP server using the provided credentials
                await client.AuthenticateAsync(_configuration.UserName, _configuration.Password);

                // Send the email message
                await client.SendAsync(mailMessage);

                // If the email is sent successfully, return "success"
                return "success";
            }
            catch (SmtpException ex)
            {
                // Return a meaningful error message
                return "An SMTP error occurred while sending an email. Please try again later.";
            }
            catch (Exception ex)
            {
                // Return a generic error message
                return "An error occurred while sending an email. Please try again later.";
            }
            finally
            {
                // Disconnect and dispose the client
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
        private MimeMessage CreateEmailMessage(string Name, string Link,string Email)
        {

            var content = $@"
{Name} مرحبًا،

لقد تلقينا طلبًا لإعادة تعيين كلمة المرور لحسابك لدينا.

إذا كنت أنت من طلب ذلك، يرجى الضغط على الرابط التالي لإعادة تعيين كلمة المرور:
{Link}

إذا لم تطلب ذلك، يمكنك تجاهل هذه الرسالة، وستظل كلمة المرور الخاصة بك آمنة.

شكرًا لك،
فريق الدعم ";

            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("شركة ...", _configuration.From));
            mailMessage.To.Add(new MailboxAddress(Name,Email));
            mailMessage.Subject = "إعادة تعيين كلمة المرور";

            mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = content };

            return mailMessage;
        }

    }
}
