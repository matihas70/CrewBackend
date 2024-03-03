namespace CrewBackend.Interfaces
{
    public interface IGroupNotificator
    {
        void Attach(IGroupObserver observer);
        void Detach(IGroupObserver observer);
        void SendNotifications();
    }
}
