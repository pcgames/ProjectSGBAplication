using Controllers;
using Controllers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace SGBFormAplication
{
    public partial class Form1 : Form
    {
        readonly ControllerSGBApplication _controller;
        private GUIData _dataPack;
        public Form1()
        {
            InitializeComponent();
            _controller = new ControllerSGBApplication();
        }

        private void Go_Click(object sender, EventArgs e)
        {
            if (CheckingSimulateSignal.Checked)
            {
                _dataPack.StartIndex = "0";
                fileName.Text = "simulatedSignalnew.csv";
                var snr = Convert.ToDouble(SNR.Text);
                _controller.SimulateSignal(snr, fileName.Text);
            }

            InitializeGUIDataPack();
            ProcessingType type = SelectProcessorType();

            List<List<Complex>> rnewDataAndSpectrum = _controller.StartDecoder(type, ref _dataPack);

            DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
        }

        private ProcessingType SelectProcessorType()
        {
            if(checkResempling.Checked)
            {
                if(checkUsePLLForProcessing.Checked)
                {
                    return ProcessingType.ResemplingWithPll;
                }
                else
                {
                    return ProcessingType.ResemplingWithoutPll;
                }
            }
            else
            {
                if (checkUsePLLForProcessing.Checked)
                {
                    return ProcessingType.NonResemplingWithPll;
                }
                else
                {
                    return ProcessingType.NonResemplingWithoutPll;
                }
            }
        }

        private void StatisticButton_Click(object sender, EventArgs e)
        {
            if (checkResempling.Checked == true)
            {
                if (checkUsePLLForStatictic.Checked == true)
                {
                    InitializeGUIDataPack();

                    _controller.GenerateRealResemplingDataStatisticsWithPll(_dataPack);
                }
                else
                {
                    _controller.GenerateRealResemplingDataStatistics(_dataPack);
                }
            }
        }

        private void StatisticGenerator_Click(object sender, EventArgs e)
        {
            int countMessages = 10000;

            InitializeGUIDataPack();

            if (checkUsePLLForStatictic.Checked)
            {
                _controller.GenerateStatisticsWithPLL(countMessages, _dataPack);
            }
            else
            {
                _controller.GenerateStatistics(countMessages, _dataPack);
            }
        }

        private void InitializeGUIDataPack()
        {
            _dataPack = new GUIData();

            _dataPack.CurrentFrequency_Hz = currentFrequancy.Text;
            _dataPack.FileName = fileName.Text;
            _dataPack.fileOfPackages = fileOfPackages.Text;
            _dataPack.FullMessage = fullMessage.Text;
            _dataPack.SNR = SNR.Text;
            _dataPack.StartIndex = startIndex.Text;
            _dataPack.Country = country.Text;
        }

        private void DrawOfBPSKSignalAndSpectrum(List<Complex> newDataWindowed, List<Complex> rnewData)
        {
            List<Complex> spectrum = new List<Complex>();
            List<double> xValues = new List<double>();
            _controller.GetDataForSpectrumChart(ref spectrum, ref xValues, newDataWindowed);
            List<double> signalsWithIQChanals = ControllerSGBApplication.GetDataForSignalChart(rnewData);

            new SGBAplication.Drawing.DrawingSignals(signalChart).DrawChart(signalsWithIQChanals);

            new SGBAplication.Drawing.DrawingSpectrum(spectrumChart).DrawChart(spectrum, xValues);
        }
    }
}
