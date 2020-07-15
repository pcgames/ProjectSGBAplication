using DataAccess.Models;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    interface IDataReader
    {
        InputData GetSamples(string fileName, int numberOfElements, Int64 startIndex = 0, char seporator = ';');

        List<List<string>> GetNumbersOfStartPackages(string fileName);
    }
}
