using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class DateTime2Should
    {
        [Fact]
        public void ReturnCorrectDefinitionWithoutPrecision()
        {
            DataType dataType = DataType.DateTime2();
            Assert.Equal("datetime2", dataType.Definition);
        }

        [Theory]
        [ClassData(typeof(ValidDateTimePrecisionValues))]
        public void ReturnCorrectDefinitionWithPrecision(int precision)
        {
            DataType dataType = DataType.DateTime2(precision);
            string expectedDefinition = precision == 7 ? "datetime2" : $"datetime2({precision})";
            Assert.Equal(expectedDefinition, dataType.Definition);
        }

        [Fact]
        public void ReturnCorrectSizeWithoutPrecision()
        {
            DataType dataType = DataType.DateTime2();
            Assert.Equal(8, dataType.Size);
        }

        [Theory]
        [ClassData(typeof(ValidDateTimePrecisionValues))]
        public void ReturnCorrectSizeWithPrecision(int precision)
        {
            DataType dataType = DataType.DateTime2(precision);
            int expectedSize;
            if (precision < 3) expectedSize = 6;
            else if (precision < 5) expectedSize = 7;
            else expectedSize = 8;
            Assert.Equal(expectedSize, dataType.Size);
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
        [InlineData(-1)]
        [InlineData(8)]
        [InlineData(239)]
        [InlineData(2389)]
        [InlineData(12389)]
        [InlineData(123789)]
        [InlineData(1234789)]
        [InlineData(12346789)]
        [InlineData(123456789)]
        [InlineData(int.MaxValue)]
        public void ThrowInvalidPrecisionException(int precision)
        {
            Assert.Throws<InvalidPrecisionException>(() => DataType.DateTime2(precision));
        }
    }

    public class ValidDateTimePrecisionValues : IEnumerable<object[]>
    {
        private List<object[]> data = new List<object[]>();

        public ValidDateTimePrecisionValues()
        {
            for (int precision = 0; precision <= 7; precision++)
            {
                data.Add(new object[] { precision });
            }
        }

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
