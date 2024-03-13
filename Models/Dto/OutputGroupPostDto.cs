namespace CrewBackend.Models.Dto
{
    public record OutputGroupPostDto(
          GroupPostAuthor Author,
          long Id,
          string Title,
          string Body,
          DateTime CreateDate
        );

    public record GroupPostAuthor(
            long Id,
            string Fullname,
            string Role
        );
}
