using System.Collections.Generic;
using Snackr.DataLayer;

namespace Snackr.Models
{
    public class GetRequestsModel
    {
        public string Email { get; set; }
        public string SnackBrand { get; set; }
        public string SnackName { get; set; }
        public int SnackCount { get; set; }
        public List<Request> RequestList { get; set; }
    }
}