using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DigitalSignalProcessing;

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
        //        k1=.02790479
        //k2=0.0000099
        private double _k1 = .02790479;
        private double _k2 = .0000099;
        private int countOfCoeffs;




        public PLL(double Fs, double omega, double input_phasa, int count_of_coeffs = 101)
        {
            this._Fs = Fs;
            this._inputPhasa = input_phasa;
            while (Math.Log(count_of_coeffs+1, 2) % 1 != 0)
            {
                count_of_coeffs+= 1;
            }
            countOfCoeffs = count_of_coeffs;
            _omega = omega;

        }
        private static void ShiftRegistr<T>(T[] arr, int shifts)
        {
            Array.Copy(arr, shifts, arr, 0, arr.Length - shifts);
            Array.Clear(arr, arr.Length - shifts, shifts);
        }
        private static Complex PhaseDetector(Complex first,Complex second)
        {
            return Complex.Multiply(first, Complex.Conjugate(second));
        }
        private static Complex OutPhaseDetector(Complex first, Complex second)
        {
            return first.Real * second.Real + Complex.ImaginaryOne * first.Imaginary * second.Imaginary;
   
        }

        private Complex VCOGenerator(double omega, int i, double dphi= 0)
        {
            return Complex.Exp(Complex.ImaginaryOne*(omega * i / _Fs + dphi));

        }
        private static List<Complex> getimpulseResponse(int ImpulseResponseLength,double CutoffFrequency,double SamplingFrequency)
        {
            int cutoffFreqIndex = (int)Math.Ceiling(ImpulseResponseLength * Math.Abs(CutoffFrequency) / SamplingFrequency);

            Complex[] H = new Complex[ImpulseResponseLength];

            for (int i = ImpulseResponseLength / 2 - cutoffFreqIndex; i <= ImpulseResponseLength / 2 + cutoffFreqIndex; i++)
            {
                H[i] = 1;// e_iPIover4;
            }
            return DigitalSignalProcessing.Filters.FilterCharacteristic.GetImpulseResponse(H.ToList());

        }
        public List<Complex> pll_from_class_mamedov(List<Complex> realSignal, List<double> QSignal=null)
        {
            var returnSignal = new List<Complex>();
            var dphi = 0.0;
            var downLoop = 0.0;
            var VCOLoop = 0.0;
            double multiply = Math.Abs(Math.Cos(_inputPhasa)) > .2 ? Math.Cos(_inputPhasa) : Math.Sin(_inputPhasa);
            var exp = new Complex(Math.Cos(_inputPhasa), Math.Sin(_inputPhasa));
            var register = new Complex[countOfCoeffs];
            var register1 = new double[countOfCoeffs];
            //var coeffs = new List<double>();
            var resultData = new List<Complex>();
            _lisOmega = new List<double>();
            var coeffs = ComplexSignals.Real(getimpulseResponse(countOfCoeffs+1,(_omega) * 0.03, _Fs).GetRange(1,countOfCoeffs));
            //var cc = new DigitalSignalProcessing.Filters.Nonrecursive.LPF(Math.Abs(_omega) * 0.03,128, _Fs);
            //cc.StartOperation(realSignal);

            //realSignal=ComplexSignals.Shift(realSignal, 0, 76800, _inputPhasa);
            if (350 * 2 * 2 * Math.PI<Math.Abs(_omega) && Math.Abs(_omega) < 400 * 2 * 2 * Math.PI)
            {
                coeffs = new List<double>();
                DataAccess.DataReader.GetSamples("coeffs_in_pll_357.csv", ref coeffs, countOfCoeffs, 0);
            }
            if (550 * 2 * 2 * Math.PI<Math.Abs(_omega) && Math.Abs(_omega) < 650 * 2 * 2 * Math.PI)
            {
                coeffs = new List<double>();
                DataAccess.DataReader.GetSamples("coeffs_in_pll_599.csv", ref coeffs, countOfCoeffs, 0);
            }
            if (440 * 2 * 2 * Math.PI<Math.Abs(_omega) && Math.Abs(_omega) < 500 * 2 * 2 * Math.PI)
            {
                coeffs = new List<double>();
                DataAccess.DataReader.GetSamples("coeffs_in_pll_475.csv", ref coeffs, countOfCoeffs, 0);
            }

            //ReaderAndWriter.getSamples("coeffs_in_pll1.csv", ref coeffs, countOfCoeffs, 0);

            var gamma = 0.9999;
            for (var i = 0; i < realSignal.Count(); i++)
            {
                realSignal[i] *= exp;
                QSignal[i] *= multiply;
            }
            var vcoSig = new Complex();
            var mulRes = new Complex();
            var outputSignal = new Complex();
            var realResultData = 0.0;
            var imagResultData = 0.0;
            var imagPartOfMulRes = 0.0;
            var error = 0.0;
            var exp_d = 1.0;
            var exp_d1 = 1.0;
            var signR = 1.0;
            var signI = 1.0;
            var preambulaMeanR = 0.0;
            var preambulaMeanI = 0.0;
            var _powerI = 0.0;
            var _powerQ = 0.0;
            //_omega = -7533.539183308324;
            //QSignal.Aggregate(x=> x*multiply);
            for (var i = 0; i < realSignal.Count(); i++)
            {

                vcoSig = VCOGenerator(_omega, i, dphi);

                exp_d = gamma * exp_d + (1 - gamma) *realSignal[i].Magnitude;


                exp_d1 = gamma * exp_d1 + (1 - gamma) * Complex.Pow(realSignal[i], 2).Magnitude;//не уверен что здесь именно то что надо

                mulRes = PhaseDetector((Complex.Pow(realSignal[i], 2) / exp_d1), vcoSig);
                if (QSignal.Count()!= 0)
                {

                    outputSignal = OutPhaseDetector(realSignal[i].Real/exp_d + Complex.ImaginaryOne * QSignal[i]/exp_d, VCOGenerator(_omega / 2, i, dphi) * Complex.Exp(Complex.ImaginaryOne*Math.PI / 4));
                }
                else
                {
                    outputSignal = OutPhaseDetector(realSignal[i] / exp_d, VCOGenerator(_omega / 2, i, dphi) * Complex.Exp(Complex.ImaginaryOne * Math.PI / 4));
                }
                ShiftRegistr(register, 1);
                register[countOfCoeffs-1] = outputSignal;//не уверен 
                realResultData = MathAndPhysics.LinearAlgebra.Vector.ScalarProduct(new
                    MathAndPhysics.LinearAlgebra.Vector(ComplexSignals.Real(register.ToList()).ToArray()), new
                    MathAndPhysics.LinearAlgebra.Vector(coeffs.ToArray()));
                imagResultData = MathAndPhysics.LinearAlgebra.Vector.ScalarProduct(new
                    MathAndPhysics.LinearAlgebra.Vector(ComplexSignals.Imaginary(register.ToList()).ToArray()), new
                    MathAndPhysics.LinearAlgebra.Vector(coeffs.ToArray()));
                if (i < 12000)
                {
                    preambulaMeanR += realResultData/12000;
                    preambulaMeanI += imagResultData/12000;

                }
                if (i == 12000)
                {
                    signI = preambulaMeanI > 0 ? 1 : -1;
                    signR = preambulaMeanR > 0 ? 1 : -1;
                }
                resultData.Add(new Complex(signR*realResultData,signI*imagResultData));
                _powerI += realResultData * realResultData;
                _powerQ += imagResultData * imagResultData;
                imagPartOfMulRes = mulRes.Imaginary;
                ShiftRegistr(register1, 1);
                register1[countOfCoeffs-1] = imagPartOfMulRes;//не уверен 
                imagPartOfMulRes = MathAndPhysics.LinearAlgebra.Vector.ScalarProduct(new
                    MathAndPhysics.LinearAlgebra.Vector(register1.ToList().ToArray()), new
                    MathAndPhysics.LinearAlgebra.Vector(coeffs.ToArray()));
                downLoop += _k2 * imagPartOfMulRes;
                error =  (imagPartOfMulRes * _k1 + downLoop);
                _omega += (VCOLoop + error);
                _lisOmega.Add(_omega / 2/2/Math.PI);


            }

            _stdOmega = MathAndPhysics.Statistics.CalculateStandardDeviation(_lisOmega.GetRange(12000,64850));
            _meanOmega = MathAndPhysics.Statistics.CalculateMean(_lisOmega.GetRange(12000, 64850));
            //_powerI=_powerI / realSignal.Count;
            //_powerQ /= realSignal.Count;
            return resultData;

        }



    }
}
