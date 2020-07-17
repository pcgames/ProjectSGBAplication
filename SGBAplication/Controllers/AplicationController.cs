using Controllers.Models;
using Controllers.Statistic;
using DigitalSignalProcessing;
using MathAndProcess.Calculations;
using System;
using System.Collections.Generic;
using System.Linq;
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

        
        public List<List<Complex>> StartDecoder(ProcessingType processingType, ref GUIData dataPack)
        {
            switch (processingType)
            {
                case ProcessingType.ResemplingWithPll:
                    return _controllerMAP.StartDecoderOfResemplingSignalWithPll(ref dataPack);

                case ProcessingType.NonResemplingWithPll:
                    return _controllerMAP.StartDecoderOfNonResemplingSignalWithPll(ref dataPack);

                case ProcessingType.ResemplingWithoutPll:
                    return _controllerMAP.StartDecoderOfResemplingSignalWithoutPll(ref dataPack);

                case ProcessingType.NonResemplingWithoutPll:
                    return _controllerMAP.StartDecoderOfNonResemplingSignalWithPll(ref dataPack);
            }
            throw new Exception();
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
        public void SimulateSignal(double snr, string fileName)
        {
            List<Complex> sgbSignal = new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(snr, 900.2, 102300).GetSGBSignal().ToList();
            DataAccess.DataWriter.WriteToFile(sgbSignal, fileName);
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
