using CrewBackend.Consts;
using CrewBackend.Interfaces;
using CrewBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CrewBackend.Controllers
{
    [ApiController]
    [Route("Account")]
    public class UserController : Controller
    {
        private readonly IAccountService accountService;

        public UserController(IAccountService _accountService)
            =>  accountService = _accountService;



        [HttpPost("Login")]
        public IActionResult Login(LoginUserDto dto)
        {
            ResponseModel<Guid> response = accountService.Login(dto);
            if (response.Status == Enums.StatusEnum.NotFound)
            {
                return NotFound(response);
            }   
            else if(response.Status == Enums.StatusEnum.AuthenticationError)
            {
                return BadRequest(response);
            }
            Response.Cookies.Append("Session", response.ResponseData.ToString());

            return NoContent();
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody]RegisterUserDto dto)
        {
            ResponseModel<object> response = accountService.Register(dto, CreateActivationLink());
            if (response.Status == Enums.StatusEnum.ResourceExist)
                return BadRequest(response);

            return Created();
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
            return "https://" + Request.Host.ToString() + "/Account/activate/";
        }

        [HttpGet("activate/{id}")]
        public IActionResult ActivateAccount([FromRoute]string id)
        {
            ResponseModel<object> response = accountService.ActiveAccount(Guid.Parse(id));
            if(response.Status == Enums.StatusEnum.NotFound)
                return BadRequest(response);
            else if(response.Status == Enums.StatusEnum.Expired)
                return Redirect(Urls.Front.Activated + "?isActivated=false");
            

            return Redirect(Urls.Front.Activated + "?isActivated=true");
        }
    }
}
