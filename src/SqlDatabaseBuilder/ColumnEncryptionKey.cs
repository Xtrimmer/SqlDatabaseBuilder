using System.Data.SqlClient;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class ColumnEncryptionKey : DatabaseResource
    {
        public string ColumnMasterKeyName { get; set; }

        public string EncryptedValue { get; set; }

        public ColumnEncryptionKey(string keyName, ColumnMasterKey columnMasterKey, string encryptedValue)
            : this(keyName, columnMasterKey.Name, encryptedValue) { }

        public ColumnEncryptionKey(string keyName, string columnMasterKeyName, string encryptedValue) : base(keyName)
        {
            ColumnMasterKeyName = columnMasterKeyName;
            EncryptedValue = encryptedValue;
        }

        public override void Create(SqlConnection sqlConnection)
        {
            sqlConnection.ThrowIfNull(nameof(sqlConnection));
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = SqlDefinition;
                sqlCommand.ExecuteNonQuery();
            }
        }

        public override void Drop(SqlConnection sqlConnection)
        {
            sqlConnection.ThrowIfNull(nameof(sqlConnection));
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = $"DROP COLUMN ENCRYPTION KEY [{Name}];";
                sqlCommand.ExecuteNonQuery();
            }
        }

        internal override string SqlDefinition => $"CREATE COLUMN ENCRYPTION KEY [{Name}] WITH VALUES (COLUMN_MASTER_KEY = {ColumnMasterKeyName}, ALGORITHM = 'RSA_OAEP', ENCRYPTED_VALUE = {EncryptedValue})";
    }
}
