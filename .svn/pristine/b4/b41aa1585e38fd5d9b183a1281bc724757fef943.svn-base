using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MathAndProcess.Calculations
{
    public class ModulatingSignal//данный класс будет переделываться от абстрактного класса генерирования сигналов,необходимый для генерации, так же здесь будут различные типы модуляции сигналов
    {
        public static List<double> generatingBPSKSignal(List<Complex> QPSKSignal,int lengthOfOneBit)
        {

            var result = new List<double>();
            for (var i =0;i<QPSKSignal.Count/lengthOfOneBit;i++)
            
                for (var j = 0; j < lengthOfOneBit; j++)
                {
                    if (j > lengthOfOneBit/2)
                    {
                        result.Add(QPSKSignal[i * lengthOfOneBit + j].Imaginary);
                    }
                    else
                    {
                        result.Add(QPSKSignal[i * lengthOfOneBit + j].Real);
                    }

                }
            return result;

        }
    }
}
