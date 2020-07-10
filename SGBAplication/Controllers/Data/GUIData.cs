using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathAndProcessing;

namespace Controllers.Data
{
    public class GUIData
    {

        public string startIndex { get; set; }

        public string fileName { get; set; }

        public string SNR { get; set; }
        
        public string fullMessage { get; set; }

        public string country { get; set; }

        public string currentFrequancy { get; set; }

        public string fileOfPackages { get; set; }

        public MathAndProcessing.OutputData GUI2OutputDataConverter()
        {
            var dataPack = new MathAndProcessing.OutputData();
            dataPack.Country = country;
            dataPack.CurrentFrequency_Hz = currentFrequancy;
            dataPack.FullMessage = fullMessage;
            return dataPack;
            //dataPack.country = country;

        }
        public MathAndProcessing.OutputDataPLL GUI2OutputPLLDataConverter()
        {
            var dataPack = new MathAndProcessing.OutputDataPLL();
            dataPack.Country = this.country;
            dataPack.CurrentFrequency_Hz = this.currentFrequancy;
            dataPack.FullMessage = this.fullMessage;
            return dataPack;
            //dataPack.country = country;

        }
        public void Output2GUIDataConverter(AOutputData dataPack)
        {
            //var dataPack = new MathAndProcessing.OutputData();
            this.country = dataPack.Country;
            this.currentFrequancy = dataPack.CurrentFrequency_Hz;
            this.fullMessage = dataPack.FullMessage;
            //dataPack.country = country;

        }

        public void Output2GUIDataConverter(OutputDataPLL dataPack)
            {
            //var dataPack = new MathAndProcessing.OutputData();
            this.country = dataPack.Country;
            this.currentFrequancy = dataPack.CurrentFrequency_Hz;
            this.fullMessage = dataPack.FullMessage;
            //dataPack.country = country;

        }

    }
}
