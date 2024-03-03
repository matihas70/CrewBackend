namespace CrewBackend.Interfaces
{
    public interface IEmailAccountService
    {
        void SendActivateMail(string to, string activationLink);
    }
}
