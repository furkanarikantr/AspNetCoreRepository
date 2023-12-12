using ServiceContracts.DTOs.PersonDto;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonService
    {
        Task<PersonResponse?> AddPerson(PersonAddRequest? personAddRequest);
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest personUpdateRequest);
        Task<bool> DeletePerson(Guid? personId);

        Task<List<PersonResponse>> GetAllPerson();
        Task<PersonResponse?> GetPersonByPersonId(Guid? personId);
        Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString);
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOption sortOrder);
    }
}
