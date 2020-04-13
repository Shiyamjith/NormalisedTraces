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
            Assert.Throws<ArgumentNullException>(() => TraceHelper.AverageCentre(null));
        }
        
        [Test]
        [TestCase(10, "10")]
        [TestCase(7, "10, 4")]
        [TestCase(20, "100, 1, 20")]
        [TestCase(61, "100, 1000, 22, 34, 50, 60, 70, 65")]
        public void AverageCentreOneValue(int expected, string values)
        {
            var data = values.Split(",").Select(int.Parse).ToList();
            var result = TraceHelper.AverageCentre(data);
            Assert.AreEqual(expected, result);
        }
    }
}
