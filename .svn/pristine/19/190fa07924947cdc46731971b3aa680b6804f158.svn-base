using System;
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
        public Form1()
        {
            InitializeComponent();
        }
        private Controllers.Data.GUIData dataPack;


        private void go_Click(object sender, EventArgs e)//Я думаю это стоит перенести в отдельный класс который будет выполнять только список функций определенных кнопок
        {


            switch (checkResempling.Checked)
            {
                case true:
                    InitializeGUIDataPack();
                    var rnewDataAndSpectrum = Controller.DecoderOfResemplingSignal( ref dataPack);
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

                        rnewDataAndSpectrum = Controller.DecoderOfResemplingSignal(ref dataPack);

                        InitializeForm();

                        DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    else
                    {
                        InitializeGUIDataPack();
                        rnewDataAndSpectrum = Controller.DecoderOfNonResemplingSignal(ref dataPack);
                        InitializeForm();
                        DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    
                    break;
                
            }
            
        }

        private void statisticButton_Click(object sender, EventArgs e)
        {

            if (checkResempling.Checked == true)
            {
                if (checkUsePLL.Checked == true)
                {
                    InitializeGUIDataPack();

                    Controller.StatisticsWithPll(dataPack);
                }
                else
                {
                    InitializeGUIDataPack();

                    Controller.Statistics(dataPack);

                }

            }

        }

        private void statisticGenerator_Click(object sender, EventArgs e)
        {
            if (checkUsePLL.Checked)
            {
                InitializeGUIDataPack();

                Controllers.Statistic.GenerateStatisic.StatisticsGeneratorForPLL(10000,dataPack);//это ужасно!!!!!
            }
            else
            {
                InitializeGUIDataPack();

                Controllers.Statistic.GenerateStatisic.StatisticsGenerator(10000, dataPack);//АНАЛОГИЧНО

            }

        }

        private void pllProcess_Click(object sender, EventArgs e)
        {

            switch (checkResempling.Checked)
            {
                case true:
                    InitializeGUIDataPack();

                    var rnewDataAndSpectrum = Controller.DecoderOfResemplingSignalWithPll(ref dataPack);
                    DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    break;
                case false:
                    if (CheckingSimulateSignal.Checked)
                    {
                        startIndex.Text = "0";
                        fileName.Text = "simulatedSignalnew.csv";
                        InitializeGUIDataPack();

                        DataAccess.DataWriter.WriteToFile(new Generator.ImitationSignals.GeneratorOfSgbSignalResemplig(Convert.ToDouble(SNR.Text), 900.2, 102300).GetSGBSignal().ToList(), fileName.Text);
                        rnewDataAndSpectrum = Controller.DecoderOfNonResemplingSignalWithPll(ref dataPack);
                        InitializeForm();
                        DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }
                    else
                    {
                        rnewDataAndSpectrum = Controller.DecoderOfNonResemplingSignalWithPll(ref dataPack);
                        DrawingOfBPSKSignalAndSpectrum(rnewDataAndSpectrum[0], rnewDataAndSpectrum[1]);
                    }

                    break;

            }

        }
        private void InitializeGUIDataPack()
        {
            dataPack = new Controllers.Data.GUIData();

            dataPack.currentFrequancy = currentFrequancy.Text;
            dataPack.fileName = fileName.Text;
            dataPack.fileOfPackages = fileOfPackages.Text;
            dataPack.fullMessage = fullMessage.Text;
            dataPack.SNR = SNR.Text;
            dataPack.startIndex = startIndex.Text;
            dataPack.country = country.Text;
        }
        private void InitializeForm()
        {

            currentFrequancy.Text = dataPack.currentFrequancy;
            fileName.Text = dataPack.fileName;
            fileOfPackages.Text = dataPack.fileOfPackages;
            fullMessage.Text = dataPack.fullMessage;
            SNR.Text = dataPack.SNR;
            startIndex.Text = dataPack.startIndex;
            country.Text = dataPack.country;
        }
        private void DrawingOfBPSKSignalAndSpectrum(List<System.Numerics.Complex> newDataWindowed, List<System.Numerics.Complex> rnewData)
        {
            var spectrum = new List<System.Numerics.Complex>();
            List<double> xValues = new List<double>();
            Controller.GetDataForSpectrumChart(ref spectrum, ref xValues, newDataWindowed);
            var signalsWithIQChanals= Controller.GetDataForSignalChart(rnewData);

            new SGBAplication.Drawing.DrawingSignals(signalChart).DrawChart(signalsWithIQChanals);

            new SGBAplication.Drawing.DrawingSpectrum(spectrumChart, 76800).DrawChart(spectrum, xValues);
        }



 
    }
}
