using Microsoft.AspNetCore.Mvc;

namespace RaceDay.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Pass the shared event list to the home view
            return View(EventStore.Events);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}