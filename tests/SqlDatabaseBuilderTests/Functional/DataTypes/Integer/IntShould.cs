using System;
using System.Collections.Generic;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class IntShould
    {
        [Fact]
        public void ReturnCorrectDefinition()
        {
            DataType dataType = DataType.Int();
            Assert.Equal("int", dataType.Definition);
        }

        [Fact]
        public void ReturnCorrectSize()
        {
            DataType dataType = DataType.Int();
            Assert.Equal(4, dataType.Size);
        }
    }
}
