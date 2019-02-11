using Xtrimmer.SqlDatabaseBuilder;
using System;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class ColumnShould
    {
        private string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath", EnvironmentVariableTarget.User);

        [Fact]
        public void DefineDataTypesCorrectly()
        {
            string tableName = nameof(DefineDataTypesCorrectly);
            Table table = CreateTableWithEveryDataType(tableName);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(tableName, sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(tableName, sqlConnection));
                Assert.True(AllDataTypesMatch(table, sqlConnection));
                table.Drop(sqlConnection);
            }
        }

        [Fact]
        public void DefineNotNullCorrectly()
        {
            string tableName = nameof(DefineNotNullCorrectly);
            Table table = CreateTableWithEveryDataTypeNonNullable(tableName);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(tableName, sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(tableName, sqlConnection));
                Assert.True(AllColumnsNotNull(table, sqlConnection));
                table.Drop(sqlConnection);
            }
        }

        private bool AllColumnsNotNull(Table table, SqlConnection sqlConnection)
        {
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                string sql = $"SELECT IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table.Name}'";
                sqlCommand.CommandText = sql;
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);

                if (!table.Columns.Count.Equals(dataTable.Rows.Count)) return false;

                bool allNull = true;

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    string actualNullability = (string)dataTable.Rows[i][0];
                    allNull &= "NO".Equals(actualNullability);
                }

                return allNull;
            }
        }

        private static bool AllDataTypesMatch(Table table, SqlConnection sqlConnection)
        {
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                string sql = $"SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table.Name}'";
                sqlCommand.CommandText = sql;
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(dataReader);

                if (!table.Columns.Count.Equals(dataTable.Rows.Count)) return false;

                bool allMatch = true;

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    string dataDefinition = table.Columns[i].DataType.Definition;
                    int index = dataDefinition.IndexOf("(");
                    string expectedDataType = index == -1 ? dataDefinition : dataDefinition.Substring(0, index);
                    string actualDataType = (string)dataTable.Rows[i][0];
                    allMatch &= expectedDataType.Equals(actualDataType);
                }

                return allMatch;
            }
        }

        private Table CreateTableWithEveryDataTypeNonNullable(string tableName)
        {
            Table table = CreateTableWithEveryDataType(tableName);
            table.Columns.ForEach(c => c.Nullable = false);
            return table;
        }

        private static Table CreateTableWithEveryDataType(string tableName)
        {
            Table table = new Table(tableName);
            table.Columns.Add(new Column("ID", DataType.BigInt()));
            table.Columns.Add(new Column("Amount", DataType.Int()));
            table.Columns.Add(new Column("Maximum", DataType.TinyInt()));
            table.Columns.Add(new Column("Minimum", DataType.SmallInt()));
            table.Columns.Add(new Column("Payment", DataType.Money()));
            table.Columns.Add(new Column("Change", DataType.SmallMoney()));
            table.Columns.Add(new Column("Decimal", DataType.Decimal()));
            table.Columns.Add(new Column("Decimal-1", DataType.Decimal(10)));
            table.Columns.Add(new Column("Decimal-2", DataType.Decimal(20, 5)));
            table.Columns.Add(new Column("Numeric", DataType.Numeric()));
            table.Columns.Add(new Column("Numeric-1", DataType.Numeric(15)));
            table.Columns.Add(new Column("Numeric-2", DataType.Numeric(22, 11)));
            table.Columns.Add(new Column("VarChar", DataType.VarChar()));
            table.Columns.Add(new Column("VarChar-128", DataType.VarChar(128)));
            table.Columns.Add(new Column("VarChar-MAX", DataType.VarChar(DataType.MAX)));
            table.Columns.Add(new Column("Char", DataType.Char()));
            table.Columns.Add(new Column("Char-256", DataType.Char(256)));
            table.Columns.Add(new Column("VarBinary", DataType.VarBinary()));
            table.Columns.Add(new Column("VarBinary-512", DataType.VarBinary(512)));
            table.Columns.Add(new Column("VarBinary-MAX", DataType.VarBinary(DataType.MAX)));
            table.Columns.Add(new Column("Binary", DataType.Binary()));
            table.Columns.Add(new Column("Binary-1024", DataType.Binary(1024)));
            table.Columns.Add(new Column("NVarChar", DataType.NVarChar()));
            table.Columns.Add(new Column("NVarChar-248", DataType.NVarChar(248)));
            table.Columns.Add(new Column("NVarChar-MAX", DataType.NVarChar(DataType.MAX)));
            table.Columns.Add(new Column("NChar", DataType.NChar()));
            table.Columns.Add(new Column("NChar-400", DataType.NChar(400)));
            table.Columns.Add(new Column("Date", DataType.Date()));
            table.Columns.Add(new Column("DateTime", DataType.DateTime()));
            table.Columns.Add(new Column("DateTime2", DataType.DateTime2()));
            table.Columns.Add(new Column("DateTime2-2", DataType.DateTime2(2)));
            table.Columns.Add(new Column("Time", DataType.Time()));
            table.Columns.Add(new Column("Time-1", DataType.Time(1)));
            table.Columns.Add(new Column("UniqueIdentifier", DataType.UniqueIdentifier()));
            return table;
        }
    }
}
