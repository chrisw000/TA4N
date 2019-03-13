using System;

/*
The MIT License (MIT)

Copyright (c) 2014-2016 Marc de Verdelhan & respective authors (see AUTHORS)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
namespace TA4N.Examples.indicators
{

	using Tick = TA4N.Tick;
	using TimeSeries = TA4N.TimeSeries;
	using ClosePriceIndicator = TA4N.indicators.simple.ClosePriceIndicator;
	using ChartFactory = org.jfree.chart.ChartFactory;
	using ChartPanel = org.jfree.chart.ChartPanel;
	using JFreeChart = org.jfree.chart.JFreeChart;
	using NumberAxis = org.jfree.chart.axis.NumberAxis;
	using DatasetRenderingOrder = org.jfree.chart.plot.DatasetRenderingOrder;
	using XYPlot = org.jfree.chart.plot.XYPlot;
	using CandlestickRenderer = org.jfree.chart.renderer.xy.CandlestickRenderer;
	using XYLineAndShapeRenderer = org.jfree.chart.renderer.xy.XYLineAndShapeRenderer;
	using Second = org.jfree.data.time.Second;
	using TimeSeriesCollection = org.jfree.data.time.TimeSeriesCollection;
	using DefaultHighLowDataset = org.jfree.data.xy.DefaultHighLowDataset;
	using OHLCDataset = org.jfree.data.xy.OHLCDataset;
	using ApplicationFrame = org.jfree.ui.ApplicationFrame;
	using RefineryUtilities = org.jfree.ui.RefineryUtilities;
	using Period = org.joda.time.Period;
	using CsvTradesLoader = TA4N.Examples.loaders.CsvTradesLoader;

	/// <summary>
	/// This class builds a traditional candlestick chart.
	/// </summary>
	public class CandlestickChart
	{

		/// <summary>
		/// Builds a JFreeChart OHLC dataset from a ta4j time series. </summary>
		/// <param name="series"> a time series </param>
		/// <returns> an Open-High-Low-Close dataset </returns>
		private static OHLCDataset createOHLCDataset(TimeSeries series)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nbTicks = series.getTickCount();
			int nbTicks = series.TickCount;

			DateTime[] dates = new DateTime[nbTicks];
			double[] opens = new double[nbTicks];
			double[] highs = new double[nbTicks];
			double[] lows = new double[nbTicks];
			double[] closes = new double[nbTicks];
			double[] volumes = new double[nbTicks];

			for (int i = 0; i < nbTicks; i++)
			{
				Tick tick = series.getTick(i);
				dates[i] = tick.EndTime.toDate();
				opens[i] = tick.OpenPrice.toDouble();
				highs[i] = tick.MaxPrice.toDouble();
				lows[i] = tick.MinPrice.toDouble();
				closes[i] = tick.ClosePrice.toDouble();
				volumes[i] = tick.Volume.toDouble();
			}

			OHLCDataset dataset = new DefaultHighLowDataset("btc", dates, highs, lows, opens, closes, volumes);

			return dataset;
		}

		/// <summary>
		/// Builds an additional JFreeChart dataset from a ta4j time series. </summary>
		/// <param name="series"> a time series </param>
		/// <returns> an additional dataset </returns>
		private static TimeSeriesCollection createAdditionalDataset(TimeSeries series)
		{
			ClosePriceIndicator indicator = new ClosePriceIndicator(series);
			TimeSeriesCollection dataset = new TimeSeriesCollection();
			org.jfree.data.time.TimeSeries chartTimeSeries = new org.jfree.data.time.TimeSeries("Btc price");
			for (int i = 0; i < series.TickCount; i++)
			{
				Tick tick = series.getTick(i);
				chartTimeSeries.add(new Second(tick.EndTime.toDate()), indicator.getValue(i).toDouble());
			}
			dataset.addSeries(chartTimeSeries);
			return dataset;
		}

		/// <summary>
		/// Displays a chart in a frame. </summary>
		/// <param name="chart"> the chart to be displayed </param>
		private static void displayChart(JFreeChart chart)
		{
			// Chart panel
			ChartPanel panel = new ChartPanel(chart);
			panel.FillZoomRectangle = true;
			panel.MouseWheelEnabled = true;
			panel.PreferredSize = new java.awt.Dimension(740, 300);
			// Application frame
			ApplicationFrame frame = new ApplicationFrame("Ta4j example - Candlestick chart");
			frame.ContentPane = panel;
			frame.pack();
			RefineryUtilities.centerFrameOnScreen(frame);
			frame.Visible = true;
		}

		public static void Main(string[] args)
		{
			/// <summary>
			/// Getting time series
			/// </summary>
			TimeSeries series = CsvTradesLoader.loadBitstampSeries().subseries(0, Period.hours(6));

			/// <summary>
			/// Creating the OHLC dataset
			/// </summary>
			OHLCDataset ohlcDataset = createOHLCDataset(series);

			/// <summary>
			/// Creating the additional dataset
			/// </summary>
			TimeSeriesCollection xyDataset = createAdditionalDataset(series);

			/// <summary>
			/// Creating the chart
			/// </summary>
			JFreeChart chart = ChartFactory.createCandlestickChart("Bitstamp BTC price", "Time", "USD", ohlcDataset, true);
			// Candlestick rendering
			CandlestickRenderer renderer = new CandlestickRenderer();
			renderer.AutoWidthMethod = CandlestickRenderer.WIDTHMETHOD_SMALLEST;
			XYPlot plot = chart.XYPlot;
			plot.Renderer = renderer;
			// Additional dataset
			int index = 1;
			plot.setDataset(index, xyDataset);
			plot.mapDatasetToRangeAxis(index, 0);
			XYLineAndShapeRenderer renderer2 = new XYLineAndShapeRenderer(true, false);
			renderer2.setSeriesPaint(index, Color.blue);
			plot.setRenderer(index, renderer2);
			// Misc
			plot.RangeGridlinePaint = Color.lightGray;
			plot.BackgroundPaint = Color.white;
			NumberAxis numberAxis = (NumberAxis) plot.RangeAxis;
			numberAxis.AutoRangeIncludesZero = false;
			plot.DatasetRenderingOrder = DatasetRenderingOrder.FORWARD;

			/// <summary>
			/// Displaying the chart
			/// </summary>
			displayChart(chart);
		}
	}

}