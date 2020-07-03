using System.Numerics;
using System.Collections.Generic;
using DigitalSignalProcessing;
using System.Linq;
using System.IO;

namespace MathAndProcess.Transformation
{
    /// <summary>
    /// Данный класс является библиотекой методов,
    /// позволяющих производить снятие ПСП
    /// </summary>
    public class Mseqtransform
    {
        /// <summary>
        /// часть исходного сигнала,в которой нет никакой информации,
        /// с которой будет сниматься псевдо последовательность
        /// </summary>
        public static List<Complex> EmptyPartOfOriginalSignal { get; private set; }
        private static double _startIndex;
        /// <summary>
        /// функция,снимающая псевдослучайную последовательность
        /// </summary>
        /// <param name="MsequanceI">ПСП I канала</param>
        /// <param name="MsequanceQ">ПСП Q канала</param>
        /// <returns>часть сигнала со снятой ПСП</returns>
        private static List<Complex> PseudosequanceMultiplication(List<Complex> signal, List<double> MsequanceI, List<double> MsequanceQ)
        {
            List<Complex> result = new List<Complex>();
            for (var i = 1; i < signal.Count + 1; i++)
            {
                var r1 = signal[i - 1].Real;//ïî÷åìó çäåñü 2*i?????????????
                var r2 = signal[i - 1].Imaginary;
                var re = r1 * MsequanceI[i + 1] + r2 * MsequanceQ[i];//не знаем почему так...
                var im = -r1 * MsequanceQ[i] + r2 * MsequanceI[i + 1];
                result.Add(new Complex(re, im));
            }//i1
            //былвывод result!
            //var rnewData = new DigitalSignalProcessing.Filters.Nonrecursive.LPF(37000, 1000, 76800).
            //            StartOperation(result);
            DataAccess.DataWriter.Writer(signal, "emptyData.txt");
            DataAccess.DataWriter.Writer(result, "empty_data_wo.txt");
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
            for (var i = 1; i < signal.Count + 1; i++)
            {
                var r1 = signal[i - 1].Real;//ïî÷åìó çäåñü 2*i?????????????
                var r2 = signal[i - 1].Imaginary;
                var re = r1 * MsequanceI[i + 1] ;
                var im = r2 * MsequanceQ[i] ;
                result.Add(new Complex(re, im));
            }//i1
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
            var numberOfElements = 128 * 32;//128*32-часть сигнала на не модулированной несущей
            var MsequanceI = new List<double>();
            var MsequanceQ = new List<double>();
            _startIndex = startIndex;
            EmptyPartOfOriginalSignal = Enumerable.Range(0, numberOfElements * 2).Select(i => new Complex(ImSamples[startIndex + i], QSamples[startIndex + i])).ToList();
            DataAccess.DataWriter.Writer(EmptyPartOfOriginalSignal, "emptyData.txt");
            PseudorandomSequence.GetSequensies_2chipsPerBit(numberOfElements+1, out MsequanceI, out MsequanceQ);
            //I.Reverse();
            //Q.Reverse();

            return PseudosequanceMultiplication(EmptyPartOfOriginalSignal,MsequanceI, MsequanceQ);
        }
        /// <summary>
        /// Функция которая осуществляет снятие ПСП с нужной части сигнала
        /// </summary>
        /// <param name="signal">Комплексный сигнал</param>
        /// <param name="startIndex">индекс начала</param>
        /// <returns>часть сигнала с которой снимается м последовательность начиная со стартового индекса,длинной 16368 элементов</returns>

        public static List<Complex> GetSamplesOfEmptyPart(List<Complex> signal, int startIndex = 0)
        {
            var numberOfElements = 128 * 32;//128*32-часть сигнала с немодулированной несущей
            var MsequanceI = new List<double>();
            var MsequanceQ = new List<double>();

            _startIndex = startIndex;
            EmptyPartOfOriginalSignal = Enumerable.Range(0, numberOfElements * 2).Select(i => (signal[startIndex + i])).ToList();

            PseudorandomSequence.GetSequensies_2chipsPerBit(numberOfElements+1, out MsequanceI, out MsequanceQ);
            return PseudosequanceMultiplication(EmptyPartOfOriginalSignal,MsequanceI, MsequanceQ);
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
            var numberOfElements = 128 * 300;//128*150-пполная часть сигнала
            var MsequanceI = new List<double>();
            var MsequanceQ = new List<double>();
            //_startIndex = startIndex;
            var Fullpackage = Enumerable.Range(0, numberOfElements * 2).Select(i => new Complex(ImSamples[i], QSamples[i])).ToList();
            PseudorandomSequence.GetSequensies_2chipsPerBit(numberOfElements+1, out MsequanceI, out MsequanceQ);
            return PseudosequanceMultiplication(Fullpackage, MsequanceI, MsequanceQ);
        }
        /// <summary>
        /// Функция которая осуществляет снятие ПСП с нужной части сигнала
        /// </summary>
        /// <param name="signal">Комплексный сигнал</param>
        /// <param name="startIndex">индекс начала</param>
        /// <returns>часть сигнала с которой снимается м последовательность начиная со стартового индекса,длинной 16368 элементов</returns>

        public static List<Complex> GetSamplesOfFullPackage(List<Complex> signal)
        {
            var numberOfElements = 128 * 300;//128*300-пполная часть сигнала
            var MsequanceI = new List<double>();
            var MsequanceQ = new List<double>();
            PseudorandomSequence.GetSequensies_2chipsPerBit(numberOfElements+4, out MsequanceI, out MsequanceQ);
            return PseudosequanceMultiplicationOfFullPackage(signal,MsequanceI, MsequanceQ);
        }
    }
}
