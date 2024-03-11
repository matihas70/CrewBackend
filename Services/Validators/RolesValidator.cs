using CrewBackend.Entities;
using CrewBackend.Interfaces;

namespace CrewBackend.Services
{
    public class RolesValidator : IRolesValidator
    {
        private readonly CrewDbContext db;

        public RolesValidator(CrewDbContext _db)
            => db = _db;
        
        public bool IsAdmin(long userId, long groupId)
        {
            return db.UsersGroups.Any(x => x.UserId == userId && x.GroupId == groupId && x.RoleId == (int)Data.Enums.Roles.Admin);
        }
        public bool IsMember(long userId, long groupId)
        {
            return db.UsersGroups.Any(x => x.UserId == userId && x.GroupId == groupId);
        }
    }
}
