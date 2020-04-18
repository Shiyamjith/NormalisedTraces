using System.Collections.Generic;

namespace NormaliseTrace.Application
{
    public interface IFileWriter
    {
        void WriteGoodFile(List<int> data);
        void WriteBadFile(List<int> data);
        void WriteDeltaFile(List<double> delta);
    }
}