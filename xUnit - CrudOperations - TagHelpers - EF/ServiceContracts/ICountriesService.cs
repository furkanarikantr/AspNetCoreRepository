﻿using ServiceContracts.DTOs.CountryDto;

namespace ServiceContracts
{
    public interface ICountriesService
    {
        List<CountryResponse> GetAllCountries();

        CountryResponse? GetCountryByCountryId(Guid? countryId);

        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

    }
}