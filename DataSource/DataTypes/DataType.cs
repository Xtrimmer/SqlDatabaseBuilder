using System;
using System.Collections.Generic;
using System.Text;

namespace Xtrimmer.SqlDatabaseBuilder
{
    public abstract class DataType
    {
        public const int MAX = -1;

        public static DataType BigInt()
        {
            return new BigInt();
        }

        public static DataType Int()
        {
            return new Int();
        }

        public static DataType SmallInt()
        {
            return new SmallInt();
        }

        public static DataType TinyInt()
        {
            return new TinyInt();
        }

        public static DataType Bit()
        {
            return new Bit();
        }

        public static DataType Money()
        {
            return new Money();
        }

        public static DataType SmallMoney()
        {
            return new SmallMoney();
        }

        public static DataType Decimal()
        {
            return new Decimal();
        }

        public static DataType Decimal(int precision)
        {
            return new Decimal(precision);
        }

        public static DataType Decimal(int precision, int scale)
        {
            return new Decimal(precision, scale);
        }

        public static DataType Numeric()
        {
            return new Numeric();
        }

        public static DataType Numeric(int precision)
        {
            return new Numeric(precision);
        }

        public static DataType Numeric(int precision, int scale)
        {
            return new Numeric(precision, scale);
        }

        public static DataType Float()
        {
            return new Float();
        }

        public static DataType Float(int mantissa)
        {
            return new Float(mantissa);
        }

        public static DataType Real()
        {
            return new Real();
        }

        public static DataType VarChar()
        {
            return new VarChar();
        }

        public static DataType VarChar(int n)
        {
            return new VarChar(n);
        }

        public static DataType Char()
        {
            return new Char();
        }

        public static DataType Char(int n)
        {
            return new Char(n);
        }
    

        public static DataType VarBinary()
        {
            return new VarBinary();
        }

        public static DataType VarBinary(int n)
        {
            return new VarBinary(n);
        }

        public static DataType Binary()
        {
            return new Binary();
        }

        public static DataType Binary(int n)
        {
            return new Binary(n);
        }

        public static DataType NVarChar()
        {
            return new NVarChar();
        }

        public static DataType NVarChar(int n)
        {
            return new NVarChar(n);
        }

        public static DataType NChar()
        {
            return new NChar();
        }

        public static DataType NChar(int n)
        {
            return new NChar(n);
        }

        public static DataType Date()
        {
            return new Date();
        }

        public static DataType DateTime()
        {
            return new DateTime();
        }

        public static DataType DateTime2()
        {
            return new DateTime2();
        }

        public static DataType DateTime2(int precision)
        {
            return new DateTime2(precision);
        }

        public static DataType Time()
        {
            return new Time();
        }

        public static DataType Time(int scale)
        {
            return new Time(scale);
        }

        public static DataType UniqueIdentifier()
        {
            return new UniqueIdentifier();
        }

        public abstract string Definition { get; }
        public abstract int Size { get; }
    }
}
