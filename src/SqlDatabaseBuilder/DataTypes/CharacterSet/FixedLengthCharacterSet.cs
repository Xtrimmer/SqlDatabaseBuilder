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
                if (value < MinN || value > MaxN) throw new InvalidCharacterSetLengthException($"N value must be between {MinN} and {MaxN}");
                n = value;
            }
        }
    }
}
