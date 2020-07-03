using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecoderSGB;

namespace Controllers.Statistic
{
    class ProcessRealData
    {
        public static void ProcessRealResemplingData(System.Windows.Forms.TextBox fileOfPackages, System.Windows.Forms.TextBox startIndex, System.Windows.Forms.TextBox fileName,
    System.Windows.Forms.RichTextBox fullMessage, System.Windows.Forms.TextBox country, System.Windows.Forms.TextBox currentFrequancy)
        {
            List<string> dataToWrite = new List<string>();
            var dataOfPackages = DataAccess.DataReader.getNumbersOfpackages(fileOfPackages.Text);
            var indexes = new List<Int64>();
            for (var i = 0; i < dataOfPackages.Count; i++)
            {

                if (i > 0 && Convert.ToInt64(dataOfPackages[i - 1][0]) + 1 == Convert.ToInt64(dataOfPackages[i][0]))
                {
                    if (Convert.ToDouble((dataOfPackages[i][1]).Replace('.', ',')) > Convert.ToDouble((dataOfPackages[i - 1][1]).Replace('.', ',')))
                    {
                        startIndex.Text = dataOfPackages[i][0];
                        DecoderSGB.DecoderOfResemplingSignal(startIndex, fileName, fullMessage, country, currentFrequancy);
                        startIndex.Text = dataOfPackages[i][0];
                        string toWrite = startIndex.Text + ";" + country.Text + ";" + currentFrequancy.Text + ";" + fullMessage.Text;
                        dataToWrite[dataToWrite.Count - 1] = toWrite;
                        dataOfPackages.Remove(dataOfPackages[i - 1]);
                        i -= 1;
                    }
                    else continue;

                }
                else
                {
                    startIndex.Text = dataOfPackages[i][0];
                    DecoderSGB.DecoderOfResemplingSignal(startIndex, fileName, fullMessage, country, currentFrequancy);
                    string toWrite = startIndex.Text + ";" + country.Text + ";" + currentFrequancy.Text + ";" + fullMessage.Text;
                    dataToWrite.Add(toWrite);
                }


                //indexes.Add(Convert.ToInt64(dataOfPackages[i][0]));
            }
            DataAccess.DataWriter.Writer(dataToWrite, fileName.Text + "_statistics.csv");

            //for 
        }
        public static void ProcessRealResemplingDataWithPLL(System.Windows.Forms.TextBox fileOfPackages, System.Windows.Forms.TextBox startIndex, System.Windows.Forms.TextBox fileName,
           System.Windows.Forms.RichTextBox fullMessage, System.Windows.Forms.TextBox country, System.Windows.Forms.TextBox currentFrequancy)
        {
            List<string> dataToWrite = new List<string>();
            var dataOfPackages = DataAccess.DataReader.getNumbersOfpackages(fileOfPackages.Text);
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
                        startIndex.Text = dataOfPackages[i][0];
                        DecoderSGB.DecoderOfResemplingSignalWithPll(startIndex, fileName, fullMessage, country, currentFrequancy,ref std,ref meanFreq,ref phasa,ref iteration);
                        startIndex.Text = dataOfPackages[i][0];
                        string toWrite = startIndex.Text + ";" + country.Text + ";" + currentFrequancy.Text 
                            + ";" + fullMessage.Text + std.ToString()+ ";" + meanFreq.ToString()+ ";" + phasa.ToString()+ ";" + iteration.ToString();
                        dataToWrite[dataToWrite.Count - 1] = toWrite;
                        dataOfPackages.Remove(dataOfPackages[i - 1]);
                        i -= 1;
                    }
                    else continue;

                }
                else
                {
                    startIndex.Text = dataOfPackages[i][0];
                    DecoderSGB.DecoderOfResemplingSignalWithPll(startIndex, fileName, fullMessage, country, currentFrequancy, ref std, ref meanFreq, ref phasa, ref iteration);
                    string toWrite = startIndex.Text + ";" + country.Text + ";" + currentFrequancy.Text
                        + ";" + fullMessage.Text + std.ToString() + ";" + meanFreq.ToString() + ";" + phasa.ToString() + ";" + iteration.ToString();
                    dataToWrite.Add(toWrite);
                }


                //indexes.Add(Convert.ToInt64(dataOfPackages[i][0]));
            }
             DataAccess.DataWriter.Writer(dataToWrite, fileName.Text + "_statistics.csv");

            //for 
        }
    }
}
