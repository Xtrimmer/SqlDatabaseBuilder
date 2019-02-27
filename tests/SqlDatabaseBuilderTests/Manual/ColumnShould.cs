using System;
using System.Data;
using System.Data.SqlClient;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class ColumnShould
    {
        private string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath");

        [Fact]
        public void DefineDataTypesCorrectly()
        {
            string tableName = nameof(DefineDataTypesCorrectly);
            Table table = CreateTableWithEveryDataType(tableName);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(sqlConnection));
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
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(sqlConnection));
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
            table.Columns.AddAll(
                new Column("ID", DataType.BigInt()),
                new Column("Amount", DataType.Int()),
                new Column("Maximum", DataType.TinyInt()),
                new Column("Minimum", DataType.SmallInt()),
                new Column("Payment", DataType.Money()),
                new Column("Change", DataType.SmallMoney()),
                new Column("Decimal", DataType.Decimal()),
                new Column("Decimal-1", DataType.Decimal(10)),
                new Column("Decimal-2", DataType.Decimal(20, 5)),
                new Column("Numeric", DataType.Numeric()),
                new Column("Numeric-1", DataType.Numeric(15)),
                new Column("Numeric-2", DataType.Numeric(22, 11)),
                new Column("VarChar", DataType.VarChar()),
                new Column("VarChar-128", DataType.VarChar(128)),
                new Column("VarChar-MAX", DataType.VarChar(DataType.MAX)),
                new Column("Char", DataType.Char()),
                new Column("Char-256", DataType.Char(256)),
                new Column("VarBinary", DataType.VarBinary()),
                new Column("VarBinary-512", DataType.VarBinary(512)),
                new Column("VarBinary-MAX", DataType.VarBinary(DataType.MAX)),
                new Column("Binary", DataType.Binary()),
                new Column("Binary-1024", DataType.Binary(1024)),
                new Column("NVarChar", DataType.NVarChar()),
                new Column("NVarChar-248", DataType.NVarChar(248)),
                new Column("NVarChar-MAX", DataType.NVarChar(DataType.MAX)),
                new Column("NChar", DataType.NChar()),
                new Column("NChar-400", DataType.NChar(400)),
                new Column("Date", DataType.Date()),
                new Column("DateTime", DataType.DateTime()),
                new Column("DateTime2", DataType.DateTime2()),
                new Column("DateTime2-2", DataType.DateTime2(2)),
                new Column("Time", DataType.Time()),
                new Column("Time-1", DataType.Time(1)),
                new Column("UniqueIdentifier", DataType.UniqueIdentifier())
            );
            return table;
        }
    }
}
