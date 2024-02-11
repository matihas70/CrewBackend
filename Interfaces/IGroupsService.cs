using CrewBackend.Models;
using CrewBackend.Models.Dto;

namespace CrewBackend.Interfaces
{
    public interface IGroupsService
    {
        ResponseModel<object> CreateGroup(CreateGroupDto dto, long userId);
    }
}
