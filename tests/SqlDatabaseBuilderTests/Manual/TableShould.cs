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
    public class TableShould
    {
        private string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath", EnvironmentVariableTarget.User);

        [Fact]
        public void CreateAndDropTableSuccessfully()
        {
            string tableName = nameof(CreateAndDropTableSuccessfully);
            Table table = new Table(tableName);
            table.Columns.Add(new Column("Id", DataType.Int()));            

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(tableName, sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(tableName, sqlConnection));
                table.Drop(sqlConnection);
                Assert.False(table.IsTablePresentInDatabase(tableName, sqlConnection));
            }
        }
    }
}
