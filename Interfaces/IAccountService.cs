using CrewBackend.Models;

namespace CrewBackend.Interfaces
{
    public interface IAccountService
    {
        ResponseModel<object> Register(RegisterUserDto dto, string link);
        ResponseModel<LoginOutput> Login(LoginUserDto dto);
        string GetToken(string sessionString);
        ResponseModel<object> SendActivationMail(string email, string link);
        ResponseModel<object> ActiveAccount(Guid id);
        bool Logout(string session);

    }
}
