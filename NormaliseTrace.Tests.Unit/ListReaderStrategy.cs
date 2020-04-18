using System.Collections.Generic;
using NormaliseTrace.Application;

namespace NormaliseTrace.Tests.Unit
{
    public class ListReaderStrategy : IReaderStrategy
    {
        private readonly List<List<int>> _data;

        public ListReaderStrategy(List<List<int>> data)
        {
            _data = data;
        }

        public (bool success, List<List<int>> data) ReadInput(string inputSource)
        {
            return (true, _data);
        }
    }
}