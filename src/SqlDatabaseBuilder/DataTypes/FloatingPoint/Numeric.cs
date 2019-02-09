using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class Numeric : ExactFloatingPoint
    {
        internal Numeric()
        { }

        internal Numeric(int precision)
        {
            Precision = precision;
        }

        internal Numeric(int precision, int scale) : this(precision)
        {
            Scale = scale;
        }

        protected override string TypeValue { get; } = "numeric";
    }
}
