using Entities;
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
            PersonResponse? personResponseFromGet = _personService.GetPersonByPersonId(personId);

            //Assert
            Assert.Null(personResponseFromGet);
        }

        [Fact]
        public void GetPersonByPersonId_IsValidPersonId()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Test"
            };

            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "example name",
                Email = "example@example.com",
                Address = "example address",
                CountryId = countryResponse.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);
            PersonResponse? personResponseFromGet = _personService.GetPersonByPersonId(personResponseFromAdd.PersonId);

            //Assert
            Assert.Equal(personResponseFromAdd, personResponseFromGet);
        }
        #endregion

        #region GetAllPerson
        [Fact]
        public void GetAllPerson_EmptyList()
        {
            List<PersonResponse> personResponseListFromGet = _personService.GetAllPerson();

            Assert.Empty(personResponseListFromGet);
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

            CountryResponse countryResponseFromAdd1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponseFromAdd2 = _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "example name",
                Email = "example@example.com",
                Address = "example address",
                CountryId = countryResponseFromAdd1.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "example name",
                Email = "example@example.com",
                Address = "example address",
                CountryId = countryResponseFromAdd2.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> personRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2 };

            List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personRequests)
            {
                PersonResponse personResponse = _personService.AddPerson(personAddRequest);
                personResponseListFromAdd.Add(personResponse);
            }

            //Expectec List
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseFromAdd.ToString());
            }

            List<PersonResponse> personListFromGet = _personService.GetAllPerson();

            //Actual List
            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personResponseFromGet in personListFromGet)
            {
                _testOutputHelper.WriteLine(personResponseFromGet.ToString());
            }

            foreach (PersonResponse personResponseFromAdd in personResponseListFromAdd)
            {
                Assert.Contains(personResponseFromAdd, personListFromGet);
            }
        }
        #endregion

        #region GetFilteredPersons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "UK"
            };

            CountryResponse countryResponseGetTest1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponseGetTest2 = _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Furkan",
                Email = "furkan@example.com",
                Address = "address of Furkan",
                CountryId = countryResponseGetTest1.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Serkan",
                Email = "serkan@example.com",
                Address = "address of Serkan",
                CountryId = countryResponseGetTest1.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Beyza",
                Email = "beyza@example.com",
                Address = "address of Beyza",
                CountryId = countryResponseGetTest1.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> personAddRequestGetTest = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };

            List<PersonResponse> personResponseListGetTest = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personAddRequestGetTest)
            {
                PersonResponse personResponseGetTest = _personService.AddPerson(personAddRequest);
                personResponseListGetTest.Add(personResponseGetTest);
            }

            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseGetTest in personResponseListGetTest)
            {
                _testOutputHelper.WriteLine(personResponseGetTest.ToString());
            }

            List<PersonResponse> personListGet = _personService.GetFilteredPersons(nameof(Person.PersonName),"");

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personGet in personListGet)
            {
                _testOutputHelper.WriteLine(personGet.ToString());
            }

            foreach (PersonResponse personResponse in personListGet)
            {
                Assert.Contains(personResponse, personResponseListGetTest);
            }
        }

        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "UK"
            };

            CountryResponse countryResponseGetTest1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponseGetTest2 = _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "FurkaN",
                Email = "furkan@example.com",
                Address = "address of Furkan",
                CountryId = countryResponseGetTest1.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Serkan",
                Email = "serkan@example.com",
                Address = "address of Serkan",
                CountryId = countryResponseGetTest2.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Beyza",
                Email = "beyza@example.com",
                Address = "address of Beyza",
                CountryId = countryResponseGetTest1.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> personAddRequestGetTest = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };

            List<PersonResponse> personResponseListGetTest = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personAddRequestGetTest)
            {
                PersonResponse personResponseGetTest = _personService.AddPerson(personAddRequest);
                personResponseListGetTest.Add(personResponseGetTest);
            }

            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseGetTest in personResponseListGetTest)
            {
                _testOutputHelper.WriteLine(personResponseGetTest.ToString());
            }

            List<PersonResponse> personListGet = _personService.GetFilteredPersons(nameof(Person.PersonName), "an");

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personGet in personListGet)
            {
                _testOutputHelper.WriteLine(personGet.ToString());
            }

            foreach (PersonResponse personResponse in personListGet)
            {
                if (personResponse.PersonName != null)
                {
                    if (personResponse.PersonName.Contains("an", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(personResponse, personResponseListGetTest);
                    }
                }
            }
        }
        #endregion

        #region GetSortedPersons
        [Fact]
        public void GetSorterPersons_() 
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "UK"
            };

            CountryResponse countryResponseGetTest1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponseGetTest2 = _countriesService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "FurkaN",
                Email = "furkan@example.com",
                Address = "address of Furkan",
                CountryId = countryResponseGetTest1.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Serkan",
                Email = "serkan@example.com",
                Address = "address of Serkan",
                CountryId = countryResponseGetTest2.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Beyza",
                Email = "beyza@example.com",
                Address = "address of Beyza",
                CountryId = countryResponseGetTest1.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Female,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> personRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2, personAddRequest3 };

            List<PersonResponse> personResponseListFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personRequests)
            {
                PersonResponse personResponseGetTest = _personService.AddPerson(personAddRequest);
                personResponseListFromAdd.Add(personResponseGetTest);
            }

            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseGetTest in personResponseListFromAdd)
            {
                _testOutputHelper.WriteLine(personResponseGetTest.ToString());
            }

            List<PersonResponse> allPersons = _personService.GetAllPerson();
            List<PersonResponse> personsListFromSort = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOption.DESC);

            _testOutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personGet in personsListFromSort)
            {
                _testOutputHelper.WriteLine(personGet.ToString());
            }

            personResponseListFromAdd = personResponseListFromAdd.OrderByDescending(temp => temp.PersonName).ToList();

            for (int i = 0; i < personResponseListFromAdd.Count; i++)
            {
                Assert.Equal(personResponseListFromAdd[i], personsListFromSort[i]);
            }
        }
        #endregion

        #region UpdatePerson
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() 
            {
                PersonId = Guid.NewGuid()
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_NullPersonName() 
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Furkan",
                Email = "furkan@example.com",
                Address = "address of Furkan",
                CountryId = countryResponse.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_PersonFullDetails()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Furkan",
                Email = "furkan@example.com",
                Address = "address of Furkan",
                CountryId = countryResponse.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "Beyza";
            personUpdateRequest.Email = "beyza@example.com";

            PersonResponse personResponseFromUpdate = _personService.UpdatePerson(personUpdateRequest);
            PersonResponse? personResponseFromGet = _personService.GetPersonByPersonId(personResponseFromUpdate.PersonId);

            Assert.Equal(personResponseFromGet, personResponseFromUpdate);
        }
        #endregion

        #region DeletePerson
        [Fact]
        public void DeletePerson_ValidPersonId()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Furkan",
                Email = "furkan@example.com",
                Address = "address of Furkan",
                CountryId = countryResponse.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);

            bool isDeleted = _personService.DeletePerson(personResponseFromAdd.PersonId);

            Assert.True(isDeleted);
        }

        [Fact]
        public void DeletePerson_InvalidPersonId()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Furkan",
                Email = "furkan@example.com",
                Address = "address of Furkan",
                CountryId = countryResponse.CountryId,
                Gender = ServiceContracts.Enums.GenderOption.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonResponse personResponseFromAdd = _personService.AddPerson(personAddRequest);

            bool isDeleted = _personService.DeletePerson(Guid.NewGuid());

            Assert.False(isDeleted);
        }
        #endregion
    }
}
