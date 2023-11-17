using Microsoft.AspNetCore.Mvc;
using ViewFourthPartViewComponent.Models;

namespace ViewFourthPartViewComponent.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("friends-list")]
        public IActionResult LoadFriendList()
        {
            PersonGridModel personGridModelForIndex = new PersonGridModel()
            {
                GridTitle = "Index Persons",
                Persons = new List<Person>()
                {
                    new Person
                    {
                        PersonName = "Serkan",
                        JobTitle = "Soldier"
                    },
                    new Person
                    {
                        PersonName = "Kadriye",
                        JobTitle = "Nurse"
                    }
                }
            };

            return ViewComponent("Grid", personGridModelForIndex);
        }
    }
}
