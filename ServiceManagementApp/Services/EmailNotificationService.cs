using ServiceManagementApp.Interfaces;
using System.Net;
using System.Net.Mail;

namespace ServiceManagementApp.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly SmtpClient _smtpClient;

        public EmailNotificationService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
        {
            _smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };
        }
        public async Task SendAsync(string recipient, string subject, string message)
        {
            var mailMessage = new MailMessage("pstoinov@dagaplus.com", recipient, subject, message);
            await _smtpClient.SendMailAsync(mailMessage);
        }
    } 
}
