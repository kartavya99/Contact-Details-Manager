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
    public interface IPersonsDeleterService
    {
        /// <summary>
        /// Delete a person based on the given person id
        /// </summary>
        /// <param name="PersonID">PersonID to delete</param>
        /// <returns>Returns true, if the deletion is successful; otherwise false</returns>
        Task<bool> DeletePerson(Guid? personID);        
    }
}
