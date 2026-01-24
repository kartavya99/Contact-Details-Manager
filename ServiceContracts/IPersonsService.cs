using System;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represetns business logic for manipulating entity
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public interface IPersonsService
    {
        /// <summary>
        /// Add a new person into list of persons
        /// </summary>
        /// <param name="person">Person to add</param>
        /// <returns>Returns the same person details along with newly generated PersonID</returns>
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        List<PersonResponse> GetAllPersons();
    }
}
