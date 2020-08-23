using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using JunsBlog.Interfaces.Settings;
using JunsBlog.Models.Notifications;
using MailKit.Net.Smtp;
using MimeKit;

namespace JunsBlog.Models.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly IEmailSettings emailSettings;

        public EmailNotificationService(IEmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }

        public void SendNotification(Notification notification)
        {
            using var client = new SmtpClient();
            client.Connect(emailSettings.Host, emailSettings.Port);
            client.Authenticate(emailSettings.Username, emailSettings.Password);
            client.Send(CreateEmailMessage(notification));
            client.Disconnect(true);
        }

        private MimeMessage CreateEmailMessage(Notification notification)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(emailSettings.Sender, emailSettings.From));
            mimeMessage.To.Add(new MailboxAddress(notification.ReceiverName, notification.ReceiverEmail));
            mimeMessage.Subject = notification.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = notification.Message };
            return mimeMessage;
        }
    }
}
