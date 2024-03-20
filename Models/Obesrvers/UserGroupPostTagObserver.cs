using CrewBackend.Data.Enums;
using CrewBackend.Entities;
using CrewBackend.Interfaces;
using System.Text.Json;

namespace CrewBackend.Models.Obesrvers
{
    public class UserGroupPostTagObserver : UserGroupObserverBase
    {
        private readonly User taggedBy;
        public UserGroupPostTagObserver(User _user, CrewDbContext _db, User _taggedBy) : base(_user, _db)
        {
            taggedBy = _taggedBy;
        }

        public override void Notify(Group group)
        {
            UsersNotification usersNotification = new UsersNotification
            {
                UserId = user.Id,
                NotifyId = (int)NotifyEnum.TaggedUser,
                Seen = false,
                Details = JsonSerializer.Serialize(new
                {
                    userId = user.Id,
                    groupId = group.Id
                })
            };
            db.UsersNotifications.Add(usersNotification);
            db.SaveChanges();
        }
        
        public User GetTaggedBy() => taggedBy;
    }
}
