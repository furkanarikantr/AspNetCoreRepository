using Microsoft.AspNetCore.Mvc;

namespace ViewSecondPart.Controllers
{
    public class ProductsController : Controller
    {
        [Route("products")]
        public IActionResult Index()
        {
            return View();
        }

        //Url: /search-products
        //Url: /search-products/1
        [Route("search-products/{productId?}")]
        public IActionResult Search(int? productId)
        {
            ViewBag.ProductId = productId;
            return View();
        }

        [Route("order-product")]
        public IActionResult Order()
        {
            return View();
        }
    }
}
