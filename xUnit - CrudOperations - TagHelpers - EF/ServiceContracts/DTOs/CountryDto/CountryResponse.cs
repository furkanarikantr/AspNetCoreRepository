using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTOs.CountryDto
{
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        //Geçerli nesnesi CounrtyResponse türündeki başka bir nesneyle karşılaştırır ve her iki değer de aynıysa ture döndürür.
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }

            CountryResponse country_to_compare = (CountryResponse)obj;

            return CountryId == country_to_compare.CountryId && CountryName == country_to_compare.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    //Country objesini CountryResponse objesine çeviriyoruz.
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName
            };
        }
    }
}