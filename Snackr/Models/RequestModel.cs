using System.Collections.Generic;
using Snackr.DataLayer;

namespace Snackr.Models
{
    public class RequestModel
    {
        public GetRequestsModel GetRequestsModel { get; set; }
        public MakeRequestModel MakeRequestModel { get; set; }
    }
}