﻿using System;
using Cassandra;

namespace Snackr
{
    public interface IConnection
    {
        Cluster Cluster { get; set; }
        ISession Session { get; set; }
        string Keyspace { get; set; }
        int Port { get; set; }
        string HostName { get; set; } 
        
        ISession Connect();
    }
}