using System;
using System.Data.SqlClient;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class PrimaryKeyConstraintShould
    {
        private string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath");

        [Fact]
        public void CreateTableWithSingleColumnPrimaryKey()
        {
            const string ColumnName = "Id";
            const string TableName = nameof(CreateTableWithSingleColumnPrimaryKey);

            Table table = new Table(TableName);
            Column column = new Column(ColumnName, DataType.Int());
            table.Columns.Add(column);
            PrimaryKeyConstraint primaryKeyConstraint = new PrimaryKeyConstraint("PrimaryKeyConstraint");
            primaryKeyConstraint.AddColumn(column);
            table.Constraints.Add(primaryKeyConstraint);

            VerifyPrimaryKey(ColumnName, TableName, table);
        }

        [Fact]
        public void CreateTableWithMultiColumnPrimaryKeyWithColumnSort()
        {
            const string ColumnName1 = "Id";
            const string ColumnName2 = "Id2";
            const string TableName = nameof(CreateTableWithMultiColumnPrimaryKeyWithColumnSort);

            Table table = new Table(TableName);
            Column column = new Column(ColumnName1, DataType.Int());
            Column column2 = new Column(ColumnName2, DataType.Int());
            table.Columns.AddAll(column, column2);
            PrimaryKeyConstraint primaryKeyConstraint = new PrimaryKeyConstraint("PrimaryKeyConstraint", column, ColumnSort.DESC);
            primaryKeyConstraint.AddColumns(column2);
            table.Constraints.Add(primaryKeyConstraint);

            VerifyPrimaryKey(ColumnName1, TableName, table);
        }

        [Fact]
        public void CreateTableWithMultiColumnPrimaryKey()
        {
            const string ColumnName1 = "Id";
            const string ColumnName2 = "Id2";
            const string ColumnName3 = "Id3";
            const string TableName = nameof(CreateTableWithMultiColumnPrimaryKey);

            Table table = new Table(TableName);
            Column column = new Column(ColumnName1, DataType.Int());
            Column column2 = new Column(ColumnName2, DataType.Int());
            Column column3 = new Column(ColumnName3, DataType.Int());
            table.Columns.AddAll(column, column2, column3);
            PrimaryKeyConstraint primaryKeyConstraint = new PrimaryKeyConstraint();
            primaryKeyConstraint.AddColumns
            (
                Tuple.Create(column, ColumnSort.ASC),
                Tuple.Create(column2, ColumnSort.DESC),
                Tuple.Create(column3, ColumnSort.ASC)
            );
            table.Constraints.Add(primaryKeyConstraint);

            VerifyPrimaryKey(ColumnName1, TableName, table);
        }

        private void VerifyPrimaryKey(string COLUMN_NAME, string tableName, Table table)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(sqlConnection));

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1 AND TABLE_NAME = '{tableName}'";
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
