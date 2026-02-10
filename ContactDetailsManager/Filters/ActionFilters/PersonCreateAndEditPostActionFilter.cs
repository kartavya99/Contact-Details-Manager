using ContactDetailsManager.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace ContactDetailsManager.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesService _countriesService;

        public PersonCreateAndEditPostActionFilter(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //TO DO: before logic
            if(context.Controller is PersonsController personController)
            {
                if(!personController.ModelState.IsValid)
                {
                    List<CountryResponse> countries = await _countriesService.GetAllCountries();
                    personController.ViewBag.Countries = countries.Select(temp => new SelectListItem()
                    {
                        Text = temp.CountryName,
                        Value = temp.CountryID.ToString()
                    });

                    personController.ViewBag.Errors = personController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    var personRequest = context.ActionArguments["personRequest"];

                    context.Result = personController.View(personRequest);
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();

            }

            //TO DO: after logic
        }
    }
}
