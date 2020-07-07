using Xtrimmer.SqlDatabaseBuilder;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class SmallDateTimeShould
    {
        [Fact]
        public void ReturnCorrectDefinition()
        {
            DataType dataType = DataType.SmallDateTime();
            Assert.Equal("smalldatetime", dataType.Definition);
        }

        [Fact]
        public void ReturnCorrectSize()
        {
            DataType dataType = DataType.SmallDateTime();
            Assert.Equal(4, dataType.Size);
        }
    }
}
