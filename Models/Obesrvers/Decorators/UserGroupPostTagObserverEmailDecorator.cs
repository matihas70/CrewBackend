using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrewBackend.Models.Obesrvers.Decorators
{
    public class UserGroupPostTagObserverEmailDecorator : UserGroupObserverDecorator
    {
        private readonly IEmailNotificationService emailNotificator;
        public UserGroupPostTagObserverEmailDecorator(UserGroupPostTagObserver groupObserver, 
                                                      IEmailNotificationService _emailNotificator) : base(groupObserver) 
            => emailNotificator = _emailNotificator;

        public void Notify(Group group)
        {
            base.Notify(group);

            var db = groupObserver.GetDb();
            
            User taggedBy = groupObserver.GetTaggedBy();

            string? email = groupObserver.GetUser().Email;
            string taggedByName = taggedBy.Name + taggedBy.Surname;

            if (email != null)
            {
                emailNotificator.SendTagNotification(email, group.Name, taggedByName);
                return;
            }
        }
    }
}
