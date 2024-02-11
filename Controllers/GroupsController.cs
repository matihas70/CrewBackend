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
    }
}
