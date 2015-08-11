using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunLengthsProcessor
{
    public class RoadLimit
    {
        public string HWY { get; set; }
        public int DIR { get; set; }
        public decimal FROMMEASURE { get; set; }
        public decimal TOMEASURE { get; set; }

        public RoadLimit() { }
    }
}
