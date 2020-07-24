using MathAndPhysics.NumeralSystems;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace MathAndProcess.Decoding
{
    public class Decoder
    {
        readonly int _startBitInd = 25;
        readonly int _countBit = 150;
        readonly int _countSamplesPerBit = 512;

        public string GetFullMessage(List<Complex> signal)
        {
            string decodeMesage = "";

            //при кодировании сообщения в OQPSK произошло разделение битов на чётные и нечетные,
            //в результате длина битов увеличилась вдвое-> всего 150 бит
            for (int i = _startBitInd; i < _countBit; i++)
            {
                double r1 = 0.0;//energy odd
                double r2 = 0.0;//energy not odd
                double rc = 0.0;//нечетный знак
                double rs = 0.0;//четный знак
                for (int j = 0; j < 512; j++)//512 длина одного бита в случае передескритизации
                {
                    r1 = r1 + Math.Abs(signal[i * _countSamplesPerBit + j].Real);
                    r2 = r2 + Math.Abs(signal[i * _countSamplesPerBit + j].Imaginary);
                    rc = rc + signal[i * _countSamplesPerBit + j].Real;
                    rs = rs + signal[i * _countSamplesPerBit + j].Imaginary;
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

