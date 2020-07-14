﻿using Controllers.Data;
using DataAccess;
using DataAccess2.Models;
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
            var inputData = ResampleData(_dataReader.GetSamples(dataPack.FileName, 10000000));

            IProcessing processor = new ProcessingPLL();

            return DecoderOfResemplingSignal_Test(inputData, processor, ref dataPack);
        }

        //TODO: вот эта фигня не вписывается в декоратор, нужно переделывать 
        #region фигня статистическая
        public List<List<Complex>> DecoderOfNonResemplingSignalWithPll(GUIData dataPack, ref OutputDataPLL data)
        {
            var inputData = _dataReader.GetSamples(dataPack.FileName, 10000000);
            var rI = ResemplingOfSignal.GetResemplingSamples(inputData.I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(inputData.Q);

            ProcessingPLL processor = new ProcessingPLL();
            var output = processor.Decoder(rI, rQ, dataPack.StartIndex);
            data = (OutputDataPLL)processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);

            return output;
        }
        #endregion

        public List<List<Complex>> DecoderOfResemplingSignalWithPll(ref GUIData dataPack)
        {
            var inputData = _dataReader.GetSamples(dataPack.FileName, 76809, Convert.ToInt64(dataPack.StartIndex), ';');

            IProcessing processor = new ProcessingPLL();

            return DecoderOfResemplingSignal_Test(inputData, processor, ref dataPack);
        }

        public List<List<Complex>> DecoderOfNonResemplingSignal(ref GUIData dataPack)
        {
            var inputData = ResampleData(_dataReader.GetSamples(dataPack.FileName, 10000000));

            IProcessing processor = new Processing();

            return DecoderOfResemplingSignal_Test(inputData, processor, ref dataPack);
        }

        public List<List<Complex>> DecoderOfResemplingSignal(ref GUIData dataPack)
        {
            var inputData = _dataReader.GetSamples(dataPack.FileName, 76809, Convert.ToInt64(dataPack.StartIndex), ';');

            IProcessing processor = new Processing();

            return DecoderOfResemplingSignal_Test(inputData, processor, ref dataPack);
        }

        private List<List<Complex>> DecoderOfResemplingSignal_Test(InputData inputData, IProcessing processor , ref GUIData dataPack)
        {
            var output = processor.Decoder(inputData.I, inputData.Q, dataPack.StartIndex);
            var data = (OutputData)processor.GetOutputData();
            dataPack.Output2GUIDataConverter(data);
            return output;
        }

        private InputData ResampleData(InputData nonReseplingData)
        {
            var rI = ResemplingOfSignal.GetResemplingSamples(nonReseplingData.I);
            var rQ = ResemplingOfSignal.GetResemplingSamples(nonReseplingData.Q);
            return new InputData(rI, rQ);
        }
    }
}
