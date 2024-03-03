using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrewBackend.Services
{
    public class GroupMembersNotificator:IGroupNotificator
    {
        private readonly IDbContextFactory<CrewDbContext> dbFactory;
        private readonly IEmailNotificationService notificationService;
        private readonly long groupId;
        public GroupMembersNotificator(IDbContextFactory<CrewDbContext> _dbFactory, long _groupId) =>
            (dbFactory, groupId) = (_dbFactory, _groupId);

        private List<IGroupObserver> observers = [];
        public void Attach(IGroupObserver observer)
        {
            observers.Add(observer);
        }
        public void Detach(IGroupObserver observer)
        {
            observers.Remove(observer);
        }
        public void SendNotifications()
        {
            using CrewDbContext db = dbFactory.CreateDbContext();
            string? groupName = db.Groups.FirstOrDefault(g => g.Id == groupId)?.Name;

            if(groupName is null)
            {
                throw new Exception("Group not found");
            }
            foreach (IGroupObserver observer in observers)
            {
                observer.Notify(groupName);
            }
        }
    }
}
