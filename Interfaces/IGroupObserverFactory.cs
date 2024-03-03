using CrewBackend.Entities;

namespace CrewBackend.Interfaces
{
    public interface IGroupObserverFactory
    {
        IGroupObserver Create(long userId, long taggedBy, CrewDbContext db);
    }
}
