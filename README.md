# Xtrimmer SqlDatabaseBuilder

Xtrimmer SqlDatabaseBuilder is a simple library designed to help you build a Sql Server database using C#.

  - Write some SqlDatabaseBuilder code
  - Connect to a Sql Server Database server
  - Magic

# New Features!

  - Added Primary Key and Unique Key table constraint support

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
# SQL UNIQUE Constraint
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
