using CrewBackend.Data.Enums;
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
        private readonly IUserContextInfo userContextInfo;
        private readonly IUserService userService;
        public UserController(IUserContextInfo _userContextInfo, IUserService _userService) => 
            (userContextInfo, userService) = (_userContextInfo, _userService);

        [HttpGet]
        public IActionResult GetUserData()
        {
            long id = GetUserId();
            ResponseModel<GetUserDataDto> response = userService.GetUserData(id);
            if (response.Status == StatusEnum.Ok)
                return Ok(response.ResponseData);

            return BadRequest();
        }
        [HttpGet("Education")]
        public IActionResult GetUserEducationData()
        {
            long id = GetUserId();
            ResponseModel<List<GetUserEducationDto>> response = userService.GetUserEducationData(id);
            if(response.Status == StatusEnum.NotFound)
            {
                return NotFound();
            }
            return Ok(new { dto = response.ResponseData, educationTypes = Data.Dictionaries.EducationTypes });
        }
        [HttpGet("Photo")]
        public IActionResult GetProfilePicture()
        {
            long userId = GetUserId();
            ResponseModel<byte[]> response = userService.GetProfilePicture(userId);

            if (response.Status == StatusEnum.NotFound)
            {
                return NotFound();
            }
            return File(response.ResponseData, "image/jpg");

        }

        [HttpPatch]
        public IActionResult SaveUserData([FromBody] SaveUserDataDto dto)
        {
            ResponseModel<object> response = userService.SaveUserData(dto, GetUserId());

            if(response.Status == StatusEnum.NotFound)
            {
                return BadRequest();
            }

            return Ok("Saved successfuly");

        }
        [HttpPatch("Photo")]
        public async Task<IActionResult> SaveUserPhoto()
        {
            long userId = GetUserId();
            using var buffer = new System.IO.MemoryStream();
            await Request.Body.CopyToAsync(buffer);
            var imageBytes = buffer.ToArray();
            userService.SaveUserProfilePicture(userId, imageBytes);
            return NoContent();
        }
        [HttpPut("Education")]
        public IActionResult SaveUserEducation([FromBody]List<SaveUserEducationDto> dto)
        {
            long userId = GetUserId();
            ResponseModel<IEnumerable<long>> response = userService.SaveEducationData(dto, userId);
            
            if(response.Status == StatusEnum.NotFound)
            {
                return NotFound();
            }

            return Ok(response.ResponseData);
        }
        private long GetUserId()
        {
            var identity = Request.HttpContext.User.Identity as ClaimsIdentity;
            return Int64.Parse(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
