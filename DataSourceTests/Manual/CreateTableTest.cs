using Xtrimmer.SqlDatabaseBuilder;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class CreateTableTest : IDisposable
    {
        List<DatabaseResource> temporaryDatabaseResources = new List<DatabaseResource>();

        [Fact]
        public void createTableWithEveryDataType() {

            string tableName = "TestTable";

            Table table = new Table(tableName);
            table.Columns.Add(new Column("ID", DataType.BigInt()) { Nullable = true });
            table.Columns.Add(new Column("Amount", DataType.Int()));
            table.Columns.Add(new Column("Maximum", DataType.TinyInt()));
            table.Columns.Add(new Column("Minimum", DataType.SmallInt()));
            table.Columns.Add(new Column("Payment", DataType.Money()) { Nullable = false });
            table.Columns.Add(new Column("Change", DataType.SmallMoney()));
            table.Columns.Add(new Column("Decimal", DataType.Decimal()));
            table.Columns.Add(new Column("Decimal-1", DataType.Decimal(10)));
            table.Columns.Add(new Column("Decimal-2", DataType.Decimal(20, 5)));
            table.Columns.Add(new Column("Numeric", DataType.Numeric()));
            table.Columns.Add(new Column("Numeric-1", DataType.Numeric(15)));
            table.Columns.Add(new Column("Numeric-2", DataType.Numeric(22, 11)));
            table.Columns.Add(new Column("VarChar", DataType.VarChar()) { Nullable = false });
            table.Columns.Add(new Column("VarChar-128", DataType.VarChar(128)));
            table.Columns.Add(new Column("VarChar-MAX", DataType.VarChar(DataType.MAX)));
            table.Columns.Add(new Column("Char", DataType.Char()));
            table.Columns.Add(new Column("Char-256", DataType.Char(256)));
            table.Columns.Add(new Column("VarBinary", DataType.VarBinary()));
            table.Columns.Add(new Column("VarBinary-512", DataType.VarBinary(512)));
            table.Columns.Add(new Column("VarBinary-MAX", DataType.VarBinary(DataType.MAX)));
            table.Columns.Add(new Column("Binary", DataType.Binary()) { Nullable = false });
            table.Columns.Add(new Column("Binary-1024", DataType.Binary(1024)));
            table.Columns.Add(new Column("NVarChar", DataType.NVarChar()));
            table.Columns.Add(new Column("NVarChar-248", DataType.NVarChar(248)));
            table.Columns.Add(new Column("NVarChar-MAX", DataType.NVarChar(DataType.MAX)));
            table.Columns.Add(new Column("NChar", DataType.NChar()));
            table.Columns.Add(new Column("NChar-400", DataType.NChar(400)));
            table.Columns.Add(new Column("Date", DataType.Date()) { Nullable = false });
            table.Columns.Add(new Column("DateTime", DataType.DateTime()));
            table.Columns.Add(new Column("DateTime2", DataType.DateTime2()));
            table.Columns.Add(new Column("DateTime2-2", DataType.DateTime2(2)));
            table.Columns.Add(new Column("Time", DataType.Time()));
            table.Columns.Add(new Column("Time-1", DataType.Time(1)));
            table.Columns.Add(new Column("UniqueIdentifier", DataType.UniqueIdentifier()) { Nullable = true });


            temporaryDatabaseResources.Add(table);

            string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath", EnvironmentVariableTarget.User);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                table.Create(sqlConnection);

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $@"
                        SELECT DATA_TYPE 
                        FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_NAME = '{tableName}'";

                    sqlCommand.CommandText = sql;

                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(dataReader);

                    Assert.True(dataTable.Rows.Count > 0);
                    Assert.Equal(table.Columns.Count, dataTable.Rows.Count);
                }
            }
        }

        public void Dispose()
        {
            using (SqlConnection sqlConnection = new SqlConnection(Environment.GetEnvironmentVariable("AzureSqlServerPath", EnvironmentVariableTarget.User)))
            {
                sqlConnection.Open();
                temporaryDatabaseResources.ForEach(o => o.Drop(sqlConnection));
            }
        }
    }
}
