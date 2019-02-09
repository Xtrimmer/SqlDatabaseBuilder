using Xtrimmer.SqlDatabaseBuilder;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class DataTypeShould
    {
        [Fact]
        public void returnBigIntType()
        {
            DataType dataType = DataType.BigInt();

            Assert.Equal(8, dataType.Size);
            Assert.Equal("bigint", dataType.Definition);
        }

        [Fact]
        public void returnIntType()
        {
            DataType dataType = DataType.Int();

            Assert.Equal(4, dataType.Size);
            Assert.Equal("int", dataType.Definition);
        }

        [Fact]
        public void returnSmallIntType()
        {
            DataType dataType = DataType.SmallInt();

            Assert.Equal(2, dataType.Size);
            Assert.Equal("smallint", dataType.Definition);
        }

        [Fact]
        public void returnTinyIntType()
        {
            DataType dataType = DataType.TinyInt();

            Assert.Equal(1, dataType.Size);
            Assert.Equal("tinyint", dataType.Definition);
        }

        [Fact]
        public void returnBitType()
        {
            DataType dataType = DataType.Bit();

            Assert.Equal(1, dataType.Size);
            Assert.Equal("bit", dataType.Definition);
        }

        [Fact]
        public void returnMoneyType()
        {
            DataType dataType = DataType.Money();

            Assert.Equal(8, dataType.Size);
            Assert.Equal("money", dataType.Definition);
        }

        [Fact]
        public void returnSmallMoneyType()
        {
            DataType dataType = DataType.SmallMoney();

            Assert.Equal(4, dataType.Size);
            Assert.Equal("smallmoney", dataType.Definition);
        }

        [Fact]
        public void returnDecimalType()
        {
            DataType dataType = DataType.Decimal();

            Assert.Equal(9, dataType.Size);
            Assert.Equal("decimal", dataType.Definition);

            dataType = DataType.Decimal(19);

            Assert.Equal(9, dataType.Size);
            Assert.Equal("decimal(19)", dataType.Definition);

            dataType = DataType.Decimal(28, 20);

            Assert.Equal(13, dataType.Size);
            Assert.Equal("decimal(28, 20)", dataType.Definition);
        }

        [Fact]
        public void returnNumericType()
        {
            DataType dataType = DataType.Numeric();

            Assert.Equal(9, dataType.Size);
            Assert.Equal("numeric", dataType.Definition);

            dataType = DataType.Numeric(38);

            Assert.Equal(17, dataType.Size);
            Assert.Equal("numeric(38)", dataType.Definition);

            dataType = DataType.Numeric(29, 10);

            Assert.Equal(17, dataType.Size);
            Assert.Equal("numeric(29, 10)", dataType.Definition);
        }

        [Fact]
        public void returnFloatType()
        {
            DataType dataType = DataType.Float();

            Assert.Equal(8, dataType.Size);
            Assert.Equal("float", dataType.Definition);

            dataType = DataType.Float(24);

            Assert.Equal(4, dataType.Size);
            Assert.Equal("float(24)", dataType.Definition);
        }

        [Fact]
        public void returnRealType()
        {
            DataType dataType = DataType.Real();

            Assert.Equal(4, dataType.Size);
            Assert.Equal("real", dataType.Definition);
        }
    }
}
