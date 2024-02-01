using CrewBackend.Interfaces;
using CrewBackend.Models;
using CrewBackend.Models.Dto;
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
            long id = GetUserId();
            ResponseModel<UserDataDto> response = userService.GetUserData(id);
            if (response.Status == Enums.StatusEnum.Ok)
                return Ok(response.ResponseData);

            return BadRequest();
        }
        [HttpGet("ProfilePhoto")]
        public IActionResult GetUserProfilePhoto([FromQuery] long id)
        {
            return null;
        }
        [HttpGet("ProfilePicture")]
        public IActionResult GetProfilePicture()
        {
            long userId = GetUserId();
            ResponseModel<byte[]> response = userService.GetProfilePicture(userId);

            if (response.Status == Enums.StatusEnum.NotFound)
            {
                return NotFound();
            }
            return File(response.ResponseData, "image/jpg");

        }

        [HttpPatch]
        public IActionResult SaveUserData([FromBody] SaveUserDataDto dto)
        {
            ResponseModel<object> response = userService.SaveUserData(dto, GetUserId());

            if(response.Status == Enums.StatusEnum.NotFound)
            {
                return BadRequest();
            }

            return Ok("Saved successfuly");

        }
        [HttpPatch("SaveProfilePhoto")]
        public async Task<IActionResult> SaveUserPhoto()
        {
            long userId = GetUserId();
            using var buffer = new System.IO.MemoryStream();
            await Request.Body.CopyToAsync(buffer);
            var imageBytes = buffer.ToArray();
            userService.SaveUserProfilePicture(userId, imageBytes);
            return NoContent();
        }

        private long GetUserId()
        {
            var identity = Request.HttpContext.User.Identity as ClaimsIdentity;
            return Int64.Parse(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
