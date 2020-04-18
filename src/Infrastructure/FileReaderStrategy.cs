using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NormaliseTrace.Application;

namespace NormaliseTrace.Infrastructure
{
    public class FileReaderStrategy : IReaderStrategy
    {
        /// <summary>
        /// Reads input data from a file and returns it
        /// </summary>
        /// <param name="inputSource">The name of the file to open and read</param>
        /// <returns>True if successful, including the data, false otherwise with null data</returns>
        public (bool success, List<List<int>> data) ReadInput(string inputSource)
        {
            if (string.IsNullOrWhiteSpace(inputSource))
                return (false, null);

            if (!File.Exists(inputSource))
            {
                Console.WriteLine("Input file {0} does not exist", inputSource);
                return (false, null);
            }

            try
            {
                Console.WriteLine(inputSource);

                var data = File.ReadLines(inputSource)
                    .Select(line => line.Split(','))
                    .Select(values => values
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(int.Parse)
                        .ToList())
                    .ToList();

                return (true, data);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to read input file {0}", inputSource);
                Console.WriteLine(e.Message);
                return (false, null);
            }
        }
    }
}