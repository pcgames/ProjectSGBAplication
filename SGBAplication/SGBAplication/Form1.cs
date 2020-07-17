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
            InitializeGUIDataPack();

            List<List<Complex>> rnewDataAndSpectrum;
            if (checkResempling.Checked)
            {
                rnewDataAndSpectrum = _controller.StartDecoderOfResemplingSignalWithoutPll(ref _dataPack);
                DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
            }
            else
            {
                if (CheckingSimulateSignal.Checked)
                {
                    startIndex.Text = "0";

                    fileName.Text = "simulatedSignalnew.csv";

                    var snr = Convert.ToDouble(SNR.Text);

                    var sgbSignal = new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(snr, 900.2, 102300).GetSGBSignal().ToList();

                    DataAccess.DataWriter.WriteToFile(sgbSignal, fileName.Text);

                    rnewDataAndSpectrum = _controller.StartDecoderOfNonResemplingSignalWithoutPll(ref _dataPack);

                    DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                }
                else
                {
                    rnewDataAndSpectrum = _controller.StartDecoderOfNonResemplingSignalWithoutPll(ref _dataPack);
                    DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                }

            }
        }

        private void StatisticButton_Click(object sender, EventArgs e)
        {
            if (checkResempling.Checked == true)
            {
                if (checkUsePLL.Checked == true)
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

            if (checkUsePLL.Checked)
            {
                _controller.GenerateStatisticsWithPLL(countMessages, _dataPack);
            }
            else
            {
                _controller.GenerateStatistics(countMessages, _dataPack);
            }
        }

        private void PllProcess_Click(object sender, EventArgs e)
        {
            List<List<Complex>> rnewDataAndSpectrum;
            InitializeGUIDataPack();

            if (checkResempling.Checked)
            {
                rnewDataAndSpectrum = _controller.StartDecoderOfResemplingSignalWithPll(ref _dataPack);
                DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
            }
            else 
            { 
                if (CheckingSimulateSignal.Checked)
                {
                    startIndex.Text = "0";
                    fileName.Text = "simulatedSignalnew.csv";
                    InitializeGUIDataPack();

                    DataAccess.DataWriter.WriteToFile(new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(Convert.ToDouble(SNR.Text), 900.2, 102300).GetSGBSignal().ToList(), fileName.Text);
                    rnewDataAndSpectrum = _controller.StartDecoderOfNonResemplingSignalWithPll(ref _dataPack);

                    InitializeForm();
                    DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                }
                else
                {
                    rnewDataAndSpectrum = _controller.StartDecoderOfNonResemplingSignalWithPll(ref _dataPack);
                    DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
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

        private void InitializeForm()
        {
            currentFrequancy.Text = _dataPack.CurrentFrequency_Hz;
            fileName.Text = _dataPack.FileName;
            fileOfPackages.Text = _dataPack.fileOfPackages;
            fullMessage.Text = _dataPack.FullMessage;
            SNR.Text = _dataPack.SNR;
            startIndex.Text = _dataPack.StartIndex;
            country.Text = _dataPack.Country;
        }

        private void DrawOfBPSKSignalAndSpectrum(List<Complex> newDataWindowed, List<Complex> rnewData)
        {
            List<Complex> spectrum = new List<Complex>();
            List<double> xValues = new List<double>();
            _controller.GetDataForSpectrumChart(ref spectrum, ref xValues, newDataWindowed);
            List<double> signalsWithIQChanals = ControllerSGBApplication.GetDataForSignalChart(rnewData);

            new SGBAplication.Drawing.DrawingSignals(signalChart).DrawChart(signalsWithIQChanals);

            new SGBAplication.Drawing.DrawingSpectrum(spectrumChart, 76800).DrawChart(spectrum, xValues);
        }
    }
}
