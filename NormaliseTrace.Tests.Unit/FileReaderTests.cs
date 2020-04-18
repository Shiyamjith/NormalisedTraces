using System.Collections.Generic;
using NormaliseTrace.Application;
using NUnit.Framework;

namespace NormaliseTrace.Tests.Unit
{
    [TestFixture]
    public class FileReaderTests
    {
        [Test]
        public void Todo()
        {
            // Arrange
            var data = new List<List<int>>
            {
                new List<int> { 3, 1, 2 },
                new List<int> { 600, 200, 300 }
            };
            var strategy = new ListReaderStrategy(data);
            var sut = new FileReader(strategy);

            // Act
            var path = System.Reflection.Assembly.GetEntryAssembly().Location;


            //var result = sut.ReadFiles()

            // Assert
            //Assert.AreEqual(expected, result)
        }
    }
}