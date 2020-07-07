using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Xtrimmer.SqlDatabaseBuilderTests.Functional
{
    public class ValidScaledTemporalScaleValues : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new List<object[]>();

        public ValidScaledTemporalScaleValues()
        {
            for (int precision = 0; precision <= 7; precision++)
            {
                data.Add(new object[] { precision });
            }
        }

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class InvalidScaledTemporalScaleValuesAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { int.MinValue };
            yield return new object[] { -123456789 };
            yield return new object[] { -23456789 };
            yield return new object[] { -2345678 };
            yield return new object[] { -345678 };
            yield return new object[] { -34567 };
            yield return new object[] { -4567 };
            yield return new object[] { -456 };
            yield return new object[] { -56 };
            yield return new object[] { -5 };
            yield return new object[] { -1 };
            yield return new object[] { 8 };
            yield return new object[] { 239 };
            yield return new object[] { 2389 };
            yield return new object[] { 12389 };
            yield return new object[] { 123789 };
            yield return new object[] { 1234789 };
            yield return new object[] { 12346789 };
            yield return new object[] { 123456789 };
            yield return new object[] { int.MaxValue };
        }
    }
}
