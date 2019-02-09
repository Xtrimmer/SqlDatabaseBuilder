
using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal abstract class ExactFloatingPoint : DataType
    {
        private const int MAX_PRECISION = 38;
        private const int MIN_PRECISION = 1;
        private const int DEFAULT_PRECISION = 18;
        private const int MIN_SCALE = 0;
        private const int DEFAULT_SCALE = 0;

        private int precision = DEFAULT_PRECISION;

        public int Precision
        {
            get { return precision; }
            set
            {
                if (value < MIN_PRECISION || value > MAX_PRECISION) throw new InvalidPrecisionException($"Precision must be between {MIN_PRECISION} and {MAX_PRECISION}");
                precision = value;
            }
        }

        private int scale = DEFAULT_SCALE;

        public int Scale
        {
            get { return scale; }
            set
            {
                if (value < MIN_SCALE || value > Precision) throw new InvalidScaleException($"Scale must be between {MIN_SCALE} and the defined precision");
                scale = value;
            }
        }

        protected abstract string TypeValue { get; }

        public override string Definition
        {
            get
            {
                string s = Scale == DEFAULT_SCALE ? "" : $", {Scale}";
                string ps = Precision == DEFAULT_PRECISION ? "" : $"({Precision}{s})";
                return $"{TypeValue}{ps}";
            }
        }

        public override int Size
        {
            get
            {
                switch (Precision)
                {
                    case int n when (n <= 9):
                        return 5;
                    case int n when (n <= 19):
                        return 9;
                    case int n when (n <= 28):
                        return 13;
                    case int n when (n <= MAX_PRECISION):
                        return 17;
                    default:
                        return 0;
                }
            }
        }
    }
}
