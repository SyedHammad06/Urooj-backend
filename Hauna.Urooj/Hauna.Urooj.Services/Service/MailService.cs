using Hauna.Urooj.Hauna.Urooj.Models;
using Hauna.Urooj.Hauna.Urooj.Services.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Hauna.Urooj.Hauna.Urooj.Services.Service
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailsettings) 
        {
            _mailSettings = mailsettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailDetails)
        {
            string FilePath = Directory.GetCurrentDirectory() + "/Template/MailTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            var email = new MimeMessage();
            MailText = MailText.Replace("[username]", mailDetails.UserName).Replace("[password]", mailDetails.Password);
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailDetails.ToEmail));
            email.Subject = "Congratulations your account has been approved";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            return;
        }

        public async Task SendEmailForSubAsync(MailRequest mailDetails)
        {
            mailDetails.ToEmail = _mailSettings.Mail;
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailDetails.ToEmail));
            email.Subject = "A Query has been Requested";
            var builder = new BodyBuilder();
            builder.HtmlBody = mailDetails.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            return;
        }
    }
}
