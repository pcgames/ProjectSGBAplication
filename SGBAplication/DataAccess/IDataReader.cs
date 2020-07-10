using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess2
{
    interface IDataReader
    {
        void GetSamples(string fileName, ref List<double> I, ref List<double> Q, int numberOfElements, Int64 startIndex = 0, char seporator = ';');

        List<List<string>> GetNumbersOfpackages(string fileName);
    }
}
