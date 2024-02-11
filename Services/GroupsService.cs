using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using CrewBackend.Models;
using CrewBackend.Models.Dto;
namespace CrewBackend.Services
{
    public class GroupsService:IGroupsService
    {
        private readonly IDbContextFactory<CrewDbContext> dbFactory;
        public GroupsService(IDbContextFactory<CrewDbContext> _dbFactory) =>
            dbFactory = _dbFactory;

        public ResponseModel<object> CreateGroup(CreateGroupDto dto, long userId)
        {
            ResponseModel<object> response = new ResponseModel<object>();

            using CrewDbContext db = dbFactory.CreateDbContext();

            if(db.Groups.Any(g => g.Name == dto.Name))
            {
                response.Status = Data.Enums.StatusEnum.ResourceExist;
                response.Message = "Group name already exist";
                return response;
            }

            Group newGroup = new Group
            {
                Name = dto.Name,
                CreatedBy = userId,
                CreateDate = DateTime.Now
            };

            UsersGroup newUserGroup = new UsersGroup
            {
                UserId = userId,
                Group = newGroup,
                RoleId = (int)Data.Enums.Roles.Admin
            };

            db.Groups.Add(newGroup);
            db.UsersGroups.Add(newUserGroup);
            db.SaveChanges();

            response.Status = Data.Enums.StatusEnum.Ok;
            return response;

        }

    }
}
