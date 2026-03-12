using Microsoft.AspNetCore.Mvc;

namespace Autoservice.Api.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
