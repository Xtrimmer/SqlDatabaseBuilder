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
        public void returnMaxValue()
        {
            Assert.Equal(-1, DataType.MAX);
        }

        [Fact]
        public void returnBigIntType()
        {
            DataType dataType = DataType.BigInt();
            Assert.Equal("bigint", dataType.Definition);
        }

        [Fact]
        public void returnIntType()
        {
            DataType dataType = DataType.Int();
            Assert.Equal("int", dataType.Definition);
        }

        [Fact]
        public void returnSmallIntType()
        {
            DataType dataType = DataType.SmallInt();
            Assert.Equal("smallint", dataType.Definition);
        }

        [Fact]
        public void returnTinyIntType()
        {
            DataType dataType = DataType.TinyInt();
            Assert.Equal("tinyint", dataType.Definition);
        }

        [Fact]
        public void returnBitType()
        {
            DataType dataType = DataType.Bit();
            Assert.Equal("bit", dataType.Definition);
        }

        [Fact]
        public void returnMoneyType()
        {
            DataType dataType = DataType.Money();
            Assert.Equal("money", dataType.Definition);
        }

        [Fact]
        public void returnSmallMoneyType()
        {
            DataType dataType = DataType.SmallMoney();
            Assert.Equal("smallmoney", dataType.Definition);
        }

        [Fact]
        public void returnDecimalType()
        {
            DataType dataType = DataType.Decimal();
            Assert.Equal("decimal", dataType.Definition);
        }

        [Fact]
        public void returnNumericType()
        {
            DataType dataType = DataType.Numeric();
            Assert.Equal("numeric", dataType.Definition);
        }

        [Fact]
        public void returnFloatType()
        {
            DataType dataType = DataType.Float();
            Assert.Equal("float", dataType.Definition);
        }

        [Fact]
        public void returnRealType()
        {
            DataType dataType = DataType.Real();
            Assert.Equal("real", dataType.Definition);
        }

        [Fact]
        public void returnCharType()
        {
            DataType dataType = DataType.Char();
            Assert.Equal("char", dataType.Definition);
        }

        [Fact]
        public void returnVarCharType()
        {
            DataType dataType = DataType.VarChar();
            Assert.Equal("varchar", dataType.Definition);
        }

        [Fact]
        public void returnBinaryType()
        {
            DataType dataType = DataType.Binary();
            Assert.Equal("binary", dataType.Definition);
        }

        [Fact]
        public void returnVarBinaryType()
        {
            DataType dataType = DataType.VarBinary();
            Assert.Equal("varbinary", dataType.Definition);
        }

        [Fact]
        public void returnNCharType()
        {
            DataType dataType = DataType.NChar();
            Assert.Equal("nchar", dataType.Definition);
        }

        [Fact]
        public void returnNVarCharType()
        {
            DataType dataType = DataType.NVarChar();
            Assert.Equal("nvarchar", dataType.Definition);
        }

        [Fact]
        public void returnDateType()
        {
            DataType dataType = DataType.Date();
            Assert.Equal("date", dataType.Definition);
        }

        [Fact]
        public void returnDateTimeType()
        {
            DataType dataType = DataType.DateTime();
            Assert.Equal("datetime", dataType.Definition);
        }

        [Fact]
        public void returnDateTime2Type()
        {
            DataType dataType = DataType.DateTime2();
            Assert.Equal("datetime2", dataType.Definition);
        }

        [Fact]
        public void returnTimeType()
        {
            DataType dataType = DataType.Time();
            Assert.Equal("time", dataType.Definition);
        }
    }
}
