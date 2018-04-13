using System;
using System.Collections.Generic;
using Snackr.Interfaces;

namespace Snackr.DataLayer
{
    public class Request
    {
        public string _email { get; set; }
        public string _snack_brand { get; set; }
        public string _snack_name { get; set; }
        public int? _request_count { get; set; }
        
        public Request() {}

        public Request(string email, string snack_brand, string snack_name)
        {
            _email = email;
            _snack_brand = snack_brand;
            _snack_name = snack_name;
        }
        
        public Request(string email, string snack_brand, string snack_name, int request_count)
        {
            _email = email;
            _snack_brand = snack_brand ;
            _snack_name = snack_name ;
            _request_count = request_count;
        }
        
    }
}