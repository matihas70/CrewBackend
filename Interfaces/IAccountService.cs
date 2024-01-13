using CrewBackend.Models;

namespace CrewBackend.Interfaces
{
    public interface IAccountService
    {
        bool Register(RegisterUserDto dto, string host);
        bool Login();
        bool ActiveAccount(Guid id);

    }
}
