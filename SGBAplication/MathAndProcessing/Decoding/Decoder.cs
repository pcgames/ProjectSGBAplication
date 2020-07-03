using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using MathAndPhysics;

namespace DecoderSGB.Decoding
{
    class Decoder
    {

        //        for(i1=0;i1<150;i1++)//что такое 150 512???
        //	{
        //   r1=0.;
        //   r2=0.;
        //   rc=0.;
        //   rs=0.;
        //	for(i2=0;i2<512;i2++)
        //	{
        //   r1=r1+(fabs) (br[i1 * 512 + i2]);
        //   r2=r2+(fabs) (bi[i1 * 512 + i2]);
        //   rc=rc+br[i1 * 512 + i2];
        //   rs=rs+bi[i1 * 512 + i2];
        //   }
        //    printf("\n_ %4d %f %f %f", i1, rc, rs, r1+r2);
        //    getch();
        //}//i1
        // вообще не понятно что происзодит в модуле выше...

        //     ng[0]=0;
        //ng[1]=0;
        //ng[2]=0;
        //ng[3]=0;
        //ng[4]=0;
        //i4=5;
        public static string fullMessage(List<Complex> signal)
        {
            var decodeMesage = "";

            //при кодировании сообщения в OQPSK произошло разделение битов на чётные и нечетные,
            //в результате длина битов увеличилась вдвое-> всего 150 бит
            for (var i = 25; i < 150; i++)
            {
                var r1 = 0.0;//energy odd
                var r2 = 0.0;//energy not odd
                var rc = 0.0;//нечетный знак
                var rs = 0.0;//четный знак
                for (var j = 0; j < 512; j++)//512 длина одного бита в случае передескритизации
                {
                    r1 = r1 + Math.Abs(signal[i * 512 + j].Real);
                    r2 = r2 + Math.Abs(signal[i * 512 + j].Imaginary);
                    rc = rc + signal[i * 512 + j].Real;
                    rs = rs + signal[i * 512 + j].Imaginary;
                }//i2


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
                //ng[i4] = i3;
                //i4 = i4 + 1;
                //i3 = 0; if (rs < 0.) { i3 = 1; }
                //ng[i4] = i3;
                //i4 = i4 + 1;
            }//i1
            return decodeMesage;

        }

        public static int decodeCountry(string fullMesage)
        {
            return Convert.ToInt16(MathAndPhysics.NumeralSystems.
                NumeralSystemConverter.
                ConvertNumber(MathAndPhysics.NumeralSystems.NumeralSystem.Binary,
                    MathAndPhysics.NumeralSystems.NumeralSystem.Decimal,
                    fullMesage.Substring(30, 10)));
        }



    }//i
}

//   CodeCountry=ng[44]+ng[43]*2+ng[42]*4+ng[41]*8+ng[40]*16+ng[39]*32+ng[38]*64+ng[37]*128+ng[36]*256+ng[35]*512;
//   Degree_1=ng[55]+ng[54]*2+ng[53]*4+ng[52]*8+ng[51]*16+ng[50]*32+ng[49]*64;
//   Degree_2=ng[79]+ng[78]*2+ng[77]*4+ng[76]*8+ng[75]*16+ng[74]*32+ng[73]*64+ng[72]*128;
//   Degree_1_d=ng[56]* b[0]+ng[57]* b[1]+ng[58]* b[2]+ng[59]* b[3]+ng[60]* b[4]+ng[61]* b[5]+ng[62]* b[6]+ng[63]* b[7]+ng[64]* b[8]+ng[65]* b[9]+ng[66]* b[10]+ng[67]* b[11]+ng[68]* b[12]+ng[69]* b[13]+ng[70]* b[14];
//Degree_2_d=ng[80]* b[0]+ng[81]* b[1]+ng[82]* b[2]+ng[83]* b[3]+ng[84]* b[4]+ng[85]* b[5]+ng[86]* b[6]+ng[87]* b[7]+ng[88]* b[8]+ng[89]* b[9]+ng[90]* b[10]+ng[91]* b[11]+ng[92]* b[12]+ng[93]* b[13]+ng[94]* b[14];
//printf("\n CodeCountry= %d", CodeCountry);
//printf("\n Degree_1= %d %f", Degree_1, Degree_1_d);
//printf("\n Degree_2= %d %f", Degree_2, Degree_2_d);
//getch();


////	  printf("\n___ %4d",i4);
////	  getch();

//	for(i1=0;i1<255;i1++)
//	{
////	  printf("\n___ %4d ng= %4d",i1,ng[i1]);
////	  getch();
//	}//i1
//    }
//}
