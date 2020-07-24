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
            List<double> coeffs = ComplexSignals.Real(GetImpulseResponse(bpfImpRespLength + 1, (_omega) * 0.03, countPackageSamples).GetRange(1, bpfImpRespLength));

            if (_lowFreq350_Hz * 2 * 2 * Math.PI < Math.Abs(_omega) && Math.Abs(_omega) < _highFreq400_Hz * 2 * 2 * Math.PI)
            {
                coeffs = _coffReader.GetCoefficients("coeffs_in_pll_357.csv", bpfImpRespLength);
            }
            if (_lowFreq550_Hz * 2 * 2 * Math.PI < Math.Abs(_omega) && Math.Abs(_omega) < _highFreq650_Hz * 2 * 2 * Math.PI)
            {
                coeffs = _coffReader.GetCoefficients("coeffs_in_pll_599.csv", bpfImpRespLength);
            }
            if (_lowFreq440_Hz * 2 * 2 * Math.PI < Math.Abs(_omega) && Math.Abs(_omega) < _highFreq500_Hz * 2 * 2 * Math.PI)
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
