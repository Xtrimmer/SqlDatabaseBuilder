using System;
using System.Collections.Generic;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class CharShould
    {
        [Fact]
        public void ReturnCorrectDefinitionWithoutN()
        {            
            DataType dataType = DataType.Char();
            Assert.Equal("char", dataType.Definition);
        }

        public static object[][] validNValues = new object[][]
        {
            new object[] { 1 },
            new object[] { 13 },
            new object[] { 144 },
            new object[] { 1597 },
            new object[] { 4000 },
            new object[] { 8000 },
        };

        [Theory]
        [MemberData(nameof(validNValues))]
        public void ReturnCorrectDefinitionWithN(int n)
        {
            DataType dataType = DataType.Char(n);
            string expectedDefinition = n == 1 ? "char" : $"char({n})";
            Assert.Equal(expectedDefinition, dataType.Definition);
        }

        [Fact]
        public void ReturnCorrectSizeWithoutN()
        {
            DataType dataType = DataType.Char();
            Assert.Equal(1, dataType.Size);
        }

        [Theory]
        [MemberData(nameof(validNValues))]
        public void ReturnCorrectSizeWithN(int n)
        {
            DataType dataType = DataType.Char(n);
            Assert.Equal(n, dataType.Size);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(1597000)]
        [InlineData(159700)]
        [InlineData(15970)]
        [InlineData(8001)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-130)]
        [InlineData(-1440)]
        [InlineData(-1597)]
        [InlineData(int.MinValue)]
        public void ThrowInvalidCharacterSetLength(int n)
        {
            Assert.Throws<InvalidCharacterSetLengthException>(() => DataType.Char(n));            
        }
    }
}
