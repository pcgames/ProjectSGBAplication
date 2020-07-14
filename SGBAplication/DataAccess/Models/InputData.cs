using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess2.Models
{
    public class InputData
    {
        public InputData(List<double> i, List<double> q)
        {
            this.I = i;
            this.Q = q;
        }

        public InputData()
        {
            this.I = new List<double>();
            this.Q = new List<double>();
        }

        public List<double> I { get; set; }

        public List<double> Q { get; set; }
    }
}
