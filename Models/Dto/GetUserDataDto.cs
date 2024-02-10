namespace CrewBackend.Models
{
    public record GetUserDataDto(
        string Name,
        string Surname,
        string Email,
        string Callname);
    
}
