using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    abstract class VariableCharacterSet : CharacterSet
    {
        protected const int MAX_SIZE = int.MaxValue;
        protected bool isMax = false;

        protected override string NSpecification
        {
            get
            {
                string nDefinition = n == 1 ? "" : $"({n.ToString()})";
                return isMax ? "(max)" : nDefinition;
            }
        }

        public int N
        {
            get { return n; }
            set
            {
                if (value != MAX && (value < MinN || value > MaxN)) throw new InvalidCharacterSetLength($"N value must be between {MinN} and {MaxN} or DataType.MAX");
                n = value;
            }
        }
    }
}
