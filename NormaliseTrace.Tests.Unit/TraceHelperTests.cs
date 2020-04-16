using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NormaliseTrace.Tests.Unit
{
    [TestFixture]
    public class TraceHelperTests
    {
        [Test]
        public void TransposeTest()
        {
            // Arrange
            var data = new List<List<int>>
            {
                new List<int> { 3, 1, 2 },
                new List<int> { 600, 200, 300 }
            };

            // Act
            var result = TraceHelper.Transpose(data);
            foreach (var y in result)
            {
                foreach (var x in y)
                {
                    Console.Write($"{x}, ");
                }

                Console.WriteLine();
            }

            // Assert
            Assert.AreEqual(3, result.Count);

            var row = result[0];
            Assert.AreEqual(2, row.Count);
            Assert.AreEqual(3, row[0]);
            Assert.AreEqual(600, row[1]);

            row = result[1];
            Assert.AreEqual(2, row.Count);
            Assert.AreEqual(1, row[0]);
            Assert.AreEqual(200, row[1]);

            row = result[2];
            Assert.AreEqual(2, row.Count);
            Assert.AreEqual(2, row[0]);
            Assert.AreEqual(300, row[1]);
        }

        [Test]
        public void AverageCentreNull()
        {
            Assert.Throws<ArgumentNullException>(() => TraceHelper.AverageCentre(null, 1));
        }
        
        [Test]
        [TestCase(50, 10, "10")]
        [TestCase( 1, 10, "10")]
        [TestCase(99, 10, "10")]
        [TestCase(50,  7, "10, 4")]
        [TestCase( 1,  7, "10, 4")]
        [TestCase(99,  7, "10, 4")]
        [TestCase(50, 20, "100, 1, 20")]
        [TestCase( 1, 20, "100, 1, 20")]
        [TestCase(99, 20, "100, 1, 20")]
        [TestCase(50, 61, "100, 1000, 22, 34, 50, 60, 70, 65")]
        [TestCase( 1,  5, "0,1,2,3,4,5,6,7,8,9")]
        [TestCase(99,  4, "0,1,2,3,4,5,6,7,8,9")]
        [TestCase(90,  4, "0,1,2,3,4,5,6,7,8,9")]
        [TestCase(50, 3696, "0,25000,3546,3875,3668,3975,3890,3500,3333,999999")]
        public void AverageCentreOneValue(int percentageToKeep, int expected, string values)
        {
            var data = values.Split(",").Select(int.Parse).ToList();
            var result = TraceHelper.AverageCentre(data, percentageToKeep);
            Assert.AreEqual(expected, result);
        }
    }
}
