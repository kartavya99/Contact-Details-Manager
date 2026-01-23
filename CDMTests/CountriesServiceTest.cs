using System;
using System.Collections.Generic;
using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Services;


namespace CDMTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        //constructor
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }

        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrang
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }


        //When CountryName is null, it should throw ArgumentException
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrang
            CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }


        //when CountryName is duplicate, it should throw ArgurmentException
        [Fact]
        public void AddCountry_DulicateCountryName()
        {
            //Arrang
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "AUS" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "AUS" };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        //when you supply proper coutry name, it should inset (add) the country to the existing list of coutries
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //Arrang
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "New Zealand"};

            //Act
            CountryResponse response = _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllcountries = _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllcountries);            
        }

        #endregion


        #region GetAllCountries

        [Fact]
        // The list of countries should be empty by default (before adding any countries)
        public void GetALlcountries_EmptyList()
        {
            //Act
            List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actual_country_response_list);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName = "Aus" },
                new CountryAddRequest() { CountryName = "NZ" }
            };

            //Act
            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

            foreach(CountryAddRequest country_request in country_request_list)
            {
                countries_list_from_add_country.Add(_countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            //read each element from countries list_from_add_cotuntry
            foreach (CountryResponse expected_country in countries_list_from_add_country)
            {
                Assert.Contains(expected_country, actualCountryResponseList);
            }
        }

        #endregion
    }
}