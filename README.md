[![Build Status](https://travis-ci.org/Xtrimmer/SqlDatabaseBuilder.svg?branch=master)](https://travis-ci.org/Xtrimmer/SqlDatabaseBuilder)
[![NuGet Version](https://badge.fury.io/nu/SqlDatabaseBuilder.svg)](https://badge.fury.io/nu/SqlDatabaseBuilder)
# Xtrimmer SqlDatabaseBuilder

Contributors:
  - Jeff Trimmer
  - Mauricio Beithia
  
Xtrimmer SqlDatabaseBuilder is a simple library designed to help you build a Sql Server database using C#.

  - Write some SqlDatabaseBuilder code
  - Connect to a Sql Server Database server
  - Magic

# New Features!

  - Added Primary Key, Foreign key, Unique Key, and Check constraint support
  - Added Index support

# SQL CREATE DATABASE
The DATABASE object is used to create a new SQL database.
```csharp
    Database database = new Database("MyDatabase");

    using (SqlConnection sqlConnection = new SqlConnection("Server=myServerAddress;Database=myDataBase;"))
    {
        sqlConnection.Open();
        database.Create(sqlConnection);
    }
```

# SQL CREATE TABLE
The Table object is used to create database tables.
```csharp
    Table table = new Table("Persons");
    table.Columns.AddAll(
        new Column("PersonId", DataType.Int()),
        new Column("LastName", DataType.VarChar(255)),
        new Column("FirstName", DataType.VarChar(255)),
        new Column("Address", DataType.VarChar(255)),
        new Column("City", DataType.VarChar(255))
    );

    using (SqlConnection sqlConnection = new SqlConnection("Server=myServerAddress;Database=myDataBase;"))
    {
        sqlConnection.Open();
        table.Create(sqlConnection);
    }
```

# SQL NULL/NOT NULL Constraint
By default, a column can hold NULL values.
The NOT NULL constraint enforces a column to NOT accept NULL values.
```csharp
    Table table = new Table("Persons");

    Column personId = new Column("PersonId", DataType.Int());
    personId.Nullable = false; // NOT NULL

    Column lastName = new Column("LastName", DataType.VarChar(255));
    lastName.Nullable = false; // NOT NULL

    Column firstName = new Column("FirstName", DataType.VarChar(255));
    lastName.Nullable = true; // NULL

    Column age = new Column("Age", DataType.Int()); // defaults to NULL if not specified

    table.Columns.AddAll(personId, lastName, firstName, age);

    using (SqlConnection sqlConnection = new SqlConnection("Server=myServerAddress;Database=myDataBase;"))
    {
        sqlConnection.Open();
        table.Create(sqlConnection);
    }
```

# SQL PRIMARY KEY Constraint
The PRIMARY KEY constraint uniquely identifies each record in a table.
Primary keys must contain UNIQUE values, and cannot contain NULL values.
A table can have only one primary key, which may consist of single or multiple fields.
```csharp
    Table table = new Table("Persons");

    Column id = new Column("Id", DataType.Int()) { Nullable = false };
    Column lastName = new Column("LastName", DataType.VarChar(255)) { Nullable = false };
    Column firstName = new Column("FirstName", DataType.VarChar(255));
    Column age = new Column("Age", DataType.Int());

    table.Columns.AddAll(id, lastName, firstName, age);

    table.Constraints.Add(new PrimaryKeyConstraint(id));

    using (SqlConnection sqlConnection = new SqlConnection("Server=myServerAddress;Database=myDataBase;"))
    {
        sqlConnection.Open();
        table.Create(sqlConnection);
    }
```
To allow naming of a PRIMARY KEY constraint, and for defining a PRIMARY KEY constraint on multiple columns, use the following
```csharp
    Table table = new Table("Persons");
    Column id = new Column("Id", DataType.Int()) { Nullable = false };
    Column lastName = new Column("LastName", DataType.VarChar(255)) { Nullable = false };
    Column firstName = new Column("FirstName", DataType.VarChar(255));
    Column age = new Column("Age", DataType.Int());
    table.Columns.AddAll(id, lastName, firstName, age);

    PrimaryKeyConstraint primaryKeyConstraint = new PrimaryKeyConstraint(name: "PK_PERSONS");
    primaryKeyConstraint.AddColumns(id, lastName);
    table.Constraints.Add(primaryKeyConstraint);
```
The sorting order can also be defined like this:
```csharp
    PrimaryKeyConstraint primaryKeyConstraint = new PrimaryKeyConstraint(name: "PK_PERSONS");
    primaryKeyConstraint.AddColumn(id, ColumnSort.ASC);
    primaryKeyConstraint.AddColumn(lastName, ColumnSort.DESC);
    table.Constraints.Add(primaryKeyConstraint);
```
# SQL FOREIGN KEY Constraint
A FOREIGN KEY is a key used to link two tables together.
A FOREIGN KEY is a field (or collection of fields) in one table that refers to the PRIMARY KEY in another table.
```csharp
    Table personsTable = new Table("Persons");
    Column id = new Column("Id", DataType.Int()) { Nullable = false };
    Column lastName = new Column("LastName", DataType.VarChar(255));
    Column firstName = new Column("FirstName", DataType.VarChar(255));
    Column age = new Column("Age", DataType.Int());
    personsTable.Columns.AddAll(id, lastName, firstName, age);
    personsTable.Constraints.Add(new PrimaryKeyConstraint(id));

    Table OrdersTable = new Table("Orders");
    Column orderId = new Column("OrderId", DataType.Int());
    Column orderNumber = new Column("OrderNumber", DataType.Int());
    Column personId = new Column("PersonId", DataType.Int());
    OrdersTable.Columns.AddAll(orderId, orderNumber, personId);

    ForeignKeyConstraint foreignKeyConstraint = new ForeignKeyConstraint();
    foreignKeyConstraint.AddColumn(personId)
        .References(personsTable)
        .AddReferenceColumn(id);
    OrdersTable.Constraints.Add(foreignKeyConstraint);
```
# SQL UNIQUE Constraint
The UNIQUE constraint ensures that all values in a column are different.
Both the UNIQUE and PRIMARY KEY constraints provide a guarantee for uniqueness for a column or set of columns.
A PRIMARY KEY constraint automatically has a UNIQUE constraint.
However, you can have many UNIQUE constraints per table, but only one PRIMARY KEY constraint per table.
The following creates a UNIQUE constraint on the "ID" column when the "Persons" table is created:
```csharp
    Table table = new Table("Persons");

    Column id = new Column("Id", DataType.Int()) { Nullable = false };
    Column lastName = new Column("LastName", DataType.VarChar(255)) { Nullable = false };
    Column firstName = new Column("FirstName", DataType.VarChar(255));
    Column age = new Column("Age", DataType.Int());

    table.Columns.AddAll(id, lastName, firstName, age);

    table.Constraints.Add(new UniqueConstraint(id));

    using (SqlConnection sqlConnection = new SqlConnection("Server=myServerAddress;Database=myDataBase;"))
    {
        sqlConnection.Open();
        table.Create(sqlConnection);
    }
```
To name a UNIQUE constraint, and to define a UNIQUE constraint on multiple columns, use the following
```csharp
    Table table = new Table("Persons");
    Column id = new Column("Id", DataType.Int()) { Nullable = false };
    Column lastName = new Column("LastName", DataType.VarChar(255)) { Nullable = false };
    Column firstName = new Column("FirstName", DataType.VarChar(255));
    Column age = new Column("Age", DataType.Int());
    table.Columns.AddAll(id, lastName, firstName, age);

    UniqueConstraint uniqueConstraint = new UniqueConstraint(name: "UQ_PERSONS");
    uniqueConstraint.AddColumns(id, lastName);
    table.Constraints.Add(uniqueConstraint);
```
The sorting order can also be defined like this:
```csharp
    UniqueConstraint uniqueConstraint = new UniqueConstraint(name: "UQ_PERSONS");
    uniqueConstraint.AddColumn(id, ColumnSort.ASC);
    uniqueConstraint.AddColumn(lastName, ColumnSort.DESC);
    table.Constraints.Add(uniqueConstraint);
```
# SQL CHECK Constraint
The CHECK constraint is used to limit the value range that can be placed in a column.
```csharp
    Table table = new Table("Persons");
    Column id = new Column("Id", DataType.Int()) { Nullable = false };
    Column lastName = new Column("LastName", DataType.VarChar(255));
    Column firstName = new Column("FirstName", DataType.VarChar(255));
    Column age = new Column("Age", DataType.Int());
    Column city = new Column("City", DataType.VarChar(255));
    table.Columns.AddAll(id, lastName, firstName, age, city);

    CheckExpression checkExpression = new CheckExpression(age, CheckOperator.GreaterThanOrEquals, "18");
    CheckConstraint checkConstraint = new CheckConstraint(checkExpression);
    table.Constraints.Add(checkConstraint);

    using (SqlConnection sqlConnection = new SqlConnection("Server=myServerAddress;Database=myDataBase;"))
    {
        sqlConnection.Open();
        table.Create(sqlConnection);
    }
```
To allow naming of a CHECK constraint, and for defining a CHECK constraint on multiple columns, use the following:
```csharp
    CheckConstraint checkConstraint = new CheckConstraint("CHK_PersonAgeCity")
    {
        CheckExpression = new CheckExpression(age, CheckOperator.GreaterThanOrEquals, "18")
                        .And(city, CheckOperator.Equals, "'Seattle'")
    };
```
For even more complex expressions:
```csharp
    CheckConstraint checkConstraint = new CheckConstraint("CHK_PersonAgeCity")
    {
        CheckExpression = new CheckExpression("(City IN ('Sealttle','Kansas City','Dallas') OR UPPER(FirstName) LIKE 'J%') AND Address IS NOT NULL")
    };
```
# SQL CREATE INDEX
The CREATE INDEX statement is used to create indexes in tables.
```csharp
    Index index = new Index("IndexName", table, column1, column2);

    using (SqlConnection sqlConnection = new SqlConnection("Server=myServerAddress;Database=myDataBase;"))
    {
        sqlConnection.Open();
        index.Create(sqlConnection);
    }
```
To create a clustered and/or unique index
```csharp
    Index index = new Index("IndexName", table, column1, column2)
    {
        IndexType = IndexType.CLUSTERED,
        IsUnique = true
    };
```
To set column sort orders
```csharp
    Index index = new Index("IndexName", table, Tuple.Create(column1, ColumnSort.DESC));
```