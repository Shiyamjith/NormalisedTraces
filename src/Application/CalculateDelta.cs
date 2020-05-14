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

            var numColumns = Math.Min(good.Count, bad.Count);

            return good.Take(numColumns)
                .Select((goodValue, index) => goodValue / (double) (bad[index] == 0 ? goodValue : bad[index]))
                .ToList();
        }
    }
}