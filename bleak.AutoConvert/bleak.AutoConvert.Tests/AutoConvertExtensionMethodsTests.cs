using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace bleak.AutoConvert.Tests
{
    [TestClass]
    public class AutoConvertExtensionMethodsTests
    {
        [TestMethod]
        public void TestDictionaryConvert()
        {
            var source = new Dictionary<string, object>()
            {
                { "StrVal","Banana" },
                { "LongVal",12345 },
                { "IntVal", 1234 },
                { "NullableLongValSet", 12345 },
                { "NullableIntValSet", 1234 },
            };

            var destination = source.AutoConvert<Destination>();

            Assert.AreEqual("Banana", destination.StrVal);
            Assert.AreEqual((long)12345, destination.LongVal);
            Assert.AreEqual((int)1234, destination.IntVal);
            Assert.AreEqual(null, destination.NullableLongValNull);
            Assert.AreEqual(null, destination.NullableIntValNull);
            Assert.AreEqual((long)12345, destination.NullableLongValSet);
            Assert.AreEqual((int)1234, destination.NullableIntValSet);
        }


        [TestMethod]
        public void TestObjectConvert()
        {
            var source = new Source();
            source.StrVal = "Banana";
            source.LongVal = "12345";
            source.IntVal = "1234";
            source.NullableLongValSet = "12345";
            source.NullableIntValSet = "1234";

            var destination = source.AutoConvert<Destination>();

            Assert.AreEqual("Banana", destination.StrVal);
            Assert.AreEqual((long)12345, destination.LongVal);
            Assert.AreEqual((int)1234, destination.IntVal);
            Assert.AreEqual(null, destination.NullableLongValNull);
            Assert.AreEqual(null, destination.NullableIntValNull);
            Assert.AreEqual((long)12345, destination.NullableLongValSet);
            Assert.AreEqual((int)1234, destination.NullableIntValSet);
        }

        [TestMethod]
        public void TestScalarIntConvert()
        {
            var intval = "1234".AutoConvert<int>();
            Assert.AreEqual(1234, intval);
        }

        [TestMethod]
        public void TestScalarNullableIntConvert()
        {
            var intval = "1234".AutoConvert<int?>();
            Assert.AreEqual(1234, intval);
        }

        [TestMethod]
        public void TestScalarNullableNullIntConvert()
        {
            var intval = "fakevalue".AutoConvert<int?>();
            Assert.AreEqual(null, intval);
        }

        [TestMethod]
        public void TestScalarNotNullableNullIntConvert()
        {
            try
            {
                var intval = "fakevalue".AutoConvert<int>();
                Assert.IsTrue(false);
            }
            catch (System.Exception ex)
            {
                Assert.AreEqual(ex.Message, "Specified argument was out of the range of valid values.\nParameter name: AutoConvert fakevalue to Int32 failed");
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
        }
    }
}
