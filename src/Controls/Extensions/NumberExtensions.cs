using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class NumberExtensions
    {
        public static Double GetRealOrDefault(this double value, double defaultValue = 0d)
        {
            return double.IsNaN(value) || double.IsInfinity(value)
                ? defaultValue
                : value;
        }
    }
}
