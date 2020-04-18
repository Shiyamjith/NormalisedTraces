using System.Collections.Generic;

namespace NormaliseTrace.Application
{
    public interface IReaderStrategy
    {
        (bool success, List<List<int>> data) ReadInput(string inputSource);
    }
}