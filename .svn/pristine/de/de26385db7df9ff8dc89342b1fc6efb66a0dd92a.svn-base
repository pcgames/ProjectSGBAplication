using System.Collections.Generic;
using System.Linq;

namespace MathAndProcess.Calculations
{
    public class FreqCalculation
    {
        /// <summary>
        ///данная функция возвращает набор частот от - freqSempling /2 до +freqSempling/2
        /// </summary>
        /// <param name="countOfElements">количество элементов</param>
        /// <param name="freqSempling"> частота дискретизации</param>
        /// <returns></returns>
        public static List<double> Getfrequancy(int countOfElements, double freqSempling)
        {
            return Enumerable.Range(0, countOfElements).Select(element => -freqSempling / 2.0 + element * freqSempling / countOfElements).ToList();

        }
    }
}