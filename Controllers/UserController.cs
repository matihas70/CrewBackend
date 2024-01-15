using CrewBackend.Consts;
using CrewBackend.Interfaces;
using CrewBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System;

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
            ResponseModel<object> response = accountService.Login(dto);
            if (response.Status == Enums.StatusEnum.NotFound)
                return NotFound(response);
            else if(response.Status == Enums.StatusEnum.AuthenticationError)
                return BadRequest(response);

            return NoContent();
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody]RegisterUserDto dto)
        {
            ResponseModel<object> response = accountService.Register(dto, CreateActivationLink());
            if (response.Status == Enums.StatusEnum.ResourceExist)
                return BadRequest(response);

            return NoContent();
        }
        [HttpPost("SendActivationMail")]
        public IActionResult SendActivationMail([FromBody]string email)
        {
            ResponseModel<object> response = accountService.SendActivationMail(email, CreateActivationLink());
            if (response.Status == Enums.StatusEnum.NotFound)
                return NotFound(response);
            else if (response.Status == Enums.StatusEnum.ResourceExist)
                return BadRequest(response);

            return NoContent();

        }

        private string CreateActivationLink()
        {
            return "https://" + Request.Host.ToString() + "/User/activate/";
        }

        [HttpGet("activate/{id}")]
        public IActionResult ActivateAccount([FromRoute]string id)
        {
            ResponseModel<object> response = accountService.ActiveAccount(Guid.Parse(id));
            if(response.Status == Enums.StatusEnum.NotFound)
                return BadRequest(response);
            else if(response.Status == Enums.StatusEnum.Expired)
                return Redirect(Urls.Activated + "?isActivated=false");
            
            return Redirect(Urls.Activated + "?isActivated=true");
        }
    }
}
