using System;
using System.Collections.Generic;
using System.Linq;

namespace NormaliseTrace.Application
{
    /// <summary>
    /// This class calculates the difference between the good and the bad alpha traces files.
    /// </summary>
    public class CalculateDelta
    {
        // Return delta
        public List<double> Calculate(int numColumns, List<int> good, List<int> bad)
        {
            if (good == null) throw new ArgumentNullException(nameof(good));
            if (bad == null)  throw new ArgumentNullException(nameof(bad));

            if(good.Count < numColumns || bad.Count < numColumns)
            {
                Console.WriteLine($"Invalid data. Good count = {good.Count} and Bad count = {bad.Count}.");
                Console.WriteLine($"They must have a minimum of {numColumns} columns as specified by the options.");
                Console.WriteLine("Cannot continue.");
                return null;
            }

            return good.Take(numColumns)
                .Select((goodValue, index) => goodValue / (double) bad[index])
                .ToList();
        }
    }
}