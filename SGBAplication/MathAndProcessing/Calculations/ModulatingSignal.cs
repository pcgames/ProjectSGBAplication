using System.Collections.Generic;
using System.Numerics;

namespace MathAndProcess.Calculations
{
    public class ModulatingSignal//данный класс будет переделываться от абстрактного класса генерирования сигналов,необходимый для генерации, так же здесь будут различные типы модуляции сигналов
    {
        public static List<double> generatingBPSKSignal(List<Complex> QPSKSignal, int lengthOfOneBit)
        {

            List<double> result = new List<double>();
            for (int i = 0; i < QPSKSignal.Count / lengthOfOneBit; i++)

                for (int j = 0; j < lengthOfOneBit; j++)
                {
                    if (j > lengthOfOneBit / 2)
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
