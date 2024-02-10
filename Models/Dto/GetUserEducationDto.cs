namespace CrewBackend.Models.Dto
{
    public record GetUserEducationDto(
            long id,
            string SchoolName,
            int SchoolType,
            DateOnly DateFrom,
            DateOnly? DateTo,
            string Field,
            string Degree,
            string? AdditionalInfo
        );
    
}
