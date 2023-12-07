using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTOs.CountryDto;
using Services;

namespace CrudTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
        }

        #region AddCountry
        //CountriesService'imizdeki AddRequest'teki CountryAddRequest null olursa, hata fırlatmalı.
        [Fact]
        public void AddCountry_CountryIsNull()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //CountryName null olursa, hata fırlatmalı
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //CountryName tekrarlanırsa, hata fırlatmalı
        [Fact]
        public void AddCountry_CountryNameIsDuplicate()
        {
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        //CountryName uygunsa, eklenmeli.
        [Fact]
        public void AddCountry_CountryDetailsIsProper()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            //Act
            CountryResponse response = _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryId != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);
        }
        #endregion

        #region GetAllCountries
        //Default olarak countries listesi boş olmalı.
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            //Acts
            List<CountryResponse> actual_countries_list = _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actual_countries_list);
        }

        //
        [Fact]
        public void GetCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> countries_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest()
                {
                    CountryName = "USA"
                },
                new CountryAddRequest()
                {
                    CountryName = "UK"
                },
            };

            //Act
            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();
            foreach (CountryAddRequest country_request in countries_request_list)
            {
                countries_list_from_add_country.Add(_countriesService.AddCountry(country_request)); 
            }

            List<CountryResponse> actualCountriesResponseList = _countriesService.GetAllCountries();
            foreach(CountryResponse expected_country in countries_list_from_add_country)
            {
                Assert.Contains(expected_country, actualCountriesResponseList);
            }
        }
        #endregion

        #region GetCountryByCountryId
        [Fact]
        public void GetCountryByCountryId_CountryIdIsNull()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            CountryResponse country_response_from_get_method = _countriesService.GetCountryByCountryId(countryId);

            //Assert
            Assert.Null(country_response_from_get_method);
        }

        [Fact]
        public void GetCountryByCountryId_CountryIdIsValid()
        {
            //Arrange
            CountryAddRequest? country_add_request = new CountryAddRequest() 
            { 
                CountryName = "China" 
            };

            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

            //Act
            CountryResponse? country_response_from_get = _countriesService.GetCountryByCountryId(country_response_from_add.CountryId);

            //Assert
            Assert.Equal(country_response_from_add, country_response_from_get);
        }
        #endregion
    }
}
