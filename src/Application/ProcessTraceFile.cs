using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NormaliseTrace.Application
{
    public class ProcessTraceFile
    {
        private readonly IReaderStrategy _reader;
        private readonly List<double>    _delta;
        private readonly int             _rise;

        public ProcessTraceFile(IReaderStrategy reader, List<double> delta, int rise)
        {
            _reader = reader;
            _delta  = delta;
            _rise   = rise;
        }

        public void Process(string outputFolder, IEnumerable<string> directoryAndSearchPatterns)
        {
            Console.WriteLine("Reading trace files:");
            if (directoryAndSearchPatterns == null)
                throw new ArgumentNullException(nameof(directoryAndSearchPatterns));

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            foreach (var directoryAndSearchPattern in directoryAndSearchPatterns)
            {
                if (File.Exists(directoryAndSearchPattern))
                {
                    // This is not a directory search pattern, but a single file to read
                    var file  = directoryAndSearchPattern;
                    var input = _reader.ReadInput(file);
                    if (input.success)
                        WriteTraceOutput(file, input.data);
                }
                else
                {
                    var path          = Path.GetDirectoryName(directoryAndSearchPattern);
                    var searchPattern = Path.GetFileName(directoryAndSearchPattern);
                    var files         = Directory.EnumerateFiles(path, searchPattern);
                    ReadFiles(files);
                }
            }
        }

        public void ReadFiles(IEnumerable<string> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            foreach (var file in files)
            {
                var input = _reader.ReadInput(file);
                if (input.success)
                    WriteTraceOutput(file, input.data);
            }
        }

        private void WriteTraceOutput(string inputFile, List<List<int>> inputData)
        {
            var folder     = Path.GetDirectoryName(inputFile);
            var filename   = Path.GetFileName(inputFile);
            var outputFile = Path.Combine(folder, filename);

            Console.WriteLine($"Writing: {outputFile}");

            using var stream = new StreamWriter(outputFile, false);
            foreach (var row in inputData)
            {
                var data       = TraceProcessor.SetRiseLocationToColumnZero(row, _rise);
                var numColumns = Math.Min(data.Count, _delta.Count);

                var adjusted = _delta.Take(numColumns)
                    .Select((delta, index) => (int) Math.Round(delta * data[index]))
                    .ToList();

                stream.WriteLine(string.Join(',', adjusted));
            }
            stream.Close();
        }
    }
}
