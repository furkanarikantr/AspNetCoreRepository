using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //country list
        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation : countryAddRequest null olamaz.
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Validation : countryAddRequest.countryName empty olamaz.
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Validation : countryAddRequest.countryName List'emizdeki country.Name ile eşleşemez.
            //foreach (var selectedCountry in _countries)
            //{
            //    if (countryAddRequest.CountryName == selectedCountry.CountryName)
            //    {
            //        throw new ArgumentException(nameof(countryAddRequest.CountryName));
            //    }
            //}
            if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //CountryAddRequest'i Country nesnesine dönüştürüyoruz.
            Country country = countryAddRequest.ToCountry();

            //Country için CountryId oluşturuyoruz.
            country.CountryId = Guid.NewGuid();

            //Country nesnesini Country listesine ekliyoruz.
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
                return null;

            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryId == countryId);

            if (country_response_from_list == null)
                return null;

            return country_response_from_list.ToCountryResponse();
        }
    }
}