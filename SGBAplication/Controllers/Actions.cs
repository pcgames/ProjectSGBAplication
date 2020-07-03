using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignalProcessing;
using DigitalSignalProcessing.Windows;
using System.Windows.Forms;
using static DataAccess.DataReader;
using MathAndProcess.Calculations;
using MathAndProcess.Transformation;
using MathAndProcess;
//using DecoderSGB.Calculations.;

namespace Controllers
{
    /// <summary>
    /// данный класс создан для использования патерна Медиатор
    /// </summary>
    public class Controller
    {
        public static List<List<System.Numerics.Complex>> DecoderOfNonResemplingSignalWithPll(System.Windows.Forms.TextBox startIndex, System.Windows.Forms.TextBox fileName,
            System.Windows.Forms.RichTextBox fullMessage, System.Windows.Forms.TextBox country, System.Windows.Forms.TextBox currentFrequancy, ref double std,
            ref double meanFreq, ref double phasa, ref double iteration)
        {
            var I = new List<double>();
            var Q = new List<double>();
            getSamples(fileName.Text, ref I, ref Q, 10000000);
            var rI = ResemplingOfSignal.GetResemplingSamples(I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(Q);
            //double std = 0;
            //double meanFreq = 0;
            //double phasa = 0;
            //double iteration =0;
            return DecoderPLL(rI, rQ, startIndex, fileName, fullMessage, country, currentFrequancy, ref std, ref meanFreq, ref phasa, ref iteration);
        }
        
        public static List<List<System.Numerics.Complex>> DecoderOfResemplingSignalWithPll(System.Windows.Forms.TextBox startIndex, System.Windows.Forms.TextBox fileName,
            System.Windows.Forms.RichTextBox fullMessage, System.Windows.Forms.TextBox country, System.Windows.Forms.TextBox currentFrequancy, ref double std,
            ref double meanFreq, ref double phasa, ref double iteration)
        {
            var rI = new List<double>();
            var rQ = new List<double>();

            getSamples(fileName.Text, ref rI, ref rQ, 76809,Convert.ToInt64(startIndex.Text),';');

            System.Windows.Forms.TextBox new_ind = new System.Windows.Forms.TextBox();
            new_ind.Text = "0";//поскольку прочитали уже с нужного индекса и выбрали посылку
            return DecoderPLL(rI, rQ, new_ind, fileName, fullMessage, country, currentFrequancy, ref std, ref meanFreq, ref phasa, ref iteration);

        }
               public static List<List<System.Numerics.Complex>> DecoderOfNonResemplingSignal(System.Windows.Forms.TextBox startIndex, System.Windows.Forms.TextBox fileName,
            System.Windows.Forms.RichTextBox fullMessage, System.Windows.Forms.TextBox country, System.Windows.Forms.TextBox currentFrequancy)
        {
            var I = new List<double>();
            var Q = new List<double>();
            getSamples(fileName.Text, ref I, ref Q, 10000000);
            var rI = ResemplingOfSignal.GetResemplingSamples(I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(Q);

            return Decoder(rI, rQ, startIndex, fileName, fullMessage, country, currentFrequancy);
        }
        
        public static List<List<System.Numerics.Complex>> DecoderOfResemplingSignal(System.Windows.Forms.TextBox startIndex, System.Windows.Forms.TextBox fileName,
            System.Windows.Forms.RichTextBox fullMessage, System.Windows.Forms.TextBox country, System.Windows.Forms.TextBox currentFrequancy)
        {
            var rI = new List<double>();
            var rQ = new List<double>();

            getSamples(fileName.Text, ref rI, ref rQ, 76809,Convert.ToInt64(startIndex.Text),';');

            System.Windows.Forms.TextBox new_ind = new System.Windows.Forms.TextBox();
            new_ind.Text = "0";//поскольку прочитали уже с нужного индекса и выбрали посылку
            return Decoder(rI, rQ, new_ind, fileName, fullMessage, country, currentFrequancy);

        }
        public static void DrawingOfBPSKSignalAndSpectrum(System.Windows.Forms.DataVisualization.Charting.Chart signalChart, 
            System.Windows.Forms.DataVisualization.Charting.Chart spectrumChart, List<System.Numerics.Complex> rnewData,
            List<System.Numerics.Complex> newDataWindowed)
        {
            new DrawPlot.Drawing.DrawingSignals(signalChart).DrawChart(ModulatingSignal.generatingBPSKSignal(rnewData, 512).GetRange(20000, 10000));
            new DrawPlot.Drawing.DrawingSpectrum(spectrumChart, 76800).DrawChart(FFT.Forward(newDataWindowed.GetRange(0,8192)));
        }


        public static void Statistics(System.Windows.Forms.TextBox fileOfPackages, System.Windows.Forms.TextBox startIndex, System.Windows.Forms.TextBox fileName,
            System.Windows.Forms.RichTextBox fullMessage, System.Windows.Forms.TextBox country, System.Windows.Forms.TextBox currentFrequancy)
        {
            Statistic.ProcessRealData.ProcessRealResemplingData(fileOfPackages, startIndex, fileName, fullMessage, country, currentFrequancy);
        }
        public static void StatisticsWithPll(System.Windows.Forms.TextBox fileOfPackages, System.Windows.Forms.TextBox startIndex, System.Windows.Forms.TextBox fileName,
            System.Windows.Forms.RichTextBox fullMessage, System.Windows.Forms.TextBox country, System.Windows.Forms.TextBox currentFrequancy)
        {
            Statistic.ProcessRealData.ProcessRealResemplingDataWithPLL(fileOfPackages, startIndex, fileName, fullMessage, country, currentFrequancy);
        }
        private static List<List<System.Numerics.Complex>> Decoder(List<double> rI, List<double> rQ, System.Windows.Forms.TextBox startIndex, System.Windows.Forms.TextBox fileName,
            System.Windows.Forms.RichTextBox fullMessage, System.Windows.Forms.TextBox country, System.Windows.Forms.TextBox currentFrequancy)
        {
            if (startIndex.Text != "")
            {
                if (rI.Count != 0)
                {
                    var s = Convert.ToInt32(startIndex.Text);
                    var result = Mseqtransform.GetSamplesOfEmptyPart(rI, rQ, s + 8);//9829622
                    DataAccess.DataWriter.Writer(ComplexSignals.ToComplex(rI, rQ), "resempling_signal_full.txt");
                    EvaluationAndCompensation.PreprocessingOfSignal(result);


                    DataAccess.DataWriter.Writer(result, "emptyPart.txt");
                    var newData = EvaluationAndCompensation.
                        CompensationOfPhazeAndFrequancy(ComplexSignals.ToComplex(rI, rQ).
                        GetRange(s + 8 - 1, 76801));
                    Console.WriteLine(EvaluationAndCompensation.AccuracyFreq);

                    newData = Mseqtransform.GetSamplesOfFullPackage(newData.GetRange(1, 76800));
                    //ReaderAndWriter.Writer(newData, "data_before_psp.txt");ComplexSignals.ToComplex(rI, rQ).GetRange(s + 7, 76801)
                    fullMessage.Text = MathAndProcess.Decoding.Decoder.fullMessage(newData);
                    DataAccess.DataWriter.Writer(newData, "result_without_psp.txt");
                    country.Text = Convert.ToString(MathAndProcess.Decoding.Decoder.decodeCountry(fullMessage.Text));
                    currentFrequancy.Text = Convert.ToString(EvaluationAndCompensation.AccuracyFreq);

                    var rnewData = new DigitalSignalProcessing.Filters.Nonrecursive.BPF(0, 1000, 76800, 100).
                        StartOperation(newData);

                    //new Drawing.DrawingSignals(signalChart).DrawChart(Enumerable.Range(12000, 10000).Select(i => rnewData[i].Real).ToList());
                    //количество отсчетов в 1 бите OQPSK модуляции =512 т.к. в любой из частей комплексного сигнала каждый бит занимает два бита, +передискретизации два отсчёта на чип ПСП

                    var window = new Window(WindowType.Blackman, 0.16);
                    var newDataWindowed = window.StartOperation(rnewData);

                    var listOfComplexComplex = new List<List<System.Numerics.Complex>>();
                    listOfComplexComplex.Add(rnewData);
                    listOfComplexComplex.Add(newDataWindowed);
                    return listOfComplexComplex;
                }
                else
                {
                    startIndex.Text = "";
                    fullMessage.Text = "";
                    country.Text = "";
                    currentFrequancy.Text = "";
                    return new List<List<System.Numerics.Complex>>();
                }
            }
            else
            {
                startIndex.Text = "";
                fullMessage.Text = "";
                country.Text = "";
                currentFrequancy.Text = "";
                return new List<List<System.Numerics.Complex>>();
            }

        }
        
        private static List<List<System.Numerics.Complex>> DecoderPLL(List<double> rI, List<double> rQ, System.Windows.Forms.TextBox startIndex, System.Windows.Forms.TextBox fileName,
           System.Windows.Forms.RichTextBox fullMessage, System.Windows.Forms.TextBox country, System.Windows.Forms.TextBox currentFrequancy, ref double std,
            ref double meanFreq, ref double phasa, ref double iteration)
        {
            if (startIndex.Text != "")
            {
                if (rI.Count != 0)
                {
                    var s = Convert.ToInt32(startIndex.Text);
                    var result = Mseqtransform.GetSamplesOfEmptyPart(rI, rQ, s + 8);//9829622
                    DataAccess.DataWriter.Writer(ComplexSignals.ToComplex(rI, rQ), "resempling_signal_full.txt");
                    EvaluationAndCompensation.PreprocessingOfSignal(result);
                    var cosData = ComplexSignals.ToComplex(rI.GetRange(s + 8 - 1, 76801));
                    var sinData = ComplexSignals.ToComplex(rQ.GetRange(s + 8 - 1, 76801));
                    var cosQchanel = ComplexSignals.ToComplex(rI.GetRange(s + 8 - 1, 76801), true);
                    var cosIsig = Mseqtransform.GetSamplesOfFullPackage(cosData.GetRange(1, 76800));
                    var sinIsig = Mseqtransform.GetSamplesOfFullPackage(sinData.GetRange(1, 76800));
                    var cosQsig = Mseqtransform.GetSamplesOfFullPackage(cosQchanel.GetRange(1, 76800));
                    var coeffs = new List<double>();
                    DataAccess.DataReader.getSamples("coeffs_wo_pll", ref coeffs, 101);
                    var conv = new DigitalSignalProcessing.Convolution(ConvolutionType.Common);
                    cosQsig = conv.StartMagic(cosQsig, ComplexSignals.ToComplex(coeffs));
                    sinIsig = conv.StartMagic(sinIsig, ComplexSignals.ToComplex(coeffs));
                    cosIsig = conv.StartMagic(cosIsig, ComplexSignals.ToComplex(coeffs));
                    var signPhasa = EvaluationAndCompensation.Phaza - Math.PI / 4 > 0 ? -1 : 1;
                    var phasaShift = 2 * Math.PI / 21;
                    var minStd = 1000.0;
                    List<System.Numerics.Complex> bestData = null;
                    //var pp = l[51];
                    for (var i = 0; i < 22; i++)
                    {
                        var pllResult = new PLL(76800, Math.Round(EvaluationAndCompensation.AccuracyFreq, 4) * 2 * Math.PI * 2,
                        EvaluationAndCompensation.Phaza - Math.PI / 4+signPhasa*phasaShift*i, 127);

                        var data = pllResult.pll_from_class_mamedov(ComplexSignals.ToComplex(ComplexSignals.Real(cosIsig.GetRange(0, 76850)), ComplexSignals.Real(sinIsig.GetRange(0, 76850))),
                            ComplexSignals.Imaginary(cosQsig.GetRange(0, 76850)));
                        if (pllResult._stdOmega < minStd)
                        {
                            phasa = EvaluationAndCompensation.Phaza - Math.PI / 4 + signPhasa * phasaShift * i;
                            minStd = pllResult._stdOmega;
                            bestData = data.GetRange(50, 76800);
                            std = minStd;
                            meanFreq = pllResult._meanOmega;
                            iteration = i;
                        }

                    }


                    fullMessage.Text = MathAndProcess.Decoding.Decoder.fullMessage(bestData);
                    DataAccess.DataWriter.Writer(bestData, "result_without_psp.txt");
                    country.Text = Convert.ToString(MathAndProcess.Decoding.Decoder.decodeCountry(fullMessage.Text));
                    currentFrequancy.Text = Convert.ToString(EvaluationAndCompensation.AccuracyFreq);
                    //DigitalSignalProcessing.Filters.Recursive.
                    var rnewData = new DigitalSignalProcessing.Filters.Nonrecursive.BPF(0, 1000, 76800, 100).
                        StartOperation(bestData);
                    //var l = pllResult._stdOmega;

                    //new Drawing.DrawingSignals(signalChart).DrawChart(Enumerable.Range(12000, 10000).Select(i => rnewData[i].Real).ToList());
                    //количество отсчетов в 1 бите OQPSK модуляции =512 т.к. в любой из частей комплексного сигнала каждый бит занимает два бита, +передискретизации два отсчёта на чип ПСП

                    var window = new Window(WindowType.Blackman, 0.16);
                    var newDataWindowed = window.StartOperation(rnewData);

                    var listOfComplexComplex = new List<List<System.Numerics.Complex>>();
                    listOfComplexComplex.Add(rnewData);
                    listOfComplexComplex.Add(newDataWindowed);
                    return listOfComplexComplex;
                }
                else
                {
                    startIndex.Text = "";
                    fullMessage.Text = "";
                    country.Text = "";
                    currentFrequancy.Text = "";
                    return new List<List<System.Numerics.Complex>>();
                }
            }
            else
            {
                startIndex.Text = "";
                fullMessage.Text = "";
                country.Text = "";
                currentFrequancy.Text = "";
                return new List<List<System.Numerics.Complex>>();
            }

        }

    }
}
