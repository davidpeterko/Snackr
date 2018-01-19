using System;
using Cassandra;
using System.Collections.Generic;

namespace Snackr.Interfaces
{
    public class SnackrBackend
    {
        private readonly IConnection _CassandraConnection;

        /// <summary>
        /// constructor to set the cassandra connection, constructor injection
        /// </summary>
        public SnackrBackend(IConnection c)
        {
            this._CassandraConnection = c;
        }
        
        /// <summary>
        /// get snacks objects
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        public List<Snack> GetSnacks()
        {
            var snacks = new List<Snack>();
            var cql = "SELECT * FROM snack_counts;";
            var localSession = _CassandraConnection.Session;

            do
            {
                try
                {
                    var rs = localSession.Execute(cql);
                    
                    foreach (var row in rs)
                    {
                        var b = row.GetValue<string>("snack_brand");
                        var n = row.GetValue<string>("snack_name");
                        var c = row.GetValue<int>("snack_count");
                        
                        Snack s = new Snack(b, n, c);
                        
                        snacks.Add(s);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            } while (localSession == null);

            return snacks;
        }
        
        /// <summary>
        /// get list of snack brands
        /// </summary>
        /// <returns></returns>
        public List<string> GetSnackBrandsList()
        {
            var brands = new List<string>();
            var cql = "SELECT snack_brand FROM snack_counts;";
            var localSession = _CassandraConnection.Session;

            do
            {
                try
                {
                    var rs = localSession.Execute(cql);

                    foreach (var row in rs)
                    {
                        brands.Add(row["snack_brand"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            } while (localSession == null);

            return brands;
        }

        /// <summary>
        /// get list of snacks based on snack band
        /// </summary>
        /// <returns></returns>
        public List<string> GetSnackNameList(string brand)
        {
            var names = new List<string>();
            var cql = "SELECT snack_name FROM snack_counts WHERE snack_brand='" + brand + "';";
            var localSession = _CassandraConnection.Session;

            do
            {
                try
                {
                    var rs = localSession.Execute(cql);

                    foreach (var row in rs)
                    {
                        names.Add(row["snack_name"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            } while (localSession == null);

            return names;
        }

        /// <summary>
        /// total number of snacks
        /// </summary>
        /// <returns></returns>
        public int GetTotalSnackCount()
        {
            var count = 0;
            var cql = "SELECT snack_count FROM snack_counts;";
            var localSession = _CassandraConnection.Session;

            do
            {
                try
                {
                    var rs = localSession.Execute(cql);

                    foreach (var row in rs)
                    {
                        count += row.GetValue<int>("snack_count");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            } while (localSession == null);

            return count;
        }
        
        
    }
}