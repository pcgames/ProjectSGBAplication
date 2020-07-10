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
        ControllerMathAndProcessing _controllerMAP;
        public Controller()
        {
            _controllerMAP = new ControllerMathAndProcessing();
        }

        public List<List<Complex>> DecoderOfNonResemplingSignalWithPll(ref GUIData dataPack)
        {
            return _controllerMAP.DecoderOfNonResemplingSignalWithPll(ref dataPack);
        }

        public List<List<Complex>> DecoderOfResemplingSignalWithPll(ref GUIData dataPack)
        {
            return _controllerMAP.DecoderOfResemplingSignalWithPll(ref dataPack);
        }

        public List<List<Complex>> DecoderOfNonResemplingSignal(ref GUIData dataPack)
        {
            return _controllerMAP.DecoderOfNonResemplingSignal(ref dataPack);
        }

        public List<List<Complex>> DecoderOfResemplingSignal(ref GUIData dataPack)
        {
            return _controllerMAP.DecoderOfResemplingSignal(ref dataPack);
        }

        public void GetDataForSpectrumChart(ref List<Complex> spectrum, ref List<double> xValues, List<Complex> newDataWindowed)
        {
            spectrum = FFT.Forward(newDataWindowed.GetRange(0, 8192));
            xValues = FreqCalculation.Getfrequancy(spectrum.Count, 76800);
        }

        public static List<double> GetDataForSignalChart(List<Complex> rnewData)
        {
            return ModulatingSignal.generatingBPSKSignal(rnewData, 512).GetRange(20000, 10000);
        }

        public void Statistics(GUIData dataPack)
        {

            Statistic.ProcessRealData.ProcessRealResemplingData(dataPack);
        }

        public static void StatisticsWithPll(GUIData dataPack)
        {
            Statistic.ProcessRealData.ProcessRealResemplingDataWithPLL(dataPack);
        }
    }
}
