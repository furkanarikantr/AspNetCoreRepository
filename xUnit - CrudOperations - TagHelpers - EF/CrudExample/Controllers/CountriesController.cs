using Microsoft.AspNetCore.Mvc;

namespace CrudExample.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }
    }
}
