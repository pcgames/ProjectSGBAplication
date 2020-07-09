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
            dataPack.country = country;
            dataPack.currentFrequancy = currentFrequancy;
            dataPack.fullMessage = fullMessage;
            return dataPack;
            //dataPack.country = country;

        }
        public MathAndProcessing.OutputDataPLL GUI2OutputPLLDataConverter()
        {
            var dataPack = new MathAndProcessing.OutputDataPLL();
            dataPack.country = this.country;
            dataPack.currentFrequancy = this.currentFrequancy;
            dataPack.fullMessage = this.fullMessage;
            return dataPack;
            //dataPack.country = country;

        }
        public void Output2GUIDataConverter(OutputData dataPack)
        {
            //var dataPack = new MathAndProcessing.OutputData();
            this.country = dataPack.country;
            this.currentFrequancy = dataPack.currentFrequancy;
            this.fullMessage = dataPack.fullMessage;
            //dataPack.country = country;

        }

        public void Output2GUIDataConverter(OutputDataPLL dataPack)
        {
            //var dataPack = new MathAndProcessing.OutputData();
            this.country = dataPack.country;
            this.currentFrequancy = dataPack.currentFrequancy;
            this.fullMessage = dataPack.fullMessage;
            //dataPack.country = country;

        }

    }
}
