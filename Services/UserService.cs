using CrewBackend.Entities;
using CrewBackend.Interfaces;
using CrewBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CrewBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IDbContextFactory<CrewDbContext> dbFactory;
        public UserService(IDbContextFactory<CrewDbContext> _dbFactory) =>
            dbFactory = _dbFactory;

        public ResponseModel<UserDataDto> GetUserData(long id)
        {
            using CrewDbContext db = dbFactory.CreateDbContext();

            UserDataDto? userData = db.Users.Where(u => u.Id == id)
                                       .Select(u => new UserDataDto(
                                           u.Name,
                                           u.Surname,
                                           u.Email,
                                           u.Callname
                                           )).FirstOrDefault();
            ResponseModel<UserDataDto> resposne = new ResponseModel<UserDataDto>();

            if(userData == null)
            {
                resposne.Status = Enums.StatusEnum.NotFound;
                return resposne;
            }
            resposne.Status = Enums.StatusEnum.Ok;
            resposne.ResponseData = userData;
            return resposne;
        }
    }
}
