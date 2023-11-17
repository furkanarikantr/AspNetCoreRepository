using Microsoft.AspNetCore.Mvc;
using ModelExample.Models;

namespace ModelExample.Controllers
{
    public class OrderController : Controller
    {
        [Route("/order")]
        public IActionResult Index([Bind(nameof(Order.OrderDate), nameof(Order.InvoicePrice), nameof(Order.OrderNo), nameof(Order.Products))] Order order)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("\n", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return BadRequest(messages);
            }

            Random random = new Random();
            int randomOrderNumber = random.Next(1, 99999);

            //return HTTP 200 response with order number
            return Json(new { orderNumber = randomOrderNumber });
        }
    }
}
