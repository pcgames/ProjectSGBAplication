using System;
using System.Collections.Generic;
using System.Numerics;

namespace MathAndProcess.Decoding
{
    public class Decoder
    {

        public static string GetFullMessage(List<Complex> signal)
        {
            string decodeMesage = "";

            //при кодировании сообщения в OQPSK произошло разделение битов на чётные и нечетные,
            //в результате длина битов увеличилась вдвое-> всего 150 бит
            for (int i = 25; i < 150; i++)
            {
                double r1 = 0.0;//energy odd
                double r2 = 0.0;//energy not odd
                double rc = 0.0;//нечетный знак
                double rs = 0.0;//четный знак
                for (int j = 0; j < 512; j++)//512 длина одного бита в случае передескритизации
                {
                    r1 = r1 + Math.Abs(signal[i * 512 + j].Real);
                    r2 = r2 + Math.Abs(signal[i * 512 + j].Imaginary);
                    rc = rc + signal[i * 512 + j].Real;
                    rs = rs + signal[i * 512 + j].Imaginary;
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
            return Convert.ToInt16(MathAndPhysics.NumeralSystems.
                NumeralSystemConverter.
                ConvertNumber(MathAndPhysics.NumeralSystems.NumeralSystem.Binary,
                    MathAndPhysics.NumeralSystems.NumeralSystem.Decimal,
                    fullMesage.Substring(30, 10)));
        }
    }
}

