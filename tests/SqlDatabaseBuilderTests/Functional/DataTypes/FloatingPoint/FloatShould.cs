using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class FloatShould
    {
        [Fact]
        public void ReturnCorrectDefinitionWithoutMantissa()
        {
            DataType dataType = DataType.Float();
            Assert.Equal("float", dataType.Definition);
        }

        [Theory]
        [ClassData(typeof(ValidMantissaValues))]
        public void ReturnCorrectDefinitionWithMantissa(int mantissa)
        {
            DataType dataType = DataType.Float(mantissa);
            string expectedDefinition = mantissa == 53 ? "float" : $"float({mantissa})";
            Assert.Equal(expectedDefinition, dataType.Definition);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-123456789)]
        [InlineData(-23456789)]
        [InlineData(-2345678)]
        [InlineData(-345678)]
        [InlineData(-34567)]
        [InlineData(-4567)]
        [InlineData(-456)]
        [InlineData(-56)]
        [InlineData(-5)]
        [InlineData(0)]
        [InlineData(54)]
        [InlineData(239)]
        [InlineData(2389)]
        [InlineData(12389)]
        [InlineData(123789)]
        [InlineData(1234789)]
        [InlineData(12346789)]
        [InlineData(123456789)]
        [InlineData(int.MaxValue)]
        public void ThrowInvalidMantissaException(int mantissa)
        {
            Assert.Throws<InvalidMantissaException>(() => DataType.Float(mantissa));
        }

        [Fact]
        public void ReturnTheCorrectSizeWithoutMantissaSpecified()
        {
            DataType dataType = DataType.Float();
            Assert.Equal(8, dataType.Size);
        }

        [Theory]
        [ClassData(typeof(ValidMantissaValues))]
        public void ReturnTheCorrectSizeWithMantissaSpecified(int mantissa)
        {
            DataType dataType = DataType.Float(mantissa);
            int expectedSize = mantissa < 25 ? 4 : 8;
            Assert.Equal(expectedSize, dataType.Size);
        }
    }

    public class ValidMantissaValues : IEnumerable<object[]>
    {
        private List<object[]> data = new List<object[]>();

        public ValidMantissaValues()
        {
            for (int mantissa = 1; mantissa <= 53; mantissa++)
            {
                data.Add(new object[] { mantissa });
            }
        }

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
