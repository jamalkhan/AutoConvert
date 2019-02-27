using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace bleak.AutoConvert.Tests
{
    public enum MyEnum
    {
        Value1,
        Value2,
        Value3,
        Value4
    }

    [TestClass]
    public class AutoConvertExtensionMethodsTests
    {
        [TestMethod]
        public void TestDictionaryConvert()
        {
            Guid val = Guid.NewGuid();
            var source = new Dictionary<string, object>()
            {
                { "StrVal","Banana" },
                { "LongVal",12345 },
                { "IntVal", 1234 },
                { "NullableLongValSet", 12345 },
                { "NullableIntValSet", 1234 },
                { "MyEnum", "Value2" },
                { "NullableEnumSet", "Value2" },
                { "GuidVal", val.ToString() },
                { "NullableGuidValSet", val.ToString() },

            };

            var destination = source.Convert<Destination>();

            Assert.AreEqual("Banana", destination.StrVal);
            Assert.AreEqual((long)12345, destination.LongVal);
            Assert.AreEqual((int)1234, destination.IntVal);
            Assert.AreEqual(null, destination.NullableLongValNull);
            Assert.AreEqual(null, destination.NullableIntValNull);
            Assert.AreEqual((long)12345, destination.NullableLongValSet);
            Assert.AreEqual((int)1234, destination.NullableIntValSet);
            Assert.AreEqual(MyEnum.Value2, destination.MyEnum);
            Assert.AreEqual(null, destination.NullableEnumNull);
            Assert.AreEqual(MyEnum.Value2, destination.NullableEnumSet);
            Assert.AreEqual(val, destination.GuidVal);
            Assert.AreEqual(val, destination.NullableGuidValSet);
        }


        [TestMethod]
        public void TestObjectConvert()
        {
            Guid val = Guid.NewGuid();
            var source = new Source();
            source.StrVal = "Banana";
            source.LongVal = "12345";
            source.IntVal = "1234";
            source.NullableLongValSet = "12345";
            source.NullableIntValSet = "1234";
            source.MyEnum = "Value2";
            source.NullableEnumSet = "Value2";
            source.GuidVal = val.ToString();
            source.NullableGuidValSet = val.ToString();

            var destination = source.Convert<Destination>();

            Assert.AreEqual("Banana", destination.StrVal);
            Assert.AreEqual((long)12345, destination.LongVal);
            Assert.AreEqual((int)1234, destination.IntVal);
            Assert.AreEqual(null, destination.NullableLongValNull);
            Assert.AreEqual(null, destination.NullableIntValNull);
            Assert.AreEqual((long)12345, destination.NullableLongValSet);
            Assert.AreEqual((int)1234, destination.NullableIntValSet);
            Assert.AreEqual(MyEnum.Value2, destination.MyEnum);
            Assert.AreEqual(null, destination.NullableEnumNull);
            Assert.AreEqual(MyEnum.Value2, destination.NullableEnumSet);
            Assert.AreEqual(val, destination.GuidVal);
            Assert.AreEqual(val, destination.NullableGuidValSet);
        }

        [TestMethod]
        public void TestScalarIntConvert()
        {
            var intval = "1234".Convert<int>();
            Assert.AreEqual(1234, intval);
        }

        [TestMethod]
        public void TestScalarNullableIntConvert()
        {
            var intval = "1234".Convert<int?>();
            Assert.AreEqual(1234, intval);
        }

        [TestMethod]
        public void TestScalarNullableNullIntConvert()
        {
            var intval = "fakevalue".Convert<int?>();
            Assert.AreEqual(null, intval);
        }

        [TestMethod]
        public void TestScalarNotNullableNullIntConvert()
        {
            try
            {
                var intval = "fakevalue".Convert<int>();
                Assert.IsTrue(false);
            }
            catch (System.Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("Convert fakevalue to Int32 failed"));
            }
        }

        public class Source
        {
            public string StrVal { get; set; }
            public string LongVal { get; set; }
            public string NullableLongValNull { get; set; }
            public string NullableLongValSet { get; set; }
            public string IntVal { get; set; }
            public string NullableIntValNull { get; set; }
            public string NullableIntValSet { get; set; }
            public string GuidVal { get; set; }
            public string NullableGuidValNull { get; set; }
            public string NullableGuidValSet { get; set; }
            public string MyEnum { get; set; }
            public string NullableEnumNull { get; set; }
            public string NullableEnumSet { get; set; }
        }

        public class Destination
        {
            public string StrVal { get; set; }
            public long LongVal { get; set; }
            public long? NullableLongValNull { get; set; }
            public long? NullableLongValSet { get; set; }
            public int IntVal { get; set; }
            public int? NullableIntValNull { get; set; }
            public int? NullableIntValSet { get; set; }
            public Guid GuidVal { get; set; }
            public Guid? NullableGuidValNull { get; set; }
            public Guid? NullableGuidValSet { get; set; }
            public MyEnum MyEnum { get; set; }
            public MyEnum? NullableEnumNull { get; set; }
            public MyEnum? NullableEnumSet { get; set; }
        }
    }
}
