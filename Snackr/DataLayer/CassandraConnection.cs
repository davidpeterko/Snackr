using System;
using Auth0.ManagementApi.Models;
using Cassandra;
using Microsoft.Extensions.Configuration;

namespace Snackr.Interfaces
{
    public class CassandraConnection : IConnection
    {
        public Cluster Cluster { get; set; }
        public ISession Session { get; set; }
        public string Keyspace { get; set; }
        public int Port { get; set; }
        public string HostName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        private IConfiguration _configuration;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keyspace"></param>
        /// <param name="port"></param>
        /// <param name="host"></param>
        public CassandraConnection(string keyspace, string host, int port, string user, string password)
        {
            this.Keyspace = keyspace;
            this.HostName = host;
            this.Port = port;
            this.User = user;
            this.Password = password;

            this.Session = Connect();
        }
       
        /// <summary>
        /// Connect to the cluster
        /// </summary>
        public ISession Connect()
        {
            Cluster = Cluster.Builder().AddContactPoint(HostName).WithPort(Port)
                .WithCredentials(User, Password).Build(); 
            
            return Cluster.Connect(Keyspace);
        }

    }
}