using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrewBackend.Services
{
    public class EmailNotificationService : EmailSender, IEmailNotificationService
    {
        public EmailNotificationService(CrewDbContext _db) : base(_db)
        {

        }
        public void SendTagNotification(string to, string groupName, string taggedBy)
        {
            string subject = $"Tagged in {groupName}";
            string body = $"You have been tagged in group {groupName} by {taggedBy}";
            SendEmail(to, subject, body);
        }
    }
}
