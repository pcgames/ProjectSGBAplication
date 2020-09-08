using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class CoefficientsReader
    {
        public List<double> GetCoefficients(string fileName, int numberOfElements, long startIndex = 0)
        {
            char seporator = ';';
            List<double> Samples = new List<double>();
            string pathToCoeffsFolder = "..//..//..//coeffs//";
            
            using (StreamReader sr = new StreamReader(pathToCoeffsFolder + fileName))
            {
                SkipLines(sr, startIndex);

                for (long i = 0; i < numberOfElements && sr.Peek() > 0; i++)
                {
                    string line = sr.ReadLine();
                    string[] elements = line.Split(seporator);
                    string k = elements[0];
                    Samples.Add(Convert.ToDouble(elements[0]));

                }
            }
            return Samples;
        }


        private void SkipLines(StreamReader sr, long linesToSkipCount)
        {
            for (long i = 0; i < linesToSkipCount && sr.Peek() > 0; i++)
            {
                sr.ReadLine();
            }
        }
    }
}
