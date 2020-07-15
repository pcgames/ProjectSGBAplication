using Controllers.Models;
using DataAccess;
using DataAccess.Models;
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

        public List<List<Complex>> StartDecoderOfNonResemplingSignalWithPll(ref GUIData dataPack)
        {
            InputData inputData = ResampleInputData(_dataReader.GetSamples(dataPack.FileName, 10000000));

            IProcessing processor = new ProcessingPLL();

            return StartDecoderOfResemplingSignal(inputData, processor, ref dataPack);
        }

        //TODO: вот эта фигня не вписывается в декоратор, нужно переделывать 
        #region фигня статистическая
        public List<List<Complex>> StartDecoderOfNonResemplingSignalWithPll(GUIData dataPack, ref OutputDataPLL data)
        {
            InputData inputData = _dataReader.GetSamples(dataPack.FileName, 10000000);
            List<double> rI = ResemplingOfSignal.GetResemplingSamples(inputData.I);
            List<double> rQ = ResemplingOfSignal.GetResemplingSamples(inputData.Q);

            ProcessingPLL processor = new ProcessingPLL();
            List<List<Complex>> output = processor.Decoder(rI, rQ, dataPack.StartIndex);
            data = (OutputDataPLL)processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);

            return output;
        }
        #endregion

        public List<List<Complex>> StartDecoderOfResemplingSignalWithPll(ref GUIData dataPack)
        {
            InputData inputData = _dataReader.GetSamples(dataPack.FileName, 76809, Convert.ToInt64(dataPack.StartIndex), ';');

            IProcessing processor = new ProcessingPLL();

            return StartDecoderOfResemplingSignal(inputData, processor, ref dataPack);
        }

        public List<List<Complex>> StartDecoderOfNonResemplingSignal(ref GUIData dataPack)
        {
            InputData inputData = ResampleInputData(_dataReader.GetSamples(dataPack.FileName, 10000000));

            IProcessing processor = new Processing();

            return StartDecoderOfResemplingSignal(inputData, processor, ref dataPack);
        }

        public List<List<Complex>> StartDecoderOfResemplingSignal(ref GUIData dataPack)
        {
            InputData inputData = _dataReader.GetSamples(dataPack.FileName, 76809, Convert.ToInt64(dataPack.StartIndex), ';');

            IProcessing processor = new Processing();

            return StartDecoderOfResemplingSignal(inputData, processor, ref dataPack);
        }

        private List<List<Complex>> StartDecoderOfResemplingSignal(InputData inputData, IProcessing processor, ref GUIData dataPack)
        {
            List<List<Complex>> output = processor.Decoder(inputData.I, inputData.Q, dataPack.StartIndex);
            OutputData data = (OutputData)processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);
            return output;
        }

        private InputData ResampleInputData(InputData nonReseplingData)
        {
            List<double> rI = ResemplingOfSignal.GetResemplingSamples(nonReseplingData.I);
            List<double> rQ = ResemplingOfSignal.GetResemplingSamples(nonReseplingData.Q);
            return new InputData(rI, rQ);
        }
    }
}
