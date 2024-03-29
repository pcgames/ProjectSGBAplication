﻿using DigitalSignalProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MathAndProcess.Calculations
{/// <summary>
/// Данный класс является библиотекой методов, основанной на программе назарова
/// </summary>
    public class EvaluationAndCompensation
    {
        readonly int countPackageSamples = 76800;
        readonly int countPreambuleSamples = 8192;
        readonly int countPreamFreqCells = 10000;


        private double _fsempling = 76800;
        public static List<Complex> Spectrum { get; private set; }
        public static double Phaza { get; private set; }
        public static double AccuracyFreq { get; private set; }
        public EvaluationAndCompensation()
        {

        }
        #region public methods
        /// <summary>
        /// данная функция предназначена для оценки частоты сигнала,написана она Назаровым, 
        /// поэтому не разбираться 
        /// </summary>
        /// <param name="signal">>сигнал на частоте в области ±50 кГц со снятой
        /// псевдослучайной последовательностью </param>
        /// <returns>оцененная частота чигнала</returns>
        public double EvaluationOfFreq(List<Complex> signal)////Назаров мод
        {


            //Spectrum = FFT.Forward(signal);
            double[] I = ComplexSignals.Real(signal).ToArray();
            double[] Q = ComplexSignals.Imaginary(signal).ToArray();
            //I.Reverse();
            //Q.Reverse();

            FourierTransform.Fft_f(ref I, ref Q, I.Length, -1);
            Spectrum = ComplexSignals.ToComplex(I.ToList(), Q.ToList());


            int maxIndex = FindingMaxIndex(ComplexSignals.Module(Spectrum));

            maxIndex = maxIndex > 0 ? maxIndex : 1;
            double ttt = I[maxIndex - 1] * Q[maxIndex - 1] + I[maxIndex - 1] * Q[maxIndex - 1];
            double r1 = Math.Sqrt(I[maxIndex - 1] * I[maxIndex - 1] + Q[maxIndex - 1] * Q[maxIndex - 1]) / Q.Length;
            double r2 = Math.Sqrt(I[maxIndex - 0] * I[maxIndex - 0] + Q[maxIndex - 0] * Q[maxIndex - 0]) / Q.Length;
            double r3 = Math.Sqrt(I[maxIndex + 1] * I[maxIndex + 1] + Q[maxIndex + 1] * Q[maxIndex + 1]) / Q.Length;
            double r = r1;
            if (r1 < r3) { r = r3; }
            r = 1000.0 * r / r2;

            int i3 = 0;
            List<float> freq = FourierTransform.GetFrequancyWindow();
            for (int i1 = 0; i1 < freq.Count - 1; i1++)
            {
                if ((r > freq[i1]) && (r <= freq[i1 + 1])) { i3 = i1; }
            }
            if (r1 < r3) { AccuracyFreq = maxIndex * (double)countPackageSamples / (double)countPreambuleSamples + i3 / (double)countPreamFreqCells / 2.0 * (double)countPackageSamples / (double)countPreambuleSamples; }
            if (r1 > r3) { AccuracyFreq = maxIndex * (double)countPackageSamples / (double)countPreambuleSamples - i3 / (double)countPreamFreqCells / 2.0 * (double)countPackageSamples / (double)countPreambuleSamples; }

            if (maxIndex > countPreambuleSamples / 2) { AccuracyFreq -= countPackageSamples; }

            return AccuracyFreq;
        }

        /// <summary>
        /// данная функция предназначена для оценки фазы сигнала, 
        /// переесенного в нулевую область частот
        /// </summary>
        /// <param name="signal">сигнал на низкой частоте со снятой
        /// псевдослучайной последовательностью </param>
        /// <returns>фаза сигнала на нулевой частоте</returns>
        public double EvaluationPhaze(List<Complex> signal)//Назаров мод
        {
            List<Complex> newSignal = NazarovCompensationOfFreq(signal);

            double[] I = ComplexSignals.Real(newSignal).ToArray();
            double[] Q = ComplexSignals.Imaginary(newSignal).ToArray();

            FourierTransform.Fft_f(ref I, ref Q, I.Length, -1);
            List<Complex> newSpectrum = ComplexSignals.ToComplex(I.ToList(), Q.ToList());


            Phaza = Math.Atan2(newSpectrum[FindingMaxIndex(ComplexSignals.Module(newSpectrum))].Imaginary,
            newSpectrum[FindingMaxIndex(ComplexSignals.Module(newSpectrum))].Real) + Math.PI / 4.0;//на вопрос почему пи/4 ответ один, назаров МОД
            double p = Math.Atan2(newSpectrum[FindingMaxIndex(ComplexSignals.Module(newSpectrum))].Imaginary,
            newSpectrum[FindingMaxIndex(ComplexSignals.Module(newSpectrum))].Real);
            //}
            Complex d = newSpectrum[FindingMaxIndex(ComplexSignals.Module(newSpectrum))];
            return Phaza;
        }
        /// <summary>
        /// функция компенсаци частоты и фазы сигнала с которого не снята
        /// псевдопоследовательность после произведенной оценки частоты и фазы
        /// </summary>
        /// <param name="signal">исходный передескритизованный сигнал</param>
        /// <returns>сигнал(список комплексных отсчетов) с компенсированной частотой и фазой</returns>
        public List<Complex> CompensationOfPhazeAndFrequancy(List<Complex> signal)
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
        public void PreprocessOfSignal(List<Complex> signal)
        {
            AccuracyFreq = EvaluationOfFreq(signal);
            Phaza = EvaluationPhaze(signal);
        }

        #endregion
        #region private methods

        private static int FindingMaxIndex(List<double> moduleSpectrum)
        {
            double m = moduleSpectrum.Max() / moduleSpectrum.Count();
            double b = 20 * Math.Log10((moduleSpectrum.Sum() / moduleSpectrum.Count() - m) / moduleSpectrum.Count());
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
        private List<Complex> NazarovCompensationOfFreq(List<Complex> signal)
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
        private  List<Complex> NazarovCompensationoOfPhaze(List<Complex> signal)
        {
            return ComplexSignals.Shift(signal, 0, _fsempling, -Phaza);
        }
        #endregion

    }
}
