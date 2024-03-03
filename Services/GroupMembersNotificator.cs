using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrewBackend.Services
{
    public class GroupMembersNotificator:IGroupNotificator
    {
        private readonly CrewDbContext db;
        private readonly IEmailNotificationService notificationService;
        private readonly long groupId;
        public GroupMembersNotificator(CrewDbContext _db, long _groupId) =>
            (db, groupId) = (_db, _groupId);

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
