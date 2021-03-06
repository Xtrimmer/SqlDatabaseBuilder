﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class UniqueConstraint : Constraint
    {
        private List<Tuple<Column, ColumnSort>> columns { get; set; } = new List<Tuple<Column, ColumnSort>>();

        public UniqueConstraint() : this(null) { }

        public UniqueConstraint(string name) : base(name) { }

        public UniqueConstraint(Column column, ColumnSort sortOrder = ColumnSort.ASC) : this(null, column, sortOrder) { }

        public UniqueConstraint(string name, Column column, ColumnSort sortOrder = ColumnSort.ASC) : base(name)
        {
            AddColumn(column, sortOrder);
        }

        public IndexType IndexType { get; set; } = IndexType.NONCLUSTERED;

        public UniqueConstraint AddColumn(Column column, ColumnSort sortOrder = ColumnSort.ASC)
        {
            columns.Add(new Tuple<Column, ColumnSort>(column, sortOrder));
            return this;
        }

        public UniqueConstraint AddColumns(params Column[] columns)
        {
            Array.ForEach(columns, c => this.columns.Add(Tuple.Create(c, ColumnSort.ASC)));
            return this;
        }

        public UniqueConstraint AddColumns(params Tuple<Column, ColumnSort>[] columns)
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
                return $"{constraintName}UNIQUE {indexType} ({columnDefinitions})";
            }
        }
    }
}