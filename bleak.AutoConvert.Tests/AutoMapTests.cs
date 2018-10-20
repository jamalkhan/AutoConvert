using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace bleak.AutoConvert.Tests
{
    [TestClass]
    public class AutoMapTests
    {
        [TestMethod]
        public void TestAutoMap()
        {
            var id = Guid.NewGuid();
            var foreignKey = Guid.NewGuid();
            var source = new Object1() { Id = id, Name = "Banana", ForeignKey = foreignKey };
            var destination = new Object2() { Id = id };
            AutoMap.Update(source, destination);
            Assert.AreEqual(source.Id, destination.Id);
            Assert.AreEqual(source.Name, destination.Name);
            Assert.AreEqual(source.ForeignKey, destination.ForeignKey);
        }
    }

    public class Object1
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ForeignKey { get; set; }
    }

    public class Object2
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ForeignKey { get; set; }
    }
}
