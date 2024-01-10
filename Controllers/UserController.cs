using CrewBackend.Interfaces;
using CrewBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrewBackend.Controllers
{
    [ApiController]
    [Route("/User")]
    public class UserController : Controller
    {
        private readonly IAccountService accountService;

        public UserController(IAccountService _accountService)
            =>  accountService = _accountService;



        [HttpPost("/Login")]
        public IActionResult Login(LoginUserDto dto)
        {
            return null;
        }

        [HttpPost("/Register")]
        public IActionResult Register(RegisterUserDto dto)
        {
            accountService.Register(dto);
            return NoContent();
        }
    }
}
