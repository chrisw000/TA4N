using System.Collections.Generic;

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
namespace TA4N.Examples.analysis
{

	using Indicator = TA4N.Indicator;
	using Strategy = TA4N.Strategy;
	using Decimal = TA4N.Decimal;
	using Tick = TA4N.Tick;
	using TimeSeries = TA4N.TimeSeries;
	using Trade = TA4N.Trade;
	using ClosePriceIndicator = TA4N.indicators.simple.ClosePriceIndicator;
	using ChartFactory = org.jfree.chart.ChartFactory;
	using ChartPanel = org.jfree.chart.ChartPanel;
	using JFreeChart = org.jfree.chart.JFreeChart;
	using DateAxis = org.jfree.chart.axis.DateAxis;
	using Marker = org.jfree.chart.plot.Marker;
	using ValueMarker = org.jfree.chart.plot.ValueMarker;
	using XYPlot = org.jfree.chart.plot.XYPlot;
	using Minute = org.jfree.data.time.Minute;
	using TimeSeriesCollection = org.jfree.data.time.TimeSeriesCollection;
	using ApplicationFrame = org.jfree.ui.ApplicationFrame;
	using RefineryUtilities = org.jfree.ui.RefineryUtilities;
	using CsvTradesLoader = TA4N.Examples.loaders.CsvTradesLoader;
	using MovingMomentumStrategy = TA4N.Examples.strategies.MovingMomentumStrategy;

	/// <summary>
	/// This class builds a graphical chart showing the buy/sell signals of a strategy.
	/// </summary>
	public class BuyAndSellSignalsToChart
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
				chartTimeSeries.add(new Minute(tick.EndTime.toDate()), indicator.getValue(i).toDouble());
			}
			return chartTimeSeries;
		}

		/// <summary>
		/// Runs a strategy over a time series and adds the value markers
		/// corresponding to buy/sell signals to the plot. </summary>
		/// <param name="series"> a time series </param>
		/// <param name="strategy"> a trading strategy </param>
		/// <param name="plot"> the plot </param>
		private static void addBuySellSignals(TimeSeries series, Strategy strategy, XYPlot plot)
		{
			// Running the strategy
			IList<Trade> trades = series.run(strategy).Trades;
			// Adding markers to plot
			foreach (Trade trade in trades)
			{
				// Buy signal
				double buySignalTickTime = (new Minute(series.getTick(trade.Entry.Index).EndTime.toDate())).FirstMillisecond;
				Marker buyMarker = new ValueMarker(buySignalTickTime);
				buyMarker.Paint = Color.GREEN;
				buyMarker.Label = "B";
				plot.addDomainMarker(buyMarker);
				// Sell signal
				double sellSignalTickTime = (new Minute(series.getTick(trade.Exit.Index).EndTime.toDate())).FirstMillisecond;
				Marker sellMarker = new ValueMarker(sellSignalTickTime);
				sellMarker.Paint = Color.RED;
				sellMarker.Label = "S";
				plot.addDomainMarker(sellMarker);
			}
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
			panel.PreferredSize = new Dimension(1024, 400);
			// Application frame
			ApplicationFrame frame = new ApplicationFrame("Ta4j example - Buy and sell signals to chart");
			frame.ContentPane = panel;
			frame.pack();
			RefineryUtilities.centerFrameOnScreen(frame);
			frame.Visible = true;
		}

		public static void Main(string[] args)
		{

			// Getting the time series
			TimeSeries series = CsvTradesLoader.loadBitstampSeries();
			// Building the trading strategy
			Strategy strategy = MovingMomentumStrategy.buildStrategy(series);

			/// <summary>
			/// Building chart datasets
			/// </summary>
			TimeSeriesCollection dataset = new TimeSeriesCollection();
			dataset.addSeries(buildChartTimeSeries(series, new ClosePriceIndicator(series), "Bitstamp Bitcoin (BTC)"));

			/// <summary>
			/// Creating the chart
			/// </summary>
			JFreeChart chart = ChartFactory.createTimeSeriesChart("Bitstamp BTC", "Date", "Price", dataset, true, true, false); // generate URLs? -  generate tooltips? -  create legend? -  data -  y-axis label -  x-axis label -  title
			XYPlot plot = (XYPlot) chart.Plot;
			DateAxis axis = (DateAxis) plot.DomainAxis;
			axis.DateFormatOverride = new SimpleDateFormat("MM-dd HH:mm");

			/// <summary>
			/// Running the strategy and adding the buy and sell signals to plot
			/// </summary>
			addBuySellSignals(series, strategy, plot);

			/// <summary>
			/// Displaying the chart
			/// </summary>
			displayChart(chart);
		}
	}

}