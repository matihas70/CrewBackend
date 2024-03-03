using CrewBackend.Entities;
using CrewBackend.Interfaces;
using CrewBackend.Models;

namespace CrewBackend.Factories
{
    public class GroupObserverFactory:IGroupObserverFactory
    {
        private readonly IEmailNotificationService emailNotificator;
        public GroupObserverFactory(IEmailNotificationService _emailNotificator)
        {
            emailNotificator = _emailNotificator;
        }

        public IGroupObserver Create(long userId, long taggedBy, CrewDbContext db)
        {
            return new UserGroupPostTagEmailObserver(userId, taggedBy, emailNotificator, db);
        }
    }
}
