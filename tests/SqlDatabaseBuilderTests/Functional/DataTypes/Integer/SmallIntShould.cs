using System;
using System.Collections.Generic;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class SmallIntShould
    {
        [Fact]
        public void ReturnCorrectDefinition()
        {
            DataType dataType = DataType.SmallInt();
            Assert.Equal("smallint", dataType.Definition);
        }

        [Fact]
        public void ReturnCorrectSize()
        {
            DataType dataType = DataType.SmallInt();
            Assert.Equal(2, dataType.Size);
        }
    }
}
