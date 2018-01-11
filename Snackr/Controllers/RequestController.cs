using Microsoft.AspNetCore.Mvc;

namespace Snackr.Controllers
{
    public class RequestController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}