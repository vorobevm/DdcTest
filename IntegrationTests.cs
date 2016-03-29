using System;
using System.Configuration;
using DDC.Autotests.Framework;
using NUnit.Framework;
using RestSharp;

namespace DDC.Autotests
{
    class IntegrationTests
    {
        private readonly RestClient _restClient;
        private readonly DbOperations _dbContext;

        public IntegrationTests()
        {
            _restClient = new RestClient(ConfigurationManager.AppSettings["ApiUrl"]);
            _dbContext = new DbOperations(ConfigurationManager.ConnectionStrings["db"].ConnectionString);
        }

        //[SetUp]
        //public void TestInit()
        //{
           
        //}

        [Test]
        public void Test001()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "",1,1,1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1),true, true);
            DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateRuleOfCalcDepartment(_dbContext, conditionId, "3");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(-2), "5", supplier.Id, "1", "1", "SEK");
            var client = new ServiceReference1.DataServiceClient();
            
            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] {Int32.Parse(supplier.Id)}, true, false));
            
            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.True(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, conditionId, "1", "95.0000", store, article, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }

        [Test]
        public void Test002()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "", 1, 1, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateRuleOfCalcDepartment(_dbContext, conditionId, "3");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(-2), "1", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "97.5000", "WU", article, period.StartDate.AddDays(-2), "4", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "IU", article, period.StartDate.AddDays(2), "1", supplier.Id, "1", "1", "SEK");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.True(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, conditionId, "1", "97.5000", store, article, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }

        [Test]
        public void Test003()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "", 1, 1, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateRuleOfCalcDepartment(_dbContext, conditionId, "3");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(-2), "2", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "96.0000", "WU", article, period.StartDate.AddDays(-2), "20", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "IU", article, period.StartDate.AddDays(1), "2", supplier.Id, "1", "1", "SEK");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.True(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, conditionId, "1", "96.0000", store, article, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }

        [Test]
        public void Test004()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "", 1, 1, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateRuleOfCalcDepartment(_dbContext, conditionId, "3");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(-2), "5", supplier.Id, "111", "12", "SEK");
            DdcRoutinesStatic.CreateCustomerOrder(_dbContext, store, "111", "12", article, supplier.Id, "4");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.True(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, conditionId, "1", "95.0000", store, article, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }

        [Test]
        public void Test005()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "", 1, 12345, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            //DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcProductGroup(_dbContext, conditionId, "12345");
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            //DdcRoutinesStatic.CreateRuleOfCalcDepartment(_dbContext, conditionId, "3");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(-2), "1", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "97.5000", "WU", article, period.StartDate.AddDays(-2), "4", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "IU", article, period.StartDate.AddDays(1), "1", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateCustomerOrder(_dbContext, store, "111", "12", article, supplier.Id, "3");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.True(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, conditionId, "1", "97.5000", store, article, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }

        [Test]
        public void Test006()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "", 3, 1, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            //DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateRuleOfCalcBusinessDomain(_dbContext, conditionId, "1");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(2), "2", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "96.0000", "WU", article, period.StartDate.AddDays(2), "20", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "IU", article, period.StartDate.AddDays(3), "2", supplier.Id, "1", "1", "SEK");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.True(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, conditionId, "1", "96.0000", store, article, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }

        [Test]
        public void Test007()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext,"", 3,12345, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            //DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcProductGroup(_dbContext, conditionId, "12345");
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateRuleOfCalcBusinessDomain(_dbContext, conditionId, "1");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(2), "2", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "96.0000", "WU", article, period.StartDate.AddDays(2), "20", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "IU", article, period.StartDate.AddDays(3), "2", supplier.Id, "1", "1", "SEK");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.True(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, conditionId, "1", "96.0000", store, article, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }
        
        [Test]
        public void Test008()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "", 1, 1, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(-2), "1", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "IU", article, period.StartDate.AddDays(-1), "1", supplier.Id, "1", "1", "SEK");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.False(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }

        [Test]
        public void Test009()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "", 1, 1, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(-2), "4", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "IU", article, period.StartDate.AddDays(-1), "2", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "IU", article, period.StartDate.AddDays(1), "2", supplier.Id, "1", "1", "SEK");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.False(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }

        [Test]
        public void Test010()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "", 1, 1, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddYears(-2), "1", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(-2), "4", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "IU", article, period.StartDate.AddDays(-1), "1", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "IU", article, period.StartDate.AddDays(1), "1", supplier.Id, "1", "1", "SEK");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.False(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }

        [Test]
        public void Test011()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "", 1, 1, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Supplier supplier2 = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcInvoiceRecipient(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier2.Id);
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddYears(-2), "1", supplier2.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(-2), "4", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "IU", article, period.StartDate.AddDays(-1), "1", supplier.Id, "1", "1", "SEK");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "IU", article, period.StartDate.AddDays(1), "1", supplier.Id, "1", "1", "SEK");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.False(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
            DdcRoutinesStatic.RemoveSupplier(_dbContext, supplier2.Title);
        }

        [Test]
        public void Test012()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "", 3, 1, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            //DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateRuleOfCalcBusinessDomain(_dbContext, conditionId, "1");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(1), "4", supplier.Id, "555", "22", "SEK");
            DdcRoutinesStatic.CreateCustomerOrder(_dbContext, store, "555", "22", article, supplier.Id, "4");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.False(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }

        [Test]
        public void Test013()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "", 3, 1, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            DdcRoutinesStatic.CreateRuleOfCalcArticle(_dbContext, conditionId, article);
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "90.0000", "WU", article, period.StartDate.AddDays(1), "4", supplier.Id, "555", "22", "SEK");
            DdcRoutinesStatic.CreateCustomerOrder(_dbContext, store, "666", "22", article, supplier.Id, "3");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "90.0000", "WU", article, period.StartDate.AddDays(2), "3", supplier.Id, "666", "22", "SEK");
            DdcRoutinesStatic.CreateCustomerOrder(_dbContext, store, "555", "22", article, supplier.Id, "4");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.False(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }

        [Test]
        public void Test014()
        {
            string article = DdcRoutinesStatic.CreateNewProduct(_dbContext, "", 1, 1, 1);
            string store = DdcRoutinesStatic.GetNewStore(_dbContext);
            Supplier supplier = DdcRoutinesStatic.CreateNewSupplier(_dbContext);
            Period period = DdcRoutinesStatic.CreateNewPeriod(_dbContext);
            string conractId = DdcRoutinesStatic.CreateContract(_dbContext, supplier.Id, period.StartDate.AddDays(-10),
                period.StartDate.AddDays(-10), period.StartDate.AddYears(1));
            string conditionId = DdcRoutinesStatic.CreateCondition(_dbContext, period.StartDate.AddDays(-5), period.StartDate.AddYears(1),
                conractId, period.StartDate.AddDays(1), true, true);
            DdcRoutinesStatic.CreateRuleOfCalcDistributor(_dbContext, conditionId, supplier.Id);
            DdcRoutinesStatic.CreateRuleOfCalcDepartment(_dbContext, conditionId, "3");
            DdcRoutinesStatic.CreateGoodsRecord(_dbContext, store, "1", "1", "95.0000", "WU", article, period.StartDate.AddDays(1), "5", supplier.Id, "1", "1", "SEK");
            var client = new ServiceReference1.DataServiceClient();

            Assert.DoesNotThrow(() => client.CumulativeDiscount(Int32.Parse(period.Id), new[] { Int32.Parse(supplier.Id) }, true, false));

            //var response = _restClient.Post<object>(
            //    new RestRequest("discount/updateperiod", Method.POST)
            //    .AddHeader("Content-Type", "application/json")
            //    .AddJsonBody(new
            //    {
            //        PeriodId = period.Id,
            //        SupplierIds = new[] {supplier.Id}
            //    }));
            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.False(DdcRoutinesStatic.CheckExpectedPeriodCalc(_dbContext, period.Id, supplier.Id));
            DdcRoutinesStatic.CleanUp(_dbContext, period.Id, article, conractId, store, supplier.Title);
        }

        
    }
}
