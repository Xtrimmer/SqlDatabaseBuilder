namespace Xtrimmer.SqlDatabaseBuilder
{
    class DateTime2 : ScaledTemporal
    {
        internal DateTime2() { }

        internal DateTime2(int scale)
        {
            Scale = scale;
        }

        public override string Definition
        {
            get
            {
                string scaleSpecification = Scale == DefalutScale ? "" : $"({Scale})";
                return $"datetime2{scaleSpecification}";
            }
        }

        public override int Size
        {
            get
            {
                switch (Scale)
                {
                    case int n when (n <= 2):
                        return 6;
                    case int n when (n <= 4):
                        return 7;
                    case int n when (n <= MaxScale):
                        return 8;
                    default:
                        return 0;
                }
            }
        }
    }
}
