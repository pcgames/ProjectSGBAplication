using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CoefficientsReader
    {
        public List<double> GetSamples(string fileName, int numberOfElements, Int64 startIndex = 0, char seporator = ';')
        {
            List<double> Samples = new List<double>();
            if (Convert.ToBoolean(fileName.IndexOf(".dat") >= 0))
            {
                seporator = ',';
            }
            try
            {

                StreamReader sr = new StreamReader(fileName);
                string line;
                var numberOfCurrentRow = 0;
                while ((line = sr.ReadLine()) != null && numberOfCurrentRow < numberOfElements + startIndex)
                {
                    if (numberOfCurrentRow >= (startIndex))
                    {
                        var elements = line.Split(seporator);
                        var k = elements[0];
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
