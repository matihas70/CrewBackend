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
        public AccountService(IDbContextFactory<CrewDbContext> _dbFactory)
            => dbFactory = _dbFactory;

        
        public bool Register(RegisterUserDto dto)
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
                Password = Encoding.ASCII.GetString(hashedBytes),
            };
            db.Users.Add(user);
            db.SaveChanges();
            return true;
        }
        public bool Login()
        {
            return true;
        }
    }
}
