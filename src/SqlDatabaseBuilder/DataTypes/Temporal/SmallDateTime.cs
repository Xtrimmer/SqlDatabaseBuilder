namespace Xtrimmer.SqlDatabaseBuilder
{
    class SmallDateTime : Temporal
    {
        internal SmallDateTime() { }

        public override string Definition => "smalldatetime";

        public override int Size => 4;
    }
}