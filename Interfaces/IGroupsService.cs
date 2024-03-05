using CrewBackend.Models;
using CrewBackend.Models.Dto;

namespace CrewBackend.Interfaces
{
    public interface IGroupsService
    {
        ResponseModel<object> CreateGroup(CreateGroupDto dto, long userId);
        ResponseModel<object> CreatePost(CreateGroupPostDto dto, long userId, long groupId);
        ResponseModel<object> AddUserToGroup(long groupId, long userId);
    }
}
