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
            //Console.WriteLine(db.GetState());

            string store = ddc.GetNewStore("");
            string article = ddc.GetNewArticle();
            string period = ddc.CreateNewPeriod().Id;
            DDCRoutines.Supplier supp = ddc.CreateNewSupplier();
            string contractId = ddc.CreateContract(supp.Id, DateTime.Now, ddc.newPeriod.StartDate.AddDays(5), DateTime.Now.AddYears(1));
            string conditionId = ddc.CreateCondition(ddc.newPeriod.StartDate.AddDays(10), ddc.newPeriod.FinishDate, contractId, DateTime.Now);
            ddc.CreateRuleOfCalcArticle(conditionId, article);
            ddc.CreateGoodsRecord(store, "1", "1", "100", "WU", article, ddc.newPeriod.StartDate.AddDays(20), "5", supp.Id, "1", "1", "1", "SEK");
            
            

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

            ddc.RemoveLastPeriod();
        } 
    }
}
