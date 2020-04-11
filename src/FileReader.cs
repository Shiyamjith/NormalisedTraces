using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NormaliseTrace
{
    public interface IFileReader
    {
        List<List<int>> ParseFolders(IEnumerable<string> searchPatterns);
        List<List<int>> ParseFiles(IEnumerable<string> files);
        IEnumerable<List<int>> ParseFile(string file);
    }

    public class FileReader : IFileReader
    {
        public List<List<int>> ParseFolders(IEnumerable<string> searchPatterns)
        {
            if (searchPatterns == null)
                throw new ArgumentNullException(nameof(searchPatterns));

            var traceValues = new List<List<int>>();
            foreach (var searchPattern in searchPatterns)
            {
                if(File.Exists(searchPattern))
                {
                    // This is not a directory search patter, but a single file to read
                    traceValues.AddRange(ParseFile(searchPattern));
                }
                else
                {
                    var files = Directory.EnumerateFiles(searchPattern);
                    traceValues.AddRange(ParseFiles(files));
                }
            }

            return traceValues;
        }

        public List<List<int>> ParseFiles(IEnumerable<string> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            var traceValues = new List<List<int>>();
            foreach (var file in files)
                traceValues.AddRange(ParseFile(file));

            return traceValues;
        }

        public IEnumerable<List<int>> ParseFile(string file)
        {
            return File.ReadLines(file)
                .Select(line => line.Split(','))
                .Select(values => values.Select(int.Parse).ToList());
        }
    }
}