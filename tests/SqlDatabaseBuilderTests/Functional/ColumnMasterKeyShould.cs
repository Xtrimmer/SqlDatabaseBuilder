using System;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class ColumnMasterKeyShould
    {
        [Fact]
        public void ThrowExceptionWhenWhenEnclaveIsEnabledButNoSignature()
        {
            ColumnMasterKey cmk = new ColumnMasterKey("testCmkName", "testProvider", "testKeyPath")
            {
                IsEnclaveEnabled = true
            };

            Assert.Throws<InvalidColumnMasterKeyDefinitionException>(() => cmk.Create(null));
        }

        [Fact]
        public void ThrowExceptionWhenCreateWithNullSqlConnection()
        {
            ColumnMasterKey cmk = new ColumnMasterKey("testCmkName", "testProvider", "testKeyPath");

            Assert.Throws<ArgumentNullException>(() => cmk.Create(null));
        }

        [Fact]
        public void ThrowExceptionWhenDropWithNullSqlConnection()
        {
            ColumnMasterKey cmk = new ColumnMasterKey("testCmkName", "testProvider", "testKeyPath");

            Assert.Throws<ArgumentNullException>(() => cmk.Drop(null));
        }
    }
}
