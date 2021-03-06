﻿namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Time : ScaledTemporal
    {
        internal Time() { }

        internal Time(int scale)
        {
            Scale = scale;
        }

        public override string Definition
        {
            get
            {
                string scaleSpecification = Scale == DefalutScale ? "" : $"({Scale})";
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
                    case int n when (n <= MaxScale):
                        return 5;
                    default:
                        return 0;
                }
            }
        }
    }
}
