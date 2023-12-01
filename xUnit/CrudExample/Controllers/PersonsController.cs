using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTOs.PersonDto;

namespace CrudExample.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;

        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }

        [Route("/")]
        [Route("persons/index")]
        public IActionResult Index(string searchBy, string? searchString)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name"},
                {nameof(PersonResponse.Email), "Email"},
                {nameof(PersonResponse.DateOfBirth), "Date of Birth"},
                {nameof(PersonResponse.Gender), "Gender"},
                {nameof(PersonResponse.CountryId), "Country"},
                {nameof(PersonResponse.Address), "Address"},
            };

            List<PersonResponse> persons = _personService.GetFilteredPersons(searchBy, searchString);

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;


            return View(persons);
        }
    }
}
