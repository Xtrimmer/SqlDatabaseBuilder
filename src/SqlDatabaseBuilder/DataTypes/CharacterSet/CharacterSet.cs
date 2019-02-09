
using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    abstract class CharacterSet : DataType
    {
        protected const int DEFAULT_N = 1;
        protected int n = DEFAULT_N;
        protected int MinN { get; } = 1;
        protected abstract int MaxN { get; }
        protected abstract string NSpecification { get; }
        protected abstract string TypeValue { get; }

        public int N
        {
            get { return n; }
            set
            {
                if (value < MinN || value > MaxN) throw new InvalidCharacterSetength($"N value must be between {MinN} and {MaxN}");
                n = value;
            }
        }

        public override string Definition => $"{TypeValue}{NSpecification}";
    }
}
