﻿using System.Collections.Generic;

namespace NormaliseTrace.Application
{
    public interface IAlphaFileReader
    {
        List<List<int>> ParseFolders(IEnumerable<string> directoryAndSearchPatterns);
        List<List<int>> ReadFiles(IEnumerable<string> files);
        List<int>       ReadGoodFile();
        List<double>    ReadDeltaFile();
    }
}