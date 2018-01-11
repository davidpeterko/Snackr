using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Snackr.Models;

namespace Snackr.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            var model = new InventoryViewModel
            {
                SnackList = new SnackrBackend().getSnacks().OrderBy(s => s.snack_brand).ToList()
            };

            return View(model);
        }

        private List<Snack> Sort(string key, List<Snack> snacks)
        {
            switch (key)
            {
                    case "brand":
                        return snacks.OrderBy(s => s.snack_brand).ToList();
                    case "name":
                        return snacks.OrderBy(s => s.snack_name).ToList();
                    case "count":
                        return snacks.OrderBy(s => s.snack_count).ToList();
                    default:
                        return snacks;
            }
        }
    }
}