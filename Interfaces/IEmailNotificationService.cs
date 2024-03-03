using CrewBackend.Entities;

namespace CrewBackend.Interfaces
{
    public interface IEmailNotificationService
    {
        void SendTagNotification(string to, string groupName, string taggedBy);
    }
}
