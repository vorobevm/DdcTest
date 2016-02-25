using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace ConsoleApplication3
{
    public class Period
    {
        public string Id;
        public DateTime StartDate;
        public DateTime FinishDate;
        public string Name;

        public Period(string id, DateTime startDate, DateTime finishDate, string name)
        {
            Id = id;
            StartDate = startDate;
            FinishDate = finishDate;
            Name = name;
        }
    }

    public class Supplier
    {
        public string Id;
        public string Title;
    }

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
                throw new Exception("Can't open a connection");
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
    }

    public static class DDCRoutinesStatic
    {

        /// <summary>
        /// Creates a new period in DDC.Period table. Will work till Q3 2099 inclusive.
        /// </summary>
        /// <returns>Period object</returns>
        public static Period CreateNewPeriod(DbOperations dbContext)
        {
            SqlCommand sql = new SqlCommand("insert into ddc.ddc.period (id, StartDate, finishdate, name) " +
                                            "select id + 1, DATEADD(day, 1, FinishDate), dateadd(month, 3, FinishDate)," +
                                            "case when Name like '20[0-9][0-9] Q[1-3]' then substring(Name, 1, 6) + cast(cast(substring(Name, 7, 1) as int) + 1 as nvarchar)" +
                                            "when Name like '20[0-9][0-8] Q4' then substring(Name, 1, 3) + cast(cast(substring(Name, 4, 1) as int) + 1 as nvarchar) + ' Q1'" +
                                            "when Name like '20[0-9]9 Q4' then substring(Name, 1, 2) + cast(cast(substring(Name, 3, 1) as int) + 1 as nvarchar) + '0 Q1'" +
                                            "end as [Name] from (select top 1 * from ddc.ddc.Period order by id desc) t", dbContext.GetConnection());
            sql.ExecuteNonQuery();
            sql = new SqlCommand("select top 1 id, startdate, finishdate, name from ddc.ddc.Period order by id desc", dbContext.GetConnection());
            var reader = sql.ExecuteReader();
            if (reader.Read())
            {
                Period newPeriod = new Period(reader.GetValue(0).ToString(), DateTime.Parse(reader.GetValue(1).ToString()), DateTime.Parse(reader.GetValue(2).ToString()), reader.GetValue(3).ToString());
                return newPeriod;
            }
            else
            {
                throw new Exception("Period wasn't created!");
            }
        }

        /// <summary>
        /// Creates a new period in DDC.Period table. Will work till Q3 2099 inclusive.
        /// Also fills the fields of this.newPeriod object.
        /// </summary>
        /// <returns>DDC.Period.id value</returns>
        public static string CreateNewPeriodGetId(DbOperations dbContext)
        {
            return CreateNewPeriod(dbContext).Id;
        }

        /// <summary>
        /// Removes period from DDC.Period table by id.
        /// </summary>
        public static void RemovePeriod(string id, DbOperations dbContext)
        {
            SqlCommand sql = new SqlCommand("delete from ddc.ddc.Period where id = " + id, dbContext.GetConnection());
            sql.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes last created period from DDC.Period table.
        /// </summary>
        public static void RemoveLastPeriod(DbOperations dbContext)
        {
            SqlCommand sql = new SqlCommand("delete from ddc.ddc.Period where id in (select top 1 id from ddc.ddc.Period order by id desc)", dbContext.GetConnection());
            sql.ExecuteNonQuery();
        }

        /// <summary>
        /// Creates a new supplier with random title in DDC.Supplier table. 
        /// Also fills the fields of this.newSupplier object.
        /// </summary>
        public static Supplier CreateNewSupplier(DbOperations dbContext)
        {
            Supplier newSupplier = new Supplier();
            newSupplier.Title = "TEST_" + RandomString(37);
            SqlCommand sql = new SqlCommand("insert into ddc.ddc.supplier (Title, IsVirtual) values ('" + newSupplier.Title + "',0)", dbContext.GetConnection());
            sql.ExecuteNonQuery();
            sql = new SqlCommand("select id from ddc.ddc.Supplier where Title = '" + newSupplier.Title + "' order by id desc", dbContext.GetConnection());
            var reader = sql.ExecuteReader();
            
            if (reader.Read())
            {
                newSupplier.Id = reader.GetValue(0).ToString();
            }
            else
            {
                throw new Exception("Supplier wasn't created!");
            }
            return newSupplier;
        }

        /// <summary>
        /// Remove supplier by Title. If no name specified - remove the supplier created during current session.
        /// </summary>
        /// <param name="supplierTitle">Name of supplier to be removed.</param>
        public static void RemoveSupplier(DbOperations dbContext, string supplierTitle)
        {
            SqlCommand sql = new SqlCommand("delete from ddc.ddc.supplier where Title = '" + supplierTitle + "'", dbContext.GetConnection());
            sql.ExecuteNonQuery();
        }

        /// <summary>
        /// Create new Contract using provided data. Output - DDC.Contract.Id.
        /// </summary>
        public static string CreateContract(DbOperations dbContext, string SupplierId, DateTime SignDate, DateTime StartDate, DateTime FinishDate)
        {
            Random rnd = new Random();
            string ContractNumber = "TEST_" + RandomString(4) + rnd.Next().ToString();
            SqlCommand sql = new SqlCommand("insert into ddc.ddc.Contract(ContractNumber, ContractSignDate, StartDate, FinishDate, CreateDate, CreateUser, EditDate, EditUser, SupplierId)" +
            "Values('" + ContractNumber + "', cast('" + SignDate.ToString() + "' as Datetime), cast('" + StartDate.ToString() + "' as Datetime), cast('" + FinishDate.ToString() + "' as Datetime), SYSDATETIME(), 'TEST', SYSDATETIME(), 'TEST', " + SupplierId + ")", dbContext.GetConnection());
            sql.ExecuteNonQuery();
            sql = new SqlCommand("select id from ddc.ddc.Contract where ContractNumber = '" + ContractNumber + "'", dbContext.GetConnection());
            var reader = sql.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetValue(0).ToString();
            }
            else
            {
                throw new Exception("Supplier wasn't created or already deleted.");
            }
        }

        /// <summary>
        /// Remove Contract by DDC.Contract.Id
        /// </summary>
        public static void RemoveContractById(DbOperations dbContext, string ContractId)
        {
            SqlCommand sql = new SqlCommand("delete from ddc.ddc.Contract where Id=" + ContractId + ")", dbContext.GetConnection());
            sql.ExecuteNonQuery();
        }

        /// <summary>
        /// Create new Condition using provided data. Output - DDC.Condition.Id.
        /// </summary>
        public static string CreateCondition(DbOperations dbContext, DateTime StartDate, DateTime FinishDate, string ContractId, DateTime CreateDate)
        {
            SqlCommand sql = new SqlCommand("insert into ddc.ddc.condition (StartDate, FinishDate, AmountPercent, AmountQty, ContractId, CreateDate, CreateUser, EditDate, EditUser, Status, AllBrandsSelected, AllProductsSelected, BusinessDomainId, IsDeleted)" +
            "Values(cast('" + StartDate.ToString() + "' as Datetime), cast('" + FinishDate.ToString() + "' as Datetime), 100, 1, " + ContractId + ", SYSDATETIME(), 'TEST', SYSDATETIME(), 'TEST', 1, 0, 0, 2, 0)", dbContext.GetConnection());
            sql.ExecuteNonQuery();
            sql = new SqlCommand("select id from ddc.ddc.Condition WHERE " +
                "StartDate = cast('" + StartDate.ToString() + "' as Datetime) AND " +
                "FinishDate = cast('" + FinishDate.ToString() + "' as Datetime) AND " +
                "ContractId = " + ContractId, dbContext.GetConnection());
            var reader = sql.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetValue(0).ToString();
            }
            else
            {
                throw new Exception("Condition wasn't created or already deleted.");
            }
        }

        /// <summary>
        /// Remove Condition by DDC.Condition.Id
        /// </summary>
        public static void RemoveConditionById(DbOperations dbContext, string ConditionId)
        {
            SqlCommand sql = new SqlCommand("delete ddc.ddc.Condition where id = '" + ConditionId + "'", dbContext.GetConnection());
            sql.ExecuteNonQuery();
        }

        /// <summary>
        /// Enumeration of possible Rule types in DDC.RuleOfCalculating table.
        /// </summary>
        public enum RuleType
        {
            Article = 1,
            ProductGroup = 2,
            Department = 4,
            BusinessDomain = 8,
            Brand = 16,
            Distributor = 32,
            InvoiceRecipient = 96
        }

        /// <summary>
        /// Base method for creating a row in RuleOfCalculating table
        /// </summary>
        /// <param name="Type">Taken from RuleType enumeration</param>
        /// <param name="ConditionId">DDC.Condition.id value</param>
        /// <param name="LinkedEntityId"></param>
        /// <returns>DDC.RuleOfCalculation.id</returns>
        private static string CreateRuleOfCalc(DbOperations dbContext, RuleType Type, string ConditionId, string LinkedEntityId)
        {
            SqlCommand sql = new SqlCommand("insert into ddc.ddc.RuleOfCalculating (Type, ConditionId,LinkedEntityId)" +
           "Values(" + (int)Type + "," + ConditionId + "," + LinkedEntityId + ")", dbContext.GetConnection());
            sql.ExecuteNonQuery();
            sql = new SqlCommand("select top 1 id from ddc.ddc.RuleOfCalculating order by id desc", dbContext.GetConnection());
            var reader = sql.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetValue(0).ToString();
            }
            else
            {
                throw new Exception("Rule wasn't created.");
            }
        }

        /// <summary>
        /// Remove DDC.RuleOfCalculating by Type, ConditionId, LinkedEntityId
        /// </summary>
        public static void RemoveRuleOfCalc(DbOperations dbContext, RuleType Type, string ConditionId, string LinkedEntityId)
        {
            SqlCommand sql = new SqlCommand("delete from ddc.ddc.RuleOfCalculating where" +
                "Type = " + Type.ToString() +
                " AND ConditionId = " + ConditionId +
                " AND LinkedEntityId = " + LinkedEntityId, dbContext.GetConnection());
            sql.ExecuteNonQuery();
        }

        /// <summary>
        /// Remove DDC.RuleOfCalculating by Id
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveRuleOfCalc(DbOperations dbContext, string id)
        {
            SqlCommand sql = new SqlCommand("delete from ddc.ddc.RuleOfCalculating where Id = " + id, dbContext.GetConnection());
            sql.ExecuteNonQuery();
        }

        public static string CreateRuleOfCalcArticle(DbOperations dbContext, string ConditionId, string ArticleID)
        {
            return CreateRuleOfCalc(dbContext, RuleType.Article, ConditionId, ArticleID);
        }

        public static string CreateRuleOfCalcProductGroup(DbOperations dbContext, string ConditionId, string ProductGroupId)
        {
            return CreateRuleOfCalc(dbContext, RuleType.ProductGroup, ConditionId, ProductGroupId);
        }

        public static string CreateRuleOfCalcDepartment(DbOperations dbContext, string ConditionId, string DepartmentId)
        {
            return CreateRuleOfCalc(dbContext, RuleType.Department, ConditionId, DepartmentId);
        }

        public static string CreateRuleOfCalcBusinessDomain(DbOperations dbContext, string ConditionId, string BusinessDomainId)
        {
            return CreateRuleOfCalc(dbContext, RuleType.BusinessDomain, ConditionId, BusinessDomainId);
        }

        public static string CreateRuleOfCalcBrand(DbOperations dbContext, string ConditionId, string BrandId)
        {
            return CreateRuleOfCalc(dbContext, RuleType.Brand, ConditionId, BrandId);
        }

        public static string CreateRuleOfCalcDistributor(DbOperations dbContext, string ConditionId, string SupplierId)
        {
            return CreateRuleOfCalc(dbContext, RuleType.Distributor, ConditionId, SupplierId);
        }

        public static string CreateRuleOfCalcInvoiceRecipient(DbOperations dbContext, string ConditionId, string InvoiceRecipientId)
        {
            return CreateRuleOfCalc(dbContext, RuleType.InvoiceRecipient, ConditionId, InvoiceRecipientId);
        }

        /// <summary>
        /// Method to create a record in DDC.GoodsRecord table
        /// </summary>
        /// <param name="SAP_CODE">Store SAP_CODE</param>
        /// <param name="POS_NO"></param>
        /// <param name="BUCH_NO"></param>
        /// <param name="PREIS">Price</param>
//TODO Make sure to use correct BUCH_SUB_TYP for arrivals and returns
        /// <param name="BUCH_SUB_TYP"></param>
        /// <param name="ART_NO">Article number</param>
        /// <param name="WARENEINGANG">Date of operation</param>
        /// <param name="MENGE">Number of articles</param>
        /// <param name="SupplierId"></param>
        /// <param name="BESTELL_NO"></param>
        /// <param name="BS_POS"></param>
        /// <param name="ROWVER"></param>
        /// <param name="WAEHRUNG">Currency in 3-char ISO code</param>
        /// <returns></returns>
        public static void CreateGoodsRecord(DbOperations dbContext, string SAP_CODE, string POS_NO, string BUCH_NO, string PREIS, string BUCH_SUB_TYP, string ART_NO, DateTime WARENEINGANG, string MENGE, string SupplierId, string BESTELL_NO, string BS_POS, string ROWVER, string WAEHRUNG = "SEK")
        {
            SqlCommand sql = new SqlCommand("insert into ddc.ddc.GoodsRecord (SAP_CODE, POS_NO, BUCH_NO, PREIS, BUCH_SUB_TYP, ART_NO, WARENEINGANG, MENGE, LIEF_NO, BESTELL_NO, BS_POS, ROWVER, WAEHRUNG)" + " Values('" + SAP_CODE +
                "'," + POS_NO + "," +
                BUCH_NO + "," +
                PREIS + ", '" +
                BUCH_SUB_TYP + "'," +
                ART_NO + "," +
                "cast ('" + WARENEINGANG.ToString() + "' as Date)," +
                MENGE + "," +
                SupplierId + "," +
                BESTELL_NO + "," +
                BS_POS + ",'" +
                ROWVER + "', '" +
                WAEHRUNG + "')", dbContext.GetConnection());
            sql.ExecuteNonQuery();
        }

        /// <summary>
        /// Check expected test result in DDC.PeriodCalculation table
        /// </summary>
        /// <param name="PeriodId"></param>
        /// <param name="ConditionId"></param>
        /// <param name="Status"></param>
        /// <param name="Discount"></param>
        /// <param name="SAP_CODE">Store SAP_CODE</param>
        /// <param name="ART_NO">Article number</param>
        /// <param name="SupplierId"></param>
        /// <returns></returns>
        public static Boolean CheckExpectedPeriodCalc(DbOperations dbContext, string PeriodId, string ConditionId, string Status, string Discount, string SAP_CODE, string ART_NO, string SupplierId)
        {
            // TODO Need to be tested on real data. Maybe, not count(*) should be used to check the results.
            SqlCommand sql = new SqlCommand("select count(*) from ddc.ddc.PeriodCalculation" +
                " where PeriodId=" + PeriodId +
                " AND ConditionId=" + ConditionId +
                " AND Status=" + Status +
                " AND Discount=" + Discount +
                " AND SAP_CODE='" + SAP_CODE + "'" +
                " AND ART_NO =" + ART_NO +
                " AND LIEF_NO='" + SupplierId + "'", dbContext.GetConnection());
            var reader = sql.ExecuteReader();
            if (reader.Read())
            {
                if (Int32.Parse(reader.GetValue(0).ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                throw new Exception();
            }

        }

        /// <summary>
        /// Get (new) article number that doesn't exist in GoodsRecord.ART_NO.
        /// </summary>
        public static string GetNewArticle(DbOperations dbContext)
        {
            Random rnd = new Random();
            int newArt;
            SqlDataReader reader;
            do
            {
                newArt = rnd.Next(10000, 999999);
                SqlCommand sql = new SqlCommand("select * from ddc.ddc.GoodsRecord" +
               " where ART_NO = " + newArt.ToString(), dbContext.GetConnection());
                reader = sql.ExecuteReader();
            }
            while (reader.HasRows);
            return newArt.ToString();
        }

        /// <summary>
        /// Get (new) store SAP_CODE that doesn't exist in GoodsRecord.SAP_CODE.
        /// </summary>
        public static string GetNewStore(DbOperations dbContext)
        {
            Random rnd = new Random();
            SqlDataReader reader;
            string store;
            char ch;
            do
            {
                store = RandomString(1) + rnd.Next(100, 999).ToString();
                SqlCommand sql = new SqlCommand("select * from ddc.ddc.GoodsRecord" +
               " where SAP_CODE = '" + store + "'", dbContext.GetConnection());
                reader = sql.ExecuteReader();
            }
            while (reader.HasRows);
            return store;
        }

        /// <summary>
        /// Get (new) store SAP_CODE that doesn't exist in GoodsRecord.SAP_CODE and not equal to provided ExcludedStore code
        /// </summary>
        /// <param name="ExcludedStore"></param>
        /// <returns></returns>
        public static string GetNewStore(DbOperations dbContext, string ExcludedStore)
        {
            string store;
            do
            {
                store = GetNewStore(dbContext);
            }
            while (store == ExcludedStore);
            return store;
        }

        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}
