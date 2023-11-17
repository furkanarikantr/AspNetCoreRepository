using Microsoft.AspNetCore.Mvc;
using ViewFirstPart.Models;

namespace ViewFirstPart.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("home")]
        public IActionResult Index()
        {
            List<Person> people = new List<Person>()
            {
                new Person()
                {
                    PersonName ="Furkan",
                    DateOfBirth = Convert.ToDateTime("1997-06-24"),
                    PersonGender = Gender.Male
                },
                new Person()
                {
                    PersonName ="Beyza",
                    DateOfBirth = Convert.ToDateTime("1999-03-04"),
                    PersonGender = Gender.Male
                }
            };

            //ViewData["appTitle"] = "Asp.Net Core Repo";
            //ViewData["people"] = people;

            //ViewBag.appTitle = "Asp.Net Core Repo";
            //ViewBag.people = people;

            return View(people); // - Views/Home/Index.cshtml dosyasını arayıp return eder.
            //return View("aViewPage"); // - Views/Home/aViewPage.cshtml dosyasını arayıp return eder.
            //return new ViewResult() { ViewName = "aViewPage" }; // - Views/Home/aViewPage.cshtml dosyasını arayıp return eder.
        }

        [Route("person-details/{name}")]
        public IActionResult Details(string? name)
        {
            if (name == null)
            {
                return Content("Person Name Can't be Null!");
            }

            List<Person> people = new List<Person>()
            {
                new Person()
                {
                    PersonName ="Furkan",
                    DateOfBirth = Convert.ToDateTime("1997-06-24"),
                    PersonGender = Gender.Male
                },
                new Person()
                {
                    PersonName ="Beyza",
                    DateOfBirth = Convert.ToDateTime("1999-03-04"),
                    PersonGender = Gender.Male
                }
            };

            Person matchingPerson = people.Where(temp => temp.PersonName == name).FirstOrDefault();

            return View(matchingPerson);
        }

        [Route("person-with-product")]
        public IActionResult PersonWithProduct()
        {
            Person person = new Person()
            {
                PersonName = "Furkan",
                PersonGender = Gender.Male,
                DateOfBirth = Convert.ToDateTime("1997-06-24")
            };

            Product product = new Product()
            {
                ProductId = 1,
                ProductName = "Maserati Ghibli"
            };

            PersonAndProductWrapperModel personAndProductWrapperModel = new PersonAndProductWrapperModel()
            {
                PersonData = person,
                ProductData = product
            };

            return View("PersonAndProduct", personAndProductWrapperModel);
        }
    }
}
