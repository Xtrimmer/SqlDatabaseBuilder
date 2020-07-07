using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class DateTimeOffsetShould
    {
        [Fact]
        public void ReturnCorrectDefinitionWithoutPrecision()
        {
            DataType dataType = DataType.DateTimeOffset();
            Assert.Equal("datetimeoffset", dataType.Definition);
        }

        [Theory]
        [ClassData(typeof(ValidScaledTemporalScaleValues))]
        public void ReturnCorrectDefinitionWithPrecision(int precision)
        {
            DataType dataType = DataType.DateTimeOffset(precision);
            string expectedDefinition = precision == 7 ? "datetimeoffset" : $"datetimeoffset({precision})";
            Assert.Equal(expectedDefinition, dataType.Definition);
        }

        [Fact]
        public void ReturnCorrectSizeWithoutPrecision()
        {
            DataType dataType = DataType.DateTimeOffset();
            Assert.Equal(10, dataType.Size);
        }

        [Theory]
        [ClassData(typeof(ValidScaledTemporalScaleValues))]
        public void ReturnCorrectSizeWithPrecision(int precision)
        {
            DataType dataType = DataType.DateTimeOffset(precision);
            int expectedSize;
            if (precision < 3) expectedSize = 8;
            else if (precision < 5) expectedSize = 9;
            else expectedSize = 10;
            Assert.Equal(expectedSize, dataType.Size);
        }

        [Theory]
        [InvalidScaledTemporalScaleValues]
        public void ThrowInvalidScaleException(int precision)
        {
            Assert.Throws<InvalidScaleException>(() => DataType.DateTimeOffset(precision));
        }
    }
}
