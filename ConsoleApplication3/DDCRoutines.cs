using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ConsoleApplication3
{
    public class DDCRoutines
    {
        DbOperations dbContext;
        Supplier newSupplier;

        public DDCRoutines(DbOperations db)
        {
            dbContext = db;
            newSupplier = new Supplier();
        }

        public string CreateNewPeriod()
        {
            SqlCommand sql = new SqlCommand("insert into ddc.ddc.period (id, StartDate, finishdate, name) " +
                                            "select id + 1, DATEADD(day, 1, FinishDate), dateadd(month, 3, FinishDate)," +
                                            "case when Name like '20[0-9][0-9] Q[1-3]' then substring(Name, 1, 6) + cast(cast(substring(Name, 7, 1) as int) + 1 as nvarchar)" +
                                            "when Name like '20[0-9][0-8] Q4' then substring(Name, 1, 3) + cast(cast(substring(Name, 4, 1) as int) + 1 as nvarchar) + ' Q1'" +
                                            "when Name like '20[0-9]9 Q4' then substring(Name, 1, 2) + cast(cast(substring(Name, 3, 1) as int) + 1 as nvarchar) + '0 Q1'" +
                                            "end as [Name] from(select top 1 * from ddc.ddc.Period order by id desc) t", dbContext.GetConnection());
            sql.ExecuteNonQuery();
            sql = new SqlCommand("select top 1 id from ddc.ddc.Period order by id desc", dbContext.GetConnection());
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
            SqlCommand sql = new SqlCommand("delete from ddc.ddc.Period where id in (select top 1 id from ddc.ddc.Period order by id desc)", dbContext.GetConnection());
            sql.ExecuteNonQuery();
        }

        public class Supplier
        {
            public string Id;
            public string Name;
        }

        /// <summary>
        /// Creates a new supplier with random name
        /// </summary>
        /// <returns>The name of created supplier.</returns>
        public Supplier CreateNewSupplier()
        {
            this.newSupplier.Name = RandomString(42);
            SqlCommand sql = new SqlCommand("insert into ddc.ddc.supplier (Title, IsVirtual) values ('" + this.newSupplier.Name + "',0)", dbContext.GetConnection());
            sql.ExecuteNonQuery();
            sql = new SqlCommand("select id from ddc.ddc.Supplier where Title = '" + this.newSupplier.Name + "' order by id desc", dbContext.GetConnection());
            var reader = sql.ExecuteReader();
            if (reader.Read())
            {
                this.newSupplier.Id = reader.GetValue(0).ToString();
            }
            else
            {
                throw new Exception("Supplier wasn't created or already deleted.");
            }
            return this.newSupplier;
        }

        /// <summary>
        /// Remove supplier by name. If no name specified - remove the supplier created during current session.
        /// </summary>
        /// <param name="supplierTitle">Name of supplier to be removed.</param>
        public void RemoveSupplier(string supplierTitle = "")
        {
            if (supplierTitle == "")
            {
                supplierTitle = this.newSupplier.Name;
            }
            SqlCommand sql = new SqlCommand("delete from ddc.ddc.supplier where Title = '" + supplierTitle + "'", dbContext.GetConnection());
            sql.ExecuteNonQuery();
        }

        public string GetSupplierName()
        {
            return newSupplier.Name;
        }

        public string GetSupplierId()
        {
            return newSupplier.Id;
        }

        public string CreateContract(string SupplierId, DateTime SignDate, DateTime StartDate, DateTime FinishDate)
        {
            if (SupplierId == "")
            {
                SupplierId = this.newSupplier.Id;
            }
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

        public string CreateCondition()
        {
            // insert into ddc.ddc.condition (StartDate, FinishDate, AmountPercent, AmountQty, ContractId, CreateDate, CreateUser, EditDate, EditUser, Status, AllBrandsSelected, AllProductsSelected, BusinessDomainId, IsDeleted)
            // values(cast('2015-02-03 12:00:00' as Datetime), cast('2020-02-03 12:00:00' as Datetime), 100, 1, 5, SYSDATETIME(), 'TEST', sysdatetime(), 'TEST', 1, 0, 0, 2, 0);
            return "";
        }

        private string RandomString(int size)
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
