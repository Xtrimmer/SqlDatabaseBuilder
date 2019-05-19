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

        public override string Definition => $"{TypeValue}{NSpecification}";
    }
}
