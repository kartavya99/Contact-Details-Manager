using ServiceContracts.DTO;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesGetterService
    {       
        
        /// Returns all countries from the list
        /// </summary>
        /// <returns>all countries from the list as List of CountryResponse</returns>CountryResponse></returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the give coutnry id
        /// </summary>
        /// <param name="countryID">CountryID (guid) to search</param>
        /// <returns>Matching country as CountryResponse</returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);
        
    }

}
