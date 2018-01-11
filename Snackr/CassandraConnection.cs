using Cassandra;

namespace Snackr
{
    public class CassandraConnection
    {
        private Cluster cluster;
        private ISession session;
        private readonly string contact;
        private readonly int port;
        private readonly string keyspace;

        public CassandraConnection()
        {
            contact = "10.11.110.145";
            port = 9042;
            keyspace = "snackapi";
        }
       
        /// <summary>
        /// Connect to the cluster
        /// </summary>
        public void Connect()
        {
            cluster = Cluster.Builder().AddContactPoint(contact).WithPort(port)
                .WithCredentials("admin", "texas123").Build();

            session = cluster.Connect(keyspace);
        }

        /// <summary>
        /// gets the current session
        /// </summary>
        /// <returns></returns>
        protected ISession getSession()
        {
            if (session == null)
            {
                Connect();                
            }

            return session;
        }
    }
}