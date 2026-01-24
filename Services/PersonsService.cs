using System;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;

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

            //Model validations
            ValidationHelper.ModelValidation(personAddRequest);

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
            return _person.Select(temp => temp.ToPersonResponse()).ToList();    
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null) return null;

            Person? person = _person.FirstOrDefault(temp => temp.PersonID == personID);
            if(person == null) return null;

            return person.ToPersonResponse();
        }
    }
}
