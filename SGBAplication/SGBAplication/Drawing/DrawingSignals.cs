using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace SGBAplication.Drawing
{
    public class DrawingSignals : AbstractDrawing<double>
    {
        private static double _f;
        private static double _s;

        public DrawingSignals(Chart samplesChart, double start, double finish) : base(samplesChart)
        {
            //_samplesChart = samplesChart;
            _s = start;
            _f = finish;
        }

        public DrawingSignals(Chart samplesChart) : base(samplesChart)
        {
        }

        public override void DrawChart(List<double> signalSamples)
        {
            Series ChartSeries = new Series()
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
                int countOfElements = signalSamples.Count();
                DrawSamples(ChartSeries, Enumerable.Range(0, countOfElements)
            .Select(element => _s + (_f - _s) * element / countOfElements).ToList(), signalSamples);
            }

        }
    }
}
