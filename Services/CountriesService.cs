using Entities;
using ServiceContracts;
using ServiceContracts.DTO;


namespace Services
{    
    public class CountriesService : ICountriesService
    {
        //private field
        private readonly List<Country> _countries;

        //constructor
        public CountriesService(bool initialise = true)
        {
            _countries = new List<Country>();
            if (initialise)
            {
                _countries.AddRange(new List<Country>()
                {
                    new Country() { CountryID = Guid.Parse("E523674F-A628-4B08-B90B-F1377FB1A998"), CountryName="Australia" },
                    new Country() { CountryID = Guid.Parse("47FEE773-EB29-48A0-9C25-402813BD71CA"), CountryName="New Zealand" },
                    new Country() { CountryID = Guid.Parse("E892169E-D130-4133-83D8-FB91C931E8D9"), CountryName="England" },
                    new Country() { CountryID = Guid.Parse("5A34DD9C-1B7B-436B-AA88-27AF4545B96F"), CountryName="India" },
                    new Country() { CountryID = Guid.Parse("17B72306-AFBE-40E7-8145-72AEDE0D4750"), CountryName="Singapore" },
                }); 
            }
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            // throw new NotImplementedException();

            //Validation: CountryAddRequest parameter can't be null
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Validate: CountryName can't be null
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Validation: CountryName can't be duplicate
            if(_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0 )
            {
                throw new ArgumentException("GIven country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country =countryAddRequest.ToCountry();

            //generate CountryID
            country.CountryID = Guid.NewGuid();

            //Add country object into _countries
            _countries.Add(country);

            return country.ToCountryResponse();
            
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null) 
            {
                return null;
            }

            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryID);

            if (country_response_from_list == null)
            {
                return null;
            }

            return country_response_from_list.ToCountryResponse();
        }
    }
}
