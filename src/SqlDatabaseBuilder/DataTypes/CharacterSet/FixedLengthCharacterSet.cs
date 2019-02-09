using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    abstract class FixedLengthCharacterSet : CharacterSet
    {
        protected override string NSpecification => n == 1 ? "" : $"({n.ToString()})";

        public int N
        {
            get { return n; }
            set
            {
                if (value < MinN || value > MaxN) throw new InvalidCharacterSetLength($"N value must be between {MinN} and {MaxN}");
                n = value;
            }
        }
    }
}
