namespace CrewBackend.Interfaces
{
    public interface IEmailService
    {
        bool SendActivateMail(string to, string activationLink);
    }
}
