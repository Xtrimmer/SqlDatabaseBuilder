﻿namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class VarChar : VariableCharacterSet
    {
        internal VarChar() { }

        internal VarChar(int n)
        {
            if (n == -1)
            {
                isMax = true;
            }
            else
            {
                N = n;
            }
        }

        protected override string TypeValue => "varchar";
        protected override int MaxN => 8000;
        public override int Size => isMax ? MAX_SIZE : (n + 2);
    }
}
