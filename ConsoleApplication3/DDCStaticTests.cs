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

        [Test]
        public void RemPer(
            [Values(31,32,33,34,35,36,37,38,39,40,41,42,43,44,45)] int perId)
        {
            
            DdcRoutinesStatic.RemovePeriod(dbContext, perId.ToString());
            Assert.True(true);
        }


        
    }
}
