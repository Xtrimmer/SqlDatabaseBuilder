using System;
using System.Data.SqlClient;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class UniqueConstraintShould
    {
        private string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath");

        [Fact]
        public void CreateSingleColumnUniqueConstraint()
        {
            const string ColumnName = "Unique";
            const string TableName = nameof(CreateSingleColumnUniqueConstraint);

            Table table = new Table(TableName);
            Column column = new Column(ColumnName, DataType.Int());
            table.Columns.Add(column);
            UniqueConstraint uniqueConstraint = new UniqueConstraint(column, ColumnSort.DESC);
            table.Constraints.Add(uniqueConstraint);

            VerifyUniqueConstraint(TableName, table);
        }

        [Fact]
        public void CreateMultiColumnUniqueConstraint()
        {
            const string TableName = nameof(CreateMultiColumnUniqueConstraint);

            Table table = new Table(TableName);
            Column column1 = new Column("Unique1", DataType.Int());
            Column column2 = new Column("Unique2", DataType.Int());
            Column column3 = new Column("Unique3", DataType.Int());
            table.Columns.AddAll(column1, column2, column3);
            UniqueConstraint uniqueConstraint = new UniqueConstraint("UniqueConstraint");
            uniqueConstraint.AddColumns(column1, column2, column3);
            table.Constraints.Add(uniqueConstraint);

            VerifyUniqueConstraint(TableName, table);
        }

        private void VerifyUniqueConstraint(string TableName, Table table)
        {
            Console.WriteLine($"keyword={connectionString}".Replace(".", "-"));
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(sqlConnection));

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsUniqueCnst') = 1 AND TABLE_NAME = '{TableName}'";
                    sqlCommand.CommandText = sql;
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        int index = 0;
                        while (sqlDataReader.Read())
                        {
                            Assert.Equal(table.Columns[index++].Name, sqlDataReader.GetString(0));
                        }
                    }
                }

                table.Drop(sqlConnection);
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
            }
        }
    }
}
