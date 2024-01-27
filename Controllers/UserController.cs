using CrewBackend.Interfaces;
using CrewBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrewBackend.Controllers
{
    [ApiController]
    [Route("User")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        public UserController(IUserService _userService)
            => userService = _userService;

        [HttpGet]
        public IActionResult GetUserData()
        {
            var identity = Request.HttpContext.User.Identity as ClaimsIdentity;
            long id = Int64.Parse(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            ResponseModel<UserDataDto> response = userService.GetUserData(id);
            if(response.Status == Enums.StatusEnum.Ok)
                return Ok(response.ResponseData);

            return BadRequest();
        }
    }
}
