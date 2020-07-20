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
        //Constants
        readonly int numOfNonResemplingSamples = 10000000;
        readonly int numOfResemplingSamples = 76809;


        SampleReader _dataReader;

        InputData _inputData;

        IProcessing _processor;

        public ControllerMathAndProcessing()
        {
            _dataReader = new SampleReader();
        }

        private List<List<Complex>> StartDecoderOfResemplingSignal2(ref GUIData dataPack)
        {

        }

        public List<List<Complex>> StartDecoderOfNonResemplingSignalWithPll(ref GUIData dataPack)
        {
            _inputData = ResampleInputData(_dataReader.GetSamples(dataPack.FileName, numOfNonResemplingSamples, 0));

            _processor = new ProcessingPLL();

            return StartDecoder(ref dataPack);
        }

        //TODO: вот эта фигня не вписывается в декоратор, нужно переделывать
        #region фигня статистическая
        public List<List<Complex>> StartDecoderOfNonResemplingSignalWithPll(GUIData dataPack, ref OutputDataPLL data)
        {
            InputData inputData = _dataReader.GetSamples(dataPack.FileName, numOfNonResemplingSamples, 0);
            List<double> rI = ResemplingOfSignal.GetResemplingSamples(inputData.I);
            List<double> rQ = ResemplingOfSignal.GetResemplingSamples(inputData.Q);

            ProcessingPLL processor = new ProcessingPLL();
            List<List<Complex>> output = processor.Decode(rI, rQ, dataPack.StartIndex);
            data = (OutputDataPLL)processor.GetOutputData();
            dataPack.ConvertOutput2GUIData(data);

            return output;
        }
        #endregion

        public List<List<Complex>> StartDecoderOfResemplingSignalWithPll(ref GUIData dataPack)
        {
            _inputData = _dataReader.GetSamples(dataPack.FileName, numOfResemplingSamples, Convert.ToInt64(dataPack.StartIndex), ';');

            _processor = new ProcessingPLL();

            return StartDecoder(ref dataPack);
        }

        public List<List<Complex>> StartDecoderOfNonResemplingSignalWithoutPll(ref GUIData dataPack)
        {
            _inputData = ResampleInputData(_dataReader.GetSamples(dataPack.FileName, numOfNonResemplingSamples, 0));

            _processor = new Processing();

            return StartDecoder(ref dataPack);
        }

        public List<List<Complex>> StartDecoderOfResemplingSignalWithoutPll(ref GUIData dataPack)
        {
            _inputData = _dataReader.GetSamples(dataPack.FileName, numOfResemplingSamples, Convert.ToInt64(dataPack.StartIndex), ';');

            _processor = new Processing();

            return StartDecoder(ref dataPack);
        }

        private List<List<Complex>> StartDecoder(ref GUIData dataPack)
        {
            List<List<Complex>> output = _processor.Decode(_inputData.I, _inputData.Q, dataPack.StartIndex);
            OutputData data = (OutputData)_processor.GetOutputData();
            dataPack.ConvertOutput2GUIData(data);
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
