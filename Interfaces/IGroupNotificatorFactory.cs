using CrewBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrewBackend.Interfaces
{
    public interface IGroupNotificatorFactory
    {
        IGroupNotificator Create(long _groupId);
    }
}
