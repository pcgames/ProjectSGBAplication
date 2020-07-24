using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MathAndProcessing.SGBConstants;

namespace MathAndProcessing
{
    public static class SGBConstants
    {
        public const int countPackageSamples = 76800;
        public const int numOfNonResemplingSamples = 10000000;
        public const int countPreambuleSamples = 8192;
        public const int countPreamFreqCells = 10000;
        public const int bpfImpRespLength = 127;
        public const int ORIGINAL_COUNT_PACKAGE_SAMPLES = 102300;
        public const int _lowFreq350_Hz = 350;
        public const int _lowFreq550_Hz = 550;
        public const int _lowFreq440_Hz = 440;
        public const int _highFreq400_Hz = 400;
        public const int _highFreq500_Hz = 500;
        public const int _highFreq650_Hz = 650;
        public const int _unmodulatedPartOfSignal = 128 * 32;
        public const int MseqElementsCount = 128 * 300;
        public const int NazarovShift = 8;
        public const int lowFreq0_Hz = 0;
        public const int highFreq1000_Hz = 1000;
        public const double phasaShift = 2 * Math.PI / 21;
        public const int SamplesShift = 50;
        public const int startBitInd = 25;
        public const int countBit = 150;
        public const int countSamplesPerBit = 512;
        public const double origFreq_Hz = 900.2;




    }
}
