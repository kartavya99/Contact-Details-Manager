using Microsoft.AspNetCore.Mvc;

namespace ContactDetailsManager.Controllers
{
    public class PersonsController : Controller
    {
        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
