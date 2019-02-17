using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class ForeignKeyConstraint : Constraint
    {
        private List<Column> localColumns = new List<Column>();
        private List<Column> referenceColumns = new List<Column>();
        private Table referenceTable;

        public ForeignKeyConstraint() : base(null) { }

        public ForeignKeyConstraint(string name) : base(name) { }

        public ForeignKeyConstraint AddColumn(Column column)
        {
            localColumns.Add(column);
            return this;
        }

        public ForeignKeyConstraint AddColumns(params Column[] columns)
        {
            Array.ForEach(columns, c => this.localColumns.Add(c));
            return this;
        }

        public ForeignKeyConstraint References(Table table)
        {
            referenceTable = table;
            return this;
        }

        public ForeignKeyConstraint AddReferenceColumn(Column column)
        {
            referenceColumns.Add(column);
            return this;
        }

        public ForeignKeyConstraint AddReferenceColumns(params Column[] columns)
        {
            Array.ForEach(columns, c => referenceColumns.Add(c));
            return this;
        }

        internal override string SqlDefinition
        {
            get
            {
                string constraintName = Name == null ? "" : $"CONSTRAINT [{Name}] ";
                string columnNames = string.Join(", ", localColumns.Select(c => $"[{c.Name}]").ToList());
                string referenceColumnNames = string.Join(", ", referenceColumns.Select(c => c.Name).ToList());
                referenceColumnNames = referenceColumns.Any() ? $" ({referenceColumnNames})" : "";
                return $"{constraintName}FOREIGN KEY ({columnNames}) REFERENCES {referenceTable.Name}{referenceColumnNames}";
            }
        }
    }
}