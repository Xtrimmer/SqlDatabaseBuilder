using System;
using Microsoft.Data.SqlClient;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class ForeignKeyConstraintShould
    {
        private string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath");

        [Fact]
        public void CreateSingleColumnForeignKeyConstraints()
        {
            const string ForeignKeyColumnName = "PersonId";

            Table primaryTable = new Table(nameof(CreateSingleColumnForeignKeyConstraints) + "Persons");
            Column id = new Column("Id", DataType.Int()) { Nullable = false };
            Column lastName = new Column("LastName", DataType.VarChar(255)) { Nullable = false };
            Column firstName = new Column("FirstName", DataType.VarChar(255));
            Column age = new Column("Age", DataType.Int());
            primaryTable.Columns.AddAll(id, lastName, firstName, age);
            primaryTable.Constraints.Add(new PrimaryKeyConstraint(id));

            Table foreignTable = new Table(nameof(CreateSingleColumnForeignKeyConstraints) + "Orders");
            Column orderId = new Column("OrderId", DataType.Int()) { Nullable = false };
            Column orderNumber = new Column("OrderNumber", DataType.Int()) { Nullable = false };
            Column personId = new Column(ForeignKeyColumnName, DataType.Int());
            foreignTable.Columns.AddAll(orderId, orderNumber, personId);

            PrimaryKeyConstraint primaryKeyConstraint = new PrimaryKeyConstraint(orderId);
            ForeignKeyConstraint foreignKeyConstraint = new ForeignKeyConstraint();
            foreignKeyConstraint.AddColumn(personId)
                .References(primaryTable)
                .AddReferenceColumn(id);
            foreignTable.Constraints.AddAll(primaryKeyConstraint, foreignKeyConstraint);

            VerifyForeignKey(primaryTable, foreignTable);
        }

        [Fact]
        public void CreateMultiColumnForeignKeyConstraints()
        {
            const string ForeignKeyColumnName1 = "PersonId";
            const string ForeignKeyColumnName2 = "PersonName";

            Table primaryTable = new Table(nameof(CreateMultiColumnForeignKeyConstraints) + "Persons");
            Column id = new Column("Id", DataType.Int()) { Nullable = false };
            Column lastName = new Column("LastName", DataType.VarChar(255)) { Nullable = false };
            Column firstName = new Column("FirstName", DataType.VarChar(255));
            Column age = new Column("Age", DataType.Int());
            primaryTable.Columns.AddAll(id, lastName, firstName, age);
            PrimaryKeyConstraint primaryTablePrimaryKeyConstraint = new PrimaryKeyConstraint();
            primaryTablePrimaryKeyConstraint.AddColumns(id, lastName);
            primaryTable.Constraints.Add(primaryTablePrimaryKeyConstraint);

            Table foreignTable = new Table(nameof(CreateMultiColumnForeignKeyConstraints) + "Orders");
            Column orderId = new Column("OrderId", DataType.Int()) { Nullable = false };
            Column orderNumber = new Column("OrderNumber", DataType.Int()) { Nullable = false };
            Column personId = new Column(ForeignKeyColumnName1, DataType.Int());
            Column personLastName = new Column(ForeignKeyColumnName2, DataType.VarChar(255));
            foreignTable.Columns.AddAll(orderId, orderNumber, personId, personLastName);

            PrimaryKeyConstraint foreignTableprimaryKeyConstraint = new PrimaryKeyConstraint(orderId);
            ForeignKeyConstraint foreignKeyConstraint = new ForeignKeyConstraint("ForeignKeyConstraint");
            foreignKeyConstraint.AddColumns(personId, personLastName)
                .References(primaryTable)
                .AddReferenceColumns(id, lastName);
            foreignTable.Constraints.AddAll(foreignTableprimaryKeyConstraint, foreignKeyConstraint);

            VerifyForeignKey(primaryTable, foreignTable);
        }

        private void VerifyForeignKey(Table primaryTable, Table foreignTable)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                Assert.False(primaryTable.IsTablePresentInDatabase(sqlConnection));
                primaryTable.Create(sqlConnection);
                Assert.True(primaryTable.IsTablePresentInDatabase(sqlConnection));

                Assert.False(foreignTable.IsTablePresentInDatabase(sqlConnection));
                foreignTable.Create(sqlConnection);
                Assert.True(foreignTable.IsTablePresentInDatabase(sqlConnection));

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsForeignKey') = 1 AND TABLE_NAME = '{foreignTable.Name}'";
                    sqlCommand.CommandText = sql;
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        int index = 2;
                        while (sqlDataReader.Read())
                        {
                            Assert.Equal(foreignTable.Columns[index++].Name, sqlDataReader.GetString(0));
                        }
                    }
                }

                foreignTable.Drop(sqlConnection);
                Assert.False(foreignTable.IsTablePresentInDatabase(sqlConnection));
                primaryTable.Drop(sqlConnection);
                Assert.False(primaryTable.IsTablePresentInDatabase(sqlConnection));
            }
        }
    }
}
