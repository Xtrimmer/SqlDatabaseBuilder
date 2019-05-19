namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Money : Currency
    {
        public override string Definition { get; } = "money";
        public override int Size => 8;
    }
}
