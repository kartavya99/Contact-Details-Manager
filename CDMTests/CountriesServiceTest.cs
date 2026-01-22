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

        public CountriesServiceTest()
        {
            _countriesService = new CounttriesService();
        }

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

            //Assert
            Assert.True(response.CountryID != Guid.Empty);
            
        }
    }
}
