namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class SmallInt : Integer
    {
        public override string Definition => "smallint";
        public override int Size => 2;
    }
}
