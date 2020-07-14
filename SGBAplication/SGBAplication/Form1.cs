﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DigitalSignalProcessing;
using DigitalSignalProcessing.Windows;
using Controllers;
using MathAndProcessing;

namespace SGBFormAplication
{
    public partial class Form1 : Form
    {
        ControllerSGBApplication _controller;
        private Controllers.Data.GUIData dataPack;
        public Form1()
        {
            InitializeComponent();
            _controller = new ControllerSGBApplication();
        }


        private void Go_Click(object sender, EventArgs e)//Я думаю это стоит перенести в отдельный класс который будет выполнять только список функций определенных кнопок
        {
            switch (checkResempling.Checked)
            {
                case true:
                    InitializeGUIDataPack();
                    var rnewDataAndSpectrum = _controller.DecoderOfResemplingSignal( ref dataPack);
                    InitializeForm();
                    DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    break;
                case false:
                    if (CheckingSimulateSignal.Checked)
                    {
                        startIndex.Text = "0";

                        fileName.Text = "simulatedSignalnew.csv";

                        InitializeGUIDataPack();

                        DataAccess.DataWriter.WriteToFile(new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(Convert.ToDouble(SNR.Text),900.2,102300).GetSGBSignal().ToList(), fileName.Text);

                        rnewDataAndSpectrum = _controller.DecoderOfNonResemplingSignal(ref dataPack);

                        InitializeForm();

                        DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    else
                    {
                        InitializeGUIDataPack();
                        rnewDataAndSpectrum = _controller.DecoderOfNonResemplingSignal(ref dataPack);
                        InitializeForm();
                        DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    
                    break;
                
            }
            
        }

        private void StatisticButton_Click(object sender, EventArgs e)
        {

            if (checkResempling.Checked == true)
            {
                if (checkUsePLL.Checked == true)
                {
                    InitializeGUIDataPack();

                    _controller.StatisticsWithPll(dataPack);
                }
                else
                {
                    InitializeGUIDataPack();

                    _controller.Statistics(dataPack);

                }

            }

        }

        private void StatisticGenerator_Click(object sender, EventArgs e)
        {
            var countMessages = 10000;

            if (checkUsePLL.Checked)
            {
                InitializeGUIDataPack();

                _controller.StatisticsGeneratorForPLL(countMessages, dataPack);//это ужасно!!!!!
            }
            else
            {
                InitializeGUIDataPack();

                _controller.StatisticsGenerator(countMessages, dataPack);//АНАЛОГИЧНО

            }

        }

        private void PllProcess_Click(object sender, EventArgs e)
        {

            switch (checkResempling.Checked)
            {
                case true:
                    InitializeGUIDataPack();

                    var rnewDataAndSpectrum = _controller.DecoderOfResemplingSignalWithPll(ref dataPack);
                    DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    break;
                case false:
                    if (CheckingSimulateSignal.Checked)
                    {
                        startIndex.Text = "0";
                        fileName.Text = "simulatedSignalnew.csv";
                        InitializeGUIDataPack();

                        DataAccess.DataWriter.WriteToFile(new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(Convert.ToDouble(SNR.Text), 900.2, 102300).GetSGBSignal().ToList(), fileName.Text);
                        rnewDataAndSpectrum = _controller.DecoderOfNonResemplingSignalWithPll(ref dataPack);
                        
                        InitializeForm();
                        DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    else
                    {
                        rnewDataAndSpectrum = _controller.DecoderOfNonResemplingSignalWithPll(ref dataPack);
                        DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }

                    break;

            }

        }
        private void InitializeGUIDataPack()
        {
            dataPack = new Controllers.Data.GUIData();

            dataPack.CurrentFrequency_Hz = currentFrequancy.Text;
            dataPack.FileName = fileName.Text;
            dataPack.fileOfPackages = fileOfPackages.Text;
            dataPack.FullMessage = fullMessage.Text;
            dataPack.SNR = SNR.Text;
            dataPack.StartIndex = startIndex.Text;
            dataPack.Country = country.Text;
        }
        private void InitializeForm()
        {

            currentFrequancy.Text = dataPack.CurrentFrequency_Hz;
            fileName.Text = dataPack.FileName;
            fileOfPackages.Text = dataPack.fileOfPackages;
            fullMessage.Text = dataPack.FullMessage;
            SNR.Text = dataPack.SNR;
            startIndex.Text = dataPack.StartIndex;
            country.Text = dataPack.Country;
        }
        private void DrawingOfBPSKSignalAndSpectrum(List<System.Numerics.Complex> newDataWindowed, List<System.Numerics.Complex> rnewData)
        {
            var spectrum = new List<System.Numerics.Complex>();
            List<double> xValues = new List<double>();
            _controller.GetDataForSpectrumChart(ref spectrum, ref xValues, newDataWindowed);
            var signalsWithIQChanals= ControllerSGBApplication.GetDataForSignalChart(rnewData);

            new SGBAplication.Drawing.DrawingSignals(signalChart).DrawChart(signalsWithIQChanals);

            new SGBAplication.Drawing.DrawingSpectrum(spectrumChart, 76800).DrawChart(spectrum, xValues);
        }
    }
}
