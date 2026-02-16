using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ContactDetailsManager.Controllers
{
    public class HomeController : Controller
    {
        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature ? exceptionHandlerParthFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if(exceptionHandlerParthFeature != null && exceptionHandlerParthFeature.Error != null)
            {
                ViewBag.ErrorMessage = exceptionHandlerParthFeature.Error.Message;
            }

            return View(); // Views/Shared/Error
        }
    }
}
