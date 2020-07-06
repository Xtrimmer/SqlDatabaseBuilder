using System;
using Microsoft.Data.SqlClient;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class CheckConstraintShould
    {
        private readonly string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath");

        [Fact]
        public void CreateTableWithSingleCheckConstraint()
        {
            const string ColumnName = "Id";
            const string TableName = nameof(CreateTableWithSingleCheckConstraint);

            Table table = new Table(TableName);
            Column column = new Column(ColumnName, DataType.Int());
            table.Columns.Add(column);

            CheckConstraint checkConstraint = new CheckConstraint()
            {
                CheckExpression = new CheckExpression(column, CheckOperator.GreaterThan, 25)
            };
            
            table.Constraints.Add(checkConstraint);

            VerifyCheckConstraint(table, "([Id]>(25))");
        }

        [Fact]
        public void CreateTableWithMultipleCheckConstraint()
        {
            const string TableName = nameof(CreateTableWithMultipleCheckConstraint);

            Table table = new Table(TableName);
            Column column1 = new Column("column1", DataType.Int());
            Column column2 = new Column("column2", DataType.Int());
            Column column3 = new Column("column3", DataType.VarChar(10));
            table.Columns.AddAll(column1, column2, column3);

            CheckExpression expression = new CheckExpression("([column2]>=(0))");
            CheckConstraint checkConstraint1 = new CheckConstraint("CK_CheckConstraint_1")
            {
                CheckExpression = new CheckExpression(column1, CheckOperator.GreaterThanOrEquals, column2)
                .And(expression)
            };
            CheckConstraint checkConstraint2 = new CheckConstraint("CK_CheckConstraint_2")
            {
                CheckExpression = new CheckExpression(column3, CheckOperator.GreaterThanOrEquals, "'a'")
                 .And(column3, CheckOperator.LessThanOrEquals, "'zzzzzzzzzz'")
                 .Or(column3, CheckOperator.GreaterThanOrEquals, "'A'")
                 .And(column3, CheckOperator.LessThanOrEquals, "'ZZZZZZZZZZ'")
            };
            table.Constraints.AddAll(checkConstraint1, checkConstraint2);

            VerifyCheckConstraint(table,
                "([column1]>=[column2] AND [column2]>=(0))",
                "([column3]>='a' AND [column3]<='zzzzzzzzzz' OR [column3]>='A' AND [column3]<='ZZZZZZZZZZ')");
        }

        [Fact]
        public void CreateTableWithMultipleCheckConstraint2()
        {
            const string TableName = nameof(CreateTableWithMultipleCheckConstraint2);

            Table table = new Table(TableName);
            Column column1 = new Column("column1", DataType.Int());
            Column column2 = new Column("column2", DataType.Int());
            Column column3 = new Column("column3", DataType.VarChar(10));
            table.Columns.AddAll(column1, column2, column3);

            CheckExpression expression = new CheckExpression("([column2]>=0)");
            CheckConstraint checkConstraint1 = new CheckConstraint(expression);

            CheckExpression expression2 = new CheckExpression("([column2]!=(666))");
            CheckConstraint checkConstraint2 = new CheckConstraint()
            {
                CheckExpression = new CheckExpression(column3, CheckOperator.Equals, "'a'")
                 .And(column2, CheckOperator.NotEquals, column1)
                 .Or(expression2)
                 .Or(column2, CheckOperator.LessThan, column1)
                 .And("1=1")
                 .Or("2=2")
            };
            table.Constraints.AddAll(checkConstraint1, checkConstraint2);

            VerifyCheckConstraint(table,
                "([column2]>=(0))",
                "([column3]='a' AND [column2]<>[column1] OR [column2]<>(666) OR [column2]<[column1] AND (1)=(1) OR (2)=(2))");
        }

        private void VerifyCheckConstraint(Table table, params string[] expectedValues)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(sqlConnection));

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $@"
                        SELECT definition 
                        FROM sys.check_constraints ch 
                        WHERE
                        (
                            SELECT name 
                            FROM sys.objects 
                            WHERE OBJECT_ID = ch.parent_object_id
                        ) = '{table.Name}'";

                    sqlCommand.CommandText = sql;
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        int index = 0;
                        while (sqlDataReader.Read())
                        {
                            string s = sqlDataReader.GetString(0);
                            Assert.Equal(expectedValues[index++], s);
                        }
                    }
                }

                table.Drop(sqlConnection);
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
            }
        }
    }
}
