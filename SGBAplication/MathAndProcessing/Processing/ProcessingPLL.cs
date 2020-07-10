using DataAccess;
using DigitalSignalProcessing;
using DigitalSignalProcessing.Windows;
using MathAndProcess.Calculations;
using MathAndProcess.Transformation;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace MathAndProcessing.Calculations
{
    public class ProcessingPLL : IProcessing
    {
        private OutputDataPLL _dataPack { get; set; }
        private CoefficientsReader _coffReader;

        public ProcessingPLL()
        {
            _dataPack = new OutputDataPLL();
            _coffReader = new CoefficientsReader();
        }

        public List<List<Complex>> Decoder(List<double> rI, List<double> rQ, string startIndexStr)
        {
            if (startIndexStr != "")
            {
                if (rI.Count != 0)
                {
                    var startIndex = Convert.ToInt32(startIndexStr);
                    var result = Mseqtransform.GetSamplesOfEmptyPart(rI, rQ, startIndex + 8);//9829622

                    EvaluationAndCompensation.PreprocessingOfSignal(result);
                    var cosData = ComplexSignals.ToComplex(rI.GetRange(startIndex + 8 - 1, 76801));
                    var sinData = ComplexSignals.ToComplex(rQ.GetRange(startIndex + 8 - 1, 76801));
                    var cosQchanel = ComplexSignals.ToComplex(rI.GetRange(startIndex + 8 - 1, 76801), true);
                    var cosIsig = Mseqtransform.GetSamplesOfFullPackage(cosData.GetRange(1, 76800));
                    var sinIsig = Mseqtransform.GetSamplesOfFullPackage(sinData.GetRange(1, 76800));
                    var cosQsig = Mseqtransform.GetSamplesOfFullPackage(cosQchanel.GetRange(1, 76800));
                    var coeffs = _coffReader.GetCoefficients("coeffs_wo_pll", 101);
                    var conv = new Convolution(ConvolutionType.Common);
                    cosQsig = conv.StartMagic(cosQsig, ComplexSignals.ToComplex(coeffs));
                    sinIsig = conv.StartMagic(sinIsig, ComplexSignals.ToComplex(coeffs));
                    cosIsig = conv.StartMagic(cosIsig, ComplexSignals.ToComplex(coeffs));
                    var signPhasa = EvaluationAndCompensation.Phaza - Math.PI / 4 > 0 ? -1 : 1;
                    var phasaShift = 2 * Math.PI / 21;
                    var minStd = 1000.0;
                    List<Complex> bestData = null;
                    for (var i = 0; i < 22; i++)
                    {
                        var phaza = EvaluationAndCompensation.Phaza - Math.PI / 4 + signPhasa * phasaShift * i;
                        var omega = Math.Round(EvaluationAndCompensation.AccuracyFreq, 4) * 2 * Math.PI * 2;
                        var pllResult = new PLL(76800, omega, phaza, 127);

                        var FIRImpulsCharacteristics = CoeficientFinder.Find(omega);
                        var data = pllResult.pll_from_class_mamedov(ComplexSignals.ToComplex(ComplexSignals.Real(cosIsig.GetRange(0, 76850)), ComplexSignals.Real(sinIsig.GetRange(0, 76850))),
                            ComplexSignals.Imaginary(cosQsig.GetRange(0, 76850)), FIRImpulsCharacteristics);
                        if (pllResult._stdOmega < minStd)
                        {
                            _dataPack.Phase = EvaluationAndCompensation.Phaza - Math.PI / 4 + signPhasa * phasaShift * i;
                            minStd = pllResult._stdOmega;
                            bestData = data.GetRange(50, 76800);
                            _dataPack.Std = minStd;
                            _dataPack.MeanFrequency_Hz = pllResult._meanOmega;
                            _dataPack.Iteration = i;
                        }

                    }

                    _dataPack.FullMessage = MathAndProcess.Decoding.Decoder.FullMessage(bestData);
                    _dataPack.Country = Convert.ToString(MathAndProcess.Decoding.Decoder.decodeCountry(_dataPack.FullMessage));
                    _dataPack.CurrentFrequency_Hz = Convert.ToString(EvaluationAndCompensation.AccuracyFreq);
                    var rnewData = new DigitalSignalProcessing.Filters.Nonrecursive.BPF(0, 1000, 76800, 100).
                        StartOperation(bestData);

                    //new Drawing.DrawingSignals(signalChart).DrawChart(Enumerable.Range(12000, 10000).Select(i => rnewData[i].Real).ToList());
                    //количество отсчетов в 1 бите OQPSK модуляции =512 т.к. в любой из частей комплексного сигнала каждый бит занимает два бита, +передискретизации два отсчёта на чип ПСП

                    var window = new Window(WindowType.Blackman, 0.16);
                    var newDataWindowed = window.StartOperation(rnewData);

                    var listOfComplexComplex = new List<List<Complex>>();
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
