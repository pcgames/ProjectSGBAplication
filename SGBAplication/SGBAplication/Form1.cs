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
            //List<int> I = new List<int>();
            //List<int> Q = new List<int>();
            //var oldFreqIndex = (int)(Convert.ToInt32(startIndex.Text) / 76800.0 * 102300 / 400000.0) * 400000;
            //Reader.getSamples(fileName.Text, ref I, ref Q, 10000000); 
            //var rI = Calculations.ResemplingOfSignal.GetResemplingSamples(I);
            //var rQ = Calculations.ResemplingOfSignal.GetResemplingSamples(Q);

            ////первый раз переношу в область нуля часть сигнала
            ////стартовый индекс равен 0 т.к. начал считать с этого индекса
            //var s = Convert.ToInt32(startIndex.Text);
            //var result = Transformation.Mseqtransform.GetSamplesOfEmptyPart(rI, rQ, s + 8);//9829622
            //Calculations.EvaluationAndCompensation.PreprocessingOfSignal(result);
            //var newData = Calculations.EvaluationAndCompensation.
            //    CompensationOfPhazeAndFrequancy(ComplexSignals.ToComplex(rI,rQ).
            //    GetRange(s+ 8,76800));
            //Console.WriteLine(Calculations.EvaluationAndCompensation.AccuracyFreq);
            ////вотрой раз переношу в область нуля часть сигнала
            ////result = Transformation.Mseqtransform.GetSamplesOfEmptyPart(newData);//9829622
            ////Calculations.EvaluationAndCompensation.PreprocessingOfSignal(result);
            //////var newData = Calculations.EvaluationAndCompensation.CompensationOfPhazeAndFrequancy(Transformation.Mseqtransform.EmptyPartOfOriginalSignal);
            ////newData = Calculations.EvaluationAndCompensation.
            ////    CompensationOfPhazeAndFrequancy(newData);
            //newData = Transformation.Mseqtransform.GetSamplesOfFullPackage(newData);
            //fullMessage.Text=Decoding.Decoder.fullMessage(newData);
            //country.Text = Convert.ToString(Decoding.Decoder.decodeCountry(fullMessage.Text));
            //currentFrequancy.Text = Convert.ToString(Calculations.EvaluationAndCompensation.AccuracyFreq);

            //var rnewData = new DigitalSignalProcessing.Filters.Nonrecursive.BPF(0, 1000, 76800, 100).
            //    StartOperation(newData);

            ////new Drawing.DrawingSignals(signalChart).DrawChart(Enumerable.Range(12000, 10000).Select(i => rnewData[i].Real).ToList());
            //new Drawing.DrawingSignals(signalChart).DrawChart(Calculations.ModulatingSignal.generatingBPSKSignal(rnewData,512).GetRange(20000,10000));
            ////количество отсчетов в 1 бите OQPSK модуляции =512 т.к. в любой из частей комплексного сигнала каждый бит занимает два бита, +передискретизации два отсчёта на чип ПСП

            //var window = new Window(WindowType.Blackman, 0.16);
            //var newDataWindowed = window.StartOperation(rnewData);
            //new Drawing.DrawingSpectrum(spectrumChart, 76800).DrawChart(FFT.Forward(newDataWindowed));
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
