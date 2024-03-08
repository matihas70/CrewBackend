namespace CrewBackend.Models.Dto
{
    public record CreateGroupPostDto(
            string Title,
            string Body,
            List<long> TaggedMembers
           
        );
}
