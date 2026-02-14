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
    public interface IPersonsUpdaterService
    {
        /// <summary>
        /// Updates the specified person details based on the given personID
        /// </summary>
        /// <param name="personUpdateRequset">Person details to update, including person id</param>
        /// <returns>Returns the person response object after updation</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequset);
       
    }
}
