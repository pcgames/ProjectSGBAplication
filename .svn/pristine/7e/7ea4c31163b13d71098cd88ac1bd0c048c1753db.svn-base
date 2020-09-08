using MathAndProcessing;

namespace Controllers.Models
{
    public class GUIData
    {

        public string StartIndex { get; set; }

        public string FileName { get; set; }

        public string SNR { get; set; }

        public string FullMessage { get; set; }

        public string Country { get; set; }

        public string CurrentFrequency_Hz { get; set; }

        public string fileOfPackages { get; set; }

        public OutputData ConvertGUI2OutputData()
        {
            OutputData dataPack = new OutputData();
            dataPack.Country = Country;
            dataPack.CurrentFrequency_Hz = CurrentFrequency_Hz;
            dataPack.FullMessage = FullMessage;
            return dataPack;

        }
        public OutputDataPLL ConvertGUI2OutputPLLData()
        {
            OutputDataPLL dataPack = new OutputDataPLL();
            dataPack.Country = this.Country;
            dataPack.CurrentFrequency_Hz = this.CurrentFrequency_Hz;
            dataPack.FullMessage = this.FullMessage;
            return dataPack;

        }
        public void ConvertOutput2GUIData(AOutputData dataPack)
        {
            this.Country = dataPack.Country;
            this.CurrentFrequency_Hz = dataPack.CurrentFrequency_Hz;
            this.FullMessage = dataPack.FullMessage;

        }

        public void ConvertOutput2GUIData(OutputDataPLL dataPack)
        {
            this.Country = dataPack.Country;
            this.CurrentFrequency_Hz = dataPack.CurrentFrequency_Hz;
            this.FullMessage = dataPack.FullMessage;
        }

    }
}
