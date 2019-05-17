using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class Index : DatabaseResource
    {
        private Table table;
        private List<Tuple<Column, ColumnSort>> columns = new List<Tuple<Column, ColumnSort>>();

        public Index(string name, Table table, params Column[] columns) :
            this(name, table, columns.Select(c => Tuple.Create(c, ColumnSort.ASC)).ToArray())
        { }

        public Index(string name, Table table, params Tuple<Column, ColumnSort>[] columns) : base(name)
        {
            name.ThrowIfNull(nameof(name));
            table.ThrowIfNull(nameof(table));

            this.table = table;
            foreach (var t in columns)
            {
                this.columns.Add(t);
            }
        }

        public bool IsUnique { get; set; } = false;

        public IndexType IndexType { get; set; } = IndexType.NONCLUSTERED;

        internal override string SqlDefinition
        {
            get
            {
                string uniqueness = IsUnique ? "UNIQUE " : "";
                string indexType = IndexType.ToString();
                string columnDefinitions = string.Join(", ", columns.Select(t => $"[{t.Item1.Name}] {t.Item2.ToString()}").ToList());
                return $"CREATE {uniqueness}{indexType} INDEX [{Name}] ON [{table.Name}] ({columnDefinitions})";
            }
        }

        public override void Create(SqlConnection sqlConnection)
        {
            if (columns.Count == 0) throw new InvalidIndexDefinitionException("Index must specify at least one column.");
            sqlConnection.ThrowIfNull(nameof(sqlConnection));
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = SqlDefinition;
                sqlCommand.ExecuteNonQuery();
            }
        }

        public override void Drop(SqlConnection sqlConnection)
        {
            sqlConnection.ThrowIfNull(nameof(sqlConnection));
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = $"DROP INDEX [{Name}] ON [{table.Name}]";
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
