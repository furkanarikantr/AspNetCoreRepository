using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTOs.CountryDto;
using ServiceContracts.DTOs.PersonDto;
using ServiceContracts.Enums;
using Services;

namespace CrudExample.Controllers
{
    //[Route("persons")]
    [Route("[controller]")]
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
        [Route("[action]")]
        [Route("/index")]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy, SortOrderOption sortOrder = SortOrderOption.ASC)
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

            List<PersonResponse> persons = await _personService.GetFilteredPersons(searchBy, searchString);

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<PersonResponse> sortedPersons = await _personService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons);
        }

        //[Route("createperson")]
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> CreatePerson()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();

            //new SelectListItem()
            //{ Text = "Furkan", Value = "1" };

            ViewBag.Countries = countries.Select(temp => new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryId.ToString()
            });

            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> CreatePerson(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(temp => new SelectListItem()
                {
                    Text = temp.CountryName,
                    Value = temp.CountryId.ToString()
                });

                ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage).ToList();
                return View();
            }

            await _personService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Persons");
        }

        //[Route("editperson/{personid}")]
        [Route("[action]/{personid}")]
        [HttpGet]
        public async Task<IActionResult> EditPerson(Guid personId)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonId(personId);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() });

            return View(personUpdateRequest);
        }

        [Route("[action]/{personid}")]
        [HttpPost]
        public async Task<IActionResult> EditPerson(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonId(personUpdateRequest.PersonId);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                PersonResponse updatedPerson = await _personService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(temp =>
                new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() });

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
        }

        //[Route("deleteperson/{personid}")]
        [Route("[action]/{personid}")]
        [HttpGet]
        public async Task<IActionResult> DeletePerson(Guid personId)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonId(personId);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponse);
        }

        [Route("[action]/{personid}")]
        [HttpPost]
        public async Task<IActionResult> DeletePerson(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonId(personUpdateRequest.PersonId);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            await _personService.DeletePerson(personResponse.PersonId);
            return RedirectToAction("Index");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF(string searchBy, string searchString, string sortBy, SortOrderOption sortOrder = SortOrderOption.ASC)
        {
            //List of Persons
            //List<PersonResponse> persons = await _personService.GetAllPerson();

            List<PersonResponse> filteredPersons = await _personService.GetFilteredPersons(searchBy, searchString);
            List<PersonResponse> sortedPersons = await _personService.GetSortedPersons(filteredPersons, sortBy, sortOrder);

            //Return to view as pdf
            return new ViewAsPdf(sortedPersons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV(string searchBy, string searchString, string sortBy, SortOrderOption sortOrder = SortOrderOption.ASC)
        {
            //MemoryStream memoryStream = await _personService.GetPersonsCSV();
            List<PersonResponse> filteredPersons = await _personService.GetFilteredPersons(searchBy, searchString);
            Console.WriteLine("Filtered Persons Count: " + filteredPersons.Count);
            List<PersonResponse> sortedPersons = await _personService.GetSortedPersons(filteredPersons, sortBy, sortOrder);
            Console.WriteLine("Sorted Persons Count: " + sortedPersons.Count);
            MemoryStream memoryStream = await _personService.GetPersonsCSV(sortedPersons);

            return File(memoryStream, "application/octet-stream", "persons.csv");
        }
    }
}
