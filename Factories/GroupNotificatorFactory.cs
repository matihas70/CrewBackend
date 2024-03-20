using CrewBackend.Entities;
using CrewBackend.Interfaces;
using CrewBackend.Services;
using Microsoft.EntityFrameworkCore;
namespace CrewBackend.Factories
{
    public class GroupNotificatorFactory : IGroupNotificatorFactory
    {
        private readonly CrewDbContext db;
        private readonly IEmailNotificationService emailNotificationService;
        public GroupNotificatorFactory(CrewDbContext _db)
            => (db) = (_db);
        public IGroupNotificator Create(long groupId)
        {
            return new GroupNotificator(db, groupId);
        }
    }
}
