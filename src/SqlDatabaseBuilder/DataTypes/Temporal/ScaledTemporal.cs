namespace Xtrimmer.SqlDatabaseBuilder
{
    abstract class ScaledTemporal : Temporal
    {
        protected const int MaxScale = 7;
        protected const int MinScale = 0;
        protected const int DefalutScale = 7;

        private int scale = DefalutScale;

        public int Scale
        {
            get => scale;
            set
            {
                if (value < MinScale || value > MaxScale) throw new InvalidScaleException($"Scale must be between {MinScale} and {MaxScale}");
                scale = value;
            }
        }

        protected ScaledTemporal() { }

        protected ScaledTemporal(int scale)
        {
            Scale = scale;
        }
    }
}
