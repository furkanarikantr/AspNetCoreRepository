using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTOs.CountryDto;
using ServiceContracts.DTOs.PersonDto;
using ServiceContracts.Enums;
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
        private readonly IFixture _fixture;

        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();

            var countriesInitialData = new List<Country>() { };
            var personsInitialData = new List<Person>() { };

            //Craete mock for DbContext
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
              new DbContextOptionsBuilder<ApplicationDbContext>().Options
             );

            //Access Mock DbContext object
            ApplicationDbContext dbContext = dbContextMock.Object;

            //Create mocks for DbSets'
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);

            //Create services based on mocked DbContext object
            //_countriesService = new CountriesService(dbContext);
            _countriesService = new CountriesService(null);
            _personService = new PersonService(null);

            _testOutputHelper = testOutputHelper;
        }  

        #region AddPerson
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    //Act
            //    await _personService.AddPerson(personAddRequest);
            //});
            Func<Task> action = async () =>
            {
                await _personService.AddPerson(personAddRequest);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddPerson_NullPersonName()
        {
            //Arrange
            //PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, null as string).Create();

            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    await _personService.AddPerson(personAddRequest);
            //});

            Func<Task> action = async () =>
            {
                //Act
                await _personService.AddPerson(personAddRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_ProperPersonDetail()
        {
            //Arrange
            //PersonAddRequest personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "example name",
            //    Email = "example@example.com",
            //    Address = "example address",
            //    CountryId = Guid.NewGuid(),
            //    Gender = ServiceContracts.Enums.GenderOption.Male,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    ReceiveNewsLetters = true
            //};

            //PersonAddRequest? personAddRequest = _fixture.Create<PersonAddRequest>();
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "example@example.com").Create();

            //Act
            PersonResponse personResponseFromAdd = await _personService.AddPerson(personAddRequest);
            List<PersonResponse> personsList = await _personService.GetAllPerson();

            //Assert
            //Assert.True(personResponseFromAdd.PersonId != Guid.Empty);
            personResponseFromAdd.Should().NotBe(Guid.Empty);
            //Assert.Contains(personResponseFromAdd, personsList);
            personsList.Should().Contain(personResponseFromAdd);
        }
        #endregion

        #region GetPersonByPersonId
        [Fact]
        public async Task GetPersonByPersonId_NullPersonId()
        {
            //Arrange
            Guid? personId = null;

            //Act 
            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonId(personId);

            //Assert
            //Assert.Null(personResponseFromGet);
            personResponseFromGet.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByPersonId_IsValidPersonId()
        {
            //CountryAddRequest countryAddRequest = new CountryAddRequest()
            //{
            //    CountryName = "Test"
            //};
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();

            CountryResponse ? countryResponse = await _countriesService.AddCountry(countryAddRequest);

            //PersonAddRequest personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "example name",
            //    Email = "example@example.com",
            //    Address = "example address",
            //    CountryId = countryResponse.CountryId,
            //    Gender = ServiceContracts.Enums.GenderOption.Male,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    ReceiveNewsLetters = true
            //};
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").Create();

            PersonResponse personResponseFromAdd = await _personService.AddPerson(personAddRequest);
            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonId(personResponseFromAdd.PersonId);

            //Assert
            Assert.Equal(personResponseFromAdd, personResponseFromGet);
            personResponseFromGet?.Should().Be(personResponseFromAdd);
        }
        #endregion

        #region GetAllPerson
        [Fact]
        public async Task GetAllPerson_EmptyList()
        {
            List<PersonResponse> personResponseListFromGet = await _personService.GetAllPerson();

            Assert.Empty(personResponseListFromGet);
            personResponseListFromGet.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllPerson_AddPersons()
        {
            CountryAddRequest countryAddRequest1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest countryAddRequest2 = _fixture.Create<CountryAddRequest>();

            CountryResponse? countryResponseFromAdd1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse? countryResponseFromAdd2 = await _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").Create();
            PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").Create();

            List<PersonAddRequest> personRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2 };

            List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personRequests)
            {
                PersonResponse personResponse = await _personService.AddPerson(personAddRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            //Expectec List
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            List<PersonResponse> personListFromGet = await _personService.GetAllPerson();

            //Actual List
            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponseFromGet in personListFromGet)
            {
                _testOutputHelper.WriteLine(personResponseFromGet.ToString());
            }

            //foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            //{
            //    Assert.Contains(personResponseFromAdd, personListFromGet);
               
            //}
            personListFromGet.Should().BeEquivalentTo(personResponseListFromAdd);
        }
        #endregion

        #region GetFilteredPersons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            CountryAddRequest countryAddRequest1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest countryAddRequest2 = _fixture.Create<CountryAddRequest>();

            CountryResponse? countryResponseGetTest1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse? countryResponseGetTest2 = await _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").Create();
            PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").Create();
            PersonAddRequest personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").Create();

            List<PersonAddRequest> personAddRequestGetTest = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };

            List<PersonResponse> personResponseListGetTest = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personAddRequestGetTest)
            {
                PersonResponse personResponseGetTest = await _personService.AddPerson(personAddRequest);
                personResponseListGetTest.Add(personResponseGetTest);
            }

            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseGetTest in personResponseListGetTest)
            {
                _testOutputHelper.WriteLine(personResponseGetTest.ToString());
            }

            List<PersonResponse> personListGet = await _personService.GetFilteredPersons(nameof(Person.PersonName),"");

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personGet in personListGet)
            {
                _testOutputHelper.WriteLine(personGet.ToString());
            }

            //foreach (PersonResponse personResponse in personListGet)
            //{
            //    Assert.Contains(personResponse, personResponseListGetTest);
            //}
            personResponseListGetTest.Should().BeEquivalentTo(personListGet);
        }

        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            CountryAddRequest countryAddRequest1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest countryAddRequest2 = _fixture.Create<CountryAddRequest>();

            CountryResponse? countryResponseGetTest1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse? countryResponseGetTest2 = await _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName,"Furkan").With(temp => temp.Email, "email@sample.com").Create();
            PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").Create();
            PersonAddRequest personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").Create();

            List<PersonAddRequest> personAddRequestGetTest = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };

            List<PersonResponse> personResponseListGetTest = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personAddRequestGetTest)
            {
                PersonResponse personResponseGetTest = await _personService.AddPerson(personAddRequest);
                personResponseListGetTest.Add(personResponseGetTest);
            }

            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseGetTest in personResponseListGetTest)
            {
                _testOutputHelper.WriteLine(personResponseGetTest.ToString());
            }

            List<PersonResponse> personListGet = await _personService.GetFilteredPersons(nameof(Person.PersonName), "an");

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personGet in personListGet)
            {
                _testOutputHelper.WriteLine(personGet.ToString());
            }

            //foreach (PersonResponse personResponse in personListGet)
            //{
            //    if (personResponse.PersonName != null)
            //    {
            //        if (personResponse.PersonName.Contains("an", StringComparison.OrdinalIgnoreCase))
            //        {
            //            Assert.Contains(personResponse, personResponseListGetTest);
            //        }
            //    }
            //}
            personListGet.Should().OnlyContain(temp => temp.PersonName.Contains("an", StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region GetSortedPersons
        [Fact]
        public async Task GetSorterPersons_() 
        {
            CountryAddRequest countryAddRequest1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest countryAddRequest2 = _fixture.Create<CountryAddRequest>();

            CountryResponse? countryResponseGetTest1 = await _countriesService.AddCountry(countryAddRequest1);
            CountryResponse? countryResponseGetTest2 = await _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").Create();
            PersonAddRequest personAddRequest2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").Create();
            PersonAddRequest personAddRequest3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").Create();

            List<PersonAddRequest> personRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };

            List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personRequests)
            {
                PersonResponse personResponseGetTest = await _personService.AddPerson(personAddRequest);
                personResponseListFromAdd.Add(personResponseGetTest);
            }

            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseGetTest in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseGetTest.ToString());
            }

            List<PersonResponse> allPersons = await _personService.GetAllPerson();
            List<PersonResponse> personsListFromSort = await _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOption.DESC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personGet in personsListFromSort)
            {
                _testOutputHelper.WriteLine(personGet.ToString());
            }

            //personResponseListFromAdd = personResponseListFromAdd.OrderByDescending(temp => temp.PersonName).ToList();
            //for (int i = 0; i < personResponseListFromAdd.Count; i++)
            //{
            //    Assert.Equal(personResponseListFromAdd[i], personsListFromSort[i]);
            //}
            //personsListFromSort.Should().BeEquivalentTo(personResponseListFromAdd);

            personsListFromSort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }
        #endregion

        #region UpdatePerson
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            //Assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    //Act
            //    await _personService.UpdatePerson(personUpdateRequest);
            //});
            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonId()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = _fixture.Create<PersonUpdateRequest>();

            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    await _personService.UpdatePerson(personUpdateRequest);
            //});

            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_NullPersonName() 
        {
            //Arrange
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse? countryResponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").With(temp => temp.CountryId, countryResponse.CountryId).Create();

            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    await _personService.UpdatePerson(personUpdateRequest);
            //});
            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            };
            action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdatePerson_PersonFullDetails()
        {
            //Arrange
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse? countryResponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").With(temp => temp.CountryId, countryResponse.CountryId).Create();
            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "Beyza";
            personUpdateRequest.Email = "beyza@example.com";

            PersonResponse personResponseFromUpdate = await _personService.UpdatePerson(personUpdateRequest);
            PersonResponse? personResponseFromGet = await _personService.GetPersonByPersonId(personResponseFromUpdate.PersonId);

            //Assert.Equal(personResponseFromGet, personResponseFromUpdate);
            personResponseFromUpdate.Should().Be(personResponseFromGet);
        }
        #endregion

        #region DeletePerson
        [Fact]
        public async Task DeletePerson_ValidPersonId()
        {
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse? countryResponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").With(temp => temp.CountryId, countryResponse.CountryId).Create();
            PersonResponse personResponseFromAdd = await _personService.AddPerson(personAddRequest);

            bool isDeleted = await _personService.DeletePerson(personResponseFromAdd.PersonId);

            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeletePerson_InvalidPersonId()
        {
            CountryAddRequest countryAddRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse? countryResponse = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@sample.com").With(temp => temp.CountryId, countryResponse.CountryId).Create();
            PersonResponse personResponseFromAdd = await _personService.AddPerson(personAddRequest);

            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

            //Assert.False(isDeleted);
            isDeleted.Should().BeFalse();
        }
        #endregion
    }
}
