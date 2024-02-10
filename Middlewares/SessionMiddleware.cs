using CrewBackend.Data.Consts;
using CrewBackend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CrewBackend.Middlewares
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IDbContextFactory<CrewDbContext> dbFactory;
        public SessionMiddleware(RequestDelegate _next, IDbContextFactory<CrewDbContext> _dbFactory)
            => (next, dbFactory) = (_next, _dbFactory);
        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value;
            bool isSession = await checkSession(context);

            if (path.StartsWith("/Account"))
            {
                if (isSession)
                {
                    context.Response.Redirect(Urls.Front.Home);
                    return;
                }
                    
            }
            else
            {
                if (!isSession)
                {
                    context.Response.Redirect(Urls.Front.Login);
                    return;
                }
            }


            next.Invoke(context);
        }
        private async Task<bool> checkSession(HttpContext context)
        {
            var sessionString = context.Request.Cookies["Session"];

            if (sessionString == null)
            {
                return false;
            }
            if (!Guid.TryParse(sessionString, out Guid guid))
            {
                return false;
            }

            using CrewDbContext db = await dbFactory.CreateDbContextAsync();

            Session session = db.Sessions.FirstOrDefault(s => s.Id == guid);

            if (session == null)
            {
                return false;
            }
            return true;
        }
    }
}
