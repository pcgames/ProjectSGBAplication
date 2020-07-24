using DataAccess;
using DigitalSignalProcessing;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static MathAndProcessing.SGBConstants;

namespace MathAndProcess.Transformation
{
    /// <summary>
    /// Данный класс является библиотекой методов,
    /// позволяющих производить снятие ПСП
    /// </summary>
    public class Mseqtransform
    {

        public static List<Complex> EmptyPartOfOriginalSignal { get; private set; }
        
        /// <summary>
        /// функция,снимающая псевдослучайную последовательность
        /// </summary>
        /// <param name="MsequanceI">ПСП I канала</param>
        /// <param name="MsequanceQ">ПСП Q канала</param>
        /// <returns>часть сигнала со снятой ПСП</returns>
        private static List<Complex> PseudosequanceMultiplication(List<Complex> signal, List<double> MsequanceI, List<double> MsequanceQ)
        {
            List<Complex> result = new List<Complex>();
            for (int i = 1; i < signal.Count + 1; i++)
            {
                double r1 = signal[i - 1].Real;
                double r2 = signal[i - 1].Imaginary;
                double re = r1 * MsequanceI[i + 1] + r2 * MsequanceQ[i];
                double im = -r1 * MsequanceQ[i] + r2 * MsequanceI[i + 1];
                result.Add(new Complex(re, im));
            }
            return result;


        }

        /// <summary>
        /// функция,снимающая псевдослучайную последовательность
        /// </summary>
        /// <param name="MsequanceI">ПСП I канала</param>
        /// <param name="MsequanceQ">ПСП Q канала</param>
        /// <returns>часть сигнала со снятой ПСП</returns>
        private static List<Complex> PseudosequanceMultiplicationOfFullPackage(List<Complex> signal, List<double> MsequanceI, List<double> MsequanceQ)
        {
            List<Complex> result = new List<Complex>();
            for (int i = 1; i < signal.Count + 1; i++)
            {
                double r1 = signal[i - 1].Real;
                double r2 = signal[i - 1].Imaginary;
                double re = r1 * MsequanceI[i + 1];
                double im = r2 * MsequanceQ[i];
                result.Add(new Complex(re, im));
            }
            return result;
        }

        /// <summary>
        /// Функция которая осуществляет снятие ПСП с нужной части сигнала
        /// </summary>
        /// <param name="ImSamples">действительная часть сигнала</param>
        /// <param name="QSamples">Мнимая часть сигнала</param>
        /// <param name="startIndex">индекс начала</param>
        /// <returns>часть сигнала с которой снимается м последовательность начиная со стартового индекса,длинной 16368 элементов</returns>
        public static List<Complex> GetSamplesOfEmptyPart(List<double> ImSamples, List<double> QSamples, int startIndex = 0)
        {
            List<double> MsequanceI = new List<double>();
            List<double> MsequanceQ = new List<double>();

            EmptyPartOfOriginalSignal = Enumerable.Range(0, _unmodulatedPartOfSignal * 2).Select(i => new Complex(ImSamples[startIndex + i], QSamples[startIndex + i])).ToList();
            ISampleWriter writer = new SampleWriter();
            
            PseudorandomSequence.GetSequensies_2chipsPerBit(_unmodulatedPartOfSignal + 1, out MsequanceI, out MsequanceQ);
            

            return PseudosequanceMultiplication(EmptyPartOfOriginalSignal, MsequanceI, MsequanceQ);
        }

        /// <summary>
        /// Функция которая осуществляет снятие ПСП с нужной части сигнала
        /// </summary>
        /// <param name="signal">Комплексный сигнал</param>
        /// <param name="startIndex">индекс начала</param>
        /// <returns>часть сигнала с которой снимается м последовательность начиная со стартового индекса,длинной 16368 элементов</returns>

        public static List<Complex> GetSamplesOfEmptyPart(List<Complex> signal, int startIndex = 0)
        {
            List<double> MsequanceI = new List<double>();
            List<double> MsequanceQ = new List<double>();

            EmptyPartOfOriginalSignal = Enumerable.Range(0, _unmodulatedPartOfSignal * 2).Select(i => (signal[startIndex + i])).ToList();

            PseudorandomSequence.GetSequensies_2chipsPerBit(_unmodulatedPartOfSignal + 1, out MsequanceI, out MsequanceQ);
            return PseudosequanceMultiplication(EmptyPartOfOriginalSignal, MsequanceI, MsequanceQ);
        }

        /// <summary>
        /// Функция которая осуществляет снятие ПСП с нужной части сигнала
        /// </summary>
        /// <param name="ImSamples">действительная часть сигнала</param>
        /// <param name="QSamples">Мнимая часть сигнала</param>
        /// <param name="startIndex">индекс начала</param>
        /// <returns>часть сигнала с которой снимается м последовательность начиная со стартового индекса,длинной 16368 элементов</returns>
        public static List<Complex> GetSamplesOfFullPackage(List<double> ImSamples, List<double> QSamples)
        {
            List<Complex> Fullpackage = Enumerable.Range(0, MseqElementsCount * 2).Select(i => new Complex(ImSamples[i], QSamples[i])).ToList();
            return GetSamplesOfFullPackage(Fullpackage);
        }

        /// <summary>
        /// Функция которая осуществляет снятие ПСП с нужной части сигнала
        /// </summary>
        /// <param name="signal">Комплексный сигнал</param>
        /// <param name="startIndex">индекс начала</param>
        /// <returns>часть сигнала с которой снимается м последовательность начиная со стартового индекса,длинной 16368 элементов</returns>

        public static List<Complex> GetSamplesOfFullPackage(List<Complex> signal)
        {
            List<double> MsequanceI = new List<double>();
            List<double> MsequanceQ = new List<double>();
            PseudorandomSequence.GetSequensies_2chipsPerBit(MseqElementsCount + 4, out MsequanceI, out MsequanceQ);
            return PseudosequanceMultiplicationOfFullPackage(signal, MsequanceI, MsequanceQ);
        }
    }
}
