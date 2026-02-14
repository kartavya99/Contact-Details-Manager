using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Globalization;
using System.IO;
using OfficeOpenXml;
using RepositoryContarcts;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;
using Exceptions;


namespace Services
{
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        //private field
        private readonly IpersonsRepository _personsRespository;
        private readonly ILogger<PersonsSorterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
               

        //Constructor
        public PersonsUpdaterService(IpersonsRepository personsRepository, ILogger<PersonsSorterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRespository = personsRepository;           
            _diagnosticContext = diagnosticContext;
            _logger = logger;
            
        }       

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
                throw new ArgumentNullException(nameof(Person));

            //validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = await _personsRespository.GetPersonByPersonID(personUpdateRequest.PersonID);
            if(matchingPerson == null)
            {
                throw new InvalidPersonIDException("Given person id doesn't exist");
            }

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            await _personsRespository.UpdatePerson(matchingPerson);

            return matchingPerson.ToPersonResponse();

        }

    }
}
