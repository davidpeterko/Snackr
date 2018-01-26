using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Snackr.Models;
using Snackr.Interfaces;
using Snackr.DataLayer;
using Snackr.Repositories;

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
                SnackList = new SnackRepository(new CassandraConnection(_configuration["CassandraCluster:Keyspace"], 
                    _configuration["CassandraCluster:Hostname"], 
                    Convert.ToInt16(_configuration["CassandraCluster:Port"]),
                    _configuration["CassandraCluster:User"],
                    _configuration["CassandraCluster:Password"]))
                    .GetSnacks()
                    .OrderBy(s => s._snack_brand)
                    .ToList()
            };

            return View(model);
        }

        private List<Snack> Sort(string key, List<Snack> snacks)
        {
            switch (key)
            {
                    case "brand":
                        return snacks.OrderBy(s => s._snack_brand).ToList();
                    case "name":
                        return snacks.OrderBy(s => s._snack_name).ToList();
                    case "count":
                        return snacks.OrderBy(s => s._snack_count).ToList();
                    default:
                        return snacks;
            }
        }
    }
}