using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace MyBlog.Services
{
    public class EmailService(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; } = configuration;

        public async Task SendEmail(string emailUser, string subject, string body)
        {
            string userName = this.Configuration.GetValue<string>("EmailCode:Username")!;
            string password = this.Configuration.GetValue<string>("EmailCode:Password")!;
            string host = this.Configuration.GetValue<string>("EmailCode:Host")!;
            int port = this.Configuration.GetValue<int>("EmailCode:Port");

            // create message
            using MimeMessage email = new();
            email.From.Add(MailboxAddress.Parse(userName));
            email.To.Add(MailboxAddress.Parse(emailUser));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Text) { Text = body };

            // send email
            using SmtpClient smtp = new();
            await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(userName, password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            }
    }
}
