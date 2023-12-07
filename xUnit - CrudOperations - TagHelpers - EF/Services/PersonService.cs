﻿using Entities;
using ServiceContracts;
using ServiceContracts.DTOs.PersonDto;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonService : IPersonService
    {
        //private readonly List<Person> _persons;
        private readonly PersonsDbContext _db;
        private readonly ICountriesService _countriesService;

        public PersonService(PersonsDbContext personsDbContext, ICountriesService countriesService)
        {
            _db = personsDbContext;
            _countriesService = countriesService;
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryId(person.CountryId)?.CountryName;
            return personResponse;
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

            //_persons.Add(person);
            _db.Persons.Add(person);
            _db.SaveChanges();

            //Convert fonksiyonuna al.
            //PersonResponse personResponse = person.ToPersonResponse();
            //personResponse.Country = _countriesService.GetCountryByCountryId(personResponse.CountryId)?.CountryName;

            PersonResponse personResponse = ConvertPersonToPersonResponse(person);

            return personResponse;
        }

        public bool DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            //Person? matchedPerson = _persons.FirstOrDefault(temp => temp.PersonId == personId);
            Person? matchedPerson = _db.Persons.FirstOrDefault(temp => temp.PersonId == personId);

            if (matchedPerson == null)
            {
                return false;
            }

            //_persons.Remove(matchedPerson);
            _db.Persons.Remove(matchedPerson);
            _db.SaveChanges();

            return true;
        }

        public List<PersonResponse> GetAllPerson()
        {
            //return _persons.Select(person => ConvertPersonToPersonResponse(person)).ToList();
            return _db.Persons.ToList().Select(person => ConvertPersonToPersonResponse(person)).ToList();
        }

        public List<PersonResponse> GetFilteredPersons(string? searchBy, string? searchString)
        {
            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return GetAllPerson();
            }

            List<PersonResponse> allPersons = GetAllPerson();
            List<PersonResponse> matchedPersons = allPersons;

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchedPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName) ? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchedPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchedPersons = allPersons.Where(temp => (temp.DateOfBirth != null) ? temp.DateOfBirth.Value.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    if (searchString == "male" || searchString == "Male")
                    {
                        matchedPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ? temp.Gender.ToLower() == searchString.ToLower() : true)).ToList();
                        break;
                    }
                    matchedPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ? temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.CountryId):
                    matchedPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Country) ? temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    matchedPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default:
                    matchedPersons = allPersons;
                    break;
            }

            return matchedPersons;
            /* İkinci Yol, Tam Oturmadı.
            Type type = typeof(Person);
            PropertyInfo searchByProperty = type.GetProperty(searchBy)!;

            if (searchByProperty.PropertyType == typeof(string))
            {
                return GetAllPerson().Where(temp => temp!.GetType()!.GetProperty(searchBy)!.GetValue(temp)!.ToString()!.ToLower() == searchString.ToLower()).ToList();
            }
            else
            {
                return GetAllPerson().Where(temp => Convert.ToInt32(temp!.GetType()!.GetProperty(searchBy)!.GetValue(this))! == Convert.ToInt32(searchString)).ToList();
            }
            */
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                return null;
            }

            //Person? person = _persons.FirstOrDefault(select => select.PersonId == personId);
            Person? person = _db.Persons.FirstOrDefault(select => select.PersonId == personId);

            if (person == null)
            {
                return null;
            }

            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOption sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOption.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), SortOrderOption.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOption.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOption.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOption.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOption.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOption.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),
                (nameof(PersonResponse.Age), SortOrderOption.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOption.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOption.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOption.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortOrderOption.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOption.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOption.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOption.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOption.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                _ => allPersons
            };

            return sortedPersons;

            /* İkinci Yol.
            if (string.IsNullOrEmpty(sortBy)) 
            { 
                return allPersons; 
            }

            PropertyInfo? sortByProperty = typeof(PersonResponse).GetProperty(sortBy);

            if (sortByProperty is null)
            {
                return allPersons;
            }

            IOrderedEnumerable<PersonResponse> sortedList;

            if (sortOrder == SortOrderOption.ASC)
            {
                sortedList = sortByProperty.PropertyType == typeof(string)
                    ? allPersons.OrderBy(person => (string?)sortByProperty.GetValue(person), StringComparer.OrdinalIgnoreCase)
                    : allPersons.OrderBy(person => sortByProperty.GetValue(person));
            }
            else
            {
                sortedList = sortByProperty.PropertyType == typeof(string)
                    ? allPersons.OrderByDescending(person => (string?)sortByProperty.GetValue(person), StringComparer.OrdinalIgnoreCase)
                    : allPersons.OrderByDescending(person => sortByProperty.GetValue(person));
            }

            return sortedList.ToList();
            */
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            ValidationHelper.ModelValidation(personUpdateRequest);

            //PersonResponse? personResponseFromGet = GetPersonByPersonId(personUpdateRequest.PersonId);
            //Person? matchedPerson = _persons.FirstOrDefault(temp => temp.PersonId  == personUpdateRequest.PersonId);
            Person? matchedPerson = _db.Persons.FirstOrDefault(temp => temp.PersonId  == personUpdateRequest.PersonId);
            if (matchedPerson == null)
            {
                throw new ArgumentException("Given person id doesn't exist!");
            }

            matchedPerson.PersonName = personUpdateRequest.PersonName;
            matchedPerson.Email = personUpdateRequest.Email;
            matchedPerson.Address = personUpdateRequest.Address;
            matchedPerson.Gender = personUpdateRequest.Gender.ToString();
            matchedPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchedPerson.CountryId = personUpdateRequest.CountryId;
            matchedPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            _db.SaveChanges();

            return ConvertPersonToPersonResponse(matchedPerson);
        }
    }
}
