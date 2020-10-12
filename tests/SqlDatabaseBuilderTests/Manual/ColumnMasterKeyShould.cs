using System;
using Microsoft.Data.SqlClient;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class ColumnMasterKeyShould
    {
        private readonly string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath", EnvironmentVariableTarget.User);

        [Fact]
        public void CreateAndDropColumnMasterKey()
        {
            string cmkName = nameof(CreateAndDropColumnMasterKey);
            string keyPath = "CurrentUser/My/BBF037EC4A133ADCA89FFAEC16CA5BFA8878FB94";
            ColumnMasterKey cmk = new ColumnMasterKey(cmkName, KeyStoreProvider.WindowsCertificateStoreProvider, keyPath);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(cmk.IsColumnMasterKeyPresentInDatabase(sqlConnection), "ColumnMasterKey should not exist in the database.");
                cmk.Create(sqlConnection);
                Assert.True(cmk.IsColumnMasterKeyPresentInDatabase(sqlConnection), "ColumnMasterKey should exist in the database.");

                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandText = $"SELECT key_store_provider_name, key_path, allow_enclave_computations, signature FROM sys.column_master_keys WHERE name = '{nameof(CreateAndDropColumnMasterKey)}'";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Assert.True(reader.HasRows, "The sql query should have returned at least one row.");
                        while (reader.Read())
                        {
                            Assert.Equal(cmk.KeyStoreProviderName, reader.GetString(0));
                            Assert.Equal(cmk.KeyPath, reader.GetString(1));
                            Assert.Equal(0, reader.GetInt32(2));
                            Assert.IsType<DBNull>(reader.GetValue(3));
                        }
                    }
                }

                cmk.Drop(sqlConnection);
                Assert.False(cmk.IsColumnMasterKeyPresentInDatabase(sqlConnection), "ColumnMasterKey should not exist in the database.");
            }
        }

        [Fact]
        public void CreateAndDropEnclaveEnabledColumnMasterKey()
        {
            string cmkName = nameof(CreateAndDropEnclaveEnabledColumnMasterKey);
            string keyPath = "CurrentUser/My/BBF037EC4A133ADCA89FFAEC16CA5BFA8878FB94";
            string provider = KeyStoreProvider.WindowsCertificateStoreProvider;
            ColumnMasterKey cmk = new ColumnMasterKey(cmkName, provider, keyPath)
            {
                IsEnclaveEnabled = true,
                Signature = "0x0123456789ABCDEF"
            };

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(cmk.IsColumnMasterKeyPresentInDatabase(sqlConnection), "ColumnMasterKey should not exist in the database.");
                cmk.Create(sqlConnection);
                Assert.True(cmk.IsColumnMasterKeyPresentInDatabase(sqlConnection), "ColumnMasterKey should exist in the database.");

                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandText = $"SELECT key_store_provider_name, key_path, allow_enclave_computations, signature FROM sys.column_master_keys WHERE name = '{nameof(CreateAndDropEnclaveEnabledColumnMasterKey)}'";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Assert.True(reader.HasRows, "The sql query should have returned at least one row.");
                        while (reader.Read())
                        {
                            Assert.Equal(cmk.KeyStoreProviderName, reader.GetString(0));
                            Assert.Equal(cmk.KeyPath, reader.GetString(1));
                            Assert.Equal(1, reader.GetInt32(2));
                            Assert.NotNull(reader.GetValue(3));
                        }
                    }
                }

                cmk.Drop(sqlConnection);
                Assert.False(cmk.IsColumnMasterKeyPresentInDatabase(sqlConnection), "ColumnMasterKey should not exist in the database.");
            }
        }
    }
}
