using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace ContactDetailsManager.Filters.ActionFilters
{
    public class PersonsListActionsFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionsFilter> _logger;

        public PersonsListActionsFilter(ILogger<PersonsListActionsFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //To do: add after logic here
            _logger.LogInformation("PersonListActionFilter.OnActionEexcuted");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //To do: add before logic here
            _logger.LogInformation("PersonListActionFilter.OnActionEexcuting");

            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

                //Validate the searchBy parameter value
                if(!string.IsNullOrEmpty(searchBy))
                {
                    var searchByOptions = new List<string>()
                    {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.DateOfBirth),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.CountryID),
                        nameof(PersonResponse.Address)
                    };

                    //reset the searchBy parameter value
                    if(searchByOptions.Any(temp => temp == searchBy) == false)
                    {
                        _logger.LogInformation("searchBy actual value {searchBy}", searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                        _logger.LogInformation("searchBy updated value {searchBy}", context.ActionArguments["searchBy"]); 
                    }
                }
            }
        }
    }
}
