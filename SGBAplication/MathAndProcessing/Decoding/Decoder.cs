using MathAndPhysics.NumeralSystems;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace MathAndProcess.Decoding
{
    public class Decoder
    {
        readonly int startBitInd = 25;
        readonly int countBit = 150;
        readonly int countSamplesPerBit = 512;

        public string GetFullMessage(List<Complex> signal)
        {
            string decodeMesage = "";

            //при кодировании сообщения в OQPSK произошло разделение битов на чётные и нечетные,
            //в результате длина битов увеличилась вдвое-> всего 150 бит
            for (int i = startBitInd; i < countBit; i++)
            {
                double r1 = 0.0;//energy odd
                double r2 = 0.0;//energy not odd
                double rc = 0.0;//нечетный знак
                double rs = 0.0;//четный знак
                for (int j = 0; j < 512; j++)//512 длина одного бита в случае передескритизации
                {
                    r1 = r1 + Math.Abs(signal[i * countSamplesPerBit + j].Real);
                    r2 = r2 + Math.Abs(signal[i * countSamplesPerBit + j].Imaginary);
                    rc = rc + signal[i * countSamplesPerBit + j].Real;
                    rs = rs + signal[i * countSamplesPerBit + j].Imaginary;
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

