using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class PrimaryKeyConstraint : Constraint
    {
        private List<Tuple<Column, ColumnSort>> columns { get; set; } = new List<Tuple<Column, ColumnSort>>();

        public PrimaryKeyConstraint() : base(null) { }

        public PrimaryKeyConstraint(string name) : base(name) { }

        public PrimaryKeyConstraint(Column column, ColumnSort sortOrder = ColumnSort.ASC) : base(null)
        {
            AddColumn(column, sortOrder);
        }

        public PrimaryKeyConstraint(string name, Column column, ColumnSort sortOrder = ColumnSort.ASC) : base(name)
        {
            AddColumn(column, sortOrder);
        }

        public IndexType IndexType { get; set; } = IndexType.CLUSTERED;

        public PrimaryKeyConstraint AddColumn(Column column, ColumnSort sortOrder = ColumnSort.ASC)
        {
            columns.Add(new Tuple<Column, ColumnSort>(column, sortOrder));
            return this;
        }

        public PrimaryKeyConstraint AddColumns(params Column[] columns)
        {
            Array.ForEach(columns, c => this.columns.Add(Tuple.Create(c, ColumnSort.ASC)));
            return this;
        }

        public PrimaryKeyConstraint AddColumns(params Tuple<Column, ColumnSort>[] columns)
        {
            Array.ForEach(columns, c => this.columns.Add(c));
            return this;
        }

        internal override string SqlDefinition
        {
            get
            {
                string constraintName = Name == null ? "" : $"CONSTRAINT [{Name}] ";
                string indexType = IndexType.ToString();
                string columnDefinitions = string.Join(", ", columns.Select(t => $"[{t.Item1.Name}] {t.Item2.ToString()}").ToList());
                return $"{constraintName}PRIMARY KEY {indexType} ({columnDefinitions})";
            }
        }
    }
}