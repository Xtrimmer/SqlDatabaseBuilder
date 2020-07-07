namespace Xtrimmer.SqlDatabaseBuilder
{
    public abstract class DataType
    {
        public const int MAX = -1;

        public static DataType BigInt() => new BigInt();
        public static DataType Int() => new Int();
        public static DataType SmallInt() => new SmallInt();
        public static DataType TinyInt() => new TinyInt();
        public static DataType Bit() => new Bit();
        public static DataType Money() => new Money();
        public static DataType SmallMoney() => new SmallMoney();
        public static DataType Decimal() => new Decimal();
        public static DataType Decimal(int precision) => new Decimal(precision);
        public static DataType Decimal(int precision, int scale) => new Decimal(precision, scale);
        public static DataType Numeric() => new Numeric();
        public static DataType Numeric(int precision) => new Numeric(precision);
        public static DataType Numeric(int precision, int scale) => new Numeric(precision, scale);
        public static DataType Float() => new Float();
        public static DataType Float(int mantissa) => new Float(mantissa);
        public static DataType Real() => new Real();
        public static DataType VarChar() => new VarChar();
        public static DataType VarChar(int n) => new VarChar(n);
        public static DataType Char() => new Char();
        public static DataType Char(int n) => new Char(n);
        public static DataType VarBinary() => new VarBinary();
        public static DataType VarBinary(int n) => new VarBinary(n);
        public static DataType Binary() => new Binary();
        public static DataType Binary(int n) => new Binary(n);
        public static DataType NVarChar() => new NVarChar();
        public static DataType NVarChar(int n) => new NVarChar(n);
        public static DataType NChar() => new NChar();
        public static DataType NChar(int n) => new NChar(n);
        public static DataType Date() => new Date();
        public static DataType DateTime() => new DateTime();
        public static DataType SmallDateTime() => new SmallDateTime();
        public static DataType DateTime2() => new DateTime2();
        public static DataType DateTime2(int scale) => new DateTime2(scale);
        public static DataType DateTimeOffset() => new DateTimeOffset();
        public static DataType DateTimeOffset(int scale) => new DateTimeOffset(scale);
        public static DataType Time() => new Time();
        public static DataType Time(int scale) => new Time(scale);
        public static DataType UniqueIdentifier() => new UniqueIdentifier();

        public abstract string Definition { get; }
        public abstract int Size { get; }
        public override bool Equals(object obj) => GetType().Equals(obj.GetType());
        public override int GetHashCode() => GetType().GetHashCode();
    }
}
