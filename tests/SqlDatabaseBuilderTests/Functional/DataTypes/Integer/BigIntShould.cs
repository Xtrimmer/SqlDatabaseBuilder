using System;
using System.Collections.Generic;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class BigIntShould
    {
        [Fact]
        public void ReturnCorrectDefinition()
        {
            DataType dataType = DataType.BigInt();
            Assert.Equal("bigint", dataType.Definition);
        }

        [Fact]
        public void ReturnCorrectSize()
        {
            DataType dataType = DataType.BigInt();
            Assert.Equal(8, dataType.Size);
        }
    }
}
