using Entities;
using ServiceContracts;
using ServiceContracts.DTOs.CountryDto;
using ServiceContracts.DTOs.PersonDto;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace CrudTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson
        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public void AddPerson_NullPersonName()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public void AddPerson_ProperPersonDetail()
        {
            //Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "example name",
                Email = "example@example.com",
                Address = "example address",
                CountryId = Guid.NewGuid(),
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            //Act
            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);
            List<PersonResponse> personsList = _personService.GetAllPerson();

            //Assert
            Assert.True(personResponseFromAdd.PersonId != Guid.Empty);
            Assert.Contains(personResponseFromAdd, personsList);
        }
        #endregion

        #region GetPersonByPersonId
        [Fact]
        public void GetPersonByPersonId_NullPersonId()
        {
            //Arrange
            Guid? personId = null;

            //Act 
            PersonResponse? personResponseGetTest = _personService.GetPersonByPersonId(personId);

            //Assert
            Assert.Null(personResponseGetTest);
        }

        [Fact]
        public void GetPersonByPersonId_IsValidPersonId()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Test"
            };

            CountryResponse countryResponseGetTest = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "example name",
                Email = "example@example.com",
                Address = "example address",
                CountryId = countryResponseGetTest.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            PersonResponse? personResponseGetTest = _personService.GetPersonByPersonId(personResponse.PersonId);

            //Assert
            Assert.Equal(personResponse, personResponseGetTest);
        }
        #endregion

        #region GetAllPerson
        [Fact]
        public void GetAllPerson_EmptyList()
        {
            List<PersonResponse> personResponseListGetTest = _personService.GetAllPerson();

            Assert.Empty(personResponseListGetTest);
        }

        [Fact]
        public void GetAllPerson_AddPersons()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "Test1"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "Test2"
            };

            CountryResponse countryResponseGetTest1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponseGetTest2 = _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "example name",
                Email = "example@example.com",
                Address = "example address",
                CountryId = countryResponseGetTest1.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "example name",
                Email = "example@example.com",
                Address = "example address",
                CountryId = countryResponseGetTest2.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> personAddRequestGetTest = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2 };

            List<PersonResponse> personResponseListGetTest = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personAddRequestGetTest)
            {
                PersonResponse personResponseGetTest = _personService.AddPerson(personAddRequest);
                personResponseListGetTest.Add(personResponseGetTest);
            }

            //Expectec List
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseGetTest in personResponseListGetTest)
            {
                _testOutputHelper.WriteLine(personResponseGetTest.ToString());
            }

            List<PersonResponse> personListGet = _personService.GetAllPerson();

            //Actual List
            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personGet in personListGet)
            {
                _testOutputHelper.WriteLine(personGet.ToString());
            }

            foreach (PersonResponse personResponse in personResponseListGetTest)
            {
                Assert.Contains(personResponse, personListGet);
            }
        }
        #endregion
    }
}
