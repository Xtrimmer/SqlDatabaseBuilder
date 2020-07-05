using System.Data.SqlClient;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class ColumnEncryptionKey : DatabaseResource
    {
        public string ColumnMasterKeyName { get; set; }

        public string EncryptedValue { get; set; }

        public ColumnEncryptionKey(string keyName, string columnMasterKeyName, string encryptedValue) : base(keyName)
        {
            ColumnMasterKeyName = columnMasterKeyName;
            EncryptedValue = encryptedValue;
        }

        public override void Create(SqlConnection sqlConnection)
        {
            throw new System.NotImplementedException();
        }

        public override void Drop(SqlConnection sqlConnection)
        {
            throw new System.NotImplementedException();
        }

        internal override string SqlDefinition => throw new System.NotImplementedException();
    }
}
