﻿using System;
using System.Collections.Generic;
using NormaliseTrace.Application;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NormaliseTrace.Tests.Unit
{
    [TestFixture]
    public class CalculateDeltaTests
    {
        [Test]
        public void CalculateUsingNull()
        {
            // Act
            Assert.Throws<ArgumentNullException>(() => TraceProcessor.CalculateDelta(null, new List<int>()));
            Assert.Throws<ArgumentNullException>(() => TraceProcessor.CalculateDelta(new List<int>(), null));
            Assert.Throws<ArgumentNullException>(() => TraceProcessor.CalculateDelta(null, null));
        }

        [Test]
        public void MinColumnCount()
        {
            var good = new List<int> { 1, 2, 3, 4 };
            var bad  = new List<int> { 5, 6 };
            var result = TraceProcessor.CalculateDelta(good, bad);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void CalculateDelta()
        {
            var good = new List<int> { 3000, 3000, 3000 };
            var bad  = new List<int> { 2900, 3000, 3111 };
            var result = TraceProcessor.CalculateDelta(good, bad);

            Assert.NotNull(result);
            Assert.AreEqual(good.Count, result.Count);
            Assert.AreEqual(3000.0 / 2900.0, result[0]);
            Assert.AreEqual(1.0,             result[1]);
            Assert.AreEqual(3000.0 / 3111.0, result[2]);
        }

        [Test]
        public void CalculateDeltaUnevenData()
        {
            var good = new List<int> { 3000, 3000, 3000 };
            var bad  = new List<int> { 2900, 3000, 3111 };
            var result = TraceProcessor.CalculateDelta(good, bad);

            Assert.NotNull(result);
            Assert.AreEqual(good.Count, result.Count);
            Assert.AreEqual(3000.0 / 2900.0, result[0]);
            Assert.AreEqual(1.0,             result[1]);
            Assert.AreEqual(3000.0 / 3111.0, result[2]);
        }
    }
}