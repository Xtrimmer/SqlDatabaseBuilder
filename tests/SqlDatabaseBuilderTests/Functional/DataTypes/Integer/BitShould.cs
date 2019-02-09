using System;
using System.Collections.Generic;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{ 
    public class BitShould
    {
        [Fact]
        public void ReturnCorrectDefinition()
        {
            DataType dataType = DataType.Bit();
            Assert.Equal("bit", dataType.Definition);
        }

        [Fact]
        public void ReturnCorrectSize()
        {
            DataType dataType = DataType.Bit();
            Assert.Equal(1, dataType.Size);
        }
    }
}
