using CrewBackend.Entities;

namespace CrewBackend.Interfaces
{
    public interface IGroupObserver
    {
        void Notify(Group group);
    }
}
