using Controllers.Models;
using Controllers.Statistic;
using DigitalSignalProcessing;
using MathAndProcess.Calculations;
using System.Collections.Generic;
using System.Numerics;

namespace Controllers
{
    /// <summary>
    /// данный класс создан для использования патерна Медиатор
    /// </summary>
    public class ControllerSGBApplication
    {
        ControllerMathAndProcessing _controllerMAP;
        ProcessRealData _statisticControl;
        StatisicGenerator _statisticGenerator;
        public ControllerSGBApplication()
        {
            _controllerMAP = new ControllerMathAndProcessing();
            _statisticControl = new ProcessRealData(_controllerMAP);
            _statisticGenerator = new StatisicGenerator();
        }

        public List<List<Complex>>  StartDecoder(bool resampling, bool usingPll, ref GUIData dataPack)
        {
            if (resampling)
            {
                if (usingPll)
                {
                    return _controllerMAP.StartDecoderOfResemplingSignalWithPll(ref dataPack);
                }
                else
                {
                    return _controllerMAP.StartDecoderOfResemplingSignalWithPll(ref dataPack);
                }
            }
            else
            {
                if (usingPll)
                {
                    return _controllerMAP.StartDecoderOfNonResemplingSignalWithPll(ref dataPack);
                }
                else
                {
                    return _controllerMAP.StartDecoderOfNonResemplingSignalWithPll(ref dataPack);
                }
            }
        }

        public List<List<Complex>> StartDecoderOfNonResemplingSignalWithPll(ref GUIData dataPack)
        {
            return _controllerMAP.StartDecoderOfNonResemplingSignalWithPll(ref dataPack);
        }

        public List<List<Complex>> StartDecoderOfResemplingSignalWithPll(ref GUIData dataPack)
        {
            return _controllerMAP.StartDecoderOfResemplingSignalWithPll(ref dataPack);
        }

        public List<List<Complex>> StartDecoderOfNonResemplingSignalWithoutPll(ref GUIData dataPack)
        {
            return _controllerMAP.StartDecoderOfNonResemplingSignalWithoutPll(ref dataPack);
        }

        public List<List<Complex>> StartDecoderOfResemplingSignalWithoutPll(ref GUIData dataPack)
        {
            return _controllerMAP.StartDecoderOfResemplingSignalWithoutPll(ref dataPack);
        }

        public void GetDataForSpectrumChart(ref List<Complex> spectrum, ref List<double> xValues, List<Complex> newDataWindowed)
        {
            spectrum = FFT.Forward(newDataWindowed.GetRange(0, 8192));
            xValues = FreqCalculation.Getfrequancy(spectrum.Count, 76800);
        }

        public static List<double> GetDataForSignalChart(List<Complex> rnewData)
        {
            return ModulatingSignal.GenerateBPSKSignal(rnewData, 512).GetRange(20000, 10000);
        }

        public void GenerateRealResemplingDataStatistics(GUIData dataPack)
        {

            _statisticControl.ProcessRealResemplingDataWithoutPll(dataPack);
        }

        public void GenerateRealResemplingDataStatisticsWithPll(GUIData dataPack)
        {
            _statisticControl.ProcessRealResemplingDataWithPLL(dataPack);
        }

        public void GenerateStatistics(int countMessages, GUIData GUIDataPack)
        {
            _statisticGenerator.GenerateStatistics(countMessages, GUIDataPack);
        }

        public void GenerateStatisticsWithPLL(int countMessages, GUIData GUIDataPack)
        {
            _statisticGenerator.GenerateStatisticsWithPLL(countMessages, GUIDataPack);
        }
    }
}
