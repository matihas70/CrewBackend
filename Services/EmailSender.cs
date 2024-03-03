using CrewBackend.Entities;
using CrewBackend.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace CrewBackend.Services
{
    public abstract class EmailSender : IDisposable
    {
        protected readonly CrewDbContext db;
        private SmtpClient client = new SmtpClient();
        protected AppSetting settings;
        public EmailSender(CrewDbContext _db)
        {
            db = _db;
            LoadSettings();
            AuthenticateClient();
        }
            
        protected void SendEmail(string to, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;

            mailMessage.Body = $"<!DOCTYPE html><html><body>{body}</body></html>";
            mailMessage.IsBodyHtml = true;

            mailMessage.From = new MailAddress(settings.Email);

            client.Send(mailMessage);
        }
        private void LoadSettings()
        {
            settings = db.AppSettings.FirstOrDefault();
        }
        private void AuthenticateClient()
        {
            client.Host = settings.EmailHost;
            client.Port = (int)settings.EmailPort;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(settings.EmailLogin, settings.EmailPassword);
            client.EnableSsl = Convert.ToBoolean(settings.EmailSsl);
        }
        public void Dispose()
        {
            client?.Dispose();
        }
    }
}
