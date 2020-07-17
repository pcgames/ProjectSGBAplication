using Controllers;
using Controllers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SGBFormAplication
{
    public partial class Form1 : Form
    {
        readonly ControllerSGBApplication _controller;
        private GUIData dataPack;
        public Form1()
        {
            InitializeComponent();
            _controller = new ControllerSGBApplication();
        }


        private void Go_Click(object sender, EventArgs e)
        {
            InitializeGUIDataPack();

            switch (checkResempling.Checked)
            {
                case true:
                    InitializeGUIDataPack();
                    List<List<System.Numerics.Complex>> rnewDataAndSpectrum = _controller.StartDecoderOfResemplingSignal(ref dataPack);
                    DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    break;
                case false:
                    if (CheckingSimulateSignal.Checked)
                    {
                        startIndex.Text = "0";

                        fileName.Text = "simulatedSignalnew.csv";

                        DataAccess.DataWriter.WriteToFile(new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(Convert.ToDouble(SNR.Text), 900.2, 102300).GetSGBSignal().ToList(), fileName.Text);

                        rnewDataAndSpectrum = _controller.StartDecoderOfNonResemplingSignal(ref dataPack);

                        DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    else
                    {
                        InitializeGUIDataPack();
                        rnewDataAndSpectrum = _controller.StartDecoderOfNonResemplingSignal(ref dataPack);
                        DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
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

                    _controller.GenerateRealResemplingDataStatisticsWithPll(dataPack);
                }
                else
                {
                    _controller.GenerateRealResemplingDataStatistics(dataPack);
                }
            }
        }

        private void StatisticGenerator_Click(object sender, EventArgs e)
        {
            int countMessages = 10000;

            InitializeGUIDataPack();

            if (checkUsePLL.Checked)
            {
                _controller.GenerateStatisticsWithPLL(countMessages, dataPack);
            }
            else
            {
                _controller.GenerateStatistics(countMessages, dataPack);
            }
        }

        private void PllProcess_Click(object sender, EventArgs e)
        {

            switch (checkResempling.Checked)
            {
                case true:
                    InitializeGUIDataPack();

                    List<List<System.Numerics.Complex>> rnewDataAndSpectrum = _controller.StartDecoderOfResemplingSignalWithPll(ref dataPack);
                    DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    break;
                case false:
                    if (CheckingSimulateSignal.Checked)
                    {
                        startIndex.Text = "0";
                        fileName.Text = "simulatedSignalnew.csv";
                        InitializeGUIDataPack();

                        DataAccess.DataWriter.WriteToFile(new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(Convert.ToDouble(SNR.Text), 900.2, 102300).GetSGBSignal().ToList(), fileName.Text);
                        rnewDataAndSpectrum = _controller.StartDecoderOfNonResemplingSignalWithPll(ref dataPack);

                        InitializeForm();
                        DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    else
                    {
                        rnewDataAndSpectrum = _controller.StartDecoderOfNonResemplingSignalWithPll(ref dataPack);
                        DrawOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }

                    break;

            }

        }
        private void InitializeGUIDataPack()
        {
            dataPack = new GUIData();

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
        private void DrawOfBPSKSignalAndSpectrum(List<System.Numerics.Complex> newDataWindowed, List<System.Numerics.Complex> rnewData)
        {
            List<System.Numerics.Complex> spectrum = new List<System.Numerics.Complex>();
            List<double> xValues = new List<double>();
            _controller.GetDataForSpectrumChart(ref spectrum, ref xValues, newDataWindowed);
            List<double> signalsWithIQChanals = ControllerSGBApplication.GetDataForSignalChart(rnewData);

            new SGBAplication.Drawing.DrawingSignals(signalChart).DrawChart(signalsWithIQChanals);

            new SGBAplication.Drawing.DrawingSpectrum(spectrumChart, 76800).DrawChart(spectrum, xValues);
        }
    }
}
