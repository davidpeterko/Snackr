using System;
using Cassandra;

namespace Snackr
{
    public class CassandraConnection : IConnection
    {
        public Cluster Cluster { get; set; }
        public ISession Session { get; set; }
        public string Keyspace { get; set; }
        public int Port { get; set; }
        public string HostName { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keyspace"></param>
        /// <param name="port"></param>
        /// <param name="host"></param>
        public CassandraConnection(string keyspace, string host, int port)
        {
            this.Keyspace = keyspace;
            this.HostName = host;
            this.Port = port;

            this.Session = Connect();
        }
       
        /// <summary>
        /// Connect to the cluster
        /// </summary>
        public ISession Connect()
        {
            this.Cluster = Cluster.Builder().AddContactPoint(this.HostName.ToString()).WithPort(this.Port)
                .WithCredentials("admin", "texas123").Build(); 
            
            return Cluster.Connect(this.Keyspace);
        }

    }
}