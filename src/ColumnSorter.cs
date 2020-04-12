using System.Collections.Generic;
using System.Linq;

namespace NormaliseTrace
{
    public static class ColumnSorter
    {
        public static List<List<int>> SortColumns(List<List<int>> data)
        {
            return data
                .SelectMany(inner => inner.Select((item, index) => new { item, index }))
                .GroupBy(i => i.index, i => i.item)
                .Select(g => g.ToList())
                .ToList();
        }
    }
}