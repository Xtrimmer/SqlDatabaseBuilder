﻿using System;
using Microsoft.Data.SqlClient;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class TableShould
    {
        private readonly string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath", EnvironmentVariableTarget.User);

        [Fact]
        public void CreateAndDropTable()
        {
            string tableName = nameof(CreateAndDropTable);
            Table table = new Table(tableName);
            table.Columns.Add(new Column("Id", DataType.Int()));

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(sqlConnection));
                table.Drop(sqlConnection);
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
            }
        }

        [Fact]
        public void CreateTableWithMultipleConstraints()
        {
            const string PrimaryKeyColumnName = "PK";
            const string UniqueColumnName1 = "UQ1";
            const string UniqueColumnName2 = "UQ2";
            string tableName = nameof(CreateTableWithMultipleConstraints);

            Table table = new Table(tableName);
            Column column1 = new Column(PrimaryKeyColumnName, DataType.Int());
            Column column2 = new Column(UniqueColumnName1, DataType.Char(10));
            Column column3 = new Column(UniqueColumnName2, DataType.Money());
            table.Columns.AddAll(column1, column2, column3);

            PrimaryKeyConstraint primaryKeyConstraint = new PrimaryKeyConstraint
            {
                IndexType = IndexType.NONCLUSTERED
            };
            primaryKeyConstraint.AddColumn(column1);

            UniqueConstraint uniqueConstraint = new UniqueConstraint
            {
                IndexType = IndexType.CLUSTERED
            };
            uniqueConstraint.AddColumns(Tuple.Create(column2, ColumnSort.DESC), Tuple.Create(column3, ColumnSort.ASC));

            table.Constraints.AddAll(primaryKeyConstraint, uniqueConstraint);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(sqlConnection));

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $@"
                        SELECT COLUMN_NAME 
                        FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                        WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1 
                            AND TABLE_NAME = '{tableName}'";

                    sqlCommand.CommandText = sql;
                    string primaryKeycolumnNameResult = (string)sqlCommand.ExecuteScalar();
                    Assert.Equal(PrimaryKeyColumnName, primaryKeycolumnNameResult);

                    sql = $@"
                        SELECT COUNT(COLUMN_NAME) 
                        FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                        WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsUniqueCnst') = 1 
                            AND TABLE_NAME = '{tableName}'";

                    sqlCommand.CommandText = sql;
                    int columnCount = (int)sqlCommand.ExecuteScalar();
                    Assert.Equal(2, columnCount);
                }

                table.Drop(sqlConnection);
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
            }
        }
    }
}
