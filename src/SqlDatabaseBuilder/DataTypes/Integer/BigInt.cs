namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class BigInt : Integer
    {
        public override string Definition { get; } = "bigint";
        public override int Size => 8;
    }
}
