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
            var localSession = _CassandraConnection.Session;

            do
            {
                try
                {
                    var statement =
                        localSession.Prepare(
                            "SELECT * FROM snack_counts WHERE snack_brand = :snack_brand AND snack_name = :snack_name;");
                    
                    var rs = localSession.Execute(statement.Bind(new {snack_brand=request._snack_brand, snack_name=request._snack_name}));

                    var currentCount = rs.GetRows().First().GetValue<int>("snack_count");
                    if (currentCount == 0)
                    {
                        return "Insufficient";
                    }
                    
                    // Check if theres already a snack request available, else just use the current passed request count since theres no request
                    statement = localSession.Prepare(
                        "SELECT * FROM requests WHERE email = :email AND snack_brand = :snack_brand AND snack_name = :snack_name;");

                    rs = localSession.Execute(statement.Bind(new {email = request._email}));

                    // If any requests available for this snack, just increment the value
                    if (rs.GetRows().Any())
                    {
                        var existingCount = rs.GetRows().First().GetValue<int>("snack_count");
                        
                        statement = localSession.Prepare(
                            "INSERT INTO snackapi.requests (email, snack_brand, snack_name, count) VALUES (:email, :snack_brand, :snack_name, :count);");

                        localSession.Execute(statement.Bind(new
                        {
                            email = request._email,
                            snack_brand = request._snack_brand,
                            snack_name = request._snack_name,
                            count = existingCount + 1
                        }));
                    }
                    else
                    {
                        statement = localSession.Prepare(
                            "INSERT INTO snackapi.requests (email, snack_brand, snack_name, count) VALUES (:email, :snack_brand, :snack_name, :count);");

                        localSession.Execute(statement.Bind(new
                        {
                            email = request._email,
                            snack_brand = request._snack_brand,
                            snack_name = request._snack_name,
                            count = request._request_count
                        }));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            } while (localSession == null);

            return "Success";
        }

        /// <summary>
        /// Decrement count of a snack
        /// </summary>
        /// <param name="request"></param>
        public void DecrementCount(Request request)
        {   
            var localSession = _CassandraConnection.Session;

            do
            {
                try
                {
                    var statement =
                        localSession.Prepare(
                            "SELECT * FROM snack_counts WHERE snack_brand = :snack_brand AND snack_name = :snack_name;");
                    
                    var rs = localSession.Execute(statement.Bind(new {snack_brand=request._snack_brand, snack_name=request._snack_name}));

                    var currentCount = rs.GetRows().First().GetValue<int>("snack_count");
                    
                    // Decrement count in db for snack_count if request was able to process
                    statement = localSession.Prepare(
                        "UPDATE snack_counts SET snack_count = :snack_count WHERE snack_brand = :snack_brand AND snack_name = :snack_name;");

                    localSession.Execute(statement.Bind(new
                    {
                        snack_count = currentCount - 1,
                        snack_brand = request._snack_brand,
                        snack_name = request._snack_name
                    }));

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            } while (localSession == null);
        }
        
        
    }
}