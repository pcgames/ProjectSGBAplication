﻿using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
            
namespace Controllers.Statistic
{
    public class ProcessRealData
    {
        public static void ProcessRealResemplingData(Data.GUIData GUIDataPack)
        {
            List<string> dataToWrite = new List<string>();
            var dataOfPackages = DataAccess.DataReader.getNumbersOfpackages(GUIDataPack.fileOfPackages);
            var dataPack = GUIDataPack.GUI2OutputDataConverter();
            var indexes = new List<Int64>();

            for (var i = 0; i < dataOfPackages.Count; i++)
            {

                if (i > 0 && Convert.ToInt64(dataOfPackages[i - 1][0]) + 1 == Convert.ToInt64(dataOfPackages[i][0]))
                {
                    if (Convert.ToDouble((dataOfPackages[i][1]).Replace('.', ',')) > Convert.ToDouble((dataOfPackages[i - 1][1]).Replace('.', ',')))
                    {
                        var startIndex = dataOfPackages[i][0];
                        Controller.DecoderOfResemplingSignal(startIndex, GUIDataPack.fileName, ref dataPack);
                        startIndex = dataOfPackages[i][0];
                        string toWrite = startIndex + ";" + dataPack.country + ";" + dataPack.currentFrequancy + ";" + dataPack.fullMessage;
                        dataToWrite[dataToWrite.Count - 1] = toWrite;
                        dataOfPackages.Remove(dataOfPackages[i - 1]);
                        i -= 1;
                    }
                    else continue;

                }
                else
                {
                    var startIndex = dataOfPackages[i][0];
                    Controller.DecoderOfResemplingSignal(startIndex, GUIDataPack.fileName, ref dataPack);
                    string toWrite = startIndex + ";" + dataPack.country + ";" + dataPack.currentFrequancy + ";" + dataPack.fullMessage;
                    dataToWrite.Add(toWrite);
                }


                //indexes.Add(Convert.ToInt64(dataOfPackages[i][0]));
            }
            DataAccess.DataWriter.WriteToFile(dataToWrite, GUIDataPack.fileName + "_statistics.csv");

            //for 
        }
        public static void ProcessRealResemplingDataWithPLL(Data.GUIData GUIDataPack)
        {
            List<string> dataToWrite = new List<string>();
            var dataOfPackages = DataAccess.DataReader.getNumbersOfpackages(GUIDataPack.fileOfPackages);
            var dataPack = GUIDataPack.GUI2OutputPLLDataConverter();

            for (var i = 0; i < dataOfPackages.Count; i++)
            {

                if (i > 0 && Convert.ToInt64(dataOfPackages[i - 1][0]) + 1 == Convert.ToInt64(dataOfPackages[i][0]))
                {
                    if (Convert.ToDouble((dataOfPackages[i][1]).Replace('.', ',')) > Convert.ToDouble((dataOfPackages[i - 1][1]).Replace('.', ',')))
                    {
                        var startIndex = dataOfPackages[i][0];
                        Controller.DecoderOfResemplingSignalWithPll(startIndex, GUIDataPack.fileName, ref dataPack);
                        string toWrite = startIndex + ";" + dataPack.country + ";" + dataPack.currentFrequancy 
                            + ";" + dataPack.fullMessage + dataPack.std.ToString()+ ";" + dataPack.meanFreq.ToString()+ ";" + dataPack.phasa.ToString()+ ";" + dataPack.iteration.ToString();
                        dataToWrite[dataToWrite.Count - 1] = toWrite;
                        dataOfPackages.Remove(dataOfPackages[i - 1]);
                        i -= 1;
                    }
                    else continue;

                }
                else
                {
                    var startIndex = dataOfPackages[i][0];
                    Controller.DecoderOfResemplingSignalWithPll(startIndex, GUIDataPack.fileName, ref dataPack);
                    string toWrite = startIndex + ";" + dataPack.country + ";" + dataPack.currentFrequancy
                        + ";" + dataPack.fullMessage + dataPack.std.ToString() + ";" + dataPack.meanFreq.ToString() + ";" + dataPack.phasa.ToString() + ";" + dataPack.iteration.ToString();
                    dataToWrite.Add(toWrite);
                }


                //indexes.Add(Convert.ToInt64(dataOfPackages[i][0]));
            }
             DataAccess.DataWriter.WriteToFile(dataToWrite, GUIDataPack.fileName + "_statistics.csv");

            //for 
        }
    }
}
