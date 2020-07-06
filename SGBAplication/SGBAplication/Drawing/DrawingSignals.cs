using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace SGBAplication.Drawing
{
     public class DrawingSignals : abstractDrawing<double>
        {
            private static double _f;
            private static double _s;
            public DrawingSignals(Chart samplesChart,double start,double finish) : base(samplesChart)
            {
                //_samplesChart = samplesChart;
                _s = start;
                _f = finish;
            }
            public DrawingSignals(Chart samplesChart):base(samplesChart)
            {
            }
        //public DrawingSignals(int t)//супер магия-необходимо для создания конструктора не основываясь на том, что в абстрактном классе
        //{
        //}
        public override void DrawChart(List<double> signalSamples)
            {
                var ChartSeries = new Series()
                {
                    XValueType = ChartValueType.Int32,
                    ChartType = SeriesChartType.Point,
                    Color = System.Drawing.Color.Blue,
                    MarkerSize = 4,
                    LegendText = "полученный сигнал",
                    Name = "SignalChart"
                };
                CleanSeries(_samplesChart, ChartSeries.Name);
                if (_f.Equals(null))
                {
                    DrawSamples(ChartSeries, Enumerable.Range(0, signalSamples.Count()).ToList()
                .Select(element => (double)element).ToList(), signalSamples);
                }
                else
                {
                    var countOfElements = signalSamples.Count();
                    DrawSamples(ChartSeries, Enumerable.Range(0, countOfElements)
                .Select(element => _s+(_f-_s)*element/countOfElements).ToList(), signalSamples);
                }

            }
        }
    }
