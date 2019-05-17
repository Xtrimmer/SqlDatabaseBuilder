namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class SmallMoney : Currency
    {
        public override string Definition { get; } = "smallmoney";
        public override int Size => 4;
    }
}
