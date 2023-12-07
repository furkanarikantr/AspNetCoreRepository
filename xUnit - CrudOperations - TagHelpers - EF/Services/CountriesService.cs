using Entities;
using ServiceContracts;
using ServiceContracts.DTOs.CountryDto;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //country list
        //private readonly List<Country> _countries;
        private readonly PersonsDbContext _db;

        public CountriesService(PersonsDbContext personsDbContext ,bool initialize = true)
        {
            /*
                _countries = new List<Country>();
                if (initialize)
                {
                    _countries.AddRange(new List<Country>()
                    {
                        new Country() {  CountryId = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B"), CountryName = "USA" },

                        new Country() { CountryId = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F"), CountryName = "Canada" },

                        new Country() { CountryId = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E"), CountryName = "UK" },

                        new Country() { CountryId = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D"), CountryName = "Germany" },

                        new Country() { CountryId = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB"), CountryName = "Italy" }
                    });
                }
            */
            _db = personsDbContext;
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

            //if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            if (_db.Countries.Count(temp => temp.CountryName == countryAddRequest.CountryName) > 0)
                {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //CountryAddRequest'i Country nesnesine dönüştürüyoruz.
            Country country = countryAddRequest.ToCountry();

            //Country için CountryId oluşturuyoruz.
            country.CountryId = Guid.NewGuid();

            //Country nesnesini Country listesine ekliyoruz.
            _db.Countries.Add(country);
            _db.SaveChanges();  //DbSet'ine verileri ekliyoruz.

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            //return _countries.Select(country => country.ToCountryResponse()).ToList();
            return _db.Countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
                return null;

            Country? country_response_from_list = _db.Countries.FirstOrDefault(temp => temp.CountryId == countryId);

            if (country_response_from_list == null)
                return null;

            return country_response_from_list.ToCountryResponse();
        }
    }
}