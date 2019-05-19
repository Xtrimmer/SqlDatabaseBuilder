namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Float : AproximateFloatingPoint
    {
        internal Float()
        {
            Mantissa = DefaultMantissa;
        }

        internal Float(int mantissa)
        {
            Mantissa = mantissa;
        }

        protected override string TypeValue => "float";

        protected override int DefaultMantissa => 53;
    }
}
