using CrewBackend.Data.Consts;
using CrewBackend.Data.Enums;
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

        public ResponseModel<GetUserDataDto> GetUserData(long id)
        {
            using CrewDbContext db = dbFactory.CreateDbContext();

            GetUserDataDto? userData = db.Users
                                       .Where(u => u.Id == id)
                                       .Select(u => new GetUserDataDto(
                                           u.Name,
                                           u.Surname,
                                           u.Email,
                                           u.Callname
                                           )).FirstOrDefault();
            ResponseModel<GetUserDataDto> resposne = new ResponseModel<GetUserDataDto>();

            if(userData == null)
            {
                resposne.Status = StatusEnum.NotFound;
                return resposne;
            }
            resposne.Status = StatusEnum.Ok;
            resposne.ResponseData = userData;
            return resposne;
        }
        public ResponseModel<List<GetUserEducationDto>> GetUserEducationData(long id)
        {
            ResponseModel<List<GetUserEducationDto>> response = new ResponseModel<List<GetUserEducationDto>>();
            using CrewDbContext db = dbFactory.CreateDbContext();
            var typesData = Data.Dictionaries.EducationTypes;
            List<GetUserEducationDto> data = db.Users.Include(u => u.UserEducations)
                                               .Where(u => u.Id == id)
                                               .SelectMany(x => x.UserEducations, (u, e) => new GetUserEducationDto(
                                                        e.Id,
                                                        e.SchoolName,
                                                        e.Type,
                                                        e.DateFrom,
                                                        e.DateTo,
                                                        e.Field,
                                                        e.Degree,
                                                        e.AdditionalInfo
                                                   )).ToList();
            if(data == null)
            {
                response.Status = StatusEnum.NotFound;
                return response;
            }
            response.ResponseData = data;
            response.Status = StatusEnum.Ok;
            return response;
        }
        public ResponseModel<IEnumerable<long>> SaveEducationData(List<SaveUserEducationDto> dto, long userId)
        {
            using CrewDbContext db = dbFactory.CreateDbContext();

            

            ResponseModel<IEnumerable<long>> response = new ResponseModel<IEnumerable<long>>();
            
            if (!db.Users.Any(u => u.Id == userId))
            {
                response.Status = StatusEnum.NotFound;
                return response;
            }

            List<UserEducation> newData = dto.Where(x => x.Id == 0)
                                             .Select(x => new UserEducation
                                             {
                                                 SchoolName = x.SchoolName,
                                                 Type = x.SchoolType,
                                                 DateFrom = x.DateFrom,
                                                 DateTo = x.DateTo,
                                                 Field = x.Field,
                                                 Degree = x.Degree,
                                                 AdditionalInfo = x.AdditionalInfo
                                             }).ToList();


            foreach(var item in dto.Where(x => x.Id != 0).ToList())
            {
                db.UserEducations.Where(x => x.Id == item.Id)
                                 .ExecuteUpdate(x =>
                                     x.SetProperty(p => p.SchoolName, item.SchoolName)
                                      .SetProperty(p => p.Type, item.SchoolType)
                                      .SetProperty(p => p.DateFrom, item.DateFrom)
                                      .SetProperty(p => p.DateTo, item.DateTo)
                                      .SetProperty(p => p.Field, item.Field)
                                      .SetProperty(p => p.Degree, item.Degree)
                                      .SetProperty(p => p.AdditionalInfo, item.AdditionalInfo)
                                 );
            }
            db.UserEducations.AddRange(newData);
            db.SaveChanges();
            response.Status = StatusEnum.Ok;
            response.ResponseData = newData.Select(x => x.Id);
            return response;
        }
        public ResponseModel<byte[]> GetProfilePicture(long userId)
        {
            using CrewDbContext db = dbFactory.CreateDbContext();
            ResponseModel<byte[]> response = new ResponseModel<byte[]>();
            string? pictureName = db.Users.FirstOrDefault(u => u.Id == userId)?.Picture;
            if(pictureName == null)
            {
                response.Status = StatusEnum.NotFound;
                response.Message = "Picture not found";
                return response;
            }
            string path = Directories.ProfilePictures + $"\\{userId}\\{pictureName}.jpg";
            response.ResponseData = File.ReadAllBytes(path);
            response.Status = StatusEnum.Ok;
            return response;
        }
        public ResponseModel<object> SaveUserData(SaveUserDataDto dto, long userId)
        {
            ResponseModel<object> response = new ResponseModel<object>();

            using CrewDbContext db = dbFactory.CreateDbContext();

            User? user = db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                response.Status = StatusEnum.NotFound;
                response.Message = "User not found";
                return response;
            }

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Callname = dto.Callname;

            db.SaveChanges();
            response.Status = StatusEnum.Ok;
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
