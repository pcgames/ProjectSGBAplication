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
    public class ProcessingPLL : IProcessing
    {
        private OutputDataPLL _dataPack { get; set; }

        public ProcessingPLL()
        {
            _dataPack = new OutputDataPLL();
        }

        public List<List<System.Numerics.Complex>> Decoder(List<double> rI, List<double> rQ, string startIndex)
        {
            if (startIndex != "")
            {
                if (rI.Count != 0)
                {
                    var s = Convert.ToInt32(startIndex);
                    var result = Mseqtransform.GetSamplesOfEmptyPart(rI, rQ, s + 8);//9829622
                    DataAccess.DataWriter.WriteToFile(ComplexSignals.ToComplex(rI, rQ), "resempling_signal_full.txt");
                    EvaluationAndCompensation.PreprocessingOfSignal(result);
                    var cosData = ComplexSignals.ToComplex(rI.GetRange(s + 8 - 1, 76801));
                    var sinData = ComplexSignals.ToComplex(rQ.GetRange(s + 8 - 1, 76801));
                    var cosQchanel = ComplexSignals.ToComplex(rI.GetRange(s + 8 - 1, 76801), true);
                    var cosIsig = Mseqtransform.GetSamplesOfFullPackage(cosData.GetRange(1, 76800));
                    var sinIsig = Mseqtransform.GetSamplesOfFullPackage(sinData.GetRange(1, 76800));
                    var cosQsig = Mseqtransform.GetSamplesOfFullPackage(cosQchanel.GetRange(1, 76800));
                    var coeffs = new List<double>();
                    DataAccess.DataReader.GetSamples("coeffs_wo_pll", ref coeffs, 101);
                    var conv = new DigitalSignalProcessing.Convolution(ConvolutionType.Common);
                    cosQsig = conv.StartMagic(cosQsig, ComplexSignals.ToComplex(coeffs));
                    sinIsig = conv.StartMagic(sinIsig, ComplexSignals.ToComplex(coeffs));
                    cosIsig = conv.StartMagic(cosIsig, ComplexSignals.ToComplex(coeffs));
                    var signPhasa = EvaluationAndCompensation.Phaza - Math.PI / 4 > 0 ? -1 : 1;
                    var phasaShift = 2 * Math.PI / 21;
                    var minStd = 1000.0;
                    List<System.Numerics.Complex> bestData = null;
                    for (var i = 0; i < 22; i++)
                    {
                        var pllResult = new PLL(76800, Math.Round(EvaluationAndCompensation.AccuracyFreq, 4) * 2 * Math.PI * 2,
                        EvaluationAndCompensation.Phaza - Math.PI / 4 + signPhasa * phasaShift * i, 127);

                        var data = pllResult.pll_from_class_mamedov(ComplexSignals.ToComplex(ComplexSignals.Real(cosIsig.GetRange(0, 76850)), ComplexSignals.Real(sinIsig.GetRange(0, 76850))),
                            ComplexSignals.Imaginary(cosQsig.GetRange(0, 76850)));
                        if (pllResult._stdOmega < minStd)
                        {
                            _dataPack.phasa = EvaluationAndCompensation.Phaza - Math.PI / 4 + signPhasa * phasaShift * i;
                            minStd = pllResult._stdOmega;
                            bestData = data.GetRange(50, 76800);
                            _dataPack.std = minStd;
                            _dataPack.meanFreq = pllResult._meanOmega;
                            _dataPack.iteration = i;
                        }

                    }


                    _dataPack.fullMessage = MathAndProcess.Decoding.Decoder.fullMessage(bestData);
                    DataAccess.DataWriter.WriteToFile(bestData, "result_without_psp.txt");
                    _dataPack.country = Convert.ToString(MathAndProcess.Decoding.Decoder.decodeCountry(_dataPack.fullMessage));
                    _dataPack.currentFrequancy = Convert.ToString(EvaluationAndCompensation.AccuracyFreq);
                    var rnewData = new DigitalSignalProcessing.Filters.Nonrecursive.BPF(0, 1000, 76800, 100).
                        StartOperation(bestData);

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
