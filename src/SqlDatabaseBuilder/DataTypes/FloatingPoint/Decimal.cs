
using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Decimal : ExactFloatingPoint
    {

        internal Decimal()
        { }

        internal Decimal(int precision)
        {
            Precision = precision;
        }

        internal Decimal(int precision, int scale) : this(precision)
        {
            Scale = scale;
        }

        protected override string TypeValue { get; } = "decimal";
    }
}
