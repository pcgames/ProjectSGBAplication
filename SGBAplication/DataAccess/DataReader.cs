using DataAccess2;
using DataAccess2.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class DataReader : IDataReader
    {
        public int _numberOfElements { get; private set; }

        public InputData GetSamples(string fileName, int numberOfElements, Int64 startIndex = 0, char seporator = ';')
        {
            if (Convert.ToBoolean(fileName.IndexOf(".dat") >= 0))
            {
                seporator = ',';
            }
            try
            {
                InputData inputData = new InputData();
                StreamReader sr = new StreamReader(fileName);
                string line;

                int numberOfCurrentRow = 0;
                while ((line = sr.ReadLine()) != null && numberOfCurrentRow < numberOfElements + startIndex)
                {
                    if (numberOfCurrentRow >= (startIndex))
                    {
                        string[] elements = line.Split(seporator);
                        switch (elements.Length)
                        {
                            case 1:
                                inputData.I.Add(Convert.ToDouble(elements[0]));
                                inputData.Q.Add(0);
                                break;
                            case 2:
                                inputData.I.Add(Convert.ToDouble(elements[0]));
                                inputData.Q.Add(Convert.ToDouble(elements[1]));
                                break;
                        }
                    }
                    numberOfCurrentRow += 1;
                }
                sr.Close();
                return inputData;
            }
            catch (Exception e)
            {
                Console.WriteLine("file didn't read");
                Console.WriteLine(e.Message);
                throw new Exception();
            }

        }


        public List<List<string>> GetNumbersOfStartPackages(string fileName)
        {
            List<List<string>> listOfNumbers = new List<List<string>>();
            FileStream fs = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] elements = line.Split(' ');
                listOfNumbers.Add(new List<string>(elements));
            }
            return listOfNumbers;
        }
    }
}
