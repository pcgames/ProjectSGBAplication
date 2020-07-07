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
using MathAndProcessing;
//using DecoderSGB.Calculations.;

namespace Controllers
{
    /// <summary>
    /// данный класс создан для использования патерна Медиатор
    /// </summary>
    public class Controller
    {
        public static List<List<System.Numerics.Complex>> DecoderOfNonResemplingSignalWithPll(TextBox startIndex, TextBox fileName, 
            RichTextBox fullMessage, TextBox country, TextBox currentFrequancy, ref double std, ref double meanFreq, ref double phasa, ref double iteration)
        {
            var I = new List<double>();
            var Q = new List<double>();
            GetSamples(fileName.Text, ref I, ref Q, 10000000);
            var rI = ResemplingOfSignal.GetResemplingSamples(I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(Q);
            //double std = 0;
            //double meanFreq = 0;
            //double phasa = 0;
            //double iteration =0;
            return Processing.DecoderPLL(rI, rQ, startIndex, fileName, fullMessage, country, currentFrequancy, ref std, ref meanFreq, ref phasa, ref iteration);
        }
        
        public static List<List<System.Numerics.Complex>> DecoderOfResemplingSignalWithPll(TextBox startIndex, TextBox fileName,
            RichTextBox fullMessage, TextBox country, TextBox currentFrequancy, ref double std,
            ref double meanFreq, ref double phasa, ref double iteration)
        {
            var rI = new List<double>();
            var rQ = new List<double>();

            GetSamples(fileName.Text, ref rI, ref rQ, 76809,Convert.ToInt64(startIndex.Text),';');

            System.Windows.Forms.TextBox new_ind = new System.Windows.Forms.TextBox();
            new_ind.Text = "0";//поскольку прочитали уже с нужного индекса и выбрали посылку
            return Processing.DecoderPLL(rI, rQ, new_ind, fileName, fullMessage, country, currentFrequancy, ref std, ref meanFreq, ref phasa, ref iteration);

        }

        public static List<List<System.Numerics.Complex>> DecoderOfNonResemplingSignal(TextBox startIndex, TextBox fileName,
           RichTextBox fullMessage, TextBox country, TextBox currentFrequancy)
        {
            var I = new List<double>();
            var Q = new List<double>();
            GetSamples(fileName.Text, ref I, ref Q, 10000000);
            var rI = ResemplingOfSignal.GetResemplingSamples(I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(Q);

            return Processing.Decoder(rI, rQ, startIndex, fileName, fullMessage, country, currentFrequancy);
        }
        
        public static List<List<System.Numerics.Complex>> DecoderOfResemplingSignal(TextBox startIndex, TextBox fileName,
            RichTextBox fullMessage, TextBox country, TextBox currentFrequancy)
        {
            var rI = new List<double>();
            var rQ = new List<double>();

            GetSamples(fileName.Text, ref rI, ref rQ, 76809,Convert.ToInt64(startIndex.Text),';');

            System.Windows.Forms.TextBox new_ind = new System.Windows.Forms.TextBox();
            new_ind.Text = "0";//поскольку прочитали уже с нужного индекса и выбрали посылку
            return Processing.Decoder(rI, rQ, new_ind, fileName, fullMessage, country, currentFrequancy);

        }

        //public static void DrawingOfBPSKSignalAndSpectrum(System.Windows.Forms.DataVisualization.Charting.Chart signalChart, 
        //    System.Windows.Forms.DataVisualization.Charting.Chart spectrumChart, List<System.Numerics.Complex> rnewData,
        //    List<System.Numerics.Complex> newDataWindowed)
        //{
        //    new DrawPlot.Drawing.DrawingSignals(signalChart).DrawChart(ModulatingSignal.generatingBPSKSignal(rnewData, 512).GetRange(20000, 10000));
        //    var spectrum = FFT.Forward(newDataWindowed.GetRange(0, 8192));
        //    var xValues = FreqCalculation.Getfrequancy(spectrum.Count, 76800);

        //    new SGBAplication.Drawing.DrawingSpectrum(spectrumChart, 76800).DrawChart(spectrum, xValues);
        //}

        public static void GetDataForSpectrumChart(ref List<System.Numerics.Complex> spectrum, ref List<double> xValues, List<System.Numerics.Complex> newDataWindowed)
        {
            spectrum = FFT.Forward(newDataWindowed.GetRange(0, 8192));
            xValues = FreqCalculation.Getfrequancy(spectrum.Count, 76800);
        }

        public static List<double> GetDataForSignalChart(List<System.Numerics.Complex> rnewData)
        {
            return ModulatingSignal.generatingBPSKSignal(rnewData, 512).GetRange(20000, 10000);
        }

        //Stat
        public static void Statistics(TextBox fileOfPackages, TextBox startIndex, TextBox fileName, RichTextBox fullMessage, TextBox country, TextBox currentFrequancy)
        {
            Statistic.ProcessRealData.ProcessRealResemplingData(fileOfPackages, startIndex, fileName, fullMessage, country, currentFrequancy);
        }

        //Stat
        public static void StatisticsWithPll(TextBox fileOfPackages, TextBox startIndex, TextBox fileName, RichTextBox fullMessage, TextBox country, TextBox currentFrequancy)
        {
            Statistic.ProcessRealData.ProcessRealResemplingDataWithPLL(fileOfPackages, startIndex, fileName, fullMessage, country, currentFrequancy);
        }
    }
}
