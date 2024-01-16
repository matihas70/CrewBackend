using CrewBackend.Models;

namespace CrewBackend.Interfaces
{
    public interface IAccountService
    {
        ResponseModel<object> Register(RegisterUserDto dto, string link);
        ResponseModel<Guid> Login(LoginUserDto dto);
        ResponseModel<object> SendActivationMail(string email, string link);
        ResponseModel<object> ActiveAccount(Guid id);

    }
}
