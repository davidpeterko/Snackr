using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Snackr.Controllers
{
    public class RequestController : Controller
    {
        // GET
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

    }
}