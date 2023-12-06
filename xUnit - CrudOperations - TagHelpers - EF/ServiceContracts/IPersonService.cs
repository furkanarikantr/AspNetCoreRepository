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
        PersonResponse AddPerson(PersonAddRequest personAddRequest);
        PersonResponse UpdatePerson(PersonUpdateRequest personUpdateRequest);
        bool DeletePerson(Guid? personId);

        List<PersonResponse> GetAllPerson();
        PersonResponse? GetPersonByPersonId(Guid? personId);
        List<PersonResponse> GetFilteredPersons(string? searchBy, string? searchString);
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOption sortOrder);
    }
}
