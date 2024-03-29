﻿using DigitalSignalProcessing;
using DigitalSignalProcessing.Generator;
using MathAndProcess.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Generator.ImitationSignals
{
    public class GeneratorOfSgbSignalResemplig : ISGBSignalGenerator
    {
        public static List<int> _message { get; private set; }
        public static double _freq { get; private set; }
        public double Fs { get; private set; }
        private static double _timeStart = 0;
        private static double _timeFinish = 1;
        private static double _amplitude = 1;
        private double SNR;
        private double _FrequancyOfSymbolsInMessage = 300;
        private double _FrequancyOfSymbolsInPRN = 38400;
        private List<double> signal = new List<double>();
        int[] k = new int[1];
        public GeneratorOfSgbSignalResemplig(double SNR, double freq = 900.6, double Fs = 76800.0, List<int> message = null)
        {
            this.Fs = Fs;
            _message = message ?? GeneratorOfMessage();
            _freq = freq;
            this.SNR = SNR;
        }
        public IEnumerable<Complex> GetSGBSignal()
        {
            List<double> ts = Enumerable.Range(0, (int)Fs + 8).Select(iteration => iteration * (_timeFinish - _timeStart) / Fs).ToList();//+1 sample to work with original part of program
            List<double> prnI, prnQ = new List<double>();
            PseudorandomSequence.GetSequensies(38404, out prnI, out prnQ);
            List<int> Iseq = prnI.Select(element => element > 0 ? -1 : 1).ToList();
            List<int> Qseq = prnQ.Select(element => element > 0 ? -1 : 1).ToList();
            List<int> messageI = _message.Where((item, index) => index % 2 == 0).ToList();
            List<int> messageQ = _message.Where((item, index) => index % 2 != 0).ToList();
            for (int i = 0; i < _message.Count; i++)
            {
                messageI.Insert(i, messageI[i]);
                messageQ[i] = messageQ[i] == 1 ? -1 : 1;
                messageQ.Insert(i, messageQ[i]);
                i++;
            }

            double W = 0.0;
            for (int i = 0; i < ts.Count(); i++)
            {
                int r = GetIValue(ts[i], _FrequancyOfSymbolsInMessage, messageI);
                int im = GetQValue(ts[i], _FrequancyOfSymbolsInMessage, messageQ);
                int rPRN = GetIValue(ts[i], _FrequancyOfSymbolsInPRN, Iseq);
                int imPRN = GetQValue(ts[i], _FrequancyOfSymbolsInPRN, Qseq);

                double real = Math.Cos(2 * Math.PI * _freq * ts[i]) * Convert.ToDouble(r * rPRN);
                double imag = Math.Sin(2 * Math.PI * _freq * ts[i]) * Convert.ToDouble(im * imPRN);

                signal.Add(_amplitude * (real + imag));

                W += Math.Pow(_amplitude * (real + imag), 2);
            }

            for (int i = 0; i < 10; i++)
            {
                signal.Insert(0, 0);
            }


            List<Complex> signalFromSattelite = AWGN.addAWGNperBit(ComplexSignals.ToComplex(signal), SNR, W, Fs, 300);


            return IQGenerator(ComplexSignals.Real(signalFromSattelite), -300, Fs);



        }
        private List<Complex> IQGenerator(List<double> signalFromAntenna, double df, double F_s)
        {
            List<Complex> shiftedSignal = new List<Complex>();
            double t = 0;
            double dt = 1.0 / F_s;

            double w = 2.0 * Math.PI * df;
            int gain = 10;

            for (int i = 0; i < signalFromAntenna.Count; i++)
            {
                double I = gain * Math.Cos(w * t) * signalFromAntenna[i];
                double Q = gain * Math.Sin(w * t) * signalFromAntenna[i];

                shiftedSignal.Add(new Complex(I, Q));

                t += dt;
            }


            return shiftedSignal.ToList();
        }
        private static List<int> GeneratorOfMessage()
        {
            List<int> message = new List<int>();
            for (int i = 0; i < 153; i++)
            {
                int symbol = i < 50 ? 0 : i % 2;
                message.Add(symbol == 1 ? 1 : -1);
                message.Add(symbol == 1 ? 1 : -1);

            }
            return message;
        }
        

        private int GetIValue(double time_Sec, double Frequancy, List<int> Sequance)
        {
            int Index = (int)Math.Floor(time_Sec * Frequancy);

            return Sequance[Index];
        }

        private int GetQValue(double time_Sec, double Frequancy, List<int> Sequance)
        {
            double delayedTime_Sec = time_Sec - 1.0 / (2 * _FrequancyOfSymbolsInPRN) + 1E-11;
            int Index = (int)Math.Floor(delayedTime_Sec * Frequancy);
            double bb = (delayedTime_Sec * Frequancy);
            return Index >= 0 ? Sequance[Index] : 1;
            
        }
    }
}
