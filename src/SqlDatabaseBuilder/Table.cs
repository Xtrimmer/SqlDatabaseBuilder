﻿
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class Table : DatabaseResource
    {
        public List<Column> Columns { get; set; } = new List<Column>();

        public Table(string name) : base(name)
        {
        }

        public override void Create(SqlConnection sqlConnection)
        {
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = SqlDefinition;
                sqlCommand.ExecuteNonQuery();
            }
        }

        public override void Drop(SqlConnection sqlConnection)
        {
            string sql = $"DROP TABLE [{Name}]";

            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                sqlCommand.CommandText = sql;
                sqlCommand.ExecuteNonQuery();
            }
        }

        internal override string SqlDefinition
        {
            get
            {
                if (Columns.Count() == 0) throw new InvalidTableDefinitionException("Table must specify at least one attribute.");

                string columnDefinitions = string.Join(", ", Columns.Select(c => c.SqlDefinition).ToList());
                return $"CREATE TABLE [{Name}] ({columnDefinitions})";
            }
        }
    }
}
