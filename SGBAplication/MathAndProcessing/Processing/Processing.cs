using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignalProcessing;
using DigitalSignalProcessing.Windows;
using static DataAccess.DataReader;
using MathAndProcess.Calculations;
using MathAndProcess.Transformation;
using MathAndProcess;

namespace MathAndProcessing
{
    public class Processing : IProcessing
    {
        private OutputData _dataPack { get; set; }

        public Processing()
        {
            _dataPack = new OutputData();
        }
        

        public List<List<System.Numerics.Complex>> Decoder(List<double> rI, List<double> rQ, string startIndex)
        {
            if (startIndex != "" && rI.Count != 0)
            {
                var s = Convert.ToInt32(startIndex);
                var result = Mseqtransform.GetSamplesOfEmptyPart(rI, rQ, s + 8);//9829622

                EvaluationAndCompensation.PreprocessingOfSignal(result);
                
                var newData = EvaluationAndCompensation.
                    CompensationOfPhazeAndFrequancy(ComplexSignals.ToComplex(rI, rQ).
                    GetRange(s + 8 - 1, 76801));

                newData = Mseqtransform.GetSamplesOfFullPackage(newData.GetRange(1, 76800));

                _dataPack.fullMessage = MathAndProcess.Decoding.Decoder.FullMessage(newData);

                _dataPack.country = Convert.ToString(MathAndProcess.Decoding.Decoder.decodeCountry(_dataPack.fullMessage));
                _dataPack.currentFrequancy = Convert.ToString(EvaluationAndCompensation.AccuracyFreq);

                var rnewData = new DigitalSignalProcessing.Filters.Nonrecursive.BPF(0, 1000, 76800, 100).
                    StartOperation(newData);

                //new Drawing.DrawingSignals(signalChart).DrawChart(Enumerable.Range(12000, 10000).Select(i => rnewData[i].Real).ToList());
                //количество отсчетов в 1 бите OQPSK модуляции =512 т.к. в любой из частей комплексного сигнала каждый бит занимает два бита, +передискретизации два отсчёта на чип ПСП

                var window = new Window(WindowType.Blackman, 0.16);
                var newDataWindowed = window.StartOperation(rnewData);

                var listOfComplexComplex = new List<List<System.Numerics.Complex>>();
                listOfComplexComplex.Add(rnewData);
                listOfComplexComplex.Add(newDataWindowed);
                return listOfComplexComplex;
                
            }
            else
            {
                _dataPack.fullMessage = "";
                _dataPack.country = "";
                _dataPack.currentFrequancy = "";
                return new List<List<System.Numerics.Complex>>();
            }

        }

        public IOutputData GetOutputData()
        {
            return _dataPack;
        }
    }
}
