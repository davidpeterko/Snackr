using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Snackr.Interfaces;
using Snackr.DataLayer;

namespace Snackr.Repositories
{
    public class RequestRepository
    {
        private readonly IConnection _CassandraConnection;

        /// <summary>
        /// constructor to set the cassandra connection, constructor injection
        /// </summary>
        /// <param name="connection"></param>
        public RequestRepository(IConnection connection)
        {
            this._CassandraConnection = connection;
        }
        
        /// <summary>
        /// get requests objects
        /// </summary>
        /// <returns></returns>
        public List<Request> GetRequests()
        {
            var requests = new List<Request>();
            var cql = "SELECT * FROM requests;";
            var localSession = _CassandraConnection.Session;

            do
            {
                try
                {
                    var rs = localSession.Execute(cql);

                    foreach (var row in rs)
                    {
                        var e = row.GetValue<string>("email");
                        var b = row.GetValue<string>("snack_brand");
                        var n = row.GetValue<string>("snack_name");
                        var c = row.GetValue<int>("count");
                        
                        Request r = new Request(e, b, n , c);
                        
                        requests.Add(r);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            } while (localSession == null);

            return requests;
        }

        /// <summary>
        /// pass in a email to check that email's requests that have available
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<Request> GetRequestByEmail(string email)
        {
            var requests = new List<Request>();
            var cql = "SELECT * FROM requests WHERE email='" + email + "';";
            var localSession = _CassandraConnection.Session;

            do
            {
                try
                {
                    var rs = localSession.Execute(cql);

                    foreach (var row in rs)
                    {
                        var e = row.GetValue<string>("email");
                        var b = row.GetValue<string>("snack_brand");
                        var n = row.GetValue<string>("snack_name");
                        var c = row.GetValue<int>("count");
                        
                        Request r = new Request(e, b, n , c);
                        
                        requests.Add(r);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            } while (localSession == null);

            return requests;
        }
        
        /// <summary>
        /// Make a request
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string MakeRequest(Request request)
        {
            var countCQL = "SELECT * FROM snack_counts WHERE snack_brand='" + request._snack_brand +
                           "' AND snack_name='" + request._snack_name + "';";
            var requestCQL = "INSERT INTO snackapi.requests (email, snack_brand, snack_name, request_count)";
            var localSession = _CassandraConnection.Session;

            do
            {
                try
                {
                    var rs = localSession.Execute(countCQL);

                    if (rs.GetRows().First().GetValue<int>("request_count") == 0)
                    {
                        return "Insufficient";
                    }
                    
                    localSession.Execute(requestCQL);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            } while (localSession == null);

            return "Success";
        }
        
        
    }
}