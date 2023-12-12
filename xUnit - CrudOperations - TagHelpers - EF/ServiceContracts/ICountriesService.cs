using ServiceContracts.DTOs.CountryDto;

namespace ServiceContracts
{
    public interface ICountriesService
    {
        //Task : Bu fonksiyonun asenktron bir operasyon olacağını belirten bir ifadedir.
        Task<List<CountryResponse>> GetAllCountries();

        Task<CountryResponse?> GetCountryByCountryId(Guid? countryId);

        Task<CountryResponse?> AddCountry(CountryAddRequest? countryAddRequest);
    }
}