﻿using Xtrimmer.SqlDatabaseBuilder;
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
        [ClassData(typeof(ValidScaledTemporalScaleValues))]
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
        [ClassData(typeof(ValidScaledTemporalScaleValues))]
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
        [InvalidScaledTemporalScaleValues]
        public void ThrowInvalidScaleException(int precision)
        {
            Assert.Throws<InvalidScaleException>(() => DataType.DateTime2(precision));
        }
    }
}
