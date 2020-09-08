using Controllers.Models;
using Generator.ImitationSignals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Controllers.Statistic
{
    public class StatisicGenerator
    {
        ControllerMathAndProcessing _controller = new ControllerMathAndProcessing();
        public void GenerateStatistics(int countMessages, GUIData GUIDataPack) //TODO: переименовать
        {
            GUIDataPack.StartIndex = "0";
            GUIDataPack.FileName = "simulatedSignalnew_";

            List<string> dataToWrite = new List<string>();
            int k = 0;
            for (int i = 0; i < countMessages; i++)
            {
                if (i % 1000 == 0)
                {
                    k -= 2;
                    DataAccess.DataWriter.WriteToFile(dataToWrite, GUIDataPack.FileName + "SNR=" + GUIDataPack.SNR + "_statistics.csv");
                }
                double rightFreq = 900.2;
                string rightMessage = GenerateRandomSignal(Convert.ToDouble(GUIDataPack.SNR) + k, ref rightFreq);

                _controller.StartDecoderOfNonResemplingSignalWithoutPll(ref GUIDataPack);

                string toWrite = (rightFreq - 300).ToString() + ";" + GUIDataPack.CurrentFrequency_Hz + ";" + rightMessage.Substring(50) + ";" + GUIDataPack.FullMessage;


                dataToWrite.Add(toWrite);

            }
            DataAccess.DataWriter.WriteToFile(dataToWrite, GUIDataPack.FileName + "SNR=" + GUIDataPack.SNR + "_statistics.csv");
        }

        public void GenerateStatisticsWithPLL(int countMessages, GUIData GUIDataPack)
        {
            GUIDataPack.StartIndex = "0";
            GUIDataPack.FileName = "simulatedSignalnew.csv";

            MathAndProcessing.OutputDataPLL dataPack = GUIDataPack.ConvertGUI2OutputPLLData();
            List<string> dataToWrite = new List<string>();
            int k = 0;
            for (int i = 0; i < countMessages; i++)
            {
                if (i % 300 == 0)
                {
                    k -= 2;
                    DataAccess.DataWriter.WriteToFile(dataToWrite, GUIDataPack.FileName + "SNR=" + GUIDataPack.SNR + "_statistics.csv");
                }
                double rightFreq = 900.2;
                string rightMessage = GenerateRandomSignal(Convert.ToDouble(GUIDataPack.SNR) + k, ref rightFreq);


                _controller.StartDecoderOfNonResemplingSignalWithPll(GUIDataPack, ref dataPack);

                string toWrite = (rightFreq - 300).ToString() + ";" + dataPack.CurrentFrequency_Hz + ";" + rightMessage.Substring(50) + ";" + dataPack.FullMessage + ";" +
                     dataPack.Std.ToString() + ";" + dataPack.MeanFrequency_Hz.ToString() + ";" + dataPack.Phase.ToString() + ";" + dataPack.Iteration.ToString();
                dataToWrite.Add(toWrite);

            }
            DataAccess.DataWriter.WriteToFile(dataToWrite, GUIDataPack.FileName + "SNR=" + GUIDataPack.SNR + "_statistics.csv");
        }

        private static List<int> GenerateMessage()
        {
            Random r = new Random();
            List<int> m = new List<int>();
            for (int i = 0; i < 306; i++)
            {
                if (i < 50)
                {
                    m.Add(0);
                    continue;
                }
                m.Add(r.Next(0, 2));
            }
            return m;
        }
        private static string GenerateRandomSignal(double SNR, ref double freq)
        {
            string fileName = "simulatedSignalnew.csv";
            Random r = new Random();
            List<int> message = GenerateMessage();
            string returnString = "";
            message.ForEach(a => returnString += a.ToString());
            List<int> newMessage = new List<int>();
            message.ForEach(a => newMessage.Add(a > 0 ? -1 : 1));
            freq = double.IsNaN(freq) ? r.Next(899, 903) + r.NextDouble() : freq;
            DataAccess.DataWriter.WriteToFile(new GeneratorOfSgbSignalResemplig(SNR, freq, 102300, newMessage).GetSGBSignal().ToList(), fileName);

            return returnString;
        }
    }
}
