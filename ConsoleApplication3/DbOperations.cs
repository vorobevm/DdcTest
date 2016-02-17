using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ConsoleApplication3
{
    public class DbOperations
    {
        private SqlConnection conn;

        public DbOperations(string ConnectionString)
        {
            conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
            }
            catch
            {
                Console.WriteLine("Can't open a connection");
            }

        }

        public string GetState()
        {
            return conn.State.ToString();
        }

        public SqlConnection GetConnection()
        {
            return conn;
        }



        ~DbOperations()
        {
            // try
            // {

            //conn.Close();
            //}
            //catch { };
        }
    }
}
