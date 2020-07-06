using System;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class TableShould
    {
        [Fact]
        public void ThrowExceptionWhenAddingMoreThanOnePrimaryKeyConstraint()
        {
            Table table = new Table("test");
            table.Constraints.Add(new PrimaryKeyConstraint());
            Assert.Throws<MultiplePrimaryKeyException>(() => table.Constraints.Add(new PrimaryKeyConstraint()));
        }

        [Fact]
        public void ThrowExceptionWhenCreatingWithEmptyName()
        {
            Assert.Throws<InvalidDatabaseIdentifierException>(() => new Table(""));
        }

        [Fact]
        public void ThrowExceptionWhenCreatingWithNullName()
        {
            Assert.Throws<InvalidDatabaseIdentifierException>(() => new Table(null));
        }

        [Fact]
        public void ThrowExceptionWhenCreatingWithNoColumns()
        {            
            Table table = new Table("test");
            Assert.Throws<InvalidTableDefinitionException>(() => table.Create(null));
        }

        [Fact]
        public void ThrowExceptionWhenCreateWithNullSqlConnection()
        {
            Table table = new Table("test");
            table.Columns.Add(new Column("test", DataType.BigInt()));
            Assert.Throws<ArgumentNullException>(() => table.Create(null));
        }

        [Fact]
        public void ThrowExceptionWhenDroppingWithNullSqlConnection()
        {
            Table table = new Table("test");
            table.Columns.Add(new Column("test", DataType.BigInt()));
            Assert.Throws<ArgumentNullException>(() => table.Drop(null));
        }
    }
}
