using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DigitalSignalProcessing;
using DigitalSignalProcessing.Windows;
using DecoderSGB.ImitationSignals;
using DecoderSGB.Statistic;

namespace DecoderSGB
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
                        var rnewDataAndSpectrum = DecoderSGB.DecoderOfResemplingSignal(startIndex, fileName, fullMessage, country, currentFrequancy);
                        DecoderSGB.DrawingOfBPSKSignalAndSpectrum(signalChart, spectrumChart, rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                        break;
                    case false:
                        if (CheckingSimulateSignal.Checked)
                        {
                            startIndex.Text = "0";
                             fileName.Text = "simulatedSignalnew.csv";
                            //var gg = ;
                            ReaderAndWriter.Writer(new GeneratorOfSgbSignalResemplig(Convert.ToDouble(SNR.Text),900.2,102300).GetSGBSignal().ToList(), fileName.Text);
                            rnewDataAndSpectrum = DecoderSGB.DecoderOfNonResemplingSignal(startIndex, fileName, fullMessage, country, currentFrequancy);
                            DecoderSGB.DrawingOfBPSKSignalAndSpectrum(signalChart, spectrumChart, rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                        }
                        else
                        {
                            rnewDataAndSpectrum = DecoderSGB.DecoderOfNonResemplingSignal(startIndex, fileName, fullMessage, country, currentFrequancy);
                            DecoderSGB.DrawingOfBPSKSignalAndSpectrum(signalChart, spectrumChart, rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
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
                    DecoderSGB.StatisticsWithPll(fileOfPackages, startIndex, fileName, fullMessage, country, currentFrequancy);
                }
                else
                {
                    DecoderSGB.Statistics(fileOfPackages, startIndex, fileName, fullMessage, country, currentFrequancy);

                }

            }

        }

        private void statisticGenerator_Click(object sender, EventArgs e)
        {
            if (checkUsePLL.Checked)
            {
                GenerateStatisic.StatisticsGeneratorForPLL(10000, SNR, startIndex, fileName, fullMessage, country, currentFrequancy);
            }
            else
            {
                GenerateStatisic.StatisticsGenerator(10000, SNR, startIndex, fileName, fullMessage, country, currentFrequancy);

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
                    var rnewDataAndSpectrum = DecoderSGB.DecoderOfResemplingSignalWithPll(startIndex, fileName, fullMessage, country, currentFrequancy,
                        ref std, ref meanFreq, ref phasa, ref iteration);
                    DecoderSGB.DrawingOfBPSKSignalAndSpectrum(signalChart, spectrumChart, rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    break;
                case false:
                    if (CheckingSimulateSignal.Checked)
                    {
                        startIndex.Text = "0";
                        fileName.Text = "simulatedSignalnew.csv";
                        //var gg = ;

                        ReaderAndWriter.Writer(new GeneratorOfSgbSignalResemplig(Convert.ToDouble(SNR.Text), 900.2, 102300).GetSGBSignal().ToList(), fileName.Text);
                        rnewDataAndSpectrum = DecoderSGB.DecoderOfNonResemplingSignalWithPll(startIndex, fileName, fullMessage, country, currentFrequancy, 
                            ref std, ref meanFreq, ref phasa, ref iteration);
                        DecoderSGB.DrawingOfBPSKSignalAndSpectrum(signalChart, spectrumChart, rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    else
                    {
                        rnewDataAndSpectrum = DecoderSGB.DecoderOfNonResemplingSignalWithPll(startIndex, fileName, fullMessage, country, currentFrequancy,
                            ref std, ref meanFreq, ref phasa, ref iteration);
                        DecoderSGB.DrawingOfBPSKSignalAndSpectrum(signalChart, spectrumChart, rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }

                    break;

            }

        }
    }
}
