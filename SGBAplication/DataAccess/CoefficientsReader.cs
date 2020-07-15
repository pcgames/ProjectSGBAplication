using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class CoefficientsReader
    {
        public List<double> GetCoefficients(string fileName, int numberOfElements, Int64 startIndex = 0, char seporator = ';')
        {
            List<double> Samples = new List<double>();
            string pathToCoeffsFolder = "..//..//..//coeffs//";
            if (Convert.ToBoolean(fileName.IndexOf(".dat") >= 0))
            {
                seporator = ',';
            }
            try
            {

                StreamReader sr = new StreamReader(pathToCoeffsFolder + fileName);
                string line;
                int numberOfCurrentRow = 0;
                while ((line = sr.ReadLine()) != null && numberOfCurrentRow < numberOfElements + startIndex)
                {
                    if (numberOfCurrentRow >= (startIndex))
                    {
                        string[] elements = line.Split(seporator);
                        string k = elements[0];
                        Samples.Add(Convert.ToDouble(elements[0].Replace('.', ',')));

                    }
                    numberOfCurrentRow += 1;
                }
                sr.Close();
                return Samples;
            }
            catch (Exception e)
            {
                Console.WriteLine("file didn't read");
                Console.WriteLine(e.Message);
                throw new Exception();
            }
        }
    }
}
