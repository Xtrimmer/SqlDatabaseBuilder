using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{

    internal class NVarChar : VariableCharacterSet
    {
        internal NVarChar() { }

        internal NVarChar(int n)
        {
            if (n == -1)
            {
                isMax = true;
            }
            else
            {
                this.n = n;
            }
        }

        protected override string TypeValue => "nvarchar";
        protected override int MaxN => 4000;
        public override int Size => isMax ? MAX_SIZE : (2 * n) + 2;
    }
}
