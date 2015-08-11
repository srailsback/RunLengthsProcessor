using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunLengthsProcessor
{
    public class Condition
    {


        public int ID { get; set; }

        public string HWY { get; set; }

        public int DIR { get; set; }

        public decimal FROMMEASURE { get; set; }

        public decimal TOMEASURE { get; set; }

        public decimal IRIAVG { get; set; }

        public Condition() { }

    }
}
