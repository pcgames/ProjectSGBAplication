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


        public List<List<Complex>> Decoder(List<double> rI, List<double> rQ, string startIndexStr)
        {
            if (startIndexStr != "" && rI.Count != 0)
            {
                int startIndex = Convert.ToInt32(startIndexStr);
                List<Complex> result = Mseqtransform.GetSamplesOfEmptyPart(rI, rQ, startIndex + 8);//9829622

                EvaluationAndCompensation.PreprocessingOfSignal(result);

                List<Complex> newData = EvaluationAndCompensation.
                    CompensationOfPhazeAndFrequancy(ComplexSignals.ToComplex(rI, rQ).
                    GetRange(startIndex + 8 - 1, 76801));
                Console.WriteLine(EvaluationAndCompensation.AccuracyFreq);

                newData = Mseqtransform.GetSamplesOfFullPackage(newData.GetRange(1, 76800));

                _dataPack.FullMessage = MathAndProcess.Decoding.Decoder.GetFullMessage(newData);

                _dataPack.Country = Convert.ToString(MathAndProcess.Decoding.Decoder.DecodeCountryCode(_dataPack.FullMessage));
                _dataPack.CurrentFrequency_Hz = Convert.ToString(EvaluationAndCompensation.AccuracyFreq);

                List<Complex> rnewData = new DigitalSignalProcessing.Filters.Nonrecursive.BPF(0, 1000, 76800, 100).
                    StartOperation(newData);

                //new Drawing.DrawingSignals(signalChart).DrawChart(Enumerable.Range(12000, 10000).Select(i => rnewData[i].Real).ToList());
                //количество отсчетов в 1 бите OQPSK модуляции =512 т.к. в любой из частей комплексного сигнала каждый бит занимает два бита, +передискретизации два отсчёта на чип ПСП

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
