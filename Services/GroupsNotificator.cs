using CrewBackend.Data.Enums;
using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrewBackend.Services
{
    public class GroupNotificator:IGroupNotificator
    {
        private readonly CrewDbContext db;
        private readonly long groupId;
        public GroupNotificator(CrewDbContext _db, long _groupId) =>
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
            Group? group = db.Groups.AsNoTracking().FirstOrDefault(g => g.Id == groupId);

            if(group is null)
            {
                throw new Exception("Group not found");
            }
            foreach (IGroupObserver observer in observers)
            {
                observer.Notify(group);
            }
        }
    }
}
