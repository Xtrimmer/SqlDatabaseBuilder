namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Time : Temporal
    {
        private const int MAX_SCALE = 7;
        private const int MIN_SCALE = 0;
        private const int DEFAULT_SCALE = 7;

        private int scale = DEFAULT_SCALE;

        internal Time() { }

        internal Time(int scale)
        {
            Scale = scale;
        }

        internal int Scale
        {
            get { return scale; }
            set
            {
                if (value < MIN_SCALE || value > MAX_SCALE) throw new InvalidScaleException($"Scale must be between {MIN_SCALE} and {MAX_SCALE}");
                scale = value;
            }
        }

        public override string Definition
        {
            get
            {
                string scaleSpecification = scale == 7 ? "" : $"({scale.ToString()})";
                return $"time{scaleSpecification}";
            }
        }

        public override int Size
        {
            get
            {
                switch (Scale)
                {
                    case int n when (n <= 2):
                        return 3;
                    case int n when (n <= 4):
                        return 4;
                    case int n when (n <= MAX_SCALE):
                        return 5;
                    default:
                        return 0;
                }
            }
        }
    }
}
