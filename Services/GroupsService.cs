using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using CrewBackend.Models;
using CrewBackend.Models.Dto;
using CrewBackend.Factories;
using System.Text.Json;
using System.Text.Json.Serialization;
using CrewBackend.Data.Enums;
namespace CrewBackend.Services
{
    public class GroupsService:IGroupsService
    {
        private readonly CrewDbContext db;
        private readonly IGroupNotificatorFactory notificatorFactory;
        private readonly IGroupObserverFactory groupObserverFactory;
        public GroupsService(CrewDbContext _db, IGroupNotificatorFactory _notificatorFactory, IGroupObserverFactory _groupObserverFactory) =>
            (db,  notificatorFactory, groupObserverFactory) = (_db, _notificatorFactory, _groupObserverFactory);

        public ResponseModel<object> CreateGroup(CreateGroupDto dto, long userId)
        {
            ResponseModel<object> response = new ResponseModel<object>();

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

        public ResponseModel<object> CreatePost(CreateGroupPostDto dto, long userId, long groupId)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            
            string? groupName = db.Groups.FirstOrDefault(g => g.Id == groupId)?.Name;
            if(groupName is null)
            {
                response.Status = StatusEnum.NotFound;
                response.Message = "Group not found";
                return response;
            }
            var notificationService = notificatorFactory.Create(groupId);
            List<long> taggedMembersIds = JsonSerializer.Deserialize<List<long>>(dto.TaggedMembers)!;
            foreach ( long id in taggedMembersIds)
            {
                var observer = groupObserverFactory.Create(id, userId, db);
                notificationService.Attach(observer);
            }
            notificationService.SendNotifications();
            GroupsPost groupPost = new GroupsPost
            {
                Title = dto.Title,
                Body = dto.Body,
                TaggedUsers = dto.TaggedMembers,
                CreatedBy = userId,
                CreateDate = DateTime.Now,
                GroupId = groupId,
            };

            db.GroupsPosts.Add(groupPost);
            db.SaveChanges();

            return response;
        }
    }
}
