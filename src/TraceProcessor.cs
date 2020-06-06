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
            return SetRiseLocationToColumnZero(result, rise);
        }

        public static List<int> AverageColumns(List<List<int>> data, int percentageToKeep)
        {
            Console.WriteLine($"Number of lines read in: {data.Count}");

            var result = new List<int>();
            var transposedData = Transpose(data);
            // Because the data has been transposed (rotated), we iterate the rows, which are in fact columns
            foreach (var column in transposedData)
                result.Add(AverageCentre(column, percentageToKeep));

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
        // and adjusts this to be column 0
        public static List<int> SetRiseLocationToColumnZero(List<int> data, int rise)
        {
            const int window = 20;
            var riseColumn = FindRiseColumn(data, rise);
            if (riseColumn < window)
                return data;

            // Take min column values from 'window' columns back from rise column to find minimum steady state value
            var steadyState = (int) Math.Round(data.Skip(riseColumn - window).Take(window - 5).Average());

            // Use this line in production as we need to calculate deltas with original data so the deltas are smaller
            data[riseColumn - 1] = steadyState;
            return data.Skip(riseColumn - 1).ToList();
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

        public static List<int> AdjustBadSteadyStateToGoodStateState(int goodSteadyState, List<int> bad)
        {
            // First value in graph is the steady state value
            var badSteadyState = bad[0];
            var adjustBadBy = goodSteadyState - badSteadyState;

            return bad.Select(x => adjustBadBy + x).ToList();
        }

        // This class calculates the difference between the good and the bad alpha traces files.
        public static List<double> CalculateDelta(List<int> good, List<int> bad)
        {
            if (good == null) throw new ArgumentNullException(nameof(good));
            if (bad == null) throw new ArgumentNullException(nameof(bad));

            var numColumns = Math.Min(good.Count, bad.Count);

            return good.Take(numColumns)
                .Select((goodValue, index) => goodValue / (double) (bad[index] == 0 ? goodValue : bad[index]))
                .ToList();
        }
    }
}