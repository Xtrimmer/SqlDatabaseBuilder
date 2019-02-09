using System;
using System.Collections.Generic;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class TinyIntShould
    {
        [Fact]
        public void ReturnCorrectDefinition()
        {
            DataType dataType = DataType.TinyInt();
            Assert.Equal("tinyint", dataType.Definition);
        }

        [Fact]
        public void ReturnCorrectSize()
        {
            DataType dataType = DataType.TinyInt();
            Assert.Equal(1, dataType.Size);
        }
    }
}
