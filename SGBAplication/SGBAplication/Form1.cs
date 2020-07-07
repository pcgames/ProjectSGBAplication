using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DigitalSignalProcessing;
using DigitalSignalProcessing.Windows;
using Controllers;


namespace SGBFormAplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void go_Click(object sender, EventArgs e)//Я думаю это стоит перенести в отдельный класс который будет выполнять только список функций определенных кнопок
        {
            string fM = "";
            string c = "";
            string freq = "";

            switch (checkResempling.Checked)
            {
                case true:
                    var rnewDataAndSpectrum = Controller.DecoderOfResemplingSignal(startIndex.Text, fileName.Text,  ref fM, ref c, ref freq);
                    fullMessage.Text = fM;
                    country.Text = c;
                    currentFrequancy.Text =freq;
                    DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    break;
                case false:
                    if (CheckingSimulateSignal.Checked)
                    {
                        startIndex.Text = "0";
                         fileName.Text = "simulatedSignalnew.csv";
                        //var gg = ;
                        DataAccess.DataWriter.WriteToFile(new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(Convert.ToDouble(SNR.Text),900.2,102300).GetSGBSignal().ToList(), fileName.Text);

                        rnewDataAndSpectrum = Controller.DecoderOfResemplingSignal(startIndex.Text, fileName.Text, ref fM, ref c, ref freq);
                        DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    else
                    {
                        rnewDataAndSpectrum = Controller.DecoderOfNonResemplingSignal(startIndex.Text, fileName.Text, ref fM, ref c, ref freq);
                        DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    
                    break;
                
            }
            
        }

        private void DrawingOfBPSKSignalAndSpectrum(List<System.Numerics.Complex> newDataWindowed, List<System.Numerics.Complex> rnewData)
        {
            var spectrum = new List<System.Numerics.Complex>();
            List<double> xValues = new List<double>();
            Controller.GetDataForSpectrumChart(ref spectrum, ref xValues, newDataWindowed);
            var signalsWithIQChanals= Controller.GetDataForSignalChart(rnewData);

            new SGBAplication.Drawing.DrawingSignals(signalChart).DrawChart(signalsWithIQChanals);

            new SGBAplication.Drawing.DrawingSpectrum(spectrumChart, 76800).DrawChart(spectrum, xValues);
        }



        private void statisticButton_Click(object sender, EventArgs e)
        {
            string fM = "";
            string c = "";
            string freq = "";
            if (checkResempling.Checked == true)
            {
                if (checkUsePLL.Checked == true)
                {
                    Controller.StatisticsWithPll(fileOfPackages.Text, startIndex.Text, fileName.Text, ref fM, ref c, ref freq);
                    fullMessage.Text = fM;
                    country.Text = c;
                    currentFrequancy.Text = freq;
                }
                else
                {
                    Controller.Statistics(fileOfPackages.Text, startIndex.Text, fileName.Text, ref fM, ref c, ref freq);
                    fullMessage.Text = fM;
                    country.Text = c;
                    currentFrequancy.Text = freq;

                }

            }

        }

        private void statisticGenerator_Click(object sender, EventArgs e)
        {
            if (checkUsePLL.Checked)
            {
                Controllers.Statistic.GenerateStatisic.StatisticsGeneratorForPLL(10000, SNR.Text);//это ужасно!!!!!
            }
            else
            {
                Controllers.Statistic.GenerateStatisic.StatisticsGenerator(10000, SNR.Text);//АНАЛОГИЧНО

            }

        }

        private void pllProcess_Click(object sender, EventArgs e)
        {
            double std = 0;
            double meanFreq = 0;
            double phasa = 0;
            double iteration = 0;
            string fM = "";
            string c = "";
            string freq = "";
            switch (checkResempling.Checked)
            {
                case true:
                    var rnewDataAndSpectrum = Controllers.Controller.DecoderOfResemplingSignalWithPll(startIndex.Text, fileName.Text, ref fM, ref c, ref freq,
                        ref std, ref meanFreq, ref phasa, ref iteration);
                    DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    break;
                case false:
                    if (CheckingSimulateSignal.Checked)
                    {
                        startIndex.Text = "0";
                        fileName.Text = "simulatedSignalnew.csv";

                        DataAccess.DataWriter.WriteToFile(new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(Convert.ToDouble(SNR.Text), 900.2, 102300).GetSGBSignal().ToList(), fileName.Text);
                        rnewDataAndSpectrum = Controllers.Controller.DecoderOfNonResemplingSignalWithPll(startIndex.Text, fileName.Text, ref fM, ref c, ref freq, 
                            ref std, ref meanFreq, ref phasa, ref iteration);
                        DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    else
                    {
                        rnewDataAndSpectrum = Controllers.Controller.DecoderOfNonResemplingSignalWithPll(startIndex.Text, fileName.Text, ref fM, ref c, ref freq,
                            ref std, ref meanFreq, ref phasa, ref iteration);
                        DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }

                    break;

            }

        }
    }
}
