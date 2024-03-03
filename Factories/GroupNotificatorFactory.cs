using CrewBackend.Entities;
using CrewBackend.Interfaces;
using CrewBackend.Services;
using Microsoft.EntityFrameworkCore;
namespace CrewBackend.Factories
{
    public class GroupNotificatorFactory : IGroupNotificatorFactory
    {
        private readonly IDbContextFactory<CrewDbContext> dbFactory;
        private readonly IEmailNotificationService emailNotificationService;
        public GroupNotificatorFactory(IDbContextFactory<CrewDbContext> _dbFactory)
            => (_dbFactory) = (dbFactory);
        public IGroupNotificator Create(long groupId)
        {
            return new GroupMembersNotificator(dbFactory, groupId);
        }
    }
}
