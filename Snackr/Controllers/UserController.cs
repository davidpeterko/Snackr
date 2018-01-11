using Microsoft.AspNetCore.Mvc;

namespace Snackr.Controllers
{
    public class UserController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}