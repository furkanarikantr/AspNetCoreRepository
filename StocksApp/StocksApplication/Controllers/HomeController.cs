using Microsoft.AspNetCore.Mvc;

namespace StocksApplication.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
