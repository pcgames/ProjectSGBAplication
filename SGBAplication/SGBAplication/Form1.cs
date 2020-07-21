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

            DrawBPSKSignal(rnewDataAndSpectrum[1]);
            DrawBPSKSpectrum(rnewDataAndSpectrum[0]);
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

        private void DrawBPSKSpectrum(List<Complex> newDataWindowed)
        {
            List<Complex> spectrum = _controller.GetSpectrumForChart(newDataWindowed);
            List<double> xValues = _controller.GetXValuesForSpectrumChart(spectrum);

            new SGBAplication.Drawing.DrawingSpectrum(spectrumChart).DrawChart(spectrum, xValues);
        }

        private void DrawBPSKSignal(List<Complex> rnewData)
        {
            List<double> signalsWithIQChanals = ControllerSGBApplication.GetDataForSignalChart(rnewData);

            new SGBAplication.Drawing.DrawingSignals(signalChart).DrawChart(signalsWithIQChanals);
        }
    }
}
