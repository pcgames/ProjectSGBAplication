using DataAccess;
using DigitalSignalProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static MathAndProcessing.SGBConstants;

namespace MathAndProcessing.Calculations
{
    internal class CoeficientFinder
    {
        internal List<double> Find(double _omega)
        {
            CoefficientsReader _coffReader = new CoefficientsReader();
            List<double> coeffs = ComplexSignals.Real(GetImpulseResponse(BPF_IMP_RESP_LENGTH + 1, (_omega) * 0.03, PACKAGE_SMPLES_COUNT).GetRange(1, BPF_IMP_RESP_LENGTH));

            if (LOW_FREQ_350_Hz * 2 * 2 * Math.PI < Math.Abs(_omega) && Math.Abs(_omega) < HIGH_FREQ_400_Hz * 2 * 2 * Math.PI)
            {
                coeffs = _coffReader.GetCoefficients("coeffs_in_pll_357.csv", BPF_IMP_RESP_LENGTH);
            }
            if (LOW_FREQ_550_Hz * 2 * 2 * Math.PI < Math.Abs(_omega) && Math.Abs(_omega) < HIGH_FREQ_650_Hz * 2 * 2 * Math.PI)
            {
                coeffs = _coffReader.GetCoefficients("coeffs_in_pll_599.csv", BPF_IMP_RESP_LENGTH);
            }
            if (LOW_FREQ_440_Hz * 2 * 2 * Math.PI < Math.Abs(_omega) && Math.Abs(_omega) < HIGH_FREQ_500_Hz * 2 * 2 * Math.PI)
            {
                coeffs = _coffReader.GetCoefficients("coeffs_in_pll_475.csv", BPF_IMP_RESP_LENGTH);
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
