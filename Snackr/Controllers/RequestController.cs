using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Snackr.Models;
using Snackr.Repositories;
using Snackr.DataLayer;
using Snackr.Interfaces;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace Snackr.Controllers
{
    [Route("Request")]
    public class RequestController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;

        public RequestController(IConfiguration configuration)
        {
            this._configuration = configuration;
            _connection = new CassandraConnection(_configuration["CassandraCluster:Keyspace"],
                        _configuration["CassandraCluster:Hostname"],
                        Convert.ToInt16(_configuration["CassandraCluster:Port"]),
                        _configuration["CassandraCluster:User"],
                        _configuration["CassandraCluster:Password"]);
        }

        [Route("Index")]
        public IActionResult Index()
        {
            var getRequestModel = new GetRequestsModel()
            {
                RequestList = new RequestRepository(_connection)
                    .GetRequestByEmail(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value),
                Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
            };
            
            var model = new RequestModel()
            {
                GetRequestsModel = getRequestModel,
            };
            
            return View(model);
            
            return View();
        }
        
        /// <summary>
        /// Takes you to the make a request form, prefilled
        /// </summary>
        /// <param name="snack_brand"></param>
        /// <param name="snack_name"></param>
        /// <returns></returns>
        [Route("RequestForm")]
        [Authorize]
        public IActionResult RequestForm(string snack_brand, string snack_name)
        {
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
                MakeRequestModel = makeRequestModel
            };
            
            return View(model);
        }

        /// <summary>
        /// Make a request function
        /// </summary>
        /// <param name="email"></param>
        /// <param name="snack_brand"></param>
        /// <param name="snack_name"></param>
        /// <param name="request_count"></param>
        /// <returns></returns>
        [Route("MakeRequest")]
        [Authorize]
        public IActionResult MakeRequest(string email, string snack_brand, string snack_name, string request_count)
        {
            Request request = new Request(email, snack_brand, snack_name, Int32.Parse(request_count));
            
            var makeRequestModel = new MakeRequestModel()
            {
                Request = new Request(email, snack_brand, snack_name, Int32.Parse(request_count))
            };

            var model = new RequestModel()
            {
                MakeRequestModel = makeRequestModel
            };
            
            var result = new RequestRepository(_connection).MakeRequest(request);

            if (result.Equals("Insufficient"))
            {
                RequestInsufficient();
            }
            else
            {
                new RequestRepository(_connection).DecrementCount(request);
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