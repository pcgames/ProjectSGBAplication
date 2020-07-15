using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace SGBAplication.Drawing
{
    /// <summary>
    /// данный класс является родительским классом отрисовки графиков
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class abstractDrawing<T> where T : struct
    {
        /// <summary>
        /// ссылка на график,
        /// на котором будет производиться отрисовка
        /// </summary>
        protected static Chart _samplesChart;

        #region public methods
        public abstractDrawing(Chart samplesChart)
        {
            _samplesChart = samplesChart;
        }
        /// <summary>
        /// функция отрисовки падаваемых элементов
        /// </summary>
        /// <param name="Samples">падаваемые элементы</param>
        public virtual void DrawChart(List<T> Samples)
        {
            Series ChartSeries = new Series()
            {
                XValueType = ChartValueType.Int32,
                ChartType = SeriesChartType.Point,
                Color = System.Drawing.Color.Blue,
                MarkerSize = 4,
                LegendText = "полученный сигнал",
                Name = "AbstractChart"
            };
            CleanSeries(_samplesChart, ChartSeries.Name);
            Enumerable.Range((int)0, (int)Samples.Count()).ToList();
            DrawSamples(ChartSeries, Enumerable.Range(0, Samples.Count()).ToList()
                .Select(element => (double)element).ToList(), Samples);
        }
        public virtual void DrawChart(List<T> Samples, List<double> xValues)
        {
            Series ChartSeries = new Series()
            {
                XValueType = ChartValueType.Int32,
                ChartType = SeriesChartType.Point,
                Color = System.Drawing.Color.Blue,
                MarkerSize = 4,
                LegendText = "полученный сигнал",
                Name = "AbstractChart"
            };
            CleanSeries(_samplesChart, ChartSeries.Name);
            Enumerable.Range((int)0, (int)Samples.Count()).ToList();
            DrawSamples(ChartSeries, Enumerable.Range(0, Samples.Count()).ToList()
                .Select(element => (double)element).ToList(), Samples);
        }
        #endregion

        #region protected methods
        protected virtual void DrawSamples(Series usefullSeries, List<double> xValues, List<T> yValues)
        {
            for (int i = 0; i < xValues.Count; i++)
            {
                DrawPoint(_samplesChart, usefullSeries, xValues[i], Convert.ToDouble(yValues[i]));
            }

        }

        protected void CleanSeries(Chart cleaningChart, string seriesName)
        {
            try
            {
                cleaningChart.Series.RemoveAt(cleaningChart.Series.IndexOf(seriesName));
            }
            catch
            {

            }

        }
        protected static void DrawPoint(Chart chart, Series series, double x, double y)
        {
            try
            {
                if (chart.Series.Contains(chart.Series.Where(element => element.Name == series.Name).Last()))
                {
                    chart.Series[series.Name].Points.AddXY(x, y);
                }
                else
                {
                    series.Points.AddXY(x, y);
                    chart.Series.Add(series);
                }
            }
            catch
            {
                try
                {
                    series.Points.AddXY(x, y);
                    chart.Series.Add(series);
                }
                catch
                {
                    throw new NotImplementedException("Вы не произвели вычисление параметра");
                }

            }
        }

        #endregion
    }
}