using System;
using Cassandra;

namespace Snackr
{
    public interface IConnection
    {
        Cluster Cluster { get; set; }
        ISession Session { get; set; }
        string Keyspace { get; set; }
        int Port { get; set; }
        Uri HostName { get; set; } 
        
        ISession Connect();
    }
}