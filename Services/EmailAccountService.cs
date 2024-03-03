using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace CrewBackend.Services
{
    public class EmailAccountService : EmailSender, IEmailAccountService
    {
        public EmailAccountService(CrewDbContext _db) : base(_db) { }

        public void SendActivateMail(string to, string activationLink)
        {
            string body = $"<p>To active your account click this link: <a href=\"{activationLink}\">Click!</a></p>";
            SendEmail(to, "Active account", body);

        }
    }
}
