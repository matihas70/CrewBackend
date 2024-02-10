namespace CrewBackend.Models.Dto
{
    public record SaveUserEducationDto(
            long Id,
            string SchoolName,
            int SchoolType,
            DateOnly DateFrom,
            DateOnly? DateTo,
            string Field,
            string Degree,
            string? AdditionalInfo
        );
}
