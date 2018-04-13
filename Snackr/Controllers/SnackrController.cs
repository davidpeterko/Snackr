using Microsoft.AspNetCore.Mvc;

namespace Snackr.Controllers
{
    public class SnackrController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}