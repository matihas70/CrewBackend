using CrewBackend.Models;
using CrewBackend.Models.Dto;

namespace CrewBackend.Interfaces
{
    public interface IUserService
    {
        ResponseModel<UserDataDto> GetUserData(long id);
        bool SaveUserProfilePicture(long userId, byte[] pictureBytes);
        ResponseModel<byte[]> GetProfilePicture(long userId);
        ResponseModel<object> SaveUserData(SaveUserDataDto dto, long userId);
    }
}
