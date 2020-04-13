using System;
using System.Collections.Generic;
using System.Linq;

namespace NormaliseTrace
{
    public static class TraceHelper
    {
        // Rotate data array 90 degrees
        public static List<List<int>> Transpose(List<List<int>> data)
        {
            return data
                .SelectMany(inner => inner.Select((item, index) => new { item, index }))
                .GroupBy(i => i.index, i => i.item)
                .Select(g => g.ToList())
                .ToList();
        }

        // Sort column into order, then throw away top and bottom 25%.
        // Returning an average of the centre 50%
        public static int AverageCentre(List<int> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            data.Sort();

            var count = data.Count;
            if (count == 1)
                return data[0];
            if (count == 2)
                return (int) Math.Round(data.Average());

            var skip = (int) Math.Round(count * 0.25);
            var take = (int) Math.Round(count * 0.50);

            if (count == 3)
            {
                skip = 1;
                take = 1;
            }

            if (take < 1)
                take = 1;

            var average = data
                .Skip(skip)
                .Take(take)
                .Average();

            return (int) Math.Round(average);
        }
    }
}