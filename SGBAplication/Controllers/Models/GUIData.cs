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

        public OutputData GUI2OutputDataConverter()
        {
            OutputData dataPack = new OutputData();
            dataPack.Country = Country;
            dataPack.CurrentFrequency_Hz = CurrentFrequency_Hz;
            dataPack.FullMessage = FullMessage;
            return dataPack;
            //dataPack.country = country;

        }
        public OutputDataPLL GUI2OutputPLLDataConverter()
        {
            OutputDataPLL dataPack = new OutputDataPLL();
            dataPack.Country = this.Country;
            dataPack.CurrentFrequency_Hz = this.CurrentFrequency_Hz;
            dataPack.FullMessage = this.FullMessage;
            return dataPack;
            //dataPack.country = country;

        }
        public void Output2GUIDataConverter(AOutputData dataPack)
        {
            //var dataPack = new MathAndProcessing.OutputData();
            this.Country = dataPack.Country;
            this.CurrentFrequency_Hz = dataPack.CurrentFrequency_Hz;
            this.FullMessage = dataPack.FullMessage;
            //dataPack.country = country;

        }

        public void Output2GUIDataConverter(OutputDataPLL dataPack)
        {
            //var dataPack = new MathAndProcessing.OutputData();
            this.Country = dataPack.Country;
            this.CurrentFrequency_Hz = dataPack.CurrentFrequency_Hz;
            this.FullMessage = dataPack.FullMessage;
            //dataPack.country = country;

        }

    }
}
