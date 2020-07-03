using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace DecoderSGB
{
    class ReaderAndWriter
    {
        public int _numberOfElements { get; private set; }
        
        public static void getSamples(string fileName,ref List<double> I,ref List<double> Q,int numberOfElements,Int64 startIndex=0,char seporator=';')
        {
            if (Convert.ToBoolean(fileName.IndexOf(".dat")>=0))
            {
                seporator = ',';
            }
            try
            {
            
            StreamReader sr = new StreamReader(fileName);
            string line;
            // Read and display lines from the file until the end of 
            // the file is reached.
            var numberOfCurrentRow = 0;
            while ((line = sr.ReadLine()) != null && numberOfCurrentRow < numberOfElements+startIndex )
            {
                //Console.WriteLine(line);
                if (numberOfCurrentRow >= (startIndex))
                    {
                        var elements = line.Split(seporator);
                        switch (elements.Length)
                        {
                            case 1:
                                I.Add(Convert.ToDouble(elements[0]));
                                Q.Add(0);
                                break;
                            case 2:
                                I.Add(Convert.ToDouble(elements[0]));
                                Q.Add(Convert.ToDouble(elements[1]));
                                break;
                        }
                    }
                numberOfCurrentRow += 1;
            }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("file didn't read");
                Console.WriteLine(e.Message);
            }

    }
        public static void getSamples(string fileName,ref List<double> I,int numberOfElements,Int64 startIndex=0,char seporator=';')
        {
            if (Convert.ToBoolean(fileName.IndexOf(".dat")>=0))
            {
                seporator = ',';
            }
            try
            {
            
            StreamReader sr = new StreamReader(fileName);
            string line;
            // Read and display lines from the file until the end of 
            // the file is reached.
            var numberOfCurrentRow = 0;
            while ((line = sr.ReadLine()) != null && numberOfCurrentRow < numberOfElements+startIndex )
            {
                //Console.WriteLine(line);
                if (numberOfCurrentRow >= (startIndex))
                    {
                        var elements = line.Split(seporator);
                        var k = elements[0];
                        I.Add(Convert.ToDouble(elements[0].Replace('.',',')));

                    }
                numberOfCurrentRow += 1;
            }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("file didn't read");
                Console.WriteLine(e.Message);
            }

    }

        public static List<List<string>> getNumbersOfpackages(string fileName)
        {
            var listOfNumbers = new List<List<string>>();
            FileStream fs = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var elements = line.Split(' ');
                listOfNumbers.Add(new List<string>(elements));
            }
            return listOfNumbers;
        }
        internal static void getSamples(int p)
        {
            throw new NotImplementedException();
        }
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
