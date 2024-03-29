﻿using CrewBackend.Data.Consts;
using CrewBackend.Data.Enums;
using CrewBackend.Interfaces;
using CrewBackend.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService _accountService, IConfiguration _config)
            =>  (accountService) = (_accountService);

        [HttpPost("Login")]
        public IActionResult Login(LoginUserDto dto)
        {
            ResponseModel<LoginOutput> response = accountService.Login(dto);
            if (response.Status == StatusEnum.NotFound)
            {
                return NotFound(response.Message);
            }   
            else if(response.Status == StatusEnum.AuthenticationError)
            {
                return BadRequest(response.Message);
            }
            Response.Cookies.Append("Session", response.ResponseData.guid.ToString(),
                new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(7),
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                    Secure = true
                });

            return Ok(response.ResponseData.token);
        }

        [HttpPost("Refresh")]
        public IActionResult CheckSession()
        {
            string session = Request.Cookies["Session"];
            if (session.IsNullOrEmpty()) 
            {
                Response.Cookies.Delete("Session");
                return Unauthorized();
            }
            string token = accountService.GetToken(session);
            if (token == null)
            {
                Response.Cookies.Delete("Session");
                return Unauthorized();
            }

            return Ok(token);
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody]RegisterUserDto dto)
        {
            
            ResponseModel<object> response = accountService.Register(dto, CreateActivationLink());
            if (response.Status == StatusEnum.ResourceExist)
                return BadRequest(response);

            return Created();
        }
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            string? session = Request.Cookies["Session"];
            Response.Cookies.Delete("Session", new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true
            });
            if (session.IsNullOrEmpty())
            {
                return Unauthorized();
            }
            if (!accountService.Logout(session))
            {
                return Unauthorized();
            }
            return NoContent();
        }
        [HttpPost("SendActivationMail")]
        public IActionResult SendActivationMail([FromBody]string email)
        {
            ResponseModel<object> response = accountService.SendActivationMail(email, CreateActivationLink());
            if (response.Status == StatusEnum.NotFound)
                return NotFound(response);
            else if (response.Status == StatusEnum.ResourceExist)
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
            if(response.Status == StatusEnum.NotFound)
                return BadRequest(response);
            else if(response.Status == StatusEnum.Expired)
                return Redirect(Urls.Front.Activated + "?isActivated=false");
            

            return Redirect(Urls.Front.Activated + "?isActivated=true");
        }
    }
}