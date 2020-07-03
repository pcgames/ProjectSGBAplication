using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MathAndPhysics;
using System.Threading.Tasks;
using DigitalSignalProcessing;
using System.IO;
//using 

namespace DecoderSGB.Calculations
{/// <summary>
/// Данный класс является библиотекой методов, основанной на программе назарова
/// </summary>
    class EvaluationAndCompensation
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
        //public static double EvaluationOfFreq(List<Complex> signal)////Назаров мод
        //{
        //    if (Spectrum == null)
        //    {
        //        //Spectrum = FFT.Forward(signal);
        //        var I = ComplexSignals.Real(signal).ToArray();
        //        var Q = ComplexSignals.Imaginary(signal).ToArray();
        //        FourierTransform.fft_f(I,Q,I.Length,1);
        //        Spectrum = ComplexSignals.ToComplex(I.ToList(), Q.ToList());
        //    }
        //    int maxIndex = FindingMaxIndex(ComplexSignals.Module(Spectrum));
        //    var r1 = Spectrum[maxIndex - 1].Magnitude / Spectrum.Count();
        //    var r2 = Spectrum[maxIndex].Magnitude / Spectrum.Count();
        //    var r3 = Spectrum[maxIndex + 1].Magnitude / Spectrum.Count();
        //    var r = r1;
        //    if (r1 < r3) { r = r3; }
        //    r = 1000.0 * r / r2;

        //    var falary = 0;
        //    var frequancy = FreqCalculation.Getfrequancy(Spectrum.Count, 76800);//числа надо поменять
        //    var maxFreq = frequancy[maxIndex];
        //    for (var i = 0; i < Spectrum.Count - 1; i++)//надо взять окошко в 10 элементов
        //    {
        //        if ((r > frequancy[i]) && (r <= frequancy[i + 1]))//как по человечески переписать это гавн.....
        //        {
        //            falary = i;
        //            break;
        //        }
        //    }//i1

        //    var resultFreq = (r1 < r3) ? maxIndex * _fsempling / Spectrum.Count + falary / 10000.0 / 2.0 * _fsempling/ Spectrum.Count :
        //        maxIndex * _fsempling/ Spectrum.Count - falary / 10000.0 / 2.0 * _fsempling/ Spectrum.Count;

        //    if (maxIndex > Spectrum.Count / 2)
        //    {
        //        resultFreq = -76800.0 + resultFreq;
        //    }
        //    AccuracyFreq = resultFreq;
        //    //printf("\n 2. %d r1= %f r2= %f r3= %f f0= %f", i2, r1, r2, r3, f0);
        //    return resultFreq;
        //}
        public static double EvaluationOfFreq(List<Complex> signal)////Назаров мод
        {


            //Spectrum = FFT.Forward(signal);
            var I = ComplexSignals.Real(signal).ToArray();
            var Q = ComplexSignals.Imaginary(signal).ToArray();
            //I.Reverse();
            //Q.Reverse();

            FourierTransform.fft_f(ref I, ref Q, I.Length, -1);
            Spectrum = ComplexSignals.ToComplex(I.ToList(), Q.ToList());
            //var nsignal = new List<string>();

            //FileStream fs = new FileStream("data_before_psp.txt", FileMode.OpenOrCreate);
            //StreamWriter sw = new StreamWriter(fs);
            //Action<string> writing = s =>  sw.WriteLine(s);
            //for (var i = 0; i < 8192; i++)
            //{
            //    writing(I[i].ToString() + '+' + Q[i].ToString() + 'j');
            //}
            ////Enumerable.Range(0, 8192).Do(element => writing(I[element].ToString() + '+' + Q[element].ToString() + 'j'));
            ////foreach (var element in nsignal)
            ////{
            ////    sw.WriteLine(element);
            ////}
            //sw.Close();
            //fs.Close();
            
            int maxIndex = FindingMaxIndex(ComplexSignals.Module(Spectrum));
            //ReaderAndWriter.Writer(Spectrum, "spectrum.txt");
            //double magn = 0;
            //bool result = false;
            //AccuracyFreq =DigitalSignalProcessing.FrequencyEstimation.FrequencyEstimation.ImproveFrequency(DigitalSignalProcessing.FrequencyEstimation.FrequencyEstimationAlgoType.Quadratic,
            //    Spectrum,maxIndex,_fsempling, ref magn, ref result);
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
                //fprintf(fp,"\n %f+%fj",br[i1-1],bi[i1-1]);
                // fprintf(fp,"%f\n",freq[i1]);
                if ((r > freq[i1]) && (r <= freq[i1 + 1])) { i3 = i1; }
            }//i1
             //fclose (fp);
            if (r1 < r3) { AccuracyFreq = maxIndex * 76800.0/ 8192 + i3 / 10000.0/ 2.0* 76800.0/ 8192; }
            if (r1 > r3) { AccuracyFreq = maxIndex * 76800.0/ 8192 - i3 / 10000.0/ 2.0* 76800.0/ 8192; }

            if (maxIndex > 8192 / 2) { AccuracyFreq -= 76800.0; }

            //if (maxIndex > Spectrum.Count / 2)
            //{
            //    AccuracyFreq = -76800.0 + AccuracyFreq;
            //}
            //ReaderAndWriter.Writer(Spectrum, "spectrum.txt");
            return AccuracyFreq;
            //var r1 = Spectrum[maxIndex - 1].Magnitude / Spectrum.Count();
            //var r2 = Spectrum[maxIndex].Magnitude / Spectrum.Count();
            //var r3 = Spectrum[maxIndex + 1].Magnitude / Spectrum.Count();
            //var r = r1;
            //if (r1 < r3) { r = r3; }
            //r = 1000.0 * r / r2;

            //var falary = 0;
            //var frequancy = FreqCalculation.Getfrequancy(Spectrum.Count, 76800);//числа надо поменять
            //var maxFreq = frequancy[maxIndex];
            //for (var i = 0; i < Spectrum.Count - 1; i++)//надо взять окошко в 10 элементов
            //{
            //    if ((r > frequancy[i]) && (r <= frequancy[i + 1]))//как по человечески переписать это гавн.....
            //    {
            //        falary = i;
            //        break;
            //    }
            //}//i1

            //var resultFreq = (r1 < r3) ? maxIndex * _fsempling / Spectrum.Count + falary / 10000.0 / 2.0 * _fsempling / Spectrum.Count :
            //    maxIndex * _fsempling / Spectrum.Count - falary / 10000.0 / 2.0 * _fsempling / Spectrum.Count;

            //if (maxIndex > Spectrum.Count / 2)
            //{
            //    resultFreq = -76800.0 + resultFreq;
            //}
            //AccuracyFreq = resultFreq;
            ////printf("\n 2. %d r1= %f r2= %f r3= %f f0= %f", i2, r1, r2, r3, f0);
            //return resultFreq;
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
            //var newSignal = CompensationOfFreq(signal);
            var newSignal = NazarovCompensationOfFreq(signal);

            var I = ComplexSignals.Real(newSignal).ToArray();
            var Q = ComplexSignals.Imaginary(newSignal).ToArray();
            //I.Reverse();
            //Q.Reverse();

            FourierTransform.fft_f(ref I, ref Q, I.Length, -1);
            var newSpectrum = ComplexSignals.ToComplex(I.ToList(), Q.ToList());
            //var newSpectrum = FFT.Inversal(newSignal);
            //if (Phaza.Equals(0))
            //{
                
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
                ReaderAndWriter.Writer(signal, "orig_signal.txt");
                resultSignal = NazarovCompensationOfFreq(signal);
                ReaderAndWriter.Writer(resultSignal, "data_without_freq.txt");

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
            //var nd = ComplexSignals.Shift(signal, -AccuracyFreq, _fsempling);

            return ComplexSignals.Shift(signal, -AccuracyFreq, _fsempling);//<-здесь изменение
            //for (var i1 = 0; i1 < signal.Count; i1++)
            //{
            //    var t = i1 * 1.0 / 76800.0;
            //    var r1 = signal[i1].Real;
            //    var r2 = signal[i1].Imaginary;
            //    var real = r1 * Math.Cos(2.0 * Math.PI * (AccuracyFreq) * t) + r2 * Math.Sin(2.0 * Math.PI * (AccuracyFreq) * t);
            //    var imaginary = -r1 * Math.Sin(2.0 * Math.PI * (AccuracyFreq) * t) + r2 * Math.Cos(2.0 * Math.PI * (AccuracyFreq) * t);
            //    signal[i1] = new Complex(real, imaginary);
            //    //	  printf("\n %4d %f %f",i1,br[i1],bi[i1]);
            //    //	  getch();
            //}//i1
            //return signal;
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

            //for (var i1 = 0; i1 < Spectrum.Count; i1++)
            //{
            //    var r1 = signal[i1].Real;
            //    var r2 = signal[i1].Imaginary;
            //    var real = r1 * Math.Cos(Phaza) + r2 * Math.Sin(Phaza);
            //    var imaginary = -r1 * Math.Sin(Phaza) + r2 * Math.Cos(Phaza);
            //    signal[i1] = new Complex(real, imaginary);

            //}
            //return signal;
            return ComplexSignals.Shift(signal, 0, _fsempling,-Phaza);
        }
        #endregion

    }
}
