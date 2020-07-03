using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecoderSGB.Calculations
{
    public static class FourierTransform
    {

        /*__________________ FFT ___________________*/
        public static void fft_f(ref double[] br, ref double[] bi, int n, int p)
        {
            float tr, ti, ur, ui, wr, wi, pil, ar, ai;
            int i, j, k, nat, le, lr, ip;

            nat = 0;
            for (i = n; i > 1; i = i / 2)
                nat++;

            j = 0;
            for (i = 2; i < n; i++)
            {
                lr = n / 2;
                if (i == 8192)
                {

                }
                while (lr <= j&& j>0)
                {
                    j = j - lr;
                    lr = lr / 2;
                }
                j = j + lr;
                if (j > i)
                {
                    tr = (float)br[j];//*(br + j);
                    ti = (float)bi[j];// *(bi + j);
                    br[j] = (float)br[i - 1];// *(br + j) = *(br + i - 1);
                    bi[j] = (float)bi[i - 1];// *(bi + j) = *(bi + i - 1);
                    br[i - 1] = tr;// *(br + i - 1) = tr;
                    bi[i - 1] = ti;// *(bi + i - 1) = ti;
                }
            }

            le = 1;
            ar = (float)1.0; ai = (float)0.0;
            for (i = 0; i < nat; i++)
            {
                lr = le;
                le = le * 2;
                ur = ar; ui = ai;
                pil = (float)Math.PI / lr;
                wr = (float)Math.Cos(pil);
                wi = p * (float)Math.Sin(pil);

                for (j = 0; j < lr; j++)
                {
                    for (k = j; k < n; k = k + le)
                    {
                        if (k % 2 == 0)
                        {

                        }
                        ip = lr + k;
                        tr = ur * (float)br[ip] - ui * (float)bi[ip];
                        ti = ui * (float)br[ip] + ur * (float)bi[ip];
                        br[ip] = (float)br[k] - tr;// *(br + ip) = *(br + k) - tr;
                        bi[ip] = (float)bi[k] - ti;// *(bi + ip) = *(bi + k) - ti;
                        br[k] = (float)br[k] + tr;// *(br + k) = *(br + k) + tr;
                        bi[k] = (float)bi[k] + ti;// *(bi + k) = *(bi + k) + ti;

                    }
                    //FileStream fs = new FileStream("data_before_psp.txt", FileMode.OpenOrCreate);
                    //StreamWriter sw = new StreamWriter(fs);
                    //Action<string> writing = s => sw.WriteLine(s);
                    //for (var w = 0; w < 8192; w++)
                    //{
                    //    writing(br[w].ToString() + '+' + bi[w].ToString() + 'j');
                    //}
                    //sw.Close();
                    //fs.Close();
                    tr = ur * wr - ui * wi;
                    ui = ui * wr + ur * wi;
                    ur = tr;
                }
            }
        } // fft
        private static List<float> generationOfFrequancyWindow()
        {
            List<float> fw = new List<float>();
            var nb1 = 8192;
            FileStream fs = new FileStream("freq_gen.dat", FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);
            

            for (var i = 0; i < 10000; i++)
            {
                int i1;
                var f0 = i / 10000.0/ 2.0;
                var bi = new double[nb1];
                var br = new double[nb1]; 
                for (i1=0; i1 < nb1; i1++)
                {
                    br[i1] = Math.Cos(2.0* Math.PI * f0 / nb1 * i1);
                    bi[i1] = Math.Sin(2.0* Math.PI * f0 / nb1 * i1);
                }//i1

                /*8. FFT */

                fft_f(ref br, ref bi, nb1, -1);

                i1 = 0;
                var r1 =Math.Sqrt(br[i1] * br[i1] + bi[i1] * bi[i1]);
                i1 = 1;
                var r2 = Math.Sqrt(br[i1] * br[i1] + bi[i1] * bi[i1]);
                fw.Add((float)(1000.0* r2 / r1));
                sw.WriteLine(fw.Last().ToString());
                //fprintf(fp," i=%d r1=%f r2=%f 1000.*r2/r1=%f f0=%f\n",i,r1,r2,1000.*r2/r1,f0);
                //	  getch();
                //printf("\n %f",freq[i]);
                //getch();
            }//i
            sw.Close();

            fs.Close();
            return fw;
        }
        public static List<float> getFrequancyWindow()
        {
            List<float> fw;
            try
            {
                fw = new List<float>();
                FileStream fs = new FileStream("freq_gen.dat", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                for(var i=0; i<10000;i++)
                {
                    fw.Add((float)Convert.ToDouble(sr.ReadLine()));
                }
                fs.Close();
                sr.Close();

            }
            catch
            {
                fw = generationOfFrequancyWindow();
            }
            return fw;
        }
    }
}
