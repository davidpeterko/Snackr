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

            /**
            cluster = Cluster.Builder()
                .WithCredentials("admin", "texas123")
                .AddContactPoint(this.contact)
                .WithPort(this.port)
                .Build();

            session = (Session) cluster.Connect(keyspace);

            Metadata metadata = cluster.Metadata;
            Console.Out.WriteLine("Name of the cluster: " + metadata.ClusterName); 
            Console.Out.WriteLine("Selected keysapce: " + metadata.GetKeyspace(keyspace).ToString());            
            Console.Out.WriteLine("Tables in the keyspace: " + metadata.GetTables(keyspace));
            **/
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