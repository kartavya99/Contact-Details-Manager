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
    public class PersonsDeleterService : IPersonsDeleterService
    {
        //private field
        private readonly IpersonsRepository _personsRespository;
        private readonly ILogger<PersonsSorterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
               

        //Constructor
        public PersonsDeleterService(IpersonsRepository personsRepository, ILogger<PersonsSorterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRespository = personsRepository;           
            _diagnosticContext = diagnosticContext;
            _logger = logger;
            
        }       
              
        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }

           Person? person = await _personsRespository.GetPersonByPersonID(personID.Value);
            if (person == null)
                return false;

            await _personsRespository.DeletePersonByPersonID(personID.Value);

            return true;

        }        
    }
}
