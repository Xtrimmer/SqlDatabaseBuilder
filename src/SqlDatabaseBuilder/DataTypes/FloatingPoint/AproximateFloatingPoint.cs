namespace Xtrimmer.SqlDatabaseBuilder
{
    internal abstract class AproximateFloatingPoint : DataType
    {
        private const int MIN_MANTISSA = 1;
        private const int MAX_MANTISSA = 53;

        private int mantissa;

        protected int Mantissa
        {
            get { return mantissa; }
            set
            {
                if (value < MIN_MANTISSA || value > MAX_MANTISSA) throw new InvalidMantissaException($"Mantissa must be between {MIN_MANTISSA} and {MAX_MANTISSA}");
                mantissa = value;
            }
        }

        protected abstract string TypeValue { get; }
        protected abstract int DefaultMantissa { get; }

        public override string Definition
        {
            get
            {
                string mantissaPart = Mantissa == DefaultMantissa ? "" : $"({Mantissa})";
                return $"{TypeValue}{mantissaPart}";
            }
        }

        public override int Size => Mantissa < 25 ? 4 : 8;
    }
}
