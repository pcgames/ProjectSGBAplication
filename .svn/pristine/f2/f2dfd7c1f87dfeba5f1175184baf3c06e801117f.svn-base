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
                    int startIndex = Convert.ToInt32(startIndexStr);
                    List<Complex> result = Mseqtransform.GetSamplesOfEmptyPart(rI, rQ, startIndex + 8);//9829622

                    EvaluationAndCompensation.PreprocessingOfSignal(result);
                    List<Complex> cosData = ComplexSignals.ToComplex(rI.GetRange(startIndex + 8 - 1, 76801));
                    List<Complex> sinData = ComplexSignals.ToComplex(rQ.GetRange(startIndex + 8 - 1, 76801));
                    List<Complex> cosQchanel = ComplexSignals.ToComplex(rI.GetRange(startIndex + 8 - 1, 76801), true);
                    List<Complex> cosIsig = Mseqtransform.GetSamplesOfFullPackage(cosData.GetRange(1, 76800));
                    List<Complex> sinIsig = Mseqtransform.GetSamplesOfFullPackage(sinData.GetRange(1, 76800));
                    List<Complex> cosQsig = Mseqtransform.GetSamplesOfFullPackage(cosQchanel.GetRange(1, 76800));
                    List<double> coeffs = _coffReader.GetCoefficients("coeffs_wo_pll", 101);
                    Convolution conv = new Convolution(ConvolutionType.Common);
                    cosQsig = conv.StartMagic(cosQsig, ComplexSignals.ToComplex(coeffs));
                    sinIsig = conv.StartMagic(sinIsig, ComplexSignals.ToComplex(coeffs));
                    cosIsig = conv.StartMagic(cosIsig, ComplexSignals.ToComplex(coeffs));
                    int signPhasa = EvaluationAndCompensation.Phaza - Math.PI / 4 > 0 ? -1 : 1;
                    double phasaShift = 2 * Math.PI / 21;
                    double minStd = 1000.0;
                    List<Complex> bestData = null;
                    for (int i = 0; i < 22; i++)
                    {
                        double phaza = EvaluationAndCompensation.Phaza - Math.PI / 4 + signPhasa * phasaShift * i;
                        double omega = Math.Round(EvaluationAndCompensation.AccuracyFreq, 4) * 2 * Math.PI * 2;
                        PLL pllResult = new PLL(76800, omega, phaza, 127);

                        List<double> FIRImpulsCharacteristics = CoeficientFinder.Find(omega);
                        List<Complex> data = pllResult.pll_from_class_mamedov(ComplexSignals.ToComplex(ComplexSignals.Real(cosIsig.GetRange(0, 76850)), ComplexSignals.Real(sinIsig.GetRange(0, 76850))),
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

                    _dataPack.FullMessage = MathAndProcess.Decoding.Decoder.GetFullMessage(bestData);
                    _dataPack.Country = Convert.ToString(MathAndProcess.Decoding.Decoder.DecodeCountryCode(_dataPack.FullMessage));
                    _dataPack.CurrentFrequency_Hz = Convert.ToString(EvaluationAndCompensation.AccuracyFreq);
                    List<Complex> rnewData = new DigitalSignalProcessing.Filters.Nonrecursive.BPF(0, 1000, 76800, 100).
                        StartOperation(bestData);

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
