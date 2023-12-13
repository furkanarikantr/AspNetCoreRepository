using CsvHelper;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTOs.PersonDto;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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

        //Artık gerek kalmadı, çünkü kendimiz eklediğimiz CountryName'i ilişkisel veritabanı kullanarak ekleyebiliyoruz.
        //private PersonResponse ConvertPersonToPersonResponse(Person person)
        //{
        //    PersonResponse personResponse = person.ToPersonResponse();
        //    //personResponse.Country = _countriesService.GetCountryByCountryId(person.CountryId)?.CountryName;
        //    personResponse.Country = person.Country?.CountryName;
        //    return personResponse;
        //}

        public async Task<PersonResponse?> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();

            //_persons.Add(person);

            //EntityFramework ile sql'siz ekleme.
            await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();

            //Entityframework ile storedprocedure kullanarak sql ile ekleme.
            //_db.sp_InsertPerson(person);

            //Convert fonksiyonuna al.
            //PersonResponse personResponse = person.ToPersonResponse();
            //personResponse.Country = _countriesService.GetCountryByCountryId(personResponse.CountryId)?.CountryName;

            //PersonResponse personResponse = ConvertPersonToPersonResponse(person);
            PersonResponse personResponse = person.ToPersonResponse();

            return personResponse;
        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            //Person? matchedPerson = _persons.FirstOrDefault(temp => temp.PersonId == personId);
            Person? matchedPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personId);

            if (matchedPerson == null)
            {
                return false;
            }

            //_persons.Remove(matchedPerson);
            _db.Persons.Remove(matchedPerson);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<List<PersonResponse>> GetAllPerson()
        {
            var persons = await _db.Persons.Include("Country").ToListAsync();

            //return _persons.Select(person => ConvertPersonToPersonResponse(person)).ToList();
            //return _db.Persons.ToList().Select(person => ConvertPersonToPersonResponse(person)).ToList();
            //return _db.sp_GetAllPersons().Select(temp => ConvertPersonToPersonResponse(temp)).ToList();

            //return persons.ToList().Select(person => ConvertPersonToPersonResponse(person)).ToList();
            return persons.ToList().Select(person => person.ToPersonResponse()).ToList();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = await GetAllPerson();
            List<PersonResponse> matchedPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchedPersons;
            }

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

        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                return null;
            }

            //Person? person = _persons.FirstOrDefault(select => select.PersonId == personId);
            Person? person = await _db.Persons.Include("Country").FirstOrDefaultAsync(select => select.PersonId == personId);

            if (person == null)
            {
                return null;
            }

            //return ConvertPersonToPersonResponse(person);
            return person.ToPersonResponse();
        }

        public async Task<MemoryStream> GetPersonsCSV(List<PersonResponse> persons)
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

            csvWriter.WriteHeader<PersonResponse>();    //PersonID,PersonName,...
            csvWriter.NextRecord();

            //List<PersonResponse> persons = _db.Persons
            //    .Include("Country")
            //    .Select(temp => temp.ToPersonResponse()).ToList();

            await csvWriter.WriteRecordsAsync(persons); //1, John, ...
            await streamWriter.FlushAsync();            //Yazma işlemi tamamlanıp, MemoryStream güncellenir.

            MemoryStream newMemoryStream = new MemoryStream(memoryStream.ToArray());    //Yeni MemoryStream'a eskisi kopyalanır.

            newMemoryStream.Position = 0;   //MemoryStream'ı ilk index'ten başlatıp return edilmesini sağlar.
            return newMemoryStream;
        }

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOption sortOrder)
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

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            ValidationHelper.ModelValidation(personUpdateRequest);

            //PersonResponse? personResponseFromGet = GetPersonByPersonId(personUpdateRequest.PersonId);
            //Person? matchedPerson = _persons.FirstOrDefault(temp => temp.PersonId  == personUpdateRequest.PersonId);
            Person? matchedPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personUpdateRequest.PersonId);
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
            await _db.SaveChangesAsync();

            //return ConvertPersonToPersonResponse(matchedPerson);
            return matchedPerson.ToPersonResponse();
        }
    }
}
