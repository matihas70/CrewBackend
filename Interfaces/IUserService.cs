using CrewBackend.Models;
using CrewBackend.Models.Dto;

namespace CrewBackend.Interfaces
{
    public interface IUserService
    {
        ResponseModel<GetUserDataDto> GetUserData(long id);
        ResponseModel<List<GetUserEducationDto>> GetUserEducationData(long id);
        ResponseModel<IEnumerable<long>> SaveEducationData(List<SaveUserEducationDto> dto, long userId);
        bool SaveUserProfilePicture(long userId, byte[] pictureBytes);
        ResponseModel<byte[]> GetProfilePicture(long userId);
        ResponseModel<object> SaveUserData(SaveUserDataDto dto, long userId);
    }
}
