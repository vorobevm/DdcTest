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
            DDCRoutines ddc = new DDCRoutines(db);
            Console.WriteLine(db.GetState());
            Console.WriteLine(ddc.CreateNewPeriod());
            //db.RemoveLastPeriod();
            Console.WriteLine(ddc.CreateNewSupplier().Name);
               
            //db.RemoveSupplier();
            Console.WriteLine(ddc.CreateContract(ddc.GetSupplierId(), DateTime.Now, DateTime.Parse("2015/03/05"), DateTime.Parse("2016/03/05")));
            Console.ReadLine();
        } 
    }
}
