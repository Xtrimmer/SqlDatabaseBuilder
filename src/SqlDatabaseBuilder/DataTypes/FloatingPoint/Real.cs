namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Real : AproximateFloatingPoint
    {
        private const int REAL_MANTISSA_VALUE = 24;

        internal Real()
        {
            Mantissa = REAL_MANTISSA_VALUE;
        }

        protected override string TypeValue => "real";

        protected override int DefaultMantissa => 24;
    }
}
