using Azure.Core;
using CrewBackend.Interfaces;
using System.Security.Claims;

namespace CrewBackend.Services
{
    public class UserContextInfo:IUserContextInfo
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserContextInfo(IHttpContextAccessor _httpContextAccessor) =>
            httpContextAccessor = _httpContextAccessor;

        public long GetUserId()
        {
            var identity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            return Int64.Parse(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
