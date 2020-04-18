using System;
using System.Collections.Generic;
using NormaliseTrace.Application;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NormaliseTrace.Tests.Unit
{
    [TestFixture]
    public class CalculateDeltaTests
    {
        private CalculateDelta _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CalculateDelta();
        }

        [Test]
        public void CalculateUsingNull()
        {
            // Act
            Assert.Throws<ArgumentNullException>(() => _sut.Calculate(5, null, new List<int>()));
            Assert.Throws<ArgumentNullException>(() => _sut.Calculate(6, new List<int>(), null));
            Assert.Throws<ArgumentNullException>(() => _sut.Calculate(504, null, null));
        }

        [Test]
        [TestCase(504)]
        [TestCase(4)]
        [TestCase(3)]
        public void TooFewColumns(int numColumns)
        {
            var good = new List<int> { 1, 2, 3, 4 };
            var bad  = new List<int> { 1, 2 };
            var result = _sut.Calculate(numColumns, good, bad);
            Assert.IsNull(result);
        }
        
        [Test]
        [TestCase(2)]
        [TestCase(1)]
        [TestCase(0)]
        public void Ok(int numColumns)
        {
            var good = new List<int> { 1, 2, 3, 4 };
            var bad  = new List<int> { 1, 2 };
            var result = _sut.Calculate(numColumns, good, bad);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(numColumns, result.Count);
        }

        [Test]
        public void CalculateDelta()
        {
            var good = new List<int> { 3000, 3000, 3000 };
            var bad  = new List<int> { 2900, 3000, 3111 };
            var result = _sut.Calculate(3, good, bad);

            Assert.NotNull(result);
            Assert.AreEqual(good.Count, result.Count);
            Assert.AreEqual(3000.0 / 2900.0, result[0]);
            Assert.AreEqual(1.0,             result[1]);
            Assert.AreEqual(3000.0 / 3111.0, result[2]);
        }
    }
}