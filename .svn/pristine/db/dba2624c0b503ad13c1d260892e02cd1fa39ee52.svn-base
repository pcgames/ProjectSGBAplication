using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAndProcess.Calculations
{
    public class ResemplingOfSignal
    {
        /// <summary>
        /// Данная функция является передискретизирующим фильтром, 
        /// полностью скопированная с функции назарова
        /// (код мясо, разбираться не надо, примите это как данность ATENTION! Dangerowus for your Life!
        /// please, don't do it!!!)
        /// </summary>
        /// <param name="inputData">исходный сигнал с приемника</param>
        /// <param name="resemplingData">массив созданный под передискретизованный сигнал</param>
        private static void Farrow(List<double> inputData,ref List<double> resemplingData)
        {
            float r=(float)102300.0/ (float)76800.0;//ratio of frequancies r= f1/f2 f1-which we have f2-which we want

            resemplingData = new List<double>();
            var j = 0;
            for(var k=2; (int)(k * r) + 2 < inputData.Count()-1;k++)//k-это новые индексы
            {
                  var n=(int)(k*r)+1;//индекс на который я собираюсь перейти в старой частоте дискритизации
                  float r1=k*r;//текущий индекс
                  var r2=n;//будущий индекс
                  var r3=(n-1);//предыдущий индекс
                  double x1=Math.Abs(r1-r2);//-расстояние между текущим и будущим
                  var x2=Math.Abs(r1-r3);//-расстояние между текущим и предыдущим
                  var n1= x1 < x2? n:n - 1;//типо если расстояние ближе к будущему индексу то в n1 ставим будущий индекс иначе -1
                  if(x1<x2){n1=n;}

                  var res=0.0;
                  var x=k*r-n1;//-расстояние между тем что в н1 и в текущем индексе
                  x1=(n1-2)-n1;
                  x2=(n1-1)-n1;
                  var x3=(n1-0)-n1;
                  var x4=(n1+1)-n1;
                  var x5=(n1+2)-n1;

                    //1
                  var a0=inputData[(n1-2)];
                  var res1=a0*(x-x2)/(-1.0)*(x-x3)/(-2.0)*(x-x4)/(-3.0)*(x-x5)/(-4.0);
                  res=res+res1;
                    //2
                  a0=inputData[(n1-1)];
                  res1=a0*(x-x1)/(1.0)*(x-x3)/(-1.0)*(x-x4)/(-2.0)*(x-x5)/(-3.0);
                  res=res+res1;
                    //3
                  a0=inputData[(n1-0)];
                  res1=a0*(x-x1)/(2.0)*(x-x2)/(1.0)*(x-x4)/(-1.0)*(x-x5)/(-2.0);
                  res=res+res1;
                    //4
                  a0=inputData[(n1+1)];
                  res1=a0*(x-x1)/(3.0)*(x-x2)/(2.0)*(x-x3)/(1.0)*(x-x5)/(-1.0);
                  res=res+res1;
                    //5
                  a0=inputData[(n1+2)];
                  res1=a0*(x-x1)/(4.0)*(x-x2)/(3.0)*(x-x3)/(2.0)*(x-x4)/(1.0);
                  res=res+res1;

                  resemplingData.Add(res);
                j += 1;
                  }//i1

            

        }
        /// <summary>
        /// данная функция осуществляет передискретизацию вхолдного сигнала
        /// </summary>
        /// <param name="inputData">взодной сигнал</param>
        /// <returns>передискретизованный сигнал</returns>
        public static List<double> GetResemplingSamples(List<double> inputData)
        {
            List<double> resemplingData = new List<double>();
            Farrow(inputData, ref resemplingData);
            return resemplingData;
        }



        private static void MyFarrow(List<double> inputData, ref List<double> resemplingData)
        {
            double p = 153600;
            double q = 102300;
            resemplingData = resampling_lagrange1(inputData,  p,  q, 0);
        }
        private static List<double> resampling_lagrange(List<double> inputData, double p, double q, double x0)
        {
            //    """
            //    % y = resample_lagrange(s, p, q, x0)
            //    % Digital resampling by polynomial Lagrange interpolation.
            //    % Function changes input signal s samplerate to p / q times and adds fractional
            //    % delay.
            //    %
            //    % Input parameters
            //    % s - input signal vector[N x 1];
            //    % p - p paramter of samplarate conversion
            //    % q - q paramter of samplarate conversion
            //    % x0 - fractional delay
            //  %

            //  % Ouptut parameters
            //  % y - Resampled signal
            //%

            //% Author: Sergey Bakhurin(dsplib.org)
            //    """
            var ind = 0;
            if (p >= 1)
                if (q == 1)
                {
                    ind = (int)(((inputData.Count - 1) * p) / (q)) + 1;
                }
                else
                {
                    ind = (int)(((inputData.Count) * p) / (q));

                }
            else
            {
                ind = (int)(((inputData.Count) * p) / (q));

            }






            inputData.Add(0);
            inputData.Add(0);
            var resample_data = new List<double>();
            var t = new List<double>();
            for (var k = 0; k < ind - 1; k++)
            {
                var x = k * q / p - x0;
                t.Add(x);
                var n = (int)(Math.Floor(x)) + 4;
                var d = Math.Floor(x) + 1 - x;
                var a0 = inputData[n - 1];
                var a3 = 1 / 6 * (inputData[n] - inputData[n - 3]) + 0.5 * (inputData[n - 2] - inputData[n - 1]);

                var a1 = 0.5 * (inputData[n] - inputData[n - 2]) - a3;
                var a2 = inputData[n] - inputData[n - 1] - a3 - a1;
                //var b = Math.Pow(d,2);
                resample_data.Add(a0 - a1 * d + a2 * Math.Pow(d, 2) - a3 * Math.Pow(d, 3));
            }

            return resample_data;
        }
        private static List<double> resampling_lagrange1(List<double> inputData, double p, double q, double x0)
        {
            //    """
            //    % y = resample_lagrange(s, p, q, x0)
            //    % Digital resampling by polynomial Lagrange interpolation.
            //    % Function changes input signal s samplerate to p / q times and adds fractional
            //    % delay.
            //    %
            //    % Input parameters
            //    % s - input signal vector[N x 1];
            //    % p - p paramter of samplarate conversion
            //    % q - q paramter of samplarate conversion
            //    % x0 - fractional delay
            //  %

            //  % Ouptut parameters
            //  % y - Resampled signal
            //%

            //% Author: Sergey Bakhurin(dsplib.org)
            //    """
            var ind = 0;
            if (p >= 1)
                if (q == 1)
                {
                    ind = (int)(((inputData.Count - 1) * p) / (q)) + 1;
                }
                else
                {
                    ind = (int)((inputData.Count) *(p / q));

                    //y = np.zeros(int(float(len(inputData) * p) / float(q)))
                }
            else
            {
                ind = (int)(((inputData.Count) * p) / (q));
                //y = np.zeros(int(float(len(inputData) * p) / float(q)))

            }






            //t = np.zeros(len(y))
            //for 
            inputData.Insert(0, 0);
            //inputData.Insert(0, 0);
            inputData.Add(0);
            inputData.Insert(0, 0);
            inputData.Add(0);
            inputData.Add(0);
            //inputData = np.concatenate((np.array([0., 0.]), inputData, np.array([0., 0.])))
            var resample_data = new List<double>();
            var t = new List<double>();
            for (var k = 0; k < ind - 1; k++)
            {
                var x = k * q / p - x0;
                t.Add(x);
                var n = (int)(Math.Floor(x)) + 5;
                var d = Math.Floor(x) + 1 - x;
                var a0 = inputData[n - 1];
                //var a3 = 1 / 6 * (inputData[n] - inputData[n - 3]) + 0.5 * (inputData[n - 2] - inputData[n - 1]);

                //var a1 = 0.5 * (inputData[n] - inputData[n - 2]) - a3;
                //var a2 = inputData[n] - inputData[n - 1] - a3 - a1;
                var a1 = -1.0 / 12 * inputData[n - 4] + 0.5 * inputData[n - 3] - 3.0 / 2 * inputData[n - 2] + 5.0 / 6 * inputData[n - 1] + 1.0 / 4 * inputData[n];
                var a2 = -1.0 / 24 * inputData[n - 4] + 1.0/6 * inputData[n - 3] + 1.0 / 4 * inputData[n - 2] - 5.0 / 6 * inputData[n - 1] + 11.0 /24 * inputData[n];
                var a3 = 1.0 / 12 * inputData[n - 4] - 0.5 * inputData[n - 3] + 1 * inputData[n - 2] - 5.0 / 6 * inputData[n - 1] + 1.0 / 4 * inputData[n];
                var a4 = 1.0 / 24 * inputData[n - 4] - 1.0/6 * inputData[n - 3] + 1 / 4.0 * inputData[n - 2] - 1.0 / 6 * inputData[n - 1] + 1.0 / 24 * inputData[n];
                //var b = Math.Pow(d,2);
                resample_data.Add(a0 - a1 * d + a2 * Math.Pow(d, 2) - a3 * Math.Pow(d, 3) + a4 * Math.Pow(d, 4));
            }

            return resample_data;
        }

    }


   
}
