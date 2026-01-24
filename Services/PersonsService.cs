using System;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        //private field
        private readonly List<Person> _person;
        private readonly ICountriesService _countriesService;

        //Constructor
        public PersonsService()
        {
            _person = new List<Person>();   
            _countriesService = new CountriesService();
        }

        private PersonResponse ConvertPersonToPersonReponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }


        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if PersonAddRequest is not null
            if(personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Validate PersonName
            if(string.IsNullOrEmpty(personAddRequest.PersonName))
            {
                throw new ArgumentException("PersonName can't be blank");
            }

            //Convert personAddRequest into Person type
            Person person = personAddRequest.Toperson();

            // generate PersonID
            person.PersonID = Guid.NewGuid();

            //add person object to person list
            _person.Add(person);

            //conver the Person object into PersonResponse type
            return ConvertPersonToPersonReponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            throw new NotImplementedException();
        }
    }
}
