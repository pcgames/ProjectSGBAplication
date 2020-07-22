using DigitalSignalProcessing;
using MathAndPhysics;
using MathAndPhysics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MathAndProcess.Calculations
{
    public class PLL
    {
        double _Fs;
        double _omega;
        public List<double> _lisOmega { get; private set; }
        public double _stdOmega { get; private set; }
        public double _meanOmega { get; private set; }

        public double _inputPhasa { get; private set; }
        private double _k1 = .02790479;
        private double _k2 = .0000099;
        private int countOfCoeffs;




        public PLL(double Fs, double omega, double input_phasa, int count_of_coeffs = 101)
        {
            this._Fs = Fs;
            this._inputPhasa = input_phasa;
            while (Math.Log(count_of_coeffs + 1, 2) % 1 != 0)
            {
                count_of_coeffs += 1;
            }
            countOfCoeffs = count_of_coeffs;
            _omega = omega;

        }
        private static void ShiftRegistr<T>(T[] arr, int shifts)
        {
            Array.Copy(arr, shifts, arr, 0, arr.Length - shifts);
            Array.Clear(arr, arr.Length - shifts, shifts);
        }
        private static Complex PhaseDetector(Complex first, Complex second)
        {
            return Complex.Multiply(first, Complex.Conjugate(second));
        }
        private static Complex OutPhaseDetector(Complex first, Complex second)
        {
            return first.Real * second.Real + Complex.ImaginaryOne * first.Imaginary * second.Imaginary;

        }

        private Complex VCOGenerator(double omega, int i, double dphi = 0)
        {
            return Complex.Exp(Complex.ImaginaryOne * (omega * i / _Fs + dphi));

        }

        public List<Complex> PllFromMamedov(List<Complex> realSignal, List<double> coeffs, List<double> QSignal = null)
        {
            List<Complex> returnSignal = new List<Complex>();
            double dphi = 0.0;
            double downLoop = 0.0;
            double VCOLoop = 0.0;
            double multiply = Math.Abs(Math.Cos(_inputPhasa)) > .2 ? Math.Cos(_inputPhasa) : Math.Sin(_inputPhasa);
            Complex exp = new Complex(Math.Cos(_inputPhasa), Math.Sin(_inputPhasa));
            Complex[] register = new Complex[countOfCoeffs];
            double[] register1 = new double[countOfCoeffs];
            List<Complex> resultData = new List<Complex>();
            _lisOmega = new List<double>();

            double gamma = 0.9999;
            for (int i = 0; i < realSignal.Count(); i++)
            {
                realSignal[i] *= exp;
                QSignal[i] *= multiply;
            }
            Complex vcoSig = new Complex();
            Complex mulRes = new Complex();
            Complex outputSignal = new Complex();
            double realResultData = 0.0;
            double imagResultData = 0.0;
            double imagPartOfMulRes = 0.0;
            double error = 0.0;
            double exp_d = 1.0;
            double exp_d1 = 1.0;
            double signR = 1.0;
            double signI = 1.0;
            double preambulaMeanR = 0.0;
            double preambulaMeanI = 0.0;
            double _powerI = 0.0;
            double _powerQ = 0.0;

            for (int i = 0; i < realSignal.Count(); i++)
            {

                vcoSig = VCOGenerator(_omega, i, dphi);

                exp_d = gamma * exp_d + (1 - gamma) * realSignal[i].Magnitude;


                exp_d1 = gamma * exp_d1 + (1 - gamma) * Complex.Pow(realSignal[i], 2).Magnitude;

                mulRes = PhaseDetector((Complex.Pow(realSignal[i], 2) / exp_d1), vcoSig);
                if (QSignal.Count() != 0)
                {

                    outputSignal = OutPhaseDetector(realSignal[i].Real / exp_d + Complex.ImaginaryOne * QSignal[i] / exp_d, VCOGenerator(_omega / 2, i, dphi) * Complex.Exp(Complex.ImaginaryOne * Math.PI / 4));
                }
                else
                {
                    outputSignal = OutPhaseDetector(realSignal[i] / exp_d, VCOGenerator(_omega / 2, i, dphi) * Complex.Exp(Complex.ImaginaryOne * Math.PI / 4));
                }
                ShiftRegistr(register, 1);
                register[countOfCoeffs - 1] = outputSignal;
                realResultData = Vector.ScalarProduct(new Vector(ComplexSignals.Real(register.ToList()).ToArray()), 
                    new Vector(coeffs.ToArray()));
                imagResultData = Vector.ScalarProduct(new Vector(ComplexSignals.Imaginary(register.ToList()).ToArray()), 
                    new Vector(coeffs.ToArray()));
                if (i < 12000)
                {
                    preambulaMeanR += realResultData / 12000;
                    preambulaMeanI += imagResultData / 12000;

                }
                if (i == 12000)
                {
                    signI = preambulaMeanI > 0 ? 1 : -1;
                    signR = preambulaMeanR > 0 ? 1 : -1;
                }
                resultData.Add(new Complex(signR * realResultData, signI * imagResultData));
                _powerI += realResultData * realResultData;
                _powerQ += imagResultData * imagResultData;
                imagPartOfMulRes = mulRes.Imaginary;
                ShiftRegistr(register1, 1);
                register1[countOfCoeffs - 1] = imagPartOfMulRes;
                imagPartOfMulRes = Vector.ScalarProduct(new Vector(register1.ToList().ToArray()), 
                    new Vector(coeffs.ToArray()));
                downLoop += _k2 * imagPartOfMulRes;
                error = (imagPartOfMulRes * _k1 + downLoop);
                _omega += (VCOLoop + error);
                _lisOmega.Add(_omega / 2 / 2 / Math.PI);


            }

            _stdOmega = Statistics.CalculateStandardDeviation(_lisOmega.GetRange(12000, 64850));
            _meanOmega = Statistics.CalculateMean(_lisOmega.GetRange(12000, 64850));

            return resultData;

        }



    }
}
