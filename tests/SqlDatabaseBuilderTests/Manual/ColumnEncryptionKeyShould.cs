using System;
using System.Data.SqlClient;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class ColumnEncryptionKeyShould
    {
        private readonly string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath");

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
                Assert.False(columnMasterKey.IsColumnMasterKeyPresentInDatabase(sqlConnection));
                Assert.False(columnEncryptionKey.IsColumnEncryptionKeyPresentInDatabase(sqlConnection));
                columnMasterKey.Create(sqlConnection);
                columnEncryptionKey.Create(sqlConnection);
                Assert.True(columnMasterKey.IsColumnMasterKeyPresentInDatabase(sqlConnection));
                Assert.True(columnEncryptionKey.IsColumnEncryptionKeyPresentInDatabase(sqlConnection));
                columnEncryptionKey.Drop(sqlConnection);
                columnMasterKey.Drop(sqlConnection);
                Assert.False(columnMasterKey.IsColumnMasterKeyPresentInDatabase(sqlConnection));
                Assert.False(columnEncryptionKey.IsColumnEncryptionKeyPresentInDatabase(sqlConnection));
            }
        }
    }
}
