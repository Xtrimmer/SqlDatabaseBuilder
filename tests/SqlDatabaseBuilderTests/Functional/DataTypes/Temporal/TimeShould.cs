using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class TimeShould
    {
        [Fact]
        public void ReturnCorrectDefinitionWithoutPrecision()
        {
            DataType dataType = DataType.Time();
            Assert.Equal("time", dataType.Definition);
        }

        [Theory]
        [ClassData(typeof(ValidTimeScaleValues))]
        public void ReturnCorrectDefinitionWithPrecision(int precision)
        {
            DataType dataType = DataType.Time(precision);
            string expectedDefinition = precision == 7 ? "time" : $"time({precision})";
            Assert.Equal(expectedDefinition, dataType.Definition);
        }

        [Fact]
        public void ReturnCorrectSize()
        {
            DataType dataType = DataType.Time();
            Assert.Equal(5, dataType.Size);
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
        public void ThrowInvalidScaleException(int scale)
        {
            Assert.Throws<InvalidScaleException>(() => DataType.Time(scale));
        }
    }

    public class ValidTimeScaleValues : IEnumerable<object[]>
    {
        private List<object[]> data = new List<object[]>();

        public ValidTimeScaleValues()
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
