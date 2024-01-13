using CrewBackend.Interfaces;
using CrewBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrewBackend.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IAccountService accountService;

        public UserController(IAccountService _accountService)
            =>  accountService = _accountService;



        [HttpPost("Login")]
        public IActionResult Login(LoginUserDto dto)
        {
            return null;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody]RegisterUserDto dto)
        {
            accountService.Register(dto, Request.Host.ToString());
            return NoContent();
        }
        [HttpGet("activate/{id}")]
        public IActionResult ActivateAccount([FromRoute]string id)
        {
            accountService.ActiveAccount(Guid.Parse(id));
            return NoContent();
        }
    }
}
