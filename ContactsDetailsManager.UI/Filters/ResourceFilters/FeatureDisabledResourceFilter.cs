using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.Eventing.Reader;

namespace ContactDetailsManager.Filters.ResourceFilters
{
    public class FeatureDisabledResourceFilter : IAsyncActionFilter
    {
        private readonly ILogger<FeatureDisabledResourceFilter> _logger;
        private readonly bool _isDisabled;


        public FeatureDisabledResourceFilter(ILogger<FeatureDisabledResourceFilter> logger, bool isDisabled = true)
        {
            _logger = logger;
            _isDisabled = isDisabled;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //TO DO: before logic
            _logger.LogInformation("{FilterName}.{Method} - before", nameof(FeatureDisabledResourceFilter), nameof(OnActionExecutionAsync));

            if(_isDisabled)
            {
                // context.Result = new NotFoundResult(); // 404 not found

                context.Result = new StatusCodeResult(501);
            }
            else
            {
                await next();                
            }

            //TO DO: after logic
            _logger.LogInformation("{FilterName}.{Method} - after", nameof(FeatureDisabledResourceFilter), nameof(OnActionExecutionAsync));
        }
    }
}
