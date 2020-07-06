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
            
            switch (checkResempling.Checked)
            {
                case true:
                    var rnewDataAndSpectrum = Controllers.Controller.DecoderOfResemplingSignal(startIndex, fileName, fullMessage, country, currentFrequancy);
                    Controllers.Controller.DrawingOfBPSKSignalAndSpectrum(signalChart, spectrumChart, rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    break;
                case false:
                    if (CheckingSimulateSignal.Checked)
                    {
                        startIndex.Text = "0";
                         fileName.Text = "simulatedSignalnew.csv";
                        //var gg = ;
                        DataAccess.DataWriter.Writer(new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(Convert.ToDouble(SNR.Text),900.2,102300).GetSGBSignal().ToList(), fileName.Text);
                        rnewDataAndSpectrum = Controllers.Controller.DecoderOfNonResemplingSignal(startIndex, fileName, fullMessage, country, currentFrequancy);
                        Controllers.Controller.DrawingOfBPSKSignalAndSpectrum(signalChart, spectrumChart, rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    else
                    {
                        rnewDataAndSpectrum = Controllers.Controller.DecoderOfNonResemplingSignal(startIndex, fileName, fullMessage, country, currentFrequancy);
                        Controllers.Controller.DrawingOfBPSKSignalAndSpectrum(signalChart, spectrumChart, rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    
                    break;
                
            }
            
        }

        private void statisticButton_Click(object sender, EventArgs e)
        {
            if (checkResempling.Checked == true)
            {
                if (checkUsePLL.Checked == true)
                {
                    Controllers.Controller.StatisticsWithPll(fileOfPackages, startIndex, fileName, fullMessage, country, currentFrequancy);
                }
                else
                {
                    Controllers.Controller.Statistics(fileOfPackages, startIndex, fileName, fullMessage, country, currentFrequancy);

                }

            }

        }

        private void statisticGenerator_Click(object sender, EventArgs e)
        {
            if (checkUsePLL.Checked)
            {
                Controllers.Statistic.GenerateStatisic.StatisticsGeneratorForPLL(10000, SNR, startIndex, fileName, fullMessage, country, currentFrequancy);//это ужасно!!!!!
            }
            else
            {
                Controllers.Statistic.GenerateStatisic.StatisticsGenerator(10000, SNR, startIndex, fileName, fullMessage, country, currentFrequancy);//АНАЛОГИЧНО

            }

        }

        private void pllProcess_Click(object sender, EventArgs e)
        {
            double std = 0;
            double meanFreq = 0;
            double phasa = 0;
            double iteration = 0;
            switch (checkResempling.Checked)
            {
                case true:
                    var rnewDataAndSpectrum = Controllers.Controller.DecoderOfResemplingSignalWithPll(startIndex, fileName, fullMessage, country, currentFrequancy,
                        ref std, ref meanFreq, ref phasa, ref iteration);
                    Controllers.Controller.DrawingOfBPSKSignalAndSpectrum(signalChart, spectrumChart, rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    break;
                case false:
                    if (CheckingSimulateSignal.Checked)
                    {
                        startIndex.Text = "0";
                        fileName.Text = "simulatedSignalnew.csv";
                        //var gg = ;

                        DataAccess.DataWriter.Writer(new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(Convert.ToDouble(SNR.Text), 900.2, 102300).GetSGBSignal().ToList(), fileName.Text);
                        rnewDataAndSpectrum = Controllers.Controller.DecoderOfNonResemplingSignalWithPll(startIndex, fileName, fullMessage, country, currentFrequancy, 
                            ref std, ref meanFreq, ref phasa, ref iteration);
                        Controllers.Controller.DrawingOfBPSKSignalAndSpectrum(signalChart, spectrumChart, rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    else
                    {
                        rnewDataAndSpectrum = Controllers.Controller.DecoderOfNonResemplingSignalWithPll(startIndex, fileName, fullMessage, country, currentFrequancy,
                            ref std, ref meanFreq, ref phasa, ref iteration);
                        Controllers.Controller.DrawingOfBPSKSignalAndSpectrum(signalChart, spectrumChart, rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }

                    break;

            }

        }
    }
}
