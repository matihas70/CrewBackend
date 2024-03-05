using Azure.Core;
using CrewBackend.Interfaces;
using CrewBackend.Models;
using CrewBackend.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrewBackend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Group")]
    public class GroupsController : Controller
    {
        private readonly IUserContextInfo userContextInfo;
        private readonly IGroupsService groupsService;
        public GroupsController(IUserContextInfo _userContextInfo, IGroupsService _groupsService) =>
           (userContextInfo, groupsService) = (_userContextInfo, _groupsService);
        
        
        [HttpPost]
        public IActionResult CreateGroup([FromBody]CreateGroupDto dto)
        {
            long userId = userContextInfo.GetUserId();
            ResponseModel<object> response = groupsService.CreateGroup(dto, userId);
            if(response.Status == Data.Enums.StatusEnum.ResourceExist)
            {
                return BadRequest(response.Message);
            }
            return Created();
        }
        [HttpPost("{groupId}/{userToAddId}")]
        public IActionResult AddUserToGroup(long groupId, long userToAddId)
        {
            ResponseModel<object> response = groupsService.AddUserToGroup(groupId, userToAddId);
            if(response.Status == Data.Enums.StatusEnum.NotFound)
            {
                return NotFound(response.Message);
            }
            else if (response.Status == Data.Enums.StatusEnum.ResourceExist)
            {
                return BadRequest(response.Message);
            }
            return Created();
        }
        [HttpDelete("{groupId}/{userToRemove}")]
        public IActionResult RemoveUserFromGroup(long groupId, long userToRemoveId)
        {
            ResponseModel<object> response = groupsService.RemoveUserFromGroup(groupId, userToRemoveId);
            if(response.Status == Data.Enums.StatusEnum.NotFound)
            {
                return NotFound(response.Message);
            }
            return NoContent();
        }


        [HttpPost("Post/{groupId}")]
        public IActionResult AddPostToGroup(CreateGroupPostDto dto, long groupId)
        {
            long userId = userContextInfo.GetUserId();
            ResponseModel<object> response = groupsService.CreatePost(dto, userId, groupId);
            if (response.Status == Data.Enums.StatusEnum.NotFound)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
