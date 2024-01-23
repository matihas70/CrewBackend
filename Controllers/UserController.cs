using CrewBackend.Consts;
using CrewBackend.Interfaces;
using CrewBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CrewBackend.Controllers
{
    [ApiController]
    [Route("Account")]
    public class UserController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IConfiguration config;

        public UserController(IAccountService _accountService, IConfiguration _config)
            =>  (accountService, config) = (_accountService, _config);

        [HttpPost("Login")]
        public IActionResult Login(LoginUserDto dto)
        {
            ResponseModel<Guid> response = accountService.Login(dto);
            if (response.Status == Enums.StatusEnum.NotFound)
            {
                return NotFound(response.Message);
            }   
            else if(response.Status == Enums.StatusEnum.AuthenticationError)
            {
                return BadRequest(response.Message);
            }
            Response.Cookies.Append("Session", response.ResponseData.ToString(),
                new CookieOptions
                {
                    Domain = Request.Host.ToString(),
                    HttpOnly = true
                });
            string token = CreateToken(dto);

            Response.Headers.Add("Token", token);
            return Ok("Signed in successfuly");
        }

        private string CreateToken(LoginUserDto user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
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
