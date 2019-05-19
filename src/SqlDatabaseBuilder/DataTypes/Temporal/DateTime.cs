namespace Xtrimmer.SqlDatabaseBuilder
{
    class DateTime : Temporal
    {
        internal DateTime() { }

        public override string Definition => "datetime";

        public override int Size => 8;
    }
}
