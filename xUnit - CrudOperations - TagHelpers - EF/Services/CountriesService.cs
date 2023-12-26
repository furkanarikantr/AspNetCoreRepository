using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTOs.CountryDto;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //country list
        //private readonly List<Country> _countries;
        //private readonly ApplicationDbContext _db;
        private readonly ICountriesRepository _countriesRepository;

        public CountriesService(ICountriesRepository countriesRepository/*ApplicationDbContext personsDbContext*/ /*,bool initialize = true*/)
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
            //_db = personsDbContext;

            _countriesRepository = countriesRepository;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
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
            if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                //<throw new ArgumentException(nameof(countryAddRequest.CountryName));
                throw new ArgumentException("Given country name already exists");
            }

            //CountryAddRequest'i Country nesnesine dönüştürüyoruz.
            Country country = countryAddRequest.ToCountry();

            //Country için CountryId oluşturuyoruz.
            country.CountryId = Guid.NewGuid();

            //Country nesnesini Country listesine ekliyoruz.
            //await _db.Countries.AddAsync(country);
            //await _db.SaveChangesAsync();  //DbSet'ine verileri ekliyoruz.
            await _countriesRepository.AddCountry(country);

            return country.ToCountryResponse();

        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            //var countries = _db.Countries.Include("Persons").ToList();

            //return _countries.Select(country => country.ToCountryResponse()).ToList();
            List<Country> countries = await _countriesRepository.GetAllCountries();
            return countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
                return null;

            Country? country_response_from_list = await _countriesRepository.GetCountryByCountryID(countryId.Value);

            if (country_response_from_list == null)
                return null;

            return country_response_from_list.ToCountryResponse();
        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            ExcelPackage excelPackage = new ExcelPackage((memoryStream));

            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Countries"];

            int rowCount = workSheet.Dimension.Rows;
            int countriesInserted = 0;
            for (int row = 2; row <= rowCount; row++)
            {
                string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);

                if (!string.IsNullOrEmpty(cellValue))
                {
                    string? countryName = cellValue;

                    if (await _countriesRepository.GetCountryByCountryName(countryName) == null)
                    {
                        Country country = new Country()
                        {
                            CountryName = countryName
                        };
                        await _countriesRepository.AddCountry(country);

                        countriesInserted++;
                    }
                }
            }

            return countriesInserted;
        }
    }
}