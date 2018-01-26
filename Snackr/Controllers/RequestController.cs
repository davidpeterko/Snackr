using System;
using System.Linq;
using System.Security.Claims;
using Cassandra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Snackr.Models;
using Snackr.Repositories;
using Snackr.DataLayer;

namespace Snackr.Controllers
{
    public class RequestController : Controller
    {
        private readonly IConfiguration _configuration;

        public RequestController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        
        [Authorize]
        public IActionResult Index(string snack_brand, string snack_name)
        {
            var getRequestModel = new GetRequestsModel()
            {
                RequestList = new RequestRepository(new CassandraConnection(_configuration["CassandraCluster:Keyspace"],
                        _configuration["CassandraCluster:Hostname"],
                        Convert.ToInt16(_configuration["CassandraCluster:Port"]),
                        _configuration["CassandraCluster:User"],
                        _configuration["CassandraCluster:Password"]))
                    .GetRequestByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value),
                Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
            };
            
            // make request
            var makeRequestModel = new MakeRequestModel()
            {
                Request = new Request(
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                    snack_brand,
                    snack_name
                )
            };

            var model = new RequestModel()
            {
                GetRequestsModel = getRequestModel,
                MakeRequestModel = makeRequestModel
            };
            
            return View(model);
        }

        [Authorize]
        public IActionResult MakeRequest(string email, string snack_brand, string snack_name, int request_count)
        {
            var makeRequestModel = new MakeRequestModel()
            {
                Request = new Request(email, snack_brand, snack_name, request_count)
            };

            var model = new RequestModel()
            {
                MakeRequestModel = makeRequestModel
            };
            
            var result = new RequestRepository(new CassandraConnection(_configuration["CassandraCluster:Keyspace"],
                _configuration["CassandraCluster:Hostname"],
                Convert.ToInt16(_configuration["CassandraCluster:Port"]),
                _configuration["CassandraCluster:User"],
                _configuration["CassandraCluster:Password"])).MakeRequest(new Request(email, snack_brand, snack_name, request_count));

            if (result.Equals("Insufficient"))
            {
                RequestInsufficient();
            }

            return View(model);
        }

        [Authorize]
        public IActionResult RequestInsufficient()
        {
            return View();
        }
        
        [Authorize]
        public IActionResult RequestFailed()
        {
            return View();
        }

    }
}