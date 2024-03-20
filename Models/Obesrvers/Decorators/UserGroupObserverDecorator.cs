using CrewBackend.Interfaces;
using CrewBackend.Entities;

namespace CrewBackend.Models.Obesrvers.Decorators
{
    public abstract class UserGroupObserverDecorator : IGroupObserver
    {
        protected readonly UserGroupPostTagObserver groupObserver;
        public UserGroupObserverDecorator(UserGroupPostTagObserver _groupObserver)
            => groupObserver = _groupObserver;

        public void Notify(Group group)
        {
            groupObserver.Notify(group);
        }
    }
}
