namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class TinyInt : Integer
    {
        public override string Definition => "tinyint";
        public override int Size => 1;
    }
}
