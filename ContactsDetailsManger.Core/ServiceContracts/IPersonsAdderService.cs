using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represetns business logic for manipulating entity
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public interface IPersonsAdderService
    {
        /// <summary>
        /// Add a new person into list of persons
        /// </summary>
        /// <param name="person">Person to add</param>
        /// <returns>Returns the same person details along with newly generated PersonID</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
       
    }
}
