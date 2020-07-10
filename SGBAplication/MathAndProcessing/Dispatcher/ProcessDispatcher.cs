using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MathAndProcessing.Dispatcher
{
    public class ProcessDispatcher
    {
        IProcessing _processor;
        private Action _correctEndCallback;
        private ConcurrentQueue<> _dataForExecuting;

        public ProcessDispatcher(IProcessing processor)
        {
            _processor = processor;
        }

        public void StartDecoding( )
        {
            Thread thread = new Thread(() => DecoderProcessing());
            thread.Start();
        }

        private void DecoderProcessing()
        {
            while (IsThereDataInQueue())
            {
                var dataset;
                if (_dataForExecuting.TryDequeue(out dataset))
                {
                    DecodeSmth(dataset);
                }
                else
                {
                    throw new Exception();
                }
                EndSeance();
            }

            EndSeance();
        }

        public void AddDataInQueue(var dataset)
        {
            _dataForExecuting.Enqueue(dataset);
        }

        public void ClearTrackList()
        {
            var dataset;
            while (!_dataForExecuting.IsEmpty)
            {
                _dataForExecuting.TryDequeue(out dataset);
            }
        }

        private bool IsThereDataInQueue()
        {
            return !_dataForExecuting.IsEmpty;
        }

        private void DecodeSmth(var dataset)
        {
            _processor.Decoder(dataset);
        }

        private void EndSeance()
        {
            _correctEndCallback();
        }
    }
}
