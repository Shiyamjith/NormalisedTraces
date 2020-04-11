using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NormaliseTrace
{
    public interface IFileReader
    {
        List<List<int>> ParseSearchPattern(IEnumerable<string> searchPatterns);
    }

    public class FileReader : IFileReader
    {
        public List<List<int>> ParseSearchPattern(IEnumerable<string> searchPatterns)
        {
            if (searchPatterns == null)
                throw new ArgumentNullException(nameof(searchPatterns));

            var traceValues = new List<List<int>>();
            foreach (var searchPattern in searchPatterns)
            {
                var files = Directory.EnumerateFiles(searchPattern);
                traceValues.AddRange(ParseFiles(files));
            }

            return traceValues;
        }

        public List<List<int>> ParseFiles(IEnumerable<string> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            var traceValues = new List<List<int>>();
            foreach (var file in files)
            {
                traceValues.AddRange(
                    File.ReadLines(file)
                    .Select(line => line.Split(','))
                    .Select(values => values.Select(int.Parse).ToList())
                );
            }

            return traceValues;
        }
    }
}