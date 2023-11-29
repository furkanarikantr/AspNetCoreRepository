using Entities;
using ServiceContracts;
using ServiceContracts.DTOs.PersonDto;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();

            _persons.Add(person);

            //Convert fonksiyonuna al.
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryId(personResponse.CountryId)?.CountryName;

            return personResponse;
        }

        public List<PersonResponse> GetAllPerson()
        {
            return _persons.Select(person => person.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                return null;
            }

            Person? person = _persons.FirstOrDefault(select => select.PersonId == personId);

            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }
    }
}
