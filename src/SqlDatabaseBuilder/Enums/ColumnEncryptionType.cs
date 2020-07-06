namespace Xtrimmer.SqlDatabaseBuilder
{
    public enum ColumnEncryptionType
    {
        [StringValue("DETERMINISTIC")]
        Deterministic,
        [StringValue("RANDOMIZED")]
        Randomized
    }
}
