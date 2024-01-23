using CrewBackend.Entities;
using CrewBackend.Models;
using Microsoft.EntityFrameworkCore.Internal;
using CrewBackend.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CrewBackend.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDbContextFactory<CrewDbContext> dbFactory;
        private readonly IEmailService emailService;
        private readonly IConfiguration config;
        public AccountService(IDbContextFactory<CrewDbContext> _dbFactory, IEmailService _emailService, IConfiguration _config)
            => (dbFactory, emailService, config) = (_dbFactory, _emailService, _config);

        
        public ResponseModel<object> Register(RegisterUserDto dto, string link)
        {
            using var db = dbFactory.CreateDbContext();
            ResponseModel<object> response = new ResponseModel<object>();
            if (db.Users.Any(u => u.Email == dto.Email))
            {
                response.Status = Enums.StatusEnum.ResourceExist;
                response.Message = "Account with given name already exist";
                return response;
            }
                

            byte[] bytes = Encoding.UTF8.GetBytes(dto.Password);
            byte[] hashedBytes = SHA256.HashData(bytes);
            
            User user = new User
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                Password = Convert.ToBase64String(hashedBytes),
                Activated = 0,
                CreateDate = DateTime.Now,
            };
            db.Users.Add(user);
            Guid guid = Guid.NewGuid();

            db.ActivateAccountRequests.Add(new ActivateAccountRequest
            {
                Id = guid,
                User = user,
                ExpirationDate = DateTime.Now.AddHours(2),
            });
            emailService.SendActivateMail(dto.Email, link + guid);
            db.SaveChanges();

            response.Status = Enums.StatusEnum.Ok;
            return response;
        }
        public ResponseModel<LoginOutput> Login(LoginUserDto dto)
        {
            using CrewDbContext db = dbFactory.CreateDbContext();

            User user = db.Users.FirstOrDefault(u => u.Email == dto.Email);
            ResponseModel<LoginOutput> response = new ResponseModel<LoginOutput>();
            if (user == null)
            {
                response.Status = Enums.StatusEnum.AuthenticationError;
                response.Message = "Wrong email or password";
                return response;
            }

            byte[] passwordBytes = Encoding.UTF8.GetBytes(dto.Password);
            byte[] hashedPassword = SHA256.HashData(passwordBytes);
            string hashedPasswordString = Convert.ToBase64String(hashedPassword); ;
            if(hashedPasswordString != user.Password)
            {
                response.Status = Enums.StatusEnum.AuthenticationError;
                response.Message = "Wrong email or password";
                return response;
            }

            if (user.Activated == 0)
            {
                response.Status = Enums.StatusEnum.AuthenticationError;
                response.Message = "Account not activated";
                return response;
            }

            Guid guid = Guid.NewGuid();
            Session session = new Session
            {
                Id = guid,
                UserId = user.Id,
                CreateDate = DateTime.Now
            };
            response.Status = Enums.StatusEnum.Ok;
            response.ResponseData.guid = guid;
            response.ResponseData.token = CreateToken(dto.Email);
            return response;
        }
        public string GetToken(string sessionString)
        {
            if (!Guid.TryParse(sessionString, out Guid guid))
            {
                return null;
            }

            using CrewDbContext db = dbFactory.CreateDbContext();

            Session session = db.Sessions.Include(s => s.User).FirstOrDefault(s => s.Id == guid);

            if (session == null)
            {
                return null;
            }
            return CreateToken(session.User.Email);
            return null;
        }
        private string CreateToken(string email)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        public ResponseModel<object> SendActivationMail(string email, string link)
        {
            using CrewDbContext db = dbFactory.CreateDbContext();
            ResponseModel<object> response = new ResponseModel<object>();
            User user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                response.Status= Enums.StatusEnum.NotFound;
                response.Message = "User with this email doesn't exist";
                return response;
            }
            else if (user.Activated == 1)
            {
                response.Status = Enums.StatusEnum.ResourceExist;
                response.Message = "User is already activated";
                return response;
            }
            Guid guid = new Guid();
            ActivateAccountRequest activeRequest = new ActivateAccountRequest
            {
                Id = guid,
                UserId = user.Id,
                ExpirationDate = DateTime.Now.AddHours(2)
            };
            db.ActivateAccountRequests.Add(activeRequest);
            db.SaveChanges();
            emailService.SendActivateMail(email, link);
            response.Status = Enums.StatusEnum.Ok;
            response.Message = "Email has been sent";
            return response;
        }
        
        public ResponseModel<object> ActiveAccount(Guid id)
        {
            using CrewDbContext db = dbFactory.CreateDbContext();
            var activateRequest = db.ActivateAccountRequests
                                    .Include(x=>x.User).FirstOrDefault(x => x.Id == id);
            ResponseModel<object> response = new ResponseModel<object>();
            if (activateRequest == null)
            {
                response.Status = Enums.StatusEnum.NotFound;
                return response;
            }
            else if (DateTime.Now > activateRequest.ExpirationDate)
            {
                response.Status = Enums.StatusEnum.Expired;
                response.Message = "Link is expired";
                return response;
            }
            activateRequest.User.Activated = 1;

            db.ActivateAccountRequests.Remove(activateRequest);
            db.SaveChanges();

            response.Status = Enums.StatusEnum.Ok;
            return response;
        }
    }
}
