﻿using DataAccess;
using DigitalSignalProcessing;
using DigitalSignalProcessing.Windows;
using MathAndProcess.Calculations;
using MathAndProcess.Transformation;
using System;
using System.Collections.Generic;
using System.Numerics;
using static MathAndProcessing.SGBConstants;

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

        public List<List<Complex>> Decode(List<double> rI, List<double> rQ, string startIndexStr)
        {
            double minStd = 1000.0;
            if (startIndexStr != "")
            {
                if (rI.Count != 0)
                {
                    int startIndex = Convert.ToInt32(startIndexStr);
                    List<Complex> result = Mseqtransform.GetSamplesOfEmptyPart(rI, rQ, startIndex + NAZAROF_SHIFT);//9829622

                    new EvaluationAndCompensation().PreprocessOfSignal(result);
                    List<Complex> cosData = ComplexSignals.ToComplex(rI.GetRange(startIndex + NAZAROF_SHIFT, PACKAGE_SMPLES_COUNT));
                    List<Complex> sinData = ComplexSignals.ToComplex(rQ.GetRange(startIndex + NAZAROF_SHIFT, PACKAGE_SMPLES_COUNT));
                    List<Complex> cosQchanel = ComplexSignals.ToComplex(rI.GetRange(startIndex + NAZAROF_SHIFT, PACKAGE_SMPLES_COUNT), true);
                    List<Complex> cosIsig = Mseqtransform.GetSamplesOfFullPackage(cosData);
                    List<Complex> sinIsig = Mseqtransform.GetSamplesOfFullPackage(sinData);
                    List<Complex> cosQsig = Mseqtransform.GetSamplesOfFullPackage(cosQchanel);
                    List<double> coeffs = _coffReader.GetCoefficients("coeffs_wo_pll", 101);
                    Convolution conv = new Convolution(ConvolutionType.Common);
                    cosQsig = conv.StartMagic(cosQsig, ComplexSignals.ToComplex(coeffs));
                    sinIsig = conv.StartMagic(sinIsig, ComplexSignals.ToComplex(coeffs));
                    cosIsig = conv.StartMagic(cosIsig, ComplexSignals.ToComplex(coeffs));
                    int signPhasa = EvaluationAndCompensation.Phaza - Math.PI / 4 > 0 ? -1 : 1;

                    List<Complex> bestData = null;
                    for (int i = 0; i < 22; i++)
                    {
                        double phaza = EvaluationAndCompensation.Phaza - Math.PI / 4 + signPhasa * PHAZA_SHIFT * i;
                        double omega = Math.Round(EvaluationAndCompensation.AccuracyFreq, 4) * 2 * Math.PI * 2;
                        PLL pllResult = new PLL(PACKAGE_SMPLES_COUNT, omega, phaza, BPF_IMP_RESP_LENGTH-1);

                        List<double> FIRImpulsCharacteristics = new CoeficientFinder().Find(omega);
                        var a = ComplexSignals.Imaginary(cosQsig.GetRange(0, PACKAGE_SMPLES_COUNT + SAMPLES_SHIFT));
                        List<Complex> data = pllResult.PllFromMamedov(ComplexSignals.ToComplex(ComplexSignals.Real(cosIsig.GetRange(0, PACKAGE_SMPLES_COUNT + SAMPLES_SHIFT)), ComplexSignals.Real(sinIsig.GetRange(0, PACKAGE_SMPLES_COUNT + SAMPLES_SHIFT)))
                            , FIRImpulsCharacteristics,ComplexSignals.Imaginary(cosQsig.GetRange(0, PACKAGE_SMPLES_COUNT + SAMPLES_SHIFT)));
                        if (pllResult._stdOmega < minStd)
                        {
                            _dataPack.Phase = EvaluationAndCompensation.Phaza - Math.PI / 4 + signPhasa * PHAZA_SHIFT * i;
                            minStd = pllResult._stdOmega;
                            bestData = data.GetRange(SAMPLES_SHIFT, PACKAGE_SMPLES_COUNT);
                            _dataPack.Std = minStd;
                            _dataPack.MeanFrequency_Hz = pllResult._meanOmega;
                            _dataPack.Iteration = i;
                        }

                    }

                    _dataPack.FullMessage = new MathAndProcess.Decoding.Decoder().GetFullMessage(bestData);
                    _dataPack.Country = Convert.ToString(MathAndProcess.Decoding.Decoder.DecodeCountryCode(_dataPack.FullMessage));
                    _dataPack.CurrentFrequency_Hz = Convert.ToString(EvaluationAndCompensation.AccuracyFreq);
                    List<Complex> rnewData = new DigitalSignalProcessing.Filters.Nonrecursive.BPF(LOW_FREQ_0_Hz, HIGH_FREQ_1000_Hz, PACKAGE_SMPLES_COUNT, BPF_IMP_RESP_LENGTH).
                        StartOperation(bestData);
                    
                    Window window = new Window(WindowType.Blackman, 0.16);
                    List<Complex> newDataWindowed = window.StartOperation(rnewData);

                    List<List<Complex>> listOfComplexComplex = new List<List<Complex>>
                    {
                        rnewData,
                        newDataWindowed
                    };
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
