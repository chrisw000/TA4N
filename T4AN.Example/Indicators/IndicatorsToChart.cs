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
	using Indicator = TA4N.Indicator;
	using Decimal = TA4N.Decimal;
	using Tick = TA4N.Tick;
	using TimeSeries = TA4N.TimeSeries;
	using ClosePriceIndicator = TA4N.indicators.simple.ClosePriceIndicator;
	using BollingerBandsLowerIndicator = TA4N.indicators.trackers.bollinger.BollingerBandsLowerIndicator;
	using BollingerBandsMiddleIndicator = TA4N.indicators.trackers.bollinger.BollingerBandsMiddleIndicator;
	using BollingerBandsUpperIndicator = TA4N.indicators.trackers.bollinger.BollingerBandsUpperIndicator;
	using ChartFactory = org.jfree.chart.ChartFactory;
	using ChartPanel = org.jfree.chart.ChartPanel;
	using JFreeChart = org.jfree.chart.JFreeChart;
	using DateAxis = org.jfree.chart.axis.DateAxis;
	using XYPlot = org.jfree.chart.plot.XYPlot;
	using Day = org.jfree.data.time.Day;
	using TimeSeriesCollection = org.jfree.data.time.TimeSeriesCollection;
	using ApplicationFrame = org.jfree.ui.ApplicationFrame;
	using RefineryUtilities = org.jfree.ui.RefineryUtilities;
	using CsvTicksLoader = TA4N.Examples.loaders.CsvTicksLoader;

	/// <summary>
	/// This class builds a graphical chart showing values from indicators.
	/// </summary>
	public class IndicatorsToChart
	{
		/// <summary>
		/// Builds a JFreeChart time series from a Ta4j time series and an indicator. </summary>
		/// <param name="tickSeries"> the ta4j time series </param>
		/// <param name="indicator"> the indicator </param>
		/// <param name="name"> the name of the chart time series </param>
		/// <returns> the JFreeChart time series </returns>
		private static org.jfree.data.time.TimeSeries buildChartTimeSeries(TimeSeries tickSeries, Indicator<Decimal> indicator, string name)
		{
			org.jfree.data.time.TimeSeries chartTimeSeries = new org.jfree.data.time.TimeSeries(name);
			for (int i = 0; i < tickSeries.TickCount; i++)
			{
				Tick tick = tickSeries.getTick(i);
				chartTimeSeries.add(new Day(tick.EndTime.toDate()), indicator.getValue(i).toDouble());
			}
			return chartTimeSeries;
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
			panel.PreferredSize = new java.awt.Dimension(500, 270);
			// Application frame
			ApplicationFrame frame = new ApplicationFrame("Ta4j example - Indicators to chart");
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
			TimeSeries series = CsvTicksLoader.loadAppleIncSeries();

			/// <summary>
			/// Creating indicators
			/// </summary>
			// Close price
			ClosePriceIndicator closePrice = new ClosePriceIndicator(series);
			// Bollinger bands
			BollingerBandsMiddleIndicator middleBBand = new BollingerBandsMiddleIndicator(closePrice);
			BollingerBandsLowerIndicator lowBBand = new BollingerBandsLowerIndicator(middleBBand, closePrice);
			BollingerBandsUpperIndicator upBBand = new BollingerBandsUpperIndicator(middleBBand, closePrice);

			/// <summary>
			/// Building chart dataset
			/// </summary>
			TimeSeriesCollection dataset = new TimeSeriesCollection();
			dataset.addSeries(buildChartTimeSeries(series, closePrice, "Apple Inc. (AAPL) - NASDAQ GS"));
			dataset.addSeries(buildChartTimeSeries(series, lowBBand, "Low Bollinger Band"));
			dataset.addSeries(buildChartTimeSeries(series, upBBand, "High Bollinger Band"));

			/// <summary>
			/// Creating the chart
			/// </summary>
			JFreeChart chart = ChartFactory.createTimeSeriesChart("Apple Inc. 2013 Close Prices", "Date", "Price Per Unit", dataset, true, true, false); // generate URLs? -  generate tooltips? -  create legend? -  data -  y-axis label -  x-axis label -  title
			XYPlot plot = (XYPlot) chart.Plot;
			DateAxis axis = (DateAxis) plot.DomainAxis;
			axis.DateFormatOverride = new SimpleDateFormat("yyyy-MM-dd");

			/// <summary>
			/// Displaying the chart
			/// </summary>
			displayChart(chart);
		}
	}
}