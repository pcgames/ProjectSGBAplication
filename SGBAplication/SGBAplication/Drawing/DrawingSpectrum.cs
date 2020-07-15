using DigitalSignalProcessing;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Windows.Forms.DataVisualization.Charting;

namespace SGBAplication.Drawing
{

    public class DrawingSpectrum : AbstractDrawing<Complex>
    {
        private static double _fSempling;

        public DrawingSpectrum(Chart samplesChart, double frequancySempling) : base(samplesChart)
        {
            _fSempling = frequancySempling;
            _samplesChart = samplesChart;
        }

        public DrawingSpectrum(Chart samplesChart) : base(samplesChart)
        {
            _samplesChart = samplesChart;
        }

        public override void DrawChart(List<Complex> spectrum, List<double> xValues)
        {
            Series ChartSeries = new Series()
            {
                XValueType = ChartValueType.Int32,
                ChartType = SeriesChartType.Point,
                Color = System.Drawing.Color.Red,
                MarkerSize = 4,
                LegendText = "полученный спектр сигнала",
                Name = "SpectrumChart"
            };

            CleanSeries(_samplesChart, ChartSeries.Name);


            if (_fSempling.Equals(0))
            {
                xValues = Enumerable.Range(0, spectrum.Count).Select<int, double>(element => element).ToList();
            }
            else
            {
                _samplesChart.ChartAreas[0].AxisX.Minimum = -38400;
                _samplesChart.ChartAreas[0].AxisX.Maximum = 38400;
                _samplesChart.ChartAreas[0].AxisX.Interval = 12800;

            }
            DrawSamples(ChartSeries, xValues, spectrum);
            DrawCarrierFrequancy(spectrum, xValues);
        }

        private void DrawCarrierFrequancy(List<Complex> spectrum, List<double> xValues)
        {

            Series ChartSeries = new Series()
            {
                XValueType = ChartValueType.Int32,
                ChartType = SeriesChartType.Point,
                Color = System.Drawing.Color.Black,
                MarkerSize = 4,
                MarkerStyle = MarkerStyle.Cross,
                Name = "carrier frequancy"
            };
            //взял список,создал новую переменную(типа Anonymous) с полями которые можно получить, 
            //далее просто применил агрегирование к списку, итератор будет смотреть на элемент 
            //который уже запомнил элемент и сравнивать со следующим элементом, после чего выбирать максимальный
            CleanSeries(_samplesChart, ChartSeries.Name);
            var maxElement = ComplexSignals.Module(spectrum)
                    .Select((value, index) => new { Value = value, Index = index })
                    .Aggregate((a, b) => (a.Value > b.Value) ? a : b)
                    ;
            ChartSeries.LegendText = "часота сигнала = " + xValues[maxElement.Index].ToString();

            DrawPoint(_samplesChart, ChartSeries, xValues[maxElement.Index], maxElement.Value);

        }

        protected override void DrawSamples(Series usefullSeries, List<double> xValues, List<Complex> yValues)
        {
            for (int i = 0; i < xValues.Count; i++)
            {
                DrawPoint(_samplesChart, usefullSeries, xValues[i], (yValues[i]).Magnitude);
            }

        }
    }
}