namespace CrewBackend.Interfaces
{
    public interface IRolesValidator
    {
        bool IsAdmin(long userId, long groupId);
        bool IsMember(long userId, long groupId);
    }
}
