using System;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class DatabaseShould
    {
        [Fact]
        public void ThrowExceptionWhenCreatingWithEmptyName()
        {
            Assert.Throws<InvalidDatabaseIdentifierException>(() => new Database(""));
        }

        [Fact]
        public void ThrowExceptionWhenCreatingWithNullName()
        {
            Assert.Throws<InvalidDatabaseIdentifierException>(() => new Database(null));
        }

        [Fact]
        public void ThrowExceptionWhenCreateWithNullSqlConnection()
        {
            Database database = new Database("test");
            Assert.Throws<ArgumentNullException>(() => database.Create(null));
        }

        [Fact]
        public void ThrowExceptionWhenDroppingWithNullSqlConnection()
        {
            Database database = new Database("test");
            Assert.Throws<ArgumentNullException>(() => database.Drop(null));
        }
    }
}
