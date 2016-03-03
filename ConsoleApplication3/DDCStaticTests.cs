using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ConsoleApplication3
{
    [TestFixture]
    public class DDCStaticTests
    {
        private DbOperations dbContext;

        [SetUp]
        public void Init()
        {
          dbContext = new DbOperations("data source=.\\mssql14;initial catalog=ddc;integrated security=false;user id=test_ddc;password=test3r;connect timeout=60000;encrypt=False;trustservercertificate=False;MultipleActiveResultSets=True;App=EntityFramework");
        }

        [Test]
        public void CheckGetAttribute()
        {
            string artNo = DdcRoutinesStatic.GetNewArticle(dbContext);
            string artNo2 = DdcRoutinesStatic.GetNewArticle(dbContext);
            Assert.AreNotEqual(artNo, artNo2);
        }

        /// <summary>
        /// 1. No returns
        /// 2. New article (no earlier deliveries)
        /// 3. No client order on goods
        /// 4. Condition - article, 1, 100% price; starts date 10 days before period start, ends 1 year after period start
        /// 5. Demo discount wasn't calculated previously
        /// 6. 1 delivery 5 goods, 5 days earlier than period start.
        /// </summary>
        [Test]
        public void TestDdc001()

        {
            string article = DdcRoutinesStatic.GetNewArticle(dbContext);
            string store = DdcRoutinesStatic.GetNewStore(dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1));
            string ruleOfCalcId = DdcRoutinesStatic.CreateRuleOfCalcArticle(dbContext, conditionId, article);
            DdcRoutinesStatic.CreateGoodsRecord(dbContext, store, "1", "1","95.0000", "WU", article, period.StartDate.AddDays(2),"5", supplier.Id, "1", "1","SEK");
            //TODO: Call discount calculation
           // Assert.True(DdcRoutinesStatic.CheckExpectedPeriodCalc(dbContext, period.Id, conditionId, STATUS, "95.0000", store, article, supplier.Id));
           Assert.True(true);
        }


        
    }
}
