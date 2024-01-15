namespace CrewBackend.Interfaces
{
    public interface IEmailService
    {
        void SendActivateMail(string to, string activationLink);
    }
}
