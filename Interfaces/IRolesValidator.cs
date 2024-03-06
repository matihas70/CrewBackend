namespace CrewBackend.Interfaces
{
    public interface IRolesValidator
    {
        bool IsAdmin(long userId, long groupId);
    }
}
