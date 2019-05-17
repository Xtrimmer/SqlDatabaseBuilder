namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Bit : Integer
    {
        public override string Definition => "bit";
        public override int Size => 1;
    }
}
