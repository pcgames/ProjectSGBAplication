using DataAccess;
using DigitalSignalProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MathAndProcessing.Calculations
{
    internal class CoeficientFinder
    {
        readonly int countPackageSamples = 76800;
        readonly int bpfImpRespLength = 127;

        internal List<double> Find(double _omega)
        {
            //int bpfImpRespLength = 127; //TODO: tudu
            CoefficientsReader _coffReader = new CoefficientsReader();
            List<double> coeffs = ComplexSignals.Real(GetImpulseResponse(bpfImpRespLength + 1, (_omega) * 0.03, countPackageSamples).GetRange(1, bpfImpRespLength));

            if (350 * 2 * 2 * Math.PI < Math.Abs(_omega) && Math.Abs(_omega) < 400 * 2 * 2 * Math.PI)
            {
                coeffs = _coffReader.GetCoefficients("coeffs_in_pll_357.csv", bpfImpRespLength);
            }
            if (550 * 2 * 2 * Math.PI < Math.Abs(_omega) && Math.Abs(_omega) < 650 * 2 * 2 * Math.PI)
            {
                coeffs = _coffReader.GetCoefficients("coeffs_in_pll_599.csv", bpfImpRespLength);
            }
            if (440 * 2 * 2 * Math.PI < Math.Abs(_omega) && Math.Abs(_omega) < 500 * 2 * 2 * Math.PI)
            {
                coeffs = _coffReader.GetCoefficients("coeffs_in_pll_475.csv", bpfImpRespLength);
            }
            return coeffs;
        }

        private static List<Complex> GetImpulseResponse(int ImpulseResponseLength, double CutoffFrequency, double SamplingFrequency)
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
