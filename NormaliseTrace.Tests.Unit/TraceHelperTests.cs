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
            var result = TraceProcessor.Transpose(data);
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
            Assert.Throws<ArgumentNullException>(() => TraceProcessor.AverageCentre(null, 1));
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
            var data   = values.Split(",").Select(int.Parse).ToList();
            var result = TraceProcessor.AverageCentre(data, percentageToKeep);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(70,  50, "1, 2, 1, 2, 70, 150, 180, 120")]
        [TestCase(70,  65, "1, 2, 1, 2, 70, 150, 180, 120")]
        [TestCase(8,  5, "1, 2, 8, 9, 70, 150, 180, 120")]
        [TestCase(2478,  20, "2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2445,2446,2446,2446,2447,2478,2649,2908,3050,3043,2992,2972,2983,3001,3011,3016,3021,3025,3031,3034,3035,3033,3034,3038,3044,3048,3046,3039,3034")]
        [TestCase(2510,  20, "2490,2490,2490,2488,2489,2489,2489,2490,2490,2490,2490,2490,2489,2490,2490,2491,2491,2491,2491,2491,2490,2489,2490,2490,2490,2488,2488,2510,2651,2919,3085,3084,3028,3004,3016,3035,3046,3052,3055,3059,3063,3067,3068,3072,3085,3106,3130,3135,3119,3092,3069")]
        public void FindRiseColumn(int expected, int rise, string values)
        {
            var data   = values.Split(",").Select(int.Parse).ToList();
            var result = TraceProcessor.FindRiseColumn(data, rise);
            Assert.AreEqual(expected, data[result]);
            Console.WriteLine("Column {0}", result);
        }

        [Test]
        [TestCase(20, "2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2446,2445,2446,2446,2446,2447,2478,2649,2908,3050,3043,2992,2972,2983,3001,3011,3016,3021,3025,3031,3034,3035,3033,3034,3038,3044,3048,3046,3039,3034", "2,33,204,463,605,598,547,527,538,556,566,571,576,580,586,589,590,588,589,593,599,603,601,594,589")]
        [TestCase(20, "2490,2490,2490,2488,2489,2489,2489,2490,2490,2490,2490,2490,2489,2490,2490,2491,2491,2491,2491,2491,2490,2489,2490,2490,2490,2488,2488,2510,2651,2919,3085,3084,3028,3004,3016,3035,3046,3052,3055,3059,3063,3067,3068,3072,3085,3106,3130,3135,3119,3092,3069", "0,22,163,431,597,596,540,516,528,547,558,564,567,571,575,579,580,584,597,618,642,647,631,604,581")]
        public void SetRiseLocationToZero(int rise, string values, string expected)
        {
            var data   = values.Split(",").Select(int.Parse).ToList();
            var result = TraceProcessor.SetRiseLocationToZero(data, rise);
            Assert.AreEqual(expected, string.Join(',', result));
        }
    }
}
