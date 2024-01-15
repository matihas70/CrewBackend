using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace CrewBackend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IDbContextFactory<CrewDbContext> dbFactory;
        public EmailService(IDbContextFactory<CrewDbContext> _dbFactory)
            => dbFactory = _dbFactory;

        public void SendActivateMail(string to, string activationLink)
        {
            string body = $"<p>To active your account click this link: <a href=\"{activationLink}\">Click!</a></p>";
            SendEmail(to, "Active account", body);

        }
        private void SendEmail(string to, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;

            mailMessage.Body = $"<!DOCTYPE html><html><body>{body}</body></html>"; ;
            mailMessage.IsBodyHtml = true;

            using CrewDbContext db = dbFactory.CreateDbContext();
            AppSetting settings = db.AppSettings.FirstOrDefault();

            mailMessage.From = new MailAddress(settings.Email);
            var client = new SmtpClient();
            client.Host = settings.EmailHost;
            client.Port = (int)settings.EmailPort;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(settings.EmailLogin, settings.EmailPassword);
            client.EnableSsl = Convert.ToBoolean(settings.EmailSsl);

            client.Send(mailMessage);
        }
    }
}
