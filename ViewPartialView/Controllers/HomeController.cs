using Microsoft.AspNetCore.Mvc;
using ViewThirdPartPartial.Models;

namespace ViewThirdPartPartial.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            //ViewBag.ListTitle = "Cities";
            //ViewBag.ListItems = new List<string>()
            //{
            //    "London",
            //    "Berlin",
            //    "Stockholm",
            //    "Paris",
            //    "Amsterdam",
            //    "Milano",
            //    "Barcelona",
            //    "Madrid",
            //    "Vienne",
            //    "Roma",
            //    "Napoli",
            //};

            return View();
        }

        [Route("about")]
        public IActionResult About() 
        {
            return View();
        }

        [Route("programming-languages")]
        public IActionResult ProgrammingLanguages()
        {
            ListModel listModel = new ListModel()
            {
                ListTitle = "Programming Languages List",
                ListItems = new List<string>()
                {
                    "C#",
                    "Java",
                    "Javascript",
                    "Phyton"
                }
            };

            return PartialView("_CitiesListPartialView", listModel);
        }
    }
}
