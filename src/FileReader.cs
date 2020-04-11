using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NormaliseTrace
{
    public interface IFileReader
    {
        List<List<int>> ParseFolders(IEnumerable<string> directoryAndSearchPatterns);
        List<List<int>> ParseFiles(IEnumerable<string> files);
        IEnumerable<List<int>> ParseFile(string file);
    }

    public class FileReader : IFileReader
    {
        public List<List<int>> ParseFolders(IEnumerable<string> directoryAndSearchPatterns)
        {
            if (directoryAndSearchPatterns == null)
                throw new ArgumentNullException(nameof(directoryAndSearchPatterns));

            var traceValues = new List<List<int>>();
            foreach (var directoryAndSearchPattern in directoryAndSearchPatterns)
            {
                if(File.Exists(directoryAndSearchPattern))
                {
                    // This is not a directory search patter, but a single file to read
                    traceValues.AddRange(ParseFile(directoryAndSearchPattern));
                }
                else
                {
                    var path          = Path.GetDirectoryName(directoryAndSearchPattern);
                    var searchPattern = Path.GetFileName(directoryAndSearchPattern);
                    var files         = Directory.EnumerateFiles(path, searchPattern);
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