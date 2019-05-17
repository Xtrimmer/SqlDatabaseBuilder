namespace Xtrimmer.SqlDatabaseBuilder
{
    internal class VarBinary : VariableCharacterSet
    {
        internal VarBinary() { }

        internal VarBinary(int n)
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

        protected override int MaxN => 8000;
        protected override string TypeValue => "varbinary";
        public override int Size => isMax ? MAX_SIZE : (n + 2);
    }
}
