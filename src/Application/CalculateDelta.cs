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
        public List<double> Calculate(List<int> good, List<int> bad)
        {
            if (good == null) throw new ArgumentNullException(nameof(good));
            if (bad == null)  throw new ArgumentNullException(nameof(bad));

            if(good.Count != bad.Count)
            {
                Console.WriteLine($"Invalid data. Good count = {good.Count} and Bad count = {bad.Count}. The must match.");
                Console.WriteLine("Cannot continue.");
                return null;
            }

            return good
                .Select((goodValue, index) => goodValue / (double) bad[index])
                .ToList();
        }
    }
}