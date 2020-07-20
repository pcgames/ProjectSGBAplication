using DigitalSignalProcessing;
using DigitalSignalProcessing.Windows;
using MathAndProcess.Calculations;
using MathAndProcess.Transformation;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace MathAndProcessing
{
    public class Processing : IProcessing
    {
        private OutputData _dataPack { get; set; }

        public Processing()
        {
            _dataPack = new OutputData();
        }


        public List<List<Complex>> Decode(List<double> rI, List<double> rQ, string startIndexStr)
        {
            if (startIndexStr != "" && rI.Count != 0)
            {
                int startIndex = Convert.ToInt32(startIndexStr);
                List<Complex> samplesOfEmptyPart = Mseqtransform.GetSamplesOfEmptyPart(rI, rQ, startIndex + 8);//9829622

                EvaluationAndCompensation.PreprocessOfSignal(samplesOfEmptyPart);

                List<Complex> signalSamples = ComplexSignals.ToComplex(rI, rQ).GetRange(startIndex + 8 - 1, 76801);
                List<Complex> preprocessedSignalSamples = EvaluationAndCompensation.CompensationOfPhazeAndFrequancy(signalSamples);

                Console.WriteLine(EvaluationAndCompensation.AccuracyFreq);

                preprocessedSignalSamples = Mseqtransform.GetSamplesOfFullPackage(preprocessedSignalSamples.GetRange(1, 76800));

                _dataPack.FullMessage = MathAndProcess.Decoding.Decoder.GetFullMessage(preprocessedSignalSamples);

                _dataPack.Country = Convert.ToString(MathAndProcess.Decoding.Decoder.DecodeCountryCode(_dataPack.FullMessage));
                _dataPack.CurrentFrequency_Hz = Convert.ToString(EvaluationAndCompensation.AccuracyFreq);

                List<Complex> rnewData = new DigitalSignalProcessing.Filters.Nonrecursive.BPF(0, 1000, 76800, 100).
                    StartOperation(preprocessedSignalSamples);
                
                Window window = new Window(WindowType.Blackman, 0.16);
                List<Complex> newDataWindowed = window.StartOperation(rnewData);

                List<List<Complex>> listOfComplexComplex = new List<List<Complex>>();
                listOfComplexComplex.Add(rnewData);
                listOfComplexComplex.Add(newDataWindowed);
                return listOfComplexComplex;

            }
            else
            {
                _dataPack.FullMessage = "";
                _dataPack.Country = "";
                _dataPack.CurrentFrequency_Hz = "";
                return new List<List<Complex>>();
            }

        }

        public AOutputData GetOutputData()
        {
            return _dataPack;
        }
    }
}
