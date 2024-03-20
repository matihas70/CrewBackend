using CrewBackend.Entities;
using CrewBackend.Interfaces;

namespace CrewBackend.Models.Obesrvers
{
    public abstract class UserGroupObserverBase : IGroupObserver
    {
        protected readonly User user;
        protected readonly CrewDbContext db;
        public UserGroupObserverBase(User _userId, CrewDbContext _db)
            => (user, db) = (_userId, _db);

        public abstract void Notify(Group group);

        public User GetUser() => user;
        public CrewDbContext GetDb() => db;
    }
}
