using Controllers.Models;
using DataAccess;
using DigitalSignalProcessing;
using MathAndProcess.Calculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static MathAndProcessing.SGBConstants;

namespace Controllers
{
    public class ControllerSGBApplication
    {
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
                    return _controllerMAP.StartDecoderOfNonResemplingSignalWithoutPll(ref dataPack);
            }
            throw new Exception();
        }

        public void GetDataForSpectrumChart(ref List<Complex> spectrum, ref List<double> xValues, List<Complex> newDataWindowed)
        {
            spectrum = FFT.Forward(newDataWindowed.GetRange(0, countPreambuleSamples));
            xValues = FreqCalculation.Getfrequancy(spectrum.Count, countPackageSamples);
        }

        public List<Complex> GetSpectrumForChart(List<Complex> newDataWindowed)
        {
            return FFT.Forward(newDataWindowed.GetRange(0, countPreambuleSamples));
        }

        public List<double> GetXValuesForSpectrumChart(List<Complex> spectrum)
        {
            return FreqCalculation.Getfrequancy(spectrum.Count, countPackageSamples);
        }

        public static List<double> GetDataForSignalChart(List<Complex> rnewData)
        {
            return ModulatingSignal.GenerateBPSKSignal(rnewData, countSamplesPerBit).GetRange(20000, 10000);
        }
        public void SimulateSignal(double snr, string fileName)
        {
            List<Complex> sgbSignal = new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(snr, origFreq_Hz, ORIGINAL_COUNT_PACKAGE_SAMPLES).GetSGBSignal().ToList();
            
            _writer.WriteToFile(sgbSignal, fileName);
        }
    }
}
