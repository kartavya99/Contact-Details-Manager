using System;
using System.Collections.Generic;
using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Services;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using Moq;
using AutoFixture;
using FluentAssertions;


namespace CDMTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly IFixture _fixture;

        //constructor
        public CountriesServiceTest()
        {
            _fixture = new Fixture();

            var countriesInititalData = new List<Country>() { };
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                    new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );

            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInititalData);

            _countriesService = new CountriesService(null);
            
        }

        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrang
            CountryAddRequest? request = null;

            //Act
            var action = async () =>
            {
               await _countriesService.AddCountry(request);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }


        //When CountryName is null, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
         
            //Arrang
            CountryAddRequest? request = _fixture.Build<CountryAddRequest>().With(temp => temp.CountryName, null as string).Create();

            //Act
            var action = async () =>
            {
                await _countriesService.AddCountry(request);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        //when CountryName is duplicate, it should throw ArgurmentException
        [Fact]
        public async Task AddCountry_DulicateCountryName()
        {
            //Arrang
            CountryAddRequest? request1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest? request2 = _fixture.Build<CountryAddRequest>().With(temp => temp.CountryName, request1.CountryName).Create();

            //Act
            var action = async () =>
            {
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            };
            //Asert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //when you supply proper coutry name, it should inset (add) the country to the existing list of coutries
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //Arrang
            CountryAddRequest? request = _fixture.Create<CountryAddRequest>();

            //Act
            CountryResponse response = await _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllcountries = await _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryID != Guid.Empty);
            //Assert.Contains(response, countries_from_GetAllcountries);            
            countries_from_GetAllcountries.Should().Contain(response);
        }

        #endregion


        #region GetAllCountries

        [Fact]
        // The list of countries should be empty by default (before adding any countries)
        public async Task GetALlcountries_EmptyList()
        {
            //Act
            List<CountryResponse> actual_country_response_list = await _countriesService.GetAllCountries();

            //Assert
            //Assert.Empty(actual_country_response_list);
            actual_country_response_list.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                _fixture.Create<CountryAddRequest>(),
                _fixture.Create<CountryAddRequest>()                
            };

            //Act
            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

            foreach(CountryAddRequest country_request in country_request_list)
            {
                countries_list_from_add_country.Add(await _countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

            ////read each element from countries list_from_add_cotuntry
            //foreach (CountryResponse expected_country in countries_list_from_add_country)
            //{
            //    Assert.Contains(expected_country, actualCountryResponseList);
            //}

            //Assert
            actualCountryResponseList.Should().BeEquivalentTo(countries_list_from_add_country);
        }

        #endregion

        #region GetCountryByCountryID

        [Fact]
        //If we supply null as CountryID it should return null as CountryResponse
        public async Task GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? countryID = null;

            //Act
            CountryResponse? country_response_from_get_method = await _countriesService.GetCountryByCountryID(countryID);

            //Assert
            //Assert.Null(country_response_from_get_method);
            country_response_from_get_method.Should().BeNull(); 
        }

        [Fact]
        //If we supply a valid country id, it should return the matching country details as CountryResponse object
        public async Task GetCountryByCountryID_Valid()
        {
            //Arrange
            CountryAddRequest country_add_request = _fixture.Create<CountryAddRequest>();
            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            //Act
            CountryResponse? country_response_from_get = await _countriesService.GetCountryByCountryID(country_response_from_add.CountryID);

            //Assert
            // Assert.Equal(country_response_from_add, country_response_from_get);
            country_response_from_get.Should().Be(country_response_from_add);
        }
        #endregion
    }
}