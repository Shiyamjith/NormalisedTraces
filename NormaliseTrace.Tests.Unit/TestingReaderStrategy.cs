using System.Collections.Generic;
using NormaliseTrace.Application;

namespace NormaliseTrace.Tests.Unit
{
    public class TestingReaderStrategy : IReaderStrategy
    {
        private readonly List<List<int>> _data;

        public TestingReaderStrategy(List<List<int>> data)
        {
            _data = data;
        }

        public (bool success, List<List<int>> data) ReadInput(string inputSource)
        {
            return (true, _data);
        }
    }
}