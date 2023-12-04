using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTOs.CountryDto;
using ServiceContracts.DTOs.PersonDto;
using ServiceContracts.Enums;

namespace CrudExample.Controllers
{
    [Route("persons")]
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        public PersonsController(IPersonService personService, ICountriesService countriesService)
        {
            _personService = personService;
            _countriesService = countriesService;
        }

        [Route("/")]
        [Route("index")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy, SortOrderOption sortOrder = SortOrderOption.ASC)
        {
            //Search
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

            //Sort
            List<PersonResponse> sortedPersons = _personService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons);
        }

        [Route("createperson")]
        [HttpGet]
        public IActionResult CreatePerson()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries;

            return View();
        }

        [Route("createperson")]
        [HttpPost]
        public IActionResult CreatePerson(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;

                ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();
                return View();
            }

            _personService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Persons");
        }
    }
}
