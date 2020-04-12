using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NormaliseTrace.Tests.Unit
{
    [TestFixture]
    public class ColumnSorterTest
    {
        [Test]
        public void SimpleTest()
        {
            // Arrange
            var data = new List<List<int>>
            {
                new List<int> { 3, 1, 2 },
                new List<int> { 600, 200, 300 }
            };

            // Act
            var result = ColumnSorter.SortColumns(data);
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
            Assert.AreEqual(1, row[0]);
            Assert.AreEqual(200, row[1]);

            row = result[1];
            Assert.AreEqual(2, row.Count);
            Assert.AreEqual(2, row[0]);
            Assert.AreEqual(300, row[1]);

            row = result[2];
            Assert.AreEqual(2, row.Count);
            Assert.AreEqual(3, row[0]);
            Assert.AreEqual(600, row[1]);
        }
    }
}
