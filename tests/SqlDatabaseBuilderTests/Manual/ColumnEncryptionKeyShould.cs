﻿using System;
using Microsoft.Data.SqlClient;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class ColumnEncryptionKeyShould
    {
        private readonly string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath", EnvironmentVariableTarget.User);

        [Fact]
        public void CreateAndDropColumnEncryptionKey()
        {
            string cmkName = nameof(CreateAndDropColumnEncryptionKey);
            string keyPath = "CurrentUser/My/BBF037EC4A133ADCA89FFAEC16CA5BFA8878FB94";
            ColumnMasterKey columnMasterKey = new ColumnMasterKey(cmkName, KeyStoreProvider.WindowsCertificateStoreProvider, keyPath);

            string cekName = nameof(CreateAndDropColumnEncryptionKey);
            ColumnEncryptionKey columnEncryptionKey = new ColumnEncryptionKey(cekName, columnMasterKey, "0x555");

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(columnMasterKey.IsColumnMasterKeyPresentInDatabase(sqlConnection), "ColumnMasterKey should not exist in the database.");
                Assert.False(columnEncryptionKey.IsColumnEncryptionKeyPresentInDatabase(sqlConnection), "ColumnEncryptionKey should not exist in the database.");
                columnMasterKey.Create(sqlConnection);
                columnEncryptionKey.Create(sqlConnection);
                Assert.True(columnMasterKey.IsColumnMasterKeyPresentInDatabase(sqlConnection), "ColumnMasterKey should exist in the database.");
                Assert.True(columnEncryptionKey.IsColumnEncryptionKeyPresentInDatabase(sqlConnection), "ColumnEncryptionKey should exist in the database.");

                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandText = $@"
                        SELECT cmk.name, v.encrypted_value 
                        FROM sys.column_encryption_keys cek JOIN sys.column_encryption_key_values v 
                        ON (cek.column_encryption_key_id = v.column_encryption_key_id)
                        JOIN sys.column_master_keys cmk 
                        ON (cmk.column_master_key_id = v.column_master_key_id)
                        WHERE cek.name = 'CreateAndDropColumnEncryptionKey'";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Assert.True(reader.HasRows, "The sql query should have returned at least one row.");
                        while (reader.Read())
                        {
                            Assert.Equal(columnEncryptionKey.ColumnMasterKeyName, reader.GetString(0));
                            Assert.NotNull(reader.GetValue(1));
                        }
                    }
                }

                columnEncryptionKey.Drop(sqlConnection);
                columnMasterKey.Drop(sqlConnection);
                Assert.False(columnMasterKey.IsColumnMasterKeyPresentInDatabase(sqlConnection), "ColumnMasterKey should not exist in the database.");
                Assert.False(columnEncryptionKey.IsColumnEncryptionKeyPresentInDatabase(sqlConnection), "ColumnEncryptionKey should not exist in the database.");
            }
        }
    }
}
