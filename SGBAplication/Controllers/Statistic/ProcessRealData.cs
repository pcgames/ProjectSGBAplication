using Controllers.Models;
using DataAccess;
using MathAndProcessing;
using System;
using System.Collections.Generic;

namespace Controllers.Statistic
{
    public class ProcessRealData
    {
        ControllerMathAndProcessing _controller;

        ISampleReader _sampleReader;

        public ProcessRealData(ControllerMathAndProcessing controller)
        {
            _controller = controller;
            _sampleReader = new SampleReader();
        }

        public void ProcessRealResemplingDataWithoutPll(GUIData GUIDataPack)
        {
            List<string> dataToWrite = new List<string>();
            List<List<string>> dataOfPackages = _sampleReader.GetStartIndexAndEnergy(GUIDataPack.fileOfPackages);
            OutputData dataPack = GUIDataPack.ConvertGUI2OutputData();

            for (int i = 0; i < dataOfPackages.Count; i++)
            {
                if (i > 0 && Convert.ToInt64(dataOfPackages[i - 1][0]) + 1 == Convert.ToInt64(dataOfPackages[i][0]))
                {
                    if (Convert.ToDouble((dataOfPackages[i][1]).Replace('.', ',')) > Convert.ToDouble((dataOfPackages[i - 1][1]).Replace('.', ',')))
                    {
                        string startIndex = dataOfPackages[i][0];
                        _controller.StartDecoderOfResemplingSignalWithoutPll(ref GUIDataPack);
                        startIndex = dataOfPackages[i][0];
                        string line = startIndex + ";" + dataPack.Country + ";" + dataPack.CurrentFrequency_Hz + ";" + dataPack.FullMessage;
                        dataToWrite[dataToWrite.Count - 1] = line;
                        dataOfPackages.Remove(dataOfPackages[i - 1]);
                        i -= 1;
                    }
                    else continue;

                }
                else
                {
                    string startIndex = dataOfPackages[i][0];
                    _controller.StartDecoderOfResemplingSignalWithoutPll(ref GUIDataPack);
                    string toWrite = startIndex + ";" + dataPack.Country + ";" + dataPack.CurrentFrequency_Hz + ";" + dataPack.FullMessage;
                    dataToWrite.Add(toWrite);
                }
            }
            DataWriter.WriteToFile(dataToWrite, GUIDataPack.FileName + "_statistics.csv");
        }
        public void ProcessRealResemplingDataWithPLL(GUIData GUIDataPack)
        {
            
            List<string> dataToWrite = new List<string>();
            List<List<string>> dataOfPackages = _sampleReader.GetStartIndexAndEnergy(GUIDataPack.fileOfPackages);
            OutputDataPLL dataPack = GUIDataPack.ConvertGUI2OutputPLLData();

            for (int i = 0; i < dataOfPackages.Count; i++)
            {

                if (i > 0 && Convert.ToInt64(dataOfPackages[i - 1][0]) + 1 == Convert.ToInt64(dataOfPackages[i][0]))
                {
                    if (Convert.ToDouble((dataOfPackages[i][1]).Replace('.', ',')) > Convert.ToDouble((dataOfPackages[i - 1][1]).Replace('.', ',')))
                    {
                        string startIndex = dataOfPackages[i][0];
                        _controller.StartDecoderOfResemplingSignalWithPll(ref GUIDataPack);
                        string line = startIndex + ";" + dataPack.Country + ";" + dataPack.CurrentFrequency_Hz
                            + ";" + dataPack.FullMessage + dataPack.Std.ToString() + ";" + dataPack.MeanFrequency_Hz.ToString() + 
                            ";" + dataPack.Phase.ToString() + ";" + dataPack.Iteration.ToString();
                        dataToWrite[dataToWrite.Count - 1] = line;
                        dataOfPackages.Remove(dataOfPackages[i - 1]);
                        i -= 1;
                    }
                    else continue;

                }
                else
                {
                    string startIndex = dataOfPackages[i][0];
                    _controller.StartDecoderOfResemplingSignalWithPll(ref GUIDataPack);
                    string toWrite = startIndex + ";" + dataPack.Country + ";" + dataPack.CurrentFrequency_Hz
                        + ";" + dataPack.FullMessage + dataPack.Std.ToString() + ";" + dataPack.MeanFrequency_Hz.ToString() + ";" + dataPack.Phase.ToString() + ";" + dataPack.Iteration.ToString();
                    dataToWrite.Add(toWrite);
                }
            }
            DataWriter.WriteToFile(dataToWrite, GUIDataPack.FileName + "_statistics.csv");
        }

        private string MakeLine(OutputDataPLL dataPack, string startIndex)
        {


            return 
        }
    }
}
