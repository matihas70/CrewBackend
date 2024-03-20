using CrewBackend.Entities;

namespace CrewBackend.Interfaces
{
    public interface IGroupObserverFactory
    {
        IGroupObserver Create(User user, User taggedBy, CrewDbContext db);
    }
}
