using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
//using MathAndPhysics;
using System.Threading.Tasks;
using DigitalSignalProcessing;
using System.IO;
//using 

namespace MathAndProcess.Calculations
{/// <summary>
/// Данный класс является библиотекой методов, основанной на программе назарова
/// </summary>
    public class EvaluationAndCompensation
    {
        private static double _fsempling  = 76800;
        public static List<Complex> Spectrum { get; private set; }
        public static double Phaza { get; private set; }
        public static double AccuracyFreq { get; private set; }
        #region public methods
        /// <summary>
        /// данная функция предназначена для оценки частоты сигнала,написана она Назаровым, 
        /// поэтому не разбираться 
        /// </summary>
        /// <param name="signal">>сигнал на частоте в области ±50 кГц со снятой
        /// псевдослучайной последовательностью </param>
        /// <returns>оцененная частота чигнала</returns>
        public static double EvaluationOfFreq(List<Complex> signal)////Назаров мод
        {


            //Spectrum = FFT.Forward(signal);
            var I = ComplexSignals.Real(signal).ToArray();
            var Q = ComplexSignals.Imaginary(signal).ToArray();
            //I.Reverse();
            //Q.Reverse();

            FourierTransform.fft_f(ref I, ref Q, I.Length, -1);
            Spectrum = ComplexSignals.ToComplex(I.ToList(), Q.ToList());

            
            int maxIndex = FindingMaxIndex(ComplexSignals.Module(Spectrum));

            maxIndex = maxIndex > 0 ? maxIndex : 1;
            double ttt = I[maxIndex - 1] * Q[maxIndex - 1] + I[maxIndex - 1] * Q[maxIndex - 1];
            var r1 = Math.Sqrt(I[maxIndex - 1] * I[maxIndex - 1] + Q[maxIndex - 1] * Q[maxIndex - 1]) / Q.Length;
            var r2 = Math.Sqrt(I[maxIndex - 0] * I[maxIndex - 0] + Q[maxIndex - 0] * Q[maxIndex - 0]) / Q.Length;
            var r3 = Math.Sqrt(I[maxIndex + 1] * I[maxIndex + 1] + Q[maxIndex  +1] * Q[maxIndex + 1]) / Q.Length;
            var r = r1;
            if (r1 < r3) { r = r3; }
            r = 1000.0* r / r2;

            var i3 = 0;
            var freq = FourierTransform.getFrequancyWindow();
            for ( var i1 = 0; i1 < freq.Count-1; i1++)
            {
                if ((r > freq[i1]) && (r <= freq[i1 + 1])) { i3 = i1; }
            }
            if (r1 < r3) { AccuracyFreq = maxIndex * 76800.0/ 8192 + i3 / 10000.0/ 2.0* 76800.0/ 8192; }
            if (r1 > r3) { AccuracyFreq = maxIndex * 76800.0/ 8192 - i3 / 10000.0/ 2.0* 76800.0/ 8192; }

            if (maxIndex > 8192 / 2) { AccuracyFreq -= 76800.0; }

            return AccuracyFreq;
         }

        /// <summary>
        /// данная функция предназначена для оценки фазы сигнала, 
        /// переесенного в нулевую область частот
        /// </summary>
        /// <param name="signal">сигнал на низкой частоте со снятой
        /// псевдослучайной последовательностью </param>
        /// <returns>фаза сигнала на нулевой частоте</returns>
        public static double EvaluationPhaze(List<Complex> signal)//Назаров мод
        {
            var newSignal = NazarovCompensationOfFreq(signal);

            var I = ComplexSignals.Real(newSignal).ToArray();
            var Q = ComplexSignals.Imaginary(newSignal).ToArray();

            FourierTransform.fft_f(ref I, ref Q, I.Length, -1);
            var newSpectrum = ComplexSignals.ToComplex(I.ToList(), Q.ToList());

                
                Phaza = Math.Atan2(newSpectrum[FindingMaxIndex(ComplexSignals.Module(newSpectrum))].Imaginary,
                newSpectrum[FindingMaxIndex(ComplexSignals.Module(newSpectrum))].Real) + Math.PI / 4.0;//на вопрос почему пи/4 ответ один, назаров МОД
            var p = Math.Atan2(newSpectrum[FindingMaxIndex(ComplexSignals.Module(newSpectrum))].Imaginary,
            newSpectrum[FindingMaxIndex(ComplexSignals.Module(newSpectrum))].Real);
            //}
            var d = newSpectrum[FindingMaxIndex(ComplexSignals.Module(newSpectrum))];
            return Phaza;
        }
        /// <summary>
        /// функция компенсаци частоты и фазы сигнала с которого не снята
        /// псевдопоследовательность после произведенной оценки частоты и фазы
        /// </summary>
        /// <param name="signal">исходный передескритизованный сигнал</param>
        /// <returns>сигнал(список комплексных отсчетов) с компенсированной частотой и фазой</returns>
        public static List<Complex> CompensationOfPhazeAndFrequancy(List<Complex> signal)
        {
            List<Complex> resultSignal;
            if (AccuracyFreq.Equals(null) != true)
            {
                //DataAccess.DataWriter.Writer(signal, "orig_signal.txt");
                resultSignal = NazarovCompensationOfFreq(signal);
                //DataAccess.DataWriter.Writer(resultSignal, "data_without_freq.txt");

                if (Phaza.Equals(null) != true)
                {
                    resultSignal = NazarovCompensationoOfPhaze(resultSignal);
                    //ReaderAndWriter.Writer(resultSignal, "data_without_freq_faza.txt");

                    return resultSignal;

                }
                else throw new Exception("не произведена оценка фазы");
            }
            else throw new Exception("не произведена оценка частоты");
        }

        /// <summary>
        /// функция которая делает предобработку и сразу оценивает 
        /// частоту и фазу,которую оставляет в классе
        /// </summary>
        /// <param name="signal">передискретизованный сигнал со снятой псевдопоследовательностью</param>
        public static void PreprocessingOfSignal(List<Complex> signal)
        {
            AccuracyFreq = EvaluationOfFreq(signal);
            Phaza = EvaluationPhaze(signal);
        }

        #endregion
        #region private methods

        private static int FindingMaxIndex(List<double> moduleSpectrum)
        {
            var m = moduleSpectrum.Max()/moduleSpectrum.Count();
            var b=20*Math.Log10((moduleSpectrum.Sum()/ moduleSpectrum.Count()-m)/ moduleSpectrum.Count());
            int k = moduleSpectrum.IndexOf(moduleSpectrum.Max());
            return k;
        }


        /// <summary>
        /// назаровская реалиация компенсации фазы 
        /// и переноса его на данную частоту
        /// </summary>
        /// <param name="signal"> сигнал со снятой 
        /// псевдослучайной последовательностью </param>
        /// <returns>сигнал(список комплексных отсчетотв выведенных)
        /// с компенсированной частотой(перенесенный в окресность нуля)</returns>
        private static List<Complex> NazarovCompensationOfFreq(List<Complex> signal)
        {

            if (Spectrum.Equals(null))
            {
                Spectrum = FFT.Forward(signal);
            }

            //var maxIndex = FindingMaxIndex(ComplexSignals.Module(spectrum));
            if (AccuracyFreq.Equals(null))
            {
                AccuracyFreq = EvaluationOfFreq(Spectrum);
            }

            return ComplexSignals.Shift(signal, -AccuracyFreq, _fsempling);//<-здесь изменение
         }

        /// <summary>
        /// назаровская реалиация компенсации фазы 
        /// и переноса его на данную частоту
        /// </summary>
        /// <param name="signal"> сигнал со снятой 
        /// псевдослучайной последовательностью </param>
        /// <returns>сигнал(список комплексных отсчетотв выведенных)
        /// с компенсированной частотой(перенесенный в окресность нуля)</returns>
        //private static List<Complex> CompensationOfFreq(List<Complex> signal) 
        //{
        //    _spectrum = FFT.Forward(signal);
        //    //var maxIndex = FindingMaxIndex(ComplexSignals.Module(spectrum));
        //    _accuracyFreq = EvaluationOfFreq(_spectrum);
        //    return NazarovCompensationOfFreq(signal);

        //}

       
        /// <summary>
        /// назаровская реалиация компенсации фазы
        /// </summary>
        /// <param name="signal">Сигнал с нулевой частотой 
        /// перенесенной в область нулевых частот</param>
        /// <returns>сигнал(список комплексных отсчетотв выведенных)
        /// с компенсированной фазой</returns>
        private static List<Complex> NazarovCompensationoOfPhaze(List<Complex> signal)
        {
            return ComplexSignals.Shift(signal, 0, _fsempling,-Phaza);
        }
        #endregion

    }
}
