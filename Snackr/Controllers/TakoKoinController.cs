using Microsoft.AspNetCore.Mvc;

namespace Snackr.Controllers
{
    public class TakoKoinController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}