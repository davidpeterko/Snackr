using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Snackr.Models;
using Snackr.Interfaces;

namespace Snackr.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IConfiguration _configuration;

        public InventoryController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        
        public IActionResult Index()
        {
            var model = new InventoryViewModel
            {
                SnackList = new SnackrBackend(new CassandraConnection(_configuration["CassandraCluster:Keyspace"], 
                    _configuration["CassandraCluster:Hostname"], 
                    Convert.ToInt16(_configuration["CassandraCluster:Port"]),
                    _configuration["CassandraCluster:User"],
                    _configuration["CassandraCluster:Password"]))
                    .GetSnacks()
                    .OrderBy(s => s.snack_brand)
                    .ToList()
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