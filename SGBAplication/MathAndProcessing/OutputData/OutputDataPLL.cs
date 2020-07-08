using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAndProcessing
{
    public class OutputDataPLL : OutputData, IOutputData
    {
        public double std { get; set; }

        public double meanFreq { get; set; }

        public double phasa { get; set; }

        public double iteration { get; set; }
    }
}
