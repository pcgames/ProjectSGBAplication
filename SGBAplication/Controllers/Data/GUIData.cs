using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Data
{
    public class GUIData
    {

        public string StartIndex { get; set; }

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
            dataPack.currentFrequancy = country;
            dataPack.fullMessage = country;
            return dataPack;
            //dataPack.country = country;

        }
        public MathAndProcessing.OutputDataPLL GUI2OutputPLLDataConverter()
        {
            var dataPack = new MathAndProcessing.OutputDataPLL();
            dataPack.country = country;
            dataPack.currentFrequancy = country;
            dataPack.fullMessage = country;
            return dataPack;
            //dataPack.country = country;

        }
        public void Output2GUIDataConverter(MathAndProcessing.OutputData dataPack)
        {
            //var dataPack = new MathAndProcessing.OutputData();
            country = dataPack.country;
            currentFrequancy = dataPack.country;
            fullMessage = dataPack.country;
            //dataPack.country = country;

        }

    }
}
