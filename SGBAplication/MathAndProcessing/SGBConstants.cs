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
        public const int PACKAGE_SMPLES_COUNT = 76800;
        public const int NON_RESEMPLING_SAMPLES_COUNT = 10000000;
        public const int PREAMBULE_SAMPLES_COUNT = 8192;
        public const int PREAM_FREQ_CELLS_COUNT = 10000;
        public const int BPF_IMP_RESP_LENGTH = 127;
        public const int ORIGINAL_COUNT_PACKAGE_SAMPLES = 102300;
        public const int LOW_FREQ_350_Hz = 350;
        public const int LOW_FREQ_550_Hz = 550;
        public const int LOW_FREQ_440_Hz = 440;
        public const int HIGH_FREQ_400_Hz = 400;
        public const int HIGH_FREQ_500_Hz = 500;
        public const int HIGH_FREQ_650_Hz = 650;
        public const int UNMODULETED_SIGNAL_PART = 128 * 32;
        public const int M_SEQUENCE_ELEMENTS_COUNT = 128 * 300;
        public const int NAZAROF_SHIFT = 8;
        public const int LOW_FREQ_0_Hz = 0;
        public const int HIGH_FREQ_1000_Hz = 1000;
        public const double PHAZA_SHIFT = 2 * Math.PI / 21;
        public const int SAMPLES_SHIFT = 50;
        public const int START_BIT_INDEX = 25;
        public const int BIT_COUNT = 150;
        public const int SAMPLES_PER_BIT_COUNT = 512;
        public const double TESTING_FREQUENCY_Hz = 900.2;
    }
}
