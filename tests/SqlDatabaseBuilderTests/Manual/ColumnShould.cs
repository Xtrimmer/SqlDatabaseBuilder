using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;
using System.Linq;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class ColumnShould
    {
        private readonly string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath");

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
        public void AddDefaultConstraintWithoutName()
        {
            string tableName = nameof(AddDefaultConstraintWithoutName);
            Table table = new Table(tableName);
            Column[] columns = TestTableHelper.DataTypeValues.Keys.Select(k => new Column(k.Definition, k)).ToArray();
            table.Columns.AddAll(columns);
            //for each column on the table, add a default type
            table.Columns.ForEach(c => c.Default = new Default(TestTableHelper.DataTypeValues[c.DataType]));
            //create table in db
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(sqlConnection));
                //insert rows without any values
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $"INSERT {tableName} DEFAULT VALUES";
                    sqlCommand.CommandText = sql;
                    int rowsUpdated = sqlCommand.ExecuteNonQuery();
                    Assert.Equal(1, rowsUpdated);
                }

                //for each column we need to look up the value that was entered vs the expected value
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $"SELECT * FROM {tableName}";
                    sqlCommand.CommandText = sql;
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();
                    dataReader.Read();
                    object[] expectedValues = TestTableHelper.DataTypeValues.Values.ToArray();
                    for (var i = 0; i < dataReader.FieldCount; i++)
                    {
                        VerifyDefaultValueIsWhatWasDefined(dataReader, expectedValues, i);
                    }
                    dataReader.Close();
                }
                table.Drop(sqlConnection);
            }

        }

        private static void VerifyDefaultValueIsWhatWasDefined(SqlDataReader dataReader, object[] expectedValues, int i)
        {
            object result = dataReader[i];
            if (result is bool && !(bool)result) result = 0;
            object expected = expectedValues[i];
            if (expected is string) expected = ((string)expected).Replace("'", "");
            if (result is byte[]) expected = new byte[] { (byte)expected };
            if (result is DateTime) expected = Convert.ToDateTime(expected);
            if (result is TimeSpan) expected = TimeSpan.Parse((string)expected);
            if (result is Guid) expected = Guid.Parse((string)expected);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SetSystemValuesByUsingFunctions()
        {
            string tableName = nameof(SetSystemValuesByUsingFunctions);
            DateTime expectedDate = DateTime.UtcNow.Date;
            Table table = new Table(tableName);
            table.Columns.Add(new Column("DefaultDate", DataType.Date()) { Default = new Default("GETDATE()") });

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(sqlConnection));
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(sqlConnection));

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $"INSERT {tableName} DEFAULT VALUES";
                    sqlCommand.CommandText = sql;
                    int rowsUpdated = sqlCommand.ExecuteNonQuery();
                    Assert.Equal(1, rowsUpdated);
                }

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $"SELECT * FROM {tableName}";
                    sqlCommand.CommandText = sql;
                    DateTime actualDate = (DateTime)sqlCommand.ExecuteScalar();
                    Assert.Equal(expectedDate, actualDate);
                }
                table.Drop(sqlConnection);
            }
        }

        [Fact]
        public void AddDefaultConstraintWithName()
        {
            string expectedDefaultName = "testDefault";
            object actualDefaultName = TestDefaultConstraint(expectedDefaultName, "'VarChar'", nameof(AddDefaultConstraintWithName));
            Assert.Equal(expectedDefaultName, actualDefaultName);
        }

        [Fact]
        public void AddDefaultConstraintWithNullNameAndNullValue()
        {
            string expectedDefaultName = null;
            object actualDefaultName = TestDefaultConstraint(expectedDefaultName, null, nameof(AddDefaultConstraintWithNullNameAndNullValue));
            Assert.Equal(expectedDefaultName, actualDefaultName);

        }

        [Fact]
        public void AddDefaultConstraintWithNameAndNullValue()
        {
            string expectedDefaultName = "testDefault";
            object actualDefaultName = TestDefaultConstraint(expectedDefaultName, null, nameof(AddDefaultConstraintWithNameAndNullValue));
            Assert.Null(actualDefaultName);
        }

        [Fact]
        public void AddCollationCorrectly()
        {
            string tableName = nameof(AddCollationCorrectly);
            string columnName = nameof(AddCollationCorrectly) + "Column";
            string expectedCollation = "Latin1_General_BIN2";
            Column column = new Column(columnName, DataType.Char())
            {
                Collation = expectedCollation
            };

            Table table = new Table(tableName);
            table.Columns.AddAll(column);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(sqlConnection), "Table should not exist in the database.");
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(sqlConnection), "Table should exist in the database.");

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $"Select collation_name from sys.columns where name = '{columnName}'";
                    sqlCommand.CommandText = sql;
                    string actualCollation = (string)sqlCommand.ExecuteScalar();
                    Assert.Equal(expectedCollation, actualCollation);
                }

                table.Drop(sqlConnection);
            }
        }

        [Fact]
        public void AddColumnEncryptionCorrectly()
        {
            string tableName = nameof(AddColumnEncryptionCorrectly);
            string columnName1 = tableName + "Column1";
            string columnName2 = tableName + "Column2";
            string columnMasterKeyName = tableName + "_CMK";
            string columnEncryptionName = tableName + "_CEK";

            ColumnMasterKey columnMasterKey = new ColumnMasterKey(columnMasterKeyName, KeyStoreProvider.AzureKeyVaultProvider, "Test/Path");
            ColumnEncryptionKey columnEncryptionKey = new ColumnEncryptionKey(columnEncryptionName, columnMasterKey.Name, "0x555");

            ColumnEncryption columnEncryption1 = new ColumnEncryption(columnEncryptionKey, ColumnEncryptionType.Deterministic);
            Column column1 = new Column(columnName1, DataType.Char())
            {
                ColumnEncryption = columnEncryption1,
                Collation = "Latin1_General_BIN2"
            };

            ColumnEncryption columnEncryption2 = new ColumnEncryption(columnEncryptionKey, ColumnEncryptionType.Randomized);
            Column column2 = new Column(columnName2, DataType.NVarChar())
            {
                ColumnEncryption = columnEncryption2
            };

            Table table = new Table(tableName);
            table.Columns.AddAll(column1, column2);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(columnMasterKey.IsColumnMasterKeyPresentInDatabase(sqlConnection), "ColumnMasterKey should not exist in the database.");
                columnMasterKey.Create(sqlConnection);
                Assert.True(columnMasterKey.IsColumnMasterKeyPresentInDatabase(sqlConnection), "ColumnMasterKey should exist in the database.");
                Assert.False(columnEncryptionKey.IsColumnEncryptionKeyPresentInDatabase(sqlConnection), "ColumnEncryptionKey should not exist in the database.");
                columnEncryptionKey.Create(sqlConnection);
                Assert.True(columnEncryptionKey.IsColumnEncryptionKeyPresentInDatabase(sqlConnection), "ColumnEncryptionKey should exist in the database.");
                Assert.False(table.IsTablePresentInDatabase(sqlConnection), "Table should not exist in the database.");
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(sqlConnection), "Table should exist in the database.");

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    foreach (Column column in table.Columns)
                    {
                        string sql = $@"
                            Select c.encryption_type_desc, k.name
                            FROM sys.columns c JOIN sys.column_encryption_keys k ON (c.column_encryption_key_id = k.column_encryption_key_id)
                            WHERE c.name = '{column.Name}'";
                        sqlCommand.CommandText = sql;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        { 
                            while (reader.Read())
                            {
                                Assert.Equal(column.ColumnEncryption.ColumnEncryptionType.GetStringValue(), reader.GetString(0));
                                Assert.Equal(column.ColumnEncryption.ColumnEncryptionKeyName, reader.GetString(1));
                            }
                        }
                    }
                }

                table.Drop(sqlConnection);
                columnEncryptionKey.Drop(sqlConnection);
                columnMasterKey.Drop(sqlConnection);
            }
        }

        private object TestDefaultConstraint(string defaultName, string defaultValue, string tableName)
        {
            string columnName = DataType.VarChar().Definition;
            object actualDefaultName;
            int length = defaultValue == null ? 1 : defaultValue.Length;
            Table table = new Table(tableName);
            Column column = new Column(columnName, DataType.VarChar(length))
            {
                Default = new Default(defaultName, defaultValue)
            };
            table.Columns.Add(column);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(table.IsTablePresentInDatabase(sqlConnection), "Table already exists in database");
                table.Create(sqlConnection);
                Assert.True(table.IsTablePresentInDatabase(sqlConnection));
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    string sql = $@"
                        SELECT 
                            constraints.name AS DefaultConstraintName
                        FROM 
                            sys.default_constraints constraints
                            INNER JOIN sys.columns columns ON (constraints.parent_object_id = columns.object_id AND constraints.parent_column_id = columns.column_id)
                            INNER JOIN sys.tables tables ON (tables.object_id = columns.object_id)
                        WHERE 
                            tables.name = '{tableName}'
                            AND columns.name = '{columnName}';
                            ";
                    sqlCommand.CommandText = sql;
                    actualDefaultName = sqlCommand.ExecuteScalar();

                }
                table.Drop(sqlConnection);
            }
            return actualDefaultName;
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
