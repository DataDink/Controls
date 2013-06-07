using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controls.Wpf
{
    public class TestDataItem
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }

        public override string ToString()
        {
            return string.Format("Value1 {0}, Value2 {1}, Value3 {2}", Value1, Value2, Value3);
        }
    }
}
