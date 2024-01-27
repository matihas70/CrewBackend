using CrewBackend.Models;

namespace CrewBackend.Interfaces
{
    public interface IUserService
    {
        ResponseModel<UserDataDto> GetUserData(long id);
    }
}
