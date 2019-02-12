using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class Column : DatabaseObject
    {
        public DataType DataType { get; }
        
        public bool Nullable { get; set; } = true;

        public Column(string name, DataType dataType) : base(name)
        {
            DataType = dataType;
        }

        internal override string SqlDefinition
        {
            get
            {
                string nullDefinition = Nullable ? "" : " NOT NULL";
                return $"[{Name}] {DataType.Definition}{nullDefinition}";
            }
        }
    }
}
