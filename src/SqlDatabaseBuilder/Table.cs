using System.Data.SqlClient;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public class Table : DatabaseResource
    {
        public ConstraintCollection Constraints { get; set; } = new ConstraintCollection();
        public DatabaseObjectCollection<Column> Columns { get; set; } = new DatabaseObjectCollection<Column>();

        public Table(string name) : base(name)
        {
            if (name == null) throw new InvalidDatabaseIdentifierException("Table name cannot be null");
        }

        public override void Create(SqlConnection sqlConnection)
        {
            if (Columns.isEmpty()) throw new InvalidTableDefinitionException("Table must specify at least one column.");
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
                string columnDefinitions = Columns.SqlDefinition;
                string constraintDefinitions = Constraints.isEmpty() ? "" : $", {Constraints.SqlDefinition}";
                return $"CREATE TABLE [{Name}] ({columnDefinitions}{constraintDefinitions});";
            }
        }
    }
}
