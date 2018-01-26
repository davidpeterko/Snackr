using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Snackr.Interfaces;
using Snackr.DataLayer;

namespace Snackr.Repositories
{
    public class UserRepository
    {
        private readonly IConnection _CassandraConnection;

        /// <summary>
        /// constructor to set the cassandra connection, constructor injection
        /// </summary>
        public UserRepository(IConnection connection)
        {
            this._CassandraConnection = connection;
        }

        public User GetUserPermissions(string email)
        {
            var user = new User();
            var cql = "SELECT * FROM users WHERE email='" + email + "';";
            var localSession = _CassandraConnection.Session;

            do
            {
                try
                {
                    var rs = localSession.Execute(cql);
                    var row = rs.First();
                    
                    user._Email = row.GetValue<string>("email");
                    user._Permission = row.GetValue<string>("permission");
                    user._Name = row.GetValue<string>("name");
                    user._TakoKoinCount = row.GetValue<int>("takokoincount");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            } while (localSession == null);

            return user;
        }
    }
}