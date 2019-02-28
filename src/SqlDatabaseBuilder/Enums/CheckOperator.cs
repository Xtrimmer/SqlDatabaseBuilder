namespace Xtrimmer.SqlDatabaseBuilder
{
    public enum CheckOperator
    {
        [StringValue("<")]
        LessThan,
        [StringValue("<=")]
        LessThanOrEquals,
        [StringValue("=")]
        Equals,
        [StringValue("!=")]
        NotEquals,
        [StringValue(">=")]
        GreaterThanOrEquals,
        [StringValue(">")]
        GreaterThan
    }
}
