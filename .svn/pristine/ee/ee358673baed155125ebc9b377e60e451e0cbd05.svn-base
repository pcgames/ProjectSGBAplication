using Controllers.Data;
using DataAccess;
using MathAndProcess.Calculations;
using MathAndProcessing;
using MathAndProcessing.Calculations;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Controllers
{
    public class ControllerMathAndProcessing
    {
        DataReader _dataReader;

        public ControllerMathAndProcessing()
        {
            _dataReader = new DataReader();
        }

        public List<List<Complex>> DecoderOfNonResemplingSignalWithPll(ref GUIData dataPack)
        {
            var I = new List<double>();
            var Q = new List<double>();

            _dataReader.GetSamples(dataPack.FileName, ref I, ref Q, 10000000);
            var rI = ResemplingOfSignal.GetResemplingSamples(I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(Q);

            ProcessingPLL processor = new ProcessingPLL();
            var output = processor.Decoder(rI, rQ, dataPack.StartIndex);
            OutputDataPLL data = (OutputDataPLL)processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);

            return output;
        }

        public List<List<Complex>> DecoderOfNonResemplingSignalWithPll(GUIData dataPack, ref OutputDataPLL data)
        {
            var I = new List<double>();
            var Q = new List<double>();

            _dataReader.GetSamples(dataPack.FileName, ref I, ref Q, 10000000);
            var rI = ResemplingOfSignal.GetResemplingSamples(I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(Q);

            ProcessingPLL processor = new ProcessingPLL();
            var output = processor.Decoder(rI, rQ, dataPack.StartIndex);
            data = (OutputDataPLL)processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);

            return output;
        }

        public List<List<Complex>> DecoderOfResemplingSignalWithPll(ref GUIData dataPack)
        {
            var rI = new List<double>();
            var rQ = new List<double>();

            _dataReader.GetSamples(dataPack.FileName, ref rI, ref rQ, 76809, Convert.ToInt64(dataPack.StartIndex), ';');

            ProcessingPLL processor = new ProcessingPLL();
            var output = processor.Decoder(rI, rQ, dataPack.StartIndex);
            var data = (OutputDataPLL)processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);
            return output;
        }

        public List<List<Complex>> DecoderOfNonResemplingSignal(ref GUIData dataPack)
        {
            var I = new List<double>();
            var Q = new List<double>();

            _dataReader.GetSamples(dataPack.FileName, ref I, ref Q, 10000000);
            var rI = ResemplingOfSignal.GetResemplingSamples(I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(Q);

            IProcessing processor = new Processing();
            var output = processor.Decoder(rI, rQ, dataPack.StartIndex);
            var data = processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);
            return output;
        }

        public List<List<Complex>> DecoderOfResemplingSignal(ref GUIData dataPack)
        {
            var rI = new List<double>();
            var rQ = new List<double>();

            _dataReader.GetSamples(dataPack.FileName, ref rI, ref rQ, 76809, Convert.ToInt64(dataPack.StartIndex), ';');

            Processing processor = new Processing();
            var output = processor.Decoder(rI, rQ, dataPack.StartIndex);
            var data = (OutputData)processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);
            return output;

        }
    }
}
