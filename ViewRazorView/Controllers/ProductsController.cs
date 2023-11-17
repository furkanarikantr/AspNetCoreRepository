using Microsoft.AspNetCore.Mvc;

namespace ViewFirstPart.Controllers
{
    public class ProductsController : Controller
    {
        [Route("products-all")]
        public IActionResult All()
        {
            return View();
        }
    }
}
