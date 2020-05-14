using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NormaliseTrace.Application
{
    public class AlphaFileReader : IAlphaFileReader
    {
        private readonly IReaderStrategy _reader;

        public AlphaFileReader(IReaderStrategy reader)
        {
            _reader = reader;
        }

        public List<List<int>> ParseFolders(IEnumerable<string> directoryAndSearchPatterns)
        {
            Console.WriteLine("Reading alpha files:");
            if (directoryAndSearchPatterns == null)
                throw new ArgumentNullException(nameof(directoryAndSearchPatterns));

            var traceValues = new List<List<int>>();
            foreach (var directoryAndSearchPattern in directoryAndSearchPatterns)
            {
                if(File.Exists(directoryAndSearchPattern))
                {
                    // This is not a directory search pattern, but a single file to read
                    var input = _reader.ReadInput(directoryAndSearchPattern);
                    if (input.success)
                        traceValues.AddRange(input.data);
                }
                else
                {
                    var path          = Path.GetDirectoryName(directoryAndSearchPattern);
                    var searchPattern = Path.GetFileName(directoryAndSearchPattern);
                    var files         = Directory.EnumerateFiles(path, searchPattern);
                    traceValues.AddRange(ReadFiles(files));
                }
            }

            return traceValues;
        }

        public List<List<int>> ReadFiles(IEnumerable<string> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            var traceValues = new List<List<int>>();
            foreach (var file in files)
            {
                var input = _reader.ReadInput(file);
                if (input.success)
                    traceValues.AddRange(input.data);
            }

            return traceValues;
        }

        public List<int> ReadGoodFile()
        {
            var folder = TraceProcessor.GetCurrentFolder();
            var filename = Path.Combine(folder, Constants.GoodTraceFilename);
            var result = _reader.ReadInput(filename);
            if (!result.success)
            {
                Console.WriteLine($"Unable to read in good alpha file {filename}");
                return null;
            }

            return result.data[0];
        }

        public List<double> ReadDeltaFile()
        {
            var folder   = TraceProcessor.GetCurrentFolder();
            var filename = Path.Combine(folder, Constants.GoodTraceFilename);

            return File.ReadAllText(filename)
                .Split(',')
                .Select(double.Parse)
                .ToList();
        }
    }
}