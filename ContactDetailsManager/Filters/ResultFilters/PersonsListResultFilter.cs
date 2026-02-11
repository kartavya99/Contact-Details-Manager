using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactDetailsManager.Filters.ResultFilters
{
    public class PersonsListResultFilter : IAsyncActionFilter
    {
        private readonly ILogger<PersonsListResultFilter> _logger;

        public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // TO DO: before logic
            _logger.LogInformation("{FilterName}.{MethodName} - before", nameof(PersonsListResultFilter), nameof(OnActionExecutionAsync));

            await next();

            //TO DO; after logic
            _logger.LogInformation("{FilterName}.{MethodName} - after", nameof(PersonsListResultFilter), nameof(OnActionExecutionAsync));

            context.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
    }
}
