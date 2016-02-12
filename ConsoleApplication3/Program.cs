using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            //DbConnect db = new DbConnect("Server=.\\MSSQL14;Database=DDC;User Id=test_ddc;Password = test3r;");
            DbOperations db = new DbOperations("data source=.\\mssql14;initial catalog=ddc;integrated security=false;user id=test_ddc;password=test3r;connect timeout=60000;encrypt=False;trustservercertificate=False;MultipleActiveResultSets=True;App=EntityFramework");
            Console.WriteLine(db.GetState());
            Console.WriteLine(db.CreateNewPeriod());
            Console.ReadLine();
            db.RemoveLastPeriod();            
        }

        class DbOperations
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
                    Console.WriteLine("Can't open connection");
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

            public string CreateNewPeriod()
            {
                SqlCommand sql = new SqlCommand("insert into ddc.ddc.period (id, StartDate, finishdate, name) " +
                                                "select id + 1, DATEADD(day, 1, FinishDate), dateadd(month, 3, FinishDate)," +
                                                "case when Name like '20[0-9][0-9] Q[1-3]' then substring(Name, 1, 6) + cast(cast(substring(Name, 7, 1) as int) + 1 as nvarchar)" +
                                                "when Name like '20[0-9][0-8] Q4' then substring(Name, 1, 3) + cast(cast(substring(Name, 4, 1) as int) + 1 as nvarchar) + ' Q1'" +
                                                "when Name like '20[0-9]9 Q4' then substring(Name, 1, 2) + cast(cast(substring(Name, 3, 1) as int) + 1 as nvarchar) + '0 Q1'" +
                                                "end as [Name] from(select top 1 * from ddc.ddc.Period order by id desc) t", this.GetConnection());
                sql.ExecuteNonQuery();
                sql = new SqlCommand("select top 1 id from ddc.ddc.Period order by id desc", this.GetConnection());
                var reader = sql.ExecuteReader();
                //Check the reader has data:
                if (reader.Read())
                {
                    return reader.GetValue(0).ToString();
                }
                else
                {
                    return "";
                }
            }

            public void RemoveLastPeriod()
            {
                SqlCommand sql = new SqlCommand("delete from ddc.ddc.Period where id in (select top 1 id from ddc.ddc.Period order by id desc)", this.GetConnection());
                sql.ExecuteNonQuery();
            }

            ~DbOperations()
            {
                try
                {
                    conn.Close();
                }
                catch { };
            }
        }
    }

    
}
