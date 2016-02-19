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
            ddc.CreateNewSupplier();
            Console.WriteLine(ddc.GetSupplierName());
            //db.RemoveSupplier();
            string contract = ddc.CreateContract(ddc.GetSupplierId(), DateTime.Now, DateTime.Parse("2015/03/05"), DateTime.Parse("2016/03/05"));
            Console.WriteLine("New contract: "+contract);
            string condition = ddc.CreateCondition(DateTime.Now, DateTime.Now.AddYears(1), contract, DateTime.Now);
            Console.WriteLine("New condition: " + condition);
            ddc.RemoveCondition(condition);
            ddc.CreateGoodsRecord("W123", "1", "1", "255.0000", "WU", "123456", DateTime.Now, "1", "100005", "123", "567", ddc.RandomString(10), ddc.RandomString(3));
            bool result = ddc.CheckExpectedPeriodCalc("27", "23", "8", "75", "W001", "1182610", "7500005");
            if (result)
            {
                Console.WriteLine("Check OK");
            }
            else
            {
                Console.WriteLine("Check Fail");
            }
            Console.ReadLine();
        } 
    }
}
