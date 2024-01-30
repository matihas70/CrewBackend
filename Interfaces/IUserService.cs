using CrewBackend.Models;

namespace CrewBackend.Interfaces
{
    public interface IUserService
    {
        ResponseModel<UserDataDto> GetUserData(long id);
        bool SaveUserProfilePicture(long userId, byte[] pictureBytes);
    }
}
