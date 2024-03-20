using CrewBackend.Entities;
using CrewBackend.Interfaces;
using CrewBackend.Models;
using CrewBackend.Models.Obesrvers;
using CrewBackend.Models.Obesrvers.Decorators;

namespace CrewBackend.Factories
{
    public class GroupObserverFactory:IGroupObserverFactory
    {
        private readonly IEmailNotificationService emailNotificator;
        public GroupObserverFactory(IEmailNotificationService _emailNotificator)
        {
            emailNotificator = _emailNotificator;
        }

        public IGroupObserver Create(User user, User taggedBy, CrewDbContext db)
        {
            var groupObserver = new UserGroupPostTagObserver(user, db, taggedBy);

            if (checkIfUserWantEmailNotification(user))
                return new UserGroupPostTagObserverEmailDecorator(groupObserver, emailNotificator);

            return groupObserver;
        }

        private bool checkIfUserWantEmailNotification(User user)
        {

            return true;
        }
    }
}
