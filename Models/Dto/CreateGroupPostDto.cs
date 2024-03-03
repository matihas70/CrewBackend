namespace CrewBackend.Models.Dto
{
    public record CreateGroupPostDto(
            string Title,
            string Body,
            string TaggedMembers
           
        );
}
