using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using Xunit;
using Xunit.Abstractions;
using static System.Collections.Specialized.BitVector32;


namespace CDMTests
{
    public class PersonsServiceTest
    {
        //private field
        private readonly IPersonsService _personService;
        private readonly ICountriesService _coutriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        
        //constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();

            var countriesInititalData = new List<Country>() { };
            var personsInititalData = new List<Person>() { };

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                    new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );

            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInititalData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInititalData);

            _coutriesService = new CountriesService(dbContext);           
            _personService = new PersonsService(dbContext, _coutriesService);
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        // When we supply null values as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Act
           Func<Task> action = async () =>
            {
               await _personService.AddPerson(personAddRequest);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }


        // When we supply null values as PersonName, it should throw ArgumentException
        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, null as string).Create();

            //Act
            Func<Task> action = async () =>
            {
                await _personService.AddPerson(personAddRequest);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // When we supply proper person details, it should insert the person into the person list; 
        // and it should return an object of PersonRequest, which includes with newly generated person id
        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone@example.com").Create();

            //Act
            PersonResponse person_response_from_add = await _personService.AddPerson(personAddRequest);

            List<PersonResponse> persons_list = await _personService.GetAllPersons();

            //Assert
            //Assert.True(person_response_from_add.PersonID != Guid.Empty);
            person_response_from_add.PersonID.Should().NotBe(Guid.Empty);

            //Assert.Contains(person_response_from_add, persons_list);
            persons_list.Should().Contain(person_response_from_add);
        }
        #endregion

        #region GetPersonByPersonID
        //If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonId_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Act
            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(personID);

            //Assert
            person_response_from_get.Should().BeNull();
        }

        //If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public async Task GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();
            CountryResponse country_response = await _coutriesService.AddCountry(country_request);

            //Act
            PersonAddRequest perons_request = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "email@example.com").Create();

            PersonResponse person_response_from_add = await _personService.AddPerson(perons_request);

            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(person_response_from_add.PersonID);

            //Assert
            //Assert.Equal(person_response_from_add, person_response_from_get);
            person_response_from_get.Should().Be(person_response_from_add);

        }
        #endregion

        #region GetAllPersons
        //The GetALlPersons() should return an empty list by default
        [Fact]
        
        public async Task GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> person_from_get = await _personService.GetAllPersons();

            //Assert
            //Assert.Empty(person_from_get);
            person_from_get.Should().BeEmpty();
        }

        //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public async Task GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _coutriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _coutriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_1@example.com").Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_2@example.com").Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_3@example.com").Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach(PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach(PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_get  = await _personService.GetAllPersons();

            //print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //Assert
            //foreach (PersonResponse person_reponse_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_reponse_from_add, persons_list_from_get);
            //}
            persons_list_from_get.Should().BeEquivalentTo(person_response_list_from_add);
        }

        #endregion

        #region GetFilteredPerson

        // If the search text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _coutriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _coutriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_1@example.com").Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_2@example.com").Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_3@example.com").Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            //print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //Assert
            //foreach (PersonResponse person_reponse_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_reponse_from_add, persons_list_from_search);
            //}
            persons_list_from_search.Should().BeEquivalentTo(person_response_list_from_add);
        }

        // First we will add few persons; and then we will search based on person name with some search string. 
        // It should return the matchin person
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _coutriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _coutriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "mary")
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .With(temp => temp.Email, "someone_2@example.com").Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Scott")
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .With(temp => temp.Email, "someone_3@example.com").Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_search = await _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            //print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            //Assert
            //foreach (PersonResponse person_reponse_from_add in person_response_list_from_add)
            //{
            //    if(person_reponse_from_add.PersonName != null)
            //    {
            //        if(person_reponse_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
            //            {
            //                Assert.Contains(person_reponse_from_add, persons_list_from_search);
            //            }
            //    }
            //}
            persons_list_from_search.Should().OnlyContain(temp => temp.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region GetSortedPerson

        //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
        [Fact]
        public async Task GetSortedPerson()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _coutriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _coutriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Mary")
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .With(temp => temp.Email, "someone_2@example.com").Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .With(temp => temp.Email, "someone_3@example.com").Create();


            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1,
                person_request_2,
                person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            List<PersonResponse> allPersons = await _personService.GetAllPersons();

            //Act
            List<PersonResponse> persons_list_from_sort = await _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();

            ////Assert
            //for (int i = 0; i < person_response_list_from_add.Count; i++)
            //{
            //    Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
            //}

            persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }
        #endregion

        #region UpdatePerson
        // When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = null;

            //Act
           Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(person_update_request);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply invalid person id, th should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = _fixture.Build<PersonUpdateRequest>().Create();

            //Assert
            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(person_update_request);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //Wehn PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonaNameIsNull()
        {
            //Arrange
            CountryAddRequest country_add_request = _fixture.Create<CountryAddRequest>();
            CountryResponse country_response_from_add = await _coutriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
               .With(temp => temp.PersonName, "Smith")
               .With(temp => temp.Email, "someone_1@example.com")
               .With(temp => temp.CountryID, country_response_from_add.CountryID)
               .Create();

            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;

            //Act
            var action = async () =>
            {
               await _personService.UpdatePerson(person_update_request);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        //Frist, add a new person and try to update the person name and email.
        [Fact]
        public async Task UpdatePerson_PersonFUllDeatilsUpdation()
        {
            //Arrange
            CountryAddRequest country_add_request = _fixture.Create<CountryAddRequest>();
            CountryResponse country_response_from_add = await _coutriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
               .With(temp => temp.PersonName, "Smith")
               .With(temp => temp.Email, "someone_1@example.com")
               .With(temp => temp.CountryID, country_response_from_add.CountryID)
               .Create();

            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "William";
            person_update_request.Email = "william@test.com";

            //Act
            PersonResponse person_response_from_update = await _personService.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(person_response_from_update.PersonID);

            //Assert
            //Assert.Equal(person_response_from_get, person_response_from_update);
            person_response_from_update.Should().Be(person_response_from_get);
            
        }


        #endregion

        #region DeltePerson

        //If you supply an valid PersonID, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            //Arrange
            CountryAddRequest country_add_request = _fixture.Create<CountryAddRequest>();
            CountryResponse country_response_from_add = await _coutriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
               .With(temp => temp.PersonName, "Smith")
               .With(temp => temp.Email, "someone_1@example.com")
               .With(temp => temp.CountryID, country_response_from_add.CountryID)
               .Create();

            PersonResponse person_response_from_add = await _personService.AddPerson(person_add_request);

            //Act
            bool isDeleted = await _personService.DeletePerson(person_response_from_add.PersonID);

            //Assert
            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }

        //If you supply an invalid PersonID, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            //Act
            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

            //Assert
            //Assert.False(isDeleted);
            isDeleted.Should().BeFalse();
        }

        #endregion

    }
}
