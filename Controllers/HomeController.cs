using CrewBackend.Consts;
using CrewBackend.Interfaces;
using CrewBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrewBackend.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect(Urls.Front);
        }
    }
}
