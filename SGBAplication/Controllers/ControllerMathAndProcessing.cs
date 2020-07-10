using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Threading.Tasks;
using DigitalSignalProcessing;
using DigitalSignalProcessing.Windows;
using System.Windows.Forms;
using static DataAccess.DataReader;
using MathAndProcess.Calculations;
using MathAndProcess.Transformation;
using MathAndProcess;
using MathAndProcessing;
using Controllers.Data;
using System.Numerics;

namespace Controllers
{
    public class ControllerMathAndProcessing
    {
        public static List<List<Complex>> DecoderOfNonResemplingSignalWithPll(ref GUIData dataPack)
        {
            var I = new List<double>();
            var Q = new List<double>();

            GetSamples(dataPack.fileName, ref I, ref Q, 10000000);
            var rI = ResemplingOfSignal.GetResemplingSamples(I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(Q);

            ProcessingPLL processor = new ProcessingPLL();
            var output = processor.Decoder(rI, rQ, dataPack.startIndex);
            OutputDataPLL data = (OutputDataPLL)processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);

            return output;
        }

        public static List<List<Complex>> DecoderOfResemplingSignalWithPll(ref GUIData dataPack)
        {
            var rI = new List<double>();
            var rQ = new List<double>();

            GetSamples(dataPack.fileName, ref rI, ref rQ, 76809, Convert.ToInt64(dataPack.startIndex), ';');

            ProcessingPLL processor = new ProcessingPLL();
            var output = processor.Decoder(rI, rQ, dataPack.startIndex);
            var data = (OutputDataPLL)processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);
            return output;
        }

        public static List<List<Complex>> DecoderOfNonResemplingSignal(ref GUIData dataPack)
        {
            var I = new List<double>();
            var Q = new List<double>();

            GetSamples(dataPack.fileName, ref I, ref Q, 10000000);
            var rI = ResemplingOfSignal.GetResemplingSamples(I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(Q);

            IProcessing processor = new Processing();
            var output = processor.Decoder(rI, rQ, dataPack.startIndex);
            var data = processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);
            return output;
        }

        public static List<List<Complex>> DecoderOfResemplingSignal(ref GUIData dataPack)
        {
            var rI = new List<double>();
            var rQ = new List<double>();

            GetSamples(dataPack.fileName, ref rI, ref rQ, 76809, Convert.ToInt64(dataPack.startIndex), ';');

            Processing processor = new Processing();
            var output = processor.Decoder(rI, rQ, dataPack.startIndex);
            var data = (OutputData)processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);
            return output;

        }
    }
}
