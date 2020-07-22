using System;
using System.Collections.Generic;
using System.Linq;

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
        private static void Farrow(List<double> inputData, ref List<double> resemplingData)
        {
            float r = (float)102300.0 / (float)76800.0;//ratio of frequancies r= f1/f2 f1-which we have f2-which we want

            resemplingData = new List<double>();
            int j = 0;
            for (int k = 2; (int)(k * r) + 2 < inputData.Count() - 1; k++)//k-это новые индексы
            {
                int n = (int)(k * r) + 1;//индекс на который я собираюсь перейти в старой частоте дискритизации
                float r1 = k * r;//текущий индекс
                int r2 = n;//будущий индекс
                int r3 = (n - 1);//предыдущий индекс
                double x1 = Math.Abs(r1 - r2);//-расстояние между текущим и будущим
                float x2 = Math.Abs(r1 - r3);//-расстояние между текущим и предыдущим
                int n1 = x1 < x2 ? n : n - 1;//типо если расстояние ближе к будущему индексу то в n1 ставим будущий индекс иначе -1
                if (x1 < x2) { n1 = n; }

                double res = 0.0;
                float x = k * r - n1;//-расстояние между тем что в н1 и в текущем индексе
                x1 = (n1 - 2) - n1;
                x2 = (n1 - 1) - n1;
                int x3 = (n1 - 0) - n1;
                int x4 = (n1 + 1) - n1;
                int x5 = (n1 + 2) - n1;

                //1
                double a0 = inputData[(n1 - 2)];
                double res1 = a0 * (x - x2) / (-1.0) * (x - x3) / (-2.0) * (x - x4) / (-3.0) * (x - x5) / (-4.0);
                res = res + res1;
                //2
                a0 = inputData[(n1 - 1)];
                res1 = a0 * (x - x1) / (1.0) * (x - x3) / (-1.0) * (x - x4) / (-2.0) * (x - x5) / (-3.0);
                res = res + res1;
                //3
                a0 = inputData[(n1 - 0)];
                res1 = a0 * (x - x1) / (2.0) * (x - x2) / (1.0) * (x - x4) / (-1.0) * (x - x5) / (-2.0);
                res = res + res1;
                //4
                a0 = inputData[(n1 + 1)];
                res1 = a0 * (x - x1) / (3.0) * (x - x2) / (2.0) * (x - x3) / (1.0) * (x - x5) / (-1.0);
                res = res + res1;
                //5
                a0 = inputData[(n1 + 2)];
                res1 = a0 * (x - x1) / (4.0) * (x - x2) / (3.0) * (x - x3) / (2.0) * (x - x4) / (1.0);
                res = res + res1;

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
    }
}
