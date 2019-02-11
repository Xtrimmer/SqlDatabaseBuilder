using Xtrimmer.SqlDatabaseBuilder;

using System;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class DatabaseIdentifierShould
    {
        const string INVALID_ID_MESSAGE = "Identifiers must contain from 1 through 128 characters.";
        const string NULL_PARAMETER_NAME = "name [System.String]";

        [Fact]
        public void ThrowInvalidDatabaseIdentifierExceptionIfNameIsTooShort()
        {
            Database database;
            Exception ex = Assert.Throws<InvalidDatabaseIdentifierException>(() => database = new Database(""));
            Assert.Equal(INVALID_ID_MESSAGE, ex.Message);
        }

        [Fact]
        public void ThrowInvalidDatabaseIdentifierExceptionIfNameIsTooLong()
        {
            string nameLength129 = "-ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            Database database;
            Exception ex = Assert.Throws<InvalidDatabaseIdentifierException>(() => database = new Database(nameLength129));
            Assert.Equal(INVALID_ID_MESSAGE, ex.Message);
        }

        [Fact]
        public void ThrowInvalidDatabaseIdentifierExceptionIfNameIsNull()
        {
            Database database;
            Assert.Throws<InvalidDatabaseIdentifierException>(() => database = new Database(null));
        }

        [Theory]
        [InlineData("A")]
        [InlineData("AB")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/")]
        public void AcceptValidLengthNames(string name)
        {            
            Database database = new Database(name);
            Assert.Equal(name, database.Name);
        }
    }
}
