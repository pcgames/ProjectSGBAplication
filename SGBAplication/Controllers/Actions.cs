using Controllers.Data;
using DigitalSignalProcessing;
using MathAndProcess.Calculations;
using System.Collections.Generic;
using System.Numerics;

namespace Controllers
{
    /// <summary>
    /// данный класс создан для использования патерна Медиатор
    /// </summary>
    public class Controller
    {
        public static List<List<Complex>> DecoderOfNonResemplingSignalWithPll(ref GUIData dataPack)
        {
            return ControllerMathAndProcessing.DecoderOfNonResemplingSignalWithPll(ref dataPack);
        }

        public static List<List<Complex>> DecoderOfResemplingSignalWithPll(ref GUIData dataPack)
        {
            return ControllerMathAndProcessing.DecoderOfResemplingSignalWithPll(ref dataPack);
        }

        public static List<List<Complex>> DecoderOfNonResemplingSignal(ref GUIData dataPack)
        {
            return ControllerMathAndProcessing.DecoderOfNonResemplingSignal(ref dataPack);
        }

        public static List<List<Complex>> DecoderOfResemplingSignal(ref GUIData dataPack)
        {
            return ControllerMathAndProcessing.DecoderOfResemplingSignal(ref dataPack);
        }

        public static void GetDataForSpectrumChart(ref List<Complex> spectrum, ref List<double> xValues, List<Complex> newDataWindowed)
        {
            spectrum = FFT.Forward(newDataWindowed.GetRange(0, 8192));
            xValues = FreqCalculation.Getfrequancy(spectrum.Count, 76800);
        }

        public static List<double> GetDataForSignalChart(List<Complex> rnewData)
        {
            return ModulatingSignal.generatingBPSKSignal(rnewData, 512).GetRange(20000, 10000);
        }

        public static void Statistics(GUIData dataPack )
        {

            Statistic.ProcessRealData.ProcessRealResemplingData(dataPack);
        }

        public static void StatisticsWithPll(GUIData dataPack)
        {
            Statistic.ProcessRealData.ProcessRealResemplingDataWithPLL(dataPack);
        }
    }
}
