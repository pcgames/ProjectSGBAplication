using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generator.ImitationSignals;

namespace Controllers.Statistic
{
    public class GenerateStatisic
    {

        public static void StatisticsGenerator(int countMessages, System.Windows.Forms.TextBox SNR, System.Windows.Forms.TextBox startIndex, System.Windows.Forms.TextBox fileName,
System.Windows.Forms.RichTextBox fullMessage, System.Windows.Forms.TextBox country, System.Windows.Forms.TextBox currentFrequancy)
        {
            startIndex.Text = "0";
            fileName.Text = "simulatedSignalnew.csv";
            //var gg = ;

            List<string> dataToWrite = new List<string>();
            var k = 0;
            for (var i = 0; i < countMessages; i++)
            {
                if (i % 1000 == 0)
                {
                    k -= 2;
                    DataAccess.DataWriter.WriteToFile(dataToWrite, fileName.Text + "SNR=" + SNR.Text + "_statistics.csv");
                }
                var rightFreq = 900.2;
                var rightMessage = generatorRandomSignal(Convert.ToDouble(SNR.Text) + k, ref rightFreq);
                Controller.DecoderOfNonResemplingSignal(startIndex, fileName, fullMessage, country, currentFrequancy);
                //var detectMessage = "";
                //for (var l = 0; l < fullMessage.Text.Count(); l += 2)
                //{

                //}
                string toWrite = (rightFreq - 300).ToString() + ";" + currentFrequancy.Text + ";" + rightMessage.Substring(50) + ";" + fullMessage.Text;
                //ReaderAndWriter.Writer(dataToWrite, fileName.Text + "SNR=" + SNR.Text + "_statistics.csv");

                dataToWrite.Add(toWrite);

            }
            DataAccess.DataWriter.WriteToFile(dataToWrite, fileName.Text + "SNR=" + SNR.Text + "_statistics.csv");

            //for 
        }
        public static void StatisticsGeneratorForPLL(int countMessages, System.Windows.Forms.TextBox SNR, System.Windows.Forms.TextBox startIndex, System.Windows.Forms.TextBox fileName,
System.Windows.Forms.RichTextBox fullMessage, System.Windows.Forms.TextBox country, System.Windows.Forms.TextBox currentFrequancy)
        {
            startIndex.Text = "0";
            fileName.Text = "simulatedSignalnew.csv";
            //var gg = ;

            List<string> dataToWrite = new List<string>();
            var k = 0;
            for (var i = 0; i < countMessages; i++)
            {
                if (i % 300 == 0)
                {
                    k -= 2;
                    DataAccess.DataWriter.WriteToFile(dataToWrite, fileName.Text + "SNR=" + SNR.Text + "_statistics.csv");
                }
                //ReaderAndWriter.Writer(dataToWrite, fileName.Text + "SNR=" + SNR.Text + "_statistics.csv");

                var rightFreq = 900.2;
                var rightMessage = generatorRandomSignal(Convert.ToDouble(SNR.Text) + k, ref rightFreq);
                double std = 0;
                double meanFreq = 0;
                double phasa = 0;
                double iteration = 0;
                Controller.DecoderOfNonResemplingSignalWithPll(startIndex, fileName, fullMessage, country, currentFrequancy, ref std, ref meanFreq, ref phasa, ref iteration);
                //var detectMessage = "";
                //for (var l = 0; l < fullMessage.Text.Count(); l += 2)
                //{

                //}
                //string toWrite = startIndex.Text + ";" + country.Text + ";" + currentFrequancy.Text
                //        + ";" + fullMessage.Text + std.ToString() + ";" + meanFreq.ToString() + ";" + phasa.ToString() + ";" + iteration.ToString();

                string toWrite = (rightFreq - 300).ToString() + ";" + currentFrequancy.Text + ";" + rightMessage.Substring(50) + ";" + fullMessage.Text + ";" +
                    std.ToString() + ";" + meanFreq.ToString() + ";" + phasa.ToString() + ";" + iteration.ToString();
                //ReaderAndWriter.Writer(dataToWrite, fileName.Text + "SNR=" + SNR.Text + "_statistics.csv");

                dataToWrite.Add(toWrite);

            }
            DataAccess.DataWriter.WriteToFile(dataToWrite, fileName.Text + "SNR=" + SNR.Text + "_statistics.csv");

            //for 
        }
        private static List<int> messageGenerator()
        {
            var r = new Random();
            var m = new List<int>();
            for (var i = 0; i < 306; i++)
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
        private static string generatorRandomSignal(double SNR, ref double freq)
        {
            var fileName = "simulatedSignalnew.csv";
            var r = new Random();
            var message = messageGenerator();
            var returnString = "";
            message.ForEach(a => returnString += a.ToString());
            var newMessage = new List<int>();
            message.ForEach(a => newMessage.Add(a > 0 ? -1 : 1));
            freq = double.IsNaN(freq) ? r.Next(899, 903) + r.NextDouble() : freq;
            DataAccess.DataWriter.WriteToFile(new GeneratorOfSgbSignalResemplig(SNR, freq, 102300,newMessage).GetSGBSignal().ToList(), fileName);

            return returnString;
        }
    }
}
