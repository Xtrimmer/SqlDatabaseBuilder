namespace Xtrimmer.SqlDatabaseBuilder
{
    public class Column : DatabaseObject
    {
        public DataType DataType { get; }

        public bool Nullable { get; set; } = true;

        public Default Default { get; set; }



        public Column(string name, DataType dataType) : base(name)
        {
            DataType = dataType;
        }

        internal override string SqlDefinition
        {
            get
            {
                string defaultDefinition = Default == null ? "" : Default.SqlDefinition;
                string nullDefinition = Nullable ? "" : " NOT NULL";
                return $"[{Name}] {DataType.Definition}{defaultDefinition}{nullDefinition}";
            }
        }
    }
}
