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
    public class PersonsAdderService : IPersonsAdderService
    {
        //private field
        private readonly IpersonsRepository _personsRespository;
        private readonly ILogger<PersonsSorterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
               

        //Constructor
        public PersonsAdderService(IpersonsRepository personsRepository, ILogger<PersonsSorterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRespository = personsRepository;           
            _diagnosticContext = diagnosticContext;
            _logger = logger;
            
        }       

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if PersonAddRequest is not null
            if(personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model validations
            ValidationHelper.ModelValidation(personAddRequest);

            //Convert personAddRequest into Person type
            Person person = personAddRequest.ToPerson();

            // generate PersonID
            person.PersonID = Guid.NewGuid();

            //add person object to person list
            await _personsRespository.AddPerson(person);
            
            //conver the Person object into PersonResponse type
            return person.ToPersonResponse();
        }          

               
    }
}
