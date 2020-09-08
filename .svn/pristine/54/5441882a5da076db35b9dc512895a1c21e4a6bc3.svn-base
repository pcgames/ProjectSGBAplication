using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class SampleReader : ISampleReader
    {
        public SampleReader(/*string fullFilePath*/)
        {
            //_filePath = fullFilePath;
        }
        public int _numberOfElements { get; private set; }

        public InputData GetSamples(string fileName, int numberOfElements, long startIndex, char separator = ',')
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                InputData inputData = new InputData();
                SkipLines(sr, startIndex);

                for (long i = 0; i < numberOfElements && sr.Peek() > 0; i++)
                {
                    string line = sr.ReadLine();

                    string[] elements = line.Split(separator);
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
                return inputData;
            }
        }

        private void SkipLines(StreamReader sr, long linesToSkipCount)
        {
            for (long i = 0; i < linesToSkipCount && sr.Peek() > 0; i++)
            {
                sr.ReadLine();
            }
        }

        //Костыль для взаимодействия с прогой Назарова
        public List<List<string>> GetStartIndexAndEnergy(string fileName)
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
