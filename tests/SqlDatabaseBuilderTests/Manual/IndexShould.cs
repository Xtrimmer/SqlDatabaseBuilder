using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class IndexShould : IClassFixture<IndexFixture>
    {
        private string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath");
        private IndexFixture fixture;

        public IndexShould(IndexFixture fixture)
        {
            this.fixture = fixture;
        }

        [Theory]
        [InlineData(IndexType.CLUSTERED, true, ColumnSort.ASC)]
        [InlineData(IndexType.CLUSTERED, true, ColumnSort.DESC)]
        [InlineData(IndexType.CLUSTERED, false, ColumnSort.ASC)]
        [InlineData(IndexType.CLUSTERED, false, ColumnSort.DESC)]
        [InlineData(IndexType.NONCLUSTERED, true, ColumnSort.ASC)]
        [InlineData(IndexType.NONCLUSTERED, true, ColumnSort.DESC)]
        [InlineData(IndexType.NONCLUSTERED, false, ColumnSort.ASC)]
        [InlineData(IndexType.NONCLUSTERED, false, ColumnSort.DESC)]
        public void CreateIndexProperly(IndexType indexType, bool isUnique, ColumnSort columnSort)
        {
            string indexName = "IX_Test";
            Table table = fixture.table;
            List<Tuple<Column, ColumnSort>> indexColumns = new List<Tuple<Column, ColumnSort>>();
            table.Columns.ForEach(c =>
            {
                indexColumns.Add(Tuple.Create(c, columnSort));
                Index index = new Index(indexName, table, indexColumns.ToArray());
                index.IsUnique = isUnique;
                index.IndexType = indexType;

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    index.Create(sqlConnection);

                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        string sql = $@"
                            SELECT COUNT(*)
                            FROM sys.indexes
                            JOIN sys.index_columns 
                                ON sys.indexes.object_id = sys.index_columns.object_id
                            where name = '{indexName}'";

                        sqlCommand.CommandText = sql;
                        int columnCount = (int)sqlCommand.ExecuteScalar();

                        Assert.Equal(indexColumns.Count, columnCount);

                        sql = $@"
                            SELECT OBJECT_NAME(sys.indexes.object_id), name, type_desc, is_unique, is_descending_key
                            FROM sys.indexes
                            JOIN sys.index_columns 
                                ON sys.indexes.object_id = sys.index_columns.object_id
                            where name = '{indexName}'";

                        sqlCommand.CommandText = sql;
                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                Assert.Equal(table.Name, sqlDataReader.GetString(0));
                                Assert.Equal(index.Name, sqlDataReader.GetString(1));
                                Assert.Equal(indexType.ToString(), sqlDataReader.GetString(2));
                                Assert.Equal(isUnique, sqlDataReader.GetBoolean(3));
                                Assert.Equal(columnSort == ColumnSort.DESC, sqlDataReader.GetBoolean(4));
                            }
                        }
                    }

                    index.Drop(sqlConnection);
                }
            });
        }
    }

    public class IndexFixture : IDisposable
    {
        private string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath");
        public Table table;

        public IndexFixture()
        {

            table = new Table("Persons");
            Column id = new Column("Id", DataType.Int()) { Nullable = false };
            Column lastName = new Column("LastName", DataType.VarChar(255));
            Column firstName = new Column("FirstName", DataType.VarChar(255));
            Column age = new Column("Age", DataType.Int());
            table.Columns.AddAll(id, lastName, firstName, age);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                table.Create(sqlConnection);
            }
        }

        public void Dispose()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                table.Drop(sqlConnection);
            }
        }
    }
}
