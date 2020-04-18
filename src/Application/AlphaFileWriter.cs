using System;
using System.Collections.Generic;
using System.IO;

namespace NormaliseTrace.Application
{
    public class AlphaFileWriter : IFileWriter
    {
        public void WriteGoodFile(List<int> data)
        {
            WriteFile(Constants.GoodTraceFilename, data);
            DeleteFile(Constants.BadTraceFilename);
            DeleteFile(Constants.DeltaTraceFilename);
        }

        public void WriteBadFile(List<int> data)
        {
            WriteFile(Constants.BadTraceFilename, data);
            DeleteFile(Constants.DeltaTraceFilename);
        }

        public void WriteDeltaFile(List<double> data)
        {
            WriteFile(Constants.DeltaTraceFilename, data);
        }

        private void DeleteFile(string filename)
        {
            var folder = TraceHelper.GetCurrentFolder();
            var file = Path.Combine(folder, filename);
            if(File.Exists(file))
                File.Delete(file);
        }

        private void WriteFile<T>(string filename, List<T> data)
        {
            var folder = TraceHelper.GetCurrentFolder();
            var output = Path.Combine(folder, filename);
            Console.WriteLine($"output: {output}");

            var text = string.Join(',', data);
            File.WriteAllText(output, text);
        }
    }
}