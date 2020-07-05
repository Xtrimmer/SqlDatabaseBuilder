using System;
using System.Data.SqlClient;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Manual
{
    public class ColumnMasterKeyShould
    {
        private readonly string connectionString = Environment.GetEnvironmentVariable("AzureSqlServerPath");

        [Fact]
        public void CreateAndDropColumnMasterKey()
        {
            string cmkName = nameof(CreateAndDropColumnMasterKey);
            string keyPath = "CurrentUser/My/BBF037EC4A133ADCA89FFAEC16CA5BFA8878FB94";
            ColumnMasterKey cmk = new ColumnMasterKey(cmkName, KeyStoreProvider.WindowsCertificateStoreProvider, keyPath);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                Assert.False(cmk.IsColumnMasterKeyPresentInDatabase(sqlConnection));
                cmk.Create(sqlConnection);
                Assert.True(cmk.IsColumnMasterKeyPresentInDatabase(sqlConnection));
                cmk.Drop(sqlConnection);
                Assert.False(cmk.IsColumnMasterKeyPresentInDatabase(sqlConnection));
            }
        }
    }
}
