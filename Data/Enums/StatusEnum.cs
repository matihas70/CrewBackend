namespace CrewBackend.Data.Enums
{
    public enum StatusEnum
    {
        Ok = 0,
        NotFound = 1,
        ValidationError = 2,
        ResourceExist = 3,
        AuthenticationError = 4,
        Expired = 5,
        AuthorizationError = 6,
        UnknownError = 1000,
    }
}
