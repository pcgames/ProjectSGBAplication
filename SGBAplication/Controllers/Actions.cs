using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
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
        public static List<List<System.Numerics.Complex>> DecoderOfNonResemplingSignalWithPll(string startIndex, string fileName, ref GUIData dataPack)
        {


        }

        public static List<List<System.Numerics.Complex>> DecoderOfNonResemplingSignalWithPll(string startIndex, string fileName, ref GUIData dataPack)
        {
            var I = new List<double>();
            var Q = new List<double>();

            GetSamples(fileName, ref I, ref Q, 10000000);
            var rI = ResemplingOfSignal.GetResemplingSamples(I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(Q);

            ProcessingPLL processor = new ProcessingPLL();
            var output = processor.Decoder(rI, rQ, startIndex);
            var data = processor.GetOutputData();

            return output;
        }
        
        public static List<List<System.Numerics.Complex>> DecoderOfResemplingSignalWithPll(string startIndex, string fileName, ref OutputDataPLL dataPack)
        {
            var rI = new List<double>();
            var rQ = new List<double>();

            GetSamples(fileName, ref rI, ref rQ, 76809,Convert.ToInt64(startIndex),';');

            ProcessingPLL processor = new ProcessingPLL();
            var output = processor.Decoder(rI, rQ, startIndex);
            var data = processor.GetOutputData();
            return output;
        }

        public static List<List<System.Numerics.Complex>> DecoderOfNonResemplingSignal(string startIndex, string fileName, ref OutputData dataPack)
        {
            var I = new List<double>();
            var Q = new List<double>();

            GetSamples(fileName, ref I, ref Q, 10000000);
            var rI = ResemplingOfSignal.GetResemplingSamples(I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(Q);

            IProcessing processor = new Processing();
            var output = processor.Decoder(rI, rQ, startIndex);
            var data = processor.GetOutputData();
            return output;
        }
        
        public static List<List<System.Numerics.Complex>> DecoderOfResemplingSignal(string startIndex, string fileName, ref OutputData dataPack)
        {
            var rI = new List<double>();
            var rQ = new List<double>();

            GetSamples(fileName, ref rI, ref rQ, 76809,Convert.ToInt64(startIndex),';');

            Processing processor = new Processing();
            var output = processor.Decoder(rI, rQ, startIndex);
            var data = processor.GetOutputData();
            return output;

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
        public static void Statistics(string fileOfPackages, string startIndex, string fileName, ref string fullMessage, ref string country, ref string currentFrequancy)
        {
            Statistic.ProcessRealData.ProcessRealResemplingData(fileOfPackages, startIndex, fileName, ref fullMessage, ref country, ref currentFrequancy);
        }

        //Stat
        public static void StatisticsWithPll(string fileOfPackages, string startIndex, string fileName, ref string fullMessage, ref string country, ref string currentFrequancy)
        {
            Statistic.ProcessRealData.ProcessRealResemplingDataWithPLL(fileOfPackages, startIndex, fileName, ref fullMessage, ref country, ref currentFrequancy);
        }
    }
}
