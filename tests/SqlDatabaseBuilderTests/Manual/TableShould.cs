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
            table.AddColumns(new Column("Id", DataType.Int()));            

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

        [Fact]
        public void CreateTableWithPrimaryKeySuccessfully()
        {
            const string COLUMN_NAME = "Id";
            string tableName = nameof(CreateTableWithPrimaryKeySuccessfully);

            Table table = new Table(tableName);
            Column column = new Column(COLUMN_NAME, DataType.Int());
            table.AddColumns(column);
            PrimaryKeyConstraint primaryKeyConstraint = new PrimaryKeyConstraint("PrimaryKeyConstraint");
            primaryKeyConstraint.AddColumn(column);
            primaryKeyConstraint.AddColumns(Tuple.Create(column, ));
            table.Constraints.Add(primaryKeyConstraint);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(tableName, sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(tableName, sqlConnection));

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1 AND TABLE_NAME = '{tableName}'";
                    sqlCommand.CommandText = sql;
                    string columnNameResult = (string) sqlCommand.ExecuteScalar();
                    Assert.Equal(COLUMN_NAME, columnNameResult);
                }

                table.Drop(sqlConnection);
                Assert.False(table.IsTablePresentInDatabase(tableName, sqlConnection));
            }
        }
    }
}
