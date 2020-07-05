using System;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class IndexShould
    {
        readonly Column column = new Column("testColumn", DataType.Int());

        [Fact]
        public void ThrowExceptionIfTableNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Index("Name", null, column));
        }

        [Fact]
        public void ThrowExceptionIfNameNull()
        {
            Table table = new Table("TestTable");
            Assert.Throws<ArgumentNullException>(() => new Index(null, table, column));
        }

        [Fact]
        public void ThrowExceptionIfNameInvalid()
        {
            Table table = new Table("TestTable");
            Assert.Throws<InvalidDatabaseIdentifierException>(() => new Index("", table, column));
        }

        [Fact]
        public void ThrowExceptionIfNoColumns()
        {
            Table table = new Table("TestTable");
            Index index = new Index("testIndex", table, new Column[] { });
            Assert.Throws<InvalidIndexDefinitionException>(() => index.Create(null));
        }

        [Fact]
        public void ThrowExceptionWhenCreateWithNullSqlConnection()
        {
            Table table = new Table("TestTable");
            Index index = new Index("testIndex", table, column);
            Assert.Throws<ArgumentNullException>(() => index.Create(null));
        }

        [Fact]
        public void ThrowExceptionWhenDroppingWithNullSqlConnection()
        {
            Table table = new Table("test");
            Index index = new Index("testIndex", table, column);
            Assert.Throws<ArgumentNullException>(() => index.Drop(null));
        }
    }
}
