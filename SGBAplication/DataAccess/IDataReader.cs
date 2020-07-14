using DataAccess2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess2
{
    interface IDataReader
    {
        InputData GetSamples(string fileName, int numberOfElements, Int64 startIndex = 0, char seporator = ';');

        List<List<string>> GetNumbersOfStartPackages(string fileName);
    }
}
