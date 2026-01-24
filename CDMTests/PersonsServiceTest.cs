using System;
using Xunit;
using ServiceContracts;
using Entities;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;


namespace CDMTests
{
    public class PersonsServiceTest
    {
        //private field
        private readonly IPersonsService _personsService;
        
        //constructor
        public PersonsServiceTest()
        {
            _personsService = new PersonsService();
        }

        #region AddPerson

        // When we supply null values as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personsService.AddPerson(personAddRequest);
            });

        }


        // When we supply null values as PersonName, it should throw ArgumentException
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.AddPerson(personAddRequest);
            });

        }

        // When we supply proper person details, it should insert the person into the person list; 
        // and it should return an object of PersonRequest, which includes with newly generated person id
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() 
            { 
                PersonName = "Person name..." ,
                Email = "person@eexample",
                Address = "sample address,",
                CountryID = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true,            
            };

            //Act
            PersonResponse person_response_from_add = _personsService.AddPerson(personAddRequest);

            List<PersonResponse> persons_list = _personsService.GetAllPersons();

            //Assert
            Assert.True(person_response_from_add.PersonID != Guid.Empty);

            Assert.Contains(person_response_from_add, persons_list);
        }
        #endregion

    }
}
