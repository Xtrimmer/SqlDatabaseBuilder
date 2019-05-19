namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Date : Temporal
    {
        internal Date() { }

        public override string Definition => "date";

        public override int Size => 3;
    }
}
