using DataAccess;
using DigitalSignalProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MathAndProcessing.Calculations
{
    internal static class CoeficientFinder
    {
        internal static List<double> Find(double _omega)
        {
             int countOfCoeffs = 127; //TODO: tudu
            CoefficientsReader _coffReader = new CoefficientsReader();
            List<double> coeffs = ComplexSignals.Real(GetimpulseResponse(countOfCoeffs + 1, (_omega) * 0.03, 76800).GetRange(1, countOfCoeffs));

            if (350 * 2 * 2 * Math.PI < Math.Abs(_omega) && Math.Abs(_omega) < 400 * 2 * 2 * Math.PI)
            {
                coeffs = _coffReader.GetCoefficients("coeffs_in_pll_357.csv", countOfCoeffs);
            }
            if (550 * 2 * 2 * Math.PI < Math.Abs(_omega) && Math.Abs(_omega) < 650 * 2 * 2 * Math.PI)
            {
                coeffs = _coffReader.GetCoefficients("coeffs_in_pll_599.csv", countOfCoeffs);
            }
            if (440 * 2 * 2 * Math.PI < Math.Abs(_omega) && Math.Abs(_omega) < 500 * 2 * 2 * Math.PI)
            {
                coeffs = _coffReader.GetCoefficients("coeffs_in_pll_475.csv", countOfCoeffs);
            }
            return coeffs;
        }

        private static List<Complex> GetimpulseResponse(int ImpulseResponseLength, double CutoffFrequency, double SamplingFrequency)
        {
            int cutoffFreqIndex = (int)Math.Ceiling(ImpulseResponseLength * Math.Abs(CutoffFrequency) / SamplingFrequency);

            Complex[] H = new Complex[ImpulseResponseLength];

            for (int i = ImpulseResponseLength / 2 - cutoffFreqIndex; i <= ImpulseResponseLength / 2 + cutoffFreqIndex; i++)
            {
                H[i] = 1;
            }
            return DigitalSignalProcessing.Filters.FilterCharacteristic.GetImpulseResponse(H.ToList());

        }
    }
}
