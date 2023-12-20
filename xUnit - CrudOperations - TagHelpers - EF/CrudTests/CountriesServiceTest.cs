using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTOs.CountryDto;
using Services;
using EntityFrameworkCoreMock;
using Moq;

namespace CrudTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            //var dbContextOptions = new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options);
            //_countriesService = new CountriesService(dbContextOptions);

            //mock kullanım adımları

            //Test aşamasında kullanılması için countries'lerin başlangıç değeri.
            var countriesInitialData = new List<Country>() { };

            //Gerçek veritabanını kullanmak yerine Entityframework kullanılarak mock nesnesi oluşturulur.
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
              new DbContextOptionsBuilder<ApplicationDbContext>().Options
             );

            //Oluşturulan mock DbContext'in gerçek DbContext'ten DbSet değerleri taklit etmesi sağlanır.
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);

            //Oluşturulan mock DbContext nesnesi kullanılarak bir ApplicationDbContext nesnesi oluşturulur.
            ApplicationDbContext dbContext = dbContextMock.Object;

            _countriesService = new CountriesService(dbContext);
        }

        #region AddCountry
        //CountriesService'imizdeki AddRequest'teki CountryAddRequest null olursa, hata fırlatmalı.
        [Fact]
        public async Task AddCountry_CountryIsNull()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }

        //CountryName null olursa, hata fırlatmalı
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }

        //CountryName tekrarlanırsa, hata fırlatmalı
        [Fact]
        public async Task AddCountry_CountryNameIsDuplicate()
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

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });
        }

        //CountryName uygunsa, eklenmeli.
        [Fact]
        public async Task AddCountry_CountryDetailsIsProper()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            //Act
            CountryResponse? response = await _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountries();

            //Assert
            Assert.True(response?.CountryId != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);
        }
        #endregion

        #region GetAllCountries
        //Default olarak countries listesi boş olmalı.
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            //Acts
            List<CountryResponse> actual_countries_list = await _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actual_countries_list);
        }

        //
        [Fact]
        public async Task GetCountries_AddFewCountries()
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
                countries_list_from_add_country.Add(await _countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actualCountriesResponseList = await _countriesService.GetAllCountries();
            foreach (CountryResponse expected_country in countries_list_from_add_country)
            {
                Assert.Contains(expected_country, actualCountriesResponseList);
            }
        }
        #endregion

        #region GetCountryByCountryId
        [Fact]
        public async Task GetCountryByCountryId_CountryIdIsNull()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            CountryResponse? country_response_from_get_method = await _countriesService.GetCountryByCountryId(countryId);

            //Assert
            Assert.Null(country_response_from_get_method);
        }

        [Fact]
        public async Task GetCountryByCountryId_CountryIdIsValid()
        {
            //Arrange
            CountryAddRequest? country_add_request = new CountryAddRequest()
            {
                CountryName = "China"
            };

            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            //Act
            CountryResponse? country_response_from_get = await _countriesService.GetCountryByCountryId(country_response_from_add.CountryId);

            //Assert
            Assert.Equal(country_response_from_add, country_response_from_get);
        }
        #endregion
    }
}
