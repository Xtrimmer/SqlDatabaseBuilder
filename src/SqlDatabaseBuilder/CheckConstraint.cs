namespace Xtrimmer.SqlDatabaseBuilder
{
    public class CheckConstraint : Constraint
    {
        public CheckConstraint() : base(null) { }

        public CheckConstraint(string name) : base(name) { }

        public CheckConstraint(CheckExpression checkExpression) : this(null, checkExpression) { }

        public CheckConstraint(string name, CheckExpression checkExpression) : base(name)
        {
            CheckExpression = checkExpression;
        }
        public CheckExpression CheckExpression { get; set; }

        internal override string SqlDefinition
        {
            get
            {
                string constraintName = Name == null ? "" : $"CONSTRAINT [{Name}] ";
                return $"{constraintName}CHECK ({CheckExpression.SqlDefinition})";
            }
        }
    }

    public class CheckExpression
    {
        public CheckExpression(string expression)
        {
            SqlDefinition = expression;
        }

        public CheckExpression(Column column, CheckOperator checkOperator, object value) : this($"[{column.Name}] {checkOperator.GetStringValue()} {value.ToString()}") { }

        public CheckExpression(Column thisColumn, CheckOperator checkOperator, Column thatColumn) : this(thisColumn, checkOperator, $"[{thatColumn.Name}]") { }

        public CheckExpression And(string expression)
        {
            SqlDefinition = string.Concat(SqlDefinition.Trim(), " AND ", expression);
            return this;
        }

        public CheckExpression And(CheckExpression checkExpression)
        {
            SqlDefinition = string.Concat(SqlDefinition.Trim(), " AND ", checkExpression.SqlDefinition.Trim());
            return this;
        }

        public CheckExpression And(Column column, CheckOperator checkOperator, object value)
        {
            SqlDefinition = string.Concat(SqlDefinition.Trim(), " AND ", $"[{column.Name}] {checkOperator.GetStringValue()} {value.ToString()}");
            return this;
        }

        public CheckExpression And(Column thisColumn, CheckOperator checkOperator, Column thatColumn)
        {
            SqlDefinition = string.Concat(SqlDefinition.Trim(), " AND ", $"[{thisColumn.Name}] {checkOperator.GetStringValue()} [{thatColumn.Name}]");
            return this;
        }

        public CheckExpression Or(string expression)
        {
            SqlDefinition = string.Concat(SqlDefinition.Trim(), " OR ", expression);
            return this;
        }

        public CheckExpression Or(CheckExpression checkExpression)
        {
            SqlDefinition = string.Concat(SqlDefinition.Trim(), " OR ", checkExpression.SqlDefinition.Trim());
            return this;
        }

        public CheckExpression Or(Column column, CheckOperator checkOperator, object value)
        {
            SqlDefinition = string.Concat(SqlDefinition.Trim(), " OR ", $"[{column.Name}] {checkOperator.GetStringValue()} {value.ToString()}");
            return this;
        }

        public CheckExpression Or(Column thisColumn, CheckOperator checkOperator, Column thatColumn)
        {
            SqlDefinition = string.Concat(SqlDefinition.Trim(), " OR ", $"[{thisColumn.Name}] {checkOperator.GetStringValue()} [{thatColumn.Name}]");
            return this;
        }

        internal string SqlDefinition { get; set; }
    }
}
