using CrewBackend.Consts;
using CrewBackend.Entities;
using CrewBackend.Interfaces;
using CrewBackend.Models;
using CrewBackend.Models.Dto;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public ResponseModel<bool> SaveUserData(long userId, SaveUserDataDto dto)
        {
            return null;
        }
        public ResponseModel<byte[]> GetProfilePicture(long userId)
        {
            using CrewDbContext db = dbFactory.CreateDbContext();
            ResponseModel<byte[]> response = new ResponseModel<byte[]>();
            string? pictureName = db.Users.FirstOrDefault(u => u.Id == userId)?.Picture;
            if(pictureName == null)
            {
                response.Status = Enums.StatusEnum.NotFound;
                response.Message = "Picture not found";
                return response;
            }
            string path = Directories.ProfilePictures + $"\\{userId}\\{pictureName}.jpg";
            response.ResponseData = File.ReadAllBytes(path);
            response.Status = Enums.StatusEnum.Ok;
            return response;
        }
        public ResponseModel<object> SaveUserData(SaveUserDataDto dto, long userId)
        {
            ResponseModel<object> response = new ResponseModel<object>();

            using CrewDbContext db = dbFactory.CreateDbContext();

            User? user = db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                response.Status = Enums.StatusEnum.NotFound;
                response.Message = "User not found";
                return response;
            }

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Callname = dto.Callname;

            db.SaveChanges();
            response.Status = Enums.StatusEnum.Ok;
            return response;
        
        }

        public bool SaveUserProfilePicture(long userId, byte[] pictureBytes)
        {
            try
            {
                string path = Directory.GetParent(Directory.GetCurrentDirectory()) + $"\\Data\\ProfilePhotos\\{userId}";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                Guid guid = Guid.NewGuid();
                using FileStream fs = new FileStream(path + $"\\{guid}.jpg", FileMode.Create);
                fs.Write(pictureBytes);
                fs.Close();

                using CrewDbContext db = dbFactory.CreateDbContext();

                db.Users.FirstOrDefault(u => u.Id == userId).Picture = guid.ToString();
                db.SaveChanges();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
