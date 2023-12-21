using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface ICountriesRepository
    {
        Task<Country> Add(Country country);
        Task<List<Country>> GetAllCountries();
        Task<Country?> GetCountryByCountryID(Guid countryID);
        Task<Country?> GetCountryByCountryName(string countryName);
    }
}
