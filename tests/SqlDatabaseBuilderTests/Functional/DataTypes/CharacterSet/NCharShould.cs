using System;
using System.Collections.Generic;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class NCharShould
    {
        [Fact]
        public void ReturnCorrectDefinitionWithoutN()
        {            
            DataType dataType = DataType.NChar();
            Assert.Equal("nchar", dataType.Definition);
        }

        public static object[][] validNValues = new object[][]
        {
            new object[] { 1 },
            new object[] { 13 },
            new object[] { 144 },
            new object[] { 1597 },
            new object[] { 4000 }
        };

        [Theory]
        [MemberData(nameof(validNValues))]
        public void ReturnCorrectDefinitionWithN(int n)
        {
            DataType dataType = DataType.NChar(n);
            string expectedDefinition = n == 1 ? "nchar" : $"nchar({n})";
            Assert.Equal(expectedDefinition, dataType.Definition);
        }

        [Fact]
        public void ReturnCorrectSizeWithoutN()
        {
            DataType dataType = DataType.NChar();
            Assert.Equal(2, dataType.Size);
        }

        [Theory]
        [MemberData(nameof(validNValues))]
        public void ReturnCorrectSizeWithN(int n)
        {
            DataType dataType = DataType.NChar(n);
            Assert.Equal(2 * n, dataType.Size);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(1597000)]
        [InlineData(159700)]
        [InlineData(15970)]
        [InlineData(8001)]
        [InlineData(4001)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-130)]
        [InlineData(-1440)]
        [InlineData(-1597)]
        [InlineData(int.MinValue)]
        public void ThrowInvalidCharacterSetLength(int n)
        {
            Assert.Throws<InvalidCharacterSetLength>(() => DataType.NChar(n));            
        }
    }
}
