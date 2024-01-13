using CrewBackend.Entities;
using CrewBackend.Models;
using Microsoft.EntityFrameworkCore.Internal;
using CrewBackend.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CrewBackend.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDbContextFactory<CrewDbContext> dbFactory;
        private readonly IEmailService emailService;
        public AccountService(IDbContextFactory<CrewDbContext> _dbFactory, IEmailService _emailService)
            => (dbFactory, emailService) = (_dbFactory, _emailService);

        
        public bool Register(RegisterUserDto dto, string host)
        {
            using var db = dbFactory.CreateDbContext();

            if (db.Users.Any(u => u.Email == dto.Email))
                return false;

            byte[] bytes = Encoding.UTF8.GetBytes(dto.Password);
            byte[] hashedBytes = SHA256.HashData(bytes);

            User user = new User
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                Password = Encoding.UTF8.GetString(hashedBytes),
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
            emailService.SendActivateMail(dto.Email, host + $"/User/activate/{guid}");
            db.SaveChanges();

            return true;
        }
        public bool Login()
        {
            return true;
        }
        public bool ActiveAccount(Guid id)
        {
            using CrewDbContext db = dbFactory.CreateDbContext();
            var activateRequest = db.ActivateAccountRequests
                                    .Include(x=>x.User).FirstOrDefault(x => x.Id == id);
            if (activateRequest == null && DateTime.Now > activateRequest.ExpirationDate)
            {
                return false;
            }
            activateRequest.User.Activated = 1;
            db.SaveChanges();
            return true;
        }
    }
}
