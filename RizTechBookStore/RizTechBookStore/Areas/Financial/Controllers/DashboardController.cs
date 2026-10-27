using Microsoft.AspNetCore.Mvc;

namespace RizTechBookStore.Areas.Financial.Controllers
{
    [Area("financial")]
    [Route("financial/[controller]/[action]")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Graph()
        {
            return View();
        }
    }
}
