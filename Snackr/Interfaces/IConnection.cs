using System;
using Cassandra;

namespace Snackr.Interfaces
{
    public interface IConnection
    {
        Cluster Cluster { get; set; }
        ISession Session { get; set; }
        string Keyspace { get; set; }
        int Port { get; set; }
        string HostName { get; set; }
        string User { get; set; }
        string Password { get; set; }
        
        ISession Connect();
    }
}