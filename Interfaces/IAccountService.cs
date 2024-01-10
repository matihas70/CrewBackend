using CrewBackend.Models;

namespace CrewBackend.Interfaces
{
    public interface IAccountService
    {
        bool Register(RegisterUserDto dto);
        bool Login();

    }
}
