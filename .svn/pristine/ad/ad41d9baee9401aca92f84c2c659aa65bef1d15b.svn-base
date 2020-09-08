using Controllers.Models;
using DataAccess;
using DataAccess.Models;
using MathAndProcess.Calculations;
using MathAndProcessing;
using MathAndProcessing.Calculations;
using System;
using System.Collections.Generic;
using System.Numerics;
using static MathAndProcessing.SGBConstants;

namespace Controllers
{
    public class ControllerMathAndProcessing
    {
        readonly int _numOfResemplingSamples = 76809;


        SampleReader _dataReader;

        InputData _inputData;

        IProcessing _processor;

        public ControllerMathAndProcessing()
        {
            _dataReader = new SampleReader();
        }

        public List<List<Complex>> StartDecoderOfNonResemplingSignalWithPll(ref GUIData dataPack)
        {
            _inputData = ResampleInputData(_dataReader.GetSamples(dataPack.FileName, NON_RESEMPLING_SAMPLES_COUNT, 0));

            _processor = new ProcessingPLL();

            return StartDecoder(ref dataPack);
        }
        
        public List<List<Complex>> StartDecoderOfResemplingSignalWithPll(ref GUIData dataPack)
        {
            _inputData = _dataReader.GetSamples(dataPack.FileName, _numOfResemplingSamples, Convert.ToInt64(dataPack.StartIndex), ';');

            _processor = new ProcessingPLL();

            return StartDecoder(ref dataPack);
        }

        public List<List<Complex>> StartDecoderOfNonResemplingSignalWithoutPll(ref GUIData dataPack)
        {
            _inputData = ResampleInputData(_dataReader.GetSamples(dataPack.FileName,
                NON_RESEMPLING_SAMPLES_COUNT, Convert.ToInt64(dataPack.StartIndex)));

            _processor = new Processing();

            return StartDecoder(ref dataPack);
        }

        public List<List<Complex>> StartDecoderOfResemplingSignalWithoutPll(ref GUIData dataPack)
        {
            _inputData = _dataReader.GetSamples(dataPack.FileName, _numOfResemplingSamples, Convert.ToInt64(dataPack.StartIndex), ';');

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
