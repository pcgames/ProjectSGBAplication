using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
            
namespace Controllers.Statistic
{
    public class ProcessRealData
    {
        public static void ProcessRealResemplingData(string fileOfPackages, string startIndex, string fileName,
    ref string fullMessage, ref string country, ref string currentFrequancy)
        {
            List<string> dataToWrite = new List<string>();
            var dataOfPackages = DataAccess.DataReader.getNumbersOfpackages(fileOfPackages);
            var indexes = new List<Int64>();
            for (var i = 0; i < dataOfPackages.Count; i++)
            {

                if (i > 0 && Convert.ToInt64(dataOfPackages[i - 1][0]) + 1 == Convert.ToInt64(dataOfPackages[i][0]))
                {
                    if (Convert.ToDouble((dataOfPackages[i][1]).Replace('.', ',')) > Convert.ToDouble((dataOfPackages[i - 1][1]).Replace('.', ',')))
                    {
                        startIndex = dataOfPackages[i][0];
                        Controller.DecoderOfResemplingSignal(startIndex, fileName, ref fullMessage, ref country, ref currentFrequancy);
                        startIndex = dataOfPackages[i][0];
                        string toWrite = startIndex + ";" + country + ";" + currentFrequancy + ";" + fullMessage;
                        dataToWrite[dataToWrite.Count - 1] = toWrite;
                        dataOfPackages.Remove(dataOfPackages[i - 1]);
                        i -= 1;
                    }
                    else continue;

                }
                else
                {
                    startIndex = dataOfPackages[i][0];
                    Controller.DecoderOfResemplingSignal(startIndex, fileName, ref fullMessage, ref country, ref currentFrequancy);
                    string toWrite = startIndex + ";" + country + ";" + currentFrequancy + ";" + fullMessage;
                    dataToWrite.Add(toWrite);
                }


                //indexes.Add(Convert.ToInt64(dataOfPackages[i][0]));
            }
            DataAccess.DataWriter.WriteToFile(dataToWrite, fileName + "_statistics.csv");

            //for 
        }
        public static void ProcessRealResemplingDataWithPLL(string fileOfPackages, string startIndex, string fileName,
           ref string fullMessage, ref string country, ref string currentFrequancy)
        {
            List<string> dataToWrite = new List<string>();
            var dataOfPackages = DataAccess.DataReader.getNumbersOfpackages(fileOfPackages);
            var indexes = new List<Int64>();
            double std = 0;
            double meanFreq = 0;
            double phasa = 0;
            double iteration = 0;
            for (var i = 0; i < dataOfPackages.Count; i++)
            {

                if (i > 0 && Convert.ToInt64(dataOfPackages[i - 1][0]) + 1 == Convert.ToInt64(dataOfPackages[i][0]))
                {
                    if (Convert.ToDouble((dataOfPackages[i][1]).Replace('.', ',')) > Convert.ToDouble((dataOfPackages[i - 1][1]).Replace('.', ',')))
                    {
                        startIndex = dataOfPackages[i][0];
                        Controller.DecoderOfResemplingSignalWithPll(startIndex, fileName, ref fullMessage, ref country, ref currentFrequancy,ref std,ref meanFreq,ref phasa,ref iteration);
                        startIndex = dataOfPackages[i][0];
                        string toWrite = startIndex + ";" + country + ";" + currentFrequancy 
                            + ";" + fullMessage + std.ToString()+ ";" + meanFreq.ToString()+ ";" + phasa.ToString()+ ";" + iteration.ToString();
                        dataToWrite[dataToWrite.Count - 1] = toWrite;
                        dataOfPackages.Remove(dataOfPackages[i - 1]);
                        i -= 1;
                    }
                    else continue;

                }
                else
                {
                    startIndex = dataOfPackages[i][0];
                    Controller.DecoderOfResemplingSignalWithPll(startIndex, fileName, ref fullMessage, ref country, ref currentFrequancy, ref std, ref meanFreq, ref phasa, ref iteration);
                    string toWrite = startIndex + ";" + country + ";" + currentFrequancy
                        + ";" + fullMessage + std.ToString() + ";" + meanFreq.ToString() + ";" + phasa.ToString() + ";" + iteration.ToString();
                    dataToWrite.Add(toWrite);
                }


                //indexes.Add(Convert.ToInt64(dataOfPackages[i][0]));
            }
             DataAccess.DataWriter.WriteToFile(dataToWrite, fileName + "_statistics.csv");

            //for 
        }
    }
}
