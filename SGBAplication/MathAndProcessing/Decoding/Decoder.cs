using MathAndPhysics.NumeralSystems;
using System;
using System.Collections.Generic;
using System.Numerics;
using static MathAndProcessing.SGBConstants;

namespace MathAndProcess.Decoding
{
    public class Decoder
    {
        public string GetFullMessage(List<Complex> signal)
        {
            string decodeMesage = "";

            //при кодировании сообщения в OQPSK произошло разделение битов на чётные и нечетные,
            //в результате длина битов увеличилась вдвое-> всего 150 бит
            for (int i = START_BIT_INDEX; i < BIT_COUNT; i++)
            {
                double r1 = 0.0;//energy odd
                double r2 = 0.0;//energy not odd
                double rc = 0.0;//нечетный знак
                double rs = 0.0;//четный знак
                for (int j = 0; j < 512; j++)//512 длина одного бита в случае передескритизации
                {
                    r1 = r1 + Math.Abs(signal[i * SAMPLES_PER_BIT_COUNT + j].Real);
                    r2 = r2 + Math.Abs(signal[i * SAMPLES_PER_BIT_COUNT + j].Imaginary);
                    rc = rc + signal[i * SAMPLES_PER_BIT_COUNT + j].Real;
                    rs = rs + signal[i * SAMPLES_PER_BIT_COUNT + j].Imaginary;
                }


                if (rc < 0.0)
                {
                    decodeMesage += "1";
                }
                else
                {
                    decodeMesage += "0";
                }
                if (rs < 0.0)
                {
                    decodeMesage += "1";
                }
                else
                {
                    decodeMesage += "0";
                }
            }
            return decodeMesage;

        }

        public static int DecodeCountryCode(string fullMesage)
        {
            return Convert.ToInt16(NumeralSystemConverter.ConvertNumber(NumeralSystem.Binary, NumeralSystem.Decimal, 
                                                                        fullMesage.Substring(30, 10)));
        }
    }
}

