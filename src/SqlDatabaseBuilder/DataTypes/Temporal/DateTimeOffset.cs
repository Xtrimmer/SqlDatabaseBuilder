namespace Xtrimmer.SqlDatabaseBuilder
{
    class DateTimeOffset : ScaledTemporal
    {
        internal DateTimeOffset() { }

        internal DateTimeOffset(int scale)
        {
            Scale = scale;
        }

        public override string Definition
        {
            get
            {
                string scaleSpecification = Scale == DefalutScale ? "" : $"({Scale})";
                return $"datetimeoffset{scaleSpecification}";
            }
        }

        public override int Size
        {
            get
            {
                switch (Scale)
                {
                    case int n when (n <= 2):
                        return 8;
                    case int n when (n <= 4):
                        return 9;
                    case int n when (n <= MaxScale):
                        return 10;
                    default:
                        return 0;
                }
            }
        }
    }
}
