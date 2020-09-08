using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace DataAccess
{
    public class SampleWriter : ISampleWriter
    {
        public void WriteToFile(List<Complex> data, string nameOfFile, char separator = ';')
        {

            FileStream fs = new FileStream(nameOfFile, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            Action<string> writing = s => sw.WriteLine(s);
            for (int w = 0; w < data.Count; w++)
            {
                writing(data[w].Real.ToString() + separator + data[w].Imaginary.ToString());
            }
            sw.Close();
            fs.Close();
        }
    }
}
