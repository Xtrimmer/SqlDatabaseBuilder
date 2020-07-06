namespace Xtrimmer.SqlDatabaseBuilder
{
    public class Column : DatabaseObject
    {
        public string Collation { get; set; }

        public ColumnEncryption ColumnEncryption { get; set; }

        public DataType DataType { get; }

        public Default Default { get; set; }

        public bool Nullable { get; set; } = true;

        public Column(string name, DataType dataType) : base(name)
        {
            DataType = dataType;
        }

        internal override string SqlDefinition
        {
            get
            {
                string collation = string.IsNullOrWhiteSpace(Collation) ? "" : $" COLLATE {Collation}";
                string encryption = ColumnEncryption == null ? "" : ColumnEncryption.SqlDefinition;
                string defaultDefinition = Default == null ? "" : Default.SqlDefinition;
                string nullDefinition = Nullable ? "" : " NOT NULL";
                return $"[{Name}] {DataType.Definition}{collation}{encryption}{defaultDefinition}{nullDefinition}";
            }
        }
    }

    public class ColumnEncryption
    {
        public string ColumnEncryptionKeyName { get; set; }

        public ColumnEncryptionType ColumnEncryptionType { get; set; }

        public ColumnEncryption(ColumnEncryptionKey columnEncryptionKey, ColumnEncryptionType encryptionType)
            : this(columnEncryptionKey.Name, encryptionType) 
        {
            columnEncryptionKey.ThrowIfNull(nameof(columnEncryptionKey));
        }

        public ColumnEncryption(string columnEncryptionKeyName, ColumnEncryptionType encryptionType)
        {
            columnEncryptionKeyName.ThrowIfNull(nameof(columnEncryptionKeyName));

            ColumnEncryptionKeyName = columnEncryptionKeyName;
            ColumnEncryptionType = encryptionType;
        }

        internal string SqlDefinition => $" ENCRYPTED WITH (COLUMN_ENCRYPTION_KEY = [{ColumnEncryptionKeyName}], ENCRYPTION_TYPE = {ColumnEncryptionType.GetStringValue()}, ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256')";
    }
}
