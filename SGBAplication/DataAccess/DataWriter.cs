using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    class DataWriter
    {
        public static void Writer(List<Complex> data, string nameOfFile, char separator = ';')
        {

            FileStream fs = new FileStream(nameOfFile, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            Action<string> writing = s => sw.WriteLine(s);
            for (var w = 0; w < data.Count; w++)
            {
                writing(data[w].Real.ToString() + separator + data[w].Imaginary.ToString());
            }
            sw.Close();
            fs.Close();
        }
        public static void Writer(List<double> data, string nameOfFile)
        {

            FileStream fs = new FileStream(nameOfFile, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            Action<string> writing = s => sw.WriteLine(s);
            for (var w = 0; w < data.Count; w++)
            {
                writing(data[w].ToString());
            }
            sw.Close();
            fs.Close();
        }
        public static void Writer(List<string> data, string nameOfFile)
        {

            FileStream fs = new FileStream(nameOfFile, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            Action<string> writing = s => sw.WriteLine(s);
            for (var w = 0; w < data.Count; w++)
            {
                writing(data[w]);
            }
            sw.Close();
            fs.Close();
        }
    }
}
