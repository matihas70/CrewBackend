using Microsoft.AspNetCore.Mvc;

namespace CrewBackend.Controllers
{
    public class GroupsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
