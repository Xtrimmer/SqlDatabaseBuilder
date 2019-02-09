﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class DecimalShould
    {
        [Fact]
        public void ReturnCorrectDefinitionWithoutPrecisionAndScale()
        {
            DataType dataType = DataType.Decimal();
            Assert.Equal("decimal", dataType.Definition);
        }

        [Theory]
        [ClassData(typeof(ValidPrecisionValues))]
        public void ReturnCorrectDefinitionWithPrecisionOnly(int precision)
        {
            DataType dataType = DataType.Decimal(precision);
            string expectedDefinition = precision == 18 ? "decimal" : $"decimal({precision})";
            Assert.Equal(expectedDefinition, dataType.Definition);
        }

        [Theory]
        [ClassData(typeof(ValidPrecisionAndScaleValues))]
        public void ReturnCorrectDefinitionWithPrecisionAndScale(int precision, int scale)
        {
            DataType dataType = DataType.Decimal(precision, scale);

            string expectedDefinition;
            if (precision == 18 && scale == 0) expectedDefinition = "decimal";
            else if (precision != 18 && scale == 0) expectedDefinition = $"decimal({precision})";
            else expectedDefinition = $"decimal({precision}, {scale})";

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
        [InlineData(39)]
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
            Assert.Throws<InvalidPrecisionException>(() => DataType.Decimal(precision));
        }

        [Theory]
        [InlineData(1, int.MinValue)]
        [InlineData(2, -123456789)]
        [InlineData(3, -23456789)]
        [InlineData(4, -2345678)]
        [InlineData(5, -345678)]
        [InlineData(6, -34567)]
        [InlineData(7, -4567)]
        [InlineData(8, -456)]
        [InlineData(9, -56)]
        [InlineData(10, -5)]
        [InlineData(11, -1)]
        [InlineData(12, 13)]
        [InlineData(1, 2)]
        [InlineData(13, 14)]
        [InlineData(14, 2389)]
        [InlineData(22, 12389)]
        [InlineData(34, 123789)]
        [InlineData(35, 1234789)]
        [InlineData(36, 12346789)]
        [InlineData(37, 123456789)]
        [InlineData(38, int.MaxValue)]
        public void ThrowInvalidScaleException(int precision, int scale)
        {
            Assert.Throws<InvalidScaleException>(() => DataType.Decimal(precision, scale));
        }

        [Fact]
        public void ReturnTheCorrectSizeWithoutPrecisionSpecified()
        {
            DataType dataType = DataType.Decimal();
            Assert.Equal(9, dataType.Size);
        }

        [Theory]
        [ClassData(typeof(ValidPrecisionValues))]
        public void ReturnTheCorrectSizeWithPrecisionSpecified(int precision)
        {
            DataType dataType = DataType.Decimal(precision);
            int expectedSize;
            if (precision < 10) expectedSize = 5;
            else if (precision < 20) expectedSize = 9;
            else if (precision < 29) expectedSize = 13;
            else expectedSize = 17;
            Assert.Equal(expectedSize, dataType.Size);
        }
    }

    public class ValidPrecisionAndScaleValues : IEnumerable<object[]>
    {
        private List<object[]> data = new List<object[]>();

        public ValidPrecisionAndScaleValues() {
            for (int precision = 1; precision <= 38; precision++)
            {
                for (int scale = 0; scale <= precision; scale++)
                {
                    data.Add(new object[] { precision, scale });
                }
            }
        }

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ValidPrecisionValues : IEnumerable<object[]>
    {
        private List<object[]> data = new List<object[]>();

        public ValidPrecisionValues()
        {
            for (int precision = 1; precision <= 38; precision++)
            {
                data.Add(new object[] { precision });
            }
        }

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
