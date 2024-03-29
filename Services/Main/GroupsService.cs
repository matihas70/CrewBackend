﻿using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using CrewBackend.Models;
using CrewBackend.Models.Dto;
using CrewBackend.Factories;
using System.Text.Json;
using System.Text.Json.Serialization;
using CrewBackend.Data.Enums;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System.Linq;
namespace CrewBackend.Services
{
    public class GroupsService:IGroupsService
    {
        private readonly CrewDbContext db;
        private readonly IGroupNotificatorFactory notificatorFactory;
        private readonly IGroupObserverFactory groupObserverFactory;
        private readonly IRolesValidator rolesValidator;
        public GroupsService(CrewDbContext _db, IGroupNotificatorFactory _notificatorFactory, IGroupObserverFactory _groupObserverFactory, IRolesValidator _rolesValidator) =>
            (db,  notificatorFactory, groupObserverFactory, rolesValidator) = (_db, _notificatorFactory, _groupObserverFactory, _rolesValidator);


        public ResponseModel<OutputGroupInfo> GetGroupInfo(long groupId, long userId)
        {
            ResponseModel<OutputGroupInfo> response = new ResponseModel<OutputGroupInfo>();
            if(!rolesValidator.IsMember(userId, groupId))
            {
                response.Status = StatusEnum.NotFound;
                return response;
            }
            OutputGroupInfo? groupInfo = db.Groups.Where(g => g.Id == groupId)
                                                 .GroupJoin(db.UsersGroups, g => g.Id, ug => ug.GroupId, (g, ug) => new { g, count = ug.Count() })
                                                 .Select(x => new OutputGroupInfo(
                                                        x.g.Name,
                                                        x.count
                                                     )).FirstOrDefault();
            if(groupInfo is null)
            {
                response.Status = StatusEnum.NotFound;
                return response;
            }
            response.Status = StatusEnum.Ok;
            response.ResponseData = groupInfo;
            return response;
        }
        public ResponseModel<IEnumerable<OutputGroupPostDto>> GetGroupPosts(GetGroupPostsDto dto, long userId)
        {
            ResponseModel<IEnumerable<OutputGroupPostDto>> response = new ResponseModel<IEnumerable<OutputGroupPostDto>>();
            if(!rolesValidator.IsMember(userId, dto.groupId))
            {
                response.Status = StatusEnum.AuthorizationError;
                return response;
            }

            response.ResponseData = db.GroupsPosts.Include(x => x.CreatedByNavigation)
                                                  .Join(db.UsersGroups.Include(x => x.Role), gp => gp.CreatedBy, ug => ug.UserId, (gp, ug) => new { gp, ug })
                                                  .Where(x => x.gp.GroupId == dto.groupId)
                                                  .OrderByDescending(x => x.gp.CreateDate)
                                                  .Select(x => new OutputGroupPostDto(
                                                        new GroupPostAuthor(
                                                                x.gp.CreatedByNavigation.Id,
                                                                x.gp.CreatedByNavigation.Name + " " + x.gp.CreatedByNavigation.Surname,
                                                                x.ug.Role.Name
                                                            ),
                                                        x.gp.Id,
                                                        x.gp.Title,
                                                        x.gp.Body,
                                                        x.gp.CreateDate
                                                      )).SkipWhile(x => x.Id < dto.lastSeendId)
                                                      .ToList();
            if(response.ResponseData is null || response.ResponseData.Count() == 0) 
            {
                response.Status = StatusEnum.NoMoreContent;
            }
            else
            {
                response.Status = StatusEnum.Ok;
            }
            return response;
        }

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
        public ResponseModel<object> AddUserToGroup(long groupId, long userToAddId, long userId)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            if (!rolesValidator.IsAdmin(userId, groupId))
            {
                response.Status = StatusEnum.AuthorizationError;
                return response;
            }
            
            if(!db.Groups.Any(x => x.Id == groupId) || !db.Users.Any(x => x.Id == userToAddId)){
                response.Status = StatusEnum.NotFound;
                response.Message = "Resource not found";
                return response;
            }

            if(db.UsersGroups.Any(x => x.UserId == userToAddId && x.GroupId == groupId)){
                response.Status = StatusEnum.ResourceExist;
                response.Message = "Resource exist";
                return response;
            } 
            UsersGroup userGroup = new UsersGroup
            {
                UserId = userToAddId,
                GroupId = groupId,
                RoleId = (int)Data.Enums.Roles.Member
            };

            db.UsersGroups.Add(userGroup);
            db.SaveChanges();
            response.Status = StatusEnum.Ok;
            response.Message = "User added";
            return response;

        }
        public ResponseModel<object> RemoveUserFromGroup(long groupId, long userToRemoveId, long userId)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            if (!rolesValidator.IsAdmin(userId, groupId)){
                response.Status = StatusEnum.AuthorizationError;
                return response;
            }
            
            if(!db.UsersGroups.Any(x => x.GroupId == groupId && x.UserId == userToRemoveId))
            {
                response.Status = StatusEnum.NotFound;
                response.Message = "Resource not found";
                return response;
            }
            db.UsersGroups.Where(x => x.GroupId == groupId && x.UserId == userToRemoveId).ExecuteDelete();
            response.Status = StatusEnum.Ok;
            return response;
        }

        public ResponseModel<object> CreatePost(CreateGroupPostDto dto, long userId, long groupId)
        {
            ResponseModel<object> response = new ResponseModel<object>();
            if (!rolesValidator.IsAdmin(userId, groupId))
            {
                response.Status = StatusEnum.AuthorizationError;
                return response;
            }
            
            string? groupName = db.Groups.FirstOrDefault(g => g.Id == groupId)?.Name;
            if(groupName is null)
            {
                response.Status = StatusEnum.NotFound;
                response.Message = "Group not found";
                return response;
            }
            var notificationService = notificatorFactory.Create(groupId);
            User taggedBy = db.Users.FirstOrDefault(x => x.Id == userId)!;

            IEnumerable<User> tagged = db.Users.Where(x => dto.TaggedMembers.Any(y => y == x.Id)).AsEnumerable();
            foreach ( User user in tagged)
            {
                var observer = groupObserverFactory.Create(user, taggedBy, db);
                notificationService.Attach(observer);
            }
            notificationService.SendNotifications();
            GroupsPost groupPost = new GroupsPost
            {
                Title = dto.Title,
                Body = dto.Body,
                TaggedUsers = JsonSerializer.Serialize(dto.TaggedMembers),
                CreatedBy = userId,
                CreateDate = DateTime.Now,
                GroupId = groupId,
            };

            db.GroupsPosts.Add(groupPost);
            db.SaveChanges();
            response.Status = StatusEnum.Ok;

            return response;
        }
    }
}
