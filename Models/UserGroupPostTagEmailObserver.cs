using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrewBackend.Models
{
    public class UserGroupPostTagEmailObserver : IGroupObserver
    {
        private readonly long userId;
        private readonly long taggedBy;
        private readonly IEmailNotificationService emailNotificator;
        private readonly CrewDbContext db;
        public UserGroupPostTagEmailObserver(long _userId, long _taggedBy, IEmailNotificationService _emailNotificator, CrewDbContext _db)
            => (userId, taggedBy, emailNotificator, db) = (_userId, _taggedBy, _emailNotificator, _db);
        
        public void Notify(string groupName)
        {
            string? email = db.Users.AsNoTracking().FirstOrDefault(u => u.Id == userId)?.Email;
            string? taggedByName = db.Users.AsNoTracking()
                                           .Where(u => u.Id == taggedBy)
                                           .Select(x => x.Name + x.Surname)
                                           .FirstOrDefault();
            
            if (email != null)
            {
                emailNotificator.SendTagNotification(email, groupName, taggedByName);
                return;
            }
            //emailNotification.SendTagNotification(email, groupName)
        }
    }
}
