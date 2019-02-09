
using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    class DateTime2 : Temporal
    {
        private const int MAX_PRECISION = 7;
        private const int MIN_PRECISION = 0;
        private const int DEFAULT_PRECISION = 7;

        private int precision = DEFAULT_PRECISION;

        internal DateTime2() { }

        internal DateTime2(int precision)
        {
            Precision = precision;
        }

        public int Precision
        {
            get { return precision; }
            set
            {
                if (value < MIN_PRECISION || value > MAX_PRECISION) throw new InvalidPrecisionException($"Precision must be between {MIN_PRECISION} and {MAX_PRECISION}");
                precision = value;
            }
        }

        public override string Definition
        {
            get
            {                
                string precisionSpecification = Precision == DEFAULT_PRECISION ? "" : $"({Precision})";
                return $"datetime2{precisionSpecification}";
            }
        }

        public override int Size
        {
            get
            {
                switch (Precision)
                {
                    case int n when (n <= 2):
                        return 6;
                    case int n when (n <= 4):
                        return 7;
                    case int n when (n <= MAX_PRECISION):
                        return 8;
                    default:
                        return 0;
                }
            }
        }
    }
}
