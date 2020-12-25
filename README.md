
[![Build Status](https://dev.azure.com/Xtrimmer/SqlDatabaseBuilder/_apis/build/status/Xtrimmer.SqlDatabaseBuilder?branchName=master)](https://dev.azure.com/Xtrimmer/SqlDatabaseBuilder/_build/latest?definitionId=4&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SqlDatabaseBuilder&metric=alert_status)](https://sonarcloud.io/dashboard?id=SqlDatabaseBuilder)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=SqlDatabaseBuilder&metric=coverage)](https://sonarcloud.io/dashboard?id=SqlDatabaseBuilder)  
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=SqlDatabaseBuilder&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=SqlDatabaseBuilder)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=SqlDatabaseBuilder&metric=bugs)](https://sonarcloud.io/dashboard?id=SqlDatabaseBuilder)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=SqlDatabaseBuilder&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=SqlDatabaseBuilder)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=SqlDatabaseBuilder&metric=code_smells)](https://sonarcloud.io/dashboard?id=SqlDatabaseBuilder)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=SqlDatabaseBuilder&metric=security_rating)](https://sonarcloud.io/dashboard?id=SqlDatabaseBuilder)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=SqlDatabaseBuilder&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=SqlDatabaseBuilder)  
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=SqlDatabaseBuilder&metric=ncloc)](https://sonarcloud.io/dashboard?id=SqlDatabaseBuilder)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=SqlDatabaseBuilder&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=SqlDatabaseBuilder)

# SqlDatabaseBuilder
  
SqlDatabaseBuilder is a simple library designed to help you build a Sql Server database 
resources using C#. This is useful for setting up and tearing down database objects for software testing.

# Database Basics
## SQL CREATE DATABASE
The DATABASE object is used to create a new SQL database.
```csharp
    Database database = new Database("MyDatabase");

    using (SqlConnection sqlConnection = new SqlConnection("Server=myServerAddress;Database=myDataBase;"))
    {
        sqlConnection.Open();
        database.Create(sqlConnection);
    }
```

## SQL CREATE TABLE
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

## SQL NULL/NOT NULL Constraint
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

## SQL PRIMARY KEY Constraint
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
## SQL FOREIGN KEY Constraint
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
## SQL UNIQUE Constraint
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
## SQL CHECK Constraint
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
## SQL DEFAULT Contraint
The DEFAULT constraint is used to provide a default value for a column. The default value will be added to all new records IF no other value is specified.
To add a DEFAULT contraint use:
```csharp
    Table table = new Table("Persons");
    Column id = new Column("Id", DataType.Int());
    Column lastName = new Column("LastName", DataType.VarChar(255)) { Default = new Default("Doe") }; 
    Column firstName = new Column("FirstName", DataType.VarChar(255)) { Default = new Default("John") }; 
    Column age = new Column("Age", DataType.Int()) { Default = new Default(18) };
    table.Columns.AddAll(id, lastName, firstName, age);
```
To add a DEFAULT constraint with a name use: 
```csharp
    Column lastName = new Column("LastName", DataType.VarChar(255)) { Default = new Default("defaultLastName", "Doe") }; 
    Column firstName = new Column("FirstName", DataType.VarChar(255)) { Default = new Default("defalutFirstName", "John") }; 
    Column age = new Column("Age", DataType.Int()) { Default = new Default("defaultAge", 18) };
```
The DEFAULT constraint can also be used to insert system values, by using functions like GETDATE(): 
```csharp
    Column date = new Column("Date", DataType.Date()) { Default = new Default("GETDATE()") }
```
## SQL Collation
Defines a collation of a database table column. Collation name can be either a Windows collation name or a SQL collation name. If not specified during table column creation, the column is assigned the default collation of the database.
```csharp
    Table table = new Table("Persons");
    Column id = new Column("Id", DataType.Int());
    Column lastName = new Column("LastName", DataType.VarChar(255)) { Collation = "Latin1_General_CS_AS_KS_WS" };
    Column firstName = new Column("FirstName", DataType.VarChar(255)) { Collation = "Traditional_Spanish_ci_ai" };
    Column age = new Column("Age", DataType.Int()) { Default = new Default(18) };
    table.Columns.AddAll(id, lastName, firstName, age);
```
## SQL CREATE INDEX
The CREATE INDEX statement is used to create indexes in tables.
```csharp
    TableIndex index = new TableIndex("IndexName", table, column1, column2);

    using (SqlConnection sqlConnection = new SqlConnection("Server=myServerAddress;Database=myDataBase;"))
    {
        sqlConnection.Open();
        index.Create(sqlConnection);
    }
```
To create a clustered and/or unique index
```csharp
    TableIndex index = new TableIndex("IndexName", table, column1, column2)
    {
        IndexType = IndexType.CLUSTERED,
        IsUnique = true
    };
```
To set column sort orders
```csharp
    TableIndex index = new TableIndex("IndexName", table, Tuple.Create(column1, ColumnSort.DESC));
```
# Always Encrypted
## CREATE COLUMN MASTER KEY
Creates a column master key metadata object in a database. A column master key metadata entry represents a key, stored in an external key store. The key protects (encrypts) column encryption keys when you're using Always Encrypted
```csharp
    ColumnMasterKey columnMasterKey = new ColumnMasterKey(
        keyName: "MyMasterKey", 
        keyStoreProviderName: KeyStoreProvider.WindowsCertificateStoreProvider, 
        keyPath: "CurrentUser/My/BBF037EC4A133ADCA89FFAEC16CA5BFA8878FB94"
    );

    using (SqlConnection sqlConnection = new SqlConnection("Server=myServerAddress;Database=myDataBase;"))
    {
        sqlConnection.Open();
        columnMasterKey.Create(sqlConnection);
    }
```
## CREATE COLUMN ENCRYPTION KEY
Creates a column encryption key metadata object for Always Encrypted. A column encryption key metadata object contains  the encrypted value of a column encryption key that is used to encrypt data in a column. Each value is encrypted using a column master key.
```csharp
    ColumnEncryptionKey columnEncryptionKey = new ColumnEncryptionKey(
        keyName: myColumnEncryptionKeyName, 
        columnMasterKeyName: myColumnMasterKeyName,
        encryptedValue: "0x0123456789ABCDEF"
    );

    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
    {
        sqlConnection.Open();
        columnEncryptionKey.Create(sqlConnection);
    }
```

## SQL Column Encryption
Specifies encrypting columns by using the Always Encrypted feature.
```csharp
    ColumnMasterKey columnMasterKey = new ColumnMasterKey("myColumnMasterKey", KeyStoreProvider.AzureKeyVaultProvider, "https://myvault.vault.azure.net:443/keys/MyCMK");
    ColumnEncryptionKey columnEncryptionKey = new ColumnEncryptionKey("myColumnEncryptionKey", columnMasterKey.Name, "0x0123456789ABCDEF");

    ColumnEncryption column1Encryption = new ColumnEncryption(columnEncryptionKey, ColumnEncryptionType.Deterministic);
    Column column1 = new Column("myColumn1", DataType.Char())
    {
        ColumnEncryption = column1Encryption,
        Collation = "Latin1_General_BIN2" // A BIN2 collation is required for deterministic encryption
    };

    ColumnEncryption column2Encryption = new ColumnEncryption(columnEncryptionKey, ColumnEncryptionType.Randomized);
    Column column2 = new Column("myColumn2", DataType.NVarChar())
    {
        ColumnEncryption = column2Encryption
    };

    Table table = new Table("myTable");
    table.Columns.AddAll(column1, column2);

    using (SqlConnection sqlConnection = new SqlConnection("Server=myServerAddress;Database=myDataBase;"))
    {
        sqlConnection.Open();
        columnMasterKey.Create(sqlConnection);
        columnEncryptionKey.Create(sqlConnection);
        table.Create(sqlConnection);
    }
```