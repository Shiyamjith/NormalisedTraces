using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NormaliseTrace
{
    public static class TraceProcessor
    {
        public static List<int> ProcessAlphaTrace(List<List<int>> data, int percentageToKeep, int rise)
        {
            var result = AverageColumns(data, percentageToKeep);
            return SetRiseLocationToZero(result, rise);
        }

        public static List<int> AverageColumns(List<List<int>> data, int percentageToKeep)
        {
            Console.WriteLine($"Number of lines read in: {data.Count}");
            var transposedData = Transpose(data);
            var result = new List<int>();
            // Because the data has been rotated, we iterate the rows, which are in fact columns
            foreach (var column in transposedData)
            {
                result.Add(AverageCentre(column, percentageToKeep));
            }

            return result;
        }

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
        public static int AverageCentre(List<int> data, int percentageToKeep)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            data.Sort();

            var count = data.Count;
            if (count == 1)
                return data[0];

            if (count == 2)
                return (int) Math.Round(data.Average());

            var takePercentage = percentageToKeep / 100.0;
            var skipPercentage = (1.0 - takePercentage) * 0.5;
            var skip = (int) Math.Round(count * skipPercentage);
            var take = (int) Math.Round(count * takePercentage);

            if (count == 3)
            {
                skip = 1;
                take = 1;
            }

            if (take < 1)
                take = 1;

            //Console.WriteLine($"Skip {skip} Take {take}"); // For debug

            var average = data
                .Skip(skip)
                .Take(take)
                .Average();

            return (int) Math.Round(average);
        }

        public static string GetCurrentFolder()
        {
            var path = System.Reflection.Assembly.GetEntryAssembly()?.Location;
            return Path.GetDirectoryName(path);
        }

        // This will take the point at which the trace sharply rises from it's steady state,
        // and adjusts this to be 0,0
        public static List<int> SetRiseLocationToZero(List<int> data, int rise)
        {
            var riseColumn = FindRiseColumn(data, rise);
            if (riseColumn < 20)
                return data;

            // Take min column values from 20 columns back from rise column to find minimum steady state value
            var steadyState = data.Skip(riseColumn - 20).Take(20).Min();

            // TODO: delete this line after showing Sham results
            return data.Skip(riseColumn - 1).Select(x => x - steadyState).ToList();

            // TODO: use this line in production as we need to calculate deltas with original data so the deltas are smaller
            //return data.Skip(riseColumn - 1).ToList();
        }

        // Find the column with a difference over the previous columns value by rise amount
        public static int FindRiseColumn(List<int> data, int rise)
        {
            var last = data[0];
            for(var n = 1; n < data.Count; ++n)
            {
                var value = data[n];
                if (value - last >= rise)
                    return n;
                
                last = value;
            }

            return 0;
        }
    }
}