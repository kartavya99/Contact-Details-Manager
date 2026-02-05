using Entities;

namespace RepositoryContarcts
{
    /// <summary>
    /// Represents data access logic for managin Country entity
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Adds a new country object to the data store
        /// </summary>
        /// <param name="country">Country object to add </param>
        /// <returns>Returns the country obnject after adding it to the data store</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// Returns all countries in the data store
        /// </summary>
        /// <param name="country"></param>
        /// <returns>Returns all countries from the table</returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on teh given country id, otherwise, it returns null
        /// </summary>
        /// <param name="country">CountryID to search</param>
        /// <returns>Matching country or null</returns>
        Task<Country?> GetCountryByCountryId(Guid countryID);

        /// <summary>
        /// Returns a country object based on the given coutnry name
        /// </summary>
        /// <param name="countryName">Country Name to search</param>
        /// <returns>Matchingcountry or null</returns>
        Task<Country?> GetCountryByCountryName(string countryName);
    }
}
