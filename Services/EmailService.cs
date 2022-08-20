using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MyBlog.Plugins.Exceptions;

namespace MyBlog.Services
{
    public class EmailService
    {
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private readonly IConfiguration configuration;

        public async Task SendEmail(string emailUser, string subject, string body)
        {
            if (emailUser == null)
            {
                throw new HttpException("هیچ ایمیلی در حساب کاربری شما ثبت نشده است", "", HttpStatusCode.NotFound);
            }

            string userName = this.configuration.GetValue<string>("EmailCode:Username");
            string password = this.configuration.GetValue<string>("EmailCode:Password");
            string host = this.configuration.GetValue<string>("EmailCode:Host");
            int port = this.configuration.GetValue<int>("EmailCode:Port");

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
