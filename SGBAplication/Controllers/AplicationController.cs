﻿using Controllers.Models;
using DataAccess;
using DigitalSignalProcessing;
using MathAndProcess.Calculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Controllers
{
    public class ControllerSGBApplication
    {
        //Constants
        readonly int numOfSamples = 76800;
        readonly int numOfSOMETHING = 8192; //переименовать и выкинуть в отдельный класс
        //Пока что все константы будут в своих классах для понимания происходящего

        //не, бля это невозможно без тебя Сань
        //выпиши все константы с нормальными названиями, я их перенесу в другой класс


        ControllerMathAndProcessing _controllerMAP;
        ISampleWriter _writer;


        public ControllerSGBApplication()
        {
            _controllerMAP = new ControllerMathAndProcessing();
            _writer = new SampleWriter();
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

        public List<Complex> GetSpectrumForChart(List<Complex> newDataWindowed)
        {
            return FFT.Forward(newDataWindowed.GetRange(0, 8192));
        }

        public List<double> GetXValuesForSpectrumChart(List<Complex> spectrum)
        {
            return FreqCalculation.Getfrequancy(spectrum.Count, 76800);
        }

        public static List<double> GetDataForSignalChart(List<Complex> rnewData)
        {
            return ModulatingSignal.GenerateBPSKSignal(rnewData, 512).GetRange(20000, 10000);
        }
        public void SimulateSignal(double snr, string fileName)
        {
            List<Complex> sgbSignal = new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(snr, 900.2, 102300).GetSGBSignal().ToList();
            
            _writer.WriteToFile(sgbSignal, fileName);
        }
    }
}
