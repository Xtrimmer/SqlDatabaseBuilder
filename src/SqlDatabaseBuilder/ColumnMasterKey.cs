using Microsoft.Data.SqlClient;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class ColumnMasterKey : DatabaseResource
    {
        public string KeyStoreProviderName { get; set; }

        public string KeyPath { get; set; }

        public bool IsEnclaveEnabled { get; set; } = false;

        public string Signature { get; set; }

        public ColumnMasterKey(string keyName, string keyStoreProviderName, string keyPath) : base(keyName)
        {
            KeyStoreProviderName = keyStoreProviderName;
            KeyPath = keyPath;
        }

        public override void Create(SqlConnection sqlConnection)
        {
            if (IsEnclaveEnabled && string.IsNullOrWhiteSpace(Signature)) throw new InvalidColumnMasterKeyDefinitionException("Enclave enabled keys require a signature.");
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
                sqlCommand.CommandText = $"DROP COLUMN MASTER KEY [{Name}];";
                sqlCommand.ExecuteNonQuery();
            }
        }

        internal override string SqlDefinition
        {
            get
            {
                string enclaveComputations = IsEnclaveEnabled ? $", ENCLAVE_COMPUTATIONS (SIGNATURE = {Signature})" : "";
                return $"CREATE COLUMN MASTER KEY [{Name}] WITH(KEY_STORE_PROVIDER_NAME = '{KeyStoreProviderName}', KEY_PATH = '{KeyPath}'{enclaveComputations});";
            }
        }
    }

    public static class KeyStoreProvider
    {
        public const string WindowsCertificateStoreProvider = "MSSQL_CERTIFICATE_STORE";
        public const string MicrosoftCryptoApiProvider = "MSSQL_CSP_PROVIDER";
        public const string CryptographyApiNextGenerationProvider = "MSSQL_CSP_PROVIDER";
        public const string AzureKeyVaultProvider = "AZURE_KEY_VAULT";
        public const string JavaKeyStore = "MSSQL_JAVA_KEYSTORE";
    }
}
